using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ChatServer.WebSockets
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate next;
        private WebSocketHandler webSocketHandler;

        public WebSocketMiddleware(RequestDelegate next, WebSocketHandler handler)
        {
            this.next = next;
            webSocketHandler = handler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await webSocketHandler.OnConnected(socket);

            await Receive(socket, async (result, buffer) =>
            {
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        await webSocketHandler.ReceiveAsync(socket, result, buffer);
                        break;
                    case WebSocketMessageType.Close:
                        await webSocketHandler.OnDisconnected(socket);
                        break;
                }
            });

            await next.Invoke(context);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}