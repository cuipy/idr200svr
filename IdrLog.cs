using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace idr200Svr1
{
    class IdrLog
    {
        public static int Debug = 1;

        public static int Info = 2;

        public static int Warning = 3;

        public static int Error = 4;


        public static void write(int level,string info)
        {
            if (level < StaticVal.LogLevel)
            {
                return;
            }

            string dir = StaticVal.LogDir;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string filePath = string.Format(dir+"/{0:yyyy-MM-dd}.log",DateTime.Now);

            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss} "+info,DateTime.Now));
            }
        }

    }
}
