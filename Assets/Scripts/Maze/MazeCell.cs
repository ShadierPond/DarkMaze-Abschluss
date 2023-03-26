namespace MazeSystem
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
        
        // Cell contains an object ? (decoration)
        public bool ContainsObject { get; set; }
        
        // Cell does not have a floor
        public bool NoFloor { get; set; }
        
        // Cell contains an Enemy Spawn Point
        public bool ContainsEnemySpawnPoint { get; set; }

        // Constructor. Sets the cell's position in the maze
        public MazeCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}