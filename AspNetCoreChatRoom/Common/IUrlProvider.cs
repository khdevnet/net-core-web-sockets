using System.Collections.Generic;

namespace NightChat.WebApi.Common
{
    public interface IUrlProvider
    {
        string GetUrlQuery(string url, IDictionary<string, string> keyValues);
    }
}