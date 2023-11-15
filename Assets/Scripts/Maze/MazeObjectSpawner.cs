using UnityEngine;
using System.Collections;

public class MazeObjectSpawner : MonoBehaviour
{
    [SerializeField] MazeGridSpawner _mSpawner;

    [SerializeField] GameObject keyPrefab;
    [SerializeField] int keyRadius;

    [SerializeField] GameObject bossPrefab;
    [SerializeField] int bossRadius;

    public Vector3Int GenerateKeyGridCoords()
    {
        return new Vector3Int(0, 0);
    }

    public Vector3Int GenerateBossGridCoods()
    {
        return new Vector3Int(10, 10);
    }

    public void SpawnKey()
    {
        Vector3 keyWorldPosition = GetCellMidpointAsWorldPosition(GenerateKeyGridCoords());
        Instantiate(keyPrefab);
        Debug.Log("Spawned Key @ " + keyWorldPosition);
        keyPrefab.transform.position = keyWorldPosition;
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
