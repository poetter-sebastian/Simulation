using UnityEngine.Localization;
using Utility;

namespace World.Player.Tasks.Missions
{
    public class TreeMission : Task
    {
        private const int FindIllTrees = 5;
        private const int PlantTrees = 5;

        private int foundIllTrees = 0;
        private int plantTrees = 0;
        private bool illTrees = false;
        private bool plantedTrees = false;

        public override string GetTaskName => nameof(TreeMission);

        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            
            manager.player.ui.guiPlaceableController.UnlockTreePlanting();
            
            manager.player.ui.guiMessageController.OnToggle(this, new GenEventArgs<(string, string)>((
                new LocalizedString("Tasks", $"{GetTaskName}Title").GetLocalizedString(),
                new LocalizedString("Tasks", $"{GetTaskName}Message").GetLocalizedString()
            )));
            
            CheckConditions();
            
            manager.player.FoundIllTrees += OnIllTrees;
            manager.player.PlantedTrees += OnPlantedTrees;
        }
        
        public override void Succeeded()
        {
            manager.player.ui.PlaySuccess();
        }

        public override void DeactivateTask()
        {
            manager.player.FoundIllTrees -= OnIllTrees;
            manager.player.PlantedTrees -= OnPlantedTrees;
        }
        
        private void OnIllTrees(object sender, GenEventArgs<int> e)
        {
            foundIllTrees = e.Value;
            illTrees = foundIllTrees >= FindIllTrees;
            CheckConditions();
        }

        private void OnPlantedTrees(object sender, GenEventArgs<int> e)
        {
            plantTrees = e.Value;
            plantedTrees = plantTrees >= PlantTrees;
            CheckConditions();
        }
        
        private void CheckConditions()
        {
            var progress = new LocalizedString("Tasks", "TreeMissionProgressIll").GetLocalizedString();
            progress += foundIllTrees + "/" + FindIllTrees + " ";
            progress += new LocalizedString("Tasks", "TreeMissionProgressPlant").GetLocalizedString();
            progress += plantTrees + "/" + PlantTrees;
            
            manager.player.ui.guiTaskController.UpdateProgress(this, new GenEventArgs<string>(progress));
            
            if (illTrees && plantedTrees)
            {
                TriggerCompletion();
            }
        }
    }
}