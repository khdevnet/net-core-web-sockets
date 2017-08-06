using System;

namespace NightChat.Core.TimeProviders
{
    public class DefaultTimeProvider : TimeProvider
    {
        public override DateTime Now => DateTime.Now;
    }
}