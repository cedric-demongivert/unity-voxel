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
  [Translate("Marshing", typeof(VoxelMesh))]
  [ExecuteInEditMode]
  public class MarshingVoxelMeshTranslator : MarshingVoxelMeshBuilder
  {
    /// <summary>
    ///   Translation process.
    /// </summary>
    protected override void DoTranslation()
    {
      Dimensions3D dimensions = this.LocalMesh.Dimensions;
      VoxelLocation end = this.LocalMesh.Start.Add(dimensions);
      VoxelLocation start = this.LocalMesh.Start;
      
      for (int x = start.X - 1; x < end.X + 1; ++x)
      {
        for (int y = start.Y - 1; y < end.Y + 1; ++y)
        {
          for (int z = start.Z - 1; z < end.Z + 1; ++z)
          {
            this.Translate(x, y, z);
          }
        }
      }

      this.Publish();
    }
  }
}
