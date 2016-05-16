using org.rnp.voxel.mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.translator.cubic
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Transform a voxel mesh part into a cubic mesh.
  /// </summary>
  [RequireComponent(typeof(MeshFilter))]
  [ExecuteInEditMode]
  public abstract class CubicVoxelMeshBuilder : Translator
  {
    /// <summary>
    ///   Computed mesh.
    /// </summary>
    private Mesh _mesh;

    /// <summary>
    ///   Builded mesh vertices.
    /// </summary>
    private List<Vector3> _meshVertices;

    /// <summary>
    ///   Builded mesh vertices colors.
    /// </summary>
    private List<Color32> _meshVerticesColor;

    /// <summary>
    ///   Builded mesh texture coordinates.
    /// </summary>
    private List<Vector2> _meshUV;

    /// <summary>
    ///   Builded mesh triangles.
    /// </summary>
    private List<int> _meshTriangles;

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected virtual void Awake()
    {      
      this._meshTriangles = new List<int>();
      this._meshUV = new List<Vector2>();
      this._meshVertices = new List<Vector3>();
      this._meshVerticesColor = new List<Color32>();

      this._mesh = new Mesh();
      this._mesh.MarkDynamic();
      this._mesh.name = "Translated Cubic Voxel Mesh";

      this.GetComponent<MeshFilter>().sharedMesh = this._mesh;
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.GetComponent<MeshFilter>().sharedMesh = null;
      DestroyImmediate(this._mesh);
      this._mesh = null;
    }

    /// <summary>
    ///   Clear generated mesh data.
    /// </summary>
    protected void Clear()
    {
      this._meshTriangles.Clear();
      this._meshUV.Clear();
      this._meshVertices.Clear();
      this._meshVerticesColor.Clear();
    }

    /// <summary>
    ///   Publish the generated mesh.
    /// </summary>
    protected void Publish()
    {
      this._mesh.Clear();

      this._mesh.vertices = this._meshVertices.ToArray();
      this._mesh.uv = this._meshUV.ToArray();
      this._mesh.colors32 = this._meshVerticesColor.ToArray();
      this._mesh.triangles = this._meshTriangles.ToArray();
      
      this._mesh.RecalculateBounds();
      this._mesh.RecalculateNormals();
      
      this._mesh.UploadMeshData(false);
      
      this.Clear();
    }

    /// <summary>
    ///   Translate a voxel of the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void Translate(int x, int y, int z)
    {
      Color32 color;

      if (!this.MeshToTranslate.IsEmpty(x, y, z))
      {
        color = this.MeshToTranslate[x, y, z];

        if (this.MeshToTranslate.IsEmpty(x, y + 1, z))
        {
          this.TranslateUp(x, y, z, color);
        }

        if (this.MeshToTranslate.IsEmpty(x, y - 1, z))
        {
          this.TranslateDown(x, y, z, color);
        }

        if (this.MeshToTranslate.IsEmpty(x - 1, y, z))
        {
          this.TranslateLeft(x, y, z, color);
        }

        if (this.MeshToTranslate.IsEmpty(x + 1, y, z))
        {
          this.TranslateRight(x, y, z, color);
        }

        if (this.MeshToTranslate.IsEmpty(x, y, z + 1))
        {
          this.TranslateFront(x, y, z, color);
        }

        if (this.MeshToTranslate.IsEmpty(x, y, z - 1))
        {
          this.TranslateBack(x, y, z, color);
        }
      }
    }

    /// <summary>
    ///  Create a top face. (y + 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="color"></param>
    private void TranslateUp(int x, int y, int z, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        new Vector3(x + 0, y + 1, z + 1),
        new Vector3(x + 1, y + 1, z + 1),
        new Vector3(x + 1, y + 1, z + 0),
        new Vector3(x + 0, y + 1, z + 0)
      }, color);
    }

    /// <summary>
    ///  Create a down face. (y - 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="color"></param>
    private void TranslateDown(int x, int y, int z, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        new Vector3(x + 0, y + 0, z + 0),
        new Vector3(x + 1, y + 0, z + 0),
        new Vector3(x + 1, y + 0, z + 1),
        new Vector3(x + 0, y + 0, z + 1)
      }, color);
    }

    /// <summary>
    ///  Create a left face. (x + 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="color"></param>
    private void TranslateLeft(int x, int y, int z, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        new Vector3(x + 0, y + 0, z + 1),
        new Vector3(x + 0, y + 1, z + 1),
        new Vector3(x + 0, y + 1, z + 0),
        new Vector3(x + 0, y + 0, z + 0)
      }, color);
    }

    /// <summary>
    ///  Create a right face. (x - 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="color"></param>
    private void TranslateRight(int x, int y, int z, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        new Vector3(x + 1, y + 0, z + 0),
        new Vector3(x + 1, y + 1, z + 0),
        new Vector3(x + 1, y + 1, z + 1),
        new Vector3(x + 1, y + 0, z + 1)
      }, color);
    }

    /// <summary>
    ///  Create a back face. (z - 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="color"></param>
    private void TranslateBack(int x, int y, int z, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        new Vector3(x + 0, y + 0, z + 0),
        new Vector3(x + 0, y + 1, z + 0),
        new Vector3(x + 1, y + 1, z + 0),
        new Vector3(x + 1, y + 0, z + 0)
      }, color);
    }

    /// <summary>
    ///  Create a front face. (z + 1)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="color"></param>
    private void TranslateFront(int x, int y, int z, Color32 color)
    {
      this.TranslateFace(new Vector3[] {
        new Vector3(x + 1, y + 0, z + 1),
        new Vector3(x + 1, y + 1, z + 1),
        new Vector3(x + 0, y + 1, z + 1),
        new Vector3(x + 0, y + 0, z + 1)
      }, color);
    }

    /// <summary>
    ///   Create a face.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="color"></param>
    private void TranslateFace(Vector3[] vertices, Color32 color)
    {
      int indexBase = this._meshVertices.Count;
      
      /*for(int i = 0; i < vertices.Length; ++i)
      {
        vertices[i] = this.gameObject.transform.TransformPoint(vertices[i]);
      }*/

      this._meshVertices.AddRange(vertices);

      this._meshVerticesColor.AddRange(new Color32[] { 
        color, color, color, color 
      });

      this._meshUV.AddRange(new Vector2[] {
        Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero
      });

      this._meshTriangles.AddRange(new int[] {
        indexBase, indexBase + 1, indexBase + 2,
        indexBase + 2, indexBase + 3, indexBase
      });
    }
  }
}
