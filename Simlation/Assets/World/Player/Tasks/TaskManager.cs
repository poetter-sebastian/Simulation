using System;
using System.Collections;
using Player;
using UnityEngine;
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
        
        private void NextTask()
        {
            ILog.L(LN, "Next Task started!");
            ILog.L(LN, "Task: " + tasks[taskCounter].title);
            tasks[taskCounter].ActivateTask(this);
            tasks[taskCounter].TaskComplete += OnTaskComplete;
        }
        
        private void OnTaskComplete(object sender, EventArgs e)
        {
            ILog.L(LN, "Task finished!");
            ILog.L(LN, "Task: " + tasks[taskCounter].title);
            tasks[taskCounter].Succeeded();
            tasks[taskCounter].DeactivateTask();
            tasks[taskCounter].TaskComplete -= OnTaskComplete;
            taskCounter++;
            NextTask();
        }
    }
}