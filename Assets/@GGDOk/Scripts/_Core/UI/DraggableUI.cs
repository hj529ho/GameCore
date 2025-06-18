using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public class DraggableUI : MonoBehaviour
    {
        private Vector2 _vector;
        private bool _isDrag = false;
        void Start()
        {
            gameObject.AddUIEvent(Ondrag, Define.UIEvent.Drag);
            gameObject.AddUIEvent(Ondrop, Define.UIEvent.Drop);
        }

        private void Ondrag(PointerEventData data)
        {
            if (!_isDrag)
            {
                _vector = transform.position - (Vector3)data.position;
                _isDrag = true;
            }
            transform.position = data.position + _vector;
        }
        private void Ondrop(PointerEventData data)
        {
            _isDrag = false;
            _vector = Vector2.zero;
        }
    }
}
