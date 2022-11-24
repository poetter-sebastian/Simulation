using System;
using System.Collections;
using Player;
using UnityEngine;
using World.Environment;

namespace World.Player.Tasks
{
    public class TaskManager : MonoBehaviour
    {
        public WorldController world;
        public ClimateHandler climate;
        public PlayerHandler player;
        public int taskCounter = 0;

        public Task[] tasks;

        public void Start()
        {
            NextTask();
        }
        
        private void NextTask()
        {
            tasks[taskCounter].ActivateTask(this);
            tasks[taskCounter].TaskComplete += OnTaskComplete;
        }
        
        private void OnTaskComplete(object sender, EventArgs e)
        {
            tasks[taskCounter].Succeeded();
            tasks[taskCounter].DeactivateTask();
            tasks[taskCounter].TaskComplete -= OnTaskComplete;
            taskCounter++;
            NextTask();
        }
    }
}