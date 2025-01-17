using UnityEngine;
using Sirenix.Utilities;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpriteGlobalConfig", menuName = "GlobalConfigs/SpriteGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class SpriteGlobalConfig : GlobalConfig<SpriteGlobalConfig>
{
    public List<ResourceSprite> sprites;

    public Sprite GetSpriteByResourceType(GameResource.Type type)
    {
        for(int i = 0;i< sprites.Count; i++)
        {
            if (sprites[i].type == type)
            {
                return sprites[i].sprite;
            }
        }
        return null;
    }
}
[System.Serializable]
public class ResourceSprite
{
    public GameResource.Type type;
    public Sprite sprite;
}