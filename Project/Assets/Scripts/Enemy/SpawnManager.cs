using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    //private Transform[] _spawners;
    //자식 오브젝트의 트랜스폼을 배열에 추가
    //스폰 시간을 각각 다르게

    [SerializeField]private EnemySpawner[] spawners;
    [SerializeField] private Transform[] spawnersTransform;

    private void Start()
    {
        spawners = transform.GetComponentsInChildren<EnemySpawner>();

        // spawnersTransform = new Transform[transform.childCount];
        //
        // int i = 0;
        // foreach (var sp in spawnersTransform)
        // {
        //     spawnersTransform[i] = sp;
        //     i++;
        //     sp.GetComponent<EnemySpawner>().
        // }

        foreach (var spawner in spawners)
        {
            var interval = Random.Range(3.0f, 8.0f);
            spawner.StartSpawn(interval);
        }
        
    }
}
