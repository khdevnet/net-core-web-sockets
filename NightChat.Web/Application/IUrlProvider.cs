using System.Collections.Generic;

namespace NightChat.Web.Common
{
    public interface IUrlProvider
    {
        string GetUrlQuery(string url, IDictionary<string, string> keyValues);
    }
}