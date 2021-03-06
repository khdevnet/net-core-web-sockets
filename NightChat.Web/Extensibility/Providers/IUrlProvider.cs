﻿using System.Collections.Generic;

namespace NightChat.Web.Extensibility.Providers
{
    public interface IUrlProvider
    {
        string GetUrlQuery(string url, IDictionary<string, string> keyValues);
    }
}