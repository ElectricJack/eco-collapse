using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using EntitySystem;
using System;

namespace Josh
{
    public class WorldGen : MonoBehaviour
    {
        public GameObject CellPrefab;

        public int worldSize;

        public World world;

        public AnimationCurve Curve;
        
        public void Awake()
        {
            world = new World(worldSize);

            foreach (var cellPair in world.cells)
            {
                var cell = cellPair.Value;

                var scalarX = 7f * ((float) cell.location.location.x) / worldSize;
                var scalarY = 7f * ((float) cell.location.location.y) / worldSize;
                GameObject clone = GameObject.Instantiate(CellPrefab, 
                    new Vector3(
                        cell.location.location.x, 
                        -2 + (5f * Mathf.PerlinNoise(scalarX, scalarY)),
                        cell.location.location.y), 
                    Quaternion.identity);

                cell.cellObject = clone;
                SetUpCell(cell, scalarX, scalarY);
            }
        }

        private void SetUpCell(Cell cell, float scalarX, float scalarY) {
            float startingElevation = Mathf.PerlinNoise(scalarX, scalarY); // elevation normalized between 0-1
            float startingTemperature = Mathf.PerlinNoise(scalarX + 1000, scalarY + 1000);
            float startingHydration = Mathf.PerlinNoise(scalarX - 1000, scalarY - 1000);
            float startingBrightness = Mathf.PerlinNoise(scalarX/10 + 10000, scalarY/10 + 10000);
            float startingHumidity = 0.0001f;
            float startingFertility = 0.0001f;

            cell.GetWorldTile().elevation = startingElevation;
            cell.GetWorldTile().temperature = startingTemperature;
            cell.GetWorldTile().hydration = startingHydration;
            cell.GetWorldTile().brightness = startingBrightness;
            cell.GetWorldTile().humidity = startingHumidity;
            cell.GetWorldTile().fertility = startingFertility;
        }

        public void OnDestroy()
        {
            World.worldInstance = null;
        }
    }

    public class V2IComparator : IEqualityComparer<Vector2Int>
    {
        private IEqualityComparer<Vector2Int> _equalityComparerImplementation;
        public bool Equals(Vector2Int x, Vector2Int y)
        {
            return x == y;
        }

        public int GetHashCode(Vector2Int obj)
        {
            return obj.x + (obj.y << 8);
        }
    }
    
    public class World
    {
        public Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>(new V2IComparator());

        public Cell[] cellArray;
        
        public int worldSize;

        public static World worldInstance;
        
        public World(int worldSize)
        {
            worldInstance = this;
            
            this.worldSize = worldSize;
            
            this.cellArray = new Cell[worldSize*worldSize];
            
            for(int x = 0; x < worldSize; x++)
            {
                for (int y = 0; y < worldSize; y++)
                {
                    var cell = new Cell(x,y);

                    cellArray[x + y * worldSize] = cell;
                    cells.Add(cell.location.location, cell);
                }
            }

            foreach (var cell in cells)
            {
                cell.Value.SetNeighbor(Direction.East, cells[cell.Value.location.GetNeighbor(Direction.East)]);
                cell.Value.SetNeighbor(Direction.West, cells[cell.Value.location.GetNeighbor(Direction.West)]);
                cell.Value.SetNeighbor(Direction.North, cells[cell.Value.location.GetNeighbor(Direction.North)]);
                cell.Value.SetNeighbor(Direction.South, cells[cell.Value.location.GetNeighbor(Direction.South)]);
                cell.Value.SetNeighbor(Direction.NorthEast, cells[cell.Value.location.GetNeighbor(Direction.NorthEast)]);
                cell.Value.SetNeighbor(Direction.NorthWest, cells[cell.Value.location.GetNeighbor(Direction.NorthWest)]);
                cell.Value.SetNeighbor(Direction.SouthEast, cells[cell.Value.location.GetNeighbor(Direction.SouthEast)]);
                cell.Value.SetNeighbor(Direction.SouthWest, cells[cell.Value.location.GetNeighbor(Direction.SouthWest)]);
            }
        }

        // Helper functions
        public Cell GetCellFromPosition(Vector2 position) {
            Vector2Int roundedPosition = new Vector2Int((int)Mathf.Round(position.x), (int)Mathf.Round(position.y));
            Debug.Log(roundedPosition);
            var c = cells[roundedPosition];
            Debug.Log(c.location.location);
            Debug.Log(cells.Any(x => x.Key == roundedPosition));
            return c;
            
        }

        public List<Entity> GatherEntities(WorldTile origin, float radius)
        {
            var nRad = (int)Math.Ceiling(radius);
            //origin.myCell.location.location;

            int x0 = origin.myCell.location.location.x - nRad;
            int x1 = origin.myCell.location.location.x + nRad;
            int y0 = origin.myCell.location.location.y - nRad;
            int y1 = origin.myCell.location.location.y + nRad;

            //startIndex = x0 + y0*numAcross;
            //@TODO
            return null;
        }
    }

    public class Loc
    {
        public static Dictionary<Direction, Vector2Int> DirMod = new Dictionary<Direction, Vector2Int>()
        {
            {Direction.North, new Vector2Int(0, 1)},
            {Direction.West, new Vector2Int(-1, 0)},
            {Direction.South, new Vector2Int(0, -1)},
            {Direction.East, new Vector2Int(1, 0)},
            {Direction.NorthEast, new Vector2Int(-1, 1)},
            {Direction.NorthWest, new Vector2Int(1, 1)},
            {Direction.SouthEast, new Vector2Int(-1, -1)},
            {Direction.SouthWest, new Vector2Int(1, -1)},
        };
    
        public Vector2Int location;

        public Loc(Vector2Int location)
        {
            this.location = location;
        }

        public Vector2Int GetNeighbor(Direction direction)
        {
            var pos = location + DirMod[direction];
            if (pos.x < 0)
            {
                pos.x = World.worldInstance.worldSize - 1;
            }

            if (pos.x >= World.worldInstance.worldSize)
            {
                pos.x = 0;
            }
            
            if (pos.y < 0)
            {
                pos.y = World.worldInstance.worldSize - 1;
            }

            if (pos.y >= World.worldInstance.worldSize)
            {
                pos.y = 0;
            }

            return pos;
        }
    }


    public class Cell
    {
        private Dictionary<Direction, Cell> neighbors = new Dictionary<Direction, Cell>();

        private WorldTile tile;

        public GameObject cellObject;

        public Loc location;
        
        public Cell(int x, int y)
        {
            location = new Loc(new Vector2Int(x, y));
            tile = new WorldTile(this);
        }
        
        public void SetNeighbor(Direction dir, Cell neighbor)
        {
            neighbors.Add(dir, neighbor);
        }

        public Cell GetNeighbor(Direction dir)
        {
            return neighbors[dir];
        }

        public Dictionary<Direction, Cell> GetNeighbors() {
            return neighbors;
        }

        public WorldTile GetWorldTile() {
            return tile;
        }
    }
    
    public enum Direction
    {
        North, // y+1
        West, // x-1
        South, // y-1
        East, // x+1
        NorthWest,
        NorthEast,
        SouthWest,
        SouthEast
    }
}