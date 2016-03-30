using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.octree;
using org.rnp.voxel.mesh.map;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.unity.components.debuggers
{
  public sealed class MapDebugguer : OctreeDebugguer
  {
    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    /// <see cref="http://docs.unity3d.com/ScriptReference/Gizmos.html"/>
    public override void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.red;

      if (this.VoxelMesh == null || this.VoxelMesh.Mesh == null) return;

      if (this.VoxelMesh.Mesh is IMapVoxelMesh)
      {
        this.DebugMap(this.VoxelMesh.Mesh.Start, (IMapVoxelMesh)this.VoxelMesh.Mesh, StartDeep, StartDeep + Deep);
      }
      else if (this.VoxelMesh.Mesh is IOctreeVoxelMesh)
      {
        this.DebugTree(this.VoxelMesh.Mesh.Start, (IOctreeVoxelMesh) this.VoxelMesh.Mesh, StartDeep, StartDeep + Deep);
      }
      else
      {
        this.DebugLeaf(this.VoxelMesh.Mesh.Start, this.VoxelMesh.Mesh, StartDeep, StartDeep + Deep);
      }
    }

    public void DebugMap(VoxelLocation start, IMapVoxelMesh map, int startDeep, int endDeep)
    {
      this.DebugLeaf(start, map, startDeep, endDeep);

      VoxelLocation chunckStart = new VoxelLocation();
      foreach (VoxelLocation chunckLocation in map.Keys())
      {
        IVoxelMesh chunck = map.GetChild(chunckLocation);
        chunckStart.Set(chunckLocation).Mul(map.ChildWidth, map.ChildHeight, map.ChildDepth);

        if (chunck is IOctreeVoxelMesh)
        {
          this.DebugTree(chunckStart, (IOctreeVoxelMesh)chunck, startDeep - 1, endDeep - 1);
        }
        else
        {
          this.DebugLeaf(chunckStart, chunck, startDeep - 1, endDeep - 1);
        }
      }
    }
  }
}
