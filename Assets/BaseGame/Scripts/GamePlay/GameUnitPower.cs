using DG.Tweening;
using Pextension;
using System;
using System.Collections;
using System.Collections.Generic;
using TW.Reactive.CustomComponent;
using UnityEngine;
using R3;

public class GameUnitPower : RoateableObject, ITransferPower
{
    [field: SerializeField] public ReactiveValue<bool> IsHavePower { get; private set; } = new(true);   
    [field: SerializeField] public ReactiveValue<bool> IsGivingPower { get; set; } = new(false);
    [field: SerializeField] public ReactiveValue<bool> IsGettingPower { get; set; } = new(true);
    [field: SerializeField] public bool IsCastingLine { get; set; } = false;
    [field: SerializeField] public GameUnitType Type { get; private set; } = GameUnitType.Power;
    //[field: SerializeField] public MiniPool<LaserLine> LaserLinePool { get; private set; } = new();
    [field: SerializeField] public GameObject FirePoint { get; private set; }
    //[field: SerializeField] public Camera Cam { get; private set; }
    //[field: SerializeField] public LaserLine Prefabs { get; private set; }
    [field: SerializeField] public LaserLine CurLine { get; private set; }
    [field: SerializeField] public GameUnit CurCastTarget { get; private set; } = null;
    [field: SerializeField] public Vector2 CurCastDirection { get; set; } = Vector2.zero;
    private IDisposable _disposable;

    public override void OnAwake()
    {
        base.OnAwake();
    }
    public override void Rotate()
    {
        if (IsCastingLine)        
        {
            InputManager.Instance.SelectGameUnit = null;
            return;
        };
        base.Rotate();
    }
    public override void Initialize()
    {
        CastNewLaser();
    }
    public Transform GetPowerPos()
    {
        return this.Transform;
    }
    public override void OnRotateCompleted()
    {
        base.OnRotateCompleted();
        CastNewLaser();
    }
    public override void OnStartRotate()
    {
        base.OnStartRotate();
        _disposable?.Dispose();
        IsGivingPower.Value = false;
        CurLine.DisablePrepare();
        CurCastTarget = null;
    }
    public bool IsCanHavePower(Vector2 curDirection, Vector2 direction)
    {
        return false;
    }
    public override void UpdateDirection()
    {
        base.UpdateDirection();
        switch (Mathf.Abs(CurrentRotation) % 360)
        {
            case 0:
                CurDirection.Value = Vector2.up;
                CurCastDirection = Vector2.up;
                break;
            case 90:
                CurDirection.Value = Vector2.right;
                CurCastDirection = Vector2.right;
                break;
            case 180:
                CurDirection.Value = Vector2.down;
                CurCastDirection = Vector2.down;
                break;
            case 270:
                CurDirection.Value = Vector2.left;
                CurCastDirection = Vector2.left;
                break;
        }

    }
    public void CastNewLaser()
    {
        IsCastingLine = true;
        Debug.Log(this.name + CurDirection.Value);
        RaycastHit2D hit = Physics2D.Raycast(FirePoint.transform.position, CurDirection);
        Vector3 endPosition = FirePoint.transform.position + (Vector3)CurDirection.Value * 100;
        Debug.DrawLine(FirePoint.transform.position, endPosition, Color.red, 2f);
        if (hit.collider != null)
        {
            GameUnit gameUnit = hit.collider.GetComponent<GameUnit>();
            if (gameUnit != null)
            {
                GameUnitCheckPoint checkPoint = hit.collider.GetComponent<GameUnitCheckPoint>();
                if (checkPoint != null)
                {
                    CurCastTarget = checkPoint;
                    if ((CurCastTarget as GameUnitCheckPoint).IsHavePower)
                    {
                        DrawLaserLine(CurCastTarget.transform.position - new Vector3(CurDirection.Value.x * 4f, CurDirection.Value.y * 4f, 0),false);
                    }
                    else
                    {
                        if ((CurCastTarget as GameUnitCheckPoint).IsCanHavePower((CurCastTarget as RoateableObject).CurDirection.Value, CurDirection))
                        {
                            DrawLaserLine(CurCastTarget.Transform.position,false, OnDrawCompleted);
                        }
                        else
                        {
                            DrawLaserLine(CurCastTarget.transform.position - new Vector3(CurDirection.Value.x * 4f, CurDirection.Value.y * 4f, 0), false);
                        }

                    }
                    _disposable?.Dispose();
                    _disposable = (CurCastTarget as GameUnitCheckPoint).CurDirection.ReactiveProperty
                        .CombineLatest((CurCastTarget as GameUnitCheckPoint).IsHavePower.ReactiveProperty, (dir, power) => (dir,power))
                        .Skip(1)
                        .Subscribe(OnTargetRotate);
                    return;
                }
                GameUnitPower power = hit.collider.GetComponent<GameUnitPower>();
                if (power != null)
                {
                    CurCastTarget = power;
                    DrawLaserLine(CurCastTarget.transform.position - new Vector3(CurDirection.Value.x * 1f, CurDirection.Value.y * 1f, 0),false);
                    return;
                }
                GameUnitBattery battery = hit.collider.GetComponent<GameUnitBattery>();
                if (battery != null)
                {
                    CurCastTarget = battery;
                    DrawLaserLine(CurCastTarget.Transform.position,false, OnDrawCompleted);
                    return;
                }
            }
        }
        else
        {

            //CurLine.DisablePrepare();
            DrawLaserLine(FirePoint.transform.position + new Vector3(CurDirection.Value.x * 5, CurDirection.Value.y * 5, 0), false);
        }
    }
    public void DrawLaserLine(Vector3 endPos,bool isImmediate, Action action = null)
    {
        if (action == null)
            action = () => 
            { 
                IsCastingLine = false;
                (CurCastTarget as GameUnitCheckPoint).SetCastline(false);
            };
        CurLine.Setup(FirePoint.transform.position, endPos, isImmediate, action);
    }
    public void OnDrawCompleted()
    {
        if (CurCastTarget is GameUnitCheckPoint)
        {
            if((CurCastTarget as GameUnitCheckPoint).IsHavePower)
            {
                IsCastingLine = false;
                return;
            }
            if ((CurCastTarget as GameUnitCheckPoint).IsCanHavePower((CurCastTarget as RoateableObject).CurDirection, CurDirection))
            {
                IsGivingPower.Value = true;
                (CurCastTarget as GameUnitCheckPoint).SetPower(this);   
            }
        }
        else if (CurCastTarget is GameUnitBattery)
        {
            if((CurCastTarget as GameUnitBattery).IsHavePower.Value)
            {
                IsCastingLine = false;
                return;
            }
            IsGivingPower.Value = true;
            (CurCastTarget as GameUnitBattery).SetPower(this);
            LevelManager.Instance.SetCheckWin(true);
        }
        IsCastingLine = false;
    }
    public void OnTargetRotate((Vector2 targetDirection, bool isHavePower) value)
    {
        //CurLine.DisablePrepare();
        Debug.Log(CurCastTarget.name + "OnTargetRotateCompleted" + Time.deltaTime);
        if (CurCastTarget != null)
        {
            if (CurCastTarget is GameUnitCheckPoint)
            {
                //if((CurCastTarget as GameUnitCheckPoint).IsHavePower.Value)
                //{
                //    IsCastingLine = true;
                //    DrawLaserLine(CurCastTarget.transform.position, true, OnDrawCompleted);
                //    Debug.Log(CurCastTarget.name + "Can have power" + Time.deltaTime);
                //    return;
                //}
                if (!(CurCastTarget as IRecivePower).IsCanHavePower((CurCastTarget as RoateableObject).CurDirection, CurDirection))
                {
                    IsCastingLine = true;
                    IsGivingPower.Value = false;
                    DrawLaserLine(CurCastTarget.transform.position - new Vector3(CurDirection.Value.x * 4f, CurDirection.Value.y * 4f, 0f), true);
                    Debug.Log(CurCastTarget.name + "Cant have power" + Time.deltaTime);
                }
                else
                {
                    IsCastingLine = true;
                    DrawLaserLine(CurCastTarget.transform.position,true,OnDrawCompleted);
                    Debug.Log(CurCastTarget.name + "Can have power" + Time.deltaTime);
                }
            }
        }
    }
    public override void OnResetLevel()
    {
        IsHavePower.Value = true;
        IsGivingPower.Value  = false;
        IsGettingPower.Value =true;
        IsCastingLine = false;
        CurLine.DisablePrepare();
        CurCastTarget = null;
        CurCastDirection= Vector2.zero;
        _disposable?.Dispose();
    }
}
