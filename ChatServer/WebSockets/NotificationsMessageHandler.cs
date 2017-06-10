using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine(id);
            var exists = rooms.TryGetValue(id, out List<string> room);
            var invalidSockets = new List<string>();
            Console.WriteLine(message);
            if (exists)
            {
                foreach (var sockId in room)
                {
                    Console.WriteLine(sockId);
                    var socket = WebSocketConnectionManager.GetSocketById(sockId);
                    if (socket == null)
                    {
                        invalidSockets.Add(sockId);
                        continue;
                    }
                    await SendMessageAsync(sockId, message);
                }
                room.RemoveAll(s => invalidSockets.Any(t => t == s));
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
                data = data.Replace("}}", "}");
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}