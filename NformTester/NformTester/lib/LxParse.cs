/*
 * Created by Ranorex
 * User: y93248
 * Date: 2011-11-17
 * Time: 16:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Windows.Forms;

namespace NformTester.lib
{
	//**************************************************************************
	/// <summary>
	/// Preparatory validation for the scripts
	/// or both.
	/// </summary>
	/// <para> Author: Peter Yao</para>
	public class LxParse
	{
		/// <summary>
		/// Define a list contains all scripts
		/// </summary>
		public ArrayList m_Scripts =  new ArrayList();
		
		//**********************************************************************
		/// <summary>
		/// Constructer.
		/// </summary>
		public LxParse()
		{
		}
		
		//**********************************************************************
		/// <summary>
		/// Do validation check for each command, remove the scripts which
		/// not pass the validation
		/// </summary>
		public void doValidate()
		{
			ArrayList alNoUse = new ArrayList();
			foreach( LxScriptItem item in m_Scripts)
			{
				if( item.m_Type.CompareTo(";") == 0 )
				{
					alNoUse.Add(item);
				}
			}
			foreach(LxScriptItem item in alNoUse)
			{
				m_Scripts.Remove(item);
			}
		}
		
		//**********************************************************************
		/// <summary>
		/// Get the list of scripts.
		/// </summary>
		public ArrayList getStepsList()
		{
			return m_Scripts;
		}
		
	}
}
