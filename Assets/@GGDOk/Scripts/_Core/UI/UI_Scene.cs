using Core.Manager;

namespace Core.UI
{
    public class UI_Scene : UI_Base
    {
        public virtual void Init()
        {
            Managers.UI.SetCanvas(gameObject, false);
        }
    }
}
