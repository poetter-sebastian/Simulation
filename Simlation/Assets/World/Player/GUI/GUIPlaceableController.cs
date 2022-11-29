using UnityEngine;
using UnityEngine.UI;

namespace Player.GUI
{
    public class GUIPlaceableController: MonoBehaviour
    {
        public GameObject digButton;
        public GameObject plantButton;
        public GameObject weatherButton;
        public GameObject satelliteButton;

        public void UnlockDigButton()
        {
            digButton.SetActive(true);
        }

        public void UnlockWeatherButton()
        {
            weatherButton.SetActive(true);
        }
        
        public void UnlockSatelliteButton()
        {
            satelliteButton.SetActive(true);
        }

        public void UnlockTreePlanting()
        {
            plantButton.SetActive(true);
        }
    }
}