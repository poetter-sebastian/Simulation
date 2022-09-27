using UnityEngine;

namespace Utility
{
    public abstract class PopupBehaviour : MonoBehaviour
    {
        public void ToggleWindow()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}