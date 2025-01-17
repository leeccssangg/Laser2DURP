using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecivePower : IPowerAble
{
    public static Transform PowerPos { get; set; }
    public bool IsCanHavePower(Vector2 curDirection, Vector2 direction);

    public Transform GetPowerPos()
    {
        return PowerPos;
    }
    public void SetPowerPos(Transform powerPos)
    {
        PowerPos = powerPos;
    }
    public void SetPower(ITransferPower transferPower)
    {

    }
    public void SubcribePowerGivingChange(bool isGivingPower)
    {

    }
}
