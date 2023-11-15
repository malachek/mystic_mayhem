using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGridSpawner : MonoBehaviour
{
    public int width;
    public int height;
    public int mazeCellSize;
    public GameObject origin;

    public Grid g; // sorry, i know this variable name is terrible. it's temporary though; the MazeGrid class will be phased out p soon.

    [SerializeField] public (int x, int y) keySpawnLocation = (-1, -1); // TODO: replace this with automatically generated location using radius away from player starting position...
    [SerializeField] public GameObject keyPrefab;

    [HideInInspector] public MazeGrid grid;

    private void Awake()
    {
        grid = new MazeGrid(width, height, mazeCellSize, Vector3Int.CeilToInt(origin.transform.position));
    }
}
