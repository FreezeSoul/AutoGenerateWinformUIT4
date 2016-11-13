using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructure
{
    public class FileHelper
    {
        public List<string> GetInputParams(string fileName)
        {
            var list = new List<string>();
            return list;
        }

        public static StreamReader GetFileContent(string fileName)
        {
            StreamReader objReader = null;
            if (File.Exists(fileName))
            {
                objReader = new StreamReader(fileName);
            }
            else
            {
                string message = string.Format("指定文件不存在，文件路径：{0}", fileName);
                throw new Exception(message);
            }
            return objReader;
        }

        public static bool CheckFileExist(string filePah)
        {
            return File.Exists(filePah);
        }

        public static string GetFileContentStr(string fileName)
        {
            var returnstr = new StringBuilder();
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read),
                                          Encoding.Default);
                returnstr.Append(reader.ReadToEnd());
            }
            catch (Exception ex)
            {
                throw new Exception("Error In File Read|" + ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return returnstr.ToString();
        }

        public static bool WriteFile(string fileName, string strContent, FileMode fileMode)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(File.Open(fileName, fileMode, FileAccess.Write, FileShare.ReadWrite),
                                          Encoding.Default);
                writer.WriteLine(strContent);
                return true;
            }
            catch (Exception m)
            {
                throw new Exception(m.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public static byte[] ReadFileByte(string fileName, FileMode fileMode, FileAccess fileAccess)
        {
            var mystream = new FileStream(fileName, fileMode, fileAccess);
            var myread = new BinaryReader(mystream);
            var myfi = new FileInfo(fileName);
            var len = (int) myfi.Length;
            byte[] fileByte = myread.ReadBytes(len);
            return fileByte;
        }

        public static void DelFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}