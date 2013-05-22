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
        /// Result of backup database. Used by restore database of every script.
        /// </summary>
    	public static bool BackupResult = false;
    	
    	/// <summary>
        /// Result of backup database. Used by restore database of every script.
        /// </summary>
    	public static LxDBOper myLxDBOper = new LxDBOper();
    	
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
         
           /*comment for test         
            //stop Nform service
			Console.WriteLine("Stop Nform service...");
			string strRst = RunCommand("sc stop Nform");
		   //Be used to check devices are avalibale or not, which are configured in Device.ini
           LxDeviceAvailable myDeviceAvailable = new LxDeviceAvailable();
           myDeviceAvailable.CheckSnmpDevice();
           // myDeviceAvailable.CheckVelDevice();
           
           //Backup Database operation. Just do once before run all scripts.
            myLxDBOper.SetDbType();
            myLxDBOper.BackUpDataBase();
            if(myLxDBOper.GetBackUpResult() == false)
            {
               Console.WriteLine("Back up database is faild!");
            }
            else
            {
            	Console.WriteLine("Back up database is successful!");
            }
    
            //start Nform service
            Console.WriteLine("Start Nform service...");
			RunCommand("sc start Nform");	
       */
          
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
			//string strRst = p.StandardOutput.ReadToEnd();
			Delay.Duration(10000);
			return  "";
		}
        
    }
}
