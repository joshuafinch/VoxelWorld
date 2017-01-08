using System;
using Aurayu.VoxelWorld.Voxel;
using UnityEngine;
using Mesh = Aurayu.VoxelWorld.Voxel.Mesh;

namespace Aurayu.VoxelWorld.Unity
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    internal class Chunk: MonoBehaviour
    {
        public Voxel.Chunk ChunkData;

        private MeshFilter _filter;
        private MeshCollider _collider;

        private void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _collider = gameObject.GetComponent<MeshCollider>();
        }

        private void Update()
        {
            if (!ChunkData.Update)
                return;

            ChunkData.Update = false;

            UpdateChunk();
        }

        /// <summary>
        /// Updates the chunk based on its contents
        /// </summary>
        private void UpdateChunk()
        {
            var mesh = new Mesh();

            for (var x = 0; x < Voxel.Chunk.Width; x++)
            {
                for (var y = 0; y < Voxel.Chunk.Height; y++)
                {
                    for (var z = 0; z < Voxel.Chunk.Depth; z++)
                    {
                        var coordinates = new Point3D(x, y, z);
                        var block = ChunkData.GetBlock(coordinates);
                        mesh = block.Mesh(ChunkData, coordinates, mesh);
                    }
                }
            }

            RenderMesh(mesh);
        }

        /// <summary>
        /// Sends the calculated mesh information to the mesh and collision components
        /// </summary>
        /// <param name="mesh"></param>
        private void RenderMesh(Mesh mesh)
        {
            Debug.Log("RenderMesh");

            _filter.mesh.Clear();
            _filter.mesh.vertices = mesh.Vertices.ToArray();
            _filter.mesh.triangles = mesh.Triangles.ToArray();
            _filter.mesh.uv = mesh.Uvs.ToArray();
            _filter.mesh.RecalculateNormals();
            _filter.mesh.Optimize();

            var colliderMesh = new UnityEngine.Mesh
            {
                vertices = mesh.ColliderVertices.ToArray(),
                triangles = mesh.ColliderTriangles.ToArray()
            };
            colliderMesh.RecalculateNormals();
            colliderMesh.Optimize();

            _collider.sharedMesh = colliderMesh;
        }
    }
}
