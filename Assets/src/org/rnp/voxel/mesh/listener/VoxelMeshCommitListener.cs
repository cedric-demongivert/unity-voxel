using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///     An object that listen voxel mesh commit in order to display or to
  ///     do somes computations on it.
  /// </summary>
  public interface IVoxelMeshCommitListener
  {
    /// <summary>
    ///     Called when you starting to listen to a mesh for commits.
    /// </summary>
    /// <param name="mesh"></param>
    void OnRegister(VoxelMesh mesh);

    /// <summary>
    ///     Called when a mesh was commited.
    /// </summary>
    /// <param name="mesh"></param>
    void OnCommitBegin(VoxelMesh mesh);

    /// <summary>
    ///     Called when a mesh (and its submesh if exist) was commited.
    /// </summary>
    /// <param name="mesh"></param>
    void OnCommitEnd(VoxelMesh mesh);

    /// <summary>
    ///     Called when you stopping to listen to a mesh for commits.
    /// </summary>
    /// <param name="mesh"></param>
    void OnUnregister(VoxelMesh mesh);
  }
}
