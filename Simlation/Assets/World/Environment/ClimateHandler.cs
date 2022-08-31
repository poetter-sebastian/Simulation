using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World.Environment
{
    public class ClimateHandler : MonoBehaviour
    {
        public static ClimateHandler Instance;
        public WindZone wind;
        [Range(0, 1)]
        public float humidity = 0.5f; //%
        [Range(980, 1060)]
        public float airPressure = 1005; //hPa
        public float windSpeed = 1; //meter per second
        public float windDirection = 0; //north = 0°, east = 90°, south = 180°, west = 270°
        public float height = 222; //meter
        [Range(0, 1)]
        public float cloudCover = 0; //% 0 no clouds, 100 cloud cover
        
        public ClimateZone zone = ClimateZone.HumidTemperate;
        public Weather weather = Weather.Clear;

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
        }

        public void OnDayChange(object sender, EventArgs e)
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

            //temperature
            var temp = e.OverNoon ? 
                //e.Hour >= 12 ==> e.Hour / 12 - 1 = 1.x - 1 = 0.x 
                Mathf.Lerp(maxDayTemperature, minDayTemperature, (e.Hour / 12f) -1) : 
                Mathf.Lerp(minDayTemperature, maxDayTemperature, e.Hour / 12f);
            temperature = temp + temp * WeatherToTemperatureMultiplier(weather); 
            
            //clouds
            //from 0 to 100
            cloudCover = Mathf.PerlinNoise(Time.time * 0.005f, 0.0f);
            
            //humidity
            var humidityCalc = 0.5f + WeatherToHumidityMultiplier(weather) * Random.Range(0, 0.75f);
            humidity = humidityCalc > 1 ? 1 : humidityCalc;
            
            //air pressure
            //from 980 to 1060
            airPressure = 980 + 80 * Mathf.PerlinNoise(Time.time * 0.025f, 0.0f);
        }

        /// <summary>
        /// Gets the pair values in °C for each month.
        /// </summary>
        /// <param name="month">Month from 1 to 12</param>
        /// <returns>Pair of min max temperature in °C</returns>
        /// <exception cref="ArgumentOutOfRangeException">If Number below 1 or greater 12</exception>
        /// <comment>Values from https://www.wetter.de/klima/europa/deutschland-c49.html (visited 16.08.2022)</comment>
        public (int, int) MonthToTemperature(TimeHandler.Months month) => month switch
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

        public float WeatherToTemperatureMultiplier(Weather current) => current switch
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
        
        public float WeatherToHumidityMultiplier(Weather current) => current switch
        {
            Weather.Rain => 0.5f,
            Weather.Drizzle => 0.35f,
            Weather.Snow => 0.25f,
            Weather.Clear => 0,
            Weather.Stormy => 0.55f,
            Weather.Blizzard => 0.4f,
            Weather.Foggy => 0.8f,
            Weather.HailStorm => 0.3f,
            Weather.HeatWave => -0.6f,
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