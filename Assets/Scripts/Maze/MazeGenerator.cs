using System;
using System.Collections.Generic;
using System.Linq;
using Maze.Navigation;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MazeSystem
{
    [RequireComponent(typeof(NavigationBaker)), RequireComponent(typeof(NavMeshSurface))]
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
        
        [Header("Enemy Settings")]
        [SerializeField] private AssetReferenceGameObject[] enemies;
        [SerializeField] private int[] enemyAmounts;
        [SerializeField] private float[] enemySpawnChance;


        [Header("Cell Settings")]
        [SerializeField] private Vector3 cellSize = new(1, 1, 1);
        
        [Header("Maze Settings")]
        [SerializeField] private Vector2Int mazeSize = new(10, 10);
        [SerializeField] private int seed = 0;
        [SerializeField] private bool randomSeed = true;
        
        [Header("AI Navigation Settings")]
        [SerializeField] private NavigationBaker navigationBaker;
        [SerializeField] private NavMeshSurface navMeshSurface;
        
        [Header("Debug")]
        private List<AsyncOperationHandle<GameObject>> objectsSpawning = new();
        private MazeCell _startCell = new(0, 0);
        private MazeCell _endCell = new(0, 0);
        [SerializeField] private bool showCeilings = true;

        private MazeCell[,] _mazeCells;

        private void OnValidate()
        {
            if (mazeSize.x < 1)
                mazeSize.x = 1;
            if (mazeSize.y < 1)
                mazeSize.y = 1;

            if (decorations.Length != decorationAmounts.Length || decorations.Length != decorationSpawnChance.Length)
            {
                decorationAmounts = new int[decorations.Length];
                decorationSpawnChance = new float[decorations.Length];
            }
            
            if (enemies.Length != enemyAmounts.Length || enemies.Length != enemySpawnChance.Length)
            {
                enemyAmounts = new int[enemies.Length];
                enemySpawnChance = new float[enemies.Length];
            }
        }

        private void Awake()
        {
            navigationBaker = GetComponent<NavigationBaker>();
            navMeshSurface = GetComponent<NavMeshSurface>();
        }

        private void Start()
        {
            seed = GameManager.Instance.RandomSeed ? Random.Range(0, int.MaxValue) : GameManager.Instance.Seed;
            Random.InitState(seed);
            GenerateMaze();
            RandomReplaceWallInCell(_startCell, playerSpawnerReference);
            RandomReplaceWallInCell(_endCell, exitReference);
            DrawMaze();
            RandomSpreadObjects();
        }

        /// <summary>
        ///   Generates a maze using the Recursive backtracking algorithm and stores it in a 2D array of cells<br/>
        ///   (https://en.wikipedia.org/wiki/Maze_generation_algorithm#Recursive_implementation)<br/>
        ///     1. Create a 2D array of cells to represent the maze<br/>
        ///     2. Create a stack to store the visited cells<br/>
        ///     3. Start at a random cell<br/>
        ///     4. Mark the cell as visited<br/>
        ///     5. Add the cell to the stack<br/>
        ///     6. While there are still cells to visit<br/>
        ///     7.     Get the unvisited neighbours of the current cell<br/>
        ///     8.     If there are unvisited neighbours<br/>
        ///     9.         Choose a random unvisited neighbour<br/>
        ///     10.        Remove the wall between the current cell and the chosen neighbour<br/>
        ///     11.        Set the chosen neighbour as the current cell<br/>
        ///     12.        Mark the chosen neighbour as visited<br/>
        ///     13.        Add the chosen neighbour to the stack<br/>
        ///     14.     Else<br/>
        ///     15.         Pop a cell from the stack and set it as the current cell<br/>
        ///     16. End while<br/>
        /// </summary>
        private void GenerateMaze()
        {
            _mazeCells = new MazeCell[mazeSize.x, mazeSize.y];
            for (var x = 0; x < mazeSize.x; x++)
                for (var y = 0; y < mazeSize.y; y++)
                    _mazeCells[x, y] = new MazeCell(x, y);

            var visitedCells = new Stack<MazeCell>();
            var currentCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
            currentCell.Visited = true;
            visitedCells.Push(currentCell);
            
            while (visitedCells.Count > 0)
            {
                var unvisitedNeighbours = GetNeighbours(currentCell).Where(n => !n.Cell.Visited).Select(n => n.Cell).ToList();
                if (unvisitedNeighbours.Count > 0)
                {
                    var randomNeighbour = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
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
                    randomNeighbour.Visited = true;
                    visitedCells.Push(randomNeighbour);
                    currentCell = randomNeighbour;
                }
                else
                {
                    visitedCells.Pop();
                    if (visitedCells.Count > 0)
                        currentCell = visitedCells.Peek();
                }
            }
            _startCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
            _endCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
            while (_endCell == _startCell)
                _endCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
        }
        
        /// <summary>
        ///  Returns a list of the neighbours of a cell<br/>
        ///  If includeVisited is true, the list will contain all neighbours, even if they have been visited<br/>
        ///  else, the list will only contain unvisited neighbours<br/>
        /// </summary>
        /// <param name="cell">MazeCell- The cell to get the neighbours of</param>
        /// <param name="includeVisited">bool- Whether to include visited neighbours</param>
        /// <returns> List of NeighbourCell- The neighbours of the cell</returns>
        private IEnumerable<NeighbourCell> GetNeighbours(MazeCell cell, bool includeVisited = false)
        {
            var neighbours = new List<NeighbourCell>();
            if (cell.X > 0)
                if (includeVisited || !_mazeCells[cell.X - 1, cell.Y].Visited)
                    neighbours.Add(new NeighbourCell(_mazeCells[cell.X - 1, cell.Y], Direction.Left));
            if (cell.X < mazeSize.x - 1)
                if (includeVisited || !_mazeCells[cell.X + 1, cell.Y].Visited)
                    neighbours.Add(new NeighbourCell(_mazeCells[cell.X + 1, cell.Y], Direction.Right));
            if (cell.Y > 0)
                if (includeVisited || !_mazeCells[cell.X, cell.Y - 1].Visited)
                    neighbours.Add(new NeighbourCell(_mazeCells[cell.X, cell.Y - 1], Direction.Bottom));
            if (cell.Y < mazeSize.y - 1)
                if (includeVisited || !_mazeCells[cell.X, cell.Y + 1].Visited)
                    neighbours.Add(new NeighbourCell(_mazeCells[cell.X, cell.Y + 1], Direction.Top));
            return neighbours;
        }

        /// <summary>
        ///  Draws the maze using the maze cells<br/>
        ///  It will instantiate a wall (walls will only be created if cell and neighbor were crossed in generation method - PATH BETWEEN!), Floor and Ceiling prefab for every Cell<br/>
        ///  They will be scaled to the correct size and position<br/><span style="color: #ff0000;">///  They will be parented to the maze object so that they can be easily deleted when the maze is regenerated</span><br/>
        ///  They will be parented to the maze object so that they can be easily deleted when the maze is regenerated<br/>
        ///  They will be named according to their position in the maze<br/>
        ///  They will be instantiated asynchronously so that the game does not freeze while the maze is being drawn<br/>
        /// </summary>
        private void DrawMaze()
        {
            for (var x = 0; x < mazeSize.x; x++)
                for (var y = 0; y < mazeSize.y; y++)
                {
                    var cell = _mazeCells[x, y];
                    var cellPosition = new Vector3(x * cellSize.x, 0, y * cellSize.z);
                    if (cell.WallLeft)
                        InstantiateAsync(wallReference, cellPosition + new Vector3(-cellSize.x / 2, 0, 0), new Vector3(wallThickness, cellSize.y, cellSize.z), Vector3.zero);
                    if (cell.WallRight)
                        InstantiateAsync(wallReference, cellPosition + new Vector3(cellSize.x / 2, 0, 0), new Vector3(wallThickness, cellSize.y, cellSize.z), Vector3.zero);
                    if (cell.WallTop)
                        InstantiateAsync(wallReference, cellPosition + new Vector3(0, 0, cellSize.z / 2), new Vector3(wallThickness, cellSize.y, cellSize.x), new Vector3(0, 90, 0));
                    if (cell.WallBottom)
                        InstantiateAsync(wallReference, cellPosition + new Vector3(0, 0, -cellSize.z / 2), new Vector3(wallThickness, cellSize.y, cellSize.x), new Vector3(0, 90, 0));
                    
                    InstantiateAsync(floorReference, cellPosition + new Vector3(0, -cellSize.y / 2, 0), new Vector3(cellSize.x, wallThickness, cellSize.z), Vector3.zero);
                    
                    if (showCeilings)
                        InstantiateAsync(ceilingReference, cellPosition + new Vector3(0, cellSize.y / 2, 0), new Vector3(cellSize.x, wallThickness, cellSize.z), Vector3.zero);
                }
        }
        
        private void InstantiateAsync(AssetReference assetReferenceGameObject, Vector3 position, Vector3 scale, Vector3 eulerRotation)
        {
            var handle = assetReferenceGameObject.InstantiateAsync(position, Quaternion.identity, gameObject.transform);
            handle.Completed += obj =>
            {
                obj.Result.name = position + "_Maze Object";
                if(scale != Vector3.one)
                    obj.Result.transform.localScale = scale;
                if(eulerRotation != Vector3.zero)
                    obj.Result.transform.Rotate(eulerRotation);
                objectsSpawning.Remove(handle);
                OnObjectsSpawningCompleted();
            };
            objectsSpawning.Add(handle);
        }
        
        private void OnObjectsSpawningCompleted()
        {
            if (objectsSpawning.Count != 0)
                return;
            navMeshSurface.BuildNavMesh();
        }
        /// <summary>
        /// Randomly spread objects with spawn chance in the Decoration array over the maze with the given amount in the DecorationAmount array
        /// If the Cell is already occupied, it will find a new one without decreasing the amount (while)
        /// </summary>
        private void RandomSpreadObjects()
        {
            for (var i = 0; i < decorations.Length; i++)
                for (var j = 0; j < decorationAmounts[i]; j++)
                {
                    var spawnCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
                    while (spawnCell.ContainsObject)
                        spawnCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
                    spawnCell.ContainsObject = true;
                    InstantiateAsync(decorations[i], new Vector3(spawnCell.X * cellSize.x, 0, spawnCell.Y * cellSize.z), Vector3.zero, Vector3.zero);
                }
            
            for (var i = 0; i < enemies.Length; i++)
                for (var j = 0; j < enemyAmounts[i]; j++)
                {
                    var spawnCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
                    while (spawnCell.ContainsEnemySpawnPoint)
                        spawnCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
                    spawnCell.ContainsEnemySpawnPoint = true;
                    InstantiateAsync(enemies[i], new Vector3(spawnCell.X * cellSize.x, 0, spawnCell.Y * cellSize.z), Vector3.one, Vector3.zero);
                }
        }

        /// <summary>
        /// Randomly picks a wall in a given cell and replaces it with a given wall like spawner, door, etc (AssetReference)
        /// </summary>
        /// <param name="spawnCell">MazeCell- The cell in which the wall should be replaced</param>
        /// <param name="wall">AssetReferenceGameObject - The reference to the wall to replace the old one with</param>
        private void RandomReplaceWallInCell(MazeCell spawnCell, AssetReferenceGameObject wall)
        {
            var cell = _mazeCells[spawnCell.X, spawnCell.Y];
            var cellPosition = new Vector3(cell.X * cellSize.x, 0, cell.Y * cellSize.z);
            var neighbours = GetNeighbours(cell, true);
            var availableWalls = new List<string>();
            
            if (cell.WallLeft)
                availableWalls.Add("Left");
            if (cell.WallRight)
                availableWalls.Add("Right");
            if (cell.WallTop)
                availableWalls.Add("Top");
            if (cell.WallBottom)
                availableWalls.Add("Bottom");
            
            switch (availableWalls[Random.Range(0, availableWalls.Count)])
            {
                case "Left":
                    cell.WallLeft = false;
                    foreach (var neighbour in neighbours.Where(neighbour => neighbour.Cell != null))
                    {
                        if(neighbour.Direction == Direction.Left)
                            neighbour.Cell.WallRight = false;
                        _mazeCells[neighbour.Cell.X, neighbour.Cell.Y] = neighbour.Cell;
                    }
                    InstantiateAsync(wall, cellPosition + new Vector3(-cellSize.x / 2, 0, 0), Vector3.zero, new Vector3(0, 90, 0));
                    break;
                case "Right":
                    cell.WallRight = false;
                    foreach (var neighbour in neighbours.Where(neighbour => neighbour.Cell != null))
                    {
                        if(neighbour.Direction == Direction.Right)
                            neighbour.Cell.WallLeft = false;
                        _mazeCells[neighbour.Cell.X, neighbour.Cell.Y] = neighbour.Cell;
                    }
                    InstantiateAsync(wall, cellPosition + new Vector3(cellSize.x / 2, 0, 0), Vector3.zero, new Vector3(0, -90, 0));
                    break;
                case "Top":
                    cell.WallTop = false;
                    foreach (var neighbour in neighbours.Where(neighbour => neighbour.Cell != null))
                    {
                        if(neighbour.Direction == Direction.Top)
                            neighbour.Cell.WallBottom = false;
                        _mazeCells[neighbour.Cell.X, neighbour.Cell.Y] = neighbour.Cell;
                    }
                    InstantiateAsync(wall, cellPosition + new Vector3(0, 0, cellSize.z / 2), Vector3.zero, new Vector3(0, 180, 0));
                    break;
                case "Bottom":
                    cell.WallBottom = false;
                    foreach (var neighbour in neighbours.Where(neighbour => neighbour.Cell != null))
                    {
                        if(neighbour.Direction == Direction.Bottom)
                            neighbour.Cell.WallTop = false;
                        _mazeCells[neighbour.Cell.X, neighbour.Cell.Y] = neighbour.Cell;
                    }
                    InstantiateAsync(wall, cellPosition + new Vector3(0, 0, -cellSize.z / 2), Vector3.zero, Vector3.zero);
                    break;
            }
            _mazeCells[cell.X, cell.Y] = cell;
        }
        
        public Vector3 GetMazePosition(Vector3 position)
        {
            return new Vector3(Mathf.Round(position.x / cellSize.x) * cellSize.x, 0, Mathf.Round(position.z / cellSize.z) * cellSize.z);
        }
        
        public Vector3 GetRandomPositionInMaze()
        {
            var cell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
            return new Vector3(cell.X * cellSize.x, 0, cell.Y * cellSize.z);
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

