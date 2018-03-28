using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

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

                    IdrLog.write(IdrLog.Debug, "测试一下idr");
                    Thread.Sleep(300);

                    //打开端口
                    int intOpenRet = InitComm(1001);
                    if (intOpenRet != 1)
                    {
                        IdrLog.write(IdrLog.Debug, "阅读机具未连接");
                        continue;
                    }

                    //卡认证
                    int intReadRet = Authenticate();
                    if (intReadRet != 1)
                    {
                        IdrLog.write(IdrLog.Debug, "卡认证失败");
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


                    string idInfo = string.Format("Name:{0} Gender:{1} Folk:{2}, BirthDay:{3}, Code:{4}, Address:{5}, Agency:{6}, ExpireStart:{7}, ExpireEnd:{8}", Name, Gender, Folk, BirthDay, Code, Address, Agency, ExpireStart, ExpireEnd);
                    IdrLog.write(IdrLog.Info, idInfo);

                    //关闭端口
                    int intCloseRet = CloseComm();
                }catch{

                }
            }
        }

    }
}
