using JetBrains.Annotations;

namespace Aurayu.VoxelWorld.Voxel
{
    public struct Point2D
    {
        public readonly int X;
        public readonly int Z;

        public Point2D(int x, int z)
        {
            X = x;
            Z = z;
        }

        public override bool Equals([CanBeNull] object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var point = (Point2D) obj;
            return X == point.X && Z == point.Z;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Z.GetHashCode();
                return hash;
            }
        }
    }
}
