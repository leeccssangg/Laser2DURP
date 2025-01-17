using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CachedCollider 
{
    private static Dictionary<Collider, RoateableObject> CacheDynamicUnits = new Dictionary<Collider, RoateableObject>();
    public static RoateableObject GetRotateableObject(this Collider collider)
    {
        if (CacheDynamicUnits.TryGetValue(collider, out RoateableObject dynamicUnit))
        {
            return dynamicUnit;
        }

        RoateableObject newUnit = collider.GetComponent<RoateableObject>();
        CacheDynamicUnits.Add(collider, newUnit);
        return newUnit;
    }
}
