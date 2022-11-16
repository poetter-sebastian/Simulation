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
        public AudioSource click;
        public AudioSource background;

        public static void CloseGame()
        {
            Application.Quit();
        }

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
        
        public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
        {
            var startVolume = audioSource.volume;
 
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
 
                yield return null;
            }
 
            audioSource.Stop();
            audioSource.volume = startVolume;
        }
 
        public static IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
        {
            var startVolume = 0.2f;
 
            audioSource.volume = 0;
            audioSource.Play();
 
            while (audioSource.volume < 1.0f)
            {
                audioSource.volume += startVolume * Time.deltaTime / fadeTime;
 
                yield return null;
            }
 
            audioSource.volume = 1f;
        }
    }
}