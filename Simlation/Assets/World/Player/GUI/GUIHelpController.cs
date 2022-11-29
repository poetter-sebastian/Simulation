using TMPro;
using UnityEngine;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIHelpController : PopupBehaviour
    {
        public TMP_Text taskText;
        
        public void OnTaskChange(object s, GenEventArgs<string> e)
        {
            taskText.text = e.Value;
        }
    }
}