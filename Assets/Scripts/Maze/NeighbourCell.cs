namespace MazeSystem
{
    public class NeighbourCell
    {
        // The neighbour cell
        public MazeCell Cell { get; set; }
        
        // The direction of the neighbour cell relative to the current cell
        private Direction Direction { get; set; }
        
        // Constructor for the neighbour cell
        public NeighbourCell(MazeCell cell, Direction direction)
        {
            Cell = cell;
            Direction = direction;
        }
        
    }
    
    // The direction of the neighbour cell relative to the current cell
    public enum Direction
    {
        Top,
        Right,
        Bottom,
        Left
    }
}