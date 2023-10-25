using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderAgent : MonoBehaviour
{
    public Pathfinder WorldGrid;
    [HideInInspector]
    public List<Vector2> movementTargets = new List<Vector2>();
    public float MovementSpeed = 0.5f;


    void Update()
    {
        //DEMO CODE:
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PathfindTo(mousPos);
        }
        //DEMO CODE


        if (movementTargets.Count > 0)
        {
            if (Vector2.Distance(transform.position, movementTargets[0]) < 0.1f)
            {
                movementTargets.RemoveAt(0);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, movementTargets[0], MovementSpeed);
            }
        }
    }
    public void PathfindTo(Vector2 target)
    {
        movementTargets = WorldGrid.getPathTo(transform.position, target);
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
