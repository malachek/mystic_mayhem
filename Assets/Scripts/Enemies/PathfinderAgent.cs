using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class PathfinderAgent : MonoBehaviour
{
    public Pathfinder WorldGrid;
    [HideInInspector]
    public List<Vector2> movementTargets = new List<Vector2>();
    public float MovementSpeed = 0.5f;
    public bool ListensForMouseDebug;
    private int refiningOffset = -1; //When refiningOffset = 0, movementTargets is recalculated to be the true fastest path.
    private bool wantsToRefine = false; //Wants to refine the path as soon as allowed.
    [HideInInspector]
    public int pathTension; //More useful for Enemy scripts, but this holds the amount of times a path has been Unioned with the old one.
    [Min(0)]
    public float minMovementTargDist=0.1f;
    private void Update()
    {
        if (ListensForMouseDebug)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                PathfindTo(mousPos);
            }
        }
    }
    private const int ALLOW_REFINECUTOFF = 40;
    void FixedUpdate()
    {


        if (movementTargets.Count > 0)
        {
            //Debug.DrawLine(transform.position, movementTargets[movementTargets.Count - 1], Color.blue, 0.1f);
            if (wantsToRefine && Vector2.Distance(transform.position, movementTargets[movementTargets.Count - 1]) < ALLOW_REFINECUTOFF)
            {
                PathfindTo(movementTargets[movementTargets.Count - 1]);
            }

            if (movementTargets.Count > 1 && CanSmallRefine(transform.position, movementTargets[movementTargets.Count - 1]))
            {
                Vector2 endLoc = movementTargets[movementTargets.Count - 1];
                movementTargets = new List<Vector2>();
                movementTargets.Add(endLoc);
            }
            if (Vector2.Distance(transform.position, movementTargets[0]) < minMovementTargDist)
            {
                movementTargets.RemoveAt(0);
                while (movementTargets.Count>3 && CanSmallRefine(movementTargets[0], movementTargets[3]))
                {
                    movementTargets.RemoveAt(1);
                }

                if(refiningOffset >= 0)
                {
                    refiningOffset--;

                    if (refiningOffset == 0 && movementTargets.Count > 1)
                        wantsToRefine = true;
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
    
    public bool CanSmallRefine(Vector2 target,Vector2 from)
    {
        RaycastHit2D hit = Physics2D.Raycast(from, (target - from).normalized, Vector2.Distance(from, target), LayerMask.GetMask("Wall"));
        //Debug.DrawRay(from, (target -from).normalized * (Vector2.Distance(from, target)-1), Color.blue, 10);
        if (hit.collider == null)
        {
            return true;
        }
        else
            return false;
    }
    public void PathfindTo(Vector2 target) //Makes a new path
    {
        refiningOffset = -1;
        wantsToRefine = false;
        pathTension = 0;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, (target-new Vector2(transform.position.x,transform.position.y)).normalized, Vector2.Distance(transform.position, target), LayerMask.GetMask("Wall"));
        //Debug.DrawRay(transform.position, (target-new Vector2(transform.position.x, transform.position.y)).normalized* Vector2.Distance(transform.position, target), Color.yellow,3);
        if (hit.collider == null)
        {
            movementTargets=new List<Vector2>();
            movementTargets.Add(transform.position);
            movementTargets.Add(target);
            return;
        }
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
            RaycastHit2D hit = Physics2D.Raycast(movementTargets[movementTargets.Count - 1], (target-movementTargets[movementTargets.Count - 1]).normalized, Vector2.Distance(movementTargets[movementTargets.Count - 1], target), LayerMask.GetMask("Wall"));
            //Debug.DrawRay(movementTargets[movementTargets.Count - 1], (target-movementTargets[movementTargets.Count - 1]).normalized* Vector2.Distance(movementTargets[movementTargets.Count - 1], target), Color.red,3);

            if (hit.collider == null)
            {
                movementTargets.Add(target);
                pathTension++;
                return;
            }

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
        //Gizmos.DrawWireSphere(transform.position, ALLOW_REFINECUTOFF);
        if (movementTargets.Count > 0)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(transform.position, movementTargets[0]);
            for (int i = 0; i < movementTargets.Count - 1; i++)
            {
                Gizmos.DrawLine(movementTargets[i], movementTargets[i + 1]);
            }
        }
    }
}
