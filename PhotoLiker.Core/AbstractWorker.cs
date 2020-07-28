using System.Threading.Tasks;

using VkNet;

namespace PhotoLiker.Core
{
    public abstract class AbstractWorker
    {
        protected VkApi Api { get; }

        public AbstractWorker(VkApi api)
        {
            Api = api;
        }

        public abstract Task Begin();
    }
}
