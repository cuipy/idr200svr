using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace idr200Svr1
{
    // 身份证监听器
    class IDListener
    {
        [DllImport("Sdtapi.dll")]
        private static extern int InitComm(int iPort);
        [DllImport("Sdtapi.dll")]
        private static extern int Authenticate();
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseInfosPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd,String photoDir);
        [DllImport("Sdtapi.dll")]
        private static extern int CloseComm();
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseMsg(byte[] pMsg, ref int len);
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseMsgW(byte[] pMsg, ref int len);

        public static void readId()
        {

            StringBuilder Name = new StringBuilder(31);
            StringBuilder Gender = new StringBuilder(3);
            StringBuilder Folk = new StringBuilder(10);
            StringBuilder BirthDay = new StringBuilder(9);
            StringBuilder Code = new StringBuilder(19);
            StringBuilder Address = new StringBuilder(71);
            StringBuilder Agency = new StringBuilder(31);
            StringBuilder ExpireStart = new StringBuilder(9);
            StringBuilder ExpireEnd = new StringBuilder(9);

            while (true)
            {
                try
                {
                    // 如果没有客户端链接，则不占用读卡器资源
                    if (StaticVal.WebSocketClientCount <= 0)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    IdrLog.write(IdrLog.Warning, "连接数:" + StaticVal.WebSocketClientCount);

                    IdrLog.write(IdrLog.Debug, "测试一下idr");
                    Thread.Sleep(300);

                    //打开端口
                    int intOpenRet = InitComm(1001);
                    if (intOpenRet != 1)
                    {
                        IdrLog.write(IdrLog.Warning, "阅读机具未连接");
                        Thread.Sleep(10000);
                        continue;
                    }

                    //卡认证
                    int intReadRet = Authenticate();
                    if (intReadRet != 1)
                    {
                        IdrLog.write(IdrLog.Debug, "卡认证失败,可能的原因是您没有将身份证放在读卡器上。");
                        CloseComm();
                        continue;
                    }



                    //三种方式读取基本信息
                    //ReadBaseInfos（推荐使用）

                    int intReadBaseInfosRet = ReadBaseInfosPhoto(Name, Gender, Folk, BirthDay, Code, Address, Agency, ExpireStart, ExpireEnd,StaticVal.LogDir);
                    if (intReadBaseInfosRet != 1)
                    {
                        IdrLog.write(IdrLog.Error, string.Format("读卡失败,ReadBaseInfosRet:{0:d}", intReadBaseInfosRet));
                        CloseComm();
                        continue;
                    }

                    // 读取身份证图片，并转换为base64位数据
                    String p1base64 = StaticVal.Base64Prefix + ImgBase64.ImgToBase64String(StaticVal.LogDir + "/1.jpg");
                    String p2base64 = StaticVal.Base64Prefix + ImgBase64.ImgToBase64String(StaticVal.LogDir + "/2.jpg");
                    String p3base64 = StaticVal.Base64Prefix + ImgBase64.ImgToBase64String(StaticVal.LogDir + "/photo.bmp");
                    String p4base64 = StaticVal.Base64Prefix + ImgBase64.ImgToBase64String(StaticVal.LogDir + "/photo.jpg");

                    Hashtable ht = new Hashtable();
                    ht.Add("Name",Name.ToString());
                    ht.Add("Gender", Gender.ToString());
                    ht.Add("Folk", Folk.ToString());
                    ht.Add("BirthDay", BirthDay.ToString());
                    ht.Add("Code", Code.ToString());
                    ht.Add("Address", Address.ToString());
                    ht.Add("Agency", Agency.ToString());
                    ht.Add("ExpireStart", ExpireStart.ToString());
                    ht.Add("ExpireEnd", ExpireEnd.ToString());
                    ht.Add("p1base64", p1base64.ToString());
                    ht.Add("p2base64", p2base64.ToString());
                    //ht.Add("p3base64", p3base64.ToString());
                    ht.Add("p4base64", p4base64.ToString());

                    string idInfo = JsonTools.ObjectToJson(ht);
                    IdrLog.write(IdrLog.Debug, idInfo);

                    //关闭端口
                    int intCloseRet = CloseComm();

                    // 通知websocket客户端

                    JsClient.sendAll(idInfo);

                }
                catch (Exception e)
                {
                    IdrLog.write(IdrLog.Error, e.Message + "\n"+ e.StackTrace);
                }
            }
        }

    }
}
