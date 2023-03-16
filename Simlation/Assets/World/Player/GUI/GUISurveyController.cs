using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;
using Utility;
using Toggle = UnityEngine.UIElements.Toggle;

namespace Player.GUI
{
    public class GUISurveyController : PopupBehaviour
    {
        public GUIDialogBoxController guiDialogBoxController;
        
        public CanvasRenderer[] windows;
        public GameObject over18Container;
        public TMP_InputField ImagineSimulationValue;
        public GameObject notOver18Container;
        public Button over18ContinueButton;
        public GameObject knowGamificationContainer;
        public GameObject notKnowGamificationContainer;

        public bool knowGamification;
        public string imagineGamification;
        public int ageArea;
        public string opinionToApp;
        public int teachingScore;
        public int funScore;
        public int systemRequirementsScore;
        public int fancyGraphicScore;
        public int realisticSimulationScore;
        public int nonRealisticSimulationScore;
        public bool tooEasy;
        public bool easyUI;
        public bool shareHardware;
        public bool shareLogs;

        /// <summary>
        /// Window count for the active window in UI
        /// </summary>
        private int wc;

        public void ActivateSurvey()
        {
            gameObject.SetActive(true);
        }

        public void OpenDialogBoxClose()
        {
            guiDialogBoxController.OnToggle(this, new GenEventArgs<(string title, string text, Action callPos, Action callNeg)>((
                new LocalizedString("UI", "Simulation/Player/GUI/YesNoBoxShadow/SurveyQuestion").GetLocalizedString(),
                new LocalizedString("UI", "Simulation/Player/GUI/YesNoBoxShadow/SurveyDesc").GetLocalizedString(),
                ToggleWindow,
                null
            )));
        }

        public void NextWindow()
        {
            windows[wc % windows.Length].gameObject.SetActive(false);
            wc++;
            windows[wc % windows.Length].gameObject.SetActive(true);
        }

        public void PreviousWindows()
        {
            windows[wc % windows.Length].gameObject.SetActive(false);
            wc--;
            windows[wc % windows.Length].gameObject.SetActive(true);
        }

        public void Over18()
        {
            over18ContinueButton.gameObject.SetActive(true);
            over18Container.SetActive(true);
            
            notOver18Container.SetActive(false);
        }
        
        public void NotOver18()
        {
            over18ContinueButton.gameObject.SetActive(false);
            over18Container.SetActive(false);
            
            notOver18Container.SetActive(true);
        }

        public void KnowGamification()
        {
            knowGamificationContainer.SetActive(true);
            notKnowGamificationContainer.SetActive(false);
            knowGamification = true;
            imagineGamification = ImagineSimulationValue.text;
        }
        
        public void NotKnowGamification()
        {
            knowGamificationContainer.SetActive(false);
            notKnowGamificationContainer.SetActive(true);
            knowGamification = false;
            imagineGamification = "";
        }
        
        public void SetImagineGamification(TMP_InputField s)
        {
            imagineGamification = s.text;
        }

        public void SetAgeArea(TMP_Dropdown d)
        {
            ageArea = d.value;
        }

        public void SetOpinionToApp(TMP_InputField s)
        {
            opinionToApp = s.text;
        }

        public void SetTeaching(int i)
        {
            teachingScore = i;
        }
        
        public void SetFun(int i)
        {
            funScore = i;
        }
        
        public void SetSystemRequirements(int i)
        {
            systemRequirementsScore = i;
        }
        
        public void SetFancyGraphics(int i)
        {
            fancyGraphicScore = i;
        }
        
        public void SetRealisticSimulation(int i)
        {
            realisticSimulationScore = i;
        }
        
        public void SetNonRealisticSimulation(int i)
        {
            nonRealisticSimulationScore = i;
        }

        public void EasyUI()
        {
            easyUI = true;
        }
        
        public void NotEasyUI()
        {
            easyUI = false;
        }
        
        public void TooEasy()
        {
            tooEasy = true;
        }
        
        public void NotTooEasy()
        {
            tooEasy = false;
        }
        
        public void ShareHardware()
        {
            shareHardware = true;
        }
        
        public void ShareNoHardware()
        {
            shareHardware = false;
        }
        
        public void ShareLogs()
        {
            shareLogs = true;
        }
        
        public void ShareNoLogs()
        {
            shareLogs = false;
        }
    }
}