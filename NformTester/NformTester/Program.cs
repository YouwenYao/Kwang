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

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Reporting;
using Ranorex.Core.Testing;
using NformTester.lib;

namespace NformTester
{
    class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
        	//Check SNMP device configured in device.ini is available.
        	List<string> NotAvailableSNMPDevice = CheckSNMPDeviceAvailable();
        	if(NotAvailableSNMPDevice!= null && NotAvailableSNMPDevice.Count>0)
        	{
        		Console.WriteLine("These devices are not available!");
        		foreach(string s_IP in NotAvailableSNMPDevice)
        		{
        			Console.WriteLine("Unavailable SNMP device is:" + s_IP);
        		}
        		MessageBox.Show("Do you want to continue? Click Ok button to continue.");
        	}
        	
        	//Check velocity device configured in device.ini is available.
        	List<string> NotAvailableVelocityDevice = CheckVelocityDeviceAvailable();
            if(NotAvailableVelocityDevice!= null && NotAvailableVelocityDevice.Count>0)
        	{
        		Console.WriteLine("These devices are not available!");
        		foreach(string s_IP in NotAvailableVelocityDevice)
        		{
        			Console.WriteLine("Unavailable Velocity devices :" + s_IP);
        		}
        		MessageBox.Show("Do you want to continue? Click Ok button to continue.");
        	}
            
        	
        	Keyboard.AbortKey = System.Windows.Forms.Keys.Pause;
            int error = 0;
            try
            {
                error = TestSuiteRunner.Run(typeof(Program), Environment.CommandLine);
            }
            catch (Exception e)
            {
                Report.Error("Unexpected exception occurred: " + e.ToString());
                error = -1;
            }
            return error;
        }
        
        	//**********************************************************************
		/// <summary>
		/// Check the SNMP devices in the Devices.ini are available or not.
		/// If some devices are unavailable, the Devices.ini should be changed to give
		/// tester all available devices.
		/// Author: Sashimi.
		/// </summary>
		public static List<String> CheckSNMPDeviceAvailable(){			
			List<string> notAvailable = new List<string>();
		    // There are all devices in Device.ini.
			string groupName="SNMPDevices";
		    string[] keyName={
		    	"GXT_1",
		    	"GXT_2",
		    	"GXT_3",
		    	"GXT_4",
		    	"GXT_5",
		    	"SNMPdevice_0",
		    	"SNMPdevice_1",
		    	"SNMPdevice_2",
		    	"Velocitydevice_1",
		    	"Velocitydevice_2",
		    	"SearchStart",
		    	"SearchEnd",
		    	"Default",
		    	"SingleAuto_0",
		    	"SinAuto_0",
		    	"SingleAuto_1",
		    	"SingleManual_1",
		    	"SwitchedPDU_0",
		    	"GXT_Ip_Port"
		    };
		    
		    //GXT_Ip_Port=10.146.88.10:163
		    string keyName_IpPort = "GXT_Ip_Port";
		    string ipPort = myparseToValue(groupName,keyName_IpPort);
		    string IP_Port = "";
			if(ipPort.IndexOf(":") != -1)
			{
				string[] spilt = ipPort.Split(':');
				//IP = strDestination.Substring(0,strDestination.IndexOf(":"));
				IP_Port = spilt[0];
			}
		    
			int timeout = 3;
			VersionCode version = VersionCode.V1;
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("1.1.1.1"),161);
			OctetString community = new OctetString("public");
            
            ObjectIdentifier objectId = new ObjectIdentifier("1.3.6.1.2.1.11.1.0");
            Variable var = new Variable(objectId);   
            IList<Variable> varlist = new System.Collections.Generic.List<Variable>();
            varlist.Add(var);
            Variable data;
            IList<Variable> resultdata;
            
            foreach(string deviceIP in keyName){
	            try
	            {
	            	if(!deviceIP.Equals("GXT_Ip_Port"))
	            		endpoint.Address = IPAddress.Parse(myparseToValue(groupName,deviceIP));
	            	else
	            		endpoint.Address = IPAddress.Parse(IP_Port);
	            	 resultdata = Messenger.Get(version,endpoint,community,varlist,timeout);
	            	 data = resultdata[0];
	            	 Console.WriteLine("Variable data :" + data.Data.ToString() + "in this device:" + deviceIP);
	            }
	            catch(Exception ex)
	            {
	            	notAvailable.Add(deviceIP);
	            	Console.WriteLine("There is no device in this ip address."+ deviceIP +"!" + ex.ToString());
	            	continue;
	            }
            }
   
		    return notAvailable;
	   }
		
		
         //**********************************************************************
		/// <summary>
		/// Check the velocity devices in the Devices.ini are available or not.
		/// If some devices are unavailable, the Devices.ini should be changed to give
		/// tester all available devices.
		/// Author: Sashimi.
		/// </summary>
		public static List<String> CheckVelocityDeviceAvailable(){
			// There are all devices in Device.ini.
			List<string> notAvailable = new List<string>();		
			string groupName="VelocityDevices";
		    string[] keyName={				
				"Velocitydevice_1",
		    	"Velocitydevice_2",
		    	"VelocitySearchStart",
		    	"VelocitySearchEnd",
		    	"VelocityDefault",
		    	"VelocitySingleAuto_0",
		    	"VelocitySingleAuto_1",
		    	"VelocitySingleManual_1",
		    };
			
         ClientEngine ve = ClientEngine.GetClientEngine();
         string GDDpath= @"D:\Velocity";
         int startupflag = ve.Startup(GDDpath);
         Console.WriteLine("GDDPath:" + startupflag);
	     CommsChannel channel = ve.OpenChannel(CommStack.BACnetIp);
	     
	     foreach(string deviceIP in keyName){
		     if(channel != null)
		     {
		     	DeviceNode node = channel.StrToEntity(myparseToValue(groupName,deviceIP));
		     	if(node != null)
		     	{
		     		Console.WriteLine("device is available!");
		     	}
		     	else
		     	{
		     		notAvailable.Add(deviceIP);
		     		Console.WriteLine("device is not available!");
		     	}
		     }
		     else
		     {
		     	Console.WriteLine("This channel is not available!");
		     }
	     }
	     channel.Dispose();
	     return notAvailable;
		}
		
		//**********************************************************************
		/// <summary>
		/// Parse the value from Devices.ini.
		/// Author: Sashimi.
		/// </summary>
		public static string myparseToValue(string GroupName, string key)
        {
		  LxIniFile confFile = new LxIniFile(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                                                 "Devices.ini"));
          string def = confFile.GetString(GroupName, "Default", "null");
          string result = confFile.GetString(GroupName, key, def);
          return result;
        }
    }
}
