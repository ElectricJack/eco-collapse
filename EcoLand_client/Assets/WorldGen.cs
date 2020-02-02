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

        public List<EntityProfile> randomTerrainEntities;
        public int numRandomSpawns = 10;
        public int randomSpawnVariance = 3;
        
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
                        -4 + ((5f * Mathf.PerlinNoise(scalarX, scalarY)) + 5f * Mathf.PerlinNoise(scalarX/4 + 100, scalarY/4 + 100)),
                        cell.location.location.y), 
                    Quaternion.identity);

                cell.cellObject = clone;
                SetUpCell(cell, scalarX, scalarY);
            }

            foreach (var cellPair in world.cells) {
                Cell cell = cellPair.Value;
                MeshFilter mesh = cell.cellObject.GetComponentInChildren<MeshFilter>();

                Vector3[] vertices = mesh.mesh.vertices;

                List<int> nwVert = new List<int>();
                List<int> neVert = new List<int>();
                List<int> swVert = new List<int>();
                List<int> seVert = new List<int>();

                for(int i = 0; i < vertices.Length; i++) {
                    if(vertices[i].y > 0) {
                        if(vertices[i].x > 0 && vertices[i].z > 0) {
                            nwVert.Add(i);
                        } else if(vertices[i].x < 0 && vertices[i].z > 0) {
                            neVert.Add(i);
                        } else if(vertices[i].x > 0 && vertices[i].z < 0) {
                            swVert.Add(i);
                        } else if(vertices[i].x < 0 && vertices[i].z < 0) {
                            seVert.Add(i);
                        }
                    }
                }

                float vertex4 = GetAverageHeightDifferenceForCorner(cell.GetWorldTile(), 
                    cell.GetNeighbor(Direction.West).GetWorldTile(),
                    cell.GetNeighbor(Direction.NorthWest).GetWorldTile(),
                    cell.GetNeighbor(Direction.North).GetWorldTile());

                float vertex5 = GetAverageHeightDifferenceForCorner(cell.GetWorldTile(),
                    cell.GetNeighbor(Direction.East).GetWorldTile(),
                    cell.GetNeighbor(Direction.NorthEast).GetWorldTile(),
                    cell.GetNeighbor(Direction.North).GetWorldTile());

                float vertex3 = GetAverageHeightDifferenceForCorner(cell.GetWorldTile(),
                    cell.GetNeighbor(Direction.West).GetWorldTile(),
                    cell.GetNeighbor(Direction.SouthWest).GetWorldTile(),
                    cell.GetNeighbor(Direction.South).GetWorldTile());

                float vertex2 = GetAverageHeightDifferenceForCorner(cell.GetWorldTile(),
                    cell.GetNeighbor(Direction.East).GetWorldTile(),
                    cell.GetNeighbor(Direction.SouthEast).GetWorldTile(),
                    cell.GetNeighbor(Direction.South).GetWorldTile());

                foreach(int i in neVert) {
                    vertices[i] = new Vector3(vertices[i].x, vertices[i].y - vertex4, vertices[i].z);
                }
                foreach (int i in nwVert) {
                    vertices[i] = new Vector3(vertices[i].x, vertices[i].y - vertex5, vertices[i].z);
                }
                foreach (int i in seVert) {
                    vertices[i] = new Vector3(vertices[i].x, vertices[i].y - vertex3, vertices[i].z);
                }
                foreach (int i in swVert) {
                    vertices[i] = new Vector3(vertices[i].x, vertices[i].y - vertex2, vertices[i].z);
                }

                mesh.mesh.vertices = vertices;
                mesh.mesh.RecalculateNormals();
            }

            SpawnRocks();
        }

        private float GetAverageHeightDifferenceForCorner(WorldTile me, WorldTile first, WorldTile second, WorldTile third) {
            float myEle = me.myCell.cellObject.transform.position.y;

            float wEle = first.myCell.cellObject.transform.position.y;
            float nwEle = second.myCell.cellObject.transform.position.y;
            float nEle = third.myCell.cellObject.transform.position.y;

            float averageEle = ((myEle - wEle) + (myEle - nwEle) + (myEle - nEle)) / 4;

            return averageEle;
        }

        private void SetUpCell(Cell cell, float scalarX, float scalarY) {
            float startingElevation = (Mathf.PerlinNoise(scalarX, scalarY) + Mathf.PerlinNoise(scalarX / 4 + 100, scalarY / 4 + 100)) / 2; // elevation normalized between 0-1
            float startingTemperature = Mathf.PerlinNoise(scalarX + 1000, scalarY + 1000);
            float startingHydration = Mathf.PerlinNoise(scalarX - 1000, scalarY - 1000);
            float startingBrightness = Mathf.PerlinNoise(scalarX/2 + 10000, scalarY/2 + 10000);
            float startingHumidity = 0.0001f;
            float startingFertility = 0.1f;

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

        private void SpawnRocks() {
            int numToSpawn = numRandomSpawns + UnityEngine.Random.Range(-randomSpawnVariance, randomSpawnVariance);
            for (int i = 0; i < numToSpawn; i++) {
                int spawnID = UnityEngine.Random.Range(0, randomTerrainEntities.Count);

                Vector3 randPos = new Vector3(UnityEngine.Random.Range(0, worldSize), 0, UnityEngine.Random.Range(0, worldSize));

                var cell = World.worldInstance.GetCellFromPosition(randPos);
                var nw = cell.GetNeighbor(Direction.NorthWest).cellObject.transform.position;
                var sw = cell.GetNeighbor(Direction.SouthWest).cellObject.transform.position;
                var ne = cell.GetNeighbor(Direction.NorthEast).cellObject.transform.position;
                var se = cell.GetNeighbor(Direction.SouthEast).cellObject.transform.position;

                float dx0 = (randPos.x - nw.x) / (ne.x - nw.x); // Along top
                float dx1 = (randPos.x - sw.x) / (se.x - sw.x); // Along bottom

                float yx0 = Mathf.Lerp(nw.y, ne.y, dx0);
                float yx1 = Mathf.Lerp(sw.y, se.y, dx1);

                float dz = (randPos.z - sw.z) / (nw.z - sw.z);

                randPos.y = Mathf.Lerp(yx1, yx0, dz) + 0.5f;

                EntityManager.instance.SpawnEntity(randPos, randomTerrainEntities[spawnID]);
            }
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
        public Cell GetCellFromPosition(Vector3 position) {
            float x1 = position.x;
            float y1 = position.z;

            x1 = Mathf.Clamp(x1, 0, worldSize - 1);
            y1 = Mathf.Clamp(y1, 0, worldSize - 1);

            return cells[new Vector2Int((int)Mathf.Round(x1), (int)Mathf.Round(y1))];
        }
        public Cell GetCellFromPosition(Vector2 position) {
            return GetCellFromPosition(new Vector3(position.x, 0, position.y));
        }

        public List<Cell> GatherNeighborCells(Vector2 position, int radius) {
            Cell centerCell = GetCellFromPosition(position);
            List<Cell> neighborCells = new List<Cell>();

            neighborCells.Add(centerCell);

            int x0 = centerCell.location.location.x - radius;
            int x1 = centerCell.location.location.x + radius;
            int y0 = centerCell.location.location.y - radius;
            int y1 = centerCell.location.location.y + radius;

            x0 = Mathf.Clamp(x0, 0, worldSize - 1);
            x1 = Mathf.Clamp(x1, 0, worldSize - 1);
            y0 = Mathf.Clamp(y0, 0, worldSize - 1);
            y1 = Mathf.Clamp(y1, 0, worldSize - 1);

            for (int i = x0; i <= x1; i++) {
                for (int j = y0; j <= y1; j++) {
                    neighborCells.Add(GetCellFromPosition(new Vector2(i, j)));
                }
            }

            return neighborCells;
        }

        public void GatherEntities(WorldTile origin, float radius, ref List<Entity> entities)
        {
            var nRad = (int)Math.Ceiling(radius);
            //origin.myCell.location.location;

            int x0 = origin.myCell.location.location.x - nRad;
            int x1 = origin.myCell.location.location.x + nRad;
            int y0 = origin.myCell.location.location.y - nRad;
            int y1 = origin.myCell.location.location.y + nRad;

            x0 = Mathf.Clamp(x0,0,worldSize-1);
            x1 = Mathf.Clamp(x1,0,worldSize-1);
            y0 = Mathf.Clamp(y0,0,worldSize-1);
            y1 = Mathf.Clamp(y1,0,worldSize-1);

            entities.Clear();
            for(int y=y0; y<=y1; ++y)
            {
                int rowOffset = y*worldSize;
                for(int x=x0; x<=x1; ++x)
                {
                    var tile = cellArray[rowOffset + x].GetWorldTile();
                    entities.AddRange(tile.GetRegisteredEntities());
                }
            }
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
            {Direction.NorthEast, new Vector2Int(1, 1)},
            {Direction.NorthWest, new Vector2Int(-1, 1)},
            {Direction.SouthEast, new Vector2Int(1, -1)},
            {Direction.SouthWest, new Vector2Int(-1, -1)},
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

        private WorldTile  tile;

        public  GameObject cellObject;

        public  Loc        location;
        
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
        North, // z+1
        West,  // x-1
        South, // z-1
        East,  // x+1
        NorthWest,
        NorthEast,
        SouthWest,
        SouthEast
    }
}