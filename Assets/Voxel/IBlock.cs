namespace Aurayu.VoxelWorld.Voxel
{
    public interface IBlock
    {
        bool IsSolid(Direction direction);

        Mesh Mesh(Chunk chunk, Point3D coordinates, Mesh mesh);
    }
}