using System;
using UnityEngine.Localization;
using UnityEngine.UI;
using Utility;

namespace World.Player.Tasks.Missions
{
    public class ViewGuidance : Task
    {
        public bool openedAridityView = false;
        public bool openedHeightView = false;
        public bool openedTypeView = false;

        public override string GetTaskName => nameof(ViewGuidance);

        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            
            manager.player.ui.guiMessageController.OnToggle(this, new GenEventArgs<(string, string)>((
                new LocalizedString("Tasks", $"{GetTaskName}Title").GetLocalizedString(),
                new LocalizedString("Tasks", $"{GetTaskName}Message").GetLocalizedString()
                )));

            CheckConditions();
            
            manager.player.ui.guiButtonPanelController.aridityViewActivated += OnAridityViewOpens;
            manager.player.ui.guiButtonPanelController.heightViewActivated += OnHeightViewOpens;
            manager.player.ui.guiButtonPanelController.groundTypeiewActivated += OnTypeViewOpens;
        }

        public override void Succeeded()
        {
            manager.player.ui.guiMessageController.OnToggle(this, new GenEventArgs<(string, string)>((
                new LocalizedString("Tasks", "FinishedTitle").GetLocalizedString(),
                new LocalizedString("Tasks", "FinishedMessage").GetLocalizedString()
            )));
            manager.player.ui.guiMessageController.GetComponentInChildren<Button>().onClick.AddListener(manager.player.ui.guiSurveyController.ActivateSurvey);
            
            
            manager.player.ui.guiTaskController.UpdateProgress(this, new GenEventArgs<string>(
                    new LocalizedString("Tasks", "Finished").GetLocalizedString()
                ));
            
            manager.player.ui.PlaySuccess();
        }

        public override void DeactivateTask()
        {
            manager.player.ui.guiButtonPanelController.aridityViewActivated -= OnAridityViewOpens;
            manager.player.ui.guiButtonPanelController.heightViewActivated -= OnHeightViewOpens;
            manager.player.ui.guiButtonPanelController.groundTypeiewActivated -= OnTypeViewOpens;
        }
        
        private void OnAridityViewOpens(object sender, EventArgs e)
        {
            openedAridityView = true;
            CheckConditions();
        }
        
        private void OnHeightViewOpens(object sender, EventArgs e)
        {
            openedHeightView = true;
            CheckConditions();
        }
        
        private void OnTypeViewOpens(object sender, EventArgs e)
        {
            openedTypeView = true;
            CheckConditions();
        }

        private void CheckConditions()
        {
            var no = new LocalizedString("Tasks", "NotFinished").GetLocalizedString();
            var yes = new LocalizedString("Tasks", "Finished").GetLocalizedString();
            
            var progress = new LocalizedString("Tasks", "ViewGuidanceProgressType").GetLocalizedString();
            if (openedTypeView)
            {
                progress += "<b>" + yes + "</b> ";
            }
            else
            {
                progress += "<b>" + no + "</b> ";
            }
            progress += "\n" + new LocalizedString("Tasks", "ViewGuidanceProgressHeight").GetLocalizedString();
            if (openedHeightView)
            {
                progress += "<b>" + yes + "</b> ";
            }
            else
            {
                progress += "<b>" + no + "</b> ";
            }
            progress += "\n" + new LocalizedString("Tasks", "ViewGuidanceProgressArid").GetLocalizedString();
            if (openedAridityView)
            {
                progress += "<b>" + yes + "</b> ";
            }
            else
            {
                progress += "<b>" + no + "</b> ";
            }
            
            manager.player.ui.guiTaskController.UpdateProgress(this, new GenEventArgs<string>(progress));
            
            if (openedTypeView && openedHeightView && openedAridityView)
            {
                TriggerCompletion();
            }
        }
    }
}