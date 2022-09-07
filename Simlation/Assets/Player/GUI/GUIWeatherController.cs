using System;
using TMPro;
using UnityEngine;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIWeatherController : MonoBehaviour
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

        public void OnTempChange(GUIEventArgs e)
        {
            tempValue.text = "" + e.Value + " °C";
        }
        public void OnTempFeelChange(GUIEventArgs e)
        {
            tempFeelValue.text = "" + e.Value + " °C";
        }
        public void OnHumChange(GUIEventArgs e)
        {
            humValue.text = "" + e.Value + " %";
        }
        public void OnPressChange(GUIEventArgs e)
        {
            pressValue.text = "" + e.Value + " hPa";
        }
        public void OnWeatherChange(GUIEventArgs e)
        {
            weatherValue.text = "" + e.Value;
        }
        public void OnWindSpChange(GUIEventArgs e)
        {
            windSpValue.text = "" + e.Value + " m/s";
        }
        public void OnWindDirChange(GUIEventArgs e)
        {
            windDirValue.text = "" + e.Value;
        }
        public void OnCloudCoverChange(GUIEventArgs e)
        {
            cloudCoverValue.text = "" + e.Value + " %";
        }
        public void OnRainPosChange(GUIEventArgs e)
        {
            rainPosValue.text = "" + e.Value + " %";
        }
    }
}