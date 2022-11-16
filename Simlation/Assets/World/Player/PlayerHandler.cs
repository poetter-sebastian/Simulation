using System;
using System.Globalization;
using Player.GUI;
using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerHandler : MonoBehaviour, ILog
    {
        public string PlayerName = "Jeff";
        public int money = 0;
        public int rubbishCount = 0;
        public int treeCount = 0;
        public int quality = 0;
        public long playtime = 0;

        public string LN() => "Time handler";
        
        public GUIController ui;

        public float hottestTemp = -40;
        public float coldestTemp = 40;
        public float highestQuality = -1;
        public float lowestQuality = 100;
        public float o2production = 0;
        public float co2Consumptin = 0;
        public float waterConsumption = 0;

        public void AddMoney(int amount)
        {
            money += amount;
            ui.guiResourcesController.OnMoneyChange(new GenEventArgs<string>(money.ToString(CultureInfo.InvariantCulture)));
        }

        public void AddRubbish()
        {
            rubbishCount++;
            CalcQuality();
        }

        public void RemoveRubbish()
        {
            rubbishCount--;
            if (rubbishCount < 0)
            {
                treeCount = 0;
                ILog.LER(LN, "Rubbish goes under 0!");
            }
            CalcQuality();
        }

        public void AddTree()
        {
            treeCount++;
            CalcQuality();
        }

        public void RemoveTree()
        {
            treeCount--;
            if (treeCount < 0)
            {
                treeCount = 0;
                ILog.LER(LN, "Tree count goes under 0!");
            }
            CalcQuality();
        }
        
        private void CalcQuality()
        {
            var rubbish = rubbishCount switch
            {
                > 100 => 25,
                < 0 => 0,
                _ => (int)Mathf.Lerp(0, 25, rubbishCount / 100f)
            };
            var tree = treeCount switch
            {
                > 300 => 0,
                < 0 => 75,
                _ => (int)Mathf.Lerp(75, 0, treeCount / 300f)
            };
            quality = 100 - rubbish - tree;
            ui.guiResourcesController.OnQualityChange(new GenEventArgs<string>(quality.ToString(CultureInfo.InvariantCulture)));
        }
    }
}