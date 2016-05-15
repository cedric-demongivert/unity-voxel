using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.gui.colorPalette
{
  public class PainterColorPaletteItem : ColorPaletteItem
  {
    public void Push()
    {
      if(this.Parent != null && this.Parent is PainterColorPalette)
      {
        PainterColorPalette parent = this.Parent as PainterColorPalette;
        parent.PushColor(this.Color);
      }
    }
  }
}
