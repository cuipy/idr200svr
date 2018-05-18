using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace idr200Svr1
{
    class JsClient : WebSocketBehavior
    {


        private static Hashtable clients = new Hashtable();

        protected override void OnOpen()
        {
            StaticVal.WebSocketClientCount++;

            base.OnOpen();

            clients.Add(this.ID, this);

        }

        protected override void OnClose(CloseEventArgs e)
        {
            StaticVal.WebSocketClientCount--;

            base.OnClose(e);

            clients.Remove(this.ID);

        }

        public static void sendAll(String msg)
        {
            foreach(JsClient jsc in clients.Values){

                jsc.Send(msg);

            }
        }
        
    }
}
