using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    bool isSpawning = false;
    public float minTime = 2.0f;
    public float maxTime = 4.0f;
    public GameObject mobPrefab;


    IEnumerator SpawnObject(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Vector3 spawnPosition = new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z);
        Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
   
        isSpawning = false;
    }

    void Update () 
    {
        if(!GameManager.Instance.IsGameOver() && !GameManager.Instance.IsStageClear()){
            if(!isSpawning)
            {
                isSpawning = true;
                StartCoroutine(SpawnObject(Random.Range(minTime, maxTime)));
            }
        }
    }

}
