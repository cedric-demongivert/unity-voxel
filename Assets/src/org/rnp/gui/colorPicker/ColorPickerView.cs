using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using org.rnp.voxel.utils;
using UnityEngine;
using UnityEngine.UI;

namespace org.rnp.gui.colorPicker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   Helper class for color picker GUI.
  /// </summary>
  public class ColorPickerView : MonoBehaviour
  {
    [SerializeField]
    private ColorPicker _parentPicker;

    [SerializeField]
    private GUIColorPicker.ColorAttr _lockedAttr = GUIColorPicker.ColorAttr.Red;

    [SerializeField]
    private Toggle _redLockToggle;

    [SerializeField]
    private Toggle _blueLockToggle;

    [SerializeField]
    private Toggle _greenLockToggle;
    
    [SerializeField]
    private Toggle _hueLockToggle;

    [SerializeField]
    private Toggle _luminosityLockToggle;
    
    [SerializeField]
    private Toggle _saturationLockToggle;

    [SerializeField]
    private InputField _redInputField;

    [SerializeField]
    private InputField _greenInputField;

    [SerializeField]
    private InputField _blueInputField;

    [SerializeField]
    private InputField _alphaInputField;

    [SerializeField]
    private InputField _luminosityInputField;

    [SerializeField]
    private InputField _hueInputField;

    [SerializeField]
    private InputField _saturationInputField;

    [SerializeField]
    private InputField _hexInputField;

    /// <summary>
    ///   Red field value (as string)
    /// </summary>
    [SerializeField]
    private String _red;

    /// <summary>
    ///   Green field value (as string)
    /// </summary>
    [SerializeField]
    private String _green;

    /// <summary>
    ///   Blue field value (as string)
    /// </summary>
    [SerializeField]
    private String _blue;

    /// <summary>
    ///   Hue field value (as string)
    /// </summary>
    [SerializeField]
    private String _hue;

    /// <summary>
    ///   Saturation field value (as string)
    /// </summary>
    [SerializeField]
    private String _saturation;

    /// <summary>
    ///   Ligthness field value (as string)
    /// </summary>
    [SerializeField]
    private String _luminosity;

    /// <summary>
    ///   Alpha field value (as string)
    /// </summary>
    [SerializeField]
    private String _alpha;

    /// <summary>
    ///   Color as an hexadecimal value (as string)
    /// </summary>
    [SerializeField]
    private String _hex;

    /// <summary>
    ///   Is true if one of the Red / Green / Blue fields has changed.
    /// </summary>
    [SerializeField]
    private bool _rgbDirty;

    /// <summary>
    ///   Is true if one of the Hue / Saturation / Luminosity fields has changed.
    /// </summary>
    [SerializeField]
    private bool _hslDirty;

    /// <summary>
    ///   Is true if the alpha field has changed.
    /// </summary>
    [SerializeField]
    private bool _alphaDirty;

    /// <summary>
    ///   Is true if the hexadecimal field has changed.
    /// </summary>
    [SerializeField]
    private bool _hexDirty;

    public InputField RedInputField
    {
      get
      {
        return this._redInputField;
      }
      set
      {
        this._redInputField = value;
        this.RefreshUI();
      }
    }

    public InputField GreenInputField
    {
      get
      {
        return this._greenInputField;
      }
      set
      {
        this._greenInputField = value;
        this.RefreshUI();
      }
    }

    public InputField BlueInputField
    {
      get
      {
        return this._blueInputField;
      }
      set
      {
        this._blueInputField = value;
        this.RefreshUI();
      }
    }

    public InputField AlphaInputField
    {
      get
      {
        return this._alphaInputField;
      }
      set
      {
        this._alphaInputField = value;
        this.RefreshUI();
      }
    }

    public InputField LuminosityInputField
    {
      get
      {
        return this._luminosityInputField;
      }
      set
      {
        this._luminosityInputField = value;
        this.RefreshUI();
      }
    }

    public InputField HueInputField
    {
      get
      {
        return this._hueInputField;
      }
      set
      {
        this._hueInputField = value;
        this.RefreshUI();
      }
    }

    public InputField SaturationInputField
    {
      get
      {
        return this._saturationInputField;
      }
      set
      {
        this._saturationInputField = value;
        this.RefreshUI();
      }
    }

    public InputField HexInputField
    {
      get
      {
        return this._hexInputField;
      }
      set
      {
        this._hexInputField = value;
        this.RefreshUI();
      }
    }

    public Toggle RedLockToggle
    {
      get
      {
        return this._redLockToggle;
      }
      set
      {
        this._redLockToggle = value;
        this.RefreshUI();
      }
    }

    public Toggle BlueLockToggle
    {
      get
      {
        return this._blueLockToggle;
      }
      set
      {
        this._blueLockToggle = value;
        this.RefreshUI();
      }
    }

    public Toggle GreenLockToggle
    {
      get
      {
        return this._greenLockToggle;
      }
      set
      {
        this._greenLockToggle = value;
        this.RefreshUI();
      }
    }

    public Toggle SaturationLockToggle
    {
      get
      {
        return this._saturationLockToggle;
      }
      set
      {
        this._saturationLockToggle = value;
        this.RefreshUI();
      }
    }

    public Toggle LuminosityLockToggle
    {
      get
      {
        return this._luminosityLockToggle;
      }
      set
      {
        this._luminosityLockToggle = value;
        this.RefreshUI();
      }
    }

    public Toggle HueLockToggle
    {
      get
      {
        return this._hueLockToggle;
      }
      set
      {
        this._hueLockToggle = value;
        this.RefreshUI();
      }
    }

    /// <summary>
    ///   Is true if any field has changed.
    /// </summary>
    public bool IsDirty
    {
      get
      {
        return _rgbDirty || _hslDirty || _alphaDirty || _hexDirty;
      }
    }

    /// <summary>
    ///   Parent Color Picker
    /// </summary>
    public ColorPicker ParentPicker
    {
      get
      {
        return this._parentPicker;
      }
      set
      {
        if (this._parentPicker == value) return;

        ColorPicker old = this._parentPicker;
        this._parentPicker = null;

        if (old != null) old.View = null;

        this._parentPicker = value;
        value.View = this;

        this.SynchronizeLockedAttrWithParent(false);
        this.SynchronizePickedColorWithParent(false);
      }
    }
    
    /// <summary>
    ///   The locked attribute 
    /// </summary>
    public GUIColorPicker.ColorAttr LockedAttribute
    {
      get
      {
        return this._lockedAttr;
      }
      set
      {
        if (this._lockedAttr == value) return;

        this._lockedAttr = value;
        this.SynchronizeLockedAttrWithParent(true);
        this.RefreshUI();
      }
    }

    /// <summary>
    ///   Red field value (as string).
    ///   
    ///   If you try to set an invalid value, the field should not change.
    /// </summary>
    public String Red
    {
      get
      {
        return this._red;
      }
      set
      {
        value = value.Trim();
        if(!this._red.Equals(value) && this.IsValid(value))
        {
          this._red = this.CheckBounds(value, 0, 255);
          this._rgbDirty = true;
          this.SynchronizePickedColorWithParent(true);
        }
      }
    }

    /// <summary>
    ///   Green field value (as string).
    ///   
    ///   If you try to set an invalid value, the field should not change.
    /// </summary>
    public String Green
    {
      get
      {
        return this._green;
      }
      set
      {
        value = value.Trim();
        if (!this._green.Equals(value) && this.IsValid(value))
        {
          this._green = this.CheckBounds(value, 0, 255);
          this._rgbDirty = true;
          this.SynchronizePickedColorWithParent(true);
        }
      }
    }

    /// <summary>
    ///   Blue field value (as string).
    ///   
    ///   If you try to set an invalid value, the field should not change.
    /// </summary>
    public String Blue
    {
      get
      {
        return this._blue;
      }
      set
      {
        value = value.Trim();
        if (!this._blue.Equals(value) && this.IsValid(value))
        {
          this._blue = this.CheckBounds(value, 0, 255);
          this._rgbDirty = true;
          this.SynchronizePickedColorWithParent(true);
        }
      }
    }

    /// <summary>
    ///   Alpha field value (as string).
    ///   
    ///   If you try to set an invalid value, the field should not change.
    /// </summary>
    public String Alpha
    {
      get
      {
        return this._alpha;
      }
      set
      {
        value = value.Trim();
        if (!this._alpha.Equals(value) && this.IsValid(value))
        {
          this._alpha = this.CheckBounds(value, 0, 255);
          this._alphaDirty = true;
          this.SynchronizePickedColorWithParent(true);
        }
      }
    }

    /// <summary>
    ///   Hue field value (as string).
    ///   
    ///   If you try to set an invalid value, the field should not change.
    /// </summary>
    public String Hue
    {
      get
      {
        return this._hue;
      }
      set
      {
        value = value.Trim();
        if (!this._hue.Equals(value) && this.IsValid(value))
        {
          this._hue = this.CheckBounds(value, 0, 360);
          this._hslDirty = true;
          this.SynchronizePickedColorWithParent(true);
        }
      }
    }

    /// <summary>
    ///   Saturation field value (as string).
    ///   
    ///   If you try to set an invalid value, the field should not change.
    /// </summary>
    public String Saturation
    {
      get
      {
        return this._saturation;
      }
      set
      {
        value = value.Trim();
        if (!this._saturation.Equals(value) && this.IsValid(value))
        {
          this._saturation = this.CheckBounds(value, 0, 100);
          this._hslDirty = true;
          this.SynchronizePickedColorWithParent(true);
        }
      }
    }

    /// <summary>
    ///   Luminosity field value (as string).
    ///   
    ///   If you try to set an invalid value, the field should not change.
    /// </summary>
    public String Luminosity
    {
      get
      {
        return this._luminosity;
      }
      set
      {
        value = value.Trim();
        if (!this._luminosity.Equals(value) && this.IsValid(value))
        {
          this._luminosity = this.CheckBounds(value, 0, 100);
          this._hslDirty = true;
          this.SynchronizePickedColorWithParent(true);
        }
      }
    }

    /// <summary>
    ///   Hexadecimal field value (as string).
    ///   
    ///   If you try to set an invalid value, the field should not change.
    /// </summary>
    public String Hex
    {
      get
      {
        return this._hex;
      }
      set
      {
        value = value.Trim();
        if (!this._hex.Equals(value) && this.IsValidHex(value))
        {
          this._hex = value;
          this._hexDirty = true;
          this.SynchronizePickedColorWithParent(true);
        }
      }
    }

    public VoxelColor PickedColor
    {
      get
      {
        VoxelColor color = new VoxelColor();
        this.CommitView(color);
        return color;
      }
      set
      {
        if(value != this.PickedColor)
        {
          this.PullModel(value);
        }
      }
    }

    public void Awake()
    {
      this.PullModel(new VoxelColor());
      this.SynchronizeLockedAttrWithParent(false);
      this.SynchronizePickedColorWithParent(false);
    }

    private void SynchronizeLockedAttrWithParent(bool overrideParent)
    {
      if (this._parentPicker != null)
      {
        if (this._parentPicker.LockedAttribute != this._lockedAttr)
        {
          if (overrideParent)
          {
            this._parentPicker.LockedAttribute = this._lockedAttr;
          }
          else
          {
            this._lockedAttr = this._parentPicker.LockedAttribute;
          }
        }
      }
    }

    private void SynchronizePickedColorWithParent(bool overrideParent)
    {
      if (this._parentPicker != null)
      {
        if (this._parentPicker.PickedColor != this.PickedColor)
        {
          if (overrideParent)
          {
            this._parentPicker.PickedColor = this.PickedColor;
          }
          else
          {
            this.PickedColor = this._parentPicker.PickedColor;
          }
        }
      }
    }

    /// <summary>
    ///   Commit view state to UI
    /// </summary>
    private void RefreshUI()
    {
      if (this.RedLockToggle != null)         this.RedLockToggle.isOn        = (this.LockedAttribute == GUIColorPicker.ColorAttr.Red);
      if (this.BlueLockToggle != null)        this.BlueLockToggle.isOn       = (this.LockedAttribute == GUIColorPicker.ColorAttr.Blue);
      if (this.GreenLockToggle != null)       this.GreenLockToggle.isOn      = (this.LockedAttribute == GUIColorPicker.ColorAttr.Green);
      if (this.LuminosityLockToggle != null)  this.LuminosityLockToggle.isOn = (this.LockedAttribute == GUIColorPicker.ColorAttr.Luminosity);
      if (this.HueLockToggle != null)         this.HueLockToggle.isOn        = (this.LockedAttribute == GUIColorPicker.ColorAttr.Hue);
      if (this.SaturationLockToggle != null)  this.SaturationLockToggle.isOn = (this.LockedAttribute == GUIColorPicker.ColorAttr.Saturation);

      if (this.RedInputField != null) this.RedInputField.text = this.Red;
      if (this.BlueInputField != null) this.BlueInputField.text = this.Blue;
      if (this.GreenInputField != null) this.GreenInputField.text = this.Green;
      if (this.AlphaInputField != null) this.AlphaInputField.text = this.Alpha;
      if (this.HueInputField != null) this.HueInputField.text = this.Hue;
      if (this.LuminosityInputField != null) this.LuminosityInputField.text = this.Luminosity;
      if (this.SaturationInputField != null) this.SaturationInputField.text = this.Saturation;
      if (this.HexInputField != null) this.HexInputField.text = this.Hex;
    }

    public void LockRed()
    {
      this.LockedAttribute = GUIColorPicker.ColorAttr.Red;
    }

    public void LockGreen()
    {
      this.LockedAttribute = GUIColorPicker.ColorAttr.Green;
    }

    public void LockBlue()
    {
      this.LockedAttribute = GUIColorPicker.ColorAttr.Blue;
    }

    public void LockHue()
    {
      this.LockedAttribute = GUIColorPicker.ColorAttr.Hue;
    }

    public void LockSaturation()
    {
      this.LockedAttribute = GUIColorPicker.ColorAttr.Saturation;
    }

    public void LockLuminosity()
    {
      this.LockedAttribute = GUIColorPicker.ColorAttr.Luminosity;
    }

    /// <summary>
    ///   Update view values from a model.
    /// </summary>
    /// <param name="model"></param>
    public void PullModel(VoxelColor model)
    {
      this.PullAlpha(model);
      this.PullRGB(model);
      this.PullHSL(model);
      this.PullHex(model);
      this.Refresh();
      this.RefreshUI();
    }

    /// <summary>
    ///   Refresh hexadecimal field.
    /// </summary>
    /// <param name="model"></param>
    private void PullHex(VoxelColor model)
    {
      this._hex = GUIColorPicker.ToHex(model);
    }

    /// <summary>
    ///   Refresh alpha field.
    /// </summary>
    /// <param name="model"></param>
    private void PullAlpha(VoxelColor model)
    {
      this._alpha = ((int)(model.A * 255)).ToString();
    }

    /// <summary>
    ///   Refresh HSL fields.
    /// </summary>
    /// <param name="model"></param>
    private void PullHSL(VoxelColor model)
    {
      this._hue = ((int)(model.Hue * 360)).ToString();
      this._saturation = ((int)(model.Saturation * 100)).ToString();
      this._luminosity = ((int)(model.Luminosity * 100)).ToString();
    }

    /// <summary>
    ///   Refresh RGB fields.
    /// </summary>
    /// <param name="model"></param>
    private void PullRGB(VoxelColor model)
    {
      this._red = ((int)(model.R * 255)).ToString();
      this._green = ((int)(model.G * 255)).ToString();
      this._blue = ((int)(model.B * 255)).ToString();
    }

    /// <summary>
    ///   Update a model from the view. (Refresh the view)
    /// </summary>
    /// <param name="model"></param>
    public void CommitView(VoxelColor model)
    {
      if(this.IsDirty)
      {
        if (this._hexDirty)
        {
          this.CommitHex(model);
        }
        else
        {
          this.CommitOther(model);
        }
      }
      else
      {
        this.ForceCommit(model);
      }

      this.Refresh();
      this.RefreshUI();
    }

    private void ForceCommit(VoxelColor model)
    {
      model.Set(
        this.Parse(this._red) / 255f,
        this.Parse(this._green) / 255f,
        this.Parse(this._blue) / 255f
      );
    }

    /// <summary>
    ///   Commit RGB / HSL / A fields to the model.
    /// </summary>
    /// <param name="model"></param>
    private void CommitOther(VoxelColor model)
    {
      if (this._rgbDirty)
      {
        model.Set(
          this.Parse(this._red) / 255f,
          this.Parse(this._green) / 255f,
          this.Parse(this._blue) / 255f
        );

        this.PullHSL(model);
      }
      else if (this._hslDirty)
      {
        model.SetHSL(
          this.Parse(this._hue) / 360f,
          this.Parse(this._saturation) / 100f,
          this.Parse(this._luminosity) / 100f
        );

        this.PullRGB(model);
      }

      if (this._alphaDirty)
      {
        model.A = this.Parse(this._alpha) / 100f;
      }

      this.PullHex(model);
    }

    /// <summary>
    ///   Commit Hex string to the model.
    /// </summary>
    /// <param name="model"></param>
    private void CommitHex(VoxelColor model)
    {
      this.ParseHex(this._hex, model);
      this.PullRGB(model);
      this.PullHSL(model);
      this.PullAlpha(model);
    }

    /// <summary>
    ///   Reset all dirty states.
    /// </summary>
    private void Refresh()
    {
      this._hslDirty = false;
      this._rgbDirty = false;
      this._alphaDirty = false;
      this._hexDirty = false;
    }

    /// <summary>
    ///   Parse a token, an empty value return 0.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private int Parse(String token)
    {
      String toTest = token.Trim();
      if (toTest.Equals("")) return 0;
      else return int.Parse(toTest);
    }

    /// <summary>
    ///   Parse a token, an empty value return 0.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private void ParseHex(String token, VoxelColor result)
    {
      String toTest = token.Trim();
      if (toTest.Equals("")) result.Set(0,0,0,0);
      else
      {
        if (Regex.IsMatch(toTest, "^[0-9ABCDEF]{8}$"))
        {
          GUIColorPicker.FromHex(toTest, result);
        }
        else if (Regex.IsMatch(toTest, "^[0-9ABCDEF]{4}$"))
        {
          GUIColorPicker.FromHex(
            Regex.Replace(toTest, "([0-9ABCDEF])", "${1}0"),
            result
          );
        }
        else
        {
          while (toTest.Length < 8) toTest += 0;
          GUIColorPicker.FromHex(toTest, result);
        }
      }
    }

    /// <summary>
    ///   Check if a token is a valid color field token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private bool IsValid(String token)
    {
      String toTest = token.Trim();
      if (toTest.Equals("")) return true;

      return Regex.IsMatch(toTest, "^\\d*$");
    }

    /// <summary>
    ///   Check bounds of a token.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private String CheckBounds(String token, int min, int max)
    {
      int result = this.Parse(token);
      if (result < min)
      {
        return min.ToString();
      }

      if (result > max)
      {
        return max.ToString();
      }

      return token;
    }

    /// <summary>
    ///   Check if a token is valid for the hex field token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private bool IsValidHex(String token)
    {
      String toTest = token.Trim();
      if (toTest.Equals("")) return true;

      return Regex.IsMatch(toTest, "^[0-9ABCDEF]{0,8}$");
    }
  }
}
