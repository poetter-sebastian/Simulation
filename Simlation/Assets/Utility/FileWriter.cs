using System;
using System.Data;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Utility
{
    public class FileWriter
    {
        private readonly string name;
        
        private static string Path => "Records/";
        
        private string File => Path + name + ".csv";
        
        public FileWriter(string fileName)
        {
            name = fileName;
            StreamWriter w;
            //create if not exists
            if(System.IO.File.Exists(File))
            {
                w = System.IO.File.AppendText(File);
            }
            else
            {
                Directory.CreateDirectory(Path);
                w = System.IO.File.CreateText(File);
                w.WriteLine("Caller,Time");
            }
            w.WriteLine("StartRecord"+","+Environment.TickCount);
            w.Close();
        }

        public void WriteData(string caller, string data)
        {
            //TODO maybe later for better debugging
            //var test = new DataTable();

            using var w = (System.IO.File.Exists(File)) ? System.IO.File.AppendText(File) : System.IO.File.CreateText(File);
            w.WriteLine(caller+","+data);
        }
    }
}