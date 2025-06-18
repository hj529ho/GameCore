using UnityEngine;
using static Define;

namespace Core.Controller
{
    [SelectionBase]
    public class BaseController : MonoBehaviour
    {
        public ObjectType ObjType { get; protected set; }
        protected virtual void Awake()
        {
            Init();
        }

        protected bool _isInit = false;
        public virtual bool Init()
        {
            if (_isInit)
                return false;
            _isInit = true;
            return true;
        }
        
    }
}
