using ChatServer.Request;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json;

namespace ChatServerTests
{
    public static class BrowserExtensions
    {
        public static void BodyJson<T>(this BrowserContext context, T data)
        {
            context.Body(JsonConvert.SerializeObject(data), "application/json");
            context.Accept(new MediaRange("application/json"));
        }

        public static T BodyJson<T>(this BrowserResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Body.AsString());
        }
    }
}