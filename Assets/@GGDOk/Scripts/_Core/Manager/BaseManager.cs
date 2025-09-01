using Cysharp.Threading.Tasks;

namespace Core.Manager
{
    public abstract class BaseManager
    {
        public abstract void Init();
        public abstract UniTask InitAsync();
    }
}