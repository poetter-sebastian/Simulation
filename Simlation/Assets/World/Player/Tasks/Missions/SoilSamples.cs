using Utility;

namespace World.Player.Tasks.Missions
{
    public class SoilSamples : Task
    {
        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            manager.player.TookSoilExample += CheckConditions;
        }

        public void CheckConditions(object sender, GenEventArgs<int> e)
        {
            if (e.Value > 5)
            {
                
                TriggerCompletion();
            }
        }

        public override void Succeeded()
        {
            manager.player.UnlockGroundTypeView();
        }

        public override void DeactivateTask()
        {
            manager.player.TookSoilExample -= CheckConditions;
        }
    }
}