using UnityEngine;
using System.Collections;

public class MazeObjectSpawner : MonoBehaviour
{
    [SerializeField] MazeGridSpawner _mSpawner;

    [Header("Key Settings")]
    [SerializeField] GameObject keyPrefab;
    [Tooltip("Key radius should not be larger than 0.5 * min(maze width, maze height) (sorry to make you do math)")]
    [SerializeField] int keyRadius;

    [Header("Boss Settings")]
    [SerializeField] GameObject bossPrefab;
    [Tooltip("Boss radius should not be larger than 0.5 * min(maze width, maze height) (sorry to make you do math)")]
    [SerializeField] int bossRadius;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    /*
     * Randomly generate key location as grid coordinates 
     */
    public Vector3Int GenerateKeyGridCoords()
    {
        Vector3Int keyLocation = Vector3Int.zero;
        bool generated = false;

        while (!generated)
        {
            // choose a random spot
            int x = Mathf.FloorToInt(Random.value * _mSpawner.width);
            int y = Mathf.FloorToInt(Random.value * _mSpawner.height);

            keyLocation = new Vector3Int(x, y);

            if ((keyLocation - Vector3Int.FloorToInt(player.transform.position)).magnitude > keyRadius)
            {
                generated = true;
            }
        }
        
        return keyLocation;
    }

    public Vector3Int GenerateBossGridCoods()
    {
        return new Vector3Int(10, 10);
    }

    public void SpawnKey()
    {
        Vector3 keyWorldPosition = GetCellMidpointAsWorldPosition(GenerateKeyGridCoords());
        GameObject key = keyPrefab;
        key.transform.position = keyWorldPosition;
        Instantiate(keyPrefab);
        Debug.Log("Spawned Key @ " + keyWorldPosition);
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

}
