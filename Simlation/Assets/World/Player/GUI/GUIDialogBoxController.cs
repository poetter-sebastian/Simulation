using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIDialogBoxController : PopupBehaviour
    {
        public TMP_Text title;
        public TMP_Text message;
        
        private Action callbackPositive;
        private Action callbackNegative;

        public void OnToggle(object s, GenEventArgs<(string title, string text, Action callPos, Action callNeg)> e)
        {
            gameObject.SetActive(true);
            title.text = e.Value.title;
            message.text = e.Value.text;
            
            callbackPositive = e.Value.callPos;
            callbackNegative = e.Value.callNeg;
        }

        public void Accept()
        {
            callbackPositive?.Invoke();
        }

        public void Decline()
        {
            callbackNegative?.Invoke();
        }
    }
}