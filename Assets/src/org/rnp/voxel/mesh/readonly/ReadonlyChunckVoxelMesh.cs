using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEngine;
using org.rnp.voxel.mesh.exceptions;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A infinite voxel mesh based on a map, not writable.
  /// </summary>
  public sealed class ReadonlyChunckVoxelMesh : ChunckVoxelMesh, IVoxelMeshCommitListener
  {
    private ChunckVoxelMesh _writableMesh;
    
    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override bool IsReadonly
    {
      get { return false; }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override Dimensions3D Dimensions
    {
      get { return this._writableMesh.Dimensions; }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override VoxelLocation Start
    {
      get { return this._writableMesh.Start; }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override bool IsDirty
    {
      get
      {
        return this._writableMesh.IsDirty;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.map.ChunckVoxelMesh"/>
    public override Dimensions3D ChunckDimensions
    {
      get
      {
        return this._writableMesh.ChunckDimensions;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.map.ChunckVoxelMesh"/>
    public override IEnumerable<VoxelLocation> ChunckLocations
    {
      get
      {
        foreach(VoxelLocation location in this._writableMesh.ChunckLocations) {
          yield return location;
        }
      }
    }

    /// <summary>
    ///   Wrap in a readonly implementation, a writable voxel map implementation.
    /// </summary>
    /// <param name="writableMap"></param>
    public ReadonlyChunckVoxelMesh(ChunckVoxelMesh writableMap)
    {
      this._writableMesh = writableMap;
      this._writableMesh.RegisterCommitListener(this);
    }

    /// <summary>
    ///   Copy another ReadonlyChunckVoxelMesh object.
    /// </summary>
    /// <param name="toCopy"></param>
    public ReadonlyChunckVoxelMesh(ReadonlyChunckVoxelMesh toCopy)
    {
      this._writableMesh = toCopy._writableMesh;
      this._writableMesh.RegisterCommitListener(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override VoxelMesh Copy()
    {
      return new ReadonlyChunckVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override VoxelMesh Readonly()
    {
      return this;
    }

    /// <see cref="org.rnp.voxel.mesh.map.ChunckVoxelMesh"/>
    public override VoxelMesh GetChunck(int x, int y, int z)
    {
      return this._writableMesh.GetChunck(x, y, z);
    }

    /// <see cref="org.rnp.voxel.mesh.map.ChunckVoxelMesh"/>
    public override VoxelMesh GetChunck(VoxelLocation location)
    {
      return this._writableMesh.GetChunck(location);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override Color32 Get(int x, int y, int z)
    {
      return this._writableMesh.Get(x, y, z);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override Color32 Get(VoxelLocation location)
    {
      return this._writableMesh.Get(location);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Set(int x, int y, int z, Color32 value)
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Clear()
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Set(VoxelLocation location, Color32 value)
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void MarkDirty()
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/ScriptableObject.html"/>
    public override void Destroy()
    {
      base.Destroy();
      this._writableMesh.UnregisterCommitListener(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Commit()
    {
      this._writableMesh.Commit();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnRegister(VoxelMesh mesh)
    { }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnCommitBegin(VoxelMesh mesh)
    {
      foreach (IVoxelMeshCommitListener listener in this.Listeners)
      {
        listener.OnCommitBegin(mesh);
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnCommitEnd(VoxelMesh mesh)
    {
      foreach (IVoxelMeshCommitListener listener in this.Listeners)
      {
        listener.OnCommitEnd(mesh);
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnUnregister(VoxelMesh mesh)
    { }
  }
}
