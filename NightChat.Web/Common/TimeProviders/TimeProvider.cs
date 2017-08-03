using System;

namespace Plugin.Core.Extensibility.TimeProviders
{
    public abstract class TimeProvider
    {
        private static TimeProvider current;

        static TimeProvider()
        {
            current = new DefaultTimeProvider();
        }

        public static TimeProvider Current
        {
            get
            {
                return current;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                current = value;
            }
        }

        public abstract DateTime Now { get; }

        public static void ResetToDefault()
        {
            current = new DefaultTimeProvider();
        }
    }
}