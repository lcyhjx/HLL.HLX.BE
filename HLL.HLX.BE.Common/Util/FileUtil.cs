using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Common.Util
{
    /// <summary>
    /// 文件读写工具类
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="fullPath">文本文件的全路径</param>
        /// <returns>返回读取的文本文件字符串</returns>
        public static string FileRead(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return null;
            }
            CreateDirIfNotExist(fullPath);

            FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string content = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return content;
        }


        /// <summary>
        /// 写入文本到文本文件
        /// </summary>
        /// <param name="fullPath">文本文件的全路径</param>
        /// <param name="content">要写入的文本</param>
        public static void FileWrite(string fullPath, string content)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return;
            }
            CreateDirIfNotExist(fullPath);

            FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(content);
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 如果目录不存在，则创建
        /// </summary>
        /// <param name="fullPath">文件全路径</param>
        public static void CreateDirIfNotExist(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return;
            }
            string dir = Path.GetDirectoryName(fullPath);
            if (string.IsNullOrEmpty(dir))
            {
                return;
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }


        public static void DeleteFile(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return;
            }
            bool isExist = File.Exists(fullPath);
            if (isExist)
            {
                File.Delete(fullPath);
            }
        }
    }
}
