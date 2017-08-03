using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Plugin.Http.Extensibility.Dto;
using Plugin.Http.Extensibility.Senders;

namespace Plugin.Http.Senders
{
    public class HttpMessageSender : IHttpMessageSender
    {
        public HttpMessageSenderResponse Post(string url, Header header, HttpContent httpContent = default(HttpContent))
        {
            HttpClient httpClient = GetHttpClient(header);
            return GetResponse(httpClient.PostAsync(url, httpContent));
        }

        public HttpMessageSenderResponse Get(string url, Header header = null)
        {
            HttpClient httpClient = GetHttpClient(header ?? new Header());
            return GetResponse(httpClient.GetAsync(url));
        }

        private HttpClient GetHttpClient(Header header)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            if (!String.IsNullOrEmpty(header.Accept))
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.Accept));
            }

            foreach (KeyValuePair<string, string> headers in header.Headers)
            {
                httpClient.DefaultRequestHeaders.Add(headers.Key, headers.Value);
            }

            return httpClient;
        }

        private HttpMessageSenderResponse GetResponse(Task<HttpResponseMessage> httpResponseMessageTask)
        {
            HttpMessageSenderResponse response;
            HttpResponseMessage httpResponseMessage = null;

            try
            {
                httpResponseMessage = httpResponseMessageTask.Result;
                response = new HttpMessageSenderResponse(httpResponseMessage);
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerExceptions.All(e => e is HttpRequestException))
            {
                response = new HttpMessageSenderResponse(httpResponseMessage, aggregateException.InnerExceptions.Cast<HttpRequestException>());
            }

            return response;
        }
    }
}