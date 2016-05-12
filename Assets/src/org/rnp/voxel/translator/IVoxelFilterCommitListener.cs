using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.translator
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   An object that listen for VoxelFilter change.
  /// </summary>
  public interface IVoxelFilterCommitListener
  {
    void OnRegister(VoxelFilter filter);
    void OnCommit(VoxelFilter filter);
    void OnUnregister(VoxelFilter filter);
  }
}
