using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aurayu.VoxelWorld.Voxel.Block
{
    public class TextureSheet
    {
        private const int TileWidth = 32;
        private const int TileHeight = 32;

        private const int TextureSheetWidth = 192;
        private const int TextureSheetHeight = 32;

        private const float Columns = TextureSheetWidth/(float) TileWidth;
        private const float Rows = TextureSheetHeight/(float) TileHeight;

        public const float ColumnWidth = (float)1.0/Columns;
        public const float RowHeight = (float)1.0/Rows;
    }

    public class SolidBlock: IBlock
    {
        private const float HalfWidth = 0.5f;

        public bool IsSolid(Direction direction)
        {
            return true;
        }

        public Mesh Mesh(Chunk chunk, Point3D coordinates, Mesh mesh)
        {
            var x = coordinates.X;
            var y = coordinates.Y;
            var z = coordinates.Z;

            if (!chunk.GetBlockAdjacent(coordinates, Direction.Up).IsSolid(Direction.Down))
            {
                mesh = FaceUp(x, y, z, mesh);
            }

            if (!chunk.GetBlockAdjacent(coordinates, Direction.Down).IsSolid(Direction.Up))
            {
                mesh = FaceDown(x, y, z, mesh);
            }

            if (!chunk.GetBlockAdjacent(coordinates, Direction.North).IsSolid(Direction.South))
            {
                mesh = FaceNorth(x, y, z, mesh);
            }

            if (!chunk.GetBlockAdjacent(coordinates, Direction.South).IsSolid(Direction.North))
            {
                mesh = FaceSouth(x, y, z, mesh);
            }

            if (!chunk.GetBlockAdjacent(coordinates, Direction.East).IsSolid(Direction.West))
            {
                mesh = FaceEast(x, y, z, mesh);
            }

            if (!chunk.GetBlockAdjacent(coordinates, Direction.West).IsSolid(Direction.East))
            {
                mesh = FaceWest(x, y, z, mesh);
            }

            return mesh;
        }

        private static IEnumerable<Vector2> FaceUVs(Direction direction)
        {
            var uvs = new Vector2[4];

            const float width = TextureSheet.ColumnWidth;
            const float height = TextureSheet.RowHeight;

            var index = TileIndex(direction);

            uvs[0] = new Vector2(width * index.X, height * index.Z); // 0, 0
            uvs[1] = new Vector2(width * index.X + width, height * index.Z); // 1, 0
            uvs[2] = new Vector2(width * index.X, height * index.Z + height); // 0, 1
            uvs[3] = new Vector2(width * index.X + width, height * index.Z + height); // 1, 1

            return uvs;
        }

        private static Point2D TileIndex(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Point2D(0, 0);
                case Direction.Down:
                    return new Point2D(1, 0);
                case Direction.North:
                    return new Point2D(2, 0);
                case Direction.East:
                    return new Point2D(3, 0);
                case Direction.South:
                    return new Point2D(4, 0);
                case Direction.West:
                    return new Point2D(5, 0);
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }

        private static Mesh FaceUp(int x, int y, int z, Mesh mesh)
        {
            var vertex0 = new Vector3(x - HalfWidth, y + HalfWidth, z - HalfWidth);
            var vertex1 = new Vector3(x + HalfWidth, y + HalfWidth, z - HalfWidth);
            var vertex2 = new Vector3(x - HalfWidth, y + HalfWidth, z + HalfWidth);
            var vertex3 = new Vector3(x + HalfWidth, y + HalfWidth, z + HalfWidth);

            AddQuad(vertex0, vertex1, vertex2, vertex3, mesh);

            mesh.Uvs.AddRange(FaceUVs(Direction.Up));

            return mesh;
        }

        private static Mesh FaceDown(int x, int y, int z, Mesh mesh)
        {
            var vertex0 = new Vector3(x - HalfWidth, y - HalfWidth, z + HalfWidth);
            var vertex1 = new Vector3(x + HalfWidth, y - HalfWidth, z + HalfWidth);
            var vertex2 = new Vector3(x - HalfWidth, y - HalfWidth, z - HalfWidth);
            var vertex3 = new Vector3(x + HalfWidth, y - HalfWidth, z - HalfWidth);

            AddQuad(vertex0, vertex1, vertex2, vertex3, mesh);

            mesh.Uvs.AddRange(FaceUVs(Direction.Down));

            return mesh;
        }

        private static Mesh FaceNorth(int x, int y, int z, Mesh mesh)
        {
            var vertex0 = new Vector3(x + HalfWidth, y - HalfWidth, z + HalfWidth);
            var vertex1 = new Vector3(x - HalfWidth, y - HalfWidth, z + HalfWidth);
            var vertex2 = new Vector3(x + HalfWidth, y + HalfWidth, z + HalfWidth);
            var vertex3 = new Vector3(x - HalfWidth, y + HalfWidth, z + HalfWidth);

            AddQuad(vertex0, vertex1, vertex2, vertex3, mesh);

            mesh.Uvs.AddRange(FaceUVs(Direction.North));

            return mesh;
        }

        private static Mesh FaceSouth(int x, int y, int z, Mesh mesh)
        {
            var vertex0 = new Vector3(x - HalfWidth, y - HalfWidth, z - HalfWidth);
            var vertex1 = new Vector3(x + HalfWidth, y - HalfWidth, z - HalfWidth);
            var vertex2 = new Vector3(x - HalfWidth, y + HalfWidth, z - HalfWidth);
            var vertex3 = new Vector3(x + HalfWidth, y + HalfWidth, z - HalfWidth);

            AddQuad(vertex0, vertex1, vertex2, vertex3, mesh);

            mesh.Uvs.AddRange(FaceUVs(Direction.South));

            return mesh;
        }

        private static Mesh FaceEast(int x, int y, int z, Mesh mesh)
        {
            var vertex0 = new Vector3(x - HalfWidth, y - HalfWidth, z + HalfWidth);
            var vertex1 = new Vector3(x - HalfWidth, y - HalfWidth, z - HalfWidth);
            var vertex2 = new Vector3(x - HalfWidth, y + HalfWidth, z + HalfWidth);
            var vertex3 = new Vector3(x - HalfWidth, y + HalfWidth, z - HalfWidth);

            AddQuad(vertex0, vertex1, vertex2, vertex3, mesh);

            mesh.Uvs.AddRange(FaceUVs(Direction.East));

            return mesh;
        }

        private static Mesh FaceWest(int x, int y, int z, Mesh mesh)
        {
            var vertex0 = new Vector3(x + HalfWidth, y - HalfWidth, z - HalfWidth);
            var vertex1 = new Vector3(x + HalfWidth, y - HalfWidth, z + HalfWidth);
            var vertex2 = new Vector3(x + HalfWidth, y + HalfWidth, z - HalfWidth);
            var vertex3 = new Vector3(x + HalfWidth, y + HalfWidth, z + HalfWidth);

            AddQuad(vertex0, vertex1, vertex2, vertex3, mesh);

            mesh.Uvs.AddRange(FaceUVs(Direction.West));

            return mesh;
        }

        private static void AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Mesh mesh)
        {
            mesh.Vertices.Add(vertex0);
            mesh.Vertices.Add(vertex1);
            mesh.Vertices.Add(vertex2);
            mesh.Vertices.Add(vertex3);

            mesh.ColliderVertices.Add(vertex0);
            mesh.ColliderVertices.Add(vertex1);
            mesh.ColliderVertices.Add(vertex2);
            mesh.ColliderVertices.Add(vertex3);

            var numberOfVertices = mesh.Vertices.Count;
            var numberOfColliderVertices = mesh.ColliderVertices.Count;

            // Bottom Left Triangle
            mesh.Triangles.Add(numberOfVertices - 4);
            mesh.Triangles.Add(numberOfVertices - 2);
            mesh.Triangles.Add(numberOfVertices - 3);

            mesh.ColliderTriangles.Add(numberOfColliderVertices - 4);
            mesh.ColliderTriangles.Add(numberOfColliderVertices - 2);
            mesh.ColliderTriangles.Add(numberOfColliderVertices - 3);

            // Top Right Triangle
            mesh.Triangles.Add(numberOfVertices - 2);
            mesh.Triangles.Add(numberOfVertices - 1);
            mesh.Triangles.Add(numberOfVertices - 3);

            mesh.ColliderTriangles.Add(numberOfColliderVertices - 2);
            mesh.ColliderTriangles.Add(numberOfColliderVertices - 1);
            mesh.ColliderTriangles.Add(numberOfColliderVertices - 3);
        }
    }
}
