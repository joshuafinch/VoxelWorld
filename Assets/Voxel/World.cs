using System.Collections.Generic;
using Aurayu.VoxelWorld.Voxel.Block;
using JetBrains.Annotations;
using UnityEngine;

namespace Aurayu.VoxelWorld.Voxel
{
    public class World
    {
        public Dictionary<Point3D, Chunk> Chunks;

        public World()
        {
            Chunks = new Dictionary<Point3D, Chunk>();
        }

        [CanBeNull]
        public Chunk GetChunk(Point3D chunkPositionInWorld)
        {
            Chunk chunk;
            Chunks.TryGetValue(chunkPositionInWorld, out chunk); 
            return chunk;
        }

        public IBlock GetBlock(Point3D worldPosition)
        {
            var chunkPositionInWorld = GetChunkPositionInWorld(worldPosition);
            var chunk = GetChunk(chunkPositionInWorld);

            if (chunk == null)
                return new AirBlock();

            var blockPositionInChunk = GetBlockPositionInChunk(worldPosition, chunk.Position);
            var block = chunk.GetBlock(blockPositionInChunk);

            return block;
        }

        public void SetBlock(Point3D worldPosition, IBlock block)
        {
            var chunkPositionInWorld = GetChunkPositionInWorld(worldPosition);
            var chunk = GetChunk(chunkPositionInWorld);

            if (chunk == null)
                return;

            var x = worldPosition.X - chunk.Position.X;
            var y = worldPosition.Y - chunk.Position.Y;
            var z = worldPosition.Z - chunk.Position.Z;
            chunk.SetBlock(new Point3D(x, y, z), block);
        }

        private static Point3D GetChunkPositionInWorld(Point3D worldPosition)
        {
            var x = Mathf.FloorToInt(worldPosition.X / (float)Chunk.Width) * Chunk.Width;
            var y = Mathf.FloorToInt(worldPosition.Y / (float)Chunk.Height) * Chunk.Height;
            var z = Mathf.FloorToInt(worldPosition.Z / (float)Chunk.Depth) * Chunk.Depth;
            return new Point3D(x, y, z);
        }

        private static Point3D GetBlockPositionInChunk(Point3D blockPosition, Point3D chunkPosition)
        {
            var x = blockPosition.X - chunkPosition.X;
            var y = blockPosition.Y - chunkPosition.Y;
            var z = blockPosition.Z - chunkPosition.Z;
            return new Point3D(x, y, z);
        }
    }
}
