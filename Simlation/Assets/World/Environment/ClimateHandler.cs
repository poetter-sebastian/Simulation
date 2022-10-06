using System;
using System.Globalization;
using NaughtyAttributes;
using Player.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Utility;
using World.Structure;
using Random = UnityEngine.Random;

namespace World.Environment
{
    public class ClimateHandler : MonoBehaviour
    {
        public static ClimateHandler Instance;
        public WindZone wind;
        public GameObject water;
        public Volume clouds;
        [Range(0, 1)]
        public float humidity = 0.5f; //%
        [Range(980, 1060)]
        public float airPressure = 1005; //hPa
        [Range(0, 3)]
        [OnValueChanged("SetWindSpeed")]
        public float windSpeed = 1; //meter per second
        [Range(0, 360)]
        [OnValueChanged("SetWindDirection")]
        public float windDirection = 0; //north = 0°, east = 90°, south = 180°, west = 270°
        public float height = 222; //meter
        [Range(0, 1)]
        public float cloudCover = 0; //% 0 no clouds, 100 cloud cover
        [Range(-0.5f, 8)]
        [OnValueChanged("SetWaterLevel")]
        public float waterLevel = 2;
        public ClimateZone zone = ClimateZone.HumidTemperate;
        [OnValueChanged("SetWeather")]
        public Weather weather = Weather.Clear;

        public GUIController ui;

        [Range(-40, 80)]
        public float temperature = 24.5f; //°C
        public float minDayTemperature = 20; //°C
        public float maxDayTemperature = 30; //°C
        public float weatherMinMultiplier = 0.25f; //%
        public float weatherMaxMultiplier = 0.35f; //%

        private TimeHandler time;
        
        private void Awake()
        {
            if(!Instance)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("World instance already set!");
            }
        }

        private void Start()
        {
            time = TimeHandler.Instance;
            time.TimeChangedToMidnight += OnDayChange;
            time.TimeHourElapsed += OnHourElapsed;
            
            OnDayChange(this, EventArgs.Empty);
            OnHourElapsed(this, new HourElapsedEventArgs(time.LocalTime.Hour));
        }

        private void OnDayChange(object sender, EventArgs e)
        {
            Debug.Log("Day has elapsed");

            var tempPair = MonthToTemperature((TimeHandler.Months)time.month);

            // Example
            // min = -7 - Rand(0, 7 * 0.25) == -7 - 1.5 = -8.5°C
            //TODO include cloudCover and humidity to this formula
            minDayTemperature = tempPair.Item1-Random.Range(0, Math.Abs(weatherMinMultiplier*tempPair.Item1));
            // Example
            // max = 4 + Rand(0, 4 * 0.35) == 4 + 1.3 = 5.3°C
            //TODO include cloudCover and humidity to this formula
            maxDayTemperature = tempPair.Item2+Random.Range(0, Math.Abs(weatherMaxMultiplier*tempPair.Item1));
        }
        
        private void OnHourElapsed(object sender, HourElapsedEventArgs e)
        {
            Debug.Log("hour has elapsed");
            
            //wind speed
            //from 0 to 3
            var spee = Mathf.Lerp(0, 3, Mathf.PerlinNoise(Time.time * 0.015f, 0.0f));
            SetWindSpeed(spee);
            
            //wind direction
            //from 0 to 360
            SetWindDirection(Mathf.Lerp(0, 360, Mathf.PerlinNoise(Time.time * 0.005f, 0.0f)));
            
            //clouds
            //from 0 to 100
            SetCloudCover(Mathf.PerlinNoise(Time.time * 0.015f, 0.0f));
            
            //humidity
            var humid = 0.3f + WeatherToHumidityMultiplier(weather) * Random.Range(0.1f, 0.75f);
            SetHumidity(humid);
            
            //air pressure
            //from 980 to 1060
            SetAirPressure(Mathf.Lerp(980, 1060, Mathf.PerlinNoise(Time.time * 0.025f, 0.0f)));
            
            //temperature and windchill
            
            SetTemperature(e.OverNoon ? 
                //e.Hour >= 12 ==> e.Hour / 12 - 1 = 1.x - 1 = 0.x 
                Mathf.Lerp(maxDayTemperature, minDayTemperature, (e.Hour / 12f) -1) : 
                Mathf.Lerp(minDayTemperature, maxDayTemperature, e.Hour / 12f));
        }

        public void SetWindSpeed(float value = -1)
        {
            windSpeed = value switch
            {
                -1 => windSpeed,
                < 0 => 0,
                >= 0 and <= 5 => value,
                > 5 => 5,
                _ => windSpeed
            };
            wind.windMain = windSpeed;
            ui.guiWeatherController.OnWindSpChange(new GenEventArgs<string>((windSpeed * 3.6f * time.TimeSpeed).ToString("0.00")));
            //TODO cloud wind speed
        }
        
        public void SetCloudCover(float value = -1)
        {
            cloudCover = value == -1 ? cloudCover : value;
            ui.guiWeatherController.OnCloudCoverChange(new GenEventArgs<string>((cloudCover * 100).ToString("0")));
        }
        
        public void SetWindDirection(float value = -1)
        {
            windDirection = value == -1 ? windDirection : value % 360;
            clouds.sharedProfile.TryGet<VolumetricClouds>(typeof(VolumetricClouds), out var cloudComp);
            var cloud = new WindParameter.WindParamaterValue
            {
                customValue = value
            };
            cloudComp.orientation.value = cloud;
            //because of https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@12.0/manual/Override-Volumetric-Clouds.html#wind orientation!
            wind.transform.rotation = Quaternion.Euler(0, -((value + 270) % 360), 0);
            ui.guiWeatherController.OnWindDirChange(new GenEventArgs<string>(Graph.DirectionToAbbr(Graph.DegreeToDirection(value))));
        }

        public void SetWaterLevel()
        {
            SetWaterLevel(waterLevel);
        }

        public void SetWaterLevel(float value)
        {
            waterLevel = waterLevel switch
            {
                < -0.5f => -0.5f,
                >= -0.5f and <= 8 => value,
                > 8 => 8,
                _ => waterLevel
            };
            water.transform.position = new Vector3(0, waterLevel, 0);
        }
        
        public void SetAirPressure(float value = -1)
        {
            airPressure = value switch
            {
                -1 => airPressure,
                < 980 => 980,
                >= 980 and <= 1060 => value,
                > 1060 => 1060,
                _ => value
            };
            ui.guiWeatherController.OnPressChange(new GenEventArgs<string>(airPressure.ToString("0.00")));
        }
        
        public void SetHumidity(float value = -1)
        {
            humidity = value == -1 ? humidity : value > 1 ? 1 : value;
            ui.guiWeatherController.OnHumChange(new GenEventArgs<string>((humidity * 100).ToString("0")));
        }

        public void SetWeather()
        {
            SetWeather(weather);
        }

        public void SetTemperature(float value = -1)
        {
            //temperature
            temperature = value == -1 ? temperature : value + value * WeatherToTemperatureMultiplier(weather);
            ui.guiWeatherController.OnTempChange(new GenEventArgs<string>(temperature.ToString("0.00")));
            //windchill
            ui.guiWeatherController.OnTempFeelChange(new GenEventArgs<string>((
                13.12f + 0.6215f * temperature + (0.3965f * temperature - 11.37f) * MathF.Pow(windSpeed * 3.6f, 0.16f)
            ).ToString("0.00")));
        }

        private void SetWeather(Weather value)
        {
            weather = value;
            clouds.sharedProfile.TryGet<VolumetricClouds>(typeof(VolumetricClouds), out var cloudComp);
            switch (weather)
            {
                case Weather.Rain:
                    cloudComp.cloudPreset.value = VolumetricClouds.CloudPresets.Stormy;
                    break;
                case Weather.Drizzle:
                    cloudComp.cloudPreset.value = VolumetricClouds.CloudPresets.Stormy;
                    break;
                case Weather.Snow:
                    break;
                case Weather.Clear:
                    cloudComp.cloudPreset.value = VolumetricClouds.CloudPresets.Sparse;
                    break;
                case Weather.Stormy:
                    cloudComp.cloudPreset.value = VolumetricClouds.CloudPresets.Stormy;
                    break;
                case Weather.Blizzard:
                    cloudComp.cloudPreset.value = VolumetricClouds.CloudPresets.Stormy;
                    break;
                case Weather.Foggy:
                    break;
                case Weather.HailStorm:
                    cloudComp.cloudPreset.value = VolumetricClouds.CloudPresets.Stormy;
                    break;
                case Weather.HeatWave:
                    cloudComp.cloudPreset.value = VolumetricClouds.CloudPresets.Sparse;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weather), weather, null);
            }
            //update the whole values because of the weather multiplier
            OnDayChange(this, EventArgs.Empty);
            OnHourElapsed(this, new HourElapsedEventArgs(time.LocalTime.Hour));
            ui.guiWeatherController.OnWeatherChange(new GenEventArgs<string>(WeatherToString(weather)));
        }
        
        /// <summary>
        /// Gets the pair values in °C for each month.
        /// </summary>
        /// <param name="month">Month from 1 to 12</param>
        /// <returns>Pair of min max temperature in °C</returns>
        /// <exception cref="ArgumentOutOfRangeException">If Number below 1 or greater 12</exception>
        /// <comment>Values from https://www.wetter.de/klima/europa/deutschland-c49.html (visited 16.08.2022)</comment>
        public static (int, int) MonthToTemperature(TimeHandler.Months month) => month switch
        {
            TimeHandler.Months.January => (-5, 4), //extreme value
            TimeHandler.Months.February => (-1, 4),
            TimeHandler.Months.March => (-1, 5),
            TimeHandler.Months.April => (1, 9),
            TimeHandler.Months.May => (5, 15),
            TimeHandler.Months.June => (9, 19),
            TimeHandler.Months.July => (12, 31), //extreme value
            TimeHandler.Months.August => (15, 36), //extreme value
            TimeHandler.Months.September => (14, 22),
            TimeHandler.Months.October => (12, 19),
            TimeHandler.Months.November => (6, 12),
            TimeHandler.Months.December => (3, 8),
            _ => throw new ArgumentOutOfRangeException(nameof(month), month, null)
        };

        public static float WeatherToTemperatureMultiplier(Weather current) => current switch
        {
            Weather.Rain => -0.25f,
            Weather.Drizzle => -0.15f,
            Weather.Snow => -0.5f,
            Weather.Clear => 0,
            Weather.Stormy => -0.35f,
            Weather.Blizzard => -0.75f,
            Weather.Foggy => -0.05f,
            Weather.HailStorm => -0.2f,
            Weather.HeatWave => 0.35f,
            _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
        };
        
        public static float WeatherToHumidityMultiplier(Weather current) => current switch
        {
            Weather.Rain => 0.5f,
            Weather.Drizzle => 0.35f,
            Weather.Snow => 0.25f,
            Weather.Clear => 0.2f,
            Weather.Stormy => 0.55f,
            Weather.Blizzard => 0.4f,
            Weather.Foggy => 0.8f,
            Weather.HailStorm => 0.3f,
            Weather.HeatWave => -0.6f,
            _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
        };
        
        public static string WeatherToString(Weather current) => current switch
        {
            Weather.Rain => new LocalizedString("Enum", "Weather.Rain").GetLocalizedString(),
            Weather.Drizzle => new LocalizedString("Enum", "Weather.Drizzle").GetLocalizedString(),
            Weather.Snow => new LocalizedString("Enum", "Weather.Snow").GetLocalizedString(),
            Weather.Clear => new LocalizedString("Enum", "Weather.Clear").GetLocalizedString(),
            Weather.Stormy => new LocalizedString("Enum", "Weather.Stormy").GetLocalizedString(),
            Weather.Blizzard => new LocalizedString("Enum", "Weather.Blizzard").GetLocalizedString(),
            Weather.Foggy => new LocalizedString("Enum", "Weather.Foggy").GetLocalizedString(),
            Weather.HailStorm => new LocalizedString("Enum", "Weather.HailStorm").GetLocalizedString(),
            Weather.HeatWave => new LocalizedString("Enum", "Weather.HeatWave").GetLocalizedString(),
            _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
        };
                
        public enum Weather
        {
            Rain,
            Drizzle,
            Snow,
            Clear, //default
            Stormy,
            Blizzard,
            Foggy,
            HailStorm,
            HeatWave,
        }
        
        /// <summary>
        /// simplified Köppen climate classification https://www.ncbi.nlm.nih.gov/pmc/articles/PMC6207062/
        /// </summary>
        public enum ClimateZone
        {
            TropicalRainforest,
            Savannah,
            Steppe,
            Desert,
            Etesian,
            HumidTemperate, //default
            Sinic,
            HumidContinental,
            TransSiberian,
            SummerDryCold,
            Tundra,
            Ice,
        }
    }
}