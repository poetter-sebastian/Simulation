using Utility;

namespace World.Player.Tasks.Missions
{
    public class TreeMission : Task
    {
        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            manager.player.FoundIllTrees += OnIllTrees;
            manager.player.PlantedTrees += OnPlantedTrees;
        }

        private const int FindIllTrees = 5;
        private const int PlantTrees = 5;
        
        private bool illTrees = false;
        private bool plantedTrees = false;

        public void OnIllTrees(object sender, GenEventArgs<int> e)
        {
            illTrees = e.Value > FindIllTrees;
            CheckConditions();
        }
        public void OnPlantedTrees(object sender, GenEventArgs<int> e)
        {
            plantedTrees = e.Value > PlantTrees;
            CheckConditions();
        }

        public override void Succeeded()
        {
            //not used
        }

        public override void DeactivateTask()
        {
            manager.player.FoundIllTrees -= OnIllTrees;
            manager.player.PlantedTrees -= OnPlantedTrees;
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