using System;
using TMPro;
using UnityEngine;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIViewerController : PopupBehaviour
    {
        public TextMeshProUGUI co2Value;

        public void OnCo2Change(GenEventArgs<string> e)
        {
            co2Value.text = "" + e.Value + " g/d";
        }
    }
}