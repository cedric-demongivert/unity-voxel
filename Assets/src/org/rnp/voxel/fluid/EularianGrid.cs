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

    /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
    ///
    /// <summary>
    ///   Fluid data.
    /// </summary>
    [Serializable]
    public struct Fluid
    {
      [SerializeField]
      private bool _isFilled;

      [SerializeField]
      private Vector3 _fluidSpeed;

      [SerializeField]
      private float _pressure;

      public float Pressure
      {
        get
        {
          if (this.IsEmpty)
          {
            return 0;
          }
          else
          {
            return this._pressure;
          }
        }
        set
        {
          this._pressure = value;
        }
      }

      public Vector3 FluidSpeed
      {
        get
        {
          if(this.IsEmpty)
          {
            return Vector3.zero;
          }
          else
          {
            return this._fluidSpeed;
          }
        }
        set
        {
          if(this.IsEmpty)
          {
            this._fluidSpeed = Vector3.zero;
          }
          else
          {
            this._fluidSpeed = value;
          }
        }
      }

      public bool IsEmpty
      {
        get
        {
          return !this._isFilled;
        }
        set
        {
          this._isFilled = !value;

          if(value)
          {
            this.FluidSpeed = Vector3.zero;
          }
        }
      }

      public Fluid(Fluid toCopy)
      {
        this.IsEmpty = toCopy.IsEmpty;
        this.FluidSpeed = toCopy.FluidSpeed;
      }

      public Fluid(Vector3 speed)
      {
        this.IsEmpty = false;
        this.FluidSpeed = speed;
      }

      public Fluid(bool isEmpty, Vector3 speed)
      {
        this.IsEmpty = isEmpty;
        this.FluidSpeed = speed;
      }

      public Fluid Fill()
      {
        this.IsEmpty = false;
        return this;
      }

      public Fluid Dry()
      {
        this.IsEmpty = true;
        return this;
      }

      public Fluid Add(Fluid other)
      {
        this.FluidSpeed += other.FluidSpeed;
        return this;
      }

      public Fluid Sub(Fluid other)
      {
        this.FluidSpeed -= other.FluidSpeed;
        return this;
      }

      public Fluid Set(Fluid other)
      {
        this.IsEmpty = other.IsEmpty;
        this.FluidSpeed = other.FluidSpeed;
        return this;
      }

      public Fluid Mul(float scalar)
      {
        this.FluidSpeed *= scalar;
        return this;
      }

      public Fluid Div(float scalar)
      {
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
    
    public Dimensions3D Size
    {
      get
      {
        return this._size;
      }
    }
    
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

    public Fluid this[int x, int y, int z]
    {
      get
      {
        return this.Get(x, y, z);
      }
      set
      {
        this.Set(x, y, z, value);
      }
    }

    public EularianGrid(Dimensions3D size)
    {
      this._size = size;
      this._nodes = new Fluid[this._size.Width, this._size.Height, this._size.Depth];
    }
    
    public void Remove(Vector3 location)
    {
      this[location] = new Fluid();
    }
    
    public void Add(Vector3 location, Fluid fluid)
    {
      this[location] = this[location].Fill().Add(fluid);
    }

    public void Set(Vector3 location, Fluid value)
    {
      this.Set(
           Mathf.FloorToInt(location.x),
           Mathf.FloorToInt(location.y),
           Mathf.FloorToInt(location.z),
           value
      );
    }

    public void Set(int x, int y, int z, Fluid value)
    {
      this.ResetPressure(x, y, z);
      this._nodes[x, y, z] = value;
      this.UpdatePressure(x, y, z);
    }

    private void UpdatePressure(int x, int y, int z)
    {
      if (this.Contains(x + 1, y, z))
      {
        this._nodes[x + 1, y, z].Pressure += this._nodes[x, y, z].FluidSpeed.x;
      }

      if (this.Contains(x, y + 1, z))
      {
        this._nodes[x, y + 1, z].Pressure += this._nodes[x, y, z].FluidSpeed.y;
      }

      if (this.Contains(x, y, z + 1))
      {
        this._nodes[x, y, z + 1].Pressure += this._nodes[x, y, z].FluidSpeed.z;
      }
      
      if (this.Contains(x - 1, y, z))
      {
        this._nodes[x, y, z].Pressure += this._nodes[x - 1, y, z].FluidSpeed.x;
      }

      if (this.Contains(x, y - 1, z))
      {
        this._nodes[x, y, z].Pressure += this._nodes[x, y - 1, z].FluidSpeed.y;
      }

      if (this.Contains(x, y, z - 1))
      {
        this._nodes[x, y, z].Pressure += this._nodes[x, y, z - 1].FluidSpeed.z;
      }

      this._nodes[x, y, z].Pressure = 0;
    }

    private void ResetPressure(int x, int y, int z)
    {
      if(this.Contains(x + 1, y, z))
      {
        this._nodes[x + 1, y, z].Pressure -= this._nodes[x, y, z].FluidSpeed.x;
      }

      if (this.Contains(x, y + 1, z))
      {
        this._nodes[x, y + 1, z].Pressure -= this._nodes[x, y, z].FluidSpeed.y;
      }

      if (this.Contains(x, y, z + 1))
      {
        this._nodes[x, y, z + 1].Pressure -= this._nodes[x, y, z].FluidSpeed.z;
      }

      this._nodes[x, y, z].Pressure = 0;
    }

    public Fluid Get(Vector3 vector)
    {
      return this.Get(
        Mathf.FloorToInt(vector.x),
        Mathf.FloorToInt(vector.y),
        Mathf.FloorToInt(vector.z)
      );
    }

    public Fluid Get(int i, int j, int k)
    {
      if (this.Contains(i, j, k))
      {
        return this._nodes[i, j, k];
      }
      else
      {
        return new Fluid();
      }
    }

    public bool Contains(Vector3 location)
    {
      return location.x >= 0 && location.x < this._size.Width &&
             location.y >= 0 && location.y < this._size.Height &&
             location.z >= 0 && location.z < this._size.Depth;
    }
    
    public bool Contains(int x, int y, int z)
    {
      return x >= 0 && x < this._size.Width  && 
             y >= 0 && y < this._size.Height && 
             z >= 0 && z < this._size.Depth;
    }
    
    public Vector3 GetMaxSpeed()
    {
      Vector3 max = Vector3.zero;

      for(int i = 0; i < this._size.Width; ++i)
      {
        for (int j = 0; j < this._size.Height; ++j)
        {
          for (int k = 0; k < this._size.Depth; ++k)
          {
            if(this.Get(i,j,k).FluidSpeed.sqrMagnitude > max.sqrMagnitude)
            {
              max = this.Get(i, j, k).FluidSpeed;
            }
          }
        }
      }

      return max;
    }
  }
}
