using System;
using System.Collections.Generic;
using System.Net;
using NaughtyAttributes;
using Newtonsoft.Json;
using Player;
using UnityEngine;
using World.Environment;
using System.Net.Http;
using System.Threading.Tasks;
using Settings;

namespace Utility.Analytics
{
    public class ResultSender : MonoBehaviour, ILog
    {
        public WorldController world;
        
        public PlayerHandler player;
        
        public List<string> logStack = new List<string>();
        
        public event EventHandler<HttpStatusCode> SendComplete;
        
        private CompactPackage package;
        
        public string LN()
        {
            return "Analytics Sender";
        }
        
        [Button("Send")]
        public void Send()
        {
            package = new CompactPackage(player);
            package.timeComplexities.Add(world.comp.comp);
            package.timeComplexities.Add(world.compHandle.comp);
            package.log = logStack;
            SendComplete += Completed;
            Task.Run(Sender);
        }
        
        public async Task Sender()
        {
            try
            {
                var json = JsonConvert.SerializeObject(package, new JsonSerializerSettings());
                var httpClient = new HttpClient();
                
                httpClient.DefaultRequestHeaders.Add("X-API-KEY", Credentials.KEY);
                httpClient.DefaultRequestHeaders.Add("User-Agent", Credentials.AGENT);
                
                var statusCode = HttpStatusCode.BadRequest;
                try
                {
                    var responseMessage = await httpClient.PostAsync(new Uri(Credentials.URL), new StringContent(json));
                    statusCode = responseMessage.StatusCode;
                }
                catch (Exception e)
                {
                    ILog.L(LN, e.Message);
                }
                SendComplete?.Invoke(this, statusCode);
            }
            catch (Exception e)
            {
                SendComplete -= Completed;
                ILog.L(LN, e.Message);
            }
        }
        
        private void Completed(object s, HttpStatusCode code)
        {
            ILog.L(LN, code == HttpStatusCode.OK ? "Send complete!" : "Send error!");
            SendComplete -= Completed;
        }
        
        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }
        
        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }
        
        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            logStack.Add(logString);
        }
    }
}