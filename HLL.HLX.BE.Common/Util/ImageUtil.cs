using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using Abp.Logging;

namespace HLL.HLX.BE.Common.Util
{
    public static class ImageUtil
    {
        public static byte[] ImageToBytes(string imageFileName)
        {
            //Get an image from file
            Image image = Image.FromFile(imageFileName);
            //Image image = Image.FromFile("D:\\test.jpg");
            //Bitmap bitmap = new Bitmap("D:\\test.jpg");
            return ImageToBytes(image);
        }

        /// <summary>
        ///把一张图片（png bmp jpeg bmp gif）转换为byte数组。
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// byte数组转换为Image对象
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = Image.FromStream(ms);
            return image;
        }

        /// <summary>
        /// 从图片byte数组得到对应图片的格式，生成一张图片保存到磁盘上
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CreateImageFromBytes(string fullName, byte[] buffer)
        {
            //string file = fullName;

            //string fullName = AppDomain.CurrentDomain.BaseDirectory  + file;
            //System.IO.FileInfo info = new System.IO.FileInfo(fullName);                     
            //System.IO.Directory.CreateDirectory(info.Directory.FullName);

            //FileHelper.DeleteFile(fullName);
            File.WriteAllBytes(fullName, buffer);
            return fullName;
        }

        /// <summary>
        /// 获取图片的扩展名
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string GetImageExtension(byte[] buffer)
        {
            string extension = string.Empty;
            Image image = BytesToImage(buffer);
            ImageFormat format = image.RawFormat;
            if (format.Equals(ImageFormat.Jpeg))
            {
                extension = ".jpeg";
            }
            else if (format.Equals(ImageFormat.Png))
            {
                extension = ".png";
            }
            else if (format.Equals(ImageFormat.Bmp))
            {
                extension = ".bmp";
            }
            else if (format.Equals(ImageFormat.Gif))
            {
                extension = ".gif";
            }
            else if (format.Equals(ImageFormat.Icon))
            {
                extension = ".icon";
            }
            return extension;
        }

        //图片 转为 base64编码的文本
        public static string ImageToBase64String(string imgFilename)
        {
            try
            {
                Bitmap bmp = new Bitmap(imgFilename);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return strbaser64;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(string.Format("图片文件({0})转BASE64字符串出错", imgFilename), ex);
                return null;
            }
        }

        //base64编码的文本 转为 图片
        public static string Base64StringToImage(string imgBase64,string filePath)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(imgBase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                var filename = Guid.NewGuid().ToString() + ".jpg";
                var folder = HttpContext.Current.Server.MapPath(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fullname = Path.Combine(folder, filename);
                bmp.Save(fullname, System.Drawing.Imaging.ImageFormat.Jpeg);
                //bmp.Save(txtFileName + ".bmp", ImageFormat.Bmp);
                //bmp.Save(txtFileName + ".gif", ImageFormat.Gif);
                //bmp.Save(txtFileName + ".png", ImageFormat.Png);
                ms.Close();

                return filename;
            }
            catch (Exception ex)
            {                
                LogHelper.Logger.Error(string.Format("图片BASE64({0})转图片出错", imgBase64), ex);
                return null;
            }
        }
    }
}
