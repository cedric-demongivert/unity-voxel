using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.utils
{
  public sealed class  Voxels
  {
    public static readonly Color32 Empty = new Color32(0,0,0,255);

    /// <summary>
    ///   Check if a voxel is empty.
    /// </summary>
    /// <param name="Voxel"></param>
    /// <returns></returns>
    public static bool IsEmpty(Color32 voxel)
    {
      return voxel.a == 255;
    }

    /// <summary>
    ///   Check if a voxel is not empty.
    /// </summary>
    /// <param name="Voxel"></param>
    /// <returns></returns>
    public static bool IsNotEmpty(Color32 voxel)
    {
      return !Voxels.IsEmpty(voxel);
    }

    /// <summary>
    ///   Transform a vector in a voxel direction vector.
    ///   
    ///   For each value of the vector, if a value is negative this method
    ///   set it to -1f, if a value is positive this method set it to 1f and if
    ///   a value is null, this method set it to 0f.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 Direction(Vector3 vector)
    {
      Vector3 result = new Vector3();

      if (Mathf.Approximately(vector.x, 0f))
      {
        result.x = 0f;
      }
      else if (vector.x > 0f)
      {
        result.x = 1f;
      }
      else
      {
        result.x = -1f;
      }

      if (Mathf.Approximately(vector.y, 0f))
      {
        result.y = 0f;
      }
      else if (vector.y > 0f)
      {
        result.y = 1f;
      }
      else
      {
        result.y = -1f;
      }

      if (Mathf.Approximately(vector.z, 0f))
      {
        result.z = 0f;
      }
      else if (vector.z > 0)
      {
        result.z = 1f;
      }
      else
      {
        result.z = -1f;
      }

      return result;
    }

    /// <summary>
    ///   Keep only the maximum coordinate to 1f (or -1f).
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 KeepMax(Vector3 vector)
    {
      float x = Mathf.Abs(vector.x);
      float y = Mathf.Abs(vector.y);
      float z = Mathf.Abs(vector.z);

      if (x >= y && x >= z)
      {
        if (vector.x >= 0)
        {
          return new Vector3(1f, 0f, 0f);
        }
        else
        {
          return new Vector3(-1f, 0f, 0f);
        }
      }
      else if (y >= z)
      {
        if (vector.y >= 0)
        {
          return new Vector3(0f, 1f, 0f);
        }
        else
        {
          return new Vector3(0f, -1f, 0f);
        }
      }
      else
      {
        if (vector.z >= 0)
        {
          return new Vector3(0f, 0f, 1f);
        }
        else
        {
          return new Vector3(0f, 0f, -1f);
        }
      }
    }

    /// <summary>
    ///   Return true if any coordinate of the vector is under 0.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static bool HasNegative(Vector3 vector)
    {
      return vector.x < 0 || vector.y < 0 || vector.z < 0;
    }

    /// <summary>
    ///   Return true if any coordinate of the vector is upper 0.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static bool HasPositive(Vector3 vector)
    {
      return vector.x > 0 || vector.y > 0 || vector.z > 0;
    }

    /// <summary>
    ///   Keep only the maximum coordinate to 1f.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 KeepMaxAbs(Vector3 vector)
    {
      float x = Mathf.Abs(vector.x);
      float y = Mathf.Abs(vector.y);
      float z = Mathf.Abs(vector.z);

      if (x >= y && x >= z)
      {
        return new Vector3(1f, 0f, 0f);
      }
      else if (y >= z)
      {
        return new Vector3(0f, 1f, 0f);
      }
      else
      {
        return new Vector3(0f, 0f, 1f);
      }
    }
  }
}
