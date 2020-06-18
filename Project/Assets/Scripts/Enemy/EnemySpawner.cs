using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    //일정한 시간간격으로 등록된 enemy를 생성한다.

    [SerializeField] private float spawnInterval = 5;
    public GameObject enemyPrefab;

    //private float test = 0;

    private void Start()
    {
    }


    public void StartSpawn(float interval)
    {
        spawnInterval = interval;
        StartCoroutine(SpawnEnemy(interval));
    }

    //public void SpawnStart()
    IEnumerator SpawnEnemy(float interval)
    {
        while (true)
        {
            // var spawnPosition = transform.position;
            // spawnPosition += new Vector3(0,1,0);

            if (GameManager.gm.gameState == GameManager.GameState.Run)
            {
                var rot = new Vector3(0, Random.Range(0, 3), 0);

                //var pos = Quaternion.AngleAxis(Random.Range(0, 359), Vector3.up) * rot;

                var pos = GenerateSpawnPoint(3.0f);


                // if (test < vec.magnitude)
                // {
                //     test = vec.magnitude;
                //     print(test);
                // }

                Instantiate(enemyPrefab, transform.position + pos, Quaternion.identity);
            }

            yield return new WaitForSeconds(interval);
        }
    }

    private Vector3 GenerateSpawnPoint(float radius)
    {
        int num = 0;
        while (true)
        {
            var angle = Random.Range(0, 2 * Mathf.PI);
            var range = Random.Range(0.0f, radius);
            var vec = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            var check = vec + Vector3.up * enemyPrefab.GetComponent<CapsuleCollider>().height * 0.5f;


            Collider[] cols = Physics.OverlapSphere(check, 0.5f);

            if (cols.Length == 0)
            {
                return vec;
            }

            num++;
            if (num > 50)
            {
                return vec;
            }
        }
    }
}