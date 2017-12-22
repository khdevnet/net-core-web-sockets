using System.Collections.Generic;

namespace NightChat.Web.Application
{
    public interface IUrlProvider
    {
        string GetUrlQuery(string url, IDictionary<string, string> keyValues);
    }
}