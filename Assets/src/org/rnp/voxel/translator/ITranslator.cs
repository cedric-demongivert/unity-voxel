using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.translator
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   An object that transform a voxel mesh to a renderable mesh.
  /// </summary>
  public interface ITranslator
  {
    /// <summary>
    ///   Translated mesh.
    /// </summary>
    IVoxelMesh Mesh
    {
      get;
      set;
    }

    /// <summary>
    ///   Recalculate the final renderable mesh but don't send moifications
    /// to the graphic card.
    /// </summary>
    void Translate();

    /// <summary>
    ///   Send the new rendered mesh to the graphic card.
    /// </summary>
    void Publish();
  }
}
