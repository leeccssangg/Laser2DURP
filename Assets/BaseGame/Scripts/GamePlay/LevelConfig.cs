using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TW.Utility.CustomComponent;
using UnityEngine;

public class LevelConfig : ACachedMonoBehaviour
{
    public int level;
    public List<GameUnitPower> power = new();
    public List<GameUnitCheckPoint> checkPoints = new();
    public List<GameUnitBattery> batteries = new();

#if UNITY_EDITOR
    [Button]
    public void SaveLevelConfig()
    {
        LevelGlobalConfig.Instance.SaveLevelConfig(this);
    }
    [Button]
    public void LoadLevelConfig()
    {
        foreach (var item in power)
        {
            DestroyImmediate(item.gameObject);
        }
        foreach (var item in checkPoints)
        {
            DestroyImmediate(item.gameObject);
        }
        foreach (var item in batteries)
        {
            DestroyImmediate(item.gameObject);
        }
        power.Clear();
        checkPoints.Clear();
        batteries.Clear();
        LevelConfigData levelConfigData = LevelGlobalConfig.Instance.GetLevelConfigData(level);
        for (int i = 0; i < levelConfigData.gameUnitConfigDatas.Count; i++)
        {
            GameUnitConfigData config = levelConfigData.gameUnitConfigDatas[i];
            switch (config.type)
            {
                case GameUnitType.Power:
                    GameObject prefabPower = Resources.Load<GameObject>("GamePlay/GameUnitPower");
                    GameObject gameObjectPower = GameObject.Instantiate(prefabPower);
                    GameUnitPower gameUnitPower = gameObjectPower.GetComponent<GameUnitPower>();
                    gameUnitPower.transform.SetParent(this.Transform);
                    gameUnitPower.transform.localScale = Vector3.one;
                    gameUnitPower.transform.localPosition = config.position;
                    gameUnitPower.transform.rotation = Quaternion.Euler(0, 0, config.GetCurrentRotation());
                    power.Add(gameUnitPower);
                    break;
                case GameUnitType.Checkpoint:
                    GameObject prefabCheckPoint = Resources.Load<GameObject>("GamePlay/GameUnitCheckPoint");
                    GameObject gameObjectCheckPoint = GameObject.Instantiate(prefabCheckPoint);
                    GameUnitCheckPoint gameUnitCheckPoint = gameObjectCheckPoint.GetComponent<GameUnitCheckPoint>();
                    gameUnitCheckPoint.transform.SetParent(this.Transform);
                    gameUnitCheckPoint.transform.localScale = Vector3.one;
                    gameObjectCheckPoint.transform.localPosition = config.position;
                    gameUnitCheckPoint.transform.rotation = Quaternion.Euler(0, 0, config.GetCurrentRotation());
                    checkPoints.Add(gameUnitCheckPoint);
                    break;
                case GameUnitType.Battery:
                    GameObject prefabBattery = Resources.Load<GameObject>("GamePlay/GameUnitBattery");
                    GameObject gameObjectBattery = GameObject.Instantiate(prefabBattery);
                    GameUnitBattery gameUnitBattery = gameObjectBattery.GetComponent<GameUnitBattery>();
                    gameUnitBattery.transform.SetParent(this.Transform);
                    gameUnitBattery.transform.localScale = Vector3.one;
                    gameUnitBattery.transform.localPosition = config.position;
                    gameUnitBattery.transform.rotation = Quaternion.Euler(0, 0, config.GetCurrentRotation());
                    batteries.Add(gameUnitBattery);
                    break;
            }
        }
    }
#endif
}
