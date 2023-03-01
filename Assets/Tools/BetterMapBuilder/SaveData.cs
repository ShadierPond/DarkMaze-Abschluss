using System.Collections.Generic;
using UnityEngine;

namespace Tools.BetterMapBuilder
{
    [System.Serializable]
    public class MapData
    {
        // Map Data
        public MapCell[,] Map;
        public Vector2Int mapSize;
        public Vector3Int tileSize;

        public MapData(MapCell[,] map, Vector2Int mapSize, Vector3Int tileSize)
        {
            Map = map;
            this.mapSize = mapSize;
            this.tileSize = tileSize;
        }
    }

    [System.Serializable]
    public class BrushesData
    {
        public List<MapBrush> Brushes;
        
        public BrushesData(List<MapBrush> brushes)
        {
            Brushes = brushes;
        }
    }
}