/*
 * Created by Ranorex
 * User: y93248
 * Date: 2011-11-17
 * Time: 15:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using Ranorex;

namespace NformTester.lib
{
	//**************************************************************************
	/// <summary>
	/// Define a class to extend Report, Log can output to report and logfile,
	/// or both.
	/// </summary>
	/// <para> Author: </para>
	public class LxLog 
	{
		//**********************************************************************
		/// <summary>
		/// Constructer.
		/// </summary>
		public LxLog()
		{
		}
		
		/// <summary>
		/// Define a flag to indicate the output destination
		/// </summary>
		public static int flagOutput = 0;
		
		/// <summary>
		/// Define a path where to save the logfile
		/// </summary>
		public static string logFile = "";

		//**********************************************************************
		/// <summary>
		/// Set the flag how to log.
		/// </summary>
		public static void Setup(int flagOutput)
		{
			LxLog.flagOutput = flagOutput;
		}
		
		//**********************************************************************
		/// <summary>
		/// Info level log method.
		/// </summary>
		public static void Info(string category, string message)
		{
			if(flagOutput == 1)
			{
				LogToFile("Info",category, message);
				return;
			}
			
			Report.Info(category, message);
		}
		
		//**********************************************************************
		/// <summary>
		/// Error level log method.
		/// </summary>
		public static void Error(string category, string message)
		{
			if(flagOutput == 1)
			{
				LogToFile("Error",category, message);
				return;
			}
			Report.Error(category, message);
		}
		
		//**********************************************************************
		/// <summary>
		/// Internal method to save logfile.
		/// </summary>
		public static void LogToFile(string type, string category, string message)
		{
			
		}
	}
}
