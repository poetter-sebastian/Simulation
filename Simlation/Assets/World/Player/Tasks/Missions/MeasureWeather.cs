using System;
using Utility;

namespace World.Player.Tasks.Missions
{
    public class MeasureWeather : Task
    {
        private bool builtWeatherStation = false;
        private bool tookSatellitePictures = false;
        
        public override void ActivateTask(TaskManager manager)
        {
            this.manager = manager;
            manager.player.BuildedWeatherStation += OnWeatherStationBuild;
            manager.player.TookSatellitePicture += OnSatellitePictureTaken;
        }

        public void OnWeatherStationBuild(object sender, EventArgs e)
        {
            builtWeatherStation = true;
            CheckConditions();
        }

        public void OnSatellitePictureTaken(object sender, EventArgs e)
        {
            tookSatellitePictures = true;
            CheckConditions();
        }
        
        public override void Succeeded()
        {
            manager.player.UnlockAridityView();
            manager.player.UnlockHeightView();
        }

        public override void DeactivateTask()
        {
            manager.player.BuildedWeatherStation -= OnWeatherStationBuild;
            manager.player.TookSatellitePicture -= OnSatellitePictureTaken;
        }
        
        private void CheckConditions()
        {
            if (builtWeatherStation && tookSatellitePictures)
            {
                TriggerCompletion();
            }
        }
    }
}