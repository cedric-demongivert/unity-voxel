using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.octree;
using org.rnp.voxel.unity.components.translators;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.unity.components.translators.cubic
{
  [Translate("Cubes", typeof(IOctreeVoxelMesh))]
  [ExecuteInEditMode]
  public class CubicOctreeVoxelMeshTranslator : CubicVoxelMeshBuilder
  {
    /// <summary>
    ///   Translation process.
    /// </summary>
    protected override void DoTranslation()
    {
      IOctreeVoxelMesh mesh = this.PartToTranslate as IOctreeVoxelMesh;

      this.TranslateOctree(mesh, this.PartWorldLocation);

      this.Publish();
    }

    /// <summary>
    ///   Translate an octree node.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="worldLocation"></param>
    protected void TranslateOctree(IOctreeVoxelMesh mesh, VoxelLocation worldLocation)
    {
      for (int x = 0; x < 2; ++x)
      {
        for (int y = 0; y < 2; ++y)
        {
          for (int z = 0; z < 2; ++z)
          {
            IReadonlyVoxelMesh child = mesh.GetChild(x, y, z);

            if (child != null)
            {
              if (child is IOctreeVoxelMesh)
              {
                this.TranslateOctree(
                  child as IOctreeVoxelMesh,
                  new VoxelLocation(x, y, z).Mul(mesh.ChildDimensions).Add(worldLocation)
                );
              }
              else
              {
                this.TranslateLeaf(
                  child,
                  new VoxelLocation(x, y, z).Mul(mesh.ChildDimensions).Add(worldLocation)
                );
              }
            }
          }
        }
      }
    }

    /// <summary>
    ///   Translate a leaf node.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="worldLocation"></param>
    protected void TranslateLeaf(IVoxelMesh mesh, VoxelLocation worldLocation)
    {
      Dimensions3D dimensions = mesh.Dimensions;
      VoxelLocation end = worldLocation.Add(dimensions);
      VoxelLocation start = worldLocation;
      
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
    }
  }
}
