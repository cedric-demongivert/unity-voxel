using JetBrains.Annotations;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh.builder
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A builder that produce voxel mesh.
  /// </summary>
  public interface IVoxelMeshBuilder : ICopiable<IVoxelMeshBuilder>
  {
    /// <summary>
    ///   Build a new voxel mesh.
    /// </summary>
    /// <returns></returns>
    IVoxelMesh Build();
  }
}