using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TZC
{
    static public class ConfigFileHelperClass
    {
        static Dictionary<string,int> zones = new Dictionary<string,int>();
        static ConfigFileHelperClass()
        {
            getConfig(ref zones);
            checkConfigFileChange();
        }
        static public Dictionary<string,int> getZones()
        {
            return zones;
        }
        public static void getConfig(ref Dictionary<string,int> zones)
        {
            zones.Clear();
            try
            {
                using (StreamReader sr = new StreamReader("zones.txt")) 
                {
                    
                    while (sr.Peek() >= 0)
                    {
                        string[] a = sr.ReadLine().Split(',');
                        string k = a[0];
                        int v = Convert.ToInt32(a[1]);
                        zones.Add(k,v);
                    }
                }
                
            }
            catch (Exception e)
            {
                zones.Add("nope", 0);
            }
        }
        public static DateTime lastConfigFileChange;
        public static bool checkConfigFileChange()
        {
            FileInfo fInfo = new FileInfo("zones.txt");
            DateTime lastWriteTime = fInfo.LastWriteTime;
            if ( lastWriteTime != lastConfigFileChange)
            {
                lastConfigFileChange = lastWriteTime;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
