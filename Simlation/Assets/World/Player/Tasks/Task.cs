using System;
using UnityEngine;
using UnityEngine.Localization;

namespace World.Player.Tasks
{
    [Serializable]
    public abstract class Task : MonoBehaviour, ITask
    {
        public event EventHandler TaskComplete;

        protected TaskManager manager;

        public abstract string GetTaskName { get; }

        public abstract void ActivateTask(TaskManager taskManager);
        
        public abstract void Succeeded();

        public abstract void DeactivateTask();

        public string GetTitle()
        {
            return new LocalizedString("Tasks", $"{GetTaskName}Title").GetLocalizedString();
        }

        public string GetDescription()
        {
            return new LocalizedString("Tasks", $"{GetTaskName}Desc").GetLocalizedString();
        }

        public void TriggerCompletion()
        {
            TaskComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}