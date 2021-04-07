using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;

namespace Assets.Scripts
{
    public static class WebSocketExtensions
    {
        public static void SendJson(this WebSocket ws, object obj)
        {
            ws.Send(JsonConvert.SerializeObject(obj));
        }
    }
}
