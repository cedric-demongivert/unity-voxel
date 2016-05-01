using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.fluid
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Eularian grid for eularian fluid simulation.
  /// </summary>
  [Serializable]
  public class EularianGrid
  {
    [SerializeField]
    private Dimensions3D _size;

    private struct Interpolation
    {
      public float[] interpolationValues;
      public Vector3 a;
      public Vector3 b;
      public Vector3 c;
      public Vector3 d;

      public Vector3 GetPoint(int indx)
      {
        switch(indx)
        {
          case 0: return a;
          case 1: return b;
          case 2: return d;
          case 3: return c;
          default: throw new IndexOutOfRangeException();
        }
      }

      public float GetCoef(int indx)
      {
        return this.interpolationValues[indx];
      }

      public int Count()
      {
        return 3;
      }

      public Interpolation(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 location)
      {
        Vector3 localized = location - a;

        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;

        if (localized.sqrMagnitude < (0.0001 * 0.0001))
        {
          this.interpolationValues = new float[] { 1, 0, 0, 0 };
        }
        else
        {
          this.interpolationValues = new float[] { 0, 0, 0, 0 };
          this.Initialize(location);
        }
      }

      public void Initialize(Vector3 location)
      {
        Vector3 localized = location - a;

        this.interpolationValues[0] = RNPMath.Clamp((1f - Mathf.Abs(localized.x) + 1f - Mathf.Abs(localized.y))/ 2f, 0, 0.1f);
        this.interpolationValues[1] = RNPMath.Clamp((Mathf.Abs(localized.x)) / 2f, 0, 0.1f);
        this.interpolationValues[2] = RNPMath.Clamp((Mathf.Abs(localized.y)) / 2f, 0, 0.1f);
      }
    }

    /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
    ///
    /// <summary>
    ///   Fluid data.
    /// </summary>
    [Serializable]
    public struct Fluid
    {
      public float FluidQuantity;
      public Vector3 FluidSpeed;

      public Fluid(Fluid toCopy)
      {
        this.FluidQuantity = toCopy.FluidQuantity;
        this.FluidSpeed = toCopy.FluidSpeed;
      }

      public Fluid(float quantity, Vector3 speed)
      {
        this.FluidQuantity = quantity;
        this.FluidSpeed = speed;
      }

      public Fluid Add(Fluid other)
      {
        this.FluidQuantity += other.FluidQuantity;
        this.FluidSpeed += other.FluidSpeed;
        return this;
      }

      public Fluid Sub(Fluid other)
      {
        this.FluidQuantity -= other.FluidQuantity;
        this.FluidSpeed -= other.FluidSpeed;
        return this;
      }

      public Fluid Set(Fluid other)
      {
        this.FluidQuantity = other.FluidQuantity;
        this.FluidSpeed = other.FluidSpeed;
        return this;
      }

      public Fluid Mul(float scalar)
      {
        this.FluidQuantity *= scalar;
        this.FluidSpeed *= scalar;
        return this;
      }

      public Fluid Div(float scalar)
      {
        this.FluidQuantity /= scalar;
        this.FluidSpeed /= scalar;
        return this;
      }

      public Fluid Copy()
      {
        return new Fluid(this);
      }
    }

    [SerializeField]
    private Fluid[,,] _nodes;
    
    /// <summary>
    ///   Return the size of the eularian grid.
    /// </summary>
    public Dimensions3D Size
    {
      get
      {
        return this._size;
      }
    }
    
    /// <summary>
    ///   Get or set fluid data.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public Fluid this[Vector3 location]
    {
      get
      {
        return this.Get(location);
      }
      set
      {
        this.Set(location, value);
      }
    }

    /// <summary>
    ///   Create a new grid.
    /// </summary>
    /// <param name="simulationSize"></param>
    public EularianGrid(Dimensions3D size)
    {
      this._size = size;
      this._nodes = new Fluid[this._size.Width + 1, this._size.Height + 1, this._size.Depth + 1];
    }

    /// <summary>
    ///   
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    private Vector3 GetReferer(Vector3 location)
    {
      return new Vector3(
        Mathf.RoundToInt(location.x),
        Mathf.RoundToInt(location.y),
        Mathf.RoundToInt(location.z)
      );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    private Interpolation Interpolate(Vector3 location)
    {
      Vector3 referer = this.GetReferer(location);
      Vector3 localized = location - referer;
      Vector3 directions = Voxels.StrictDirection(localized);
      
      return new Interpolation(
        referer,
        referer + Voxels.Mask(directions, 1),
        referer + Voxels.Mask(directions, 3),
        referer + Voxels.Mask(directions, 2),
        location
      );
    }

    /// <summary>
    ///   Return interpolated data.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public Fluid Get(Vector3 location)
    {
      if(this.Contains(location))
      {
        Interpolation interpolation = this.Interpolate(location);
        Fluid fluid = new Fluid();

        for (int i = 0; i < interpolation.Count(); ++i)
        {
          fluid.Add(
            this.GetNode(interpolation.GetPoint(i))
                .Copy().Mul(interpolation.GetCoef(i))
          );
        }

        return fluid;
      }
      else
      {
        return new Fluid();
      }
      
    }

    /// <summary>
    ///   Add some values
    /// </summary>
    /// <param name="location"></param>
    /// <param name="data"></param>
    public void Add(Vector3 location, Fluid data)
    {
      if (this.Contains(location))
      {
        Vector3 referer = this.GetReferer(location);
        Interpolation interpolation = this.Interpolate(location);
        
        for (int i = 0; i < interpolation.Count(); ++i)
        {
          float coef = interpolation.GetCoef(i);

          if (coef >= 0.01)
          {
            Fluid fluid = this.GetNode(interpolation.GetPoint(i));
            fluid.Add(data.Copy().Mul(interpolation.GetCoef(i)));
            this.SetNode(interpolation.GetPoint(i), fluid);
          }
        }
      }
    }

    /// <summary>
    ///   Sub some values
    /// </summary>
    /// <param name="location"></param>
    /// <param name="data"></param>
    public void Sub(Vector3 location, Fluid data)
    {
      if (this.Contains(location))
      {
        Vector3 referer = this.GetReferer(location);
        Interpolation interpolation = this.Interpolate(location);

        for (int i = 0; i < interpolation.Count(); ++i)
        {
          Fluid fluid = this.GetNode(interpolation.GetPoint(i));
          fluid.Sub(data.Copy().Mul(interpolation.GetCoef(i)));
          this.SetNode(interpolation.GetPoint(i), fluid);
        }
      }
    }

    /// <summary>
    ///   Change interpolated data.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="newData"></param>
    public void Set(Vector3 location, Fluid newData)
    {
      if (this.Contains(location))
      {
        Vector3 referer = this.GetReferer(location);
        Interpolation interpolation = this.Interpolate(location);
        
        for (int i = 0; i < interpolation.Count(); ++i)
        {
          Fluid fluid = this.GetNode(interpolation.GetPoint(i));
          fluid.Set(newData.Copy().Mul(interpolation.GetCoef(i)));
          this.SetNode(interpolation.GetPoint(i), fluid);
        }
      }
    }

    /// <summary>
    ///   Return a node data. This method don't interpolate data.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public Fluid GetNode(Vector3 vector)
    {
      if (this.Contains(vector))
      {
        return this._nodes[
          Mathf.RoundToInt(vector.x),
          Mathf.RoundToInt(vector.y),
          Mathf.RoundToInt(vector.z)
        ];
      }
      else
      {
        return new Fluid();
      }
    }

    /// <summary>
    ///   Return a node data. This method don't interpolate data.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Fluid GetNode(int x, int y, int z)
    {
      if(this.Contains(x,y,z))
      {
        return this._nodes[x, y, z];
      }
      else
      {
        return new Fluid();
      }
    }

    /// <summary>
    ///   Change node data.
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="data"></param>
    public void SetNode(Vector3 vector, Fluid data)
    {
      if(this.Contains(vector))
      {
        this._nodes[
          Mathf.RoundToInt(vector.x),
          Mathf.RoundToInt(vector.y),
          Mathf.RoundToInt(vector.z)
        ] = data;
      }
    }

    /// <summary>
    ///   Change node data.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="data"></param>
    public void SetNode(int x, int y, int z, Fluid data)
    {
      this._nodes[x, y, z] = data;
    }

    /// <summary>
    ///   Check if a location is in the grid.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public bool Contains(Vector3 location)
    {
      return location.x >= 0 && location.x <= this._size.Width &&
             location.y >= 0 && location.y <= this._size.Height &&
             location.z >= 0 && location.z <= this._size.Depth;
    }

    /// <summary>
    ///   Return true if this grid contains a (x, y, z) node.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public bool Contains(int x, int y, int z)
    {
      return x >= 0 && x <= this._size.Width  && 
             y >= 0 && y <= this._size.Height && 
             z >= 0 && z <= this._size.Depth;
    }

    /// <summary>
    ///   Return the maximum speed that is in the grid.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMaxSpeed()
    {
      Vector3 max = Vector3.zero;

      for(int i = 0; i < this._size.Width; ++i)
      {
        for (int j = 0; j < this._size.Height; ++j)
        {
          for (int k = 0; k < this._size.Depth; ++k)
          {
            if(this.GetNode(i,j,k).FluidSpeed.sqrMagnitude > max.sqrMagnitude)
            {
              max = this.GetNode(i, j, k).FluidSpeed;
            }
          }
        }
      }

      return max;
    }
  }
}
