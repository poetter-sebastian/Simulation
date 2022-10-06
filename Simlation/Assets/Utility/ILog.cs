using System;
using UnityEngine;

namespace Utility
{
    public interface ILog
    {
        static void L<T>(Func<string> name, T msg, LogType type = LogType.Log)
        {
            Debug.unityLogger.Log(type, name(), name() + ": " + msg);
        }
        
        static void LER<T>(Func<string> name, T msg)
        {
            Debug.unityLogger.Log(LogType.Error, name(),name() + ": " + msg);
        }
        
        static void LE<T>(Func<string> name, T msg)
        {
            Debug.unityLogger.Log(LogType.Exception, name(),name() + ": " + msg);
        }

        string LN();
    }
}