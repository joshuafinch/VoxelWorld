using Aurayu.VoxelWorld.Voxel;
using System.Collections.Generic;
using Aurayu.VoxelWorld.Voxel.Block;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Aurayu.VoxelWorld.Unity
{
    internal class World: MonoBehaviour
    {
        [SerializeField]
        private GameObject _chunkPrefab = null;

        private readonly Dictionary<Point3D, Chunk> _chunks = new Dictionary<Point3D, Chunk>();
        private readonly Voxel.World _world = new Voxel.World();

        internal void Start()
        {
            Debug.Assert(_chunkPrefab != null, "_chunkPrefab != null");

            for (var x = -2; x < 2; x++)
            {
                for (var y = -1; y < 1; y++)
                {
                    for (var z = -1; z < 1; z++)
                    {
                        CreateChunk(new Point3D(x * Voxel.Chunk.Width,
                            y * Voxel.Chunk.Height,
                            z * Voxel.Chunk.Depth));
                    }
                }
            }

            //CreateChunk(new Point3D(0, 0, 0));
        }

        internal void CreateChunk(Point3D coordinates)
        {
            var newChunkObject = Instantiate(
                _chunkPrefab,
                new Vector3(coordinates.X, coordinates.Y, coordinates.Z),
                Quaternion.Euler(Vector3.zero)) as GameObject;

            Debug.Assert(newChunkObject != null, "newChunkObject != null");
            var newChunk = newChunkObject.GetComponent<Chunk>();

            newChunk.ChunkData = new Voxel.Chunk(coordinates, _world);

            _world.Chunks.Add(coordinates, newChunk.ChunkData);
            _chunks.Add(coordinates, newChunk);

            for (var x = 0; x < Voxel.Chunk.Width; x++)
            {
                for (var y = 0; y < Voxel.Chunk.Height; y++)
                {
                    for (var z = 0; z < Voxel.Chunk.Depth; z++)
                    {
                        if (y <= 7)
                        {
                            var p = new Point3D(x + coordinates.X, y + coordinates.Y, z + coordinates.Z);
                            _world.SetBlock(p, new SolidBlock());
                        }
                        else
                        {
                            var p = new Point3D(x + coordinates.X, y + coordinates.Y, z + coordinates.Z);
                            _world.SetBlock(p, new AirBlock());
                        }
                    }
                }
            } 
        }

        internal void DestroyChunk(Point3D coordinates)
        {
            Chunk chunk;

            if (!_chunks.TryGetValue(coordinates, out chunk))
                return;

            Destroy(chunk.gameObject);
            _chunks.Remove(coordinates);
        }

        internal IBlock GetBlock(Point3D positionInWorld)
        {
            return _world.GetBlock(positionInWorld);
        }

        internal bool SetBlock(Point3D positionInWorld, IBlock block)
        {
            return _world.SetBlock(positionInWorld, block, true);
        }
    }
}
