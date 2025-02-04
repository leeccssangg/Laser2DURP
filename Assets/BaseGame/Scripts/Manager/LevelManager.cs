using CodeStage.AntiCheat.Storage;
using MemoryPack;
using Sirenix.OdinInspector;
using TW.Reactive.CustomComponent;
using TW.Utility.DesignPattern;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Pextension;
using System;
using Cysharp.Threading.Tasks;
using TW.UGUI.Core.Modals;
using TW.UGUI.Core.Views;

public class LevelManager : Singleton<LevelManager>
{
    private MiniPool<GameUnitPower> _gameUnitPowerPool = new();
    private MiniPool<GameUnitCheckPoint> _gameUnitCheckPointPool = new();
    private MiniPool<GameUnitBattery> _gameUnitBatteryPool = new();

    [field: SerializeField] public GameUnitPower PrefabPower { get; private set; }
    [field: SerializeField] public GameUnitCheckPoint PrefabCheckPoint { get; private set; }
    [field: SerializeField] public GameUnitBattery PrefabBattery { get; private set; }

    [field: SerializeField] public Transform TfContainer { get; private set; }
    [field: SerializeField] public LevelSaveData LevelSaveData { get; private set; } = new();
    [field: SerializeField] public ReactiveValue<int> CurrentLevel { get; private set; } = new(1);
    [field: SerializeField] public List<GameUnitPower> Powers { get; private set; } = new();
    [field: SerializeField] public List<GameUnitCheckPoint> CheckPoints { get; private set; } = new();
    [field: SerializeField] public List<GameUnitBattery> Batteries { get; private set; } = new();
    [field: SerializeField] public ReactiveValue<bool> IsWinning { get; private set; } = new(false);
    [field: SerializeField] public ReactiveValue<bool> IsCheckingWin { get; private set; } = new(false);

    #region Unity Functions
    private void Awake()
    {
        _gameUnitBatteryPool.OnInit(PrefabBattery, 5, TfContainer);
        _gameUnitCheckPointPool.OnInit(PrefabCheckPoint, 5, TfContainer);
        _gameUnitPowerPool.OnInit(PrefabPower, 5, TfContainer);
    }
    private void Start()
    {
        LoadData();
    }
    #endregion
    #region Save Load Functions
    private void LoadData()
    {
        LevelSaveData = InGameDataManager.Instance.InGameData.LevelSaveData;
        CurrentLevel = LevelSaveData.Level;
    }
    public void SaveData()
    {
        LevelSaveData.Level.Value = CurrentLevel;
        InGameDataManager.Instance.SaveData();
    }
    #endregion
    #region Manager Functions
    public void SetCurLevel(int level)
    {
        IsWinning.Value = false;
        IsCheckingWin.Value = false;
        LevelConfigData levelConfigData = LevelGlobalConfig.Instance.GetLevelConfigData(level);
        Camera.main.orthographicSize = levelConfigData.cameraSize;
        for (int i = 0; i < levelConfigData.gameUnitConfigDatas.Count; i++)
        {
            GameUnitConfigData config = levelConfigData.gameUnitConfigDatas[i];
            switch (config.type)
            {
                case GameUnitType.Power:
                    GameUnitPower gameUnitPower = _gameUnitPowerPool.Spawn(TfContainer.position,Quaternion.identity);
                    gameUnitPower.transform.localPosition = config.position;
                    gameUnitPower.transform.rotation = Quaternion.Euler(0, 0, config.GetCurrentRotation());
                    Powers.Add(gameUnitPower);
                    break;
                case GameUnitType.Checkpoint:
                    GameUnitCheckPoint gameUnitCheckPoint = _gameUnitCheckPointPool.Spawn(TfContainer.position, Quaternion.identity);
                    gameUnitCheckPoint.transform.localPosition = config.position;
                    gameUnitCheckPoint.transform.rotation = Quaternion.Euler(0, 0, config.GetCurrentRotation());
                    CheckPoints.Add(gameUnitCheckPoint);
                    break;
                case GameUnitType.Battery:
                    GameUnitBattery gameUnitBattery = _gameUnitBatteryPool.Spawn(TfContainer.position, Quaternion.identity);
                    gameUnitBattery.transform.localPosition = config.position;
                    gameUnitBattery.transform.rotation = Quaternion.Euler(0, 0, config.GetCurrentRotation());
                    Batteries.Add(gameUnitBattery);
                    break;
            }
        }
        foreach (var item in Powers)
        {
            item.Setup();
            item.Initialize();
        }
        foreach (var item in CheckPoints)
        {
            item.Setup();
            item.Initialize();
        }
        foreach (var item in Batteries)
        {
            item.Setup();
            item.Initialize();
        }
        InputManager.Instance.SetActive(true);
    }
    public void ClearCurLevel()
    {
        InputManager.Instance.SetActive(false);
        foreach (var power in Powers)
        {
            power.OnResetLevel();
        }
        foreach (var checkPoint in CheckPoints)
        {
            checkPoint.OnResetLevel();
        }
        foreach (var battery in Batteries)
        {
            battery.OnResetLevel();
        }
        Powers.Clear();
        CheckPoints.Clear();
        Batteries.Clear();
        _gameUnitBatteryPool.Collect();
        _gameUnitCheckPointPool.Collect();
        _gameUnitPowerPool.Collect();
    }
    public void SetCheckWin(bool isCheckWin)
    {
        IsCheckingWin.Value = isCheckWin;
    }
    public bool CheckWin()
    {
        for (int i = 0; i < Batteries.Count; i++)
        {
            if (!Batteries[i].IsHavePower)
            {
                SetCheckWin(false);
                return false;
            }
        }
        IsWinning.Value = true;
        OnWinning().Forget();
        //SetCheckWin(false);
        return true;
    }
    private async UniTask OnWinning()
    {
        await UniTask.Delay(500, cancellationToken: this.GetCancellationTokenOnDestroy());
        ViewOptions options = new(nameof(ModalWin));
        await ModalContainer.Find(ContainerKey.Modals).PushAsync(options);
    }
    #endregion
}
