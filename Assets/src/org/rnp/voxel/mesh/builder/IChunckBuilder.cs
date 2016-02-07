using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.mesh.builder
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A builder that make chuncks for MapVoxelMesh.
  /// </summary>
  public interface IChunckBuilder : IVoxelMeshBuilder
  {
    /// <summary>
    ///   Get chunck width.
    /// </summary>
    int Width {
      get;
    }

    /// <summary>
    ///   Get chunck height.
    /// </summary>
    int Height
    {
      get;
    }

    /// <summary>
    ///   Get chunck depth.
    /// </summary>
    int Depth
    {
      get;
    }
  }
}
