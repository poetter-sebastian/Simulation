using System;
using UnityEngine;

namespace World.Environment
{
    public interface ISpawner
    {
        public void Spawn(Vector2Int size);

        public class NoSpawnerException : Exception
        {
            
        }
    }
}
