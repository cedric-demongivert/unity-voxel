using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh.submesh
{
  /// <author>Cédric DEMONGIVERT</author>
  /// <summary>
  ///   A part of a mesh.
  /// </summary>
  public interface ISubMesh : IVoxelMesh
  {
    /// <summary>
    ///   Get the entire mesh.
    /// </summary>
    IVoxelMesh ParentMesh
    {
      get;
    }

    /// <summary>
    ///   The offset of the submesh.
    /// </summary>
    VoxelLocation Offset
    {
      get;
    }
  }
}
