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
    
    public TextAsset MeshFile;

    public override VoxelMesh Mesh
    {
      get
      {
        return this._mesh;
      }
    }

    public void Awake()
    {
      this._mesh = VoxelFile.Load(MeshFile.bytes, this._mesh);
      this._mesh.Commit();
    }
  }
}
