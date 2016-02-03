using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.mesh
{
  /// <auhtor>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</auhtor>
  /// <summary>
  ///   An unmodifiable voxel mesh. 
  ///   A call to a method that will modify the mesh should throw an error.
  /// </summary>
  public interface IReadonlyVoxelMesh : IVoxelMesh
  { }
}
