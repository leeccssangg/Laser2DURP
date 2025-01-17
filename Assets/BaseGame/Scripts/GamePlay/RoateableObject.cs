using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TW.Reactive.CustomComponent;
using TW.Utility.CustomComponent;
using R3;
using UnityEngine;
using UnityEngine.UI;


public enum ObjectType
{
    None = -1,
    Checkpoint,
    Power,
    Battery,

}
public class RoateableObject : GameUnit
{
    [field: SerializeField] public float RotationSpeed { get; set; } = 0.25f;
    [field: SerializeField] public bool IsRotate { get; set; } = false;
    [field: SerializeField] public float CurrentRotation { get; set; } = 0f;
    [field: SerializeField] public float NextRotation { get; set; } = 0f;
    [field: SerializeField] public ReactiveValue<Vector2> CurDirection { get; set; } = new(Vector2.zero);

    public override void Setup()
    {
        //BtnRotate.SetOnClickDestination(Rotate);
        OnAwake();
    }
    public virtual void Rotate()
    {
        if (LevelManager.Instance.IsCheckingWin || LevelManager.Instance.IsWinning)
        {
            InputManager.Instance.SelectGameUnit = null;
            return;
        }
        if (!IsRotate)
        {
            OnStartRotate();
            NextRotation = CurrentRotation - 90;
            transform.DORotate(new Vector3(0, 0, NextRotation), RotationSpeed)
                .OnComplete(OnRotateCompleted);
        }
        else
        {
            InputManager.Instance.SelectGameUnit = null;
        }
    }
    public virtual void OnAwake()
    {
        //BtnRotate.SetOnClickDestination(Rotate);
        GetCurrentDirection();
    }
    public virtual void OnRotateCompleted()
    {
        CurrentRotation = NextRotation;
        UpdateDirection();
        //BtnRotate.interactable = true;
        IsRotate = false;
        InputManager.Instance.SelectGameUnit = null;
        //LevelManager.Instance.OnRotateGameUnit();
    }
    public virtual void OnStartRotate()
    {
        IsRotate = true;
        //BtnRotate.interactable = false;
        //LevelManager.Instance.OnStartRotateGameUnit();
    }
    public virtual void UpdateDirection()
    {

    }
    public virtual void GetCurrentDirection()
    {
        switch (this.Transform.rotation.eulerAngles.z)
        {
            case 0:
                CurDirection = new(Vector2.up);
                CurrentRotation = 0;
                NextRotation = 0;
                break;
            case 90:
                CurDirection = new(Vector2.left);
                CurrentRotation = -270;
                NextRotation = -270;
                break;
            case 180:
                CurDirection = new(Vector2.down);
                CurrentRotation = -180;
                NextRotation = -180;
                break;
            case 270:
                CurDirection = new(Vector2.right);
                CurrentRotation = -90;
                NextRotation = -90;
                break;
        }
    }
}
