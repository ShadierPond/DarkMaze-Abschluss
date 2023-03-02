using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        [Header("Wall Settings")]
        [SerializeField] private float wallThickness = 0.1f;
        [SerializeField] private AssetReferenceGameObject wallReference;
        [SerializeField] private AssetReferenceGameObject floorReference;
        [SerializeField] private AssetReferenceGameObject ceilingReference;

        [Header("Gameplay Settings")]
        [SerializeField] private AssetReferenceGameObject playerSpawnerReference;
        [SerializeField] private AssetReferenceGameObject exitReference;

        [Header("Decorations Settings")]
        [SerializeField] private AssetReferenceGameObject[] decorations;
        [SerializeField] private int[] decorationAmounts;
        [SerializeField] private float[] decorationSpawnChance;
        
        
        [Header("Cell Settings")]
        [SerializeField] private Vector3 cellSize = new(1, 1, 1);
        
        [Header("Maze Settings")]
        [SerializeField] private Vector2Int mazeSize = new(10, 10);
        [SerializeField] private int seed = 0;
        [SerializeField] private bool randomSeed = true;
        
        [Header("Debug")]
        private MazeCell _startCell = new(0, 0);
        private MazeCell _endCell = new(0, 0);
        [SerializeField] private bool showCeilings = true;

        private MazeCell[,] _mazeCells;

        private void OnValidate()
        {
            if (mazeSize.x < 1)
            {
                mazeSize.x = 1;
            }
            if (mazeSize.y < 1)
            {
                mazeSize.y = 1;
            }
            
            if (decorations.Length != decorationAmounts.Length || decorations.Length != decorationSpawnChance.Length)
            {
                decorationAmounts = new int[decorations.Length];
                decorationSpawnChance = new float[decorations.Length];
            }
        }

        private void Start()
        {
            if (randomSeed)
            {
                seed = Random.Range(0, int.MaxValue);
            }
            Random.InitState(seed);
            GenerateMaze();
            RandomReplaceWallInCell(_startCell, playerSpawnerReference);
            RandomReplaceWallInCell(_endCell, exitReference);
            DrawMaze();
            RandomSpreadObjects();
        }
        
        private void GenerateMaze()
        {
            _mazeCells = new MazeCell[mazeSize.x, mazeSize.y];
            for (int x = 0; x < mazeSize.x; x++)
            {
                for (int y = 0; y < mazeSize.y; y++)
                {
                    _mazeCells[x, y] = new MazeCell(x, y);
                }
            }
            
            // Create a stack to store the visited cells
            Stack<MazeCell> visitedCells = new Stack<MazeCell>();
            // Start at a random cell
            MazeCell currentCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
            // Mark the cell as visited
            currentCell.Visited = true;
            // Add the cell to the stack
            visitedCells.Push(currentCell);
            
            // While there are still cells to visit
            while (visitedCells.Count > 0)
            {
                // Get the unvisited neighbours of the current cell
                List<MazeCell> unvisitedNeighbours = GetNeighbours(currentCell);
                // If there are unvisited neighbours
                if (unvisitedNeighbours.Count > 0)
                {
                    // Choose a random unvisited neighbour
                    MazeCell randomNeighbour = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                    // Remove the wall between the current cell and the random neighbour
                    if (randomNeighbour.X < currentCell.X)
                    {
                        currentCell.WallLeft = false;
                        randomNeighbour.WallRight = false;
                    }
                    else if (randomNeighbour.X > currentCell.X)
                    {
                        currentCell.WallRight = false;
                        randomNeighbour.WallLeft = false;
                    }
                    else if (randomNeighbour.Y < currentCell.Y)
                    {
                        currentCell.WallBottom = false;
                        randomNeighbour.WallTop = false;
                    }
                    else if (randomNeighbour.Y > currentCell.Y)
                    {
                        currentCell.WallTop = false;
                        randomNeighbour.WallBottom = false;
                    }
                    // Mark the random neighbour as visited
                    randomNeighbour.Visited = true;
                    // Add the random neighbour to the stack
                    visitedCells.Push(randomNeighbour);
                    // Set the random neighbour as the current cell
                    currentCell = randomNeighbour;
                }
                // If there are no unvisited neighbours
                else
                {
                    // Remove the current cell from the stack
                    visitedCells.Pop();
                    // If there are still cells in the stack
                    if (visitedCells.Count > 0)
                    {
                        // Set the current cell to the last cell in the stack
                        currentCell = visitedCells.Peek();
                    }
                }
            }
            // Set random Start and End cells
            _startCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
            _endCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
            while (_endCell == _startCell)
            {
                _endCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
            }
        }
        
        private List<MazeCell> GetNeighbours(MazeCell cell, bool includeVisited = false)
        {
            List<MazeCell> neighbours = new List<MazeCell>();
            if (cell.X > 0 && (includeVisited || !_mazeCells[cell.X - 1, cell.Y].Visited))
            {
                neighbours.Add(_mazeCells[cell.X - 1, cell.Y]);
            }
            if (cell.Y > 0 && (includeVisited || !_mazeCells[cell.X, cell.Y - 1].Visited))
            {
                neighbours.Add(_mazeCells[cell.X, cell.Y - 1]);
            }
            if (cell.X < mazeSize.x - 1 && (includeVisited || !_mazeCells[cell.X + 1, cell.Y].Visited))
            {
                neighbours.Add(_mazeCells[cell.X + 1, cell.Y]);
            }
            if (cell.Y < mazeSize.y - 1 && (includeVisited || !_mazeCells[cell.X, cell.Y + 1].Visited))
            {
                neighbours.Add(_mazeCells[cell.X, cell.Y + 1]);
            }
            return neighbours;
        }

        private void DrawMaze()
        {
            Debug.Log("Drawing maze");
            for (int x = 0; x < mazeSize.x; x++)
            {
                for (int y = 0; y < mazeSize.y; y++)
                {
                    MazeCell cell = _mazeCells[x, y];
                    Vector3 cellPosition = new Vector3(x * cellSize.x, 0, y * cellSize.z);
                    if (cell.WallLeft)
                    {
                        wallReference.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += wall =>
                        {
                            wall.Result.name = "Maze Wall";
                            wall.Result.transform.localScale = new Vector3(wallThickness, cellSize.y, cellSize.z);
                            wall.Result.transform.Translate(new Vector3(-cellSize.x / 2, 0, 0));
                        };
                    }
                    if (cell.WallRight)
                    {
                        wallReference.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += wall =>
                        {
                            wall.Result.name = "Maze Wall";
                            wall.Result.transform.localScale = new Vector3(wallThickness, cellSize.y, cellSize.z);
                            wall.Result.transform.Translate(new Vector3(cellSize.x / 2, 0, 0));
                        };
                    }
                    if (cell.WallTop)
                    {
                        wallReference.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += wall =>
                        {
                            wall.Result.name = "Maze Wall";
                            wall.Result.transform.localScale = new Vector3(wallThickness, cellSize.y, cellSize.x);
                            wall.Result.transform.Translate(new Vector3(0, 0, cellSize.z / 2));
                            wall.Result.transform.Rotate(new Vector3(0, 90, 0));
                        };
                    }
                    if (cell.WallBottom)
                    {
                        wallReference.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += wall =>
                        {
                            wall.Result.name = "Maze Wall";
                            wall.Result.transform.localScale = new Vector3(wallThickness, cellSize.y, cellSize.x);
                            wall.Result.transform.Translate(new Vector3(0, 0, -cellSize.z / 2));
                            wall.Result.transform.Rotate(new Vector3(0, 90, 0));
                        };
                    }
                    
                    floorReference.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += floor =>
                    {
                        floor.Result.name = "Maze Floor";
                        floor.Result.transform.localScale = new Vector3(cellSize.x, wallThickness, cellSize.z);
                        floor.Result.transform.Translate(new Vector3(0, -cellSize.y / 2, 0));
                    };

                    if (showCeilings)
                    {
                        ceilingReference.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += ceiling =>
                        {
                            ceiling.Result.name = "Maze Ceiling";
                            ceiling.Result.transform.localScale = new Vector3(cellSize.x, wallThickness, cellSize.z);
                            ceiling.Result.transform.Translate(new Vector3(0, cellSize.y / 2, 0));
                        };
                    }
                }
            }
        }

        private void SpawnObjectInCell(AssetReferenceGameObject obj, MazeCell cell)
        {
            
            cell.ContainsObject = true;
            Vector3 cellPosition = new Vector3(cell.X * cellSize.x, 0, cell.Y * cellSize.z);
            obj.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += objInstance =>
            {
                objInstance.Result.name = "Maze Object";
            };
        }
        
        private void RandomSpreadObjects()
        {
            for (int i = 0; i < decorations.Length; i++)
            {
                for (int j = 0; j < decorationAmounts[i]; j++)
                {
                    MazeCell spawnCell = _mazeCells[Random.Range(0, (int)mazeSize.x), Random.Range(0, (int)mazeSize.y)];
                    while (spawnCell.ContainsObject)
                    {
                        spawnCell = _mazeCells[Random.Range(0, (int)mazeSize.x), Random.Range(0, (int)mazeSize.y)];
                    }
                    SpawnObjectInCell(decorations[i], spawnCell);
                }
            }
        }

        private void RandomReplaceWallInCell(MazeCell spawnCell, AssetReference wall)
        {
            var cell = _mazeCells[spawnCell.X, spawnCell.Y];
            var cellPosition = new Vector3(cell.X * cellSize.x, 0, cell.Y * cellSize.z);
            var neighbours = GetNeighbours(cell, true);
            Debug.Log("Neighbours: " + neighbours.Count);
            var availableWalls = new List<string>();
            
            if (cell.WallLeft && neighbours[0] != null)
                availableWalls.Add("Left");
            if (cell.WallRight && neighbours[1] != null)
                availableWalls.Add("Right");
            if (cell.WallTop && neighbours[2] != null)
                availableWalls.Add("Top");
            if (cell.WallBottom && neighbours[3] != null)
                availableWalls.Add("Bottom");
            var wallToReplace = availableWalls[Random.Range(0, availableWalls.Count)];
            Debug.Log(availableWalls.Count);

            switch (wallToReplace)
            {
                case "Left":
                    cell.WallLeft = false;
                    neighbours[0].WallRight = false;
                    _mazeCells[neighbours[0].X, neighbours[0].Y] = neighbours[0];
                    Debug.Log("Replaced Left");
                    wall.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += spawner =>
                    {
                        spawner.Result.name = "Player Spawner";
                        spawner.Result.transform.Translate(new Vector3(-cellSize.x / 2, 0, 0));
                        spawner.Result.transform.Rotate(new Vector3(0, -90, 0));
                    };
                    break;
                case "Right":
                    cell.WallRight = false;
                    neighbours[1].WallLeft = false;
                    _mazeCells[neighbours[1].X, neighbours[1].Y] = neighbours[1];
                    Debug.Log("Replaced Right");
                    wall.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += spawner =>
                    {
                        spawner.Result.name = "Player Spawner";
                        spawner.Result.transform.Translate(new Vector3(cellSize.x / 2, 0, 0));
                        spawner.Result.transform.Rotate(new Vector3(0, 90, 0));
                    };
                    break;
                case "Top":
                    cell.WallTop = false;
                    neighbours[2].WallBottom = false;
                    _mazeCells[neighbours[2].X, neighbours[2].Y] = neighbours[2];
                    Debug.Log("Replaced Top");
                    wall.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += spawner =>
                    {
                        spawner.Result.name = "Player Spawner";
                        spawner.Result.transform.Translate(new Vector3(0, 0, cellSize.z / 2));
                    };
                    break;
                case "Bottom":
                    cell.WallBottom = false;
                    neighbours[3].WallTop = false;
                    _mazeCells[neighbours[3].X, neighbours[3].Y] = neighbours[3];
                    Debug.Log("Replaced Bottom");
                    wall.InstantiateAsync(cellPosition, Quaternion.identity, gameObject.transform).Completed += spawner =>
                    {
                        spawner.Result.name = "Player Spawner";
                        spawner.Result.transform.Translate(new Vector3(0, 0, -cellSize.z / 2));
                        spawner.Result.transform.Rotate(new Vector3(0, 180, 0));
                    };
                    break;
            }
            _mazeCells[cell.X, cell.Y] = cell;
            Debug.Log($"Spawned Wall at {cell.X}, {cell.Y} Replace {wallToReplace}");
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying)
                return;
            for (var x = 0; x < mazeSize.x; x++) 
                for (var y = 0; y < mazeSize.y; y++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(new Vector3(x * cellSize.x, 0, y * cellSize.z), cellSize);
                }
        }
    }
}

