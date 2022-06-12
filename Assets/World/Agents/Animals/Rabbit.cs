namespace World.Agents.Animals
{
    public class Rabbit : FaunaAgent
    {
        public Rabbit()
        {
            domain = "Eukarya";
            kingdom = "Animalia";
            phylum = "Chordata";
            agentClass = "Mammalia";
            order = "Lagomorpha";
            family = "Leporidae";
            genus = "Lepus";
            species = "Lepus europaeus";
            
        }

        public override void Handle()
        {
            throw new System.NotImplementedException();
        }
    }
}