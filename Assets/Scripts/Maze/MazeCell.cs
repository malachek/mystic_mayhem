using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MazeCell
{
    Vector3Int _coords;
    int _setValue;
    bool _hasBottomWall = false;

    public MazeCell(Vector3Int coords)
    {
        _coords = coords;
    }

    public MazeCell(Vector3Int coords, int setValue)
    {
        _coords = coords;
        _setValue = setValue;
    }

    public void SetSetVal(int setVal)
    {
        _setValue = setVal;
    }

    public int GetSetValue()
    {
        return _setValue;
    }

    public Vector3Int GetCoords()
    {
        return _coords;
    }

    public void SetCoords(Vector3Int coords)
    {
        _coords = coords;
    }

    public bool GetBottomWallStatus()
    {
        return _hasBottomWall;
    }

    public void SetBottomWallStatus(bool status)
    {
        _hasBottomWall = status;
    }
}
