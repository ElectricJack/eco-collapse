using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class WorldGen : MonoBehaviour
{
    public GameObject CellPrefab;

    public int worldSize;
    
    public void Awake()
    {
        for(int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                GameObject.Instantiate(CellPrefab);
            }
        }
    }
}


public class Cell
{
    private Dictionary<Direction, Cell> neighbors = new Dictionary<Direction, Cell>();
    
    public void SetNeighbor(Direction dir, Cell neighbor)
    {
        
    }
}

public enum Direction
{
    North,
    West,
    South,
    East,
    NorthWest,
    NorthEast,
    SouthWest,
    SouthEast
}

public class WorldInstance
{

}