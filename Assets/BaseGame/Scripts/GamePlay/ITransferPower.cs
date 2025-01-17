using System;
using System.Collections;
using System.Collections.Generic;
using TW.Reactive.CustomComponent;
using UnityEngine;

public interface ITransferPower : IPowerAble
{
    public ReactiveValue<bool> IsGivingPower { get; set; }
    public bool IsCastingLine { get; set; }
    public Vector2 CurCastDirection { get; set; }
    //public ReactiveValue<bool> IsGettingPower { get; set; }
    public bool IsCanHavePower(Vector2 curDirection,Vector2 direction);

    public Transform GetPowerPos();

    public void CastNewLaser();

    public void DrawLaserLine(Vector3 endPos,bool isImmediate, Action action = null);

    public void OnDrawCompleted();

    public void OnTargetRotate((Vector2 targetDirection, bool isHavePower) value);
}
