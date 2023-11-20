using UnityEngine;
using System.Collections;

public class MazeObjectSpawner : MonoBehaviour
{
    [SerializeField] MazeGridSpawner _mSpawner;

    [Header("Key Settings")]
    [SerializeField] GameObject keyPrefab;
    [Tooltip("Key radius should not be larger than 0.5 * min(maze width, maze height) (sorry to make you do math)")]
    [SerializeField] int keyRadius;
    [SerializeField] bool debugSpawnAtOrigin = false;

    [Header("Boss Settings")]
    [SerializeField] GameObject bossPrefab;
    [Tooltip("Boss radius should not be larger than 0.5 * min(maze width, maze height) (sorry to make you do math)")]
    [SerializeField] int bossRadius;

    private GameObject player;
    private Vector3Int playerpos;
    private Vector3Int keyFinalLocation;

    private void Awake()
    {
        player = GameObject.Find("Player");
        playerpos = Vector3Int.zero;
    }

    public void SpawnKey()
    {
        Vector3 keyWorldPosition = GetCellMidpointAsWorldPosition(GenerateKeyGridCoords());
        keyPrefab.transform.position = keyWorldPosition;
    }

    public void SpawnBoss()
    {
        Vector3 bossWorldPosition = GetCellMidpointAsWorldPosition(GenerateBossGridCoords());
        bossPrefab.transform.position = bossWorldPosition;
    }

    #region HelperFunctions
    /*
     * Randomly generate key location as grid coordinates 
     */
    private Vector3Int GenerateKeyGridCoords()
    {
        Vector3Int keyLocation = Vector3Int.zero;
        if (debugSpawnAtOrigin) return keyLocation;
        bool generated = false;
        int times = 0;

        while (!generated)
        {
            if (times > 1000)
            {
                Debug.Log("Couldn't generate key with radius " + keyRadius + ", try something lower.");
                return Vector3Int.zero;
            }
            // choose a random spot
            int x = Mathf.FloorToInt(Random.value * _mSpawner.width);
            int y = Mathf.FloorToInt(Random.value * _mSpawner.height);

            keyLocation = new Vector3Int(x, y);

            if (Vector3Int.Distance(Vector3Int.FloorToInt(playerpos), keyLocation) > keyRadius)
            {
                generated = true;
            }
            times++;
        }

        keyFinalLocation = keyLocation;
        return keyLocation;
    }

    private Vector3Int GenerateBossGridCoords()
    {
        Vector3Int bossLocation = Vector3Int.zero;

        bool generated = false;
        int times = 0;

        while (!generated)
        {
            if (times > 1000)
            {
                Debug.Log("Couldn't generate key with radius " + keyRadius + ", try something lower.");
                return Vector3Int.zero;
            }

            // choose a random spot
            int x = Mathf.FloorToInt(Random.value * _mSpawner.width);
            int y = Mathf.FloorToInt(Random.value * _mSpawner.height);

            bossLocation = new Vector3Int(x, y);

            if (Vector3Int.Distance(bossLocation, keyFinalLocation) > bossRadius)
            {
                generated = true;
            }
            times++;
        }

        return bossLocation;
    }

    /*
     * Returns true if the given vector3 int maze coordinate is within the bounds of the maze.
     * Returns false otherwise.
     */
    private bool InBounds(Vector3Int gridCoords)
    {
        return false;
    }

    private Vector3 GetCellMidpointAsWorldPosition(Vector3Int gridCoords)
    {
        Vector3Int mazeCoords = MazeToGridCoords(gridCoords);
        float x = mazeCoords.x + (0.5f * _mSpawner.mazeCellSize);
        float y = -(mazeCoords.y + (0.5f * _mSpawner.mazeCellSize)) + _mSpawner.mazeCellSize;

        return new Vector3(x, y);
    }

    private Vector3Int MazeToGridCoords(Vector3Int coords)
    {
        return coords * _mSpawner.mazeCellSize + Vector3Int.FloorToInt(_mSpawner.origin.transform.position);
    }

    #endregion
}
