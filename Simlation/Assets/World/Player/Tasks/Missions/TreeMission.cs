using Utility;

namespace World.Player.Tasks.Missions
{
    public class TreeMission : Task
    {
        private const int FindIllTrees = 5;
        private const int PlantTrees = 5;
        
        private bool illTrees = false;
        private bool plantedTrees = false;

        public override string GetTaskName => nameof(TreeMission);

        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
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
            illTrees = e.Value > FindIllTrees;
            CheckConditions();
        }
        
        private void OnPlantedTrees(object sender, GenEventArgs<int> e)
        {
            plantedTrees = e.Value > PlantTrees;
            CheckConditions();
        }
        
        private void CheckConditions()
        {
            if (illTrees && plantedTrees)
            {
                TriggerCompletion();
            }
        }
    }
}