/**
 * MazeGenerator.cs
 * Gavin Gee
 * 
 * Randomly generates a maze utilizing Eller's Algorithm.
 * 
 * Based on @ShenShaw on Youtube's great series on generating mazes with
 * Eller's Algorithm (https://www.youtube.com/watch?v=5nWUX2TMJrY).
 * The algorithm is adapted here to be a bit more readable and work with 
 * 2d tilemaps.
 * 
 * TODO LIST
 * - TODO: implement step 6
 * 
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Grid grid;                 // the actual grid that tiles exist on
    Tilemap tilemap;			                // the tilemap that tiles exist on
    [SerializeField] TileBase wallTile;         // the tile used to generate walls
    [SerializeField] bool generate = false;     // Used for testing, determines whether the map should be generated.

    [SerializeField] MazeGridSpawner _mSpawner; // the spawner object
    [SerializeField] private MazeGrid _mGrid;   // the MazeGrid object // TODO: phase this out ... not necessary for functionality, just useful for testing.
    private int _maxSetValue = 0;               // the current max set value


    // Use this for initialization
    void Start()
    {
        _mGrid = _mSpawner.grid;
        tilemap = grid.GetComponentInChildren<Tilemap>();

        if (generate) GenerateMaze();
    }


    /** 
     * Generates the maze via the steps of Eller's Algorithm
     * 
     */
    public void GenerateMaze()
    {
        // Step 1 -- create the first row of empty cells ( this also includes step 2 for the first run... )
        List<MazeCell> firstRow = new List<MazeCell>();

        for (int x = 0; x < _mSpawner.width; x++)
        {
            Vector3Int coords = new Vector3Int(x, 0);
            firstRow.Add(new MazeCell(coords, _maxSetValue));
            _maxSetValue++;

            
            DebugDrawMiddleBlock(firstRow[x].GetCoords()); // draw middle block
        }

        List<MazeCell> current = firstRow;
        // generation loop
        for (int i = 0; i < _mSpawner.height; i++)// _mSpawner.height; i++)
        {
            // step 3 -- create right walls & merge sets
            List<MazeCell> currentJoined = Step3(current);
            WorldTestRow(currentJoined, Color.red);

            // step 4 -- generate bottom walls
            Step4(current);

            // step 5 (+ 2) -- create the next row & clear walls
            current = Step5(current);
        }

        // step 6 -- create boundary walls & bottom walls // TODO: implement me ?
    }

    #region Steps

    /*
     * TODO:
     * - document me
     * - test me
     */
    private List<MazeCell> Step3(List<MazeCell> row)
    {
        List<MazeCell> target = new List<MazeCell>(row);

        for (int i = 0; i < _mSpawner.width-1; i++)
        {
            MazeCell current = target[i];
            MazeCell rightAdjacent = target[i + 1];

            if (current.GetSetValue() == rightAdjacent.GetSetValue())
            {
                // create a wall
                Vector3Int gridCoords = MazeToGridCoords(current.GetCoords());
                PlaceRightWall(gridCoords.x, gridCoords.y);
            }
            else
            {
                // 50/50 create a wall
                if (Random.value > 0.5f)
                {
                    // create a wall
                    Vector3Int gridCoords = MazeToGridCoords(current.GetCoords());
                    PlaceRightWall(gridCoords.x, gridCoords.y);
                }
                else
                {
                    // union c & r.a to the same set
                    rightAdjacent.SetSetVal(current.GetSetValue());
                }
            }
        }

        return target;
    }

    /**
     * TODO:
     * - test me
     * - document me
     */
    private void Step4(List<MazeCell> row)
    {
        List<MazeCell> target = new List<MazeCell>(row);
        List<List<MazeCell>> sets = SeparateIntoSets(target);
        List<List<MazeCell>> shuffled = ShuffleSets(sets);

        foreach (List<MazeCell> set in shuffled) // for each set
        {
            for (int i = 0; i < set.Count; i++)  // iterate through its elements.
            {
                if (i == 0) continue; // if its the first element skip it

                // otherwise, 50/50 chance whether to add a bottom wall or not.
                if (Random.value > 0.5f)
                {
                    set[i].SetBottomWallStatus(true);
                }
            }
        }

        // now actually generate the walls
        foreach (MazeCell current in row)
        {
            if (current.GetBottomWallStatus())
            {
                Vector3Int gridCoords = MazeToGridCoords(current.GetCoords());
                Debug.Log("Placing bottom wall @ " + current.GetCoords());
                PlaceBottomWall(gridCoords.x, gridCoords.y);
            }
        }
    }

    /*
     * TODO:
     * - document me
     * - test me
     */
    private List<MazeCell> Step5(List<MazeCell> row)
    {
        List<MazeCell> next = new List<MazeCell>(row); // no need to remove right walls.. they technically aren't ever stored anywhere

        foreach (MazeCell cell in next)
        {
            if (cell.GetBottomWallStatus())
            {
                // remove the bottom wall
                cell.SetBottomWallStatus(false);

                // remove the cell from its set (give it a new distinct set..)
                cell.SetSetVal(_maxSetValue);
                _maxSetValue++;
            }
            cell.SetCoords(new Vector3Int(cell.GetCoords().x, cell.GetCoords().y - 1)); // increment the axis by 1
        }
        
        return next;   
    }



    #endregion

    #region Helper_Functions

    /*
     * From https://www.youtube.com/watch?v=5nWUX2TMJrY.
     * 
     * Displays the set value of the current cell in the cell as
     * a text mesh object.
     */
    private void WorldTestRow(List<MazeCell> row, Color textColor)
    {
        GameObject rowObj = new GameObject();
        foreach (MazeCell cell in row)
        {
            Vector3Int coords = cell.GetCoords();
            int setValue = cell.GetSetValue();

            GameObject text = new GameObject();
            TextMesh tm = text.AddComponent<TextMesh>();
            tm.text = setValue.ToString();
            tm.color = textColor;

            // center text
            Vector3Int worldpos = _mGrid.GetWorldPosition(coords.x, coords.y);
            worldpos = new Vector3Int(worldpos.x + _mGrid.GetCellSize() / 2, worldpos.y + _mGrid.GetCellSize() / 2);

            text.transform.position = worldpos;
            text.transform.parent = rowObj.transform;
        }
    }


    private Vector3Int MazeToGridCoords(Vector3Int coords)
    {
        return coords * _mSpawner.mazeCellSize + Vector3Int.FloorToInt(_mSpawner.origin.transform.position);
    }

    /*
     * Places a right wall at the cell at grid coordinates x, y. 
     * TODO: make this take maze coords? looks a little more readable.
     */
    private void PlaceRightWall(int x, int y)
    {
        for (int i = 0; i < _mSpawner.mazeCellSize; i++) // if there's overlap, try mazeCellSize-1
        {
            Vector3Int pos = new Vector3Int(x + _mSpawner.mazeCellSize, y+i);
            tilemap.SetTile(pos, wallTile);
        }
    }

    /*
     * Places a bottom wall at the cell at grid coordinates x, y. 
     * TODO: make this take maze coords? looks a little more readable.
     */
    private void PlaceBottomWall(int x, int y)
    {
        for (int i = 0; i < _mSpawner.mazeCellSize + 1; i++)
        {
            Vector3Int pos = new Vector3Int(x + i, y);
            tilemap.SetTile(pos, wallTile);
        }
    }

    /*
     * Places a single tile at the center of the cell at the given coords.
     */ 
    private void DebugDrawMiddleBlock(Vector3Int coords)
    {
        Vector3Int gridCoords = MazeToGridCoords(coords);
        tilemap.SetTile(new Vector3Int(gridCoords.x + (_mSpawner.mazeCellSize / 2), gridCoords.y + (_mSpawner.mazeCellSize / 2)), wallTile);
    }

    /*
     * Separates a list of MazeCells into a list of lists of MazeCells, where
     * each List contains only Cells of one set.
     */
    private List<List<MazeCell>> SeparateIntoSets(List<MazeCell> row)
    {
        List<List<MazeCell>> sets = new List<List<MazeCell>>();
        List<MazeCell> currentSet = new List<MazeCell>();

        sets.Add(currentSet);
        currentSet.Add(row[0]);
        int lastSetID = currentSet[0].GetSetValue();

        for (int i = 1; i < row.Count; i++)
        {
            if (row[i].GetSetValue() != lastSetID)
            {
                // create a new set and add this element to it
                currentSet = new List<MazeCell>();
                sets.Add(currentSet);
                currentSet.Add(row[i]);
                lastSetID = currentSet[0].GetSetValue();
            }
            else
            {
                // add this element to the current set
                currentSet.Add(row[i]);
            }
        }

        return sets;
    }

    /*
     * this uses a little bit of Linq magic that I don't totally understand...
     * TODO: look into System.Linq!
     */

    private List<List<MazeCell>> ShuffleSets(List<List<MazeCell>> sets)
    {
        List<List<MazeCell>> shuffled = new List<List<MazeCell>>();
        foreach (List<MazeCell> set in sets)
        {
            shuffled.Add(set.OrderBy(a => Random.Range(0, 101)).ToList()); // maybe replace w/ rng.Next()...
        }
        return shuffled;
    }

    // here in case the above function doesn't work w/ Random.Range...
    // private static System.Random rng = new System.Random();

    #endregion
}
