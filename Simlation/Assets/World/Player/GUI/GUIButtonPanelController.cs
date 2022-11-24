using UnityEngine;
using UnityEngine.UI;

namespace Player.GUI
{
    public class GUIButtonPanelController: MonoBehaviour
    {
        public Button textureViewButton;
        public Button heightView;
        public Button aridityView;
        public Button groundTypeView;

        public void ActivateTextureButton()
        {
            textureViewButton.gameObject.SetActive(true);
        }
        
        public void ActivateHeightButton()
        {
            heightView.gameObject.SetActive(true);
        }
        
        public void ActivateAridityButton()
        {
            aridityView.gameObject.SetActive(true);
        }
        
        public void ActivateGroundButton()
        {
            groundTypeView.gameObject.SetActive(true);
        }
    }
}