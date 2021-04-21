using System;
using System.IO;
using System.Threading;
using System.Text;

namespace Progbase.FileIpc
{
    public static class FileSystem
    {
        public static void WriteTo(string filePath, string message)
        {
            FileStream fs = WaitForFileReady(filePath, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs);
            writer.Write(message);

            writer.Dispose();
            fs.Dispose();
        }

        public static string ReadFrom(string filePath)
        {
            WaitForContent(filePath);

            FileStream fs = WaitForFileReady(filePath, FileAccess.Read);

            StreamReader reader = new StreamReader(fs);
            string text = reader.ReadToEnd();
            reader.Dispose();
            fs.Dispose();

            File.Delete(filePath);

            return text;
        }

        private static FileStream WaitForFileReady(string filePath, FileAccess fileAccess)
        {
            while (true)
            {
                try
                {
                    return File.Open(filePath, FileMode.OpenOrCreate, fileAccess);
                }
                catch (Exception)
                {
                    Thread.Sleep(50);
                }
            }
        }

        private static void WaitForContent(string filePath)
        {
            while (!File.Exists(filePath))
            {
                Thread.Sleep(50);
            }

            while (new FileInfo(filePath).Length == 0)
            {
                Thread.Sleep(50);
            }
        }
    }
}