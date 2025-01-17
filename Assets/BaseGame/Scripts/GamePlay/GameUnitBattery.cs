using System.Collections;
using System.Collections.Generic;
using TW.Reactive.CustomComponent;
using UnityEngine;
using R3;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class GameUnitBattery : GameUnit ,IRecivePower
{
    [field: SerializeField]  public GameUnitType Type { get; private set; } = GameUnitType.Battery;
    [field: SerializeField]  public ReactiveValue<bool> IsHavePower { get; private set; } = new(false);
    [field: SerializeField]  public GameObject ImgHavePower { get; private set; }
    [field: SerializeField] public GameUnit CurPoweredTarget { get; private set; } = null;
    [field: SerializeField] public Vector2 CurDirection { get; private set; } = Vector2.zero;
    private IDisposable _disposablePowerTarget;
    private Tween _tween;

    public override void Setup()
    {
        switch (this.Transform.rotation.eulerAngles.z)
        {
            case 0:
                CurDirection = Vector2.up;
                break;
            case 90:
                CurDirection = Vector2.left;
                break;
            case 180:
                CurDirection = Vector2.down;
                break;
            case 270:
                CurDirection = Vector2.right;
                break;
        }
    }
    public override void Initialize()
    {
        this.IsHavePower.ReactiveProperty.Subscribe(OnHavePowerChange).AddTo(this);
    }
    public void OnHavePowerChange(bool isHavingPower)
    {
        Debug.Log(this.name + " SetPower" + isHavingPower);
        if (IsHavePower)
        {
            ImgHavePower.gameObject.SetActive(true);
            _tween?.Kill();
            _tween = ImgHavePower.transform.DOScaleY(0.5f, 0.25f).OnComplete(() =>
            {
                LevelManager.Instance.CheckWin();
            }).SetEase(Ease.Linear);
        }
        else
        {
            _tween?.Kill();
            _tween = ImgHavePower.transform.DOScaleY(0, 0.15f).OnComplete(() =>
            {
                ImgHavePower.gameObject.SetActive(false);
                CurPoweredTarget = null;
                _disposablePowerTarget?.Dispose();
            }).SetEase(Ease.Linear);
        }
    }
    public void SetPower(ITransferPower transferPower)
    {
        CurPoweredTarget = transferPower as GameUnit;
        IsHavePower.Value = transferPower.IsGivingPower.Value;
        _disposablePowerTarget?.Dispose();
        _disposablePowerTarget = (CurPoweredTarget as ITransferPower).IsGivingPower.ReactiveProperty.Skip(1).Subscribe(SubcribePowerGivingChange).AddTo(this);
    }
    public bool IsCanHavePower(Vector2 direction, Vector2 powerDirection)
    {
        return direction == powerDirection;
    }
    public void SubcribePowerGivingChange(bool isGivingPower)
    {
        IsHavePower.Value = isGivingPower;
    }
    public override void OnResetLevel()
    {
        IsHavePower.Value = false;
        CurPoweredTarget  = null;
        CurDirection= Vector2.zero;
        _disposablePowerTarget?.Dispose();
    }
}
