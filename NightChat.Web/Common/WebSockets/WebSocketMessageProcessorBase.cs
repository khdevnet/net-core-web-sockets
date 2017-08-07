using System;
using Microsoft.AspNetCore.Http;

namespace NightChat.Web.Common.WebSockets
{
    internal abstract class WebSocketMessageProcessorBase<TRequestMessageType, TResponseMessageType> : IWebSoketMessageProcessor
        where TRequestMessageType : class
        where TResponseMessageType : class
    {
        public string MessageType { get; } = typeof(TRequestMessageType).Name;

        public Type DataType { get; } = typeof(TRequestMessageType);

        public object Process(HttpContext context, object request)
        {
            return Process(context, Cast<TRequestMessageType>(request));
        }

        protected abstract TResponseMessageType Process(HttpContext context, TRequestMessageType request);

        private TType Cast<TType>(object request)
        {
            return (TType)request;
        }
    }
}