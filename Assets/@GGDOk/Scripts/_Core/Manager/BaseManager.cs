using Cysharp.Threading.Tasks;

namespace Core.Manager
{
    public class BaseManager
    {
        public virtual void Init()
        {
            
        }

        public virtual async UniTask InitAsync()
        {
            await UniTask.Yield();
        }
    }
}