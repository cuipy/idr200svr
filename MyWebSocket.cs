using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace idr200Svr1
{
	class MyWebSocket
	{

        public static void init(int port)
        {
            var wssv = new WebSocketServer(port);

            wssv.AddWebSocketService<JsClient>("/jsclient");
            wssv.AddWebSocketService<IdReader>("/idreader");

            wssv.Start();

        }
	
	
	}
}
