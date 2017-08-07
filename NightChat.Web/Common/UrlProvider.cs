using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace NightChat.Web.Common
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

            List<KeyValuePair<string, string>> parameters = GetQueryParameters(httpValueCollection);
            ub.Query = new QueryBuilder(parameters).ToQueryString().Value;
            return ub.Uri.ToString();
        }

        private static List<KeyValuePair<string, string>> GetQueryParameters(Dictionary<string, StringValues> httpValueCollection)
        {
            return httpValueCollection.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();
        }
    }
}