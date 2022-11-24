using UnityEngine;
using UnityEngine.UI;

namespace Player.GUI
{
    public class GUIOptionsPanelController: MonoBehaviour
    {
        public Button weatherButton;

        public void ActivateWeatherButton()
        {
            weatherButton.gameObject.SetActive(true);
        }
    }
}