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
  ///   A voxel location in space.
  ///   
  ///   That object is used in voxel mesh in order to identify a 3D location.
  ///   In place of a common vector object, a voxel location use int coordinates.
  /// 
  ///   Voxel location can be also used with hashtable.
  ///   
  ///   A voxel location is immutable.
  /// </summary>
  /// 
  /// <see cref="org.rnp.voxel.utils.VoxelLocation"/>
  [System.Serializable]
  public sealed class VoxelLocation : ICopiable<VoxelLocation>
  {
    #region Fields
    [SerializeField]
    private int _x;

    [SerializeField]
    private int _y;

    [SerializeField]
    private int _z;

    public static readonly VoxelLocation Zero = new VoxelLocation();
    public static readonly VoxelLocation Up = new VoxelLocation(Vector3.up);
    public static readonly VoxelLocation Down = new VoxelLocation(Vector3.down);
    public static readonly VoxelLocation Left = new VoxelLocation(Vector3.left);
    public static readonly VoxelLocation Right = new VoxelLocation(Vector3.right);
    public static readonly VoxelLocation Forward = new VoxelLocation(Vector3.forward);
    public static readonly VoxelLocation Back = new VoxelLocation(Vector3.back);
    #endregion

    #region Getters & Setters
    public int X
    {
      get { return this._x; }
    }

    public int Y
    {
      get { return this._y; }
    }

    public int Z
    {
      get { return this._z; }
    }
    #endregion

    #region Constructors
    /// <summary>
    ///   A zero location.
    /// </summary>
    public VoxelLocation()
    {
      this._x = this._y = this._z = 0;
    }

    /// <summary>
    ///   A new voxel location.
    /// </summary>
    /// <param name="pos"></param>
    public VoxelLocation(int pos)
    {
      this._x = this._y = this._z = pos;
    }

    /// <summary>
    ///   A new voxel location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public VoxelLocation(int x, int y, int z)
    {
      this._x = x;
      this._y = y;
      this._z = z;
    }

    /// <summary>
    ///   A new voxel location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation(float x, float y, float z)
    {
      this._x = Mathf.FloorToInt(x);
      this._y = Mathf.FloorToInt(y);
      this._z = Mathf.FloorToInt(z);
    }

    /// <summary>
    ///   A new voxel location.
    /// </summary>
    /// <param name="pos"></param>
    public VoxelLocation(float pos)
    {
      this._x = this._y = this._z = Mathf.FloorToInt(pos);
    }

    /// <summary>
    ///   Copy an existing VoxelLocation.
    /// </summary>
    /// <param name="toCopy"></param>
    public VoxelLocation(VoxelLocation toCopy)
    {
      this._x = toCopy._x;
      this._y = toCopy._y;
      this._z = toCopy._z;
    }

    /// <summary>
    ///   Copy an existing Vector3 location.
    /// </summary>
    /// <param name="vector"></param>
    public VoxelLocation(Vector3 vector)
    {
      this._x = Mathf.FloorToInt(vector.x);
      this._y = Mathf.FloorToInt(vector.y);
      this._z = Mathf.FloorToInt(vector.z);
    }

    /// <summary>
    ///   Copy an existing Vector2 location.
    /// </summary>
    /// <param name="vector"></param>
    public VoxelLocation(Vector2 vector)
    {
      this._x = Mathf.FloorToInt(vector.x);
      this._y = Mathf.FloorToInt(vector.y);
      this._z = 0;
    }
    #endregion
    
    #region Addition
    /// <summary>
    ///   Add another location fields to this location.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public VoxelLocation Add(VoxelLocation location)
    {
      return new VoxelLocation(
        this._x + location._x,
        this._y + location._y,
        this._z + location._z
      );
    }

    /// <summary>
    ///   Translate this location by a (x, y, z) vector.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation Add(float x, float y, float z)
    {
      return new VoxelLocation(
        this._x + x,
        this._y + y,
        this._z + z
      );
    }

    /// <summary>
    ///   Translate this location by a (x, y, z) vector.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation Add(int x, int y, int z)
    {
      return new VoxelLocation(
        this._x + x,
        this._y + y,
        this._z + z
      );
    }

    /// <summary>
    ///   Translate this location by a (x, y, z) vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Add(Vector3 vector)
    {
      return new VoxelLocation(
        this._x + vector.x,
        this._y + vector.y,
        this._z + vector.z
      );
    }

    /// <summary>
    ///   Translate this location by a (x, y) vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Add(Vector2 vector)
    {
      return new VoxelLocation(
        this._x + vector.x,
        this._y + vector.y,
        this._z
      );
    }

    /// <summary>
    ///   Add a dimension object.
    /// </summary>
    /// <param name="dimensions"></param>
    /// <returns></returns>
    public VoxelLocation Add(Dimensions3D dimensions)
    {
      return new VoxelLocation(
        this._x + dimensions.Width,
        this._y + dimensions.Height,
        this._z + dimensions.Depth
      );
    }
    #endregion

    #region Substraction
    /// <summary>
    ///   Substract another location fields to this location.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public VoxelLocation Sub(VoxelLocation location)
    {
      return new VoxelLocation(
        this._x - location._x,
        this._y - location._y,
        this._z - location._z
      );
    }

    /// <summary>
    ///   Translate this location by a (-x, -y, -z) vector.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation Sub(float x, float y, float z)
    {
      return new VoxelLocation(
        this._x - x,
        this._y - y,
        this._z - z
      );
    }

    /// <summary>
    ///   Translate this location by a (-x, -y, -z) vector.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation Sub(int x, int y, int z)
    {
      return new VoxelLocation(
        this._x - x,
        this._y - y,
        this._z - z
      );
    }

    /// <summary>
    ///   Translate this location by a (-x, -y, -z) vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Sub(Vector3 vector)
    {
      return new VoxelLocation(
        this._x - vector.x,
        this._y - vector.y,
        this._z - vector.z
      );
    }

    /// <summary>
    ///   Translate this location by a (-x, -y) vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Sub(Vector2 vector)
    {
      return new VoxelLocation(
        this._x - vector.x,
        this._y - vector.y,
        this._z
      );
    }
    
    /// <summary>
    ///   Subtract a dimension object.
    /// </summary>
    /// <param name="dimensions"></param>
    /// <returns></returns>
    public VoxelLocation Sub(Dimensions3D dimensions)
    {
      return new VoxelLocation(
        this._x - dimensions.Width,
        this._y - dimensions.Height,
        this._z - dimensions.Depth
      );
    }
    #endregion

    #region Multiplication
    /// <summary>
    ///   Multiply each coordinates by a specific factor.
    /// </summary>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    public VoxelLocation Mul(float sx, float sy, float sz)
    {
      return new VoxelLocation(
        this._x * sx,
        this._y * sy,
        this._z * sz
      );
    }

    /// <summary>
    ///   Multiply each coordinates by a specific factor.
    /// </summary>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    public VoxelLocation Mul(int sx, int sy, int sz)
    {
      return new VoxelLocation(
        this._x * sx,
        this._y * sy,
        this._z * sz
      );
    }

    /// <summary>
    ///   Multiply each coordinates by a specific factor.
    /// </summary>
    /// <param name="dimensions"></param>
    /// <returns></returns>
    public VoxelLocation Mul(Dimensions3D dimensions)
    {
      return new VoxelLocation(
        this._x * dimensions.Width,
        this._y * dimensions.Height,
        this._z * dimensions.Depth
      );
    }

    /// <summary>
    ///   Multiply each coordinates by a factor.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelLocation Mul(float s)
    {
      return new VoxelLocation(
        this._x * s,
        this._y * s,
        this._z * s
      );
    }

    /// <summary>
    ///   Multiply each coordinates by a factor.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelLocation Mul(int s)
    {
      return new VoxelLocation(
        this._x * s,
        this._y * s,
        this._z * s
      );
    }
    #endregion

    #region Division
    /// <summary>
    ///   Divide each coordinates by a specific factor.
    /// </summary>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    public VoxelLocation Div(float sx, float sy, float sz)
    {
      return new VoxelLocation(
        this._x / sx,
        this._y / sy,
        this._z / sz
      );
    }

    /// <summary>
    ///   Divide each coordinates by a specific factor.
    /// </summary>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    public VoxelLocation Div(int sx, int sy, int sz)
    {
      return new VoxelLocation(
        this._x / sx,
        this._y / sy,
        this._z / sz
      );
    }

    /// <summary>
    ///   Divide each coordinates by a specific factor.
    /// </summary>
    /// <param name="dimensions"></param>
    /// <returns></returns>
    public VoxelLocation Div(Dimensions3D dimensions)
    {
      return new VoxelLocation(
        this._x / dimensions.Width,
        this._y / dimensions.Height,
        this._z / dimensions.Depth
      );
    }

    /// <summary>
    ///   Divide each coordinates by a factor.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelLocation Div(float s)
    {
      return new VoxelLocation(
        this._x / s,
        this._y / s,
        this._z / s
      );
    }

    /// <summary>
    ///   Divide each coordinates by a factor.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelLocation Div(int s)
    {
      return new VoxelLocation(
        this._x / s,
        this._y / s,
        this._z / s
      );
    }
    #endregion

    #region Modulo
    /// <summary>
    ///   Apply a modulo operation on each coordinates.
    /// </summary>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    public VoxelLocation Mod(int sx, int sy, int sz)
    {
      return new VoxelLocation(
        this._x % sx,
        this._y % sy,
        this._z % sz
      );
    }

    /// <summary>
    ///   Apply a modulo operation on each coordinates.
    /// </summary>
    /// <param name="dimensions"></param>
    /// <returns></returns>
    public VoxelLocation Mod(Dimensions3D dimensions)
    {
      return new VoxelLocation(
        this._x % dimensions.Width,
        this._y % dimensions.Height,
        this._z % dimensions.Depth
      );
    }

    /// <summary>
    ///   Apply a modulo operation on each coordinates.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelLocation Mod(int s)
    {
      return new VoxelLocation(
        this._x % s,
        this._y % s,
        this._z % s
      );
    }
    #endregion

    #region Methods
    /// <summary>
    ///   Return true if at less one coordinates of both locations are the same.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool AnyEquals(VoxelLocation other)
    {
      return this._x == other._x || this._y == other._y || this._z == other._z;
    }

    /// <summary>
    ///   Change a coordinate if the new value is lower than the old value.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation SetIfMin(int x, int y, int z)
    {
      return new VoxelLocation(
        (this._x < x) ? this._x : x,
        (this._y < y) ? this._y : y,
        (this._z < z) ? this._z : z
      );
    }

    /// <summary>
    ///   Change a coordinate if the new value is greater than the old value.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation SetIfMax(int x, int y, int z)
    {
      return new VoxelLocation(
        (this._x > x) ? this._x : x,
        (this._y > y) ? this._y : y,
        (this._z > z) ? this._z : z
      );
    }
    
    /// <summary>
    ///   Change a coordinate if the new value is lower than the old value.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelLocation SetIfMin(VoxelLocation other)
    {
      return new VoxelLocation(
        (this._x < other._x) ? this._x : other._x,
        (this._y < other._y) ? this._y : other._y,
        (this._z < other._z) ? this._z : other._z
      );
    }

    /// <summary>
    ///   Change a coordinate if the new value is greater than the old value.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelLocation SetIfMax(VoxelLocation other)
    {
      return new VoxelLocation(
        (this._x > other._x) ? this._x : other._x,
        (this._y > other._y) ? this._y : other._y,
        (this._z > other._z) ? this._z : other._z
      );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float SquaredLength()
    {
      return this._x * this._x + this._y * this._y + this._z * this._z;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float Length()
    {
      return Mathf.Sqrt(this._x * this._x + this._y * this._y + this._z * this._z);
    }
    #endregion

    #region Base objects methods
    /// <see cref="object"/>
    public override string ToString()
    {
      return "VoxelLocation (" + this._x + ", " + this._y + ", " + this._z + ")";
    }

    /// <see cref="object"/>
    public override int GetHashCode()
    {
      return ((this._x * 31 + this._y) * 31 + this._z) * 31;
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

      if (obj is VoxelLocation)
      {
        VoxelLocation toCmp = (VoxelLocation) obj;
        return this._x == toCmp._x && this._y == toCmp._y && this._z == toCmp._z;
      }

      return false;
    }
    #endregion

    #region Operators overiding

    #region Cast
    /// <summary>
    ///   Implicit conversion from VoxelLocation to Vector3
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static implicit operator Vector3(VoxelLocation location)
    {
      return new Vector3(location._x, location._y, location._z);
    }

    /// <summary>
    ///   Implicit conversion from Vector3 to VoxelLocation;
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static implicit operator VoxelLocation(Vector3 location)
    {
      return new VoxelLocation(location);
    }

    /// <summary>
    ///   Implicit conversion from VoxelLocation to Vector2
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static implicit operator Vector2(VoxelLocation location)
    {
      return new Vector2(location._x, location._y);
    }

    /// <summary>
    ///   Implicit conversion from Vector2 to VoxelLocation;
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static implicit operator VoxelLocation(Vector2 location)
    {
      return new VoxelLocation(location);
    }
    #endregion

    #region Addition
    public static VoxelLocation operator +(VoxelLocation a, VoxelLocation b)
    {
      return a.Add(b);
    }

    public static Vector3 operator +(Vector3 a, VoxelLocation b)
    {
      return b.Add(a);
    }

    public static VoxelLocation operator +(VoxelLocation a, Vector3 b)
    {
      return a.Add(b);
    }

    public static Vector2 operator +(Vector2 a, VoxelLocation b)
    {
      return b.Add(a);
    }

    public static VoxelLocation operator +(VoxelLocation a, Vector2 b)
    {
      return a.Add(b);
    }
    #endregion

    #region Substraction
    public static VoxelLocation operator -(VoxelLocation a, VoxelLocation b)
    {
      return a.Sub(b);
    }

    public static Vector3 operator -(Vector3 a, VoxelLocation b)
    {
      Vector3 result = a;
      result.x -= b._x;
      result.y -= b._y;
      result.z -= b._z;
      return result;
    }

    public static VoxelLocation operator -(VoxelLocation a, Vector3 b)
    {
      return a.Sub(b);
    }

    public static Vector2 operator -(Vector2 a, VoxelLocation b)
    {
      Vector2 result = a;
      result.x -= b._x;
      result.y -= b._y;
      return result;
    }

    public static VoxelLocation operator -(VoxelLocation a, Vector2 b)
    {
      return a.Sub(b);
    }

    public static VoxelLocation operator -(VoxelLocation a)
    {
      return a.Mul(-1);
    }
    #endregion

    #region Multiplication
    public static VoxelLocation operator *(VoxelLocation a, float b)
    {
      return a.Mul(b);
    }

    public static VoxelLocation operator *(float a, VoxelLocation b)
    {
      return b.Mul(a);
    }

    public static VoxelLocation operator *(VoxelLocation a, int b)
    {
      return a.Mul(b);
    }

    public static VoxelLocation operator *(int a, VoxelLocation b)
    {
      return b.Mul(a);
    }
    #endregion

    #region Division
    public static VoxelLocation operator /(VoxelLocation a, float b)
    {
      return a.Div(b);
    }

    public static VoxelLocation operator /(VoxelLocation a, int b)
    {
      return a.Div(b);
    }
    #endregion

    #endregion

    #region ICopiable
    public VoxelLocation Copy()
    {
      return new VoxelLocation(this);
    }
    #endregion
  }
}
