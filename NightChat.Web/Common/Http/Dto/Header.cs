using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Plugin.Http.Extensibility.Dto
{
    public class Header
    {
        public Header(string accept = null, IDictionary<string, string> headers = null)
        {
            Accept = accept;
            Headers = new ReadOnlyDictionary<string, string>(headers ?? new Dictionary<string, string>());
        }

        public string Accept { get; }

        public IReadOnlyDictionary<string, string> Headers { get; }
    }
}