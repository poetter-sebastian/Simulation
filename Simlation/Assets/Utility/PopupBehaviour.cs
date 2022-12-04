using System;
using UnityEngine;

namespace Utility
{
    public abstract class PopupBehaviour : MonoBehaviour
    {
        public event EventHandler<GenEventArgs<bool>> windowOpens;
        public event EventHandler<GenEventArgs<bool>> windowClosed;
        
        public void ToggleWindow()
        {
            if (gameObject.activeSelf)
            {
                windowClosed?.Invoke(this, new GenEventArgs<bool>(false));
            }
            else
            {
                windowOpens?.Invoke(this, new GenEventArgs<bool>(true));
            }
            gameObject.SetActive(!gameObject.activeSelf);
        }
        
        public void CloseWindow()
        {
            gameObject.SetActive(false);
            windowClosed?.Invoke(this, new GenEventArgs<bool>(false));
        }
        
        public void OpenWindow()
        {
            gameObject.SetActive(true);
            windowOpens?.Invoke(this, new GenEventArgs<bool>(true));
        }
    }
}