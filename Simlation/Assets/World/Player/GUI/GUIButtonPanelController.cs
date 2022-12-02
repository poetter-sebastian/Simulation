using System;
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

        public event EventHandler textureViewActivated;
        public event EventHandler heightViewActivated; 
        public event EventHandler aridityViewActivated; 
        public event EventHandler groundTypeiewActivated; 

        public void ActivateTextureButton()
        {
            textureViewButton.gameObject.SetActive(true);
            textureViewActivated?.Invoke(this, EventArgs.Empty);
        }
        
        public void ActivateHeightButton()
        {
            heightView.gameObject.SetActive(true);
            heightViewActivated?.Invoke(this, EventArgs.Empty);
        }
        
        public void ActivateAridityButton()
        {
            aridityView.gameObject.SetActive(true);
            aridityViewActivated?.Invoke(this, EventArgs.Empty);
        }
        
        public void ActivateGroundButton()
        {
            groundTypeView.gameObject.SetActive(true);
            groundTypeiewActivated?.Invoke(this, EventArgs.Empty);
        }
    }
}