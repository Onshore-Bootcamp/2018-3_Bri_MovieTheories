using System;
using System.IO;
using System.Configuration;

namespace DAL
{
    public class ErrorLogger
    {
        private static string _LogPath = ConfigurationManager.AppSettings["ErrorLogPath"];

        public void ErrorLog(string className, string methodName, Exception sqlEx, string level = "Error")
        {
            //Stack trace
            string stackTrace = sqlEx.StackTrace;

            using (StreamWriter errorWriter = new StreamWriter(_LogPath, true))
            {
                errorWriter.WriteLine(new string('-', 40));

                errorWriter.WriteLine($"Class: {className} Method: {methodName} / {DateTime.Now.ToString()} / {level}\n{sqlEx.Message}\n{stackTrace}");

                errorWriter.Close();
            }
        }
    }
}
