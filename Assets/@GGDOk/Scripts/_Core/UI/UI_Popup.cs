using System.Collections;
using Core.Manager;

namespace Core.UI
{
    public class UI_Popup : UI_Base
    {
        
        
        public virtual void Init()
        {
            Managers.UI.SetCanvas(gameObject, true);
            StartCoroutine(UpdateCoroutine());
        }

        public virtual void ClosePopupUI()
        {
            Managers.UI.ClosePopupUI(this);
        }
        public virtual void Refresh()
        {
        
        }

        protected virtual void OnUpdate()
        {
            
        }

        private IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                OnUpdate();
                yield return null;
            }
        }
    }
}

