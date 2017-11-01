using System;

namespace NightChat.Web.Application.Sockets
{
    public interface ISocketMessageProcessor
    {
        string MessageType { get; }

        Type MessageDataType { get; }

        object Process(dynamic message);
    }
}