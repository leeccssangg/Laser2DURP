using System.Collections;
using System.Collections.Generic;
using TW.Utility.CustomComponent;
using UnityEngine;

public enum GameUnitType
{
    None = -1,
    Power,
    Battery,
    Checkpoint,
    Line,
}
public class GameUnit : ACachedMonoBehaviour
{
    [field: SerializeField] public GameUnitType UnitType { get; private set; } = GameUnitType.None;

    public virtual void Setup()
    {

    }
    public virtual void Initialize()
    {

    }
    public virtual void OnResetLevel()
    {

    }
}
