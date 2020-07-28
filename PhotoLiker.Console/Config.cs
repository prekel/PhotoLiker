using System;
using System.Collections.Generic;

namespace PhotoLiker
{
    public class Config
    {
        public ulong? AppId { get; set; }

        public string? Login { get; set; }

        public string? AccessToken { get; set; }

        public enum AppMode
        {
            None = 0,
            AutoLiker = 1,
            Liker = 2,
            GoodMorning = 4,
            Saver = 8,
            HappyNewYear = 16,
            Onliner = 32,
            DbSaver = 64
        }

        public AppMode Mode { get; set; } = AppMode.None;

        public IList<long> AutoLikerIds { get; set; } = new List<long>();

        public TimeSpan LikerWaitOfflineTime { get; set; }
        public long LikerId { get; set; }
        public int LikerCount { get; set; }

        public class GoodMorningEntity
        {
            public long GoodMorningId { get; set; }
            public string GoodMorningMessage { get; set; } = "";
            public TimeSpan GoodMorningDelay { get; set; }
        }

        public IList<GoodMorningEntity> GoodMornings { get; set; } = new List<GoodMorningEntity>();

        public class HappyNewYearEntity
        {
            public long HappyNewYearId { get; set; }
            public string HappyNewYearMessage { get; set; } = "";
            public DateTimeOffset HappyNewYearTime { get; set; }
        }

        public IList<long> OnlinerIds { get; set; } = new List<long>();

        public IList<HappyNewYearEntity> NewYears { get; set; } = new List<HappyNewYearEntity>();

        public long SaverId { get; set; }
    }
}
