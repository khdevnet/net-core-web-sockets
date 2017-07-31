using System.Net.Http;
using Plugin.Http.Extensibility.Dto;

namespace Plugin.Http.Extensibility.Senders
{
    public interface IHttpMessageSender
    {
        HttpMessageSenderResponse Get(string url, Header header = null);

        HttpMessageSenderResponse Post(string url, Header header, HttpContent httpContent = default(HttpContent));
    }
}