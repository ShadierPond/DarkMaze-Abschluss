using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MazeSystem
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class MazeGenerator : MonoBehaviour
    {
        [Header("Wall Settings")]
        [SerializeField] private float wallThickness = 0.1f;
        [SerializeField] private AssetReferenceGameObject wallReference;
        [SerializeField] private AssetReferenceGameObject floorReference;

        [Header("Player Settings")]
        [SerializeField] private AssetReferenceGameObject playerReference;
        [SerializeField] private Vector3 playerSpawnOffset = new(0, 0, 0);
        [SerializeField] private AssetReferenceGameObject playerSpawnerReference;
        [SerializeField] private AssetReferenceGameObject playerExitReference;
        
        [Header("Enemy Settings")]
        [SerializeField] private AssetReferenceGameObject enemy;
        [SerializeField] private int enemyAmount;
        
        [Header("Jump scare Settings")]
        [SerializeField] private AssetReferenceGameObject jumpScare;
        [SerializeField] private int jumpScareAmount;

        [Header("Cell Settings")]
        [SerializeField] private Vector3 cellSize = new(1, 1, 1);
        
        [Header("Maze Settings")]
        [SerializeField] private Vector2Int mazeSize = new(10, 10);
        [SerializeField] private int seed;
        
        [Header("AI Navigation Settings")]
        public NavMeshSurface navMeshSurface;
        
        [Header("Debug")]
        private readonly List<AsyncOperationHandle<GameObject>> _objectsSpawning = new();
        private MazeCell _startCell = new(0, 0);
        private MazeCell _endCell = new(0, 0);
        private MazeCell[,] _mazeCells;

        /// <summary>
        /// This method is called when the script is loaded or a value is changed in the inspector. It clamps the maze size values to be at least 1.
        /// </summary>
        private void OnValidate()
        {
            // Check if the maze size x value is less than 1
            if (mazeSize.x < 1)
                // If yes, set it to 1
                mazeSize.x = 1;
            // Check if the maze size y value is less than 1
            if (mazeSize.y < 1)
                // If yes, set it to 1
                mazeSize.y = 1;
        }

        /// <summary>
        /// This method is called when the script instance is being loaded. It sets the seed to a random integer value and sets the maze size to a random vector2Int value between 10 and 30 for both x and y components. It also initializes the random state with the seed value.
        /// </summary>
        private void Awake()
        {
            // Set the seed to a random integer value between 0 and int.MaxValue
            seed = Random.Range(0, int.MaxValue);
            // Set the maze size to a random vector2Int value between 10 and 30 for both x and y components
            mazeSize = new Vector2Int(Random.Range(10, 30), Random.Range(10, 30));
            // Initialize the random state with the seed value
            Random.InitState(seed);
        }

        /// <summary>
        /// This method is called before the first frame update. It generates the maze using the GenerateMaze method. It also replaces the floor in the start and end cells with player spawner and player exit references. It also instantiates a player object asynchronously at the start cell position with an offset. It also draws the maze using the DrawMaze method. It also spreads some objects randomly using the RandomSpreadObjects method.
        /// </summary>
        private void Start()
        {
            // Generate the maze using the GenerateMaze method
            GenerateMaze();
            // Replace the floor in the start cell with the player spawner reference using the ReplaceFloorInCell method
            ReplaceFloorInCell(_startCell, playerSpawnerReference);
            // Replace the floor in the end cell with the player exit reference using the ReplaceFloorInCell method
            ReplaceFloorInCell(_endCell, playerExitReference);
            // Instantiate a player object asynchronously at the start cell position with an offset using the InstantiateAsync method
            InstantiateAsync(playerReference, new Vector3(_startCell.X * cellSize.x + playerSpawnOffset.x, playerSpawnOffset.y, _startCell.Y * cellSize.z + playerSpawnOffset.z), Vector3.zero, Vector3.zero);
            // Draw the maze using the DrawMaze method
            DrawMaze();
            // Spread some objects randomly using the RandomSpreadObjects method
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
        /// This method returns a list of neighbour cells for a given maze cell. It can optionally include visited cells or only unvisited cells.
        /// </summary>
        /// <param name="cell">MazeCell - The maze cell to get the neighbours for.</param>
        /// <param name="includeVisited">bool - Whether to include visited cells or not. The default value is false.</param>
        /// <returns>IEnumerable NeighbourCell - The list of neighbour cells.</returns>
        private IEnumerable<NeighbourCell> GetNeighbours(MazeCell cell, bool includeVisited = false)
        {
            // Create a new list of NeighbourCell objects
            var neighbours = new List<NeighbourCell>();
            // Check if the cell is not on the left edge of the maze
            if (cell.X > 0)
                // If yes, check if the includeVisited flag is true or the left neighbour cell is not visited
                if (includeVisited || !_mazeCells[cell.X - 1, cell.Y].Visited)
                    // If yes, add the left neighbour cell and its direction to the list
                    neighbours.Add(new NeighbourCell(_mazeCells[cell.X - 1, cell.Y], Direction.Left));
            // Check if the cell is not on the right edge of the maze
            if (cell.X < mazeSize.x - 1)
                // If yes, check if the includeVisited flag is true or the right neighbour cell is not visited
                if (includeVisited || !_mazeCells[cell.X + 1, cell.Y].Visited)
                    // If yes, add the right neighbour cell and its direction to the list
                    neighbours.Add(new NeighbourCell(_mazeCells[cell.X + 1, cell.Y], Direction.Right));
            // Check if the cell is not on the bottom edge of the maze
            if (cell.Y > 0)
                // If yes, check if the includeVisited flag is true or the bottom neighbour cell is not visited
                if (includeVisited || !_mazeCells[cell.X, cell.Y - 1].Visited)
                    // If yes, add the bottom neighbour cell and its direction to the list
                    neighbours.Add(new NeighbourCell(_mazeCells[cell.X, cell.Y - 1], Direction.Bottom));
            // Check if the cell is not on the top edge of the maze
            if (cell.Y < mazeSize.y - 1)
                // If yes, check if the includeVisited flag is true or the top neighbour cell is not visited
                if (includeVisited || !_mazeCells[cell.X, cell.Y + 1].Visited)
                    // If yes, add the top neighbour cell and its direction to the list
                    neighbours.Add(new NeighbourCell(_mazeCells[cell.X, cell.Y + 1], Direction.Top));
            // Return the list of neighbour cells
            return neighbours;
        }

        /// <summary>
        /// This method draws the maze by instantiating wall and floor objects for each maze cell based on its wall and floor flags. It uses the cell size and wall thickness values to position and scale the objects.
        /// </summary>
        private void DrawMaze()
        {
            // Loop through each x coordinate of the maze
            for (var x = 0; x < mazeSize.x; x++)
                // Loop through each y coordinate of the maze
                for (var y = 0; y < mazeSize.y; y++)
                {
                    // Get the maze cell at the current coordinates
                    var cell = _mazeCells[x, y];
                    // Calculate the cell position based on the cell size and coordinates
                    var cellPosition = new Vector3(x * cellSize.x, 0, y * cellSize.z);
                    // Check if the cell has a left wall
                    if (cell.WallLeft)
                        // If yes, instantiate a wall object asynchronously at the left side of the cell position with the appropriate scale and rotation using the InstantiateAsync method
                        InstantiateAsync(wallReference, cellPosition + new Vector3(-cellSize.x / 2, 0, 0), new Vector3(wallThickness, cellSize.y, cellSize.z), Vector3.zero);
                    // Check if the cell has a right wall
                    if (cell.WallRight)
                        // If yes, instantiate a wall object asynchronously at the right side of the cell position with the appropriate scale and rotation using the InstantiateAsync method
                        InstantiateAsync(wallReference, cellPosition + new Vector3(cellSize.x / 2, 0, 0), new Vector3(wallThickness, cellSize.y, cellSize.z), Vector3.zero);
                    // Check if the cell has a top wall
                    if (cell.WallTop)
                        // If yes, instantiate a wall object asynchronously at the top side of the cell position with the appropriate scale and rotation using the InstantiateAsync method
                        InstantiateAsync(wallReference, cellPosition + new Vector3(0, 0, cellSize.z / 2), new Vector3(wallThickness, cellSize.y, cellSize.x), new Vector3(0, 90, 0));
                    // Check if the cell has a bottom wall
                    if (cell.WallBottom)
                        // If yes, instantiate a wall object asynchronously at the bottom side of the cell position with the appropriate scale and rotation using the InstantiateAsync method
                        InstantiateAsync(wallReference, cellPosition + new Vector3(0, 0, -cellSize.z / 2), new Vector3(wallThickness, cellSize.y, cellSize.x), new Vector3(0, 90, 0));
                    // Check if the cell has a floor
                    if(!cell.NoFloor)
                        // If yes, instantiate a floor object asynchronously at the center of the cell position with the appropriate scale and rotation using the InstantiateAsync method
                        InstantiateAsync(floorReference, cellPosition + new Vector3(0, -cellSize.y / 2, 0), new Vector3(cellSize.x, wallThickness, cellSize.z), Vector3.zero);
                }
        }
        
        
        /// <summary>
        /// This method spreads some enemy and jump scare objects randomly in the maze cells. It uses the enemy amount and jump scare amount values to determine how many objects to spawn. It also checks if the cells already contain an enemy spawn point or a jump scare before spawning an object. It uses the InstantiateAsync method to instantiate the objects at the cell positions with the appropriate scale and rotation.
        /// </summary>
        private void RandomSpreadObjects()
        {
            // Loop for the enemy amount times
            for (var i = 0; i < enemyAmount; i++)
            {
                // Get a random maze cell
                var spawnCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
                // While the cell contains an enemy spawn point, get another random cell
                while (spawnCell.ContainsEnemySpawnPoint)
                    spawnCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
                // Set the cell's enemy spawn point flag to true
                spawnCell.ContainsEnemySpawnPoint = true;
                // Instantiate an enemy object asynchronously at the cell position with the default scale and rotation using the InstantiateAsync method
                InstantiateAsync(enemy, new Vector3(spawnCell.X * cellSize.x, 0, spawnCell.Y * cellSize.z), Vector3.one, Vector3.zero);
            }

            // Loop for the jump scare amount times
            for (var i = 0; i < jumpScareAmount; i++)
            {
                // Get a random maze cell
                var spawnCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
                // While the cell contains a jump scare, get another random cell
                while (spawnCell.ContainsJumpScare)
                    spawnCell = _mazeCells[Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y)];
                // Set the cell's jump scare flag to true
                spawnCell.ContainsJumpScare = true;
                // Instantiate a jump scare object asynchronously at the cell position with the cell size as the scale and the default rotation using the InstantiateAsync method
                InstantiateAsync(jumpScare, new Vector3(spawnCell.X * cellSize.x, 0, spawnCell.Y * cellSize.z), cellSize, Vector3.zero);
            }
        }

        /// <summary>
        /// This method replaces the floor in a given maze cell with a given floor asset reference. It sets the cell's no floor flag to true and instantiates the floor object asynchronously at the center of the cell position with the appropriate scale and rotation using the InstantiateAsync method.
        /// </summary>
        /// <param name="spawnCell">MazeCell - The maze cell to replace the floor in.</param>
        /// <param name="floor">AssetReference - The floor asset reference to instantiate.</param>
        private void ReplaceFloorInCell(MazeCell spawnCell, AssetReference floor)
        {
            // Get the maze cell at the spawn cell coordinates
            var cell = _mazeCells[spawnCell.X, spawnCell.Y];
            // Set the cell's no floor flag to true
            cell.NoFloor = true;
            // Calculate the cell position based on the cell size and coordinates
            var cellPosition = new Vector3(cell.X * cellSize.x, -(cellSize.y / 2), cell.Y * cellSize.z);
            // Instantiate a floor object asynchronously at the cell position with the appropriate scale and rotation using the InstantiateAsync method
            InstantiateAsync(floor, cellPosition , new Vector3(cellSize.x, wallThickness, cellSize.z), Vector3.zero);
        }
        
        
        /// <summary>
        /// This method instantiates a game object asynchronously from an asset reference at a given position, scale, and rotation. It also adds the instantiated object to a list of objects spawning and assigns a callback when the instantiation is completed. The callback sets the parent, name, scale, and rotation of the object and removes it from the list. It also calls the OnObjectsSpawningCompleted method to check if all the objects are spawned and build the nav mesh.
        /// </summary>
        /// <param name="assetReferenceGameObject">AssetReference - The asset reference of the game object to instantiate.</param>
        /// <param name="position">Vector3 - The position of the game object.</param>
        /// <param name="scale">Vector3 - The scale of the game object.</param>
        /// <param name="eulerRotation">Vector3 - The euler rotation of the game object.</param>
        private void InstantiateAsync(AssetReference assetReferenceGameObject, Vector3 position, Vector3 scale, Vector3 eulerRotation)
        {
            // Instantiate the game object asynchronously from the asset reference at the given position with an identity rotation and this game object as the parent
            var handle = assetReferenceGameObject.InstantiateAsync(new Vector3(position.x + (cellSize.x / 2), position.y + (cellSize.y / 2), position.z + (cellSize.z / 2)), Quaternion.identity, gameObject.transform);
            // Assign a callback when the instantiation is completed
            handle.Completed += obj =>
            {
                // Check if the instantiated object name contains "PlayerEnv"
                if (obj.Result.name.Contains("PlayerEnv"))
                    // If yes, set its parent to this game object's parent
                    obj.Result.transform.parent = gameObject.transform.parent;
                
                // Check if the instantiated object name contains "(Clone)"
                if (obj.Result.name.Contains("(Clone)"))
                    // If yes, remove it from the name
                    obj.Result.name = obj.Result.name.Replace("(Clone)", "");
                // Append the position vector to the instantiated object name
                obj.Result.name = new Vector3Int((int)position.x, (int)position.y, (int)position.z) + " " + obj.Result.name;
                // Set the instantiated object scale to the given scale or one if zero
                obj.Result.transform.localScale = scale != Vector3.zero ? scale : Vector3.one;
                // Check if the euler rotation is not zero
                if(eulerRotation != Vector3.zero)
                    // If yes, rotate the instantiated object by the euler rotation
                    obj.Result.transform.Rotate(eulerRotation);
                else
                    // If not, set the instantiated object rotation to identity
                    obj.Result.transform.rotation = Quaternion.identity;
                // Remove the handle from the list of objects spawning
                _objectsSpawning.Remove(handle);
                // Call the OnObjectsSpawningCompleted method to check if all the objects are spawned and build the nav mesh
                OnObjectsSpawningCompleted();
            };
            // Add the handle to the list of objects spawning
            _objectsSpawning.Add(handle);
        }

        /// <summary>
        /// This method checks if all the objects are spawned by counting the list of objects spawning. If yes, it builds the nav mesh using the nav mesh surface component.
        /// </summary>
        private void OnObjectsSpawningCompleted()
        {
            // Check if there are still objects spawning in the list
            if (_objectsSpawning.Count != 0)
                // If yes, return from the method
                return;
            // If not, build the nav mesh using the nav mesh surface component
            navMeshSurface.BuildNavMesh();
        }

        /// <summary>
        /// This method draws a wire cube gizmo for each maze cell in green color. It uses the cell size and coordinates to position and scale the gizmos. It only draws when not in play mode.
        /// </summary>
        private void OnDrawGizmos()
        {
            // Check if in play mode
            if(Application.isPlaying)
                // If yes, return from the method
                return;
            // Set the gizmo color to green
            Gizmos.color = Color.green;
            // Loop through each x coordinate of the maze
            for (var x = 0; x < mazeSize.x; x++) 
                // Loop through each y coordinate of the maze
            for (var y = 0; y < mazeSize.y; y++)
                // Draw a wire cube gizmo at each cell position with the cell size as the scale
                Gizmos.DrawWireCube(new Vector3(x * cellSize.x + (cellSize.x / 2), cellSize.y / 2, y * cellSize.z + (cellSize.z / 2)), cellSize);
        }
    }
}

