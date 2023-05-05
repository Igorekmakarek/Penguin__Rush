using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint instance = null;
    public Transform[] spawnPoints = new Transform[12];

    private void Start()
    {
        instance = this;

        for (int i = 0; i < transform.childCount; i++)
            spawnPoints[i] = transform.GetChild(i).transform;
    }

    public Vector3 RandomSpawnPoint()
    {
        int randnum = Random.Range(0, transform.childCount);

        return spawnPoints[randnum].transform.position;
    }
}
