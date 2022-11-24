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
        public GUIWeatherController guiWeatherController;
        public GUIResourcesController guiResourcesController;
        public GUIStatisticsController guiStatisticsController;
        public GUIButtonPanelController guiButtonPanelController;
        
        public AudioSource click;
        public AudioSource background;

        public void PlayClick()
        {
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