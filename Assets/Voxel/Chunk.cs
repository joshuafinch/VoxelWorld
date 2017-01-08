using System;

namespace Aurayu.VoxelWorld.Voxel
{
    public class Chunk
    {
        public const int Width = 16;
        public const int Height = 128;
        public const int Depth = 16;

        public IBlock[] Blocks { get; set; }

        public readonly Point3D Position;
        private readonly World _world;

        public bool update = false;

        public Chunk(Point3D position, World world)
        {
            Position = position;
            _world = world;

            const int size = Width * Height * Depth;
            Blocks = new IBlock[size];
        }

        // Sets the block at the relative coordinates
        public void SetBlock(Point3D coordinates, IBlock block)
        {
            if (InRangeX(coordinates.X) &&
                InRangeY(coordinates.Y) &&
                InRangeZ(coordinates.Z))
            {
                var index = GetBlockIndex(coordinates);
                Blocks[index] = block;
            }
            else
            {
                var x = Position.X + coordinates.X;
                var y = Position.Y + coordinates.Y;
                var z = Position.Z + coordinates.Z;
                _world.SetBlock(new Point3D(x, y, z), block);
            }

            update = true;
        }

        // Gets the block at the relative coordinates
        public IBlock GetBlock(Point3D coordinates)
        {
            if (InRangeX(coordinates.X) &&
                InRangeY(coordinates.Y) &&
                InRangeZ(coordinates.Z))
            {
                var index = GetBlockIndex(coordinates);
                return Blocks[index];
            }

            var x = Position.X + coordinates.X;
            var y = Position.Y + coordinates.Y;
            var z = Position.Z + coordinates.Z;
            return _world.GetBlock(new Point3D(x, y, z));
        }

        public IBlock GetBlockAdjacent(Point3D coordinates, Direction direction)
        {
            var adjacentBlockCoordinates = GetBlockAdjacentCoordinates(coordinates, direction);

            if (InRangeX(adjacentBlockCoordinates.X) &&
                InRangeY(adjacentBlockCoordinates.Y) &&
                InRangeZ(adjacentBlockCoordinates.Z))
            {
                var index = GetBlockAdjacentIndex(coordinates, direction);
                return Blocks[index];
            }

            var x = Position.X + adjacentBlockCoordinates.X;
            var y = Position.Y + adjacentBlockCoordinates.Y;
            var z = Position.Z + adjacentBlockCoordinates.Z;
            return _world.GetBlock(new Point3D(x, y, z));
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

        private static bool InRangeX(int index)
        {
            return index >= 0 && index < Width;
        }

        private static bool InRangeY(int index)
        {
            return index >= 0 && index < Height;
        }

        private static bool InRangeZ(int index)
        {
            return index >= 0 && index < Depth;
        }
    }
}
