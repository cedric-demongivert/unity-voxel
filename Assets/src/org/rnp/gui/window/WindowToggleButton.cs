using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace org.rnp.gui.window
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  /// 
  /// </summary>
  [ExecuteInEditMode]
  public class WindowToggleButton : MonoBehaviour
  {
    public Image Maximize;
    public Image Minimize;
    public Window ParentWindow;

    public void Toggle()
    {
      if(this.ParentWindow != null) this.ParentWindow.Toggle();
      this.UpdateImageState();
    }

    public void Update()
    {
      this.UpdateImageState();
    }

    public void Awake()
    {
      this.UpdateImageState();
    }

    private void UpdateImageState()
    {
      if (this.Maximize != null) this.Maximize.gameObject.SetActive(this.ParentWindow != null && this.ParentWindow.Minimized);
      if (this.Minimize != null) this.Minimize.gameObject.SetActive(this.ParentWindow != null && !this.ParentWindow.Minimized);
    }
  }
}
