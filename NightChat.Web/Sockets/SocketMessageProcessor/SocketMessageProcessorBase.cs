using System;
using NightChat.Web.Extensibility.Sockets;

namespace NightChat.Web.Sockets.SocketMessageProcessor
{
    public abstract class SocketMessageProcessorBase<TRequestMessageType, TResponseMessageType> : ISocketMessageProcessor
        where TRequestMessageType : class
        where TResponseMessageType : class
    {
        public string MessageType { get; } = typeof(TRequestMessageType).Name;

        public Type MessageDataType { get; } = typeof(TRequestMessageType);

        public object Process(object request)
        {
            return Process(Cast<TRequestMessageType>(request));
        }

        protected abstract TResponseMessageType Process(TRequestMessageType request);

        private TType Cast<TType>(object request)
        {
            return (TType)request;
        }
    }
}