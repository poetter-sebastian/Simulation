using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Utility;
using World.Environment;

namespace Player.GUI
{
    public class GUILegendController : PopupBehaviour
    {
        public Texture2D aridityLegend;
        public Texture2D typeLegend;
        public Texture2D heightLegend;

        public RawImage activeLegend;
        public TMP_Text description;

        public void ShowAridLegend()
        {
            activeLegend.texture = aridityLegend;
            description.text = new LocalizedString("UI", "Simulation/Player/GUI/Legend/Arid").GetLocalizedString();
            gameObject.SetActive(true);
        }

        public void ShowTypeLegend()
        {
            activeLegend.texture = typeLegend;
            description.text = new LocalizedString("UI", "Simulation/Player/GUI/Legend/GroundType").GetLocalizedString();
            gameObject.SetActive(true);
        }
        
        public void ShowHeightLegend()
        {
            activeLegend.texture = heightLegend;
            description.text = new LocalizedString("UI", "Simulation/Player/GUI/Legend/Height").GetLocalizedString();
            gameObject.SetActive(true);
        }
    }
}