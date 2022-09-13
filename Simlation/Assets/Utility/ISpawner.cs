using System;
using UnityEngine;

namespace World.Environment
{
    /// <summary>
    /// Interface for spawner to 
    /// </summary>
    public interface ISpawner
    {
        public void Spawn(Vector2Int size);

        public class NoSpawnerException : Exception
        {
            // ReSharper disable once InconsistentNaming
            public new string Message = "The selected object is not a spawner.";
        }
    }
}
