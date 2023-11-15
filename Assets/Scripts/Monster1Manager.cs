using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Manager : MonoBehaviour
{
    [SerializeField] GameObject monster;
    [SerializeField] float scaleability;
    [SerializeField] int spawnAmount;
    [SerializeField] int spawnlimit;
    [SerializeField] float spawnTimer;
    

    float timer;

    private float halfed_width;
    private float halfed_height;
    
    bool spawned = false;

    private Vector2 screenBounds;

    void Start(){
        
        timer = 0;
        halfed_height = Screen.height/scaleability / 2;
        halfed_width = Screen.width/scaleability / 2;
    }
    

    private void SpawnEnemy(int amt)
    {
        var playerPos = GameObject.Find("Player").transform.position;

        float screenie_north = playerPos.y + halfed_height;
        float screenie_south = playerPos.y - halfed_height;
        float screenie_east  = playerPos.x + halfed_width;
        float screenie_west  = playerPos.x - halfed_width;

        Debug.Log(screenie_north + " - " + screenie_south + " - " + screenie_east + " - " + screenie_west);
        for (int i=0; i<amt; i++)
        {
            int direction = Random.Range(0, 4); //gets range between 0 and 3 inc
            

            // playerpos.x + halfed_width
            // playerpos.x - halfed_width
            // playerpos.y + halfed_height
            // playerpos.y - halfed_height

            //0-left
            //1-up
            //2-right
            //3-down

            
            float x = 0f;
            float y = 0f;


            if (direction == 0){
                // LEFT /  west
                x = UnityEngine.Random.Range(screenie_west - spawnlimit, screenie_west);
                y = UnityEngine.Random.Range(screenie_south, screenie_north);
                
            }else if(direction == 1){
                // UP / north
                x = UnityEngine.Random.Range(screenie_west, screenie_east);
                y = UnityEngine.Random.Range(screenie_north, screenie_north + spawnlimit);
                
            }else if(direction == 2){
                // RIGHT / east
                x = UnityEngine.Random.Range(screenie_east, screenie_east + spawnlimit);
                y = UnityEngine.Random.Range(screenie_south, screenie_north);
               
            }else{
                // DOWN / south
                x = UnityEngine.Random.Range(screenie_west, screenie_east);
                y = UnityEngine.Random.Range(screenie_south - spawnlimit, screenie_south);
                
            }
            Debug.Log(x + " --- " + y);
            Vector3 position = new Vector3(x, y, 0f);
            GameObject newEnemy = Instantiate(monster);
            newEnemy.SetActive(true);
            newEnemy.transform.position = position;

        }
    }

    void Update(){


        if(timer <= spawnTimer)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            SpawnEnemy(spawnAmount);
        }
    }

    void LateUpdate(){
    
        if(!(spawned)){
            SpawnEnemy(spawnAmount);
            spawned = true;
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
