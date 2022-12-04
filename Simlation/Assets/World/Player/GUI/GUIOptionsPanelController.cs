using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Utility;

namespace Player.GUI
{
    public class GUIOptionsPanelController: MonoBehaviour
    {
        public GUIDialogBoxController guiDialogBoxController;
        
        public Button weatherButton;

        public void ActivateWeatherButton()
        {
            weatherButton.gameObject.SetActive(true);
        }

        public void CloseApp()
        {
            guiDialogBoxController.OnToggle(this, new GenEventArgs<(string title, string text, Action callPos, Action callNeg)>((
                new LocalizedString("UI", "Simulation/Player/GUI/YesNoBoxShadow/CloseAppQuestion").GetLocalizedString(),
                new LocalizedString("UI", "Simulation/Player/GUI/YesNoBoxShadow/CloseAppDesc").GetLocalizedString(),
                Application.Quit,
                null
            )));
        }
    }
}