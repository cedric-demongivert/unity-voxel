using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.utils
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A simple struct for 3D voxel cubes dimensions.
  /// </summary>
  [System.Serializable]
  public sealed class Dimensions3D
  {
    #region Constants
    public static readonly Dimensions3D Empty = new Dimensions3D();
    #endregion

    #region Fields
    [SerializeField]
    private int _width;

    [SerializeField]
    private int _height;

    [SerializeField]
    private int _depth;
    #endregion

    #region Getters & Setters
    public int Width
    {
      get { return _width; }
    }

    public int Height
    {
      get { return _height; }
    }

    public int Depth
    {
      get { return _depth; }
    }
    #endregion

    #region Constructors
    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    public Dimensions3D()
    {
      this._width = this._height = this._depth = 0;
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="size"></param>
    public Dimensions3D(int size)
    {
      this.AssertIsValidDimensions(size, size, size);
      this._width = this._height = this._depth = size;
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public Dimensions3D(int width, int height, int depth)
    {
      this.AssertIsValidDimensions(width, height, depth);
      this._width = width;
      this._height = height;
      this._depth = depth;
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public Dimensions3D(float width, float height, float depth)
    {
      this.AssertIsValidDimensions(width, height, depth);
      this._width = Mathf.CeilToInt(width);
      this._height = Mathf.CeilToInt(height);
      this._depth = Mathf.CeilToInt(depth);
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="size"></param>
    public Dimensions3D(float size)
    {
      this.AssertIsValidDimensions(size, size, size);
      this._width = this._height = this._depth = Mathf.CeilToInt(size);
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="dimensions"></param>
    public Dimensions3D(Vector3 dimensions)
    {
      this.AssertIsValidDimensions(dimensions.x, dimensions.y, dimensions.z);
      this._width = Mathf.CeilToInt(dimensions.x);
      this._height = Mathf.CeilToInt(dimensions.y);
      this._depth = Mathf.CeilToInt(dimensions.z);
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="dimensions"></param>
    public Dimensions3D(VoxelLocation dimensions)
    {
      this.AssertIsValidDimensions(dimensions.X, dimensions.Y, dimensions.Z);
      this._width = dimensions.X;
      this._height = dimensions.Y;
      this._depth = dimensions.Z;
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="dimensions"></param>
    public Dimensions3D(VoxelLocation start, VoxelLocation end)
    {
      this._width = Mathf.Abs(end.X - start.X);
      this._height = Mathf.Abs(end.Y - start.Y);
      this._depth = Mathf.Abs(end.Z - start.Z);
    }

    /// <summary>
    ///   Create a copy of an existing Dimensions3D.
    /// </summary>
    /// <param name="toCopy"></param>
    public Dimensions3D(Dimensions3D toCopy)
    {
      this.AssertIsValidDimensions(toCopy._width, toCopy._height, toCopy._depth);
      this._width = toCopy._width;
      this._height = toCopy._height;
      this._depth = toCopy._depth;
    }
    #endregion

    #region Assertions
    /// <summary>
    ///   Check if all parameters are positive.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    private void AssertIsValidDimensions(int width, int height, int depth)
    {
      if(width < 0 || height < 0 || depth < 0) {
        throw new ArgumentException("A dimension attribute can't be negative (width : " + width + ", height : " + height + ", depth : " + depth + ")");
      }
    }

    /// <summary>
    ///   Check if all parameters are positive.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    private void AssertIsValidDimensions(float width, float height, float depth)
    {
      if(width < 0 || height < 0 || depth < 0) {
        throw new ArgumentException("A dimension attribute can't be negative (width : " + width + ", height : " + height + ", depth : " + depth + ")");
      }
    }
    #endregion

    #region Addition
    /// <summary>
    ///   Addition
    /// </summary>
    /// <param name="toAdd"></param>
    /// <returns></returns>
    public Dimensions3D Add(Vector3 toAdd)
    {
      return new Dimensions3D(
        this._width + toAdd.x,
        this._height + toAdd.y,
        this._depth + toAdd.z
      );
    }

    /// <summary>
    ///   Addition
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Dimensions3D Add(Dimensions3D other)
    {
      return new Dimensions3D(
        this._width + other._width,
        this._height + other._height,
        this._depth + other._depth
      );
    }

    /// <summary>
    ///   Addition
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Dimensions3D Add(VoxelLocation other)
    {
      return new Dimensions3D(
        this._width + other.X,
        this._height + other.Y,
        this._depth + other.Z
      );
    }

    /// <summary>
    ///   Addition
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public Dimensions3D Add(int width, int height, int depth)
    {
      return new Dimensions3D(
        this._width + width,
        this._height + height,
        this._depth + depth
      );
    }
    #endregion
    
    #region Substraction
    /// <summary>
    ///   Subtraction
    /// </summary>
    /// <param name="toSub"></param>
    /// <returns></returns>
    public Dimensions3D Sub(Vector3 toSub)
    {
      return new Dimensions3D(
        this._width - toSub.x,
        this._height - toSub.y,
        this._depth - toSub.z
      );
    }

    /// <summary>
    ///   Subtraction
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Dimensions3D Sub(Dimensions3D other)
    {
      return new Dimensions3D(
        this._width - other._width,
        this._height - other._height,
        this._depth - other._depth
      );
    }

    /// <summary>
    ///   Subtraction
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Dimensions3D Sub(VoxelLocation other)
    {
      return new Dimensions3D(
        this._width - other.X,
        this._height - other.Y,
        this._depth - other.Z
      );
    }

    /// <summary>
    ///   Subtraction
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public Dimensions3D Sub(int width, int height, int depth)
    {
      return new Dimensions3D(
        this._width - width,
        this._height - height,
        this._depth - depth
      );
    }
    #endregion
    
    #region Multiplication
    /// <summary>
    ///   Multiplication
    /// </summary>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    public Dimensions3D Mul(int sx, int sy, int sz)
    {
      return new Dimensions3D(
        this._width * sx,
        this._height * sy,
        this._depth * sz
      );
    }

    /// <summary>
    ///   Multiplication
    /// </summary>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    public Dimensions3D Mul(float sx, float sy, float sz)
    {
      return new Dimensions3D(
        this._width * sx,
        this._height * sy,
        this._depth * sz
      );
    }

    /// <summary>
    ///   Multiplication
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public Dimensions3D Mul(float s)
    {
      return new Dimensions3D(
        this._width * s,
        this._height * s,
        this._depth * s
      );
    }

    /// <summary>
    ///   Multiplication
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public Dimensions3D Mul(int s)
    {
      return new Dimensions3D(
        this._width * s,
        this._height * s,
        this._depth * s
      );
    }
    #endregion

    #region Division
    /// <summary>
    ///   Division
    /// </summary>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    public Dimensions3D Div(int sx, int sy, int sz)
    {
      return new Dimensions3D(
        this._width / sx,
        this._height / sy,
        this._depth / sz
      );
    }

    /// <summary>
    ///   Division
    /// </summary>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    public Dimensions3D Div(float sx, float sy, float sz)
    {
      return new Dimensions3D(
        this._width / sx,
        this._height / sy,
        this._depth / sz
      );
    }

    /// <summary>
    ///   Division
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public Dimensions3D Div(float s)
    {
      return new Dimensions3D(
        this._width / s,
        this._height / s,
        this._depth / s
      );
    }

    /// <summary>
    ///   Division
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public Dimensions3D Div(int s)
    {
      return new Dimensions3D(
        this._width / s,
        this._height / s,
        this._depth / s
      );
    }
    #endregion
    
    #region Methods
    /// <summary>
    ///   Return true if any dimension is null.
    /// </summary>
    /// <returns></returns>
    public bool HasNull()
    {
      return this._width == 0 || this._height == 0 || this._depth == 0;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
      return this._width == 0 && this._height == 0 && this._depth == 0;
    }
    #endregion

    #region Base objects methods
    /// <see cref="object"/>
    public override string ToString()
    {
      return "Dimensions3D (" + this._width + ", " + this._height + ", " + this._depth + ")";
    }

    /// <see cref="object"/>
    public override int GetHashCode()
    {
      return ((this._width * 31 + this._height) * 31 + this._depth) * 31;
    }

    /// <see cref="object"/>
    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        return false;
      }

      if (base.Equals(obj))
      {
        return true;
      }

      if (obj is Dimensions3D)
      {
        Dimensions3D toCmp = (Dimensions3D) obj;
        return this._width == toCmp._width && this._height == toCmp._height && this._depth == toCmp._depth;
      }

      return false;
    }
    #endregion
    
    #region ICopiable
    /// <see cref="org.rnp.voxel.utils.ICopiable"></see>
    public Dimensions3D Copy()
    {
      return new Dimensions3D(this);
    }
    #endregion
  }
}
