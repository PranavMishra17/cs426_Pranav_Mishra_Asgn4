using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private static Transform spawnLoc;

    void Start()
    {
        GameState.spawnLoc = this.transform.GetChild(0);
    }

    public static Vector3 GetSpawnLoc()
    {
        return GameState.spawnLoc.position;
    }
}
