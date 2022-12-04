using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUITaskController : MonoBehaviour
    {
        public TMP_Text taskCount;
        public TMP_Text taskTitle;
        public TMP_Text progress;
        
        public void OnTaskChange(object s, GenEventArgs<(string, string)> e)
        {
            taskCount.text = e.Value.Item1;
            taskTitle.text = e.Value.Item2;
        }

        public void UpdateProgress(object s, GenEventArgs<string> e)
        {
            progress.text = e.Value;
        }
    }
}