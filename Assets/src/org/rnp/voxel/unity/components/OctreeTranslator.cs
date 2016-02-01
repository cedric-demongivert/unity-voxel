using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.translator;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.octree;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.unity.components
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A simple protoypic translator. Do not use in final games.
  /// </summary>
  [ExecuteInEditMode]
  public class OctreeTranslator : PrototypeTranslator
  {


    /// <see cref="org.rnp.voxel.translator.ITranslator"/>
    public override void Translate()
    {
      this.Clear();

      if (this.VoxelMesh == null || this.VoxelMesh.Mesh == null)
      {
        return;
      }

      if (this.VoxelMesh.Mesh is VoxelOctree)
      {
        this.TranslateTree(this.VoxelMesh.Mesh.Start, (VoxelOctree)this.VoxelMesh.Mesh);
      }
      else
      {
        this.TranslateLeaf(this.VoxelMesh.Mesh.Start, this.VoxelMesh.Mesh);
      }
    }

    public void TranslateTree(VoxelLocation root, VoxelOctree octree)
    {
      VoxelLocation nextRoot = new VoxelLocation();
      for (int indx = 0; indx < 8; ++indx)
      {
        IVoxelMesh child = octree.GetChild(indx);
        nextRoot.X = root.X + child.Start.X;
        nextRoot.Y = root.Y + child.Start.Y;
        nextRoot.Z = root.Z + child.Start.Z;

        if (child is VoxelOctree)
        {
          this.TranslateTree(nextRoot, (VoxelOctree)child);
        }
        else
        {
          this.TranslateLeaf(nextRoot, child);
        }
      }
    }

    public void TranslateLeaf(VoxelLocation root, IVoxelMesh mesh)
    {
      for (int x = 0; x < mesh.Width; ++x)
      {
        for (int y = 0; y < mesh.Height; ++y)
        {
          for (int z = 0; z < mesh.Depth; ++z)
          {
            this.Translate(root.X + x, root.Y + y, root.Z + z);
          }
        }
      }
    }
  }
}
