using System;

namespace NightChat.Core.Sockets
{
    public interface ISocketMessageProcessor
    {
        string MessageType { get; }

        Type MessageDataType { get; }

        object Process(dynamic message);
    }
}