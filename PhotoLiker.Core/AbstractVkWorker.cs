using System.Threading.Tasks;

using VkNet;

namespace PhotoLiker.Core
{
    public abstract class AbstractVkWorker : AbstractWorker
    {
        protected VkApi Api { get; }

        protected AbstractVkWorker(VkApi api)
        {
            Api = api;
        }
    }
}
