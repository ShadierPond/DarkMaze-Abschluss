using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] private int cellSize = 1;
        [SerializeField] private Vector2Int mazeSize = new Vector2Int(10, 10);
        [SerializeField] private int seed = 0;
        [SerializeField] private bool randomSeed = true;
        

        private MazeCell[,] _mazeCells;
        
        private void Start()
        {
            GenerateMaze();
        }
        
        private void GenerateMaze()
        {
            _mazeCells = new MazeCell[mazeSize.x, mazeSize.y];
            for (int x = 0; x < mazeSize.x; x++)
            {
                for (int y = 0; y < mazeSize.y; y++)
                {
                    _mazeCells[x, y] = new MazeCell();
                    _mazeCells[x, y].Position = new Vector2Int(x, y);
                }
            }
            
            for (int x = 0; x < mazeSize.x; x++)
            {
                for (int y = 0; y < mazeSize.y; y++)
                {
                    _mazeCells[x, y].Neighbors = GetNeighbors(x, y);
                }
            }
        }
        
        private MazeCell[] GetNeighbors(int x, int y)
        {
            List<MazeCell> neighbors = new List<MazeCell>();
            if (x > 0)
            {
                neighbors.Add(_mazeCells[x - 1, y]);
            }
            if (x < mazeSize.x - 1)
            {
                neighbors.Add(_mazeCells[x + 1, y]);
            }
            if (y > 0)
            {
                neighbors.Add(_mazeCells[x, y - 1]);
            }
            if (y < mazeSize.y - 1)
            {
                neighbors.Add(_mazeCells[x, y + 1]);
            }
            return neighbors.ToArray();
        }
        
        private void OnDrawGizmos()
        {
            if (_mazeCells == null)
            {
                return;
            }
            
            for (int x = 0; x < mazeSize.x; x++)
            {
                for (int y = 0; y < mazeSize.y; y++)
                {
                    Gizmos.color = Color.white;
                    if (_mazeCells[x, y].IsWall)
                    {
                        Gizmos.color = Color.black;
                    }
                    if (_mazeCells[x, y].IsStart)
                    {
                        Gizmos.color = Color.green;
                    }
                    if (_mazeCells[x, y].IsEnd)
                    {
                        Gizmos.color = Color.red;
                    }
                    if (_mazeCells[x, y].IsPath)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    Gizmos.DrawCube(new Vector3(x * cellSize, 0, y * cellSize), Vector3.one * cellSize);
                }
            }
        }
        
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
        }
    }
}

