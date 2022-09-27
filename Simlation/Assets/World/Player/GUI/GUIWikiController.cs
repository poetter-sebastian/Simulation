using System;
using TMPro;
using UnityEngine;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIWikiController : MonoBehaviour
    {
        public TextMeshProUGUI monValue;

        public void OnMoneyChange(GenEventArgs<string> e)
        {
            monValue.text = "" + e.Value + " ¤";
        }
    }
}