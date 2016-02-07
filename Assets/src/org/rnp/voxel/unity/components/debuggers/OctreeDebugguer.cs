using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.octree;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.unity.components.debuggers
{
  public sealed class OctreeDebugguer : MonoBehaviour
  {

    /// <summary>
    ///   The mesh to translate.
    /// </summary>
    [SerializeField] private VoxelMesh _voxelMesh;

    public int StartDeep;

    public int Deep;

    /// <summary>
    ///   The mesh to translate.
    /// </summary>
    public VoxelMesh VoxelMesh
    {
      get { return this._voxelMesh; }
      set { this._voxelMesh = value; }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    /// <see cref="http://docs.unity3d.com/ScriptReference/Gizmos.html"/>
    public void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.red;

      if (this.VoxelMesh == null || this.VoxelMesh.Mesh == null) return;

      if (this.VoxelMesh.Mesh is IOctreeVoxelMesh)
      {
        this.DebugTree(this.VoxelMesh.Mesh.Start, (IOctreeVoxelMesh) this.VoxelMesh.Mesh, StartDeep, StartDeep + Deep);
      }
      else
      {
        this.DebugLeaf(this.VoxelMesh.Mesh.Start, this.VoxelMesh.Mesh, StartDeep, StartDeep + Deep);
      }
    }

    /// <summary>
    ///   Debug a tree node.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="octree"></param>
    public void DebugTree(IVoxelLocation root, IOctreeVoxelMesh octree, int minDeep, int maxDeep)
    {
      if (maxDeep <= 0) return;

      if (minDeep <= 0)
      {
        this.DrawCube(root, octree);
      }

      VoxelLocation nextRoot = new VoxelLocation();
      for (int i = 0; i < 2; ++i)
      {
        for (int j = 0; j < 2; ++j)
        {
          for (int k = 0; k < 2; ++k)
          {
            IVoxelMesh child = octree.GetChild(i, j, k);
            if (child != null)
            {
              nextRoot.X = root.X + i * octree.Width / 2;
              nextRoot.Y = root.Y + j * octree.Height / 2;
              nextRoot.Z = root.Z + k * octree.Depth / 2;

              if (child is IOctreeVoxelMesh)
              {
                this.DebugTree(nextRoot, (IOctreeVoxelMesh) child, minDeep - 1, maxDeep - 1);
              }
              else
              {
                this.DebugLeaf(nextRoot, child, minDeep - 1, maxDeep - 1);
              }
            }
          }
        }
      }
    }

    /// <summary>
    ///   Debug a tree leaf.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="mesh"></param>
    public void DebugLeaf(IVoxelLocation root, IVoxelMesh mesh, int minDeep, int maxDeep)
    {
      if (maxDeep <= 0) return;

      if (minDeep <= 0)
      {
        this.DrawCube(root, mesh);
      }
    }

    /// <summary>
    ///   Draw a debug cube.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="dimensions"></param>
    private void DrawCube(IVoxelLocation root, IVoxelMesh dimensions)
    {
      Vector3 toMid = new Vector3(
        dimensions.Width / 2f,
        dimensions.Height / 2f,
        dimensions.Depth / 2f
      );

      Gizmos.DrawWireCube(
        VoxelLocation.ToVector3(root) + toMid,
        new Vector3(
          dimensions.Width,
          dimensions.Height,
          dimensions.Depth
        )  
      );
    }
  }
}
