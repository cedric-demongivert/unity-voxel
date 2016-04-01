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
  public sealed class Dimensions3D
  {
    #region Constants
    public static readonly Dimensions3D Empty = new Dimensions3D();
    #endregion

    #region Fields
    public readonly int Width;
    public readonly int Height;
    public readonly int Depth;
    private int v;
    #endregion

    #region Constructors
    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    public Dimensions3D()
    {
      this.Width = this.Height = this.Depth = 0;
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="size"></param>
    public Dimensions3D(int size)
    {
      this.AssertIsValidDimensions(size, size, size);
      this.Width = this.Height = this.Depth = size;
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
      this.Width = width;
      this.Height = height;
      this.Depth = depth;
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
      this.Width = Mathf.CeilToInt(width);
      this.Height = Mathf.CeilToInt(height);
      this.Depth = Mathf.CeilToInt(depth);
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="size"></param>
    public Dimensions3D(float size)
    {
      this.AssertIsValidDimensions(size, size, size);
      this.Width = this.Height = this.Depth = Mathf.CeilToInt(size);
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="dimensions"></param>
    public Dimensions3D(Vector3 dimensions)
    {
      this.AssertIsValidDimensions(dimensions.x, dimensions.y, dimensions.z);
      this.Width = Mathf.CeilToInt(dimensions.x);
      this.Height = Mathf.CeilToInt(dimensions.y);
      this.Depth = Mathf.CeilToInt(dimensions.z);
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="dimensions"></param>
    public Dimensions3D(VoxelLocation dimensions)
    {
      this.AssertIsValidDimensions(dimensions.X, dimensions.Y, dimensions.Z);
      this.Width = dimensions.X;
      this.Height = dimensions.Y;
      this.Depth = dimensions.Z;
    }

    /// <summary>
    ///   Create a custom Dimensions3D.
    /// </summary>
    /// <param name="dimensions"></param>
    public Dimensions3D(VoxelLocation start, VoxelLocation end)
    {
      this.Width = Mathf.Abs(end.X - start.X);
      this.Height = Mathf.Abs(end.Y - start.Y);
      this.Depth = Mathf.Abs(end.Z - start.Z);
    }

    /// <summary>
    ///   Create a copy of an existing Dimensions3D.
    /// </summary>
    /// <param name="toCopy"></param>
    public Dimensions3D(Dimensions3D toCopy)
    {
      this.AssertIsValidDimensions(toCopy.Width, toCopy.Height, toCopy.Depth);
      this.Width = toCopy.Width;
      this.Height = toCopy.Height;
      this.Depth = toCopy.Depth;
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
        this.Width + toAdd.x,
        this.Height + toAdd.y,
        this.Depth + toAdd.z
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
        this.Width + other.Width,
        this.Height + other.Height,
        this.Depth + other.Depth
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
        this.Width + other.X,
        this.Height + other.Y,
        this.Depth + other.Z
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
        this.Width + width,
        this.Height + height,
        this.Depth + depth
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
        this.Width - toSub.x,
        this.Height - toSub.y,
        this.Depth - toSub.z
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
        this.Width - other.Width,
        this.Height - other.Height,
        this.Depth - other.Depth
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
        this.Width - other.X,
        this.Height - other.Y,
        this.Depth - other.Z
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
        this.Width - width,
        this.Height - height,
        this.Depth - depth
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
        this.Width * sx,
        this.Height * sy,
        this.Depth * sz
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
        this.Width * sx,
        this.Height * sy,
        this.Depth * sz
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
        this.Width * s,
        this.Height * s,
        this.Depth * s
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
        this.Width * s,
        this.Height * s,
        this.Depth * s
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
        this.Width / sx,
        this.Height / sy,
        this.Depth / sz
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
        this.Width / sx,
        this.Height / sy,
        this.Depth / sz
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
        this.Width / s,
        this.Height / s,
        this.Depth / s
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
        this.Width / s,
        this.Height / s,
        this.Depth / s
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
      return this.Width == 0 || this.Height == 0 || this.Depth == 0;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
      return this.Width == 0 && this.Height == 0 && this.Depth == 0;
    }
    #endregion

    #region Base objects methods
    /// <see cref="object"/>
    public override string ToString()
    {
      return "Dimensions3D (" + this.Width + ", " + this.Height + ", " + this.Depth + ")";
    }

    /// <see cref="object"/>
    public override int GetHashCode()
    {
      return ((this.Width * 31 + this.Height) * 31 + this.Depth) * 31;
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
        return this.Width == toCmp.Width && this.Height == toCmp.Height && this.Depth == toCmp.Depth;
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
