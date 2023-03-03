using System.Collections.Generic;
using UnityEngine;

namespace GlobalSaveLoad
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
        public List<Vector3> enemyPositions;
        public List<Quaternion> enemyRotations;
        public List<int> enemyHealth;
    }
}