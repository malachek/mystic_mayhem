using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoyT.AStar;
using UnityEngine.Tilemaps;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using System.Linq;

public class Pathfinder : MonoBehaviour
{
    //A* pathfinding attempts to find the "cheapest" path between each point, to do this we assign a grid of points with those costs first.

    [Header("Static Info (Mostly Locked At Creation)")]
    public Vector2Int GridSize=new Vector2Int(10,10);
    public Vector2Int GridOffset = Vector2Int.zero;
    [Min(1)]
    public int DefaultCellCost=9999; //Default tiles in the universe are set to this cost.
    [Min(1)]
    public int FreeCellCost = 1; //Empty tiles are set to this cost.
    [Min(0)]
    public int NeighboringWallCellCost=0; //If a tile is one tile away from a wall, how much more does this tile cost.
    public enum WallCost {FreeCellCost,DefaultCellCost,InfiniteCellCost}; //Should obstacles be ignored, avoided but passed over if need be, or never crossed ever.
    public WallCost ObstacleCost = WallCost.InfiniteCellCost;

    [Header("Dynamic Info (Can Change Before Baking)")]
    public Tilemap CollisionMap;
    public bool isInitialized = false;
    public enum DoubleBakeRule { PaintEmpties, PaintWalls, PaintFull }; //When double baking, should only empty squares be painted, only obstacles, or everything.
    public enum StartUpBakeAction {DoNothing,BakeNow,BakeInABit};// Actions to do when Pathfinder.Start() is called.
    public StartUpBakeAction OnStartUp; 
    public DoubleBakeRule BakeRule = DoubleBakeRule.PaintFull;
    public Vector2 SubTileOffset = new Vector2(0.5f, 0.5f); //All pathfinding paths point to the bottom left most part of a tile by default, this shifts it to the middle.
    private RoyT.AStar.Grid grid;

    private RoyT.AStar.Position toPos(Vector2 pos)
    {
        return toPos(Vector2Int.FloorToInt(pos));
    }
    private RoyT.AStar.Position toPos(Vector2Int pos)
    {
        pos -= GridOffset;
        pos -= Vector2Int.FloorToInt(transform.position);
        return new RoyT.AStar.Position(pos.x, pos.y);
    }
    private Vector2Int toVector(RoyT.AStar.Position pos)
    {
        Vector2Int npos = new Vector2Int(pos.X, pos.Y);
        npos += GridOffset;
        npos += Vector2Int.FloorToInt(transform.position);
        return npos;
    }
    private bool isPosInBounds(RoyT.AStar.Position pos)
    {
        return ((pos.X >= 0 && pos.Y >= 0) && (pos.X < GridSize.x&& pos.Y < GridSize.y));
    }
    public void ResetGrid()
    {
        grid = new RoyT.AStar.Grid(GridSize.x, GridSize.y,DefaultCellCost);
    }
    private void Start()
    {
        ResetGrid();

        if (CollisionMap != null)
        {
            if (OnStartUp == StartUpBakeAction.BakeNow)
                BakeMap();
            else if (OnStartUp== StartUpBakeAction.BakeInABit)
                StartCoroutine(BakeInASec());
        }
    }
    public IEnumerator BakeInASec()
    {
        yield return new WaitForSecondsRealtime(1);

        if (CollisionMap != null)
            BakeMap();
    }
    public List<Vector2> getPathTo(Vector2 start, Vector2 location)
    {
        return getPathTo(Vector2Int.FloorToInt(start), Vector2Int.FloorToInt(location));
    }
    public List<Vector2> getPathTo(Vector2Int start,Vector2Int location)
    {
        //Returns a list of locations as a path to location if it can be made, empty if no such path exists.
        if(isPosInBounds(toPos(start)) && isPosInBounds(toPos(location)))
        {
            if(grid.GetCellCost(toPos(location))==float.PositiveInfinity)
            {
                Debug.LogWarning("Cannot create paths that end inside of a wall!");
                return new List<Vector2>();
            }
            else if (grid.GetCellCost(toPos(start)) == float.PositiveInfinity)
            {
                Debug.LogWarning("Cannot create paths that start inside of a wall!");
                return new List<Vector2>();
            }
            RoyT.AStar.Position[] royPath= grid.GetPath(toPos(start), toPos(location));

            if (royPath.Length <= 0)
            {
                if (Vector2.Distance(start, location) > 1)
                    Debug.LogWarning("Lagilly tried to create a path to an inaccessable location, perhaps the maze is bad?");
                return new List<Vector2>();
            }

            List<Vector2> path = new List<Vector2>();
            foreach (RoyT.AStar.Position p in royPath)
            {
                path.Add(toVector(p)+ SubTileOffset);
            }
           return path;
        }
        else
        {
            Debug.LogWarning("Cannot create paths that go beyond the given pathfinding scope!");
            return new List<Vector2>();
        }
    }
    public void BakeMap(Tilemap tilemap)
    {
        //Only the last collision map is stored (Really should only be one as of now), but you can double bake if you want.
        CollisionMap = tilemap;
        BakeMap();
    }
   
    public void BakeMap()
    {
        //You can bake so long as a collision map exists, you can set it in inspector or provide as parameter.

        if(CollisionMap!=null)
        {
            //Loop through all positions in the world grid, and if there is a tile there, follow the selected choices to set cell cost accordingly, if nothing is there do the same but for the empty cell rules.
            Vector2Int startPos = Vector2Int.FloorToInt((Vector2)transform.position + (Vector2)GridOffset);
            for(int x= 0; x< GridSize.x;x++)
            {
                for (int y =0; y < GridSize.y; y++)
                {
                    RoyT.AStar.Position pos = toPos(startPos+new Vector2(x, y));
                    if (CollisionMap.GetTile(new Vector3Int(startPos.x+x,startPos.y + y))!=null)
                    {
                        if (BakeRule == DoubleBakeRule.PaintFull || BakeRule == DoubleBakeRule.PaintWalls)
                        {
                            if (ObstacleCost == WallCost.InfiniteCellCost)
                                grid.BlockCell(pos);
                            else
                            {
                                grid.UnblockCell(pos); //If you double bake, you might have blocked cells that are now free
                                if (ObstacleCost == WallCost.FreeCellCost)
                                    grid.SetCellCost(pos, FreeCellCost);
                                else if (ObstacleCost == WallCost.DefaultCellCost)
                                    grid.SetCellCost(pos, DefaultCellCost);
                            }


                        }
                    }
                    else
                    {
                        if(BakeRule==DoubleBakeRule.PaintFull|| BakeRule == DoubleBakeRule.PaintEmpties)
                        {
                            grid.UnblockCell(pos);

                            int proximityCost = 0;
                            if(NeighboringWallCellCost!=0)
                            {
                                for(int dx=-1; dx <= 1; dx++)
                                {
                                    for (int dy = -1; dy <= 1; dy++)
                                    {
                                        if(CollisionMap.GetTile(new Vector3Int(startPos.x+ x +dx, startPos.y+ y +dy)) != null)
                                        {
                                            proximityCost+=NeighboringWallCellCost;
                                        }
                                    }
                                }
                            }
                            grid.SetCellCost(pos, FreeCellCost+ proximityCost);
                        }
                    }
                }
            }
            isInitialized = true;
        }
        else
        {
            Debug.LogWarning("Cannot bake pathfinding without a tilemap!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(GridSize.x%2==1)
        {
            GridSize.x += 1;
        }
        if (GridSize.y % 2 == 1)
        {
            GridSize.y += 1;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position+(Vector2)GridOffset + (Vector2)(GridSize / 2), (Vector2)GridSize);
    }
}
