using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class PathfinderAgent : MonoBehaviour
{
    public Pathfinder WorldGrid;
    public List<Vector2> movementTargets = new List<Vector2>();
    [HideInInspector]
    public GameObject WhenIdleGoTo;
    public float MovementSpeed = 0.5f;
    public bool ListensForMouseDebug;
    private int refiningOffset = -1; //When refiningOffset = 0, movementTargets is recalculated to be the true fastest path.
    private bool wantsToRefine = false; //Wants to refine the path as soon as allowed.
    [HideInInspector]
    public int pathTension; //More useful for Enemy scripts, but this holds the amount of times a path has been Unioned with the old one.
    [Min(0)]
    public float minMovementTargDist=0.1f;

    public bool KillWhenNoPath;
    public bool isBoss;
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
            
            if (movementTargets.Count > 0 && CanSmallRefine(transform.position, movementTargets[movementTargets.Count - 1]) && Vector2.Distance(transform.position, movementTargets[movementTargets.Count - 1]) < ALLOW_REFINECUTOFF)
            {
                Vector2 endLoc = movementTargets[movementTargets.Count - 1];
                movementTargets = new List<Vector2>() { endLoc };
              
            }

            if (movementTargets.Count > 0)
            {
                if (movementTargets.Count > 3 && CanSmallRefine(movementTargets[0], movementTargets[3]) && Vector2.Distance(transform.position, movementTargets[movementTargets.Count - 1]) < ALLOW_REFINECUTOFF)
                {
                    movementTargets.RemoveAt(1);
                }

                if (Vector2.Distance(transform.position, movementTargets[0]) < minMovementTargDist)
                {
                    movementTargets.RemoveAt(0);

                    if (refiningOffset >= 0)
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
        }
        else
        {
            if(WhenIdleGoTo!=null&&(!isCalculatingPath||Vector2.Distance(transform.position,WhenIdleGoTo.transform.position)<20))
            {
                transform.position = Vector2.MoveTowards(transform.position, WhenIdleGoTo.transform.position, MovementSpeed);
                
            }
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
            movementTargets=new List<Vector2>() {transform.position ,target};
            return;
        }
        else
        {
            movementTargets = new List<Vector2>();
            RequestPathTo(transform.position, target, movementTargets);
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
            RequestPathTo(movementTargets[movementTargets.Count - 1], target, movementTargets);

            pathTension++;
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

    
    public bool isCalculatingPath;
    private int WaitForOffset = 1000;
    private int pathfindSubId;

    private void Start()
    {

        if (isBoss)
        {
            pathfindSubId = 0;
        }
        else
        {
            pathfindSubId = 1;
        }
    }
    public void RequestPathTo(Vector3 from, Vector3 to,List<Vector2> n_path)
    {
        RaycastHit2D hit = Physics2D.Raycast(from, (to - from).normalized, 2, LayerMask.GetMask("Wall"));

        if (!isCalculatingPath&&(Vector2.Distance(from,to)<50||hit.collider!=null))
        {
            AbortPathRequest();
            isCalculatingPath = true;
            StartCoroutine(CalcPath(WorldGrid.toPos(from), WorldGrid.toPos(to),n_path));
        }
    }
    public void AbortPathRequest()
    {
        if (isCalculatingPath)
        {
            isCalculatingPath = false;
            StopAllCoroutines();
        }
    }
    private void OnDestroy()
    {
        if(isCalculatingPath)
        {
            CalculatingPaths--;
        }
    }
    private Vector2 secretPathTarg;
    public static int CalculatingPaths;
    public IEnumerator CalcPath(Vector3Int from, Vector3Int to,List<Vector2> n_path)
    {
        while(CalculatingPaths>=World.MAX_PATHCLASSES-2 && !isBoss)
        {
            yield return new WaitForFixedUpdate();
        }

        if (isBoss)
        {
            if(Vector2.Distance(WorldGrid.toVector(from), WorldGrid.toVector(to)) <30)
            {

                WaitForOffset = 5000;
            }
            else
                WaitForOffset = 2000;
        }

        int offsetWaitForOffset = 1;
        if (!isBoss)
        {
            pathfindSubId = CalculatingPaths + 1;
            offsetWaitForOffset =Mathf.Max(1,pathfindSubId);
        }
        //Clone Template Dictionary:
        isCalculatingPath = true;
        CalculatingPaths++;
        // Debug.Log("Working..");

        if (from == to)
        {
            Debug.Log("No need to pathfind!");
            isCalculatingPath = false;
            CalculatingPaths--;
            yield break; // No need to pathfind, we are already there!
        }


        if (!WorldGrid.grid.isPos(to) || !WorldGrid.grid.isPos(from))
        {
            Debug.LogError("Cannot pathfind outside of the given world scope!");
            isCalculatingPath = false;
            CalculatingPaths--;
            yield break;
        }
        secretPathTarg = WorldGrid.toVector(to);
        List<World.Tile> live = new List<World.Tile>();
        List<World.Tile> cleared = new List<World.Tile>();

        World.Tile start = WorldGrid.grid.getTile(from);
        World.Tile tile = start;
        World.Tile target = WorldGrid.grid.getTile(to);


        if (start.pathCost == -1 || target.pathCost == -1)
        {
            //   Debug.LogError("Cannot pathfind to or from invalid locations!");
            isCalculatingPath = false;
            CalculatingPaths--;
            yield break;
        }

        if(CanSmallRefine(WorldGrid.toVector(from), secretPathTarg))
        {
            n_path.Add(secretPathTarg);
            movementTargets = n_path;
            isCalculatingPath = false;
            CalculatingPaths--;
            //  Debug.Log("Beelined!");
            yield break;
        }

        int waitForOffset = 1;

        live.Add(tile);

        while (tile.location != to)
        {

            World.Tile cheapest = live[0];
            foreach (World.Tile option in live)
            {
                if (cheapest.HCost(WorldGrid.grid.CostTemplate, pathfindSubId) > option.HCost(WorldGrid.grid.CostTemplate,pathfindSubId))
                {
                    cheapest = option;
                }


                if (waitForOffset % (WaitForOffset/ offsetWaitForOffset) == 0)
                    yield return new WaitForEndOfFrame();
                waitForOffset++;
            }
            tile = cheapest;


            FetchNeighbors(tile, target, live, cleared,WorldGrid.grid.CostTemplate);
            live.Remove(tile);
            cleared.Add(tile);
            //Debug.DrawLine(WorldGrid.toVector(from), WorldGrid.toVector(tile.location), Color.green,0.2f);

            if (waitForOffset % (WaitForOffset/ offsetWaitForOffset) == 0)
                yield return new WaitForEndOfFrame();
            waitForOffset++;


            if (!isBoss&& waitForOffset> WaitForOffset*10 && KillWhenNoPath && GetComponent<Enemy>() != null)
            {
               // Debug.Log("Despawning...");
                isCalculatingPath = false;
                CalculatingPaths--;
                GetComponent<Enemy>().Despawn();
                yield break;
            }
            //   yield return new WaitForSeconds(0.1f);

            if (live.Count == 0)
            {
              //  Debug.Log("No path possible!");
                isCalculatingPath = false;
                CalculatingPaths--;
                yield break;
            }
        }
        //   Debug.Log("Backtracking...");
        tile = target;
        while (tile != start)
        {
            n_path.Add(WorldGrid.toVector(new Vector3Int(tile.location.x, tile.location.y,tile.location.z)));
            tile = WorldGrid.grid.getTile(tile.pathPointer);
            if (waitForOffset % WaitForOffset == 0)
                yield return new WaitForEndOfFrame();
            waitForOffset++;
            //   yield return new WaitForSeconds(0.1f);
        }

        n_path.Reverse();

        movementTargets = n_path;
        CalculatingPaths--;
        isCalculatingPath = false;
        //  Debug.Log("Made it!");

    }
    public void FetchNeighbors(World.Tile tile, World.Tile target, List<World.Tile> live, List<World.Tile> cleared, Dictionary<Vector3Int, Vector2Int> costMap)
    {
        Vector3Int loc = tile.location;


        loc = tile.location + new Vector3Int(1, 0, 0);
        if (WorldGrid.grid.isPos(loc))
        {
            AddNeighbor(tile, WorldGrid.grid.getTile(loc), target, live, cleared, costMap);
        }
        loc = tile.location + new Vector3Int(-1, 0, 0);
        if (WorldGrid.grid.isPos(loc))
        {
            AddNeighbor(tile, WorldGrid.grid.getTile(loc), target, live, cleared, costMap);
        }
        loc = tile.location + new Vector3Int(0, 1, 0);
        if (WorldGrid.grid.isPos(loc))
        {
            AddNeighbor(tile, WorldGrid.grid.getTile(loc), target, live, cleared, costMap);
        }
        loc = tile.location + new Vector3Int(0, -1, 0);
        if (WorldGrid.grid.isPos(loc))
        {
            AddNeighbor(tile, WorldGrid.grid.getTile(loc), target, live, cleared, costMap);
        }

        
        loc = tile.location + new Vector3Int(1, 1, 0);
        if (WorldGrid.grid.isPos(loc))
        {
            AddNeighbor(tile, WorldGrid.grid.getTile(loc), target, live, cleared, costMap);
        }
        loc = tile.location + new Vector3Int(1, -1, 0);
        if (WorldGrid.grid.isPos(loc))
        {
            AddNeighbor(tile, WorldGrid.grid.getTile(loc), target, live, cleared, costMap);
        }
        loc = tile.location + new Vector3Int(-1, 1, 0);
        if (WorldGrid.grid.isPos(loc))
        {
            AddNeighbor(tile, WorldGrid.grid.getTile(loc), target, live, cleared, costMap);
        }
        loc = tile.location + new Vector3Int(-1, -1, 0);
        if (WorldGrid.grid.isPos(loc))
        {
            AddNeighbor(tile, WorldGrid.grid.getTile(loc), target, live, cleared, costMap);
        }
        

    }
    public void AddNeighbor(World.Tile tile, World.Tile tile_new, World.Tile target, List<World.Tile> live, List<World.Tile> cleared, Dictionary<Vector3Int, Vector2Int> costMap)
    {
        if (tile_new.pathCost < 0)
        {
            return; //This tile is a wall.
        }
        bool alreadyHas = live.Contains(tile_new) || cleared.Contains(tile_new);
        if (!alreadyHas)
        {
            live.Add(tile_new);
        }

        tile_new.UpdateCosts(tile, target, !alreadyHas, costMap, pathfindSubId);
    }

    private void OnDrawGizmos()
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
