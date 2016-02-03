using JetBrains.Annotations;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.mesh.builder
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A builder that produce voxel mesh.
  /// </summary>
  public interface IVoxelMeshBuilder
  {
    /// <summary>
    ///   Build a new voxel mesh of a specific size.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    IVoxelMesh Build(int width, int height, int depth);

    /// <summary>
    ///   Copy the builder.
    /// </summary>
    /// <returns></returns>
    IVoxelMeshBuilder Copy();
  }
}