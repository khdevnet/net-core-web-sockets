using System;

namespace NightChat.Domain.Entities
{
    internal class Token
    {
        public string UserId { get; set; }

        public DateTime ExpiredTimestamp { get; set; }

        public string Value { get; set; }
    }
}