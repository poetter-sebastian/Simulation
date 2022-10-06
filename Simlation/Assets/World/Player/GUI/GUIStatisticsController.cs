using System;
using TMPro;
using UnityEngine;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIStatisticsController : PopupBehaviour
    {
        public TextMeshProUGUI co2Value;
        public TextMeshProUGUI waterConsumptionValue;
        public TextMeshProUGUI pollutionValue;
        public TextMeshProUGUI approvalValue;
        public TextMeshProUGUI minTempValue;
        public TextMeshProUGUI maxTempValue;

        public void OnCo2Change(GenEventArgs<string> e)
        {
            co2Value.text = "" + e.Value + " g/d";
        }
        public void OnWaterConsumptionChange(GenEventArgs<string> e)
        {
            waterConsumptionValue.text = "" + e.Value + " LN/d";
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