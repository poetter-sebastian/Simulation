using UnityEngine;

namespace Utility
{
    public interface IColliderBehaviour
    {
        /// <summary>
        /// If something was sensed
        /// </summary>
        public event TriggerHandler OnSense;
        /// <summary>
        /// If something is in radius
        /// </summary>
        public event TriggerHandler OnRadiusSense;
        /// <summary>
        /// If something leave the radius
        /// </summary>
        public event TriggerHandler OnRadiusExitSense;
        
        /// <summary>
        /// On collision enter
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other);
        
        /// <summary>
        /// On collision exit
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit(Collider other);
        
        /// <summary>
        /// On collision stays inside
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerStay(Collider other);

        /// <summary>
        /// Delegate for event handler
        /// </summary>
        public delegate void TriggerHandler(GameObject obj);
    }
}