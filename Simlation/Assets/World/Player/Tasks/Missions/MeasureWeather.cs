using System;
using Player.GUI;
using UnityEngine.Localization;
using UnityEngine.UI;
using Utility;

namespace World.Player.Tasks.Missions
{
    public class MeasureWeather : Task
    {
        private bool builtWeatherStation = false;
        private bool tookSatellitePictures = false;

        public override string GetTaskName => nameof(MeasureWeather);

        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            
            manager.player.ui.guiPlaceableController.UnlockWeatherButton();
            manager.player.ui.guiPlaceableController.UnlockSatelliteButton();
            
            manager.player.ui.guiMessageController.OnToggle(this, new GenEventArgs<(string, string)>((
                new LocalizedString("Tasks", $"{GetTaskName}Title").GetLocalizedString(),
                new LocalizedString("Tasks", $"{GetTaskName}Message").GetLocalizedString()
            )));
            
            CheckConditions();
            
            manager.player.BuiltWeatherStation += OnWeatherStationBuild;
            manager.player.TookSatellitePicture += OnSatellitePictureTaken;
        }

        public override void Succeeded()
        {
            manager.player.ui.PlaySuccess();
            manager.player.UnlockAridityView();
            manager.player.UnlockHeightView();
            manager.player.UnlockWeatherUI();
        }

        public override void DeactivateTask()
        {
            manager.player.BuiltWeatherStation -= OnWeatherStationBuild;
            manager.player.TookSatellitePicture -= OnSatellitePictureTaken;
        }

        private void OnWeatherStationBuild(object sender, EventArgs e)
        {
            builtWeatherStation = true;
            CheckConditions();
        }

        private void OnSatellitePictureTaken(object sender, EventArgs e)
        {
            tookSatellitePictures = true;
            CheckConditions();
        }
        
        private void CheckConditions()
        {
            var no = new LocalizedString("Tasks", "NotFinished").GetLocalizedString();
            var yes = new LocalizedString("Tasks", "Finished").GetLocalizedString();
            
            var progress = new LocalizedString("Tasks", "MeasureWeatherProgressWeather").GetLocalizedString();
            if (builtWeatherStation)
            {
                progress += "<b>" + yes + "</b> ";
            }
            else
            {
                progress += "<b>" + no + "</b> ";
            }
            progress += "\n" + new LocalizedString("Tasks", "MeasureWeatherProgressSatellite").GetLocalizedString();
            if (tookSatellitePictures)
            {
                progress += "<b>" + yes + "</b> ";
            }
            else
            {
                progress += "<b>" + no + "</b> ";
            }
            
            manager.player.ui.guiTaskController.UpdateProgress(this, new GenEventArgs<string>(progress));
            
            if (builtWeatherStation && tookSatellitePictures)
            {
                TriggerCompletion();
            }
        }
    }
}