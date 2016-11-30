using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aurayu.VoxelWorld.Voxel.Block
{
    public class AirBlock: IBlock
    {
        public bool IsSolid(Direction direction)
        {
            return false;
        }

        public Mesh Mesh(Chunk chunk, Point3D coordinates, Mesh mesh)
        {
            return mesh;
        }
    }
}
