using System;
using TMPro;
using UnityEngine;
using Utility;

namespace Player.GUI
{
    /// <summary>
    /// Controller for the GUI elements of the popup for weather data. The whole 
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIWeatherController : PopupBehaviour
    {
        public TextMeshProUGUI tempValue;
        public TextMeshProUGUI tempFeelValue;
        public TextMeshProUGUI humValue;
        public TextMeshProUGUI pressValue;
        public TextMeshProUGUI weatherValue;
        public TextMeshProUGUI windSpValue;
        public TextMeshProUGUI windDirValue;
        public TextMeshProUGUI cloudCoverValue;
        public TextMeshProUGUI rainPosValue;

        public void OnTempChange(GenEventArgs<string> e)
        {
            tempValue.text = "" + e.Value + " °C";
        }
        public void OnTempFeelChange(GenEventArgs<string> e)
        {
            tempFeelValue.text = "" + e.Value + " °C";
        }
        public void OnHumChange(GenEventArgs<string> e)
        {
            humValue.text = "" + e.Value + " %";
        }
        public void OnPressChange(GenEventArgs<string> e)
        {
            pressValue.text = "" + e.Value + " hPa";
        }
        public void OnWeatherChange(GenEventArgs<string> e)
        {
            weatherValue.text = "" + e.Value;
        }
        public void OnWindSpChange(GenEventArgs<string> e)
        {
            windSpValue.text = "" + e.Value + " km/h";
        }
        public void OnWindDirChange(GenEventArgs<string> e)
        {
            windDirValue.text = "" + e.Value;
        }
        public void OnCloudCoverChange(GenEventArgs<string> e)
        {
            cloudCoverValue.text = "" + e.Value + " %";
        }
        public void OnRainPosChange(GenEventArgs<string> e)
        {
            rainPosValue.text = "" + e.Value + " %";
        }
    }
}