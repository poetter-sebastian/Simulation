using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIViewerController : PopupBehaviour
    {
        public TextMeshProUGUI nameValue;
        public TextMeshProUGUI waterValue;
        public TextMeshProUGUI co2Value;
        public TextMeshProUGUI o2Value;
        public TextMeshProUGUI diseaseValue;

        public void OnNameChange(GenEventArgs<string> e)
        {
            nameValue.text = "" + e.Value + "";
        }
        
        public void OnWaterChange(GenEventArgs<string> e)
        {
            waterValue.text = "" + e.Value + " "+new LocalizedString("Units", "LitersPerDay").GetLocalizedString();
        }
        
        public void OnCo2Change(GenEventArgs<string> e)
        {
            co2Value.text = "" + e.Value + " "+new LocalizedString("Units", "GramPerDay").GetLocalizedString();
        }        
        
        public void OnO2Change(GenEventArgs<string> e)
        {
            o2Value.text = "" + e.Value + " "+new LocalizedString("Units", "GramPerDay").GetLocalizedString();
        }        
        
        public void OnDiseaseChange(GenEventArgs<string> e)
        {
            diseaseValue.text = "" + e.Value + "";
        }
    }
}