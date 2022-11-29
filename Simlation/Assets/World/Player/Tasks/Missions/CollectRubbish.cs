using UnityEngine.Localization;
using Utility;

namespace World.Player.Tasks.Missions
{
    public class CollectRubbish : Task
    {
        private const int MoneyToGet = 500;

        public override string GetTaskName => nameof(CollectRubbish);

        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            manager.player.ui.guiMessageController.OnToggle(this, new GenEventArgs<(string, string)>((
                new LocalizedString("Tasks", $"{GetTaskName}Title").GetLocalizedString(),
                new LocalizedString("Tasks", $"{GetTaskName}Message").GetLocalizedString()
            )));
            manager.player.GotMoney += CheckConditions;
        }

        public override void Succeeded()
        {
            manager.player.ui.PlaySuccess();
        }

        public override void DeactivateTask()
        {
            manager.player.GotMoney -= CheckConditions;
        }

        private void CheckConditions(object sender, GenEventArgs<int> e)
        {
            if (e.Value > MoneyToGet)
            {
                TriggerCompletion();
            }
        }
    }
}