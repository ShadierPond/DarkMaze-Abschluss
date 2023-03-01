using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.BetterMapBuilder
{
    [System.Serializable]
    public class MapCell
    {
        public Vector2Int Position;
        public GameObject GameObject;
        public Texture2D Texture;
        public int RotationY;

        public MapCell(Vector2Int position, GameObject gameObject, Texture2D texture, int rotationY)
        {
            Position = position;
            GameObject = gameObject;
            Texture = texture;
            RotationY = rotationY;
        }
    }
}