using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TZC
{
    public partial class MainForm : Form
    {
        TallyCounter fileLoadTallyCounter = new TallyCounter(5);                                               // keep track of when to check for a new config
        TallyCounter repaintTallyCounter = new TallyCounter(60 - DateTime.Now.Second + 1);                     // keep track of when to update the time shown (1/minute)

        Dictionary<string, int> zones = ConfigFileHelper.getZones();                            // holds the locations and time zone offsets

        public MainForm()                                                                       // ya gotta start somewhere
        {
            InitializeComponent();                                                              // form designer code kicking off

            int xPosition = (int)(Screen.PrimaryScreen.WorkingArea.Width * .85);                // putting the window in the upper right hand corner
            int yPosition = (int)(Screen.PrimaryScreen.WorkingArea.Height * .05);               //
            this.DesktopLocation = new Point(xPosition, yPosition);                             //

            SetupTable(ref zones, ref tbl);                                                     // initial set up and population of table and labels  
            UpdateTimes(ref zones, ref tbl);                                                    //
         }
        private void timer1_Tick(object sender, EventArgs e)                                    // manages time related tasks...
        {
            if (fileLoadTallyCounter.click() && ConfigFileHelper.checkConfigFileChange())            // checks if it's time to check the config, then checks
            {                                                                                   // for a change to the config, if so...
                ConfigFileHelper.updateConfig();                                                // re-reads the config file
                tbl.Controls.Clear();                                                           // clears the table
                SetupTable(ref zones, ref tbl);                                                 // re-sets up the tables and populates them
                UpdateTimes(ref zones, ref tbl);                                                //
            }

            if (repaintTallyCounter.click())                                                         // checks if it's time to show the next minute
            {                                                                                   //
                UpdateTimes(ref zones, ref tbl);                                                // if so... gets the new time and displays it
                repaintTallyCounter.resetMax(60 - DateTime.Now.Second + 1);
            }
        }
        private void SetupTable(ref Dictionary<string, int> zones, ref TableLayoutPanel tbl)    // puts a label for each location in the table
        {
            for (int i=0;i<zones.Count;i++)
            {
                Label l = new Label();
                l.Width = 120;
                l.Anchor = AnchorStyles.None;
                l.TextAlign = ContentAlignment.MiddleRight;
                //l.BorderStyle = BorderStyle.FixedSingle;
                tbl.Controls.Add(l, 1, tbl.Controls.Count);
            }
        }
        private void UpdateTimes(ref Dictionary<string, int> zones, ref TableLayoutPanel tbl)   // updates each label with new location data
        {
            int i = 0;
            foreach (KeyValuePair<string, int> kvp in zones)
            {
                tbl.Controls[i].Text = kvp.Key + " : " + GetTime(kvp.Value);
                i++;
            }
        }
        private string GetTime(int iOffset)                                                     // formats a string with the time at the reqeusted offset
        {
            if (DateTime.Now.IsDaylightSavingTime())
            {
                iOffset++;
            }
            DateTime dt = new DateTime();
            dt = DateTime.UtcNow.AddHours(iOffset);
            string s = dt.ToString("h:mm tt").Trim();
            if (s.Length==7){s = "  " + s;}
            return s;
        }
        private void MainForm_Load(object sender, EventArgs e)                                  // cause ya gotta...
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
    }
}
