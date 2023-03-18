using UnityEngine;

namespace Management.SaveSystem
{
    [System.Serializable]
    public class Data
    {
        // Maze Data
        public  int levelSeed;
        
        // Player Data
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        public int playerHealth;
        
        // Weapon Data
        public int weaponIndex;
        public int weaponAmmo;
        public int weaponClipAmmo;
        
        // Enemy Data
        public Vector3[] enemyPositions;
        public Quaternion[] enemyRotations;
        public int[] enemyHealths;
        
        // Maze Data
        public Vector2Int mazeSize;
        public int mazeSeed;
    }
}