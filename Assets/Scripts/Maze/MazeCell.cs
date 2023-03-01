using UnityEngine;

namespace Maze
{
    public class MazeCell
    {
        // Mark if the cell has been visited by the maze generation algorithm
        public bool Visited { get; set; }
        // The cell's position in the maze
        public int X { get; }
        public int Y { get; }
        
        // The cell's walls
        public bool WallTop { get; set; } = true;
        public bool WallRight { get; set; } = true;
        public bool WallBottom { get; set; } = true;
        public bool WallLeft { get; set; } = true;
        
        public bool ContainsObject { get; set; } = false;

        // Constructor. Sets the cell's position in the maze
        public MazeCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}