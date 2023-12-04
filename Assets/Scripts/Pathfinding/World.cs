using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using UnityEditor.Tilemaps;
using UnityEngine;
// using UnityEngine.WSA;

[System.Serializable]
public class World
{
    public Vector2Int WorldSize;
    public int WorldHeight;
    public Tile[][][] Grid;
    public Dictionary<Vector3Int, Vector2Int> CostTemplate;
    private int m_defaultCost;
    public static int MAX_PATHCLASSES = 25;
    public World(int height,int size_x,int size_y,int default_cost)
    {
        CostTemplate = new Dictionary<Vector3Int, Vector2Int>();
   
        WorldSize = new Vector2Int(size_x,size_y);
        WorldHeight = height;
        m_defaultCost = default_cost;
        Grid = new Tile[WorldHeight][][];
        for (int z = 0; z< Grid.Length; z++)
        {
            Grid[z]= new Tile[WorldSize.y][];
            for (int x = 0; x < Grid[z].Length; x++)
            {
                Grid[z][x]= new Tile[WorldSize.x];


                for (int y = 0; y < Grid[z][x].Length; y++)
                {
                    Grid[z][x][y] = new Tile(new Vector3Int(x,y,z),default_cost);

                    for (int k = 0; k < MAX_PATHCLASSES; k++)
                    { 
                        CostTemplate[Grid[z][x][y].location+new Vector3Int(0,0,k)] = Vector2Int.zero;
                    }
                }
            }
        }
    }
    public Tile getTile(Vector3Int pos)
    {
        if(!isPos(pos))
        {
            Debug.LogError("Position "+pos+" is out of bounds!");
        }
        return Grid[pos.z][pos.x][pos.y]; //z is the vertical slice, x and y are the grid coordinates.
    }
    public void BlockCell(Vector3Int pos)
    {
        getTile(pos).pathCost = -1;
    }
    public void UnblockCell(Vector3Int pos)
    {

        getTile(pos).pathCost =m_defaultCost;
    }
    
    public void SetCellCost(Vector3Int pos,int cost)
    {

        getTile(pos).pathCost = cost;
    }
    
    
    public int getCostAt(Vector3Int pos)
    {
        return getTile(pos).pathCost;
    }
    public bool isPos(Vector3Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.z >= 0 && pos.x < WorldSize.x && pos.y < WorldSize.y && pos.z < WorldHeight;
    }
    [System.Serializable]
    public class Tile
    {
        public Tile(Vector3Int location, int cost)
        {
            pathCost = cost;
            this.location = location;
        }
        public Tile(Vector3Int location) //Empty Tile = 0
        {
            pathCost = -1;
            this.location = location;
        }
        public Vector3Int location;
        public int pathCost=1;

        
        //Pathfinding Data:
        public Vector3Int pathPointer;


        public void UpdateCosts(Tile neighbor,Tile target,bool forced,Dictionary<Vector3Int,Vector2Int> costs,int pathfindSubId)
        {
            int g_distance = pathCost+ costs[neighbor.location + new Vector3Int(0, 0, pathfindSubId)].x+ Mathf.RoundToInt(Vector3.Distance(location, neighbor.location)*10);
            int h_distance = Mathf.RoundToInt(Mathf.Sqrt(5*Mathf.Pow((location.x-target.location.x),2) +Mathf.Pow((location.y - target.location.y), 2)) * 50);
            if (forced||(HCost(costs, pathfindSubId) >(g_distance+h_distance)))
            {
                pathPointer = neighbor.location;
                costs[location+ new Vector3Int(0,0, pathfindSubId)]=new Vector2Int(g_distance,h_distance);
            }
        }

        public int HCost(Dictionary<Vector3Int, Vector2Int> costs,int pathfindSubId)
        {
            return costs[location + new Vector3Int(0, 0, pathfindSubId)].x + costs[location + new Vector3Int(0, 0, pathfindSubId)].y;
        }
    }
}
