using System;
using System.Globalization;
using Player.Camera;
using Player.GUI;
using UnityEngine;
using Utility;
using World.Environment;

namespace Player
{
    public class PlayerHandler : MonoBehaviour, ILog
    {
        public string LN() => "Time handler";
        
        [Header("Player properties")]
        public string PlayerName = "Jeff";
        public long playtime = 0;
        public GUIController ui;
        public FreeLookUserInput movement;
        
        [Header("Player resources")]
        public int money = 0;
        public int quality = 0;

        [Header("Unlocks")]
        public bool hasWeatherUIUnlocked = false;
        public bool hasTextureViewUnlocked = false;
        public bool hasAridityViewUnlocked = false;
        public bool hasGroundTypeViewUnlocked = false;
        public bool hasHeightViewUnlocked = false;

        [Header("Statistics")]
        public int rubbishCount = 0;
        public int treeCount = 0;
        public int playerTreeCount = 0;
        public int playerCutTreeCount = 0;
        public int playerFoundIllTreesCount = 0;
        public int soilSampleCount = 0;
        public float hottestTemp = -40;
        public float coldestTemp = 40;
        public float highestQuality = -1;
        public float lowestQuality = 100;
        public float o2Production = 0;
        public float co2Consumption = 0;
        public float waterConsumption = 0;

        [Header("Place objects")] 
        public bool activateDigging;
        public bool activateWeatherStationBuilding;
        public bool activateTreePlanting;
        public GameObject hole;
        public GameObject weatherStation;

        public event EventHandler<GenEventArgs<int>> GotMoney;
        public event EventHandler<GenEventArgs<int>> SpendMoney;
        public event EventHandler<GenEventArgs<int>> TookSoilExample;
        public event EventHandler<GenEventArgs<int>> FoundIllTrees;
        public event EventHandler<GenEventArgs<int>> CutTrees;
        public event EventHandler<GenEventArgs<int>> PlantedTrees;
        public event EventHandler BuildedWeatherStation;
        public event EventHandler TookSatellitePicture;
        public event EventHandler UnlockedElements;

        public void UnlockWeatherUI()
        {
            UnlockedElements?.Invoke(this, EventArgs.Empty);
            ui.guiOptionsController.ActivateWeatherButton();
            hasWeatherUIUnlocked = true;
        }

        public void UnlockHeightView()
        {
            UnlockedElements?.Invoke(this, EventArgs.Empty);
            ui.guiButtonPanelController.heightView.gameObject.SetActive(true);
            hasHeightViewUnlocked = true;
        }
        
        public void UnlockAridityView()
        {
            UnlockedElements?.Invoke(this, EventArgs.Empty);
            ui.guiButtonPanelController.aridityView.gameObject.SetActive(true);
            hasAridityViewUnlocked = true;
        }
        
        public void UnlockGroundTypeView()
        {
            UnlockedElements?.Invoke(this, EventArgs.Empty);
            ui.guiButtonPanelController.groundTypeView.gameObject.SetActive(true);
            hasGroundTypeViewUnlocked = true;
        }
        
        public void UnlockTextureView()
        {
            UnlockedElements?.Invoke(this, EventArgs.Empty);
            ui.guiButtonPanelController.textureViewButton.gameObject.SetActive(true);
            hasTextureViewUnlocked = true;
        }

        public void ActivateTree()
        {
            activateDigging = false;
            activateWeatherStationBuilding = false;
            activateTreePlanting = true;
            movement.blockWorldInteractions = true;
        }
        
        public void ActivateSoilSample()
        {
            activateDigging = true;
            activateWeatherStationBuilding = false;
            activateTreePlanting = false;
            movement.blockWorldInteractions = true;
        }
        
        public void ActivateWeatherStation()
        {
            activateDigging = false;
            activateWeatherStationBuilding = true;
            activateTreePlanting = false;
            movement.blockWorldInteractions = true;
        }

        public void AddMoney(int amount)
        {
            money += amount;
            GotMoney?.Invoke(this, new GenEventArgs<int>(money));
            ui.guiResourcesController.OnMoneyChange(new GenEventArgs<string>(money.ToString(CultureInfo.InvariantCulture)));
        }

        public bool RemoveMoney(int amount)
        {
            if (money - amount >= 0)
            {
                money -= amount;
                SpendMoney?.Invoke(this, new GenEventArgs<int>(money));
                ui.guiResourcesController.OnMoneyChange(new GenEventArgs<string>(money.ToString(CultureInfo.InvariantCulture)));
                return true;
            }
            return false;
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

        public void AddSoilSample()
        {
            soilSampleCount++;
            TookSoilExample?.Invoke(this, new GenEventArgs<int>(soilSampleCount));
        }

        public void PlayerFoundIllTree()
        {
            playerFoundIllTreesCount++;
            FoundIllTrees?.Invoke(this, new GenEventArgs<int>(playerFoundIllTreesCount));
        }
        
        public void PlayerRemovedTree()
        {
            playerCutTreeCount++;
            CutTrees?.Invoke(this, new GenEventArgs<int>(playerCutTreeCount));
            RemoveTree();
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
        
        public void AddPlayerPlantedTree()
        {
            playerTreeCount++;
            PlantedTrees?.Invoke(this, new GenEventArgs<int>(playerTreeCount));
            AddTree();
        }
        
        public void AddTree()
        {
            treeCount++;
            CalcQuality();
        }

        public void PlantTree()
        {
            AddPlayerPlantedTree();
        }
        
        public void BuildWeatherStation()
        {
            BuildedWeatherStation?.Invoke(this, EventArgs.Empty);
        }
        
        public void TakeSatellitePicture()
        {
            if (RemoveMoney(500))
            {
                TookSatellitePicture?.Invoke(this, EventArgs.Empty);
            }
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

        public void SpawnObjOnWorld(Vector3 pos)
        {
            if (activateDigging && RemoveMoney(50))
            {
                WorldController.Instance.Spawn(hole, pos);
                AddSoilSample();
            }
            else if(activateWeatherStationBuilding && RemoveMoney(250))
            {
                WorldController.Instance.Spawn(weatherStation, pos);
                BuildWeatherStation();
            }
            else if(activateTreePlanting && RemoveMoney(75))
            {
                WorldController.Instance.Spawn(weatherStation, pos);
                PlantTree();
            }
            else
            {
                ILog.LER(LN, "Placed obj without activating it!");
            }
        }
    }
}