using Aurayu.VoxelWorld.Voxel;
using Aurayu.VoxelWorld.Voxel.Block;
using UnityEngine;

namespace Aurayu.VoxelWorld.Unity
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    internal class Chunk: MonoBehaviour
    {
        private Voxel.Chunk _chunk;

        private MeshFilter _filter;
        private MeshCollider _collider;

        private void Start()
        {
            _chunk = new Voxel.Chunk(new Point2D(0, 0));

            _filter = gameObject.GetComponent<MeshFilter>();
            _collider = gameObject.GetComponent<MeshCollider>();

            for (var x = 0; x < Voxel.Chunk.Width; x++)
            {
                for (var y = 0; y < Voxel.Chunk.Height; y++)
                {
                    for (var z = 0; z < Voxel.Chunk.Depth; z++)
                    {
                        _chunk.SetBlock(new Point3D(x, y, z), new AirBlock());
                    }
                }
            }

            _chunk.SetBlock(new Point3D(3, 5, 2), new SolidBlock());
            _chunk.SetBlock(new Point3D(4, 5, 2), new SolidBlock());

            UpdateChunk();
        }

        // Updates the chunk based on its contents
        private void UpdateChunk()
        {
            var mesh = new Voxel.Mesh();

            for (var x = 0; x < Voxel.Chunk.Width; x++)
            {
                for (var y = 0; y < Voxel.Chunk.Height; y++)
                {
                    for (var z = 0; z < Voxel.Chunk.Depth; z++)
                    {
                        var coordinates = new Point3D(x, y, z);
                        var block = _chunk.GetBlock(coordinates);
                        mesh = block.Mesh(_chunk, coordinates, mesh);
                    }
                }
            }

            RenderMesh(mesh);
        }
        // Sends the calculated mesh information
        // to the mesh and collision components
        private void RenderMesh(Voxel.Mesh mesh)
        {
            _filter.mesh.Clear();
            _filter.mesh.vertices = mesh.Vertices.ToArray();
            _filter.mesh.triangles = mesh.Triangles.ToArray();
            _filter.mesh.uv = mesh.Uvs.ToArray();
            _filter.mesh.RecalculateNormals();
        }
    }
}
