using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.mesh.absolute
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   An absolute positionned mesh.
  /// </summary>
  public interface IAbsoluteVoxelMesh : IVoxelMesh
  {
    IVoxelMesh RelativeMesh
    {
      get;
    }
  }
}
