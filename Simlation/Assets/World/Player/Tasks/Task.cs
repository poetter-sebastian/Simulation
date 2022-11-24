using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace World.Player.Tasks
{
    [Serializable]
    public abstract class Task : MonoBehaviour, ITask
    {
        public string title = "";
        public string description = "";
        public event EventHandler TaskComplete;

        protected TaskManager manager;

        public abstract void ActivateTask(TaskManager taskManager);
        
        public abstract void Succeeded();

        public abstract void DeactivateTask();

        public void TriggerCompletion()
        {
            TaskComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}