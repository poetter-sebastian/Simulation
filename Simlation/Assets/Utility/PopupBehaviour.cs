using System;
using UnityEngine;

namespace Utility
{
    public abstract class PopupBehaviour : MonoBehaviour
    {
        public EventHandler<GenEventArgs<bool>> windowOpens;
        
        public void ToggleWindow()
        {
            windowOpens?.Invoke(this, new GenEventArgs<bool>(gameObject.activeSelf));
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}