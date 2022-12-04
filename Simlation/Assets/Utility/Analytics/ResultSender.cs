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
        
        public List<string> logStack = new();
        
        public event EventHandler<HttpStatusCode> SendComplete;

        private string json;
        private CompactPackage package;
        private HttpStatusCode statusCode = HttpStatusCode.BadRequest;
        
        public string LN()
        {
            return "Analytics Sender";
        }
        
        [Button("Send")]
        public void Send()
        {
            package = new CompactPackage(player);
            if (package.shareLogs)
            {
                package.timeComplexities.Add(world.comp.comp);
                package.timeComplexities.Add(world.compHandle.comp);
                package.log = logStack;
            }
            SendComplete += Completed;
            Task.Run(Sender).Wait(15000);
            if (statusCode != HttpStatusCode.Accepted)
            {
                player.ui.guiErrorHandlingController.PlaceError(json);
            }
        }
        
        private async Task Sender()
        {
            try
            {
                ILog.L(LN, "Init sending!");
                json = JsonConvert.SerializeObject(package, new JsonSerializerSettings());
                var httpClient = new HttpClient();
                
                httpClient.DefaultRequestHeaders.Add("X-API-KEY", Credentials.KEY);
                httpClient.DefaultRequestHeaders.Add("User-Agent", Credentials.AGENT);
                
                ILog.L(LN, "Start sending!");
                try
                {
                    var responseMessage = await httpClient.PostAsync(new Uri(Credentials.URL), new StringContent(json));
                    statusCode = responseMessage.StatusCode;
                    if (statusCode == HttpStatusCode.NotImplemented)
                    {
                        ILog.L(LN, "Error while sending!");
                        ILog.LE(LN, responseMessage.Content.ReadAsStringAsync());
                    }
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
            ILog.L(LN, code == HttpStatusCode.Accepted ? "Send complete!" : "Send error!");
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