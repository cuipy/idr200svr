using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace idr200Svr1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //StaticVal.GlobalWebSocket = new WebSocket();
            //StaticVal.GlobalWebSocket.start(StaticVal.WebSocketPort);
            MyWebSocket.init(StaticVal.WebSocketPort);

            IdrLog.write(IdrLog.Debug, string.Format("{0:T},idr200 service started！", DateTime.Now));

            // 线程启动身份证监听
            Thread t1 = new Thread(new ThreadStart(IDListener.readId));
            t1.IsBackground = true;
            t1.Start();

        }

        protected override void OnStop()
        {
            IdrLog.write(IdrLog.Debug, string.Format("{0:T},idr200 service stoped！", DateTime.Now));
           
        }
    }
}
