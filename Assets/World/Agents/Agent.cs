namespace World.Agents
{
    public class Agent : WorldObject
    {
        public bool moveAble;
        public bool alive = true;

        public string domain = "Unknown";
        public string kingdom = "Unknown";
        public string phylum = "Unknown";
        public string agentClass = "Unknown";
        public string order = "Unknown";
        public string family = "Unknown";
        public string genus = "Unknown";
        public string species = "Unknown";

        public float o2Modifier;
        public float co2Modifier;
        public float ch4Modifier;
        public float weight;
        public float temperature;
        public float size;
        public float age;
        
        //do stuff on the day or at night
        public bool diurnal = true;

        public int health = 100;
        public int hunger;
        public int thirst;
        public int[] injuries;
        public int[] diseases;

        //how many children
        public int progeny = 1;
        public float gestationPeriods;
        public float timeToMature;

        public AgentBehaviour behaviour;

        public void Handle()
        {
            
        }
    }
}
