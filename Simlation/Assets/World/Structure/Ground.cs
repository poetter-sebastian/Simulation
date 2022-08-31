
namespace World.Structure
{
    public class Ground 
    {
        /// <summary>
        /// Percentage of sand in ground from 0% to 100%
        /// </summary>
        private readonly int sand;
        /// <summary>
        /// Percentage of silt in ground from 0% to 100%
        /// </summary>
        private readonly int silt;
        /// <summary>
        /// Percentage of clay in ground from 0% to 100%
        /// </summary>
        private readonly int clay;
        /// <summary>
        /// Percentage of loam in ground from 0% to 100%
        /// </summary>
        private readonly int loam;

        private Node node;
        
        public int Sand => sand;
        public int Silt => silt;
        public int Clay => clay;
        public int Loam => loam;

        public Ground(Node node, float sand, float clay, float silt, float loam)
        {
            this.node = node;
            
            var sum = sand + clay + silt + loam;
            this.sand = (int)(sand / sum * 100);
            this.clay = (int)(clay / sum * 100);
            this.silt = (int)(silt / sum * 100);
            this.loam = (int)(loam / sum * 100);
            
            if (sand == 0 && clay == 0 && silt == 0 && loam == 0)
            {
                this.loam = 100;
            }
        }
    }
}
