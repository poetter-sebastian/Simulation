using System;
using System.Collections;
using UnityEngine;

namespace Player.GUI
{
    /// <summary>
    /// Main class for GUI controlling
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class GUIController : MonoBehaviour
    {
        [Header("UI Controller")]
        public GUIWeatherController guiWeatherController;
        public GUIResourcesController guiResourcesController;
        public GUIStatisticsController guiStatisticsController;
        public GUIButtonPanelController guiButtonPanelController;
        public GUIWikiController guiWikiController;
        public GUIHelpController guiHelpController;
        public GUITaskController guiTaskController;
        public GUIOptionsPanelController guiOptionsController;
        public GUIMessageController guiMessageController;
        public GUIPlaceableController guiPlaceableController;

        [Header("Audio Sources")]
        public AudioSource click;
        public AudioSource background;
        public AudioClip successSound;
        public AudioClip clickSound;
        public AudioClip diggingSound;
        public AudioClip placeSound;
        

        public void PlayDigging()
        {
            click.clip = diggingSound;
            click.Play();
        }
        
        public void PlayPlace()
        {
            click.clip = placeSound;
            click.Play();
        }
        
        public void PlaySuccess()
        {
            click.clip = successSound;
            click.Play();
        }
        
        public void PlayClick()
        {
            click.clip = clickSound;
            click.Play();
        }

        public void PlayBackgroundSound(AudioClip clip)
        {
            background.Stop();
            background.clip = clip;
            background.Play();
        }

        public void UnlockButtonPanel()
        {
            guiButtonPanelController.gameObject.SetActive(true);
        }
        
        public static void CloseGame()
        {
            Application.Quit();
        }
    }
}