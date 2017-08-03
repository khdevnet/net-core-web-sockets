using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Plugin.Http.Extensibility.Dto
{
    public class HttpMessageSenderResponse
    {
        public HttpMessageSenderResponse(HttpResponseMessage response, IEnumerable<HttpRequestException> exceptions = null)
        {
            Response = response;
            Exceptions = new ReadOnlyCollection<HttpRequestException>((exceptions ?? Enumerable.Empty<HttpRequestException>()).ToList());
        }

        public HttpResponseMessage Response { get; }

        public HttpStatusCode? StatusCode => HasResponse ? Response.StatusCode : (HttpStatusCode?)null;

        public IReadOnlyCollection<HttpRequestException> Exceptions { get; }

        public bool HasResponse => Response != null;
    }
}