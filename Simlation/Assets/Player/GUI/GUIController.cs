using System;
using UnityEngine;

namespace Player.GUI
{
    /// <summary>
    /// Main class for GUI controlling
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class GUIController : MonoBehaviour
    {
        public GUIWeatherController guiWeatherController;
        public GUIResourcesController guiResourcesController;
    }
}