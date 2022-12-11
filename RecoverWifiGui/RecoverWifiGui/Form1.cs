using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RecoverWifiGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string recover()
        {


            Process processWifi = new Process();
            processWifi.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processWifi.StartInfo.FileName = "netsh";
            processWifi.StartInfo.Arguments = "wlan show profile";
            processWifi.StartInfo.UseShellExecute = false;
            processWifi.StartInfo.RedirectStandardError = true;
            processWifi.StartInfo.RedirectStandardInput = true;
            processWifi.StartInfo.RedirectStandardOutput = true;
            processWifi.StartInfo.CreateNoWindow = true;
            processWifi.Start();
            string output = processWifi.StandardOutput.ReadToEnd();
            string err = processWifi.StandardError.ReadToEnd();
            processWifi.WaitForExit();
            return output;
        }
        public string recoverpassword(string ssid)
        {


            Process processWifi = new Process();
            processWifi.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processWifi.StartInfo.FileName = "netsh";
            processWifi.StartInfo.Arguments = "wlan show profile " + ssid + " key=clear";
            processWifi.StartInfo.UseShellExecute = false;
            processWifi.StartInfo.RedirectStandardError = true;
            processWifi.StartInfo.RedirectStandardInput = true;
            processWifi.StartInfo.RedirectStandardOutput = true;
            processWifi.StartInfo.CreateNoWindow = true;
            processWifi.Start();
            string output = processWifi.StandardOutput.ReadToEnd();
            string err = processWifi.StandardError.ReadToEnd();
            processWifi.WaitForExit();
            return output;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            L1.Items.Clear();
           
            string DATA = recover();
            if (DATA.Contains("There is no wireless interface on the system"))
            {
                MessageBox.Show("Wifi Adapter Not Detected","WIFI Recovery",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            }
            string[] lines = DATA.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (line.Contains(":") && line.Contains("interface") == false)
                {

                    String[] sp = { ":", "" };
                    Int32 n = 2;

                    // using the method
                    String[] splited = line.Split(sp, n,
                           StringSplitOptions.RemoveEmptyEntries);
                    string extracted = recoverpassword(splited[1]);
                    string[] parsing = extracted.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string pars in parsing)
                    {

                        if (pars.Contains("Key Content"))
                        {


                            // using the method
                            String[] sppass = pars.Split(sp, n,
                                   StringSplitOptions.RemoveEmptyEntries);
                           var L = L1.Items.Add(splited[1]);

                            L.SubItems.Add(sppass[1]);
                        }
                    }
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by youhacker55", "WIFI Recovery", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start("https://github.com/youhacker55/");

        }
    }

}
