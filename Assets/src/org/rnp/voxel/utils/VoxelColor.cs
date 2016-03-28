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
  ///   A VoxelColor implementation.
  ///   
  ///   This object is able to transform RGB values to HSL, it will be
  /// used for color manipulation in general. Without that, you should prefer
  /// the Color struct of Unity.
  /// </summary>
  public sealed class VoxelColor : ICopiable<VoxelColor>
  {
    #region Fields
    /// <summary>
    ///   A red value from 0f to 1f.
    /// </summary>
    private float _r;

    /// <summary>
    ///   A green value from 0f to 1f.
    /// </summary>
    private float _g;

    /// <summary>
    ///   A blue value from 0f to 1f.
    /// </summary>
    private float _b;

    /// <summary>
    ///   An alpha value from 0f to 1f.
    /// </summary>
    private float _a;
    #endregion

    #region Getters & Setters
    /// <summary>
    ///   Red value between 0f and 1f.
    /// </summary>
    public float R
    {
      get { return this._r; }
      set { this._r = value; }
    }

    /// <summary>
    ///   Green value between 0f and 1f.
    /// </summary>
    public float G
    {
      get { return this._g; }
      set { this._g = value; }
    }

    /// <summary>
    ///   Blue value between 0f and 1f.
    /// </summary>
    public float B
    {
      get { return this._b; }
      set { this._b = value; }
    }

    /// <summary>
    ///   Alpha value between 0f and 1f.
    /// </summary>
    public float A
    {
      get { return this._a; }
      set { this._a = value; }
    }

    private float _lastHue = 0;

    /// <summary>
    ///   Hue value between 0f and 1f.
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/HSL_and_HSV"/>
    public float Hue
    {
      get
      {
        float max = Mathf.Max(Mathf.Max(this._r, this._g), this._b);
        float min = Mathf.Min(Mathf.Min(this._r, this._g), this._b);
        float c = max - min;

        if (c <= 0) return _lastHue;

        if(this._r >= this._b && this._r >= this._g)
        {
          return _lastHue = (((((this._g - this._b) / c) + 6) % 6) * 60f) / 360f;
        }
        else if(this._g >= this._b)
        {
          return _lastHue = ((((this._b - this._r) / c) + 2) * 60f) / 360f;
        }
        else {
          return _lastHue = ((((this._r - this._g) / c) + 4) * 60f) / 360f;
        }
      }
      set
      {
        this.SetHSL(value, this.Saturation, this.Luminosity);
      }
    }

    /// <summary>
    ///   Saturation value between 0f and 1f.
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/HSL_and_HSV"/>
    public float Saturation
    {
      get
      {
        float max = Mathf.Max(Mathf.Max(this._r, this._g), this._b);

        if (max <= 0) return 0f;

        float min = Mathf.Min(Mathf.Min(this._r, this._g), this._b);

        return (max - min)/max;
      }
      set
      {
        this.SetHSL(this.Hue, value, this.Luminosity);
      }
    }

    /// <summary>
    ///   Luminosity value between 0f and 1f.
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/HSL_and_HSV"/>
    public float Luminosity
    {
      get
      {
        return Mathf.Max(Mathf.Max(this._r, this._g), this._b);
      }
      set
      {
        this.SetHSL(this.Hue, this.Saturation, value);
      }
    }
    #endregion

    #region Constructors
    /// <summary>
    ///   Simple dark color.
    /// </summary>
    public VoxelColor()
    {
      this.Set(0f, 0f, 0f, 1f);
    }

    /// <summary>
    ///   RGB color.
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public VoxelColor(float r, float g, float b)
    {
      this.Set(r, g, b, 1f);
    }

    /// <summary>
    ///   RGBA color.
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    public VoxelColor(float r, float g, float b, float a)
    {
      this.Set(r, g, b, a);
    }

    /// <summary>
    ///   Copy constructor.
    /// </summary>
    public VoxelColor(VoxelColor toCopy)
    {
      this.Set(toCopy);
    }

    /// <summary>
    ///   Copy constructor.
    /// </summary>
    public VoxelColor(Color toCopy)
    {
      this.Set(toCopy);
    }

    /// <summary>
    ///   Copy constructor.
    /// </summary>
    public VoxelColor(Color32 toCopy)
    {
      this.Set(toCopy);
    }
    #endregion

    #region Definition
    /// <summary>
    ///   Set equal to another color.
    /// </summary>
    /// <param name="color"></param>
    public VoxelColor Set(Color32 color)
    {
      this._r = color.r / 255f;
      this._b = color.b / 255f;
      this._g = color.g / 255f;
      this._a = color.a / 255f;
      return this;
    }

    /// <summary>
    ///   Set equal to another color.
    /// </summary>
    /// <param name="color"></param>
    public VoxelColor Set(Color color)
    {
      this._r = color.r;
      this._b = color.b;
      this._g = color.g;
      this._a = color.a;
      return this;
    }

    /// <summary>
    ///   Set equal to another color.
    /// </summary>
    /// <param name="other"></param>
    public VoxelColor Set(VoxelColor other)
    {
      this._r = other.R;
      this._g = other.G;
      this._b = other.B;
      this._a = other.A;
      return this;
    }

    /// <summary>
    ///   Change RGBA values.
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    public VoxelColor Set(float r, float g, float b, float a)
    {
      this._r = r;
      this._g = g;
      this._b = b;
      this._a = a;
      return this;
    }

    /// <summary>
    ///   Change RGB values.
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public VoxelColor Set(float r, float g, float b)
    {
      this._r = r;
      this._g = g;
      this._b = b;
      return this;
    }

    /// <summary>
    ///   Change the HSL value of this color.
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="saturation"></param>
    /// <param name="lightness"></param>
    public VoxelColor SetHSL(float hue, float saturation, float lightness)
    {
      return this.SetHSL(hue, saturation, lightness, this._a);
    }

    /// <summary>
    ///   Change the HSL value of this color.
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="saturation"></param>
    /// <param name="lightness"></param>
    /// <param name="alpha"></param>
    /// <see cref="https://en.wikipedia.org/wiki/HSL_and_HSV"/>
    public VoxelColor SetHSL(float hue, float saturation, float lightness, float alpha)
    {
      float C = lightness * saturation;
      float T = (hue * 360f) / 60f;
      float X = C * (1 - Mathf.Abs((T % 2f) - 1f));
      
      if (T < 1)
      {
        this._r = C;
        this._g = X;
        this._b = 0;
      }
      else if (T < 2)
      {
        this._r = X;
        this._g = C;
        this._b = 0;
      }
      else if (T < 3)
      {
        this._r = 0;
        this._g = C;
        this._b = X;
      }
      else if (T < 4)
      {
        this._r = 0;
        this._g = X;
        this._b = C;
      }
      else if (T < 5)
      {
        this._r = X;
        this._g = 0;
        this._b = C;
      }
      else if (T < 6)
      {
        this._r = C;
        this._g = 0;
        this._b = X;
      }

      float m = lightness - C;
      this._r += m;
      this._g += m;
      this._b += m;

      this._a = alpha;

      return this;
    }
    #endregion

    #region Addition
    /// <summary>
    ///   Add another color to this color.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor Add(VoxelColor other)
    {
      this._r += other._r;
      this._g += other._g;
      this._b += other._b;
      return this;
    }

    /// <summary>
    ///   Add another color to this color (with alpha).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor AddAll(VoxelColor other)
    {
      this._r += other._r;
      this._g += other._g;
      this._b += other._b;
      this._a += other._a;
      return this;
    }

    /// <summary>
    ///   Add another color to this color.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor Add(Color other)
    {
      this._r += other.r;
      this._g += other.g;
      this._b += other.b;
      return this;
    }

    /// <summary>
    ///   Add another color to this color (with alpha).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor AddAll(Color other)
    {
      this._r += other.r;
      this._g += other.g;
      this._b += other.b;
      this._a += other.a;
      return this;
    }

    /// <summary>
    ///   Add another color to this color.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor Add(Color32 other)
    {
      this._r += other.r / 255f;
      this._g += other.g / 255f;
      this._b += other.b / 255f;
      return this;
    }

    /// <summary>
    ///   Add another color to this color (with alpha).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor AddAll(Color32 other)
    {
      this._r += other.r / 255f;
      this._g += other.g / 255f;
      this._b += other.b / 255f;
      this._a += other.a / 255f;
      return this;
    }
    #endregion

    #region Substraction
    /// <summary>
    ///   Sub another color to this color.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor Sub(VoxelColor other)
    {
      this._r -= other._r;
      this._g -= other._g;
      this._b -= other._b;
      return this;
    }

    /// <summary>
    ///   Sub another color to this color (with alpha).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor SubAll(VoxelColor other)
    {
      this._r -= other._r;
      this._g -= other._g;
      this._b -= other._b;
      this._a -= other._a;
      return this;
    }

    /// <summary>
    ///   Sub another color to this color.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor Sub(Color other)
    {
      this._r -= other.r;
      this._g -= other.g;
      this._b -= other.b;
      return this;
    }

    /// <summary>
    ///   Sub another color to this color (with alpha).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor SubAll(Color other)
    {
      this._r -= other.r;
      this._g -= other.g;
      this._b -= other.b;
      this._a -= other.a;
      return this;
    }

    /// <summary>
    ///   Sub another color to this color.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor Sub(Color32 other)
    {
      this._r -= other.r / 255f;
      this._g -= other.g / 255f;
      this._b -= other.b / 255f;
      return this;
    }

    /// <summary>
    ///   Sub another color to this color (with alpha).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public VoxelColor SubAll(Color32 other)
    {
      this._r -= other.r / 255f;
      this._g -= other.g / 255f;
      this._b -= other.b / 255f;
      this._a -= other.a / 255f;
      return this;
    }
    #endregion

    #region Multiplication
    /// <summary>
    ///   Multiply each unit by a scalar.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor Mul(float s)
    {
      this._r *= s;
      this._g *= s;
      this._b *= s;
      return this;
    }

    /// <summary>
    ///   Multiply each unit by a scalar. (with alpha)
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor MulAll(float s)
    {
      this._r *= s;
      this._g *= s;
      this._b *= s;
      this._a *= s;
      return this;
    }

    /// <summary>
    ///   Multiply each unit by its own scalar.
    /// </summary>
    /// <param name="sr"></param>
    /// <param name="sg"></param>
    /// <param name="sb"></param>
    /// <returns></returns>
    public VoxelColor Mul(float sr, float sg, float sb)
    {
      this._r *= sr;
      this._g *= sg;
      this._b *= sb;
      return this;
    }

    /// <summary>
    ///   Multiply each unit by its own scalar. (with alpha)
    /// </summary>
    /// <param name="sr"></param>
    /// <param name="sg"></param>
    /// <param name="sb"></param>
    /// <param name="sa"></param>
    /// <returns></returns>
    public VoxelColor MulAll(float sr, float sg, float sb, float sa)
    {
      this._r *= sr;
      this._g *= sg;
      this._b *= sb;
      this._a *= sa;
      return this;
    }
    #endregion

    #region Division
    /// <summary>
    ///   Divide each unit by a scalar.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor Div(float s)
    {
      this._r /= s;
      this._g /= s;
      this._b /= s;
      return this;
    }

    /// <summary>
    ///   Divide each unit by a scalar. (with alpha)
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor DivAll(float s)
    {
      this._r /= s;
      this._g /= s;
      this._b /= s;
      this._a /= s;
      return this;
    }

    /// <summary>
    ///   Divide each unit by its own scalar.
    /// </summary>
    /// <param name="sr"></param>
    /// <param name="sg"></param>
    /// <param name="sb"></param>
    /// <returns></returns>
    public VoxelColor Div(float sr, float sg, float sb)
    {
      this._r /= sr;
      this._g /= sg;
      this._b /= sb;
      return this;
    }

    /// <summary>
    ///   Divide each unit by its own scalar. (with alpha)
    /// </summary>
    /// <param name="sr"></param>
    /// <param name="sg"></param>
    /// <param name="sb"></param>
    /// <param name="sa"></param>
    /// <returns></returns>
    public VoxelColor DivAll(float sr, float sg, float sb, float sa)
    {
      this._r /= sr;
      this._g /= sg;
      this._b /= sb;
      this._a /= sa;
      return this;
    }
    #endregion

    #region Mix
    /// <summary>
    ///   Mix with another color.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor Mix(VoxelColor other, float s)
    {
      this._r = (this._r) * (1f - s) + other._r * s;
      this._g = (this._g) * (1f - s) + other._g * s;
      this._b = (this._b) * (1f - s) + other._b * s;
      return this;
    }

    /// <summary>
    ///   Mix with another color. (with alpha)
    /// </summary>
    /// <param name="other"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor MixAll(VoxelColor other, float s)
    {
      this._r = (this._r) * (1f - s) + other._r * s;
      this._g = (this._g) * (1f - s) + other._g * s;
      this._b = (this._b) * (1f - s) + other._b * s;
      this._a = (this._a) * (1f - s) + other._a * s;
      return this;
    }

    /// <summary>
    ///   Mix with another color.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor Mix(Color other, float s)
    {
      this._r = (this._r) * (1f - s) + other.r * s;
      this._g = (this._g) * (1f - s) + other.g * s;
      this._b = (this._b) * (1f - s) + other.b * s;
      return this;
    }

    /// <summary>
    ///   Mix with another color. (with alpha)
    /// </summary>
    /// <param name="other"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor MixAll(Color other, float s)
    {
      this._r = (this._r) * (1f - s) + other.r * s;
      this._g = (this._g) * (1f - s) + other.g * s;
      this._b = (this._b) * (1f - s) + other.b * s;
      this._a = (this._a) * (1f - s) + other.a * s;
      return this;
    }

    /// <summary>
    ///   Mix with another color.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor Mix(Color32 other, float s)
    {
      this._r = (this._r) * (1f - s) + (other.r / 255f) * s;
      this._g = (this._g) * (1f - s) + (other.g / 255f) * s;
      this._b = (this._b) * (1f - s) + (other.b / 255f) * s;
      return this;
    }

    /// <summary>
    ///   Mix with another color. (with alpha)
    /// </summary>
    /// <param name="other"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public VoxelColor MixAll(Color32 other, float s)
    {
      this._r = (this._r) * (1f - s) + (other.r / 255f) * s;
      this._g = (this._g) * (1f - s) + (other.g / 255f) * s;
      this._b = (this._b) * (1f - s) + (other.b / 255f) * s;
      this._a = (this._a) * (1f - s) + (other.a / 255f) * s;
      return this;
    }
    #endregion

    #region Methods
    /// <summary>
    ///   Change to a lighter color.
    /// </summary>
    /// <param name="percentage"></param>
    public VoxelColor Lighter(float percentage)
    {
      float coef = 1f + percentage;
      this.SetHSL(this.Hue, this.Saturation, this.Luminosity * coef);
      return this;
    }

    /// <summary>
    ///   Change to a darker color.
    /// </summary>
    /// <param name="percentage"></param>
    public VoxelColor Darker(float percentage)
    {
      float coef = 1f - percentage;
      this.SetHSL(this.Hue, this.Saturation, this.Luminosity * coef);
      return this;
    }

    /// <summary>
    ///   Return true if the red, green and blue values are null.
    /// </summary>
    /// <returns></returns>
    public bool IsNull()
    {
      return this.R == this.B && this.B == this.G && this.G == 0;
    }
    #endregion

    #region ICopiable
    public VoxelColor Copy()
    {
      return new VoxelColor(this);
    }
    #endregion

    #region Object methods
    public override int GetHashCode()
    {
      return (((int)(this.R * 255f) * 31 + (int)(this.G * 255f)) * 31 + (int)(this.B * 255f)) * 31 + (int)(this.A * 255f);
    }

    public override bool Equals(object obj)
    {
      if (obj == null) return false;

      if (base.Equals(obj)) return true;

      if(obj is VoxelColor)
      {
        VoxelColor toCmp = (VoxelColor) obj;

        return (int)(toCmp.R * 255f) == (int)(this.R * 255f)
                && (int)(toCmp.G * 255f) == (int)(this.G * 255f)
                && (int)(toCmp.B * 255f) == (int)(this.B * 255f)
                && (int)(toCmp.A * 255f) == (int)(this.A * 255f);
      }
      
      return base.Equals(obj);
    }
    #endregion

    #region Operators overinding

    #region Cast
    public static implicit operator Color(VoxelColor color)
    {
      return new Color(color.R, color.G, color.B, color.A);
    }

    public static implicit operator VoxelColor(Color color)
    {
      return new VoxelColor(color);
    }

    public static implicit operator Color32(VoxelColor color)
    {
      return new Color32(
        (byte)(color.R * 255f), 
        (byte)(color.G * 255f),
        (byte)(color.B * 255f),
        (byte)(color.A * 255f)
      );
    }

    public static implicit operator VoxelColor(Color32 color)
    {
      return new VoxelColor(color);
    }
    #endregion

    #endregion
  }
}
