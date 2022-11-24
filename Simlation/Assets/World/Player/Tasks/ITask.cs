using System.Collections;
using UnityEngine;
using Utility;

namespace World.Player.Tasks
{
    public interface ITask
    {
        public void ActivateTask(TaskManager manager);
        public void Succeeded();
        public void DeactivateTask();
    }
}