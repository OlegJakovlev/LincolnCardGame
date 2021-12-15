using System;
using System.IO;
using System.Text;

namespace DeckOfCards.Logs
{
    static class Logger
    {
        private static DateTime s_date;
        private static bool s_initialized = false;
        private static readonly string s_nameOfFile = "log.txt";

        public static void WriteData(string text)
        {
            if (!s_initialized && File.Exists(GetPath()))
            {
                File.Delete(GetPath());
                s_initialized = true;
            }

            s_date = DateTime.Now;

            File.AppendAllText(GetPath(), "["+s_date.ToString("F")+"]" +"\t"+ text+"\n");
        }

        private static string GetPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), s_nameOfFile);
        }
    }
}
