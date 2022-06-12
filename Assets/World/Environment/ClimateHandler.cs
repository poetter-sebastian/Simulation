using UnityEngine;

namespace World.Environment
{
    public class ClimateHandler : MonoBehaviour
    {
        public static ClimateHandler Instance;
        public float temperature = 24.5f;
        public float humidity = 30;
        public float airPressure = 0; //hPa
        public float windSpeed = 0; //km per s
        public float windDirection = 0; //north = 0, east = 90, south = 180, west = 270
        public float height = 222; //meter
        public float CloudCover = 0; //0 no clouds, 100 full clouds
        public float Visibility = 20; //km

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
        
        public enum Weather
        {
            Rain,
            Drizzle,
            Snow,
            Clear,
            Stormy,
            Blizzard,
            Foggy,
            Hail,
        }
        
        public enum ClimateZone
        {
            TropicalRainforest,
            Savannah,
            Steppe,
            Desert,
            Etesian,
            HumidTemperate,
            Sinic,
            HumidContinental,
            TransSiberian,
            SummerDryCold,
            Tundra,
            Ice,
        }
    }
}