using UnityEngine;

namespace Tools.BetterMapBuilder
{
    public class MapBrush
    {
        public Texture2D Texture;
        public GameObject GameObject;
        public int RotationY;
        
        public MapBrush(Texture2D texture, GameObject gameObject, int rotationY)
        {
            Texture = texture;
            GameObject = gameObject;
            RotationY = rotationY;
        }
        
        public MapBrush(Texture2D texture, GameObject gameObject) : this(texture, gameObject, 0)
        {
        }
        
        public MapBrush(Texture2D texture) : this(texture, null, 0)
        {
        }
        
        public MapBrush(GameObject gameObject) : this(null, gameObject, 0)
        {
        }
        
        public MapBrush() : this(null, null, 0)
        {
        }
        
        public MapBrush(Texture2D texture, int rotationY) : this(texture, null, rotationY)
        {
        }
        
        public MapBrush(GameObject gameObject, int rotationY) : this(null, gameObject, rotationY)
        {
        }
        
        public MapBrush(int rotationY) : this(null, null, rotationY)
        {
        }
        
        public MapBrush(Texture2D texture, GameObject gameObject, float rotationY) : this(texture, gameObject, (int) rotationY)
        {
        }
        
        public MapBrush(Texture2D texture, float rotationY) : this(texture, null, (int) rotationY)
        {
        }
        
        public MapBrush(GameObject gameObject, float rotationY) : this(null, gameObject, (int) rotationY)
        {
        }
        
        public MapBrush(float rotationY) : this(null, null, (int) rotationY)
        {
        }
        
        public MapBrush(Texture2D texture, GameObject gameObject, double rotationY) : this(texture, gameObject, (int) rotationY)
        {
        }
        
        public MapBrush(Texture2D texture, double rotationY) : this(texture, null, (int) rotationY)
        {
        }
        
        public MapBrush(GameObject gameObject, double rotationY) : this(null, gameObject, (int) rotationY)
        {
        }
        
        public MapBrush(double rotationY) : this(null, null, (int) rotationY)
        {
        }
        
        public MapBrush(Texture2D texture, GameObject gameObject, long rotationY) : this(texture, gameObject, (int) rotationY)
        {
        }
        
        public MapBrush(Texture2D texture, long rotationY) : this(texture, null, (int) rotationY)
        {
        }
        
        public MapBrush(GameObject gameObject, long rotationY) : this(null, gameObject, (int) rotationY)
        {
        }
        
        public MapBrush(long rotationY) : this(null, null, (int) rotationY)
        {
        }
        
        public MapBrush(Texture2D texture, GameObject gameObject, short rotationY) : this(texture, gameObject, (int) rotationY)
        {
        }
        
        public MapBrush(Texture2D texture, short rotationY) : this(texture, null, (int) rotationY)
        {
        }
    }
}