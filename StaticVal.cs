using System;
using System.Collections.Generic;
using System.Text;

namespace idr200Svr1
{
    class StaticVal
    {


        // 日志存储文件夹
        public static string LogDir = "c:/idr200log";

        // 服务的日志级别，低于这个级别的日志将不会保存
        public static int LogLevel = IdrLog.Warning;

        // websocket监听端口
        public static int WebSocketPort = 8202;


        // base64图片前缀
        public static String Base64Prefix = "data:image/jpeg;base64,";


    }
}
