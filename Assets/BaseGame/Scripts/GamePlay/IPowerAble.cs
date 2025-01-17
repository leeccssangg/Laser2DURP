using System.Collections;
using System.Collections.Generic;
using TW.Reactive.CustomComponent;
using UnityEngine;

public interface IPowerAble 
{
    public static ReactiveValue<bool> IsHavePower { get; }


    public void SetPower(bool isHavePower, Vector2 direction)
    {
        
    }
    public bool GetPowerStatus()
    {
        return IsHavePower;
    }
}
