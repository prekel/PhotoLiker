using System.Threading.Tasks;

namespace PhotoLiker.Core
{
    public abstract class AbstractWorker
    {
        public abstract Task Begin();
    }
}
