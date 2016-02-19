using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.translator;
using org.rnp.voxel.mesh;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.unity.components.translators
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
    protected MeshFilter _publisher;

    /// <summary>
    ///   The mesh to translate.
    /// </summary>
    [SerializeField]
    protected VoxelMesh _voxelMesh;

    /// <summary>
    ///   Where we need to publish the result of the translation.
    /// </summary>
    public MeshFilter Publisher
    {
      get
      {
        return _publisher;
      }
      set
      {
        if (this._publisher != null)
        {
          this._publisher.sharedMesh = null;
        }
        this._publisher = value;
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
        return this._voxelMesh;
      }
      set
      {
        this._voxelMesh = value;
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
    public virtual void Translate()
    {
      this.Clear();

      if (this.VoxelMesh == null || this.VoxelMesh.Mesh == null)
      {
        return;
      }

      IVoxelMesh mesh = this.VoxelMesh.Mesh;
      this.TranslateMesh(VoxelLocation.Zero, mesh);
    }

    public void TranslateMesh(VoxelLocation location, IVoxelMesh mesh)
    {
      VoxelLocation start = mesh.Start;
      VoxelLocation end = mesh.End;
      VoxelLocation finalLocation = new VoxelLocation();

      for (int x = start.X; x < end.X; ++x)
      {
        for (int y = start.Y; y < end.Y; ++y)
        {
          for (int z = start.Z; z < end.Z; ++z)
          {
            finalLocation.Set(location).Add(x,y,z);
            this.Translate(finalLocation, mesh, finalLocation);
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
    protected void Translate(VoxelLocation finalLocation, IVoxelMesh mesh, VoxelLocation voxelLocation)
    {
      if (!mesh.IsEmpty(voxelLocation))
      {
        if (mesh.IsEmpty(voxelLocation.X, voxelLocation.Y + 1, voxelLocation.Z))
        {
          this.TranslateUp(finalLocation, mesh[voxelLocation]);
        }
        if (mesh.IsEmpty(voxelLocation.X, voxelLocation.Y - 1, voxelLocation.Z))
        {
          this.TranslateDown(finalLocation, mesh[voxelLocation]);
        }
        if (mesh.IsEmpty(voxelLocation.X - 1, voxelLocation.Y, voxelLocation.Z))
        {
          this.TranslateLeft(finalLocation, mesh[voxelLocation]);
        }
        if (mesh.IsEmpty(voxelLocation.X + 1, voxelLocation.Y, voxelLocation.Z))
        {
          this.TranslateRight(finalLocation, mesh[voxelLocation]);
        }
        if (mesh.IsEmpty(voxelLocation.X, voxelLocation.Y, voxelLocation.Z + 1))
        {
          this.TranslateFront(finalLocation, mesh[voxelLocation]);
        }
        if (mesh.IsEmpty(voxelLocation.X, voxelLocation.Y, voxelLocation.Z - 1))
        {
          this.TranslateBack(finalLocation, mesh[voxelLocation]);
        }
      }
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
    protected Vector3 GetVector(VoxelLocation finalLocation, int dx, int dy, int dz)
    {
      return new Vector3(
        (float) (finalLocation.X + dx),
        (float) (finalLocation.Y + dy),
        (float) (finalLocation.Z + dz)
      );
    }

    /// <summary>
    ///  Create a top face. (y + 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateUp(VoxelLocation finalLocation, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(finalLocation, 0, 1, 1),
        this.GetVector(finalLocation, 1, 1, 1),
        this.GetVector(finalLocation, 1, 1, 0),
        this.GetVector(finalLocation, 0, 1, 0)
      }, color);
    }

    /// <summary>
    ///  Create a down face. (y - 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateDown(VoxelLocation finalLocation, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(finalLocation, 0, 0, 0),
        this.GetVector(finalLocation, 1, 0, 0),
        this.GetVector(finalLocation, 1, 0, 1),
        this.GetVector(finalLocation, 0, 0, 1)
      }, color);
    }

    /// <summary>
    ///  Create a left face. (x + 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateLeft(VoxelLocation finalLocation, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(finalLocation, 0, 0, 1),
        this.GetVector(finalLocation, 0, 1, 1),
        this.GetVector(finalLocation, 0, 1, 0),
        this.GetVector(finalLocation, 0, 0, 0)
      }, color);
    }

    /// <summary>
    ///  Create a right face. (x - 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateRight(VoxelLocation finalLocation, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(finalLocation, 1, 0, 0),
        this.GetVector(finalLocation, 1, 1, 0),
        this.GetVector(finalLocation, 1, 1, 1),
        this.GetVector(finalLocation, 1, 0, 1)
      }, color);
    }

    /// <summary>
    ///  Create a back face. (z - 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateBack(VoxelLocation finalLocation, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(finalLocation, 0, 0, 0),
        this.GetVector(finalLocation, 0, 1, 0),
        this.GetVector(finalLocation, 1, 1, 0),
        this.GetVector(finalLocation, 1, 0, 0)
      }, color);
    }

    /// <summary>
    ///  Create a front face. (z + 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void TranslateFront(VoxelLocation finalLocation, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        this.GetVector(finalLocation, 1, 0, 1),
        this.GetVector(finalLocation, 1, 1, 1),
        this.GetVector(finalLocation, 0, 1, 1),
        this.GetVector(finalLocation, 0, 0, 1)
      }, color);
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
