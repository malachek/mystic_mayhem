using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class PathfinderAgent : MonoBehaviour
{
    public Pathfinder WorldGrid;
    [HideInInspector]
    public List<Vector2> movementTargets = new List<Vector2>();
    public float MovementSpeed = 0.5f;
    public bool ListensForMouseDebug;
    private int refiningOffset = -1; //When refiningOffset = 0, movementTargets is recalculated to be the true fastest path.
    [HideInInspector]
    public int pathTension; //More useful for Enemy scripts, but this holds the amount of times a path has been Unioned with the old one.
    [Min(0)]
    public float minMovementTargDist=0.1f;
    void FixedUpdate()
    {
        if (ListensForMouseDebug)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                PathfindTo(mousPos);
            }
        }

        if (movementTargets.Count > 0)
        {
            if (Vector2.Distance(transform.position, movementTargets[0]) < minMovementTargDist)
            {
                movementTargets.RemoveAt(0);
                if(refiningOffset >= 0)
                {
                    refiningOffset--;

                    if (refiningOffset == 0&&movementTargets.Count>1)
                        PathfindTo(movementTargets[movementTargets.Count - 1]);
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, movementTargets[0], MovementSpeed);
            }
        }
        else
        {
            pathTension = 0;
        }
    }
    public void PathfindTo(Vector2 target) //Makes a new path
    {
        pathTension = 0;
        movementTargets = WorldGrid.getPathTo(transform.position, target);
        if(movementTargets.Count>1)
        {
            if(Vector2.Distance(movementTargets[0], movementTargets[1])> Vector2.Distance(transform.position, movementTargets[1]))
            {
                movementTargets.RemoveAt(0);
            }
        }
    }
    public void UnionPathfindTo(Vector2 target) //Takes the old path and continues it 
    {

        if (pathTension == 0)
        {
            requireRefining();
        }
        if (movementTargets.Count > 0)
        {
            int pt = pathTension;
            movementTargets = movementTargets.Union(WorldGrid.getPathTo(movementTargets[movementTargets.Count - 1], target)).ToList();

            pathTension = pt + 1;
        }
        else
        {
            PathfindTo(target);
        }
    }
    public void requireRefining()
    {
        refiningOffset = movementTargets.Count;
        
    }
    public bool isPathing()
    {
        return movementTargets.Count > 0;
    }
    private void OnDrawGizmosSelected()
    {
        if (movementTargets.Count > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < movementTargets.Count - 1; i++)
            {
                Gizmos.DrawLine(movementTargets[i], movementTargets[i + 1]);
            }
        }
    }
}
