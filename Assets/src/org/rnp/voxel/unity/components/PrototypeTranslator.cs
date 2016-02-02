﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.translator;
using org.rnp.voxel.mesh;
using UnityEngine;

namespace org.rnp.voxel.unity.components
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A simple protoypic translator. Do not use in final games.
  /// </summary>
  [ExecuteInEditMode]
  public class PrototypeTranslator : MonoBehaviour, ITranslator
  {
    /// <summary>
    ///   Where we need to publish the result of the translation.
    /// </summary>
    [SerializeField]
    protected MeshFilter _Publisher;

    /// <summary>
    ///   The mesh to translate.
    /// </summary>
    [SerializeField]
    protected VoxelMesh _VoxelMesh;

    /// <summary>
    ///   Where we need to publish the result of the translation.
    /// </summary>
    public MeshFilter Publisher
    {
      get
      {
        return _Publisher;
      }
      set
      {
        if (this._Publisher != null)
        {
          this._Publisher.sharedMesh = null;
        }
        this._Publisher = value;
        this.Publish();
      }
    }

    /// <summary>
    ///   The mesh to translate.
    /// </summary>
    public VoxelMesh VoxelMesh
    {
      get
      {
        return this._VoxelMesh;
      }
      set
      {
        this._VoxelMesh = value;
        this.Translate();
        this.Publish();
      }
    }

    /// <summary>
    ///   Builded mesh vertices.
    /// </summary>
    protected List<Vector3> meshVertices;

    /// <summary>
    ///   Builded mesh vertices colors.
    /// </summary>
    protected List<Color32> meshVerticesColor;

    /// <summary>
    ///   Builded mesh texture coordinates.
    /// </summary>
    protected List<Vector2> meshUV;

    /// <summary>
    ///   Builded mesh triangles.
    /// </summary>
    protected List<int> meshTriangles;

    /// <see cref="org.rnp.voxel.translator.ITranslator"/>
    public IVoxelMesh Mesh
    {
      get
      {
        return VoxelMesh.Mesh;
      }
      set
      {
        VoxelMesh.Mesh = value;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    void Start()
    {
      this.meshTriangles = new List<int>();
      this.meshUV = new List<Vector2>();
      this.meshVertices = new List<Vector3>();
      this.meshVerticesColor = new List<Color32>();

      Translate();
      Publish();
    }

    /// <summary>
    ///   Clear generated mesh data.
    /// </summary>
    protected void Clear()
    {
      this.meshTriangles.Clear();
      this.meshUV.Clear();
      this.meshVertices.Clear();
      this.meshVerticesColor.Clear();
    }

    /// <see cref="org.rnp.voxel.translator.ITranslator"/>
    public void Translate()
    {
      this.Clear();

      if (this.VoxelMesh == null || this.VoxelMesh.Mesh == null)
      {
        return;
      }

      for (int x = 0; x < this.VoxelMesh.Mesh.Width; ++x)
      {
        for (int y = 0; y < this.VoxelMesh.Mesh.Height; ++y)
        {
          for (int z = 0; z < this.VoxelMesh.Mesh.Depth; ++z)
          {
            this.Translate(x, y, z);
          }
        }
      }
    }

    /// <summary>
    ///   Translate a voxel of the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void Translate(int x, int y, int z)
    {
      if (this.AsVoxelAt(x, y, z))
      {
        if (!this.AsVoxelAt(x, y + 1, z))
        {
          this.TranslateUp(x, y, z);
        }
        if (!this.AsVoxelAt(x, y - 1, z))
        {
          this.TranslateDown(x, y, z);
        }
        if (!this.AsVoxelAt(x - 1, y, z))
        {
          this.TranslateLeft(x, y, z);
        }
        if (!this.AsVoxelAt(x + 1, y, z))
        {
          this.TranslateRight(x, y, z);
        }
        if (!this.AsVoxelAt(x, y, z + 1))
        {
          this.TranslateFront(x, y, z);
        }
        if (!this.AsVoxelAt(x, y, z - 1))
        {
          this.TranslateBack(x, y, z);
        }
      }
    }

    /// <summary>
    ///   Check if a voxel exist at a specific location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    protected bool AsVoxelAt(int x, int y, int z)
    {
      return x >= 0
          && y >= 0
          && z >= 0
          && x < this.VoxelMesh.Mesh.Width
          && y < this.VoxelMesh.Mesh.Height
          && z < this.VoxelMesh.Mesh.Depth
          && this.VoxelMesh.Mesh[x, y, z].a == 0;
    }

    /// <summary>
    ///   Get a point of a voxel cube.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="dz"></param>
    /// <returns></returns>
    protected Vector3 GetVector(int x, int y, int z, int dx, int dy, int dz)
    {
      return new Vector3(
        ((float) (x + dx)) - this.VoxelMesh.Mesh.Width / 2f,
        ((float) (y + dy)) - this.VoxelMesh.Mesh.Height / 2f,
        ((float) (z + dz)) - this.VoxelMesh.Mesh.Depth / 2f
      );
    }

    /// <summary>
    ///  Create a top face. (y + 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateUp(int x, int y, int z)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(x, y, z, 0, 1, 1),
        this.GetVector(x, y, z, 1, 1, 1),
        this.GetVector(x, y, z, 1, 1, 0),
        this.GetVector(x, y, z, 0, 1, 0)
      }, this.VoxelMesh.Mesh[x, y, z]);
    }

    /// <summary>
    ///  Create a down face. (y - 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateDown(int x, int y, int z)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(x, y, z, 0, 0, 0),
        this.GetVector(x, y, z, 1, 0, 0),
        this.GetVector(x, y, z, 1, 0, 1),
        this.GetVector(x, y, z, 0, 0, 1)
      }, this.VoxelMesh.Mesh[x, y, z]);
    }

    /// <summary>
    ///  Create a left face. (x + 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateLeft(int x, int y, int z)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(x, y, z, 0, 0, 1),
        this.GetVector(x, y, z, 0, 1, 1),
        this.GetVector(x, y, z, 0, 1, 0),
        this.GetVector(x, y, z, 0, 0, 0)
      }, this.VoxelMesh.Mesh[x, y, z]);
    }

    /// <summary>
    ///  Create a right face. (x - 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateRight(int x, int y, int z)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(x, y, z, 1, 0, 0),
        this.GetVector(x, y, z, 1, 1, 0),
        this.GetVector(x, y, z, 1, 1, 1),
        this.GetVector(x, y, z, 1, 0, 1)
      }, this.VoxelMesh.Mesh[x, y, z]);
    }

    /// <summary>
    ///  Create a back face. (z - 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateBack(int x, int y, int z)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(x, y, z, 0, 0, 0),
        this.GetVector(x, y, z, 0, 1, 0),
        this.GetVector(x, y, z, 1, 1, 0),
        this.GetVector(x, y, z, 1, 0, 0)
      }, this.VoxelMesh.Mesh[x, y, z]);
    }

    /// <summary>
    ///  Create a front face. (z + 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateFront(int x, int y, int z)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(x, y, z, 1, 0, 1),
        this.GetVector(x, y, z, 1, 1, 1),
        this.GetVector(x, y, z, 0, 1, 1),
        this.GetVector(x, y, z, 0, 0, 1)
      }, this.VoxelMesh.Mesh[x, y, z]);
    }

    /// <summary>
    ///   Create a face.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="color"></param>
    protected void TranslateFace(Vector3[] vertices, Color32 color)
    {
      int indexBase = this.meshVertices.Count;

      this.meshVertices.AddRange(vertices);
      this.meshVerticesColor.AddRange(new Color32[] { 
        color, color, color, color 
      });
      this.meshUV.AddRange(new Vector2[] {
        Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero
      });
      this.meshTriangles.AddRange(new int[] {
        indexBase, indexBase + 1, indexBase + 2,
        indexBase + 2, indexBase + 3, indexBase
      });
    }

    /// <see cref="org.rnp.voxel.translator.ITranslator"/>
    public void Publish()
    {
      if (this.Publisher == null)
      {
        return;
      }

      if (this.Publisher.sharedMesh == null || !this.Publisher.sharedMesh.isReadable)
      {
        this.Publisher.sharedMesh = new Mesh();
        this.Publisher.sharedMesh.name = "Voxel Translated Mesh";
      }

      this.Publisher.sharedMesh.Clear();

      this.Publisher.sharedMesh.vertices = this.meshVertices.ToArray();
      this.Publisher.sharedMesh.uv = this.meshUV.ToArray();
      this.Publisher.sharedMesh.colors32 = this.meshVerticesColor.ToArray();
      this.Publisher.sharedMesh.triangles = this.meshTriangles.ToArray();

      this.Publisher.sharedMesh.UploadMeshData(true);
    }
  }
}