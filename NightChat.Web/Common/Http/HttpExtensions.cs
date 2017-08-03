using System.Net.Http;
using Newtonsoft.Json;

namespace AspNetCoreChatRoom.Common.Http
{
    public static class HttpExtensions
    {
        public static TContent Read<TContent>(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<TContent>(content.ReadAsStringAsync().Result);
        }
    }
}