using UnityEngine;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelGlobalConfig", menuName = "GlobalConfigs/LevelGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class LevelGlobalConfig : GlobalConfig<LevelGlobalConfig>
{
    public List<LevelConfigData> LevelConfigs = new();

    public LevelConfigData GetLevelConfigData(int level)
    {
        for(int i = 0; i < LevelConfigs.Count; i++)
        {
            if(LevelConfigs[i].level == level)
            {
                return LevelConfigs[i];
            }
        }
        return null;
    }
#if UNITY_EDITOR
    public void SaveLevelConfig(LevelConfig levelConfig)
    {
        EditorUtility.SetDirty(this);
        LevelConfigData levelConfigData = new();
        levelConfigData.level = levelConfig.level;
        for (int i = 0; i < levelConfig.power.Count; i++)
        {
            GameUnitConfigData gameUnitConfigData = new GameUnitConfigData();
            gameUnitConfigData.type = GameUnitType.Power;
            gameUnitConfigData.position = levelConfig.power[i].transform.position;
            gameUnitConfigData.direction = GetGameUnitDirection(levelConfig.power[i].transform.rotation.eulerAngles.z);
            levelConfigData.gameUnitConfigDatas.Add(gameUnitConfigData);
        }
        for (int i = 0; i < levelConfig.checkPoints.Count; i++)
        {
            GameUnitConfigData gameUnitConfigData = new GameUnitConfigData();
            gameUnitConfigData.type = GameUnitType.Checkpoint;
            gameUnitConfigData.position = levelConfig.checkPoints[i].transform.position;
            gameUnitConfigData.direction = GetGameUnitDirection(levelConfig.checkPoints[i].transform.rotation.eulerAngles.z);
            levelConfigData.gameUnitConfigDatas.Add(gameUnitConfigData);
        }
        for (int i = 0; i < levelConfig.batteries.Count; i++)
        {
            GameUnitConfigData gameUnitConfigData = new GameUnitConfigData();
            gameUnitConfigData.type = GameUnitType.Battery;
            gameUnitConfigData.position = levelConfig.batteries[i].transform.position;
            gameUnitConfigData.direction = GetGameUnitDirection(levelConfig.batteries[i].transform.rotation.eulerAngles.z);
            levelConfigData.gameUnitConfigDatas.Add(gameUnitConfigData);
        }
        LevelConfigData curLevelConfig = GetLevelConfigData(levelConfig.level);
        if(curLevelConfig!=null)
        {
            curLevelConfig.cameraSize = levelConfig.CameraSize;
            curLevelConfig.gameUnitConfigDatas.Clear();
            curLevelConfig.gameUnitConfigDatas.AddRange(levelConfigData.gameUnitConfigDatas);
        }
        else
        {
            levelConfigData.cameraSize = levelConfig.CameraSize;
            LevelConfigs.Add(levelConfigData);
        }
        AssetDatabase.SaveAssets();
    }
#endif
    public Vector2 GetGameUnitDirection(float z)
    {
        Vector2 direction = Vector2.zero;
        switch (z)
        {
            case 0:
                direction = Vector2.up;
                break;
            case 90:
                direction = Vector2.left;
                break;
            case 180:
                direction = Vector2.down;
                break;
            case 270:
                direction = Vector2.right;
                break;
        }
        return direction;
    }
}
[System.Serializable]
public class LevelConfigData
{
    public int level;
    public List<GameUnitConfigData> gameUnitConfigDatas = new();
    public float cameraSize;
}
[System.Serializable]
public class GameUnitConfigData
{
    public GameUnitType type;
    public Vector2 position;
    public Vector2 direction;

    public float GetCurrentRotation()
    {
        float rotation = 0;
        if (direction == Vector2.up)
        {
            rotation = 0;
        }
        else if (direction == Vector2.left)
        {
            rotation = -270;
        }
        else if (direction == Vector2.down)
        {
            rotation = -180;
        }
        else if (direction == Vector2.right)
        {
            rotation = -90;
        }
        return rotation;
    }
}