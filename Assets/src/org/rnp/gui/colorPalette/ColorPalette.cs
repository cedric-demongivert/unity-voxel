using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.gui.colorPalette
{
  public class ColorPalette : MonoBehaviour
  {
    [SerializeField]
    private GameObject _palette = null;

    [SerializeField]
    private GameObject _colorItem = null;

    [SerializeField]
    private Dictionary<Color32, GameObject> _colors = new Dictionary<Color32, GameObject>();

    public IEnumerable<Color32> Colors
    {
      get
      {
        foreach(Color32 color in new List<Color32>(this._colors.Keys))
        {
          yield return color;
        }
      }
    }

    public int Count
    {
      get
      {
        return this._colors.Count;
      }
    }

    public bool IsEmpty()
    {
      return this._colors.Count == 0;
    }

    /// <summary>
    ///   Object that hold palette color entities.
    /// </summary>
    public GameObject Palette
    {
      get
      {
        return this.GetPalette();
      }
      set
      {
        this.SetPalette(value);
      }
    }

    /// <summary>
    ///   Color prefab to instanciate for each color of the palette.
    /// </summary>
    public GameObject ColorItem
    {
      get
      {
        return this.GetColorItem();
      }
      set
      {
        this.SetColorItem(value);
      }
    }

    public void SetColorItem(GameObject value)
    {
      if (value == this._colorItem) return;

      if(value == null || value.GetComponentInChildren<ColorPaletteItem>() != null)
      {
        this._colorItem = value;

        foreach(Color32 key in new List<Color32>(this._colors.Keys))
        {
          this.RemoveColor(key);
          this.AddColor(key);
        }
      }
      else
      {
        throw new Exception("The ColorItem Game Object must contains a ColorPaletteItem component.");
      }
    }

    public GameObject GetColorItem()
    {
      return this._colorItem;
    }

    public void SetPalette(GameObject value)
    {
      if (value == this._palette) return;

      this._palette = value;

      foreach(GameObject item in this._colors.Values)
      {
        item.transform.SetParent((this._palette != null) ? this._palette.transform : null);
      }
    }

    public GameObject GetPalette()
    {
      return this._palette;
    }

    /// <summary>
    ///   Return an ColorPaletteItem attached for a specific color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private ColorPaletteItem GetColorItem(Color32 color)
    {
      if (this._colors.ContainsKey(color))
      {
        return this._colors[color].GetComponentInChildren<ColorPaletteItem>();
      }
      else
      {
        return null;
      }
    }
    
    public void Clear()
    {
      foreach(Color32 key in new List<Color32>(this._colors.Keys))
      {
        this.RemoveColor(key);
      }
    }
    
    public void AddColor(Color32 color)
    {
      if(!this.HasColor(color))
      {
        GameObject child = Instantiate(this.ColorItem);
        child.transform.SetParent((this.Palette != null) ? this.Palette.transform : null);

        this._colors[color] = child;

        this.GetColorItem(color).InitializeColorItem(this, color);
      }
    }

    public void RemoveColor(Color32 color)
    {
      if (this.HasColor(color))
      {
        GameObject toDetach = this._colors[color];
        this._colors.Remove(color);

        Destroy(toDetach);
      }
    }

    public void ChangeColor(Color32 initial, Color32 final)
    {
      if(this.HasColor(initial))
      {
        if(this.HasColor(final))
        {
          this.RemoveColor(final);
        }

        this._colors[final] = this._colors[initial];
        this._colors.Remove(initial);

        this.GetColorItem(final).SetColor(final);
      }
    }

    public bool HasColor(Color32 color)
    {
      return this._colors.ContainsKey(color);
    }

  }
}
