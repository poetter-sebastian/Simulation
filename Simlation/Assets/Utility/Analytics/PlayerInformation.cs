using System;
using System.Collections.Generic;
using Player;
using UnityEngine.Device;

namespace Utility.Analytics
{
    public class TimeRow
    {
        public string caller;
        public long time;

        public TimeRow(string caller, long time)
        {
            this.caller = caller;
            this.time = time;
        }
    }

    [Serializable]
    public class CompactPackage
    {
        public List<List<TimeRow>> timeComplexities = new();
        public List<string> log { get; set; }
        public readonly (string,string)[] systemInformation = new (string, string)[9];
        public readonly long playtime = 0;
        public readonly int money = 0;
        public readonly int quality = 0;
        public readonly bool hasWeatherUIUnlocked = false;
        public readonly bool hasAridityViewUnlocked = false;
        public readonly bool hasGroundTypeViewUnlocked = false;
        public readonly bool hasHeightViewUnlocked = false;
        public readonly int rubbishCount = 0;
        public readonly int treeCount = 0;
        public readonly int playerTreeCount = 0;
        public readonly int playerCutTreeCount = 0;
        public readonly int playerFoundIllTreesCount = 0;
        public readonly int soilSampleCount = 0;
        public readonly float hottestTemp = -40;
        public readonly float coldestTemp = 40;
        public readonly float highestQuality = -1;
        public readonly float lowestQuality = 100;
        public readonly float o2Production = 0;
        public readonly float co2Consumption = 0;
        public readonly float waterConsumption = 0;
        
        public readonly bool knowGamification;
        public readonly string imagineGamification;
        public readonly int ageArea;
        public readonly string opinionToApp;
        public readonly int teachingScore;
        public readonly int funScore;
        public readonly int systemRequirementsScore;
        public readonly int fancyGraphicScore;
        public readonly int realisticSimulationScore;
        public readonly int nonRealisticSimulationScore;
        public readonly bool tooEasy;
        public readonly bool shareHardware;
        public readonly bool shareLogs;

        public CompactPackage(PlayerHandler playerHandler)
        {
            playtime = playerHandler.playtime;
            money = playerHandler.money;
            quality = playerHandler.quality;
            hasWeatherUIUnlocked = playerHandler.hasWeatherUIUnlocked;
            hasAridityViewUnlocked = playerHandler.hasAridityViewUnlocked;
            hasGroundTypeViewUnlocked = playerHandler.hasGroundTypeViewUnlocked;
            hasHeightViewUnlocked = playerHandler.hasHeightViewUnlocked;
            rubbishCount = playerHandler.rubbishCount;
            treeCount = playerHandler.treeCount;
            playerTreeCount = playerHandler.playerTreeCount;
            playerCutTreeCount = playerHandler.playerCutTreeCount;
            playerFoundIllTreesCount = playerHandler.playerFoundIllTreesCount;
            soilSampleCount = playerHandler.soilSampleCount;
            hottestTemp = playerHandler.hottestTemp;
            coldestTemp = playerHandler.coldestTemp;
            highestQuality = playerHandler.highestQuality;
            lowestQuality = playerHandler.lowestQuality;
            o2Production = playerHandler.o2Production;
            co2Consumption = playerHandler.co2Consumption;
            waterConsumption = playerHandler.waterConsumption;

            knowGamification = playerHandler.ui.guiSurveyController.knowGamification;
            imagineGamification = playerHandler.ui.guiSurveyController.imagineGamification;
            ageArea = playerHandler.ui.guiSurveyController.ageArea;
            opinionToApp = playerHandler.ui.guiSurveyController.opinionToApp;
            teachingScore = playerHandler.ui.guiSurveyController.teachingScore;
            funScore = playerHandler.ui.guiSurveyController.funScore;
            systemRequirementsScore = playerHandler.ui.guiSurveyController.systemRequirementsScore;
            fancyGraphicScore = playerHandler.ui.guiSurveyController.fancyGraphicScore;
            realisticSimulationScore = playerHandler.ui.guiSurveyController.realisticSimulationScore;
            nonRealisticSimulationScore = playerHandler.ui.guiSurveyController.nonRealisticSimulationScore;
            tooEasy = playerHandler.ui.guiSurveyController.tooEasy;
            shareHardware = playerHandler.ui.guiSurveyController.shareHardware;
            shareLogs = playerHandler.ui.guiSurveyController.shareLogs;

            if (shareHardware)
            {
                systemInformation[0] = ("Device model", SystemInfo.deviceModel);
                systemInformation[1] = ("Device type", SystemInfo.deviceType.ToString());
                systemInformation[2] = ("Graphics device name", SystemInfo.graphicsDeviceName);
                systemInformation[3] = ("Graphics device vendor", SystemInfo.graphicsDeviceVendor);
                systemInformation[4] = ("Graphics device version", SystemInfo.graphicsDeviceVersion);
                systemInformation[5] = ("OS", SystemInfo.operatingSystem);
                systemInformation[6] = ("Processor count", SystemInfo.processorCount.ToString());
                systemInformation[7] = ("Processor type", SystemInfo.processorType);
                systemInformation[8] = ("Memory size", SystemInfo.systemMemorySize.ToString());
            }
        }
    }
}