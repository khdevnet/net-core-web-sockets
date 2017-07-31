using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace NightChat.WebApi.Common
{
    public class UrlProvider : IUrlProvider
    {
        public string GetUrlQuery(string url, IDictionary<string, string> keyValues)
        {
            var uri = new Uri(url);
            var ub = new UriBuilder(uri);

            Dictionary<string, StringValues> httpValueCollection = QueryHelpers.ParseQuery(uri.Query);

            foreach (KeyValuePair<string, string> keyValue in keyValues)
            {
                httpValueCollection.Add(keyValue.Key, keyValue.Value);
            }

            ub.Query = httpValueCollection.ToString();
            return ub.Uri.ToString();
        }
    }
}