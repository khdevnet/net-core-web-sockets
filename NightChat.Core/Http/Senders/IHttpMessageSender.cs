using System.Net.Http;
using NightChat.Core.Http.Dto;

namespace NightChat.Core.Http.Senders
{
    public interface IHttpMessageSender
    {
        HttpMessageSenderResponse Get(string url, Header header = null);

        HttpMessageSenderResponse Post(string url, Header header, HttpContent httpContent = default(HttpContent));
    }
}