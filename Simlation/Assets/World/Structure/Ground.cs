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
        private readonly int sand;
        /// <summary>
        /// Percentage of silt in ground from 0 to 1
        /// </summary>
        private readonly int silt;
        /// <summary>
        /// Percentage of clay in ground from 0 to 1
        /// </summary>
        private readonly int clay;
        /// <summary>
        /// Percentage of loam in ground from 0 to 1
        /// </summary>
        private readonly int loam;
        /// <summary>
        /// Percentage of the humidity from 0 to 1
        /// </summary>
        private float aridity;
        private WorldController world;
        
        private Node node;
        
        public int Sand => sand;
        public int Silt => silt;
        public int Clay => clay;
        public int Loam => loam;
        public float Aridity => aridity;
        
        public void SetAridity(float value, bool preset = false)
        {
            if (!preset)
            {
                world.ChangeValueOnColorArray(world.aridityColors, world.aridityGradient, node.ID, value);
            }
            aridity = value;
        }

        public Ground(WorldController world, Node node, float sand, float clay, float silt, float loam)
        {
            this.world = world;
            this.node = node;
            
            var sum = sand + clay + silt + loam;
            this.sand = (int)(sand / sum * 100);
            this.clay = (int)(clay / sum * 100);
            this.silt = (int)(silt / sum * 100);
            this.loam = (int)(loam / sum * 100);
            
            //if no values
            if (sand == 0 && clay == 0 && silt == 0 && loam == 0)
            {
                this.loam = 100;
            }
        }

        public Color CalcTypeColor()
        {
            if (sand > clay)
            {
                return sand > silt ? GroundColor(sand > loam ? GroundTypes.Sand : GroundTypes.Loam) : GroundColor(silt > loam ? GroundTypes.Silt : GroundTypes.Loam);
            }
            return clay > silt ? GroundColor(clay > loam ? GroundTypes.Clay : GroundTypes.Loam) : GroundColor(silt > loam ? GroundTypes.Silt : GroundTypes.Loam);
        }

        public static Color GroundColor(GroundTypes type) => type switch
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
