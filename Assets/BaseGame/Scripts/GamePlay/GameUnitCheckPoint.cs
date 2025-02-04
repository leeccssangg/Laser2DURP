using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TW.Reactive.CustomComponent;
using UnityEngine;
using R3;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks.Triggers;

public class GameUnitCheckPoint : RoateableObject, ITransferPower, IRecivePower
{
    [field: SerializeField] public GameUnitType Type { get; private set; } = GameUnitType.Checkpoint;
    [field: SerializeField] public GameObject FirePoint { get; private set; }
    [field: SerializeField] public GameObject CheckCastPoint { get; private set; }
    [field: SerializeField] public ReactiveValue<bool> IsHavePower { get; private set; } = new(false);
    [field: SerializeField] public ReactiveValue<bool> IsGivingPower { get; set; } = new(false);
    [field: SerializeField] public bool IsCastingLine { get; set; } = false;
    //[field: SerializeField] public ReactiveValue<bool> IsGettingPower { get; set; } = new(false);
    [field: SerializeField] public LaserLine CurLine { get; private set; }
    [field: SerializeField] public GameUnit CurCastTarget { get; private set; } = null;
    [field: SerializeField] public GameUnit CurPoweredTarget { get; private set; } = null;
    [field: SerializeField] public Vector2 CurCastDirection { get; set; } = Vector2.zero;
    //[field: SerializeField] public Vector2 LastPowerDirection { get; private set; } = Vector2.zero;
    private IDisposable _disposablePowerTarget;
    private IDisposable _disposableCastTarget;

    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void UpdateDirection()
    {
        base.UpdateDirection();
        switch (Mathf.Abs(CurrentRotation) % 360)
        {
            case 0:
                CurDirection.Value = Vector2.up;
                break;
            case 90:
                CurDirection.Value = Vector2.right;
                break;
            case 180:
                CurDirection.Value = Vector2.down;
                break;
            case 270:
                CurDirection.Value = Vector2.left;
                break;
        }

    }
    public override void Rotate()
    {
        if (IsCastingLine) 
        {
            InputManager.Instance.SelectGameUnit = null;
            return;
        };
        //if(IsHavePower)
        //    IsCastingLine = true;
        base.Rotate();
    }
    public override void OnStartRotate()
    {
        CurDirection.Value = Vector2.zero;
        Debug.Log(this.name + "OnStartRotate"+ Time.deltaTime);
        IsHavePower.Value = false;
        //IsGivingPower.Value = false;
        base.OnStartRotate();
    }
    public override void OnRotateCompleted()
    {
        //CurCastTarget = null;
        //_disposableCastTarget?.Dispose();
        base.OnRotateCompleted();
    }
    public override void Initialize()
    {
        this.IsHavePower.ReactiveProperty.Subscribe(OnHavePowerChange).AddTo(this);
        this.IsGivingPower.ReactiveProperty.Subscribe(OnGivingPowerChange).AddTo(this);
    }
    public void OnHavePowerChange(bool isHavingPower)
    {
        Debug.Log(this.name + " SetPower" + isHavingPower + Time.deltaTime);
        if (IsHavePower)
        {
            CastNewLaser();
        }
        else
        {
            CurLine.DisablePrepare();
            _disposablePowerTarget?.Dispose();
            CurPoweredTarget = null;
            CurCastTarget = null;
            IsCastingLine = false;
            _disposableCastTarget?.Dispose();
            if (IsGivingPower)
                IsGivingPower.Value = false;
        }
    }
    public void OnGivingPowerChange(bool isGivingPower)
    {
        if (IsGivingPower)
        {

        }
        else
        {

        }
    }
    public void SubcribePowerGivingChange(bool isGivingPower)
    {
        IsHavePower.Value = isGivingPower;
    }
    public Transform GetPowerPos()
    {
        return this.Transform;
    }
    public void SetPower(ITransferPower transferPower)
    {
        Debug.Log(this.name + " SetPower" + transferPower.GetType().Name + Time.deltaTime);
        CurPoweredTarget = transferPower as GameUnit;
        IsHavePower.Value = transferPower.IsGivingPower.Value;
        _disposablePowerTarget?.Dispose();
        _disposablePowerTarget = (CurPoweredTarget as ITransferPower).IsGivingPower.ReactiveProperty.Skip(1).Subscribe(SubcribePowerGivingChange).AddTo(this);
    }
    public void SetCastline(bool isCasting)
    {
        IsCastingLine = isCasting;
    }
    public void CastNewLaser()
    {
        IsCastingLine = true;
        CurCastDirection = GetCastDirection();
        Debug.Log(this.name + "CurDir:" + CurCastDirection + "/CastDir:" + CurCastDirection + Time.deltaTime);
        RaycastHit2D hit = Physics2D.Raycast(FirePoint.transform.position, CurCastDirection);
        Vector3 endPosition = FirePoint.transform.position + (Vector3)CurCastDirection * 100;
        if (hit.collider == null)
        {
            DrawLaserLine(FirePoint.transform.position + new Vector3(CurCastDirection.x * 50, CurCastDirection.y * 50, 0), false);
            return;
        }
        GameUnit gameUnit = hit.collider.GetComponent<GameUnit>();
        if (gameUnit == null) return;
        GameUnitCheckPoint checkPoint = hit.collider.GetComponent<GameUnitCheckPoint>();
        if (checkPoint != null)
        {
            CurCastTarget = checkPoint;
            if ((CurCastTarget as GameUnitCheckPoint).IsHavePower)
            {
                DrawLaserLine(CurCastTarget.transform.position - new Vector3(CurCastDirection.x * 4f, CurCastDirection.y * 4f, 0), false);
            }
            else
            {
                if ((CurCastTarget as GameUnitCheckPoint).IsCanHavePower((CurCastTarget as RoateableObject).CurDirection.Value, CurCastDirection))
                {
                    DrawLaserLine(CurCastTarget.Transform.position, false, OnDrawCompleted);
                }
                else
                {
                    DrawLaserLine(CurCastTarget.transform.position - new Vector3(CurCastDirection.x * 4f, CurCastDirection.y * 4f, 0), false);
                }

            }
            Debug.Log(this.name + "CastNewLaser" + CurCastTarget.name);
            _disposableCastTarget?.Dispose();
            _disposableCastTarget = (CurCastTarget as GameUnitCheckPoint).CurDirection.ReactiveProperty
                .CombineLatest((CurCastTarget as GameUnitCheckPoint).IsHavePower.ReactiveProperty, (dir, power) => (dir, power))
                .Skip(1).Subscribe(OnTargetRotate);
            return;
        }
        GameUnitPower power = hit.collider.GetComponent<GameUnitPower>();
        if (power != null)
        {
            CurCastTarget = null;
            DrawLaserLine(power.Transform.position - new Vector3(CurCastDirection.x * 4f, CurCastDirection.y * 4f, 0), false);
            return;
        }
        GameUnitBattery battery = hit.collider.GetComponent<GameUnitBattery>();
        if (battery != null)
        {
            LevelManager.Instance.SetCheckWin(true);
            CurCastTarget = battery;
            DrawLaserLine(CurCastTarget.Transform.position, false, OnDrawCompleted);
            return;
        }
    }
    public bool IsCanHavePower(Vector2 tagerDirection,Vector2 castDirection)
    {
        return (castDirection == new Vector2(-tagerDirection.y,tagerDirection.x))
            || (castDirection +tagerDirection == Vector2.zero);
    }
    public Vector2 GetCastDirection()
    {
        Vector2 direction = Vector2.zero;
        if ((CurDirection.Value + (CurPoweredTarget as ITransferPower).CurCastDirection) == Vector2.zero)
        {
            switch (CurDirection.Value)
            {
                case var a when a == Vector2.up:
                    direction = Vector2.right;
                    break;
                case var a when a == Vector2.left:
                    direction = Vector2.up;
                    break;
                case var a when a == Vector2.down:
                    direction = Vector2.left;
                    break;
                case var a when a == Vector2.right:
                    direction = Vector2.down;
                    break;
            }
        }
        else if ((CurPoweredTarget as ITransferPower).CurCastDirection == new Vector2(-CurDirection.Value.y, CurDirection.Value.x))
        {
            direction = CurDirection.Value;
        }
        return direction;
    }
    public void DrawLaserLine(Vector3 endPos,bool isImmediate, Action action = null)
    {
        if (action == null)
            action = () =>
            {
                IsCastingLine = false;
                if(CurCastTarget is GameUnitCheckPoint)
                    (CurCastTarget as GameUnitCheckPoint).SetCastline(false);
            };
        CurLine.Setup(FirePoint.transform.position, endPos, isImmediate, action);
    }
    public void OnDrawCompleted()
    {
        if (CurCastTarget is GameUnitCheckPoint)
        {
            IsCastingLine = false;
            if ((CurCastTarget as GameUnitCheckPoint).IsHavePower)
            {
                return;
            }
            if ((CurCastTarget as GameUnitCheckPoint).IsCanHavePower((CurCastTarget as RoateableObject).CurDirection.Value, CurCastDirection))
            {
                IsGivingPower.Value = true;
                (CurCastTarget as GameUnitCheckPoint).SetPower(this);
            }
        }
        else if(CurCastTarget is GameUnitBattery)
        {
            IsCastingLine = false;
            if ((CurCastTarget as GameUnitBattery).IsHavePower)
            {
                return;
            }
            IsGivingPower.Value = true;
            (CurCastTarget as GameUnitBattery).SetPower(this);
        }
        IsCastingLine = false;
    }
    public void OnTargetRotate((Vector2 targetDirection, bool isHavePower) value)
    {
        if(!IsHavePower.Value) return;
        //CurLine.DisablePrepare();
        Debug.Log(CurCastTarget.name + "OnTargetRotateCompleted" + Time.deltaTime);
        if (CurCastTarget == null) return;
        if (CurCastTarget is GameUnitCheckPoint)
        {
            if (!(CurCastTarget as IRecivePower).IsCanHavePower((CurCastTarget as RoateableObject).CurDirection.Value, CurCastDirection))
            {
                DrawLaserLine(CurCastTarget.transform.position - new Vector3(CurCastDirection.x * 4f, CurCastDirection.y * 4f, 0f), true);
                IsGivingPower.Value = false;
                Debug.Log(CurCastTarget.name + "Cant have power" + Time.deltaTime);
            }
            else
            {
                DrawLaserLine(CurCastTarget.transform.position, true, OnDrawCompleted);
                Debug.Log(CurCastTarget.name + "Can have power" + Time.deltaTime);
            }
        }
    }
    public override void OnResetLevel() 
    {         
        IsHavePower.Value = false;
        IsGivingPower.Value = false;
        IsCastingLine = false;
        CurCastTarget = null;
        CurPoweredTarget = null;
        CurLine.DisablePrepare();
        CurDirection.Value = Vector2.zero;
        _disposableCastTarget?.Dispose();
        _disposablePowerTarget?.Dispose();
    }
}
