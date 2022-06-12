using System;

namespace World.Agents
{
    public abstract class AgentBehaviour
    {
        public Func<States, States> GetState(States state) => state switch
        {
            States.GetStatus => GetStatus,
            States.GetFood => GetFood,
            States.Eat => Eat,
            States.GetWater => GetWater,
            States.Drink => Drink,
            States.GetSleepingPlace => GetSleepingPlace,
            States.Sleep => Sleep,
            States.Awake => Awake,
            States.GetPartner => GetPartner,
            States.Breeding => Breeding,
            States.GetGroup => GetGroup,
            States.Socialize => Socialize,
            States.FightAsPack => FightAsPack,
            States.Flee => Flee,
            States.Fight => Fight,
            _ => throw new ArgumentOutOfRangeException(nameof(state), $"Not expected state value: {state}"),
        };
        
        protected abstract States GetStatus(States states);
        protected abstract States GetFood(States states);
        protected abstract States Eat(States states);
        protected abstract States GetWater(States states);
        protected abstract States Drink(States states);
        protected abstract States GetSleepingPlace(States states);
        protected abstract States Sleep(States states);
        protected abstract States Awake(States states);
        protected abstract States GetPartner(States states);
        protected abstract States Breeding(States states);
        protected abstract States GetGroup(States states);
        protected abstract States Socialize(States states);
        protected abstract States FightAsPack(States states);
        protected abstract States Flee(States states);
        protected abstract States Fight(States states);

        public enum States
        {
            /// <summary>
            /// Main state and entry point 
            /// </summary>
            GetStatus,
            /// <summary>
            /// Search or hunt for food
            /// </summary>
            GetFood,
            /// <summary>
            /// Eat
            /// </summary>
            Eat,
            /// <summary>
            /// Search for a water source
            /// </summary>
            GetWater,
            /// <summary>
            /// Drink
            /// </summary>
            Drink,
            /// <summary>
            /// Search a place for sleeping or a den
            /// </summary>
            GetSleepingPlace,
            /// <summary>
            /// Sleep
            /// </summary>
            Sleep,
            /// <summary>
            /// Get awake after sleeping
            /// </summary>
            Awake,
            /// <summary>
            /// Search a partner for breeding
            /// </summary>
            GetPartner,
            /// <summary>
            /// Breeding process
            /// </summary>
            Breeding,
            /// <summary>
            /// Search conspecific
            /// </summary>
            GetGroup,
            /// <summary>
            /// Socialize with conspecifics
            /// </summary>
            Socialize,
            /// <summary>
            /// Fight with the pack
            /// </summary>
            FightAsPack,
            /// <summary>
            /// Flee against a predator
            /// </summary>
            Flee,
            /// <summary>
            /// Fight against a predator
            /// </summary>
            Fight,
        }
    }
}