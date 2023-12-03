using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;

public class CastDirection : MonoBehaviour
{
    [SerializeField]
    PlayerMove playerMove;
    public Vector3 MouseDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        return new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0f);
    }
     
    public Vector3 ClosestEnemyDirection()
    {
        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (AllEnemies.Length == 0) return Vector3.zero;
        
        GameObject NearestEnemy = null;
        float NearestDistance = 20f;

        foreach (GameObject enemy in AllEnemies)
        {
            float distance = Vector3.Distance(this.transform.position, enemy.transform.position);
            if(distance < NearestDistance)
            {
                NearestEnemy = enemy;
                NearestDistance = distance;
            }
        }

        return new(NearestEnemy.transform.position.x - transform.position.x, NearestEnemy.transform.position.y - transform.position.y, 0f);
    }

    public Vector3 RandomDirection()
    {
        return Random.insideUnitCircle;
    }

    public Vector3 Behind()
    {
        Vector3 movementVector = playerMove.GetMovementVector();
        return new(-movementVector.x, -movementVector.y, 0f);
    }
}
