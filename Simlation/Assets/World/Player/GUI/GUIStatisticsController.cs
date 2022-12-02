using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIStatisticsController : PopupBehaviour
    {
        public TextMeshProUGUI co2Value;
        public TextMeshProUGUI o2Value;
        public TextMeshProUGUI waterConsumptionValue;
        public TextMeshProUGUI pollutionValue;
        public TextMeshProUGUI approvalValue;
        public TextMeshProUGUI minTempValue;
        public TextMeshProUGUI maxTempValue;
        
        public void OnCo2Change(GenEventArgs<string> e)
        {
            co2Value.text = "" + e.Value + " "+new LocalizedString("Units", "GramPerDay").GetLocalizedString();
        }
        public void OnO2Change(GenEventArgs<string> e)
        {
            o2Value.text = "" + e.Value + " "+new LocalizedString("Units", "KiloGramPerDay").GetLocalizedString();
        }
        public void OnWaterConsumptionChange(GenEventArgs<string> e)
        {
            waterConsumptionValue.text = "" + e.Value + " "+new LocalizedString("Units", "LitersPerDay").GetLocalizedString();
        }
        public void OnPollutionChange(GenEventArgs<string> e)
        {
            pollutionValue.text = "" + e.Value + " %";
        }
        public void OnApprovalChange(GenEventArgs<string> e)
        {
            approvalValue.text = "" + e.Value + " %";
        }
        public void OnMinTempChange(GenEventArgs<string> e)
        {
            minTempValue.text = "" + e.Value + " °C";
        }
        public void OnMaxTempChange(GenEventArgs<string> e)
        {
            maxTempValue.text = "" + e.Value + " °C";
        }
    }
}