/*
 * Created by Ranorex
 * User: y93248
 * Date: 2011-11-21
 * Time: 14:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using Ranorex.Core.Repository;

namespace NformTester.lib
{
	//**************************************************************************
	/// <summary>
	/// Define a data structure for a command line in scritps
	/// </summary>
	/// <para> Author: Peter Yao</para>
	public class LxScriptItem
	{
		/// <summary>
		/// Index of command
		/// </summary>
		public string m_Index;
		
		/// <summary>
		/// Type of command
		/// </summary>
		public string m_Type;
		
		/// <summary>
		/// WindowName, which form this command work on
		/// </summary>
		public string m_WindowName;
		
		/// <summary>
		/// Component, which component this command work on
		/// </summary>
		public string m_Component;
		
		/// <summary>
		/// Action of this command
		/// </summary>
		public string m_Action;
		
		/// <summary>
		/// Arguments of this command
		/// </summary>
		public string m_Arg1;
		/// <summary>
		/// Arguments of this command
		/// </summary>
		public string m_Arg2;
		/// <summary>
		/// Arguments of this command
		/// </summary>
		public string m_Arg3;
		/// <summary>
		/// Arguments of this command
		/// </summary>
		public string m_Arg4;
		/// <summary>
		/// Arguments of this command
		/// </summary>
		public string m_Arg5;
		/// <summary>
		/// Arguments of this command
		/// </summary>
		public string m_Arg6;
		
		
		/// <summary>
		/// Get the repository instance
		/// </summary>
		public static NformRepository repo = NformRepository.Instance;
		
		//**********************************************************************
		/// <summary>
		/// Constructer.
		/// </summary>
		public LxScriptItem()
		{
		}
		
		//**********************************************************************
		/// <summary>
		/// If this command has arguments, then return true.
		/// If this command has no arguments, then return false.
		/// </summary>
		public bool hasArg()
		{
			if (m_Arg1 == "" && m_Arg2 == "" && m_Arg3 == ""
			   	&& m_Arg4 == "" && m_Arg5 == "" && m_Arg6 == "")
			{
				return false;			
			}
			return true;
		}
		
		//**********************************************************************
		/// <summary>
		/// If argument has text, then remove symbol "
		/// </summary>
		public string getArgText()
		{
			return parseToValue(m_Arg1);
		}
		
		//**********************************************************************
		/// <summary>
		/// Get argument2
		/// </summary>
		public string getArg2Text()
		{
			return parseToValue(m_Arg2);
		}
		
		//**********************************************************************
		/// <summary>
		/// Get argument3
		/// </summary>
		public string getArg3Text()
		{
			return parseToValue(m_Arg3);
		}
		
		//**********************************************************************
		/// <summary>
		/// Get argument4
		/// </summary>
		public string getArg4Text()
		{
			return parseToValue(m_Arg4);
		}
		
		//**********************************************************************
		/// <summary>
		/// Get argument5
		/// </summary>
		public string getArg5Text()
		{
			return parseToValue(m_Arg5);
		}
		
		//**********************************************************************
		/// <summary>
		/// Get argument6
		/// </summary>
		public string getArg6Text()
		{
			return parseToValue(m_Arg6);
		}				
    	
		//**********************************************************************
		/// <summary>
		/// Replace the name with value refer to app.config
		/// </summary>
		public string parseToValue(string name)
        {
            if (name.Equals(""))
            {
                return "";
            }

			LxSetup mainOp = LxSetup.getInstance();
			var configs = mainOp.configs;
            string addr = name;
            if (name.Substring(0, 1) == "$" && name.Substring(name.Length - 1, 1) == "$")
            {
                string key = name.Substring(1, name.Length - 2);
                LxIniFile confFile = new LxIniFile(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                                                 "Devices.ini"));
                string result = null;
                
                if(configs.ContainsKey(key))
                {
                	result = configs[key];
                }
                else
                {
                	result = configs["Default"];
                }
                
                /*
                string def = null;
                if(key.IndexOf("SNMP")!=-1)
                {
                	def = confFile.GetString("SNMPDevices","Default","10.146.83.50");
                	result = confFile.GetString("SNMPDevices",key,def);
                }
                
                else if(key.IndexOf("Velocity")!=-1)
                {
                	def = confFile.GetString("VelocityDevices","Default","126.4.200.116");
                	result = confFile.GetString("VelocityDevices",key,def);
                }	
                
                else if(key.IndexOf("Trap")!=-1)
                {
                	def = confFile.GetString("TrapInfo","Default","-1:2:1:1.3.6.1.4.1.476.1.42.3.2.1.38.4");
                	result = confFile.GetString("TrapInfo",key,def);
                }
                
                 else if(key.IndexOf("Sys")!=-1)
                {
                	def = confFile.GetString("NformSysInfo","Default","Nform");
                	result = confFile.GetString("NformSysInfo",key,def);
                }
                 
                 else if(key.IndexOf("DB")!=-1)
                {
                	def = confFile.GetString("Database","Default","Nform");
                	result = confFile.GetString("Database",key,def);
                }
                 
                 else if(key.IndexOf("Try")!=-1)
                {
                	def = confFile.GetString("TryToRunTimes","Default","3");
                	result = confFile.GetString("TryToRunTimes",key,def);
                }
                */
               
                addr = result;
                
                confFile = new LxIniFile(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                                                 "UsedDevices.ini"));
                confFile.WriteString("AvailableDevices",key,result);
            }

            return addr.Replace("\"","");
        }
		
		//**********************************************************************
		/// <summary>
		/// According to the name of windows and component, find the componentinfo
		/// object in repository
		/// </summary>
		public RepoItemInfo getComponentInfo()
		
		{
			string windowsName = m_WindowName;
			string componentName = m_Component;
			Type objType = repo.GetType();
           
           	object obj = repo;
           	PropertyInfo pi = objType.GetProperty("NFormApp");
           	obj = pi.GetValue(repo,null);
           	objType = obj.GetType();
           	PropertyInfo[] piArrLev1 = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
       	   	foreach (PropertyInfo piLev1 in piArrLev1)
        	{	
       	   		if(piLev1.Name.CompareTo("UseCache") == 0)
       	   		{
       	   			continue;
       	   		}
       	   		object objLogicGroup = piLev1.GetValue(obj,null);
       	   		objType = objLogicGroup.GetType();
       	   		PropertyInfo[] piArrLev2 = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
       	   		foreach (PropertyInfo piLev2 in piArrLev2)
        		{
       	   			if(piLev2.Name.CompareTo(windowsName) == 0)
       	   			{
       	   				object objWindows = piLev2.GetValue(objLogicGroup,null);
       	   				objType = objWindows.GetType();
       	   				PropertyInfo[] piArrComp = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
       	   				foreach (PropertyInfo piCom in piArrComp)
        				{
       	   					if(piCom.Name.CompareTo(componentName + "Info") == 0)
       	   					{
       	   						RepoItemInfo objComponets = (RepoItemInfo)piCom.GetValue(objWindows,null);
       	   						return objComponets;
       	   					} 
       	   				} 
       	   			} 
       	   		}  
        	} 
       	   	
       	   	return null;
		}
		
		//**********************************************************************
		/// <summary>
		/// According to the name of windows and component, find the component
		/// object in repository
		/// </summary>
		public Object getComponent()
		{
			string windowsName = m_WindowName;
			string componentName = m_Component;
			Type objType = repo.GetType();

           	object obj = repo;
           	PropertyInfo pi = objType.GetProperty("NFormApp");
           	obj = pi.GetValue(repo,null);
           	objType = obj.GetType();
           	PropertyInfo[] piArrLev1 = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
       	   	foreach (PropertyInfo piLev1 in piArrLev1)
        	{	
       	   		if(piLev1.Name.CompareTo("UseCache") == 0)
       	   		{
       	   			continue;
       	   		}
       	   		object objLogicGroup = piLev1.GetValue(obj,null);
       	   		objType = objLogicGroup.GetType();
       	   		PropertyInfo[] piArrLev2 = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
       	   		foreach (PropertyInfo piLev2 in piArrLev2)
        		{
       	   			if(piLev2.Name.CompareTo(windowsName) == 0)
       	   			{
       	   				object objWindows = piLev2.GetValue(objLogicGroup,null);
       	   				objType = objWindows.GetType();
       	   				PropertyInfo[] piArrComp = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
       	   				foreach (PropertyInfo piCom in piArrComp)
        				{
       	   					if(piCom.Name.CompareTo(componentName) == 0)
       	   					{
       	   						object objComponets = piCom.GetValue(objWindows,null);
       	   						return objComponets;
       	   					} 
       	   				} 
       	   			} 
       	   		}  
         		
        	} 
       	   	
       	   	return null;
		}
	}
}
