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
  ///   Voxel location can be also used with hashtable (and this it's main purpose).
  /// </summary>
  /// 
  /// <see cref="org.rnp.voxel.utils.VoxelLocation"/>
  public sealed class VoxelLocation : ICopiable<VoxelLocation>
  {
    #region Fields
    public int X;
    public int Y;
    public int Z;

    public static readonly VoxelLocation Zero = new VoxelLocation();
    public static readonly VoxelLocation Up = new VoxelLocation(Vector3.up);
    public static readonly VoxelLocation Down = new VoxelLocation(Vector3.down);
    public static readonly VoxelLocation Left = new VoxelLocation(Vector3.left);
    public static readonly VoxelLocation Right = new VoxelLocation(Vector3.right);
    public static readonly VoxelLocation Forward = new VoxelLocation(Vector3.forward);
    public static readonly VoxelLocation Back = new VoxelLocation(Vector3.back);
    #endregion

    #region Constructors
    /// <summary>
    ///   A zero location.
    /// </summary>
    public VoxelLocation()
    {
      this.X = this.Y = this.Z = 0;
    }

    /// <summary>
    ///   A new voxel location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public VoxelLocation(int x, int y, int z)
    {
      this.X = this.Y = this.Z = 0;
      this.Set(x, y, z);
    }

    /// <summary>
    ///   Copy an existing VoxelLocation.
    /// </summary>
    /// <param name="toCopy"></param>
    public VoxelLocation(VoxelLocation toCopy)
    {
      this.X = this.Y = this.Z = 0;
      this.Set(toCopy);
    }

    /// <summary>
    ///   Copy an existing Vector3 location.
    /// </summary>
    /// <param name="vec"></param>
    public VoxelLocation(Vector3 vec)
    {
      this.X = this.Y = this.Z = 0;
      this.Set(vec);
    }

    /// <summary>
    ///   Copy an existing Vector2 location.
    /// </summary>
    /// <param name="vec"></param>
    public VoxelLocation(Vector2 vec)
    {
      this.X = this.Y = this.Z = 0;
      this.Set(vec);
    }
    #endregion

    #region Definition
    /// <see cref="org.rnp.utils.VoxelLocation"/>
    public VoxelLocation Set(VoxelLocation location)
    {
      this.X = location.X;
      this.Y = location.Y;
      this.Z = location.Z;
      return this;
    }

    /// <summary>
    ///   Set this location to (x, y, z)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation Set(int x, int y, int z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      return this;
    }

    /// <summary>
    ///   Set this location to (x, y, z)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation Set(float x, float y, float z)
    {
      this.X = (int) x;
      this.Y = (int) y;
      this.Z = (int) z;
      return this;
    }

    /// <summary>
    ///   Set this location to (v, v, v)
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public VoxelLocation Set(int v)
    {
      this.X = this.Y = this.Z = v;
      return this;
    }

    /// <summary>
    ///   Set this location to (v, v, v)
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public VoxelLocation Set(float v)
    {
      this.X = this.Y = this.Z = (int) v;
      return this;
    }

    /// <summary>
    ///   Set this location equal to a vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Set(Vector3 vector)
    {
      this.X = (int)vector.x;
      if (vector.x <= 0) this.X -= 1;
      this.Y = (int)vector.y;
      if (vector.y <= 0) this.Y -= 1;
      this.Z = (int)vector.z;
      if (vector.z <= 0) this.Z -= 1;
      return this;
    }

    /// <summary>
    ///   Set this location equal to a vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Set(Vector2 vector)
    {
      this.X = (int)vector.x;
      if (vector.x <= 0) this.X -= 1;
      this.Y = (int)vector.y;
      if (vector.y <= 0) this.Y -= 1;
      this.Z = 0;
      return this;
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
      this.X += location.X;
      this.Y += location.Y;
      this.Z += location.Z;
      return this;
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
      this.X += (int) x;
      this.Y += (int) y;
      this.Z += (int) z;
      return this;
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
      this.X += x;
      this.Y += y;
      this.Z += z;
      return this;
    }

    /// <summary>
    ///   Translate this location by a (x, y, z) vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Add(Vector3 vector)
    {
      this.X += (int) vector.x;
      this.Y += (int) vector.y;
      this.Z += (int) vector.z;
      return this;
    }

    /// <summary>
    ///   Translate this location by a (x, y) vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Add(Vector2 vector)
    {
      this.X += (int)vector.x;
      this.Y += (int)vector.y;
      return this;
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
      this.X -= location.X;
      this.Y -= location.Y;
      this.Z -= location.Z;
      return this;
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
      this.X -= (int) x;
      this.Y -= (int) y;
      this.Z -= (int) z;
      return this;
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
      this.X -= x;
      this.Y -= y;
      this.Z -= z;
      return this;
    }

    /// <summary>
    ///   Translate this location by a (-x, -y, -z) vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Sub(Vector3 vector)
    {
      this.X -= (int) vector.x;
      this.Y -= (int) vector.y;
      this.Z -= (int) vector.z;
      return this;
    }

    /// <summary>
    ///   Translate this location by a (-x, -y) vector.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public VoxelLocation Sub(Vector2 vector)
    {
      this.X -= (int)vector.x;
      this.Y -= (int)vector.y;
      return this;
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
      this.X = (int) (this.X * sx);
      this.Y = (int) (this.Y * sy);
      this.Z = (int) (this.Z * sz);
      return this;
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
      this.X *= sx;
      this.Y *= sy;
      this.Z *= sz;
      return this;
    }

    /// <summary>
    ///   Multiply each coordinates by a factor.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelLocation Mul(float s)
    {
      this.X = (int) (this.X * s);
      this.Y = (int) (this.Y * s);
      this.Z = (int) (this.Z * s);
      return this;
    }

    /// <summary>
    ///   Multiply each coordinates by a factor.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelLocation Mul(int s)
    {
      this.X *= s;
      this.Y *= s;
      this.Z *= s;
      return this;
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
      this.X = (int)(this.X / sx);
      this.Y = (int)(this.Y / sy);
      this.Z = (int)(this.Z / sz);
      return this;
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
      this.X /= sx;
      this.Y /= sy;
      this.Z /= sz;
      return this;
    }

    /// <summary>
    ///   Divide each coordinates by a factor.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelLocation Div(float s)
    {
      this.X = (int)(this.X / s);
      this.Y = (int)(this.Y / s);
      this.Z = (int)(this.Z / s);
      return this;
    }

    /// <summary>
    ///   Divide each coordinates by a factor.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelLocation Div(int s)
    {
      this.X /= s;
      this.Y /= s;
      this.Z /= s;
      return this;
    }
    #endregion

    #region Base objects methods
    /// <see cref="object"/>
    public override string ToString()
    {
      return "VoxelLocation (" + this.X + ", " + this.Y + ", " + this.Z + ")";
    }

    /// <see cref="object"/>
    public override int GetHashCode()
    {
      return (((this.X) * 31 + this.Y) * 31 + this.Z) * 31;
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
        return this.X == toCmp.X && this.Y == toCmp.Y && this.Z == toCmp.Z;
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
      return new Vector3(location.X, location.Y, location.Z);
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
      return new Vector2(location.X, location.Y);
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
      return a.Copy().Add(b);
    }

    public static Vector3 operator +(Vector3 a, VoxelLocation b)
    {
      return b.Copy().Add(a);
    }

    public static VoxelLocation operator +(VoxelLocation a, Vector3 b)
    {
      return a.Copy().Add(b);
    }

    public static Vector2 operator +(Vector2 a, VoxelLocation b)
    {
      return b.Copy().Add(a);
    }

    public static VoxelLocation operator +(VoxelLocation a, Vector2 b)
    {
      return a.Copy().Add(b);
    }
    #endregion

    #region Substraction
    public static VoxelLocation operator -(VoxelLocation a, VoxelLocation b)
    {
      return a.Copy().Sub(b);
    }

    public static Vector3 operator -(Vector3 a, VoxelLocation b)
    {
      Vector3 result = a;
      result.x -= b.X;
      result.y -= b.Y;
      result.z -= b.Z;
      return result;
    }

    public static VoxelLocation operator -(VoxelLocation a, Vector3 b)
    {
      return a.Copy().Sub(b);
    }

    public static Vector2 operator -(Vector2 a, VoxelLocation b)
    {
      Vector2 result = a;
      result.x -= b.X;
      result.y -= b.Y;
      return result;
    }

    public static VoxelLocation operator -(VoxelLocation a, Vector2 b)
    {
      return a.Copy().Sub(b);
    }

    public static VoxelLocation operator -(VoxelLocation a)
    {
      return a.Copy().Mul(-1);
    }
    #endregion

    #region Multiplication
    public static VoxelLocation operator *(VoxelLocation a, float b)
    {
      return a.Copy().Mul(b);
    }

    public static VoxelLocation operator *(float a, VoxelLocation b)
    {
      return b.Copy().Mul(a);
    }

    public static VoxelLocation operator *(VoxelLocation a, int b)
    {
      return a.Copy().Mul(b);
    }

    public static VoxelLocation operator *(int a, VoxelLocation b)
    {
      return b.Copy().Mul(a);
    }
    #endregion

    #region Division
    public static VoxelLocation operator /(VoxelLocation a, float b)
    {
      return a.Copy().Div(b);
    }

    public static VoxelLocation operator /(VoxelLocation a, int b)
    {
      return a.Copy().Div(b);
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
