namespace World.Agents.Animals.Rabbit
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
            behaviour = new RabbitBehavior();
            maxPossibleSpeed = 5;
        }

        public override void Handle()
        {
            behaviour.Next();
        }
    }
}