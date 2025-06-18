using System;
using Core.UI;
using UnityEngine;
using UnityEngine.EventSystems;


public static class Extensions 
{
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action,Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.AddUIEvent(go,action,type);
    }
}

