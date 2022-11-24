using System;
using Utility;

namespace World.Player.Tasks.Missions
{
    public class CollectRubbish : Task
    {
        private const int MoneyToGet = 500;
        
        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            manager.player.GotMoney += CheckConditions;
        }

        private void CheckConditions(object sender, GenEventArgs<int> e)
        {
            if (e.Value > MoneyToGet)
            {
                TriggerCompletion();
            }
        }

        public override void Succeeded()
        {
            //not used
        }

        public override void DeactivateTask()
        {
            manager.player.GotMoney -= CheckConditions;
        }
    }
}