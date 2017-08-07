using System;
using Microsoft.AspNetCore.Http;

namespace NightChat.Web.Common.WebSockets
{
    public interface IWebSoketMessageProcessor
    {
        string MessageType { get; }

        Type DataType { get; }

        object Process(HttpContext context, dynamic request);
    }
}