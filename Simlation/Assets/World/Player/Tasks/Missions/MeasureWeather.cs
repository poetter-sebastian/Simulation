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
            
            manager.player.BuiltWeatherStation += OnWeatherStationBuild;
            manager.player.TookSatellitePicture += OnSatellitePictureTaken;
        }

        public override void Succeeded()
        {
            manager.player.ui.PlaySuccess();
            manager.player.UnlockAridityView();
            manager.player.UnlockHeightView();
            manager.player.UnlockWeatherUI();
            
            manager.player.ui.guiMessageController.OnToggle(this, new GenEventArgs<(string, string)>((
                new LocalizedString("Tasks", "FinishedTitle").GetLocalizedString(),
                new LocalizedString("Tasks", "FinishedMessage").GetLocalizedString()
            )));
            manager.player.ui.guiMessageController.GetComponentInChildren<Button>().onClick.AddListener(manager.player.ui.guiSurveyController.activateSurvey);
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
            if (builtWeatherStation && tookSatellitePictures)
            {
                TriggerCompletion();
            }
        }
    }
}