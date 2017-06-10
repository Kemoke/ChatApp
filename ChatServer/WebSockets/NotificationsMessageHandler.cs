using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Model;
using JWT;
using Newtonsoft.Json;

namespace ChatServer.WebSockets
{
    public class NotificationsMessageHandler : WebSocketHandler
    {
        private readonly GlobalConfig config;
        private readonly ConcurrentDictionary<int, List<string>> rooms = new ConcurrentDictionary<int, List<string>>();
        public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager, GlobalConfig config) : base(webSocketConnectionManager)
        {
            this.config = config;
        }

        public async Task SendToRoom(string message, int id)
        {
            var exists = rooms.TryGetValue(id, out List<string> room);
            if (exists)
            {
                foreach (var sockId in room)
                {
                    await SendMessageAsync(sockId, message);
                }
            }
        }

        public override Task OnDisconnected(WebSocket socket)
        {
            foreach (var room in rooms)
            {
                room.Value.Remove(WebSocketConnectionManager.GetId(socket));
            }
            return base.OnDisconnected(socket);
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            try
            {
                var data = Encoding.UTF8.GetString(buffer);
                var json = JsonConvert.DeserializeObject<NotificationMessage>(data);
                JsonWebToken.DecodeToObject<User>(json.Token, config.AppKey);
                var newId = json.NewId;
                var oldId = json.OldId;
                var exists = rooms.TryGetValue(oldId, out var oldRoom);
                if (exists)
                    oldRoom.Remove(WebSocketConnectionManager.GetId(socket));
                exists = rooms.TryGetValue(newId, out var room);
                if (!exists)
                {
                    room = new List<string>();
                    rooms[newId] = room;
                }
                if (!room.Exists(s => s == WebSocketConnectionManager.GetId(socket)))
                {
                    room.Add(WebSocketConnectionManager.GetId(socket));
                }
            }
            catch
            {
                //
            }
        }
    }
}