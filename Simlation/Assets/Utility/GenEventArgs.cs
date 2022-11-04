using System;

namespace Utility
{
    /// <summary>
    /// Help class to transmit generic values from object to event function
    /// </summary>
    public class GenEventArgs<T> : EventArgs
    {
        public T Value { get; }
        
        public GenEventArgs(T value)
        {
            Value = value;
        }
    }
}