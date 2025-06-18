using System;
using System.Collections;
using Core.UI;
using UnityEngine;

public abstract class UI_Story : UI_Base
{
    public virtual void Init()
    {
    }

    public virtual void Resetting()
    {
    }
    public abstract void Revoke();
    public abstract IEnumerator Invoke();
}
