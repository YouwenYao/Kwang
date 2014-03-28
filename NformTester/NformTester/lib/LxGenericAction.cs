﻿/*
 * Created by Ranorex
 * User: y93248
 * Time: 14:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Drawing;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace NformTester.lib
{
	//**************************************************************************
	/// <summary>
	/// Map the scripts's action to the method that contol in Ranorex will use.
	/// Perform scripts items
	/// </summary>
	/// <para> Author: Peter Yao</para>
	public class LxGenericAction
	{
		/// <summary>
		/// Get the repository instance
		/// </summary>
		public static NformRepository repo = NformRepository.Instance;
		
		/// <summary>
		/// Define a hashtable to map all the action the scripts to the real method that
		/// will be excuted
		/// </summary>
		public static Hashtable m_ActionMap = new Hashtable();
		
		
		/// <summary>
		/// Define a hashtable to include all managed application process
		/// </summary>
		public static Hashtable m_AppProcess = new Hashtable();
		
		//**********************************************************************
		/// <summary>
		/// Constructer.
		/// </summary>
		public LxGenericAction()
		{			
		}
		
		//**********************************************************************
		/// <summary>
		/// Map the action and call executeCommand to run script's steps.
		/// </summary>
		public static bool performScripts(ArrayList stepList)
		{
			LxSetup mainOp = LxSetup.getInstance();
			
			// Map action
			m_ActionMap.Clear();
			m_ActionMap.Add("InputKeys", new object[] {"PressKeys","1"});
			m_ActionMap.Add("Click", new object[] {"Click","0"});			
			m_ActionMap.Add("Set", new object[] {"Check","0"});
			m_ActionMap.Add("Clear", new object[] {"Uncheck","0"});
			// m_ActionMap.Add("UnverifiedClickTab", new object[] {"Click","0"});
			m_ActionMap.Add("DoubleClick", new object[] {"DoubleClick","0"});
			m_ActionMap.Add("Collapse", new object[] {"CollapseAll","0"});
			m_ActionMap.Add("Expand", new object[] {"ExpandAll","0"});
			m_ActionMap.Add("Select", new object[] {"SelectedItemText","S1"});
			m_ActionMap.Add("CellContentClick", new object[] {"Click","S2"});
			m_ActionMap.Add("SetTextValue", new object[] {"PressKeys","S3"});
			m_ActionMap.Add("ClickItem", new object[] {"Click","S4"});
			m_ActionMap.Add("VerifyToolTips", new object[] {"MoveTo","S5"});
			m_ActionMap.Add("VerifyContains", new object[] {"Verify","S6"});
			m_ActionMap.Add("VerifyNotContains", new object[] {"Verify","S7"});
			m_ActionMap.Add("MoveTo", new object[] {"MoveTo","S8"});
			m_ActionMap.Add("ClickCell", new object[] {"Click","S9"});
			m_ActionMap.Add("DoubleClickItem", new object[] {"DoubleClick","S10"});
			m_ActionMap.Add("RightClick", new object[] {"RightClick","S11"});
			m_ActionMap.Add("InputCell", new object[] {"Click","S12"});

			// Run the item in stepList
			// If wrongCount =3, it means that the command fails three times continuously.
			int wrongTime = 3;
			int wrongCount = 0;
			string groupName="TryToRunTimes";
            string key="Try_Times";
            wrongTime = int.Parse(myparseToValue(groupName,key));
    //        MessageBox.Show("wrong Time ="+wrongTime);
			
            
			bool finalResult = true;
			foreach(LxScriptItem item in stepList)
			{
				bool resultFlag = true;
				try 
				{
					resultFlag = executeCommand(item);
					wrongCount = 0;
				}
				catch(Exception e) 
				{
					wrongCount++;
					resultFlag = false;
					finalResult = false;
					LxLog.Error("Error",e.Message.ToString());
				}
						
				// Log each step is pass or not
				mainOp.opXls.writeCell(Convert.ToInt32(item.m_Index)+1,14,resultFlag==true?"Pass":"Fail");
				LxLog.Info("Info",item.m_Index+" "+item.m_Component+" "+item.m_Action+" "+ (resultFlag==true?"Success":"Failure"));
						
		         //If this script fails three times continuously, break this execution.
			     if(wrongCount==wrongTime) 
			     	break;
			}
				
			return finalResult;
		}
		
		//**********************************************************************
		/// <summary>
		/// Execute form command.
		/// </summary>
		public static bool executeFormCommand(LxScriptItem item)
		{
			string action = item.m_Component;
			switch(action) 
			{
				case "NformLogin":
				Login(item);
				break;	
				case "Add_Device":
				Add_Device(item);
				break;
				case "Del_Device":
				Del_Device(item);
				break;
				case "Add_Action":
				Add_Action(item);
				break;
				default:
				break;		
			}
			return true;
		}
		
		//**********************************************************************
		/// <summary>
		/// Form action: Login in.
		/// </summary>
		public static void Login(LxScriptItem item)
		{
			repo.NFormApp.LogintoLiebertNformWindow.FormLogin_to_LiebertR_Nform.Username.PressKeys(item.getArgText());
			repo.NFormApp.LogintoLiebertNformWindow.FormLogin_to_LiebertR_Nform.Password.PressKeys(item.getArg2Text());
			repo.NFormApp.LogintoLiebertNformWindow.FormLogin_to_LiebertR_Nform.ServerCombo.PressKeys(item.getArg3Text());
			repo.NFormApp.LogintoLiebertNformWindow.FormLogin_to_LiebertR_Nform.Login.Click();
			Delay.Milliseconds(3000);
			if(repo.NFormApp.LogintoLiebertNformWindow.FormEvaluation_Period_Expiration.OKInfo.Exists())
			{
				repo.NFormApp.LogintoLiebertNformWindow.FormEvaluation_Period_Expiration.OK.Click();
			}
		}
		
		//**********************************************************************
		/// <summary>
		/// Form action: Add device.
		/// </summary>
		public static void Add_Device(LxScriptItem item)
		{
			repo.NFormApp.NformG2Window.FormMain.Configure.Click();
			repo.NFormApp.NformG2Window.FormMain.Devices.Click();
			repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Add.Click();
			
			if(item.getArgText() == "SingleAuto")
			{
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Next.Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Hostname_or_IP_address.PressKeys(item.getArg2Text());
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Obtain_setting_from_device.Check();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Next.Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Finish.Click();
				Delay.Milliseconds(5000);
				repo.NFormApp.AddDeviceWizard.FormAdd_Device_Results.OK.Click();
				repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Close.Click();				
			}
			if(item.getArgText() == "SingleManual")
			{
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Next.Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Hostname_or_IP_address.PressKeys(item.getArg2Text());
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Obtain_setting_from_device.Uncheck();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Next.Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Name.PressKeys(item.getArg3Text());
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Description.PressKeys(item.getArg4Text());
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Device_type.SelectedItemText = item.getArg5Text();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Device_protocol.SelectedItemText = item.getArg6Text();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Finish.Click();
				Delay.Milliseconds(5000);
				repo.NFormApp.AddDeviceWizard.FormAdd_Device_Results.OK.Click();
				repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Close.Click();				
			}
			
			if(item.getArgText() == "MultiSearch")
			{
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Discover_devices.Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Next.Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.DeleteRow.Click();
				while(repo.NFormApp.AddDeviceWizard.FormAdd_Device.DeleteRow.Enabled==true)
				{
					repo.NFormApp.AddDeviceWizard.FormAdd_Device.DeleteRow.Click();
				}
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Search_device_table.Rows[1].Cells[0].Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Search_device_table.Rows[1].Cells[1].Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Search_device_table.Rows[1].Cells[1].Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Search_device_table.Rows[1].Cells[1].PressKeys(item.getArg2Text() + "{TAB}{CONTROL down}{Akey}{CONTROL up}" +item.getArg3Text());
				Delay.Duration(1000);				
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Next.Click();
				Delay.Duration(40000);
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Next.Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Select_all.Click();
				repo.NFormApp.AddDeviceWizard.FormAdd_Device.Finish.Click();
				Delay.Duration(20000);
				repo.NFormApp.AddDeviceWizard.FormAdd_Device_Results.OK.Click();
				repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Close.Click();								
			}			
		}
		
		//**********************************************************************
		/// <summary>
		/// Form action: Del device.
		/// </summary>
		public static void Del_Device(LxScriptItem item)
		{
			repo.NFormApp.NformG2Window.FormMain.Configure.Click();
			repo.NFormApp.NformG2Window.FormMain.Devices.Click();
			if(item.getArgText() == "SingleDel")
			{
				repo.myCustom = item.getArg2Text();
				repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.CellVariables.Click();
				repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Delete.Click();
				repo.NFormApp.ManagedDevicesWindow.FormConfirm_Device_Delete.Yes.Click();
			}
			if(item.getArgText() == "AllDel")
			{
				repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Managed_device_table.Click();
				repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Managed_device_table.PressKeys("{CONTROL down}{Akey}{CONTROL up}");
				repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Delete.Click();
				repo.NFormApp.ManagedDevicesWindow.FormConfirm_Device_Delete.Yes.Click();
				Delay.Duration(5000);	
			}	
			repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Close.Click();	
		}
		
		//**********************************************************************
		/// <summary>
		/// Form action: VerifyProperty equal, Contains, NotContains.
		/// </summary>
		public static void VerifyProperty(LxScriptItem item)
		{	
			//MessageBox.Show(item.getComponent().ToString());
			// The component is lable
			string testtemp = item.getComponent().ToString();
			if(item.getComponent().ToString().IndexOf("Lbl") != -1)
			{
				Ranorex.NativeWindow nativeWnd = item.getComponentInfo().CreateAdapter<Ranorex.NativeWindow>(false);
				string lableText = nativeWnd.WindowText;
				
				if(item.getArg2Text() == "Equal")
				{string abc = item.getArg3Text();
					Validate.AreEqual(lableText, item.getArg3Text());
				}
				if(item.getArg2Text() == "Contains")
				{
					int iFlag = lableText.IndexOf(item.getArg3Text());
					Validate.IsTrue(iFlag != -1);
				}	
				if(item.getArg2Text() == "NotContains")
				{
					int iFlag = lableText.IndexOf(item.getArg3Text());
					Validate.IsTrue(iFlag == -1);
				}				
				return;
			}

			if(item.getArg2Text() == "Equal")
			{
				Validate.Attribute(item.getComponentInfo(), item.getArgText(), item.getArg3Text());
			}
			if(item.getArg2Text() == "Contains")
			{
				Validate.Attribute(item.getComponentInfo(), item.getArgText(), new Regex(Regex.Escape(item.getArg3Text())));
			}	
			if(item.getArg2Text() == "NotContains")
			{
				Validate.Attribute(item.getComponentInfo(), item.getArgText(), new Regex("^((?!("+Regex.Escape(item.getArg3Text())+")).)*$"));
			}
			
			if(item.getArg2Text() == "ListContains")
			{
				bool Resultflag = false; 
				object objComponet = item.getComponent();
			    Ranorex.ComboBox myComboBox = (Ranorex.ComboBox)(objComponet);
			    Ranorex.Button btn = myComboBox.FindSingle("./button");  
			    btn.Click();  
			    List lst = "/list"; 
			    foreach (ListItem lst_item in lst.FindChildren<ListItem>())  
			    {
			    	if((lst_item.Text).Equals(item.getArg3Text()))
			    	{
			    		Resultflag = true;
			        	break;
			    	}
			    }
			    btn.Click();
		        Validate.AreEqual(Resultflag,true);	
		      
			}
		}
		
		/// <summary>
		/// Form action: Add Actions
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// 
		public static void Add_Action(LxScriptItem item)
		{
			repo.NFormApp.NformG2Window.FormMain.Configure.Click();
			repo.NFormApp.NformG2Window.FormMain.Actions.Click();
			
			//Action Set
			repo.NFormApp.ActionSetsWindow.FormAction_Sets.Add.Click();
			repo.NFormApp.ActionSetsWindow.FormAction_Set_Properties.Name.PressKeys("Action Test 2");
			repo.NFormApp.ActionSetsWindow.FormAction_Set_Properties.Description.PressKeys("Action Test 1 Desc");
			
			//E-mail
			repo.NFormApp.ActionSetsWindow.FormAction_Set_Properties.Add.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.E_mail.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.OK.Click();
			repo.NFormApp.ActionSetsWindow.FormE_mail_Action_Properties.Name.PressKeys("E-mail action test 1");
			repo.NFormApp.ActionSetsWindow.FormE_mail_Action_Properties.Description.PressKeys("E-mail action test 1 desc");
			repo.NFormApp.ActionSetsWindow.FormE_mail_Action_Properties.Other_recipients.PressKeys("test@test.com");
			repo.NFormApp.ActionSetsWindow.FormE_mail_Action_Properties.Subject.PressKeys("E-mail action test 1 subject");
			repo.NFormApp.ActionSetsWindow.FormE_mail_Action_Properties.Message_body.PressKeys("E-mail action test 1 body");
			repo.NFormApp.ActionSetsWindow.FormE_mail_Action_Properties.OK.Click();
			
			//Run command
			repo.NFormApp.ActionSetsWindow.FormAction_Set_Properties.Add.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.Run_command.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.OK.Click();
			repo.NFormApp.ActionSetsWindow.FormRun_Command_Action_Properties.Name.PressKeys("Run Command test 1");
			repo.NFormApp.ActionSetsWindow.FormRun_Command_Action_Properties.Description.PressKeys("Run command test 1 desc");
			repo.NFormApp.ActionSetsWindow.FormRun_Command_Action_Properties.File.PressKeys("D:\\RunCommandActiontest.txt");
			repo.NFormApp.ActionSetsWindow.FormRun_Command_Action_Properties.OK.Click();
			
			//Write file - this may not work due to Recording Format field is not populated by script.
			repo.NFormApp.ActionSetsWindow.FormAction_Set_Properties.Add.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.Write_file.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.OK.Click();
			repo.NFormApp.ActionSetsWindow.FormWrite_File_Action_Properties.Name.PressKeys("Write File Action Test 1");
			repo.NFormApp.ActionSetsWindow.FormWrite_File_Action_Properties.Description.PressKeys("Write File Action Test 1 Desc");
			repo.NFormApp.ActionSetsWindow.FormWrite_File_Action_Properties.File.PressKeys("D:\\WriteFileActionTest1.txt");
			repo.NFormApp.ActionSetsWindow.FormWrite_File_Action_Properties.Record_foramt.Click();
			repo.NFormApp.ActionSetsWindow.FormWrite_File_Action_Properties.Record_foramt.PressKeys("text");
			repo.NFormApp.ActionSetsWindow.FormWrite_File_Action_Properties.OK.Click();
			
			//Forward Trap
			repo.NFormApp.ActionSetsWindow.FormAction_Set_Properties.Add.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.Forward_trap.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.OK.Click();
			repo.NFormApp.ActionSetsWindow.FormForward_Trap_Action_Properties.Name.PressKeys("Forward Trap Action test 1");
			repo.NFormApp.ActionSetsWindow.FormForward_Trap_Action_Properties.Description.PressKeys("Foward Trap Action test 1 desc");
			repo.NFormApp.ActionSetsWindow.FormForward_Trap_Action_Properties.HostnameOrIPAddressRow0.PressKeys("126.4.100.100");
			repo.NFormApp.ActionSetsWindow.FormForward_Trap_Action_Properties.PortNumberRow0.PressKeys("162");
			repo.NFormApp.ActionSetsWindow.FormForward_Trap_Action_Properties.OK.Click();
			
			//Read Data - this may not work since managed device is dymanic and hard to 
			repo.NFormApp.ActionSetsWindow.FormAction_Set_Properties.Add.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.Read_data.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.OK.Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Name.PressKeys("Read data action test 1");
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Description.PressKeys("Read data action test 1 desc");
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Managed_devices.Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Managed_devices.Items[0].Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Device_datapoint_table.Rows[1].Cells[0].Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.OK.Click();
			
			/*
			//Write Data - this may not work since managed device is dymanic and hard to 
			repo.NFormApp.ActionSetsWindow.FormAction_Set_Properties.Add.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.Write_data.Click();
			repo.NFormApp.ActionSetsWindow.FormSelect_Action_Type.OK.Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Name.PressKeys("Write Data Action Test 1");
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Description.PressKeys("Write Data Action Test 1 Desc");
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Managed_devices.Click();
			repo.List1000.Cooling12642022171264202217.Click();	//this may not work.
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Cell_Row_0.Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.NewValueRow0.Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Selected_datapoint_table.Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Selected_datapoint_table.Rows[1].Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.Panel.Click();
			repo.NFormApp.ActionSetsWindow.FormReadWrite_Data_Action_Properties.OK.Click();
			*/
			repo.NFormApp.ActionSetsWindow.FormAction_Set_Properties.OK.Click();
			repo.NFormApp.ActionSetsWindow.FormAction_Sets.Close.Click();
		}

		
		//**********************************************************************
		/// <summary>
		/// Execute one command, parse the command.
		/// </summary>
		public static bool executeCommand(LxScriptItem item)
		{
			if(item.m_Type == "F") 
			{
				return executeFormCommand(item);
			}
			
			if(item.m_Type == "C" && item.m_WindowName == "Pause") 
			{
				Delay.Milliseconds(Convert.ToInt32(item.m_Component) * 1000);
				return true;
			}
			
			if(item.m_Type == "C" && item.m_WindowName == "VerifyTxtfileValues") 
			{				
				VerifyTxtfileValues(item);
				return true;
			}
			
			if(item.m_Type == "C" && item.m_WindowName == "VerifyExcelfileValues") 
			{				
				VerifyExcelfileValues(item);
				return true;
			}
			
			if(item.m_Type == "C" && item.m_WindowName == "SendCommandToSimulator") 
			{				
				SendCommandToSimulator(item);
				return true;
			}
			
			if(item.m_Type == "C" && item.m_WindowName == "CopyDataToFile") 
			{				
				CopyDataToFile(item);
				return true;
			}
			
			if(item.m_Type == "C" && item.m_WindowName == "LaunchApplication") 
			{				
				AppStart(item);
				return true;
			}
			
			if(item.m_Type == "C" && item.m_WindowName == "CloseApplication") 
			{				
				AppClose(item);
				return true;
			}
					
			if(item.m_Type.Substring(0,1) == ";")
			{
				return true;
			}
			
			if(item.m_Action == "Exists")
			{
				Validate.Exists(item.getComponentInfo());
				return true;
			}
			
			if(item.m_Action == "NotExists")
			{
				Validate.NotExists(item.getComponentInfo());
				return true;
			}
			
			if(item.m_Action == "VerifyProperty")
			{
				//PropertyInfo property = objType.GetProperty(item.m_Arg1);
				//property.GetValue(objComponet, null).ToString();
				//Validate.Attribute(item.getComponentInfo(),item.m_Arg1,item.m_Arg2);
				//Validate.Attribute(
				VerifyProperty(item);
				return true;
			}
			
			object[] arg = (object[])m_ActionMap[item.m_Action]; 
			MethodInfo method = null;
			object[] parameters = null;
			
			if(arg[1].ToString() == "S2")
			{   
				repo.myCustom = item.getArgText();
			}
			if(item.m_Index == "33")
			{
				Delay.Milliseconds(2000);
			}
			object objComponet = item.getComponent();
			RepoItemInfo objComponetInfo = item.getComponentInfo();
			Type objType = objComponet.GetType();
			
			if(arg[1].ToString() == "0" || arg[1].ToString() == "S2")
			{ 
				method = objType.GetMethod(arg[0].ToString(),
			                                      new Type[]{});
				parameters = new object[]{};
			}
			if(arg[1].ToString() == "1")
			{
				method = objType.GetMethod(arg[0].ToString(),
			                                      new Type[]{ typeof (string)});
				parameters = new object[]{item.getArgText()};
			}
			if(arg[1].ToString() == "S1")
			{
				PropertyInfo pi = objType.GetProperty(arg[0].ToString());
				pi.SetValue(objComponet,item.getArgText(),null);
				return true;
			}
			
			if(arg[1].ToString() == "S3")
			{
				method = objType.GetMethod(arg[0].ToString(),
			                                      new Type[]{typeof (string)});
				parameters = new object[]{item.getArgText()};
				PropertyInfo pi = objType.GetProperty("TextValue")!=null?objType.GetProperty("TextValue"):objType.GetProperty("Text");
				pi.SetValue(objComponet,"",null);
			}
			
			if(arg[1].ToString() == "S4")
			{
				Select_Item(item);
				return true;
			}
			
			if(arg[1].ToString() == "S5")
			{				
				method = objType.GetMethod(arg[0].ToString(),
			                                      new Type[]{ });
				parameters = new object[]{};
				method.Invoke(objComponet,parameters);
				Delay.Milliseconds(1500);
				Validate.AreEqual(Ranorex.ToolTip.Current.Text,item.getArgText());				
				return true;				
			}
			
			if(arg[1].ToString() == "S6")
			{
				VerifyContains(item);
				return true;
			}
			
			if(arg[1].ToString() == "S7")
			{
				VerifyNotContains(item);
				return true;
			}
			
			if(arg[1].ToString() == "S8")
			{
				MoveTo(item);
				return true;
			}
			
			if(arg[1].ToString() == "S9")
			{
				Click_Cell(item);
				return true;
			}
			
			if(arg[1].ToString() == "S10")
			{
				DoubleClick_Item(item);
				return true;
			}
			
			if(arg[1].ToString() == "S11")
			{
				RightClick_Item(item);
				return true;
			}
			
			if(arg[1].ToString() == "S12")
			{
				Input_Cell(item);
				return true;
			}
			
			
           	method.Invoke(objComponet,parameters);
           	return true;
		}		
		
		//**********************************************************************
		/// <summary>
		/// RightClick to given items in the comoponet like Container.
		/// </summary>
		public static void RightClick_Item(LxScriptItem item)
		{
			object objComponet = item.getComponent();
			RepoItemInfo objComponetInfo = item.getComponentInfo();
			Type objType = objComponet.GetType();
			if(objType.Name.ToString() == "Container")
			{                      
            	Ranorex.Container targetContainer = objComponetInfo.CreateAdapter<Ranorex.Container>(true);            	
            	targetContainer.Click(System.Windows.Forms.MouseButtons.Right);
            	// Mouse.ButtonDown(System.Windows.Forms.MouseButtons.Right);
			}
			
			
		}
		
		//**********************************************************************
		/// <summary>
		/// DoubleClick to given items in the comoponet like List, Table and Tree.
		/// </summary>
		public static void DoubleClick_Item(LxScriptItem item)
		{
			object objComponet = item.getComponent();
			RepoItemInfo objComponetInfo = item.getComponentInfo();
			Type objType = objComponet.GetType();
			
			//MessageBox.Show(objType.Name.ToString());
			
			if(objType.Name.ToString() == "List")
			{
				RepoItemInfo targetListItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableListItem", 
				                                                   objComponetInfo.Path + "/listitem[@accessiblename='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Ranorex.ListItem targetListItem = targetListItemInfo.CreateAdapter<Ranorex.ListItem>(true);            	
            	targetListItem.DoubleClick();
			}
			
			if(objType.Name.ToString() == "Table")
			{
				RepoItemInfo targetCellInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableCell", 
				                                                   objComponetInfo.Path + "/row/cell[@text='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Ranorex.Cell targetCell = targetCellInfo.CreateAdapter<Ranorex.Cell>(true);            	
            	targetCell.DoubleClick();
			}
			
			if(objType.Name.ToString() == "Tree")
			{
				int treeLevel = Convert.ToInt32(item.getArgText());
				string strTreelevel = "";
				for(int i = 1 ; i <= treeLevel; i++)
				{
					strTreelevel += "/treeitem";
				}
				RepoItemInfo targetTreeItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableTreeItem", 
				                                                   objComponetInfo.Path + strTreelevel +"[@accessiblename='"+ item.getArg2Text() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                     
            	Ranorex.TreeItem targetTreeItem = targetTreeItemInfo.CreateAdapter<Ranorex.TreeItem>(true);            	
            	targetTreeItem.DoubleClick();
   /*         	
            	Ranorex.Control treeViewControl = targetTreeItem.Element.As<Ranorex.Control>();
				System.Windows.Forms.TreeNode node = treeViewControl.InvokeMethod(
                                "GetNodeAt",
                                new object[] { targetTreeItem.Element.ClientRectangle.Location + new Size(1, 1) })
                                    as System.Windows.Forms.TreeNode;
            	object mynode = node.GetLifetimeService();
            	Ranorex.CheckBox mycheckbox = (Ranorex.CheckBox)mynode;
            	mycheckbox.Check();
*/
            	
            	
			}			
		}		

		//**********************************************************************
		/// <summary>
		/// Send command to simulator, sent traps, change data OID values.
		/// </summary>
		public static void SendCommandToSimulator(LxScriptItem item)
		{
			string strDestination = item.getArgText();
			string IP = "";
			string port = "163";
			if(strDestination.IndexOf(":") != -1)
			{
				string[] spilt = strDestination.Split(':');
				//IP = strDestination.Substring(0,strDestination.IndexOf(":"));
				//port = strDestination.Substring(strDestination.IndexOf(":")+1,strDestination.Length - strDestination.IndexOf(":"));
				IP = spilt[0];
				port = spilt[1];
			}
			else
			{
				IP = strDestination;
			}
			int circle = Convert.ToInt32(item.getArg2Text());
			string message = item.getArg3Text();
			
			IPAddress GroupAddress = IPAddress.Parse(IP);
			int GroupPort = Convert.ToInt32(port);
            UdpClient sender = new UdpClient();
            IPEndPoint groupEP = new IPEndPoint(GroupAddress, GroupPort);
            for(int i = 0;i < circle; i++)
            {
            	byte[] bytes = StrToByteArray(message);
            	sender.Send(bytes, bytes.Length, groupEP);
            	Delay.Milliseconds(50);
            }
            
            sender.Close();
        }

		private static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }
		
		//**********************************************************************
		/// <summary>
		/// Copy Copydata from Trends graph to one txt file.
		/// </summary>
		public static void CopyDataToFile(LxScriptItem item)
		{
			string strFilePath = item.getArgText();
			IDataObject iData = Clipboard.GetDataObject();
			if(iData.GetDataPresent(DataFormats.StringFormat))
			{
				string s_value = (string)iData.GetData(DataFormats.StringFormat);
				Console.WriteLine("Clipboard is:" + s_value);
				Write_text(@strFilePath, s_value);
			}
			else
			{
				Console.WriteLine("Clipboard can not convert to text string!");
			}
        }
		
		//**********************************************************************
		/// <summary>
		/// Start application.
		/// </summary>
		public static void AppStart(LxScriptItem item)
		{
			string strApplicationName = parseToValue(item.m_Component);
			m_AppProcess[item.m_Action] = Host.Local.RunApplication(strApplicationName);
        }
		
		//**********************************************************************
		/// <summary>
		/// Close application.
		/// </summary>
		public static void AppClose(LxScriptItem item)
		{
			if(m_AppProcess[item.m_Action] != null)	
			{
				Host.Local.CloseApplication(Convert.ToInt32(m_AppProcess[item.m_Action]));
			}
        }
		
		private static void Write_text(string file_path, string copydata)
		{
			if(System.IO.File.Exists(file_path))
			{
				System.IO.File.WriteAllText(file_path,copydata);
			}
			else
			{
				System.IO.StreamWriter sr;
				sr = System.IO.File.CreateText(file_path);
				sr.WriteLine(copydata);
				sr.Close();
			}
		}
		
		//**********************************************************************
		/// <summary>
		/// Open txt file, verify content contains or not contains given string.
		/// </summary>
		public static void VerifyTxtfileValues(LxScriptItem item)
		{
			string strPath = item.getArgText();
			string strFileName = strPath.Substring(strPath.LastIndexOf("/") + 1,strPath.Length - strPath.LastIndexOf("/") -1);
			string flag = item.getArg3Text();  // flag=true, contains; flag=false, not contains.
			
			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
   			//startInfo.CreateNoWindow = true;
   			startInfo.FileName = "notepad.exe";
			//startInfo.UseShellExecute = false;
			//startInfo.RedirectStandardOutput = true;
			//startInfo.RedirectStandardInput = true;
   			startInfo.Arguments = " " + strPath;
   			System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);   			
   			bool bContains = repo.ExternalApp.NotePad.MainContext.TextValue.IndexOf(item.getArg2Text())==-1?false:true;

   			Delay.Milliseconds(6000);
			process.Kill();
			if(flag.Equals("True"))
				Validate.AreEqual(bContains,true);
			else
				Validate.AreEqual(bContains,false);
			
		}
		
		//**********************************************************************
		/// <summary>
		/// Open excel file, verify content contains or not contains given string.
		/// </summary>
		public static void VerifyExcelfileValues(LxScriptItem item)
		{
			string strPath = item.getArgText();
			string strFileName = strPath.Substring(strPath.LastIndexOf("/") + 1,strPath.Length - strPath.LastIndexOf("/") -1);
			string mark = item.getArg3Text();  // flag=true, contains; flag=false, not contains.
			
			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
   			startInfo.FileName = "excel.exe";
   			startInfo.Arguments = " " + strPath;
   			System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);   			
   			RepoItemInfo targetCellInfo = new RepoItemInfo(repo.ExternalApp.FormExcel.TableEntityInfo.ParentFolder, "variableCell", 
				                                                   repo.ExternalApp.FormExcel.TableEntityInfo.Path + "/row/cell[@text='"+ item.getArg2Text() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            
   			
   			Delay.Milliseconds(6000);
   			bool bExists = targetCellInfo.Exists();   			
			process.Kill();
			if(mark.Equals("True"))
				Validate.AreEqual(bExists,true);
			else
				Validate.AreEqual(bExists,false);
		}
		
		//**********************************************************************
		/// <summary>
		/// Verify the comoponet like List, Table and Tree contains given items.
		/// </summary>
		public static void VerifyContains(LxScriptItem item)
		{
			object objComponet = item.getComponent();
			RepoItemInfo objComponetInfo = item.getComponentInfo();
			Type objType = objComponet.GetType();		
			
			if(objType.Name.ToString() == "List")
			{
				RepoItemInfo targetListItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableListItem", 
				                                                   objComponetInfo.Path + "/listitem[@accessiblename='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
				Validate.AreEqual(targetListItemInfo.Exists(),true);            	
			}
			
			if(objType.Name.ToString() == "Table")
			{
				RepoItemInfo targetCellInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableCell", 
				                                                   objComponetInfo.Path + "/row/cell[@text='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Validate.AreEqual(targetCellInfo.Exists(),true);    
			}
			
			if(objType.Name.ToString() == "Tree")
			{
				int treeLevel = Convert.ToInt32(item.getArgText());
				string strTreelevel = "";
				for(int i = 1 ; i <= treeLevel; i++)
				{
					strTreelevel += "/treeitem";
				}
				RepoItemInfo targetTreeItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableTreeItem", 
				                                                   objComponetInfo.Path + strTreelevel +"[@accessiblename='"+ item.getArg2Text() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Validate.AreEqual(targetTreeItemInfo.Exists(),true);   
			}
			
		}
		
		//**********************************************************************
		/// <summary>
		/// Verify the comoponet like List, Table and Tree not contains given items.
		/// </summary>
		public static void VerifyNotContains(LxScriptItem item)
		{
			object objComponet = item.getComponent();
			RepoItemInfo objComponetInfo = item.getComponentInfo();
			Type objType = objComponet.GetType();		
			
			if(objType.Name.ToString() == "List")
			{
				RepoItemInfo targetListItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableListItem", 
				                                                   objComponetInfo.Path + "/listitem[@accessiblename='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
				Validate.AreEqual(targetListItemInfo.Exists(),false);            	
			}
			
			if(objType.Name.ToString() == "Table")
			{
				RepoItemInfo targetCellInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableCell", 
				                                                   objComponetInfo.Path + "/row/cell[@text='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Validate.AreEqual(targetCellInfo.Exists(),false);    
			}
			
			if(objType.Name.ToString() == "Tree")
			{
				int treeLevel = Convert.ToInt32(item.getArgText());
				string strTreelevel = "";
				for(int i = 1 ; i <= treeLevel; i++)
				{
					strTreelevel += "/treeitem";
				}
				RepoItemInfo targetTreeItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableTreeItem", 
				                                                   objComponetInfo.Path + strTreelevel +"[@accessiblename='"+ item.getArg2Text() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Validate.AreEqual(targetTreeItemInfo.Exists(),false);   
			}
			
		}
		
		//**********************************************************************
		/// <summary>
		/// Move mouse to given items in the comoponet like List, Table and Tree.
		/// </summary>
		public static void MoveTo(LxScriptItem item)
		{
			object objComponet = item.getComponent();
			RepoItemInfo objComponetInfo = item.getComponentInfo();
			Type objType = objComponet.GetType();
			
			//MessageBox.Show(objType.Name.ToString());
			
			if(objType.Name.ToString() == "List")
			{
				RepoItemInfo targetListItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableListItem", 
				                                                   objComponetInfo.Path + "/listitem[@accessiblename='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Ranorex.ListItem targetListItem = targetListItemInfo.CreateAdapter<Ranorex.ListItem>(true);            	
            	targetListItem.MoveTo();
			}
			
			if(objType.Name.ToString() == "Table")
			{
				RepoItemInfo targetCellInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableCell", 
				                                                   objComponetInfo.Path + "/row/cell[@text='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Ranorex.Cell targetCell = targetCellInfo.CreateAdapter<Ranorex.Cell>(true);            	
            	targetCell.MoveTo();
			}
			
			if(objType.Name.ToString() == "Tree")
			{
				int treeLevel = Convert.ToInt32(item.getArgText());
				string strTreelevel = "";
				for(int i = 1 ; i <= treeLevel; i++)
				{
					strTreelevel += "/treeitem";
				}
				RepoItemInfo targetTreeItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableTreeItem", 
				                                                   objComponetInfo.Path + strTreelevel +"[@accessiblename='"+ item.getArg2Text() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                     
            	Ranorex.TreeItem targetTreeItem = targetTreeItemInfo.CreateAdapter<Ranorex.TreeItem>(true);            	
            	targetTreeItem.MoveTo();
			}
			
		}
		
		//**********************************************************************
		/// <summary>
		/// Click to given items in the comoponet like List, Table and Tree.
		/// </summary>
		public static void Select_Item(LxScriptItem item)
		{
			object objComponet = item.getComponent();
			RepoItemInfo objComponetInfo = item.getComponentInfo();
			Type objType = objComponet.GetType();
			
			//MessageBox.Show(objType.Name.ToString());
			
			if(objType.Name.ToString() == "List")
			{
				RepoItemInfo targetListItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableListItem", 
				                                                   objComponetInfo.Path + "/listitem[@accessiblename='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Ranorex.ListItem targetListItem = targetListItemInfo.CreateAdapter<Ranorex.ListItem>(true);            	
            	targetListItem.Click();
			}
			
			if(objType.Name.ToString() == "Table")
			{
				RepoItemInfo targetCellInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableCell", 
				                                                   objComponetInfo.Path + "/row/cell[@text='"+ item.getArgText() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                         
            	Ranorex.Cell targetCell = targetCellInfo.CreateAdapter<Ranorex.Cell>(true);            	
            	targetCell.Click();
			}
			
			if(objType.Name.ToString() == "Tree")
			{
				
				String Xpos = " ";
				String Ypos = " ";	
				String sPoint ="0;0";
				
				if((!(item.getArg3Text().Trim().Equals("")))&&(!(item.getArg4Text().Trim().Equals(""))))
				{
					Xpos = item.getArg3Text();
					Ypos = item.getArg4Text();
					sPoint =Xpos+";"+Ypos;
				}
				
				int treeLevel = Convert.ToInt32(item.getArgText());
				string strTreelevel = "";
				string strTreelevelCkb = "";
				for(int i = 1 ; i <= treeLevel; i++)
				{
					strTreelevel += "/treeitem";
					strTreelevelCkb += "/checkbox";
				}
				
				RepoItemInfo targetTreeItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableTreeItem", 
				                                                   objComponetInfo.Path + strTreelevel +"[@accessiblename='"+ item.getArg2Text() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());                     
				
				if(targetTreeItemInfo.Exists())
				{
					Ranorex.TreeItem targetTreeItem = targetTreeItemInfo.CreateAdapter<Ranorex.TreeItem>(true); 
					
					targetTreeItem.Click(sPoint);
				}
				else
				{
					targetTreeItemInfo = new RepoItemInfo(objComponetInfo.ParentFolder, "variableTreeItem1", 
				                                                   objComponetInfo.Path + strTreelevelCkb +"[@accessiblename='"+ item.getArg2Text() +"']", 
				                                                   10000, null, System.Guid.NewGuid().ToString());
					Ranorex.CheckBox targetTreeItemCkb = targetTreeItemInfo.CreateAdapter<Ranorex.CheckBox>(true);      
					targetTreeItemCkb.Click(sPoint);
				}						
				
            	
			}
			
		}
		
		//**********************************************************************
		/// <summary>
		/// Click to the Cell by given index in the Table.
		/// </summary>
		public static void Click_Cell(LxScriptItem item)
		{
			object objComponet = item.getComponent();
			RepoItemInfo objComponetInfo = item.getComponentInfo();
			Type objType = objComponet.GetType();
							
			if(objType.Name.ToString() == "Table")
			{
				Ranorex.Table tb = (Ranorex.Table)objComponet;
				tb.Rows[Convert.ToInt32(item.getArgText())].Cells[Convert.ToInt32(item.getArg2Text())].Click();
			}									
		}
		
		//**********************************************************************
		/// <summary>
		/// Input Cell by given index in the Table.
		/// </summary>
		public static void Input_Cell(LxScriptItem item)
		{
			object objComponet = item.getComponent();
			RepoItemInfo objComponetInfo = item.getComponentInfo();
			Type objType = objComponet.GetType();
							
			if(objType.Name.ToString() == "Table")
			{            
				Ranorex.Table tb = (Ranorex.Table)objComponet;
				tb.Rows[Convert.ToInt32(item.getArgText())].Cells[Convert.ToInt32(item.getArg2Text())].DoubleClick();
				Keyboard.Press("{CONTROL down}{Akey}{CONTROL up}"+item.getArg3Text());
			}									
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
		
		//**********************************************************************
		/// <summary>
		/// Replace the name with value refer to app.config
		/// </summary>
		public static string parseToValue(string name)
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
               
                addr = result;
                
                confFile = new LxIniFile(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                                                 "UsedDevices.ini"));
                confFile.WriteString("AvailableDevices",key,result);
            }

            return addr.Replace("\"","");
        }
	}
}	
