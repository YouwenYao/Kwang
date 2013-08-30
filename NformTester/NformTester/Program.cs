/*
 * Created by Ranorex
 * User: y93248
 * Date: 2011-11-15
 * Time: 15:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WinForms = System.Windows.Forms;
using Emerson.NetworkPower.Velocity;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Reporting;
using Ranorex.Core.Testing;
using NformTester.lib;

namespace NformTester
{
    class Program
    {
    	/// <summary>
        /// Get all info from app.config.
        /// </summary>
    	private static IDictionary<string, string> GetConfigs ()
		{
			var configs = new Dictionary<string, string> ();
			int len = ConfigurationManager.AppSettings.Count;
			for (int i = 0; i < len; i++)
			{
				configs.Add (
					ConfigurationManager.AppSettings.GetKey (i),
					ConfigurationManager.AppSettings[i]);
			}

			return configs;
		}
    	
    	[STAThread]
        public static int Main(string[] args)
        {
 			/*
        	if(!CheckDeviceAvailable())
        	{
        		DialogResult dr = MessageBox.Show("Do you want to continue? Click Ok button to continue.", "Warning" , MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);
            	if(dr == DialogResult.No)
            	{
            		return 0;
            	}
        	}                   
            */

             var configs = GetConfigs ();
             string CheckDevice = configs["CheckDevice_BeforeTesting"];
             string RestoreDB = configs["RestoreDB_AfterEachTestCase"];
            
            // If CheckDevice is Y, program will check these ip addresses are available or not.
             if(CheckDevice.Equals("Y"))
             {
	           //stop Nform service
				Console.WriteLine("Stop Nform service...");
				string strRst = RunCommand("sc stop Nform");
			   //Be used to check devices are avalibale or not, which are configured in Device.ini
	           LxDeviceAvailable myDeviceAvailable = new LxDeviceAvailable();
	           myDeviceAvailable.CheckSnmpDevice();
	           myDeviceAvailable.CheckVelDevice();
	           //start Nform service
	           Console.WriteLine("Start Nform service...");
			   strRst = RunCommand("sc start Nform");	
             }                        
         
        	Keyboard.AbortKey = System.Windows.Forms.Keys.Pause;
            int error = 0;
            try
            {
                error = TestSuiteRunner.Run(typeof(Program), Environment.CommandLine);            	
            }
            catch (Exception e)
            {
               MessageBox.Show("Unexpected exception occurred:");
            	Report.Error("Unexpected exception occurred: " + e.ToString());
                error = -1;
            }
            return error;
        }
        
         //**********************************************************************
		/// <summary>
		/// Run cmd command
		/// </summary>
		public static string RunCommand(string command)
		{
			Process p = new Process();
			p.StartInfo.FileName = "cmd.exe";
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardInput = true;
			p.StartInfo.RedirectStandardOutput = false;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.CreateNoWindow = true;
			p.Start();
			p.StandardInput.WriteLine(command);
			p.StandardInput.WriteLine("exit");
			Delay.Duration(10000);
			return  "";
		}
        
    }
}
