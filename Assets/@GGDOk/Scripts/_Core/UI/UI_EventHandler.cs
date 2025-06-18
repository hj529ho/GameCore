using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler,IDropHandler,IPointerUpHandler,IPointerDownHandler
    {
        public Action<PointerEventData> OnClickHandler = null;
        public Action<PointerEventData> OnDragHandler = null;
        public Action<PointerEventData> OnDropHandler = null;
        public Action<PointerEventData> OnPointerUpHandler = null;
        public Action<PointerEventData> OnPointerDownHandler = null;
        public void OnPointerClick(PointerEventData eventData)
        {
            if(OnClickHandler !=null)
                OnClickHandler.Invoke(eventData);
        }   
        public void OnDrag(PointerEventData eventData)
        {
            if(OnDragHandler !=null)
                OnDragHandler.Invoke(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(OnDropHandler !=null)
                OnDropHandler.Invoke(eventData);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if(OnPointerUpHandler !=null)
                OnPointerUpHandler.Invoke(eventData);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if(OnPointerDownHandler !=null)
                OnPointerDownHandler.Invoke(eventData);
        }
    }
}
