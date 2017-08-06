using System.Net;

namespace NightChat.Core.Http.Dto
{
    public class MessageResponse
    {
        public MessageResponse(HttpStatusCode? statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode? StatusCode { get; }

        public bool HasResponse => StatusCode.HasValue;
    }

    public class MessageResponse<TData> : MessageResponse
    {
        public MessageResponse(HttpStatusCode? statusCode, TData data, Header header)
            : base(statusCode)
        {
            Data = data;
            Header = header ?? new Header();
        }

        public TData Data { get; }

        public Header Header { get; }

        public bool HasData => Data != null;
    }
}