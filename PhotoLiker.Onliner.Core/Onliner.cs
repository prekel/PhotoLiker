using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NLog;

using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

using PhotoLiker.Core;

namespace PhotoLiker.Onliner.Core
{
    public class OnlinerEventArgs : EventArgs
    {
        public IEnumerable<OnlineCheck> Checks { get; }

        public OnlinerEventArgs(IEnumerable<OnlineCheck> checks)
        {
            Checks = checks;
        }
    }

    public class Onliner : AbstractVkWorker
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();

        public IList<long> Ids { get; }

        public override async Task Begin()
        {
            Log.Info("Запуск Onliner");

            while (true)
            {
                Log.Debug("Начат запрос");
                var checks = (await Check()).ToList();
                Log.Debug("Принято");
                Console.WriteLine(String.Join("; ", checks.Select(y => $"{y.VkId} {y.Online}")));
                OnOnlinerChecked(new OnlinerEventArgs(checks));
                await Task.Delay(9900);
            }
        }

        public delegate void OnlinerEventHandler(object sender, OnlinerEventArgs e);

        public event OnlinerEventHandler? OnlinerChecked;

        public Onliner(VkApi api, IList<long> ids) : base(api)
        {
            Ids = ids;
        }

        public async Task<IEnumerable<OnlineCheck>> Check()
        {
            return (await Api.Users.GetAsync(Ids, ProfileFields.Online))
                .Select(t => new OnlineCheck()
                {
                    VkId = t.Id,
                    Online = t.Online!.Value,
                    Time = DateTimeOffset.Now,
                    OnlineMobile = t.OnlineMobile ?? false
                });
        }

        protected virtual void OnOnlinerChecked(OnlinerEventArgs e)
        {
            OnlinerChecked?.Invoke(this, e);
        }
    }
}
