using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Manager : MonoBehaviour
{
    [SerializeField] GameObject monster;
    [SerializeField] Vector2 spawnArea;
    [SerializeField] int spawnAmount;

    bool spawned = false;

    private Vector2 screenBounds;

    void Start(){
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(screenBounds.x, screenBounds.y, 0f));
        timer = 0;
    }

    void LateUpdate(){
    
        if(!(spawned)){
            SpawnEnemy(spawnAmount);
            spawned = true;
        }
    }

    private void SpawnEnemy(int amt){
        for (int i=0; i<amt; i++){

            int direction = Random.Range(0, 4); //gets range between 0 and 3 inc

            //0-left
            //1-up
            //2-right
            //3-down


            if (direction == 0){
                Vector3 position = new Vector3(
                    UnityEngine.Random.Range(screenBounds.x - 10, screenBounds.x-2),
                    UnityEngine.Random.Range(-spawnArea.y, spawnArea.y),
                    0f);
                GameObject newEnemy = Instantiate(monster);
                newEnemy.transform.position = position;
            }else if(direction == 1){
                Vector3 position = new Vector3(
                    UnityEngine.Random.Range(-spawnArea.x, spawnArea.x),
                    UnityEngine.Random.Range((screenBounds.y*-1) +10, (screenBounds.y*-1)+2),
                    0f);
                GameObject newEnemy = Instantiate(monster);
                newEnemy.transform.position = position;
            }else if(direction == 2){
                Vector3 position = new Vector3(
                    UnityEngine.Random.Range(screenBounds.x*-1 + 10, screenBounds.x*-1 +2),
                    UnityEngine.Random.Range(-spawnArea.y, spawnArea.y),
                    0f);
                GameObject newEnemy = Instantiate(monster);
                newEnemy.transform.position = position;
            }else{
                Vector3 position = new Vector3(
                    UnityEngine.Random.Range(-spawnArea.x, spawnArea.x),
                    UnityEngine.Random.Range(screenBounds.y -10, screenBounds.y -2),
                    0f);
                GameObject newEnemy = Instantiate(monster);
                newEnemy.transform.position = position;
            }
            
            
            
        }

    }
    [SerializeField] float spawnTimer;

    float timer;

    private void Update(){
        Debug.Log(timer);
        if(timer <= spawnTimer){
            timer += Time.deltaTime;
        }
        else{
            timer = 0;
            SpawnEnemy(spawnAmount);
            Debug.Log("AAAAH");
        }
    }

    // private void SpawnEnemy(){
    //     Vector3 position = new Vector3(
    //         UnityEngine.Random.Range(-spawnArea.x, spawnArea.x),
    //         UnityEngine.Random.Range(-spawnArea.y, spawnArea.y),
    //         0f);
        
    //     GameObject newEnemy = Instantiate(monster);
    //     newEnemy.transform.position = position;
    // }
}
