using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tools.BetterMapBuilder
{
    public class BetterMapBuilder : MonoBehaviour
    {
        public MapCell[,] Map;
        public List<MapBrush> Brushes = new();
        [HideInInspector] public Vector2Int mapSize;
        [HideInInspector] public Vector3Int tileSize;
        [HideInInspector] public string mapName;
        [HideInInspector] public GameObject mapParent;

        private void OnValidate()
        {
            if (mapSize.x < 1) mapSize.x = 1;
            if (mapSize.y < 1) mapSize.y = 1;

            if (Map == null || Map.Length != mapSize.x * mapSize.y)
                Map = new MapCell[mapSize.x, mapSize.y];
            //Build();
        }
        
        public void Build()
        {
            if (tileSize == Vector3Int.zero) return;

            for (var z = 0; z < mapSize.y; z++)
            {
                for (var x = 0; x < mapSize.x; x++)
                {
                    var position = new Vector3(x*tileSize.x, 0, z * tileSize.z);
                    var index = x * mapSize.y + z;
                    var prefab = Map[x, z].GameObject;

                    if (prefab == null) 
                        continue;
                    var obj = Instantiate(prefab, position, Quaternion.Euler(0, Map[x, z].RotationY, 0f));
                    obj.transform.SetParent(mapParent.transform);
                }
            }
        }
    }
}