/*
 * Created by Ranorex
 * User: y93248
 * Date: 2011-11-16
 * Time: 13:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace NformTester.lib
{
	//**************************************************************************
	/// <summary>
	/// Clean up the environment of test
	/// </summary>
	/// <para> Author: Peter Yao</para>
	public class LxTearDown
	{
		//**********************************************************************
		/// <summary>
		/// Constructer.
		/// </summary>
		public LxTearDown()
		{
		}
		
		//**********************************************************************
		/// <summary>
		/// Clean up all devices
		/// </summary>
		public void clearDevices()
		{
		}
		
		//**********************************************************************
		/// <summary>
		/// Clean up all actions
		/// </summary>
		public void clearActions()
		{
		}
		
		//**********************************************************************
		/// <summary>
		/// Clean up all user and groups
		/// </summary>
		public void clearUserAndGroups()
		{
		}
		
		//**********************************************************************
		/// <summary>
		/// Close the test appliction
		/// </summary>
		public static void closeApp(int processId)
		{
			try 
			{
				Host.Local.CloseApplication(processId);
			}
			catch(Exception e) 
			{
				LxLog.Info("Error",e.Message.ToString());
			}			
		}
	}
}
