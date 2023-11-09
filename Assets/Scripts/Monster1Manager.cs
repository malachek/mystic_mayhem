using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Manager : MonoBehaviour
{
    [SerializeField] GameObject monster;
    [SerializeField] Vector2 spawnArea;
    [SerializeField] int spawnAmount;

    bool spawned = false;

    private void Update(){
        if(!(spawned)){
            SpawnEnemy(spawnAmount);
            spawned = true;
        }
    }

    private void SpawnEnemy(int amt){
        for (int i=0; i<amt; i++){
            Vector3 position = new Vector3(
                UnityEngine.Random.Range(-spawnArea.x, spawnArea.x),
                UnityEngine.Random.Range(-spawnArea.y, spawnArea.y),
                0f);
            
            GameObject newEnemy = Instantiate(monster);
            newEnemy.transform.position = position;
        }

    }
    // [SerializeField] float spawnTimer;

    // float timer;

    // private void Update(){
    //     timer -= Time.deltaTime;
    //     if(timer<0f){
    //         SpawnEnemy();
    //         timer = spawnTimer;
    //     }
    // }

    // private void SpawnEnemy(){
    //     Vector3 position = new Vector3(
    //         UnityEngine.Random.Range(-spawnArea.x, spawnArea.x),
    //         UnityEngine.Random.Range(-spawnArea.y, spawnArea.y),
    //         0f);
        
    //     GameObject newEnemy = Instantiate(monster);
    //     newEnemy.transform.position = position;
    // }
}
