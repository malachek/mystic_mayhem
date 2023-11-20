using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfinderAgent))]
public class Enemy : MonoBehaviour
{
    public GameObject PlayerTarget;

    [HideInInspector]
    public float LiveHP; // The real amount of health this enemy has.
    [Min(0)]
    public float MaxHP; // The maximum amount of health this enemy has.

    public float DamagePerHit = 1;
    [Min(0)]
    public float StoppingDistance = 1;


    private PathfinderAgent pathfinderAgent;
    private void Start()
    {
        LiveHP = MaxHP;
        pathfinderAgent = GetComponent<PathfinderAgent>();
    }
    private void FixedUpdate()
    {
        if(LiveHP<= 0)
        {
            Die();
        }
        else
        {
            if (PlayerTarget != null)
            {
                if (Vector2.Distance(transform.position, PlayerTarget.transform.position) > StoppingDistance)
                {
                    Follow();
                }
                else
                {
                    Attack();
                }
            }
        }
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
    private const int MAX_PATHTENSION = 100; // The max amount of temporary extensions allowed before a full recacluation is needed.
    private const int SHORTPATH_CUTOFF = 5; //No Tension Optimization will be performed if the enemy is closer than this from the player.

    public void Follow()
    {
        //TODO: Implement Resource Saving Optimization for Bee-Line walking.

        if(!pathfinderAgent.isPathing())
        {
            pathfinderAgent.PathfindTo(PlayerTarget.transform.position);
        }
        else
        {
            Vector2 targPos=  pathfinderAgent.movementTargets[pathfinderAgent.movementTargets.Count - 1];


            if (Vector2.Distance(targPos, PlayerTarget.transform.position) > StoppingDistance)
            {
                if (pathfinderAgent.pathTension >= MAX_PATHTENSION || Vector2.Distance(transform.position, PlayerTarget.transform.position) < SHORTPATH_CUTOFF)
                {
                    pathfinderAgent.PathfindTo(PlayerTarget.transform.position);
                }
                else
                {
                    pathfinderAgent.UnionPathfindTo(PlayerTarget.transform.position);
                }
            }
            
        }
    }
    public void Attack()
    {
        //TODO Attack Code Here
    }
}
