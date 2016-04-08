using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.translator;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.translator.cubic
{
  [Translate("Cubes", typeof(VoxelMesh))]
  [ExecuteInEditMode]
  public class CubicVoxelMeshTranslator : CubicVoxelMeshBuilder
  {
    /// <summary>
    ///   Translation process.
    /// </summary>
    protected override void DoTranslation()
    {
      Dimensions3D dimensions = this.LocalMesh.Dimensions;
      
      VoxelLocation end = this.LocalMeshGlobalLocation.Add(dimensions);
      VoxelLocation start = this.LocalMeshGlobalLocation;

      for (int x = start.X; x < end.X; ++x)
      {
        for (int y = start.Y; y < end.Y; ++y)
        {
          for (int z = start.Z; z < end.Z; ++z)
          {
            this.Translate(x, y, z);
          }
        }
      }

      this.Publish();
    }
  }
}
