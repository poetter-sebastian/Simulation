using System;
using NaughtyAttributes;
using Player;
using UnityEngine;
using UnityEngine.Localization;
using Utility;
using World.Environment;

namespace World.Player.Tasks
{
    public class TaskManager : MonoBehaviour, ILog
    {
        public WorldController world;
        public ClimateHandler climate;
        public PlayerHandler player;
        public int taskCounter = 0;

        public Task[] tasks;

        public string LN()
        {
            return "Task Manager";
        }
        
        public void Start()
        {
            NextTask();
        }
        
        [Button("TriggerNextTask")]
        public void TriggerNextTask()
        {
            if (taskCounter > tasks.Length)
            {
                return;
            }
            tasks[taskCounter].TriggerCompletion();
        }
        
        private void NextTask()
        {
            if (taskCounter >= tasks.Length)
            {
                return;
            }
            ILog.L(LN, "Next Task started!");
            ILog.L(LN, "Task: " + tasks[taskCounter].GetTitle());
            tasks[taskCounter].ActivateTask(this);
            tasks[taskCounter].TaskComplete += OnTaskComplete;
            player.ui.guiHelpController.OnTaskChange(this, new GenEventArgs<string>(tasks[taskCounter].GetDescription()));

            var taskText = new LocalizedString("UI", "Simulation/Player/GUI/Task/Title_1").GetLocalizedString()
                + " " + (taskCounter + 1) + " " + new LocalizedString("UI", "Simulation/Player/GUI/Task/Title_2").GetLocalizedString()
                + " " + tasks.Length;
            
            player.ui.guiTaskController.OnTaskChange(this, new GenEventArgs<(string, string)>((taskText, tasks[taskCounter].GetTitle())));
        }
        
        private void OnTaskComplete(object sender, EventArgs e)
        {
            ILog.L(LN, "Task finished!");
            ILog.L(LN, "Task: " + tasks[taskCounter].GetTitle());
            tasks[taskCounter].Succeeded();
            tasks[taskCounter].DeactivateTask();
            tasks[taskCounter].TaskComplete -= OnTaskComplete;
            taskCounter++;
            NextTask();
        }
    }
}