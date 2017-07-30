using System;

namespace NightChat.WebApi.Common.Data.Models
{
    public class Token
    {
        public string UserId { get; set; }

        public DateTime ExpiredTimestamp { get; set; }

        public string Value { get; set; }
    }
}