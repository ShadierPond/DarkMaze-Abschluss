using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Maze.Navigation
{
    public class NavigationBaker : MonoBehaviour
    {
        public void Bake(IEnumerable<NavMeshSurface> surfaces)
        {
            foreach (var surface in surfaces)
            {
                surface.BuildNavMesh();
            }
        }
    }
}