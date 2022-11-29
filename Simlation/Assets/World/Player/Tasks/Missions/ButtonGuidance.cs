using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using Utility;

namespace World.Player.Tasks.Missions
{
    public class ButtonGuidance : Task
    {
        public bool openedHelpUI = false;
        public bool openedStatisticsUI = false;

        public override string GetTaskName => nameof(ButtonGuidance);

        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            //
            manager.player.ui.guiMessageController.OnToggle(this, new GenEventArgs<(string, string)>((
                new LocalizedString("Tasks", $"{GetTaskName}Title").GetLocalizedString(),
                new LocalizedString("Tasks", $"{GetTaskName}Message").GetLocalizedString()
                )));
            //
            manager.player.ui.guiHelpController.windowOpens += OnHelpWindowOpens;
            manager.player.ui.guiStatisticsController.windowOpens += OnStatisticsWindowOpens;
        }

        public override void Succeeded()
        {
            manager.player.ui.PlaySuccess();
        }

        public override void DeactivateTask()
        {
            manager.player.ui.guiHelpController.windowOpens -= OnHelpWindowOpens;
            manager.player.ui.guiStatisticsController.windowOpens -= OnStatisticsWindowOpens;
        }
        
        private void OnHelpWindowOpens(object sender, GenEventArgs<bool> e)
        {
            openedHelpUI = true;
            CheckConditions();
        }

        private void OnStatisticsWindowOpens(object sender, GenEventArgs<bool> e)
        {
            openedStatisticsUI = true;
            CheckConditions();
        }

        private void CheckConditions()
        {
            if (openedHelpUI && openedStatisticsUI)
            {
                TriggerCompletion();
            }
        }
    }
}