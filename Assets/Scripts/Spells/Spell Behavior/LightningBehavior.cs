using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBehavior : ProjectileSpellBehavior
{
    private List<Enemy> markedEnemies;
    private List<Transform> points;
    public static LineRenderer lr;

    protected override void Start()
    { 
        base.Start();
        markedEnemies = new List<Enemy>();
        points = new List<Transform>();
        if(lr==null)
            lr = FindObjectOfType<LineRenderer>().GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    bool destroyed;
    void Update()
    {
        if (!destroyed)
        {
            if (direction == Vector3.zero)
            {
                destroyed = true;
                Destroy(gameObject, 1f);
            }
            transform.position += direction * spellData.Speed * Time.deltaTime; //set movement of BasicSpell

        }
    }
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")&&!destroyed)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            //Vector3 thisEnemyPosition = enemy.transform.position;

            //enemy.TakeDamage(GetCurrentDamage()); //Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers
            Destroy(gameObject,1f);
            destroyed = true;
            RunLightning(enemy);
            //Vector3 nextFace = NextEnemy(thisEnemyPosition);
            //DirectionChecker(nextFace);

            //markedEnemies.Add(col.gameObject);
        }
        //Debug.Log("collided w/ "  + col.gameObject.name);
        //if (col.CompareTag("Maze"))
        //{
        //    Destroy(gameObject);
        //}
    }

    private void RunLightning(Enemy startingEnemy)
    {
        Enemy currentEnemy = startingEnemy;
        markedEnemies.Add(currentEnemy);
        points.Add(currentEnemy.transform);

        for (int x=0; x<currentPierce; x++)
        {
            currentEnemy = NextEnemy(currentEnemy.transform.position);
            if (currentEnemy == null) break;
            markedEnemies.Add(currentEnemy);
            points.Add(currentEnemy.transform);
        }

        StartCoroutine(ShowLightningLines());

        foreach (Enemy enemy in markedEnemies)
        {
            enemy.SlowBy(0f, .5f);
            enemy.TakeDamage(currentDamage);
        }
    }
    private IEnumerator ShowLightningLines()
    {
        lr.startWidth = 0.5f;
        lr.endWidth = 0;
        lr.enabled = true;
        lr.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lr.SetPosition(i, points[i].position);
        }
        for(int i=0;i<8;i++)
        {
            lr.startWidth*=0.75f;
            yield return new WaitForSeconds(.1f);
        }
        lr.positionCount = 0;
        lr.enabled = false; //This isn't working, idk why - that's why lightning stays
    }

    public Enemy NextEnemy(Vector3 myPosition) //use position values from myPosition, incase myEnemy is destroyed
    {
        GameObject[] OtherEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (OtherEnemies.Length == 0) return null;

        Enemy NearestEnemy = null;
        float NearestDistance = float.MaxValue;
        float minimumDistance = .2f;

        foreach (GameObject enemyGameObject in OtherEnemies)
        {
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();
            if (!markedEnemies.Contains(enemy))
            {
                float distance = Vector3.Distance(myPosition, enemy.transform.position);
                if (distance < NearestDistance && distance > minimumDistance)
                {
                    NearestEnemy = enemy;
                    NearestDistance = distance;
                }
            }
        }
        return NearestEnemy;
    }

}
