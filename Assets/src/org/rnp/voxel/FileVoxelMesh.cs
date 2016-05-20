using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel
{
  [ExecuteInEditMode]
  public class FileVoxelMesh : VoxelMeshContainer
  {
    private VoxelMesh _mesh = new MapVoxelMesh(new Dimensions3D(8,8,8));
    
    [SerializeField]
    private TextAsset _meshFile;
    
    public TextAsset MeshFile
    {
      get
      {
        return this._meshFile;
      }
      set
      {
        if (this._meshFile == value) return;

        this._meshFile = value;
        this.Refresh();
      }
    }

    public override VoxelMesh Mesh
    {
      get
      {
        return this._mesh;
      }
    }
        
    public void Refresh()
    {
      if (this._meshFile != null)
      {
        this._mesh.Clear();
        this._mesh.Commit();
        VoxelFile.Load(_meshFile.bytes, this._mesh);
      }
      else
      {
        this._mesh.Clear();
      }

      this._mesh.Commit();
    }

    public void Awake()
    {
      this.Refresh();
    }
  }
}
