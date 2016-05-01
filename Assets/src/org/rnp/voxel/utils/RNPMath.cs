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
  ///   Somes math functions
  /// </summary>
  public static class RNPMath
  {
    /// <summary>
    ///   Return the gradient of a single value. (Derivate)
    /// </summary>
    /// <param name="start">First value of your function</param>
    /// <param name="end">Second value of your function</param>
    /// <param name="delta">Spacing between values</param>
    /// <returns>The gradient</returns>
    /// <seealso cref="https://fr.wikipedia.org/wiki/Gradient"/>
    public static float Gradient(float start, float end, float delta)
    {
      return (end - start) / (2 * delta);
    }

    /// <summary>
    ///   Return the gradient of a single value. (Derivate)
    /// </summary>
    /// <param name="start">First value of your function</param>
    /// <param name="end">Second value of your function</param>
    /// <param name="delta">Spacing between values</param>
    /// <returns></returns>
    /// <seealso cref="https://fr.wikipedia.org/wiki/Gradient"/>
    public static double Gradient(double start, double end, double delta)
    {
      return (end - start) / (2 * delta);
    }

    /// <summary>
    ///   Return the gradient of a vector3 value. (Derivate by axis)
    /// </summary>
    /// <param name="start">First values of your function</param>
    /// <param name="end">Second values of your function</param>
    /// <param name="delta">Spacing between values</param>
    /// <returns></returns>
    /// <seealso cref="https://fr.wikipedia.org/wiki/Gradient"/>
    public static Vector3 Gradient(Vector3 start, Vector3 end, Vector3 delta)
    {
      return new Vector3(
        RNPMath.Gradient(start.x, end.x, delta.x),
        RNPMath.Gradient(start.y, end.y, delta.y),
        RNPMath.Gradient(start.z, end.z, delta.z)
      );
    }

    /// <summary>
    ///   Return barycentric coordinates
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    /// <seealso cref="http://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates"/>
    public static Vector3 BarycentricCoordinates(Vector2 a, Vector2 b, Vector2 c, Vector2 point)
    {
      Vector2 v0 = b - a, v1 = c - a, v2 = point - a;
      float d00 = Vector2.Dot(v0, v0);
      float d01 = Vector2.Dot(v0, v1);
      float d11 = Vector2.Dot(v1, v1);
      float d20 = Vector2.Dot(v2, v0);
      float d21 = Vector2.Dot(v2, v1);
      float denom = d00 * d11 - d01 * d01;

      float v = (d11 * d20 - d01 * d21) / denom;
      float w = (d00 * d21 - d01 * d20) / denom;
      return new Vector3(
        1.0f - v - w,
        v,
        w
      );
    }

    /// <summary>
    ///   Return barycentric coordinates
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    /// <seealso cref="http://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates"/>
    public static Vector3 BarycentricCoordinates(Vector3 a, Vector3 b, Vector3 c, Vector3 point)
    {
      Vector3 v0 = b - a, v1 = c - a, v2 = point - a;
      float d00 = Vector3.Dot(v0, v0);
      float d01 = Vector3.Dot(v0, v1);
      float d11 = Vector3.Dot(v1, v1);
      float d20 = Vector3.Dot(v2, v0);
      float d21 = Vector3.Dot(v2, v1);
      float denom = d00 * d11 - d01 * d01;

      float v = (d11 * d20 - d01 * d21) / denom;
      float w = (d00 * d21 - d01 * d20) / denom;
      return new Vector3(
        v,
        w,
        1.0f - v - w
      );
    }

    /// <summary>
    ///   Return the gradient of a vector2 value. (Derivate by axis)
    /// </summary>
    /// <param name="start">First values of your function</param>
    /// <param name="end">Second values of your function</param>
    /// <param name="delta">Spacing between values</param>
    /// <returns></returns>
    /// <seealso cref="https://fr.wikipedia.org/wiki/Gradient"/>
    public static Vector2 Gradient(Vector2 start, Vector2 end, Vector2 delta)
    {
      return new Vector2(
        RNPMath.Gradient(start.x, end.x, delta.x),
        RNPMath.Gradient(start.y, end.y, delta.y)
      );
    }

    /// <seealso cref="https://fr.wikipedia.org/wiki/Divergence_(analyse_vectorielle)"/>
    public static float Divergence(Vector3 start, Vector3 end, Vector3 delta)
    {
      Vector3 gradient = RNPMath.Gradient(start, end, delta);
      return gradient.x + gradient.y + gradient.z;
    }

    public static float Clamp(float value, float ceil, float accuracy)
    {
      if (Mathf.Abs(value - ceil) <= accuracy) return ceil;
      else return value;
    }

    /// <seealso cref="https://fr.wikipedia.org/wiki/Divergence_(analyse_vectorielle)"/>
    public static float Divergence(Vector2 start, Vector2 end, Vector2 delta)
    {
      Vector3 gradient = RNPMath.Gradient(start, end, delta);
      return gradient.x + gradient.y;
    }

    /// <seealso cref="https://fr.wikipedia.org/wiki/Op%C3%A9rateur_laplacien"/>
    public static float Laplacian(float start, float middle, float end, float delta)
    {
      return (end - 2 * middle + start) / (delta * delta);
    }

    /// <seealso cref="https://fr.wikipedia.org/wiki/Op%C3%A9rateur_laplacien"/>
    public static float Laplacian(Vector2 start, Vector2 middle, Vector2 end, Vector2 delta)
    {
      return RNPMath.Laplacian(start.x, middle.x, end.x, delta.x)
             + RNPMath.Laplacian(start.y, middle.y, end.y, delta.y);
    }

    /// <seealso cref="https://fr.wikipedia.org/wiki/Op%C3%A9rateur_laplacien"/>
    public static float Laplacian(Vector3 start, Vector3 middle, Vector3 end, Vector3 delta)
    {
      return RNPMath.Laplacian(start.x, middle.x, end.x, delta.x)
             + RNPMath.Laplacian(start.y, middle.y, end.y, delta.y)
             + RNPMath.Laplacian(start.z, middle.z, end.z, delta.z);
    }
  }
}
