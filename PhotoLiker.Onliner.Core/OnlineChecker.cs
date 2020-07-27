using System.Linq;
using System.Threading.Tasks;

using NLog;

using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

namespace PhotoLiker.Onliner.Core
{
    public class OnlineChecker
    {
        public VkApi Api { get; private set; }
        
        public OnlineChecker(VkApi api)
        {
            Api = api;
        }

        public async Task<bool> Check()
        {
            var g = await Api.Users.GetAsync(new[] {151599521L}, ProfileFields.Online);

            return g.First().Online!.Value;
        }
    }
}
