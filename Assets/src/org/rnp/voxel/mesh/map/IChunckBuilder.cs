using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh.builder;

namespace org.rnp.voxel.mesh.map
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A builder that make chuncks for a MapVoxelMesh.
  /// </summary>
  public interface IChunckBuilder : IVoxelMeshBuilder
  {
    /// <summary>
    ///   Get chunck dimensions.
    /// </summary>
    Dimensions3D ChunckDimensions
    {
      get;
    }
  }
}
