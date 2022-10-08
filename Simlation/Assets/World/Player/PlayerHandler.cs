using Player.GUI;

namespace Player
{
    public class PlayerHandler
    {
        public string PlayerName = "Jeff";
        public int money = 0;
        public int quality = 0;
        public long playtime = 0;

        public GUIController ui;

        public float hottestTemp = float.MinValue;
        public float coldestTemp = float.MaxValue;
        public float highestQuality = -1;
        public float lowestQuality = 100;
        public float co2Consumptin = 0;
        public float waterConsumption = 0;
    }
}