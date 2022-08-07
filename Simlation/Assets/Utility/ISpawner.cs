using System;
using UnityEngine;

namespace World.Environment
{
    public interface ISpawner
    {
        public void Spawn();

        public class NoSpawnerException : Exception
        {
            
        }
    }
}
