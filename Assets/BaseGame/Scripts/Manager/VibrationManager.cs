using Lofelt.NiceVibrations;
using TW.Utility.DesignPattern;
using UnityEngine;


public class VibrationManager : Singleton<VibrationManager>
{
    [field: SerializeField] public bool IsActive {get; set;}

    private void Start()
    {
        SetVibration(InGameDataManager.Instance.InGameData.SettingData.VibrateActive);
    }

    private void SetVibration(bool value)
    {
        IsActive = value;
    }
    public void CallHaptic()
    {
        if (!IsActive) return;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
    }
}