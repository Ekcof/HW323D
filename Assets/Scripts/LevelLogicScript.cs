using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLogicScript : MonoBehaviour
{
    public static LevelLogicScript Instance = null;
    public Transform[] NavMeshZones;

#region Singleton
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
#endregion
    public Transform[] GetNavMeshZones()
    {
        return NavMeshZones;
    }
}
