using System;
using UnityEngine;

namespace Utility
{
    public interface ILog
    {
        static void L<T>(Func<string> name, T msg, LogType type = LogType.Log)
        {
            Debug.unityLogger.Log(type, "["+name()+"]", Environment.TickCount+" "+msg);
        }
        
        static void LER<T>(Func<string> name, T msg)
        {
            Debug.unityLogger.Log(LogType.Error, name(), Environment.TickCount+" "+msg);
        }
        
        static void LE<T>(Func<string> name, T msg)
        {
            Debug.unityLogger.Log(LogType.Exception, name(), Environment.TickCount+" "+msg);
        }

        string LN();
    }
}