﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aurayu.VoxelWorld.Voxel
{
    public struct Point3D
    {
        public int X;
        public int Y;
        public int Z;

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
