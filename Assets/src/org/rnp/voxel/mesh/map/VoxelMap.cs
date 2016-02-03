using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.mesh.map
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   An infinite voxel mesh.
  /// </summary>
  public sealed class VoxelMap : AbstractWritableVoxelMesh
  {
    private Dictionary<IVoxelLocation, IWritableVoxelMesh> _parts;

    public VoxelMap()
    {
      this._parts = new Dictionary<IVoxelLocation, IWritableVoxelMesh>(); 
    }
  }
}
