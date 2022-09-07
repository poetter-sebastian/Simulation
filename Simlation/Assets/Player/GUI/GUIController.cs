using System;
using UnityEngine;

namespace Player.GUI
{
    [RequireComponent(typeof(Canvas))]
    public class GUIController : MonoBehaviour
    {
        public GUIWeatherController guiWeatherController;
        public GUIResourcesController guiResourcesController;
    }
    
    public class GUIEventArgs : EventArgs
    {
        public string Value { get; }

        public GUIEventArgs(string value)
        {
            Value = value;
        }
    }
}