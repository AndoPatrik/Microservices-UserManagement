using System;
using System.IO;

namespace UserManagementAPI.Models
{
    public class Logger
    {
        public void LogAction(string logmsg)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            string path = dateTime.ToString("dd-MM-yyyy") + "_log.txt";
            string fullPath = Path.GetFullPath(path);

            try
            {
                StreamWriter sw = new StreamWriter(fullPath, true);

                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " - " + logmsg);

                sw.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
