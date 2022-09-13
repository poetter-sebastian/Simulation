using System;
using TMPro;
using UnityEngine;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIResourcesController : MonoBehaviour
    {
        public TextMeshProUGUI monValue;
        public TextMeshProUGUI qualValue;

        public void OnMoneyChange(GenEventArgs<string> e)
        {
            monValue.text = "" + e.Value + " ¤";
        }
        public void OnQualityChange(GenEventArgs<string> e)
        {
            qualValue.text = "" + e.Value + " °C";
        }
    }
}