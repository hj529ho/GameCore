using System;
using System.Collections;
using System.Collections.Generic;
using Core.Manager;
using Core.UI;
using UnityEngine;

public class UI_Loading : UI_Base
{
    public virtual void Init()
    {
        Managers.UI.SetCanvas(gameObject,false);
    }

    public virtual void DoTransition(bool isOn, Action onComplete = null)
    {
        onComplete?.Invoke();
    }

}