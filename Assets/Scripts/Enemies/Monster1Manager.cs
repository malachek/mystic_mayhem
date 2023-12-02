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

    private Pathfinder WorldGrid;
    private GameObject Player;

    public static int MonsterDespawnedDebt=0;
    void Start()
    {

        WorldGrid = GameObject.FindObjectOfType<Pathfinder>();
        Player = GameObject.Find("Player");
        MonsterDespawnedDebt = 0;
        timer = 0;
        halfed_height = Screen.height / scaleability / 2;
        halfed_width = Screen.width / scaleability / 2;
    }


    private IEnumerator SpawnEnemy(int amt)
    {

        if(!WorldGrid.isPosOk(Player.transform.position))
        {
            timer = spawnTimer + 1;
            Debug.LogWarning("Can't spawn enemies from an invalid position!");
            yield break;
        }


        //Debug.Log(screenie_north + " - " + screenie_south + " - " + screenie_east + " - " + screenie_west);

        int failedAttempts = 0;

        for (int i = 0; i < amt; i++)
        {
            var playerPos = Player.transform.position;
            int direction = Random.Range(0, 4); //gets range between 0 and 3 inc

            float screenie_north = playerPos.y + halfed_height;
            float screenie_south = playerPos.y - halfed_height;
            float screenie_east = playerPos.x + halfed_width;
            float screenie_west = playerPos.x - halfed_width;

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


            if (direction == 0)
            {
                // LEFT /  west
                x = UnityEngine.Random.Range(screenie_west - spawnlimit, screenie_west);
                y = UnityEngine.Random.Range(screenie_south, screenie_north);

            }
            else if (direction == 1)
            {
                // UP / north
                x = UnityEngine.Random.Range(screenie_west, screenie_east);
                y = UnityEngine.Random.Range(screenie_north, screenie_north + spawnlimit);

            }
            else if (direction == 2)
            {
                // RIGHT / east
                x = UnityEngine.Random.Range(screenie_east, screenie_east + spawnlimit);
                y = UnityEngine.Random.Range(screenie_south, screenie_north);

            }
            else
            {
                // DOWN / south
                x = UnityEngine.Random.Range(screenie_west, screenie_east);
                y = UnityEngine.Random.Range(screenie_south - spawnlimit, screenie_south);

            }
            //Debug.Log(x + " --- " + y);
            Vector3 position = new Vector3(x, y, 0f);


            //isPosOk checks to see if the position is not out of bounds or in a wall
            //PositionCanSeePlayer forces the enemy to spawn with beeline capablities to the player (For no lag)
            if (WorldGrid.isPosOk(position) && PositionCanSeePlayer(position))
            {
                GameObject newEnemy = Instantiate(monster);
                newEnemy.SetActive(true);
                newEnemy.transform.position = position;
                if(newEnemy.GetComponent<PathfinderAgent>()!=null)
                {
                    newEnemy.GetComponent<PathfinderAgent>().WorldGrid = WorldGrid;
                }
                if (newEnemy.GetComponent<Enemy>() != null)
                {
                    newEnemy.GetComponent<Enemy>().PlayerTarget = Player;
                }
            }
            else
            {
                i--;
                failedAttempts++; //i-- on its own could cause infinite loop, which is kinda bad, so we use a failsafe timeout if there is literally no spot to spawn :(.
                if (failedAttempts > 999)
                {
                    Debug.LogWarning("No possible location found for spawning enemies!");
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }

    }
    public bool PositionCanSeePlayer(Vector2 from)
    {
        Vector2 target = Player.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(from, (target - from).normalized, Vector2.Distance(from, target), LayerMask.GetMask("Wall"));
        //Debug.DrawRay(from, (target -from).normalized * (Vector2.Distance(from, target)-1), Color.blue, 10);
        if (hit.collider == null)
        {
            return true; //If I can see the player and I'm not blocked by any walls, I like this spawn spot!
        }
        return false;
    }

    void Update()
    {

        if(MonsterDespawnedDebt>0)
        {
            StartCoroutine(SpawnEnemy(MonsterDespawnedDebt));
            MonsterDespawnedDebt = 0;
        }
        if (timer <= spawnTimer)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            StartCoroutine(SpawnEnemy(spawnAmount));
        }
    }

    void LateUpdate()
    {

        if (!(spawned))
        {
            StartCoroutine(SpawnEnemy(spawnAmount));
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
