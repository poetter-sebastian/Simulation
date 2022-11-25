using TMPro;
using UnityEngine;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIHelpController : PopupBehaviour
    {
        public TextMeshPro taskText;
        
        public void OnTaskChange(object s, GenEventArgs<string> e)
        {
            taskText.text = e.Value;
        }
    }
}