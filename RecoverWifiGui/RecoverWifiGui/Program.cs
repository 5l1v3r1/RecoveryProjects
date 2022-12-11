using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace RecoverWifiGui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)

        {
            try
            {
                if (args[0] == "Console")
                {
                    string DATA = recover();
                    if (DATA.Contains("There is no wireless interface on the system"))
                    {
                        Console.WriteLine("Wifi Adapter Not Detected ");
                        Console.ReadLine();
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
                                    Console.WriteLine("NetworkName:" + splited[1] + " Password:" + sppass[1]);
                                }
                            }
                        }


                    }
                    Console.ReadKey();
                    Environment.Exit(0);
                


                }
                else if(args[0] == "Gui"){
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                else
                {
                    Console.WriteLine("Available args Gui,Console");
                    Console.ReadLine();
                }


                
            }
            catch (IndexOutOfRangeException)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
      
            }
            
        }
        static string recover()
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
        static string recoverpassword(string ssid)
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
    }
}
