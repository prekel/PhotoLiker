using System.Threading.Tasks;

using VkNet;

namespace PhotoLiker
{
    public abstract class Worker
    {
        public VkApi Api { get; private set; }

        public abstract Task Begin();
    }
}
