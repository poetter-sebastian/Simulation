namespace World.Agents.Animals
{
    public class RedFoxes : FaunaAgent
    {
        public RedFoxes()
        {
            domain = "Eukarya";
            kingdom = "Animalia";
            phylum = "Chordata";
            agentClass = "Mammalia";
            order = "Carnivora";
            family = "Canicae";
            genus = "Vulpes";
            species = "Vuples Vulpes";
        }

        public override void Handle()
        {
            throw new System.NotImplementedException();
        }
    }
}