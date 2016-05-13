using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace org.rnp.gui
{
  [RequireComponent(typeof(Selectable))]
  public class SelectableTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
  {
    [Serializable]
    public class OnSelectEvent : UnityEvent
    { }

    [Serializable]
    public class OnDeselectEvent : UnityEvent
    { }

    public OnSelectEvent OnSelect;
    public OnDeselectEvent OnDeselect;

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
      if(OnDeselect != null)
        OnDeselect.Invoke();
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
      if(OnSelect != null)
        OnSelect.Invoke();
    }
  }
}
