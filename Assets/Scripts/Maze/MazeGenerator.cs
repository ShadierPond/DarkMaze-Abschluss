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
        [SerializeField] private GameObject playerSpawner;
        [SerializeField] private GameObject finishPoint;
        [SerializeField] private GameObject[] spawnableObjects;
        [SerializeField] private int[] spawnableObjectAmounts;
        [SerializeField] private float[] spawnableObjectSpawnChance;



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
            
            if (spawnableObjects.Length != spawnableObjectAmounts.Length)
            {
                Array.Resize(ref spawnableObjectAmounts, spawnableObjects.Length);
            }
            if (spawnableObjects.Length != spawnableObjectSpawnChance.Length)
            {
                Array.Resize(ref spawnableObjectSpawnChance, spawnableObjects.Length);
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
            //SpawnObjectInCell(playerSpawner, _startCell);
            //SpawnObjectInCell(finishPoint, _endCell);
            //RandomSpreadObjects();
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
            // Set the start cell
            _startCell = currentCell;
            // Add the cell to the stack
            visitedCells.Push(currentCell);
            
            // While there are still cells to visit
            while (visitedCells.Count > 0)
            {
                // Get the unvisited neighbours of the current cell
                List<MazeCell> unvisitedNeighbours = GetUnvisitedNeighbours(currentCell);
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
            // Set the end cell
            _endCell = currentCell;
            // Create the maze
            DrawMaze();
        }
        
        private List<MazeCell> GetUnvisitedNeighbours(MazeCell cell)
        {
            List<MazeCell> unvisitedNeighbours = new List<MazeCell>();
            if (cell.X > 0 && !_mazeCells[cell.X - 1, cell.Y].Visited)
            {
                unvisitedNeighbours.Add(_mazeCells[cell.X - 1, cell.Y]);
            }
            if (cell.Y > 0 && !_mazeCells[cell.X, cell.Y - 1].Visited)
            {
                unvisitedNeighbours.Add(_mazeCells[cell.X, cell.Y - 1]);
            }
            if (cell.X < mazeSize.x - 1 && !_mazeCells[cell.X + 1, cell.Y].Visited)
            {
                unvisitedNeighbours.Add(_mazeCells[cell.X + 1, cell.Y]);
            }
            if (cell.Y < mazeSize.y - 1 && !_mazeCells[cell.X, cell.Y + 1].Visited)
            {
                unvisitedNeighbours.Add(_mazeCells[cell.X, cell.Y + 1]);
            }
            return unvisitedNeighbours;
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

        private void SpawnObjectInCell(GameObject obj, MazeCell cell)
        {
            if (cell.ContainsObject)
            {
                Debug.LogError("Cell already contains an object");
                return;
            }
            cell.ContainsObject = true;
            Vector3 cellPosition = new Vector3(cell.X * cellSize.x, 0, cell.Y * cellSize.z);
            GameObject spawnedObject = Instantiate(obj, cellPosition, Quaternion.identity, gameObject.transform);
            spawnedObject.SetActive(true);
        }
        
        private void RandomSpreadObjects()
        {
            for (int i = 0; i < spawnableObjects.Length; i++)
            {
                for (int j = 0; j < spawnableObjectAmounts[i]; j++)
                {
                    if(Random.Range(0f, 1f) < spawnableObjectSpawnChance[i])
                        SpawnObjectInCell(spawnableObjects[i], _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)]);
                }
            }
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

