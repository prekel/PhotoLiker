using System;

namespace PhotoLiker.Onliner.Core
{
    public class OnlineCheck
    {
        public int CheckId { get; set; }

        public long VkId { get; set; }

        public DateTimeOffset Time { get; set; }

        public bool Online { get; set; }
        
        public bool? OnlineMobile { get; set; }

        public long? OnlineApp { get; set; }
    }
}
