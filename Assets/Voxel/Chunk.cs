using System;
using System.Security.Cryptography.X509Certificates;

namespace Aurayu.VoxelWorld.Voxel
{
    public class Chunk
    {
        public const int Width = 16;
        public const int Height = 128;
        public const int Depth = 16;

        public IBlock[] Blocks { get; set; }

        public int X { get; set; }
        public int Z { get; set; }

        public Chunk()
        {
             
        }

        public Chunk(Point2D coordinates) : this()
        {
            X = coordinates.X;
            Z = coordinates.Z;

            const int size = Width * Height * Depth;
            Blocks = new IBlock[size];
        }

        // Sets the block at the relative coordinates
        public void SetBlock(Point3D coordinates, IBlock block)
        {
            var index = GetBlockIndex(coordinates);
            Blocks[index] = block;
        }

        // Gets the block at the relative coordinates
        public IBlock GetBlock(Point3D coordinates)
        {
            var index = GetBlockIndex(coordinates);
            return Blocks[index];
        }

        public IBlock GetBlockAdjacent(Point3D coordinates, Direction direction)
        {
            var index = GetBlockAdjacentIndex(coordinates, direction);
            return Blocks[index];
        }

        private static int GetBlockIndex(Point3D coordinates)
        {
            return coordinates.Y + (coordinates.Z*Height) + (coordinates.X*Height*Width);
        }

        private static int GetBlockAdjacentIndex(Point3D coordinates, Direction direction)
        {
            var adjacentCoordinates = GetBlockAdjacentCoordinates(coordinates, direction);
            return GetBlockIndex(adjacentCoordinates);
        }

        private static Point3D GetBlockAdjacentCoordinates(Point3D coordinates, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new Point3D(coordinates.X, coordinates.Y, coordinates.Z + 1);
                case Direction.East:
                    return new Point3D(coordinates.X - 1, coordinates.Y, coordinates.Z);
                case Direction.South:
                    return new Point3D(coordinates.X, coordinates.Y, coordinates.Z - 1);
                case Direction.West:
                    return new Point3D(coordinates.X + 1, coordinates.Y, coordinates.Z);
                case Direction.Up:
                    return new Point3D(coordinates.X, coordinates.Y + 1, coordinates.Z);
                case Direction.Down:
                    return new Point3D(coordinates.X, coordinates.Y - 1, coordinates.Z);
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }

        public void Update()
        {
        }

        public void Render()
        {
        }
    }
}
