using System;

namespace PhotoLiker.Onliner.Core
{
    public class OnlineCheck
    {
        public int CheckId { get; set; }

        public int VkId { get; set; }

        public DateTimeOffset Time { get; set; }
        
        public bool IsOnline { get; set; }
    }
}
