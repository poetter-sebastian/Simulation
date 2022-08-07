using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utility
{
    public interface IColliderBehaviour
    {
        public event IColliderBehaviour.TriggerHandler OnSense;
        public event IColliderBehaviour.TriggerHandler OnRadiusSense;
        public event IColliderBehaviour.TriggerHandler OnRadiusExitSense;
        
        public void OnTriggerEnter(Collider other);
        
        public void OnTriggerExit(Collider other);
        
        public void OnTriggerStay(Collider other);

        public delegate void TriggerHandler(GameObject obj);
    }
}