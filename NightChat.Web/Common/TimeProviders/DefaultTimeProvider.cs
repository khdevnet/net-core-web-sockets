using System;

namespace Plugin.Core.Extensibility.TimeProviders
{
    public class DefaultTimeProvider : TimeProvider
    {
        public override DateTime Now => DateTime.Now;
    }
}