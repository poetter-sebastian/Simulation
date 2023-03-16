using System;
using System.Globalization;
using Player.Camera;
using Player.GUI;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;
using World.Agents;
using World.Environment;
using World.Environment.Spawn;
using World.Player.Tasks;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerHandler : MonoBehaviour, ILog
    {
        public string LN() => "Time handler";
        
        [Header("Player properties")]
        public string playerName = "Jeff";
        public long playtime = 0;
        public GUIController ui;
        public FreeLookUserInput movement;
        public TaskManager manager;
        
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
        public TreeSpawner plant;

        public event EventHandler<GenEventArgs<int>> GotMoney;
        public event EventHandler<GenEventArgs<int>> SpendMoney;
        public event EventHandler<GenEventArgs<int>> TookSoilExample;
        public event EventHandler<GenEventArgs<int>> FoundIllTrees;
        public event EventHandler<GenEventArgs<int>> CutTrees;
        public event EventHandler<GenEventArgs<int>> PlantedTrees;
        public event EventHandler BuiltWeatherStation;
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
            ui.guiResourcesController.PosValues();
        }

        public bool RemoveMoney(int amount)
        {
            if (money - amount >= 0)
            {
                money -= amount;
                SpendMoney?.Invoke(this, new GenEventArgs<int>(money));
                ui.guiResourcesController.OnMoneyChange(new GenEventArgs<string>(money.ToString(CultureInfo.InvariantCulture)));
                ui.guiResourcesController.NegValues();
                return true;
            }
            ui.guiResourcesController.NoMoney();
            ui.PlayError();
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

        public void OnPlayerFoundIllTree(object s, EventArgs e)
        {
            playerFoundIllTreesCount++;
            FoundIllTrees?.Invoke(this, new GenEventArgs<int>(playerFoundIllTreesCount));
        }
        
        public void PlayerRemovedTree()
        {
            playerCutTreeCount++;
            AddMoney(Random.Range(50, 150));
            CutTrees?.Invoke(this, new GenEventArgs<int>(playerCutTreeCount));
        }
        
        public void RemoveTree(TreeAgent agent)
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
            UpdateStatisticsValue();
        }
        
        public void BuildWeatherStation()
        {
            BuiltWeatherStation?.Invoke(this, EventArgs.Empty);
        }
        
        public void TakeSatellitePicture()
        {
            if (RemoveMoney(500))
            {
                TookSatellitePicture?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SpawnObjOnWorld(Vector3 pos)
        {
            if (activateDigging && RemoveMoney(50))
            {
                WorldController.Instance.Spawn(hole, pos);
                AddSoilSample();
                ui.PlayDigging();
            }
            else if(activateWeatherStationBuilding && RemoveMoney(250))
            {
                WorldController.Instance.Spawn(weatherStation, pos);
                BuildWeatherStation();
                ui.PlayPlace();
            }
            else if(activateTreePlanting && RemoveMoney(75))
            {
                WorldController.Instance.SpawnPlant(plant.GetRandomTree(), pos);
                PlantTree();
                ui.PlayPlace();
            }
            else
            {
                ILog.LER(LN, "Placed obj without activating it!");
            }
        }

        public void UpdateStatisticsValue()
        {
            ui.guiStatisticsController.OnCo2Change(new GenEventArgs<string>(co2Consumption.ToString("0.00")));
            ui.guiStatisticsController.OnO2Change(new GenEventArgs<string>(o2Production.ToString("0.00")));
            //for better calculation (waterConsumption/10)
            ui.guiStatisticsController.OnWaterConsumptionChange(new GenEventArgs<string>((waterConsumption/10).ToString("0.00")));
        }
        
        private void Start()
        {
            WorldController.Instance.climateHandler.TemperatureChanged += OnTemperatureChange;
            
            movement.TreeWasHit += OnTreeClicked;
            
            movement.DoCheating += OnCheating;
            
            ui.guiViewerController.treeCutClicked += OnTreeCut;
            ui.guiViewerController.foundIllTree += OnPlayerFoundIllTree;

            ui.guiSurveyController.windowOpens += movement.OnUIToggle;
            ui.guiSurveyController.windowClosed += movement.OnUIToggle;
            
            ui.guiHelpController.windowOpens += movement.OnUIToggle;
            ui.guiHelpController.windowClosed += movement.OnUIToggle;
            
            ui.guiMessageController.windowOpens += movement.OnUIToggle;
            ui.guiMessageController.windowClosed += movement.OnUIToggle;
            
            ui.guiErrorHandlingController.windowOpens += movement.OnUIToggle;
            ui.guiErrorHandlingController.windowClosed += movement.OnUIToggle;
        }

        private void CalcQuality()
        {
            var rubbish = rubbishCount switch
            {
                > 100 => 25,
                < 0 => 0,
                _ => (int)Mathf.Lerp(0, 25, rubbishCount / 100f)
            };
            ui.guiStatisticsController.OnPollutionChange(new GenEventArgs<string>((100 - rubbish).ToString()));
            var tree = treeCount switch
            {
                > 300 => 0,
                < 0 => 75,
                _ => (int)Mathf.Lerp(75, 0, treeCount / 300f)
            };
            quality = 100 - rubbish - tree;
            ui.guiResourcesController.OnQualityChange(new GenEventArgs<string>(quality.ToString(CultureInfo.InvariantCulture)));
        }

        private void OnTreeClicked(object s, GenEventArgs<TreeAgent> e)
        {
            ui.PlayClick();
            ui.guiViewerController.OpenViewer(e.Value);
        }

        private void OnTreeCut(object s, GenEventArgs<TreeAgent> e)
        {
            e.Value.CutTree();
            PlayerRemovedTree();
            ui.guiViewerController.CloseWindow();
        }

        private void OnTemperatureChange(object s, GenEventArgs<float> e)
        {
            if (coldestTemp > e.Value)
            {
                coldestTemp = e.Value;
                ui.guiStatisticsController.OnMinTempChange(new GenEventArgs<string>(coldestTemp.ToString("0.00")));
            }
            if (hottestTemp < e.Value)
            {
                hottestTemp = e.Value;
                ui.guiStatisticsController.OnMaxTempChange(new GenEventArgs<string>(hottestTemp.ToString("0.00")));
            }
        }

        private void OnCheating(object s, EventArgs e)
        {
            manager.TriggerNextTask();
        }
    }
}