using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using ChatApp.Response;
using Newtonsoft.Json;

namespace ChatApp
{
    public static class Extensions
    {
        public static void JsonBody<T>(this HttpRequestMessage message, T data)
        {
            message.Headers.Accept.ParseAdd("application/json");
            message.Headers.TryAppendWithoutValidation("Content-Type", "application/json");
            message.Content = new HttpStringContent(JsonConvert.SerializeObject(data));
        }

        public static IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> PostAsync<T>(
            this HttpClient client,
            string url, T data)
        {
            var message = new HttpRequestMessage(HttpMethod.Post,new Uri(url));
            message.JsonBody(data);
            return client.SendRequestAsync(message);
        }

        public static IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> GetAsync<T>(
            this HttpClient client,
            string url, Dictionary<string,string> query = null)
        {
            if (query != null)
            {
                url = query.Aggregate(url, (current, item) => current + "/" + item.Value);
            }
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            return client.SendRequestAsync(message);
        }

        public static async Task<T> JsonBody<T>(this HttpResponseMessage message)
        {
            var input = await message.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(input);
        }

        public static async Task<string> ErrorMessage(this HttpResponseMessage message)
        {
            var error = await message.JsonBody<Error>();
            return error.Message;
        }
    }
}