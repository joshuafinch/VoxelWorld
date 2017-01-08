using System.Collections.Generic;
using UnityEngine;

namespace Aurayu.VoxelWorld.Voxel
{
    public class Mesh
    {
        public List<Vector3> Vertices = new List<Vector3>();
        public List<int> Triangles = new List<int>();
        public List<Vector2> Uvs = new List<Vector2>();

        public List<Vector3> ColliderVertices = new List<Vector3>(); 
        public List<int> ColliderTriangles = new List<int>();
    }
}
