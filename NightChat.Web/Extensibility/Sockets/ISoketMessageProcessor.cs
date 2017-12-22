using System;

namespace NightChat.Web.Extensibility.Sockets
{
    public interface ISocketMessageProcessor
    {
        string MessageType { get; }

        Type MessageDataType { get; }

        object Process(dynamic message);
    }
}