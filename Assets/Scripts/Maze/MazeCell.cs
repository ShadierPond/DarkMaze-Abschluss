using UnityEngine;

namespace Maze
{
    public class MazeCell : MonoBehaviour
    {
        public bool IsVisited { get; set; }
        public bool IsWall { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool IsPath { get; set; }
        public Vector2Int Position { get; set; }
        public MazeCell[] Neighbors { get; set; }
    }
}