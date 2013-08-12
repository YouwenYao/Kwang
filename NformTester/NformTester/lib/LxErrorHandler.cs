/*
 * Created by Ranorex
 * User: y93248
 * Date: 2011-11-16
 * Time: 14:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace NformTester.lib
{
	//**************************************************************************
	/// <summary>
	/// Custom action to handle the error when execute command.
	/// </summary>
	/// <para> Author:</para>
	public class LxErrorHandler
	{
		//**********************************************************************
		/// <summary>
		/// Constructer.
		/// </summary>
		public LxErrorHandler()
		{
		}
		
		//**********************************************************************
		/// <summary>
		/// If error happens when execute command, sometimes we need to retry this
		/// command several times to see if can be pass or not.
		/// </summary>
		public static void Retries(LxScriptItem errorItem, int retries)
		{
		}
		
		//**********************************************************************
		/// <summary>
		/// If error happens when execute command, sometimes we need to operator's 
		/// help, this method will popup a dialog and waite operator's action then
		/// keep moving on next command.
		/// </summary>
		public static void ShowDialogAndWaite(LxScriptItem errorItem, int waiteSeconds)
		{
		}
	}
}
