using System;

namespace World.Agents
{
    public interface IAgentBehaviour
    {
        public Func<States, States> Next();
        public IAgentBehaviour.States Current();
        
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