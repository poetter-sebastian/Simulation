namespace World.Player.Tasks
{
    public interface ITask
    {
        public void ActivateTask(TaskManager manager);
        public void Succeeded();
        public void DeactivateTask();
        public string GetTitle();
        public string GetDescription();
        public void TriggerCompletion();
    }
}