using System;
using Cysharp.Threading.Tasks;
using TW.UGUI.MVPPattern;
using UnityEngine;
using R3;
using Sirenix.OdinInspector;
using TMPro;
using TW.Reactive.CustomComponent;
using TW.UGUI.Core.Screens;
using UnityEngine.UI;
using TW.UGUI.Core.Views;
using TW.UGUI.Core.Modals;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using Pextension;

[Serializable]
public class ScreenInGameContext 
{
    public static class Events
    {
        public static Subject<Unit> SampleEvent { get; set; } = new();
        public static Action LoadLevel { get; set; }
    }
    
    [HideLabel]
    [Serializable]
    public class UIModel : IAModel
    {
        [field: Title(nameof(UIModel))]
        [field: SerializeField] public ReactiveValue<int> CurLevel { get; private set; }
        [field: SerializeField] public ReactiveValue<bool> IsWinning { get; private set; }
        public UniTask Initialize(Memory<object> args)
        {
            //CurrentMapProcess = LevelManager.Instance.CurrentMapProcess;
            CurLevel = InGameDataManager.Instance.InGameData.LevelSaveData.Level;
            IsWinning = LevelManager.Instance.IsWinning;
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIView : IAView
    {
        [field: Title(nameof(UIView))]
        [field: SerializeField] public CanvasGroup MainView {get; private set;}
        [field: SerializeField] public TextMeshProUGUI TxtLevel { get; private set; }
        [field: SerializeField] public Button BtnSetting { get; private set; }
        [field: SerializeField] public Transform TfLevelContainer { get; private set; }
        //[field: SerializeField] public UIBooster BtnBoosterSortCup { get; private set; }
        //[field: SerializeField] public UIBooster BtnBoosterShuffleBox { get; private set; }
        //[field: SerializeField] public UIBooster BtnBoosterSortBox { get; private set; }
        //[field: SerializeField] public UIBooster BtnBoosterSort3Box { get; private set; }


        public UniTask Initialize(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIPresenter : IAPresenter, IScreenLifecycleEventSimple
    {
        [field: SerializeField] public UIModel Model { get; private set; }
        [field: SerializeField] public UIView View { get; set; } = new();
        [field: SerializeField] public List<GameUnitPower> CurListPower { get; private set; } = new();
        [field: SerializeField] public List<GameUnitCheckPoint> CurListCheckPoint { get; private set; } = new();
        [field: SerializeField] public List<GameUnitBattery> CurListBattery { get; private set; } = new();

        public async UniTask Initialize(Memory<object> args)
        {
            await Model.Initialize(args);
            await View.Initialize(args);


            Model.CurLevel.ReactiveProperty.Subscribe(SetupUI).AddTo(View.MainView);

            View.BtnSetting.SetOnClickDestination(OnClickBtnSettings);

            //Events.LoadLevel = LoadLevel;

            //LoadLevel();
            //View.BtnBoosterSortCup.Init(OnClickBtnAddBoosterSortCup);
            //View.BtnBoosterShuffleBox.Init(OnClickBtnAddBoosterShuffleBox);
            //View.BtnBoosterSortBox.Init(OnClickBtnAddBoosterSortBox);
            //View.BtnBoosterSort3Box.Init(OnClickBtnAddBoosterSort3Box);

            //GamePlay.Instance.NewBoosterUnlocked.ReactiveProperty.Subscribe(OnUnlockBooster).AddTo(View.MainView);
        }
        private void SetupUI(int level)
        {
            View.TxtLevel.SetText($"<style=h3>LEVEL {level}");
        }
        public UniTask Cleanup(Memory<object> args)
        {
            Events.LoadLevel = null;
            return UniTask.CompletedTask;
        }
        public void DidPushEnter(Memory<object> args)
        {
            //LoadLevel();
        }
        private void OnClickBtnSettings(Unit _)
        {
            //ViewOptions options = new(nameof(ModalSettingsInGame));
            //ModalContainer.Find(ContainerKey.Modals).PushAsync(options);
        }
    }
}