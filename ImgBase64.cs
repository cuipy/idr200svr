using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;


namespace idr200Svr1
{
    class ImgBase64
    {

        public static string ImgToBase64String(string Imagefilename)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(Imagefilename))
                {

                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();

                    String strbaser64 = Convert.ToBase64String(arr);


                    return strbaser64;
                }
            }
            catch (Exception ex)
            {
                IdrLog.write(IdrLog.Error,"ImgToBase64String 转换失败\nException:" + ex.Message);
            }

            return null;
        }

    }
}
