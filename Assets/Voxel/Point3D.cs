using JetBrains.Annotations;

namespace Aurayu.VoxelWorld.Voxel
{
    public struct Point3D
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals([CanBeNull] object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var point = (Point3D) obj;

            return X == point.X && Y == point.Y && Z == point.Z;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Z.GetHashCode();
                return hash;
            }
        }
    }
}
