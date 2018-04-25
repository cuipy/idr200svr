using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace idr200Svr1
{
    class IdReader : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            String msg=e.Data;

            JsClient.sendAll(msg);

        }
        
    }
}
