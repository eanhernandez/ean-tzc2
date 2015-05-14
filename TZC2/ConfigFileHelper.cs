using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TZC
{
    static public class ConfigFileHelper
    {
        private static Dictionary<string,int> zones = new Dictionary<string,int>();     // stores locations and time zone offsets
        private static string configFileLocation = "zones.txt";                         // location of the config file
        private static DateTime lastConfigFileChange;                                   // last time the config file changed
        static ConfigFileHelper()                                                       // constructor
        {
             updateConfig();
        }                                                    
        public static Dictionary<string, int> getZones()                                // returns location and time zone offset dictionary
        {
            return zones;
        }              
        public static void updateConfig()                                               // loads locations and timezone offsets, sets lastConfigFileChange
        {
            zones.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(configFileLocation)) 
                {
                    while (sr.Peek() >= 0)
                    {
                        string[] a = sr.ReadLine().Split(',');
                        zones.Add(a[0],Convert.ToInt32(a[1]));  //brdcrp
                    }
                }
            }
            catch (Exception e)
            {
                zones.Add("fail", 0);
            }
            DateTime lastWriteTime = new FileInfo(configFileLocation).LastWriteTime;
            lastConfigFileChange = lastWriteTime;
        }
        public static bool checkConfigFileChange()                                      // checks if the config file has been changed
        {
            DateTime lastWriteTime = new FileInfo(configFileLocation).LastWriteTime;
            if (lastWriteTime != lastConfigFileChange)
                return true;
            else
                return false;
        }
    }
}
