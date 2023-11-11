using System;
using UnityEngine;

public class MazeGrid
{
    private int _width;
    private int _height;
    private int _mazeCellSize;
    private Vector3Int _origin;
    private int[,] gridArray;

    public MazeGrid(int width, int height, int mazeCellSize, Vector3Int origin)
    {
        _width = width;
        _height = height;
        _mazeCellSize = mazeCellSize;
        _origin = origin;

        gridArray = new int[width, height];

        //for (int x = 0; x < _width; x++)
        //{
        //    for (int y = 0; y < _height; y++)
        //    {
        //        Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x+1,y));

        //        Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y+1));
        //    }
        //}
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    public int GetCellSize()
    {
        return _mazeCellSize;
    }

    // returns the world position given grid coordinates.
    public Vector3Int GetWorldPosition(int x, int y)
    {
        return new Vector3Int(x, y) * _mazeCellSize + _origin;
    }

    // returns grid coords given a world position.
    public int[] GetXYGridCoords(Vector3Int worldPos)
    {
        int x = (worldPos.x - _origin.x) / _mazeCellSize;
        int y = (worldPos.y - _origin.y) / _mazeCellSize;
        return new int[] { x, y };
    }

}
