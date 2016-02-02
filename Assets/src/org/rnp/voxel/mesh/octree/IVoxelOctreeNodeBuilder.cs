using JetBrains.Annotations;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.mesh.octree
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A builder that produce octree nodes.
  /// </summary>
  public interface IVoxelOctreeNodeBuilder
  {
    /// <summary>
    ///   Build a new octree node of a specific size.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    IWritableVoxelMesh Build(int width, int height, int depth);

    /// <summary>
    ///   Copy the builder.
    /// </summary>
    /// <returns></returns>
    IVoxelOctreeNodeBuilder Copy();
  }
}