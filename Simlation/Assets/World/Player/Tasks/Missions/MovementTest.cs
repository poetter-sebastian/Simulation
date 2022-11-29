using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using Utility;

namespace World.Player.Tasks.Missions
{
    public class MovementTest : Task
    {
        public bool usedW = false;
        public bool usedA = false;
        public bool usedS = false;
        public bool usedD = false;
        
        public bool usedScrollWheel = false;
         
        public override string GetTaskName => nameof(MeasureWeather);

        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            //
            manager.player.ui.guiMessageController.OnToggle(this, new GenEventArgs<(string, string)>((
                new LocalizedString("Tasks", "WelcomeTitle").GetLocalizedString(),
                new LocalizedString("Tasks", "WelcomeMessage").GetLocalizedString()
                )));
            //
            manager.player.movement.CallW += CheckUsedW;
            manager.player.movement.CallA += CheckUsedA;
            manager.player.movement.CallS += CheckUsedS;
            manager.player.movement.CallD += CheckUsedD;
            manager.player.movement.CallRotation += CheckUsedScrollWheel;
            //Because of the first camera movement and reset flags
            StartCoroutine(ResetBool());
        }

        public override void Succeeded()
        {
            manager.player.ui.PlaySuccess();
        }

        public override void DeactivateTask()
        {
            manager.player.movement.CallW -= CheckUsedW;
            manager.player.movement.CallA -= CheckUsedA;
            manager.player.movement.CallS -= CheckUsedS;
            manager.player.movement.CallD -= CheckUsedD;
            manager.player.movement.CallRotation -= CheckUsedScrollWheel;
        }
        
        private IEnumerator ResetBool()
        {
            yield return new WaitForSeconds(1);
            usedW = false;
            usedA = false;
            usedS = false;
            usedD = false;
            yield return null;
        }
        
        private void CheckUsedW(object sender, GenEventArgs<bool> e)
        {
            usedW = e.Value;
            CheckConditions();
        }
        
        private void CheckUsedA(object sender, GenEventArgs<bool> e)
        {
            usedA = e.Value;
            CheckConditions();
        }
        
        private void CheckUsedS(object sender, GenEventArgs<bool> e)
        {
            usedS = e.Value;
            CheckConditions();
        }
        
        private void CheckUsedD(object sender, GenEventArgs<bool> e)
        {
            usedD = e.Value;
            CheckConditions();
        }
        
        private void CheckUsedScrollWheel(object sender, GenEventArgs<bool> e)
        {
            usedScrollWheel = e.Value;
            CheckConditions();
        }

        private void CheckConditions()
        {
            if (usedW && usedA && usedS && usedD && usedScrollWheel)
            {
                TriggerCompletion();
            }
        }
    }
}