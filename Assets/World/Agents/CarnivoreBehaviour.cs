using System;

namespace World.Agents
{
    public abstract class CarnivoreBehaviour : IAgentBehaviour
    {
        private IAgentBehaviour.States current = IAgentBehaviour.States.GetStatus;

        public IAgentBehaviour.States Current()
        {
            return current;
        }

        public Func<IAgentBehaviour.States, IAgentBehaviour.States> Next() => current switch
        {
            IAgentBehaviour.States.GetStatus => GetStatus,
            IAgentBehaviour.States.GetFood => GetFood,
            IAgentBehaviour.States.Eat => Eat,
            IAgentBehaviour.States.GetWater => GetWater,
            IAgentBehaviour.States.Drink => Drink,
            IAgentBehaviour.States.GetSleepingPlace => GetSleepingPlace,
            IAgentBehaviour.States.Sleep => Sleep,
            IAgentBehaviour.States.Awake => Awake,
            IAgentBehaviour.States.GetPartner => GetPartner,
            IAgentBehaviour.States.Breeding => Breeding,
            IAgentBehaviour.States.GetGroup => GetGroup,
            IAgentBehaviour.States.Socialize => Socialize,
            IAgentBehaviour.States.FightAsPack => FightAsPack, //because carnivores don't hunt 
            IAgentBehaviour.States.Flee => Flee,
            IAgentBehaviour.States.Fight => Fight,
            _ => throw new ArgumentOutOfRangeException(nameof(current), $"Not expected state value: {current}"),
        };

        protected abstract IAgentBehaviour.States FightAsPack(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States Fight(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States Flee(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States Socialize(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States GetGroup(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States Breeding(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States GetPartner(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States Awake(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States Sleep(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States GetSleepingPlace(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States Drink(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States GetWater(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States Eat(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States GetFood(IAgentBehaviour.States arg);
        protected abstract IAgentBehaviour.States GetStatus(IAgentBehaviour.States arg);
    }
}