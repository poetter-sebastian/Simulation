using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using World.Environment;

namespace World.Structure
{
    public class Ground
    {
        /// <summary>
        /// Percentage of sand in ground from 0 to 1
        /// </summary>
        private readonly float sand;
        /// <summary>
        /// Percentage of silt in ground from 0 to 1
        /// </summary>
        private readonly float silt;
        /// <summary>
        /// Percentage of clay in ground from 0 to 1
        /// </summary>
        private readonly float clay;
        /// <summary>
        /// Percentage of loam in ground from 0 to 1
        /// </summary>
        private readonly float loam;
        /// <summary>
        /// Value of the current water in the ground from 0 to waterCapacity
        /// </summary>
        private float currentWater = 0;
        /// <summary>
        /// Maximum capacity of water from 1000 to 0
        /// </summary>
        private int waterCapacity = 0;

        private WorldController world;
        
        private Node node;

        public int WaterCapacity => waterCapacity;
        public float CurrentWater => currentWater;
        
        public Color typColor;

        public float Sand => sand;
        public float Silt => silt;
        public float Clay => clay;
        public float Loam => loam;

        public Node Node => node;
        
        public Ground(WorldController world, Node node, float sand, float clay, float silt, float loam)
        {
            this.world = world;
            this.node = node;
            
            var sum = sand + clay + silt + loam;
            this.sand = sand / sum;
            this.clay = clay / sum;
            this.silt = silt / sum;
            this.loam = loam / sum;

            //TODO find a better way to perform this
            waterCapacity += (int)(Sand * GroundWaterCapacity(GroundTypes.Sand));
            waterCapacity += (int)(Clay *  GroundWaterCapacity(GroundTypes.Clay));
            waterCapacity += (int)(Loam * GroundWaterCapacity(GroundTypes.Loam));
            waterCapacity += (int)(Silt * GroundWaterCapacity(GroundTypes.Silt));
            currentWater = (float)waterCapacity / 2;
                
            //if no values (rare case if all is 0)
            if (sand == 0 && clay == 0 && silt == 0 && loam == 0)
            {
                this.silt = 100;
            }
            
            typColor = CalcTypeColor();
        }

        public WorldController RefWorld()
        {
            return world;
        }

        /// <summary>
        /// Init the water value 
        /// </summary>
        /// <param name="value">Water from 0 to 1</param>
        public void InitWater(float value)
        {
            currentWater = Mathf.Lerp(0, waterCapacity, value);
        }
        
        /// <summary>
        /// Sets the water value and update the node color of the ground
        /// </summary>
        /// <param name="value">Water from 0 to 1000</param>
        public void SetWater(float value)
        {
            currentWater = value > waterCapacity ? waterCapacity : value;
            world.UpdateGroundColor(node.ID, Mathf.InverseLerp(waterCapacity, 0, currentWater));
        }
        
        /// <summary>
        /// Get water out of the ground and returns false if there is no water
        /// </summary>
        /// <param name="consume">Negative number to reduce the current water value</param>
        /// <returns>True if there is enough water in ground</returns>
        public bool GetWater(float consume)
        {
            world.CalcWaterArea(this, consume);
            if (currentWater < 1)
            {
                currentWater = 0;
            }
            return currentWater > 0;
        }
        
        public Color CalcTypeColor()
        {
            if (sand > clay)
            {
                return sand > silt ? GroundColor(sand > loam ? GroundTypes.Sand : GroundTypes.Loam) : GroundColor(silt > loam ? GroundTypes.Silt : GroundTypes.Loam);
            }
            return clay > silt ? GroundColor(clay > loam ? GroundTypes.Clay : GroundTypes.Loam) : GroundColor(silt > loam ? GroundTypes.Silt : GroundTypes.Loam);
        }
        
        /// <summary>
        /// Get the water capacity value for each ground type
        /// </summary>
        /// <param name="type">Ground type</param>
        /// <returns>Water capacity</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int GroundWaterCapacity(GroundTypes type) => type switch
        {
            GroundTypes.Clay => 1000,
            GroundTypes.Silt => 600,
            GroundTypes.Loam => 1000,
            GroundTypes.Sand => 50,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        public static Color32 GroundColor(GroundTypes type) => type switch
        {
            GroundTypes.Clay => new Color32(131, 126, 123, 255),
            GroundTypes.Silt => new Color32(222, 169, 127, 255),
            GroundTypes.Loam => new Color32(51, 49, 52, 255),
            GroundTypes.Sand => new Color32(228, 174, 41, 255),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        
        public static string GroundTypeToString(GroundTypes type) => type switch
        {
            GroundTypes.Clay => new LocalizedString("Enum", "GroundTypes.Clay").GetLocalizedString(),
            GroundTypes.Silt => new LocalizedString("Enum", "GroundTypes.Silt").GetLocalizedString(),
            GroundTypes.Loam => new LocalizedString("Enum", "GroundTypes.Loam").GetLocalizedString(),
            GroundTypes.Sand => new LocalizedString("Enum", "GroundTypes.Sand").GetLocalizedString(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        
        public enum GroundTypes
        {
            Clay,
            Silt,
            Loam,
            Sand,
        }
    }
}
