using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoyT.AStar;
using UnityEngine.Tilemaps;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using System.Linq;
using System.Runtime.ConstrainedExecution;

public class Pathfinder : MonoBehaviour
{
    //A* pathfinding attempts to find the "cheapest" path between each point, to do this we assign a grid of points with those costs first.

    [Header("Static Info (Mostly Locked At Creation)")]
    
    public MazeGridSpawner MyMazeGrid; // Will ovveride gridsize/offset to match this MazeGridSpawner's values.
    [HideInInspector]
    public Vector2Int GridSize=new Vector2Int(10,10);
    [HideInInspector]
    public Vector2Int GridOffset = Vector2Int.zero;
    [Min(1)]
    public int DefaultCellCost=9999; //Default tiles in the universe are set to this cost.
    [Min(1)]
    public int FreeCellCost = 1; //Empty tiles are set to this cost.
    [Min(0)]
    public int NeighboringWallCellCost=0; //If a tile is one tile away from a wall, how much more does this tile cost.
    [Min(1)]
    public int NeighboringWallMaxDist=1;
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
    [HideInInspector]
    public World grid;
   
    [Header("Extra Crud (Will Be Run During Baking)")]
    public Tilemap EmptySpace;
    public Tile[] PaintGround;

    private bool isPosInBounds(Vector3Int pos)
    {
        return grid.isPos(pos);
    }
    public void ResetGrid()
    {
        grid = new World(1,GridSize.x, GridSize.x, DefaultCellCost);
    }
    private void Start()
    {
        if (MyMazeGrid != null)
        {
            SnapToGrid(MyMazeGrid);
        }
        ResetGrid();

        if (CollisionMap != null)
        {
            if (OnStartUp == StartUpBakeAction.BakeNow)
                BakeMap();
            else if (OnStartUp== StartUpBakeAction.BakeInABit)
                StartCoroutine(BakeInASec());
        }
    }
    public Vector3Int toPos(Vector2 pos)
    {
        pos -= GridOffset;
        pos -= Vector2Int.FloorToInt(transform.position);
        return new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0);
    }
    public Vector2 toVector(Vector3Int pos)
    {
        Vector2 npos = new Vector2(pos.x,(pos.y));
        npos += new Vector2(GridOffset.x, GridOffset.y);
        npos += Vector2Int.FloorToInt(transform.position);
        return npos;
    }
    public bool isPosOk(Vector2 position)
    {
        return isPosInBounds(toPos(position)) && (grid.getTile(toPos(position)).pathCost != -1);
    }
    public IEnumerator BakeInASec()
    {
        yield return new WaitForSecondsRealtime(1);

        if (CollisionMap != null)
            BakeMap();
    }


    public void BakeMap(Tilemap tilemap)
    {
        //Only the last collision map is stored (Really should only be one as of now), but you can double bake if you want.
        CollisionMap = tilemap;
        BakeMap();

        CollisionMap.gameObject.isStatic = true;
        EmptySpace.gameObject.isStatic = true;
    }
   
    public void BakeMap()
    {
        //You can bake so long as a collision map exists, you can set it in inspector or provide as parameter.

        if(CollisionMap!=null)
        {
            //Loop through all positions in the world grid, and if there is a tile there, follow the selected choices to set cell cost accordingly, if nothing is there do the same but for the empty cell rules.
            Vector2Int startPos = Vector2Int.FloorToInt((Vector2)transform.position + (Vector2)GridOffset);
            for(int x= 0; x< GridSize.x-1;x++)
            {
                for (int y =1; y < GridSize.y; y++)
                {
                    Vector3Int pos = toPos(startPos+new Vector2(x, y));

                    if (EmptySpace != null)
                    {
                        EmptySpace.SetTile(new Vector3Int(startPos.x + x, startPos.y + y), PaintGround[Random.Range(0, PaintGround.Length)]);
                    }
                    if (CollisionMap.GetTile(new Vector3Int(startPos.x+x,startPos.y + y))!=null)
                    {
                        if (BakeRule == DoubleBakeRule.PaintFull || BakeRule == DoubleBakeRule.PaintWalls)
                        {
                            if (ObstacleCost == WallCost.InfiniteCellCost)
                            {
                                grid.BlockCell(pos);
                             //   alt_grid.SetCellCost(pos, DefaultCellCost);
                            }
                            else
                            {
                                grid.UnblockCell(pos); //If you double bake, you might have blocked cells that are now free
                              //  alt_grid.UnblockCell(pos);
                                if (ObstacleCost == WallCost.FreeCellCost)
                                {
                                    grid.SetCellCost(pos, FreeCellCost);
                                  //  alt_grid.SetCellCost(pos,FreeCellCost);
                                }
                                else if (ObstacleCost == WallCost.DefaultCellCost)
                                {
                                    grid.SetCellCost(pos, DefaultCellCost);
                                 //   alt_grid.SetCellCost(pos, DefaultCellCost);
                                }
                            }


                        }
                    }
                    else
                    {
                        if(BakeRule==DoubleBakeRule.PaintFull|| BakeRule == DoubleBakeRule.PaintEmpties)
                        {
                            if(EmptySpace!=null)
                            {
                                EmptySpace.SetTile(new Vector3Int(startPos.x + x, startPos.y + y), PaintGround[Random.Range(0, PaintGround.Length)]);
                            }
                            grid.UnblockCell(pos);
                          //  alt_grid.UnblockCell(pos);

                            int proximityCost = 0;
                            if(NeighboringWallCellCost!=0)
                            {
                                for(int dx=-NeighboringWallMaxDist; dx <= NeighboringWallMaxDist; dx++)
                                {
                                    for (int dy = -NeighboringWallMaxDist; dy <= NeighboringWallMaxDist; dy++)
                                    {
                                        if(CollisionMap.GetTile(new Vector3Int(startPos.x+ x +dx, startPos.y+ y +dy)) != null)
                                        {
                                            float dist = Vector2.Distance(Vector2.zero, new Vector2(dx, dy));
                                            if (dist == 0)
                                                dist = 1;
                                            proximityCost +=Mathf.RoundToInt(NeighboringWallCellCost/dist);
                                        }
                                    }
                                }
                            }
                            grid.SetCellCost(pos, FreeCellCost+ proximityCost);
                           // alt_grid.SetCellCost(pos, FreeCellCost + proximityCost);
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
    public void SnapToGrid(MazeGridSpawner gridSpawner)
    {
        GridSize.x = gridSpawner.width * gridSpawner.mazeCellSize+1;
        GridSize.y = gridSpawner.height * gridSpawner.mazeCellSize+1;
        GridOffset.x = 0;
        GridOffset.y = -GridSize.y+gridSpawner.mazeCellSize;

        if (GridSize.x % 2 == 1)
        {
            GridSize.x += 1;
        }
        if (GridSize.y % 2 == 1)
        {
            GridSize.y += 1;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(MyMazeGrid != null&&!Application.isPlaying)
        {
            SnapToGrid(MyMazeGrid);
        }
        if(GridSize.x%2==1)
        {
            GridSize.x += 1;
        }
        if (GridSize.y % 2 == 1)
        {
            GridSize.y += 1;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position+(Vector2)GridOffset + (Vector2)(new Vector2(GridSize.x -1, GridSize.y+1) / 2), new Vector2(GridSize.x-1,GridSize.y-1));
    }
}
