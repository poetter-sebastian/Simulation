using UnityEngine.Localization;
using Utility;

namespace World.Player.Tasks.Missions
{
    public class SoilSamples : Task
    {
        private const int sampleCount = 10;
            
        public override string GetTaskName => nameof(SoilSamples);
        
        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            
            manager.player.ui.guiMessageController.OnToggle(this, new GenEventArgs<(string, string)>((
                new LocalizedString("Tasks", $"{GetTaskName}Title").GetLocalizedString(),
                new LocalizedString("Tasks", $"{GetTaskName}Message").GetLocalizedString()
            )));
            
            CheckConditions(this, new GenEventArgs<int>(0));
            
            manager.player.ui.guiPlaceableController.UnlockDigButton();
            
            manager.player.TookSoilExample += CheckConditions;
        }

        public override void Succeeded()
        {
            manager.player.ui.PlaySuccess();
            manager.player.UnlockGroundTypeView();
            manager.player.UnlockTextureView();
            manager.player.ui.UnlockButtonPanel();
        }

        public override void DeactivateTask()
        {
            manager.player.TookSoilExample -= CheckConditions;
        }
        
        private void CheckConditions(object sender, GenEventArgs<int> e)
        {
            var progress = new LocalizedString("Tasks", "SoilSampleProgress").GetLocalizedString();
            progress += e.Value + "/" + sampleCount;
            manager.player.ui.guiTaskController.UpdateProgress(this, new GenEventArgs<string>(progress)
            );
            
            if (e.Value >= sampleCount)
            {
                TriggerCompletion();
            }
        }
    }
}