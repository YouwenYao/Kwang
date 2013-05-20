/*
 * Created by Ranorex
 * User: x93292
 * Date: 2013/5/8
 * Time: 14:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;
using System.Net.Sockets;
using Emerson.NetworkPower.Velocity;

namespace NformTester.lib
{
	/// <summary>
	/// Description of LxDeviceAvailable.
	/// </summary>
	public class LxDeviceAvailable
	{
		//**********************************************************************
		/// <summary>
		/// Constructer.
		/// </summary>
		public LxDeviceAvailable()
		{	
		}
		/// <summary>
		///SNMP device Key name in Device.ini
		/// </summary>
		public List<string> snmpKeyName;
		
		/// <summary>
		///Velocity device Key name in Device.ini
		/// </summary>
		public List<string> velKeyName;	
		
		/// <summary>
		///Set SNMP device Key name
		/// </summary>
		/// 
		public void setSnmpKeyName()
		{
			string[] keyName={
		    	"Default",
				"SNMP_GXT_1",
				"SNMP_GXT_2",
				"SNMP_GXT_3",
				"SNMP_GXT_4",
				"SNMP_GXT_5",
				"SNMP_device_0",
				"SNMP_device_1",
				"SNMP_device_2",
				"SNMP_SearchStart",
				"SNMP_SearchEnd",
				"SNMP_SingleAuto_0",
				"SNMP_SinAuto_0",
				"SNMP_SingleAuto_1",
				"SNMP_SingleManual_1",
				"SNMP_SwitchedPDU_0",
				"SNMP_GXT_Ip_Port",
				"SNMP_SingleAuto_1_NAME",
				"SNMP_GXT_5_NAME",
				"SNMP_GXT_0",
				"SNMP_GXT_0_NAME",
				"SNMP_GXT_1_NAME",
				"SNMP_SPDU_0"
		    };
			snmpKeyName = new List<string>(keyName);	
		}
		/// <summary>
		///Set Vel device Key name
		/// </summary>
		public void setVelKeyName()
		{
			string[] keyName={
				"Default",
				"Velocity_device_1",
				"Velocity_device_2",
				"Velocity_SearchStart",
				"Velocity_SearchEnd",
				"Velocity_SingleAuto_0",
				"Velocity_SinAuto_0",
				"Velocity_SingleAuto_1",
				"Velocity_SingleManual_1"
		    };	
			velKeyName = new List<string>(keyName);
		}
		
		/// <summary>
		///Get available snmp device list.
		/// </summary>
		public void CheckSnmpDevice()
		{
			setSnmpKeyName();
			CheckSNMPDeviceAvailable(snmpKeyName);
		}
		
		/// <summary>
		///Get available velocity device list.
		/// </summary>
		public void CheckVelDevice()
		{
			setVelKeyName();
			CheckVelocityDeviceAvailable(velKeyName);
		}
		
		 //**********************************************************************
		/// <summary>
		/// Check the SNMP devices in the Devices.ini are available or not.
		/// If some devices are unavailable, the Devices.ini should be changed to give
		/// tester all available devices.
		/// Author: Sashimi.
		/// </summary>
		public static List<String> CheckSNMPDeviceAvailable(List<string> keyName){			
			List<string> notAvailable = new List<string>();
		    // There are all devices in Device.ini.
			string groupName="SNMPDevices";
		    //GXT_Ip_Port=10.146.88.10:163
		    string keyName_IpPort = "SNMP_GXT_Ip_Port";
		    string ipPort = myparseToValue(groupName,keyName_IpPort);
		    string IP_Port = "";
			if(ipPort.IndexOf(":") != -1)
			{
				string[] spilt = ipPort.Split(':');
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
            	string strIP = myparseToValue(groupName,deviceIP);
	            try
	            {	            	
	            	if(!deviceIP.Equals("SNMP_GXT_Ip_Port"))
	            		endpoint.Address = IPAddress.Parse(strIP);
	            	else
	            		endpoint.Address = IPAddress.Parse(IP_Port);
	            	 resultdata = Messenger.Get(version,endpoint,community,varlist,timeout);
	            	 data = resultdata[0];	            
	            	 Console.WriteLine("The device:" + deviceIP + "("+ strIP +")"+ " is availabe");
	            }
	            catch(Exception ex)
	            {
	            	notAvailable.Add(deviceIP);
	            	Console.WriteLine("There is no device in this ip address."+ deviceIP + "("+ strIP +")!" );
	            	string log = ex.ToString();
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
		public static List<String> CheckVelocityDeviceAvailable(List<string> keyName){
			// There are all devices in Device.ini.
			List<string> notAvailable = new List<string>();		
			string groupName="VelocityDevices";	
         ClientEngine ve = ClientEngine.GetClientEngine();                                                  
            	
         string GDDpath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                                                 "mds");
         int startupflag = ve.Startup(GDDpath);
         Console.WriteLine("GDDPath:" + startupflag);
	     CommsChannel channel = ve.OpenChannel(CommStack.BACnetIp);
	     
	     foreach(string deviceIP in keyName){
		     if(channel != null)
		     {
		     	string strIP = myparseToValue(groupName,deviceIP);
		     	DeviceNode node = channel.StrToEntity(strIP);
		     	if(node != null)
		     	{
		     		Console.WriteLine("The device:" + deviceIP + "("+ strIP +")"+ " is availabe");
		     	}
		     	else
		     	{
		     		notAvailable.Add(deviceIP);
		     		Console.WriteLine("There is no device in this ip address."+ deviceIP + "("+ strIP +")!" );
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