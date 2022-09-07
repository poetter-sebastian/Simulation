using System;
using TMPro;
using UnityEngine;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIResourcesController : MonoBehaviour
    {
        public TextMeshProUGUI monValue;
        public TextMeshProUGUI qualValue;

        public void OnMoneyChange(GUIEventArgs e)
        {
            monValue.text = "" + e.Value + " °C";
        }
        public void OnQualityChange(GUIEventArgs e)
        {
            qualValue.text = "" + e.Value + " °C";
        }
    }
}