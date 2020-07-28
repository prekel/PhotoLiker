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
    public class Onliner : AbstractWorker
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();
        
        public IList<long> Ids { get; }

        public override async Task Begin()
        {
            Log.Info("Запуск Onliner");
            
            await Check();
        }

        public Onliner(VkApi api, IList<long> ids) : base(api)
        {
            Ids = ids;
        }

        public async Task<bool> Check()
        {
            var g = await Api.Users.GetAsync(new[] {151599521L}, ProfileFields.Online);

            return g.First().Online!.Value;
        }
    }
}
