using System;

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
            
            manager.player.BuildedWeatherStation += OnWeatherStationBuild;
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
            manager.player.BuildedWeatherStation -= OnWeatherStationBuild;
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
            if (builtWeatherStation && tookSatellitePictures)
            {
                TriggerCompletion();
            }
        }
    }
}