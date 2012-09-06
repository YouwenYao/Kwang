/*
 * Created by Ranorex
 * User: y93248
 * Date: 2012-5-15
 * Time: 16:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using System.Reflection;

using WinForms = System.Windows.Forms;
using NformTester.lib;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

using System.Windows.Forms;

namespace NformTester
{
    /// <summary>
    /// Description of UserCodeModule1.
    /// </summary>
    [TestModule("1E793A50-DA64-41A8-915C-08CBC67D8EB5", ModuleType.UserCode, 1)]
    public class GenerateAllForms : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public GenerateAllForms()
        {
            // Do not delete - a parameterless constructor is required!
        }

        /// <summary>
		/// Create excel contains all dependent values for creating select list
		/// </summary>
        public void generateFormValDependList(string fileName)
        {
        	
        	NformRepository repo = NformRepository.Instance;
        	
        	string excelPath = "keywordscripts/"+ fileName +".xlsx";                                           
           	
            excelPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                                                 excelPath);
            
			LxXlsOper opXls = new LxXlsOper();
			
        	// opXls.open("c:/test.xlsx");
        	opXls.create(excelPath);
        	
        	Excel.Workbook xBook = opXls.xBook;
       	   	Excel.Worksheet xSheet = opXls.xSheet;
        	Excel.Worksheet xSheetActions = (Excel.Worksheet)xBook.Sheets[2];;
        	
			Type objType = repo.GetType();
			int iFormRow = 1;
			int iComponentRow = 1;
			int iComponentCol = 2;
			int iActionsCol = 1;
			
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
       	   			
       	   			if(piLev2.Name.Substring(0,4).CompareTo("Form") != 0)
       	   			{
       	   				continue;
       	   			}
       	   			opXls.writeCell(iFormRow++,1,piLev2.Name);
					
       	   			// MessageBox.Show(objLogicGroup.ToString());
       	   			object objWindows = piLev2.GetValue(objLogicGroup,null);
       	   			objType = objWindows.GetType();
       	   			PropertyInfo[] piArrComp = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
       	   			foreach (PropertyInfo piCom in piArrComp)
        			{       
       	   				object objComponetInfo = null;
       	   				if(piCom.Name == "Self" || piCom.Name == "SelfInfo" || piCom.Name == "BasePath" 
       	   				   || piCom.Name == "ParentFolder" || piCom.Name == "AbsoluteBasePath" 
       	   				   || piCom.Name == "SearchTimeout" || piCom.Name == "UseCache")
       	   				{
							continue;
       	   				}       	   			     	   					
//       	   				if(piCom.Name.Length > 8) 				
//       	   				{
//       	   					if(piCom.Name.Substring(piCom.Name.Length-4,4) == "Info"
//       	   					   && piCom.Name.Substring(piCom.Name.Length-8,4) != "Info")
//       	   					continue;
//       	   				} else if(piCom.Name.Length > 4 && piCom.Name.Substring(piCom.Name.Length-4,4) == "Info")
//       	   				{
//       	   					objComponetInfo = piCom.GetValue(objWindows,null);
//       	   					//MessageBox.Show(objComponetInfo.GetType().ToString());
//       	   					
//       	   					PropertyInfo rxPath = objComponetInfo.GetType().GetProperty("AbsolutePath");
//       	   					RxPath componentRxPath = (RxPath)rxPath.GetValue(objComponetInfo,null);
//       	   					string strComponentRxPath = componentRxPath.ToString();
//       	   					//strComponentRxPath.LastIndexOf("[");
//       	   					//strComponentRxPath.LastIndexOf("/");
//       	   					string strComponentType = strComponentRxPath.Substring(
//       	   						strComponentRxPath.LastIndexOf("/")+1,strComponentRxPath.LastIndexOf("[") - strComponentRxPath.LastIndexOf("/") - 1);
//       	   					
//       	   					MessageBox.Show(componentRxPath.ToString() + "     " + strComponentType);
//       	   					continue;
//       	   				}
       	   				
       	   				if(piCom.Name.Length > 4 && piCom.Name.Substring(piCom.Name.Length-4,4) == "Info")
       	   				{
       	   					objComponetInfo = piCom.GetValue(objWindows,null);
       	   					PropertyInfo rxPath = objComponetInfo.GetType().GetProperty("AbsolutePath");
       	   					RxPath componentRxPath = (RxPath)rxPath.GetValue(objComponetInfo,null);
       	   					string strComponentRxPath = componentRxPath.ToString();
       	   					string strComponentType = "";
       	   					if(strComponentRxPath.LastIndexOf("[") - strComponentRxPath.LastIndexOf("/") > 0)
       	   					{	
       	   						strComponentType = strComponentRxPath.Substring(
       	   							strComponentRxPath.LastIndexOf("/")+1,strComponentRxPath.LastIndexOf("[") - strComponentRxPath.LastIndexOf("/") - 1);
       	   					}
       	   					else
       	   						strComponentType = strComponentRxPath.Substring(strComponentRxPath.LastIndexOf("/")+1, strComponentRxPath.Length - strComponentRxPath.LastIndexOf("/") -1);
       	   					
       	   					string comName = piCom.Name.Substring(0,piCom.Name.Length - 4);
       	   					
       	   					if(strComponentType == "button")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
	    	   				}
       	   					else if (strComponentType == "text")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "InputKeys";
       	   						xSheetActions.Cells[3,iActionsCol] = "SetTextValue";
       	   						xSheetActions.Cells[4,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[5,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[6,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[7,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "combobox")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Select";
       	   						xSheetActions.Cells[3,iActionsCol] = "SetTextValue";
       	   						xSheetActions.Cells[4,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[5,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[6,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[7,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "treeitem")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "menuitem")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "tabpage")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "indicator")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "menubar")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "list")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "ClickItem";
       	   						xSheetActions.Cells[3,iActionsCol] = "DoubleClickItem";
       	   						xSheetActions.Cells[4,iActionsCol] = "MoveTo";
       	   						xSheetActions.Cells[5,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[6,iActionsCol] = "NotExists";       	   						
       	   						xSheetActions.Cells[7,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[8,iActionsCol] = "VerifyToolTips";
       	   						xSheetActions.Cells[9,iActionsCol] = "VerifyContains";
       	   						xSheetActions.Cells[10,iActionsCol] = "VerifyNotContains";
       	   					}
       	   					else if (strComponentType == "table")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "ClickItem";
       	   						xSheetActions.Cells[3,iActionsCol] = "DoubleClickItem";
       	   						xSheetActions.Cells[4,iActionsCol] = "ClickCell";
       	   						xSheetActions.Cells[5,iActionsCol] = "InputKeys";
       	   						xSheetActions.Cells[6,iActionsCol] = "MoveTo";
       	   						xSheetActions.Cells[7,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[8,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[9,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[10,iActionsCol] = "VerifyToolTips";
       	   						xSheetActions.Cells[11,iActionsCol] = "VerifyContains";
       	   						xSheetActions.Cells[12,iActionsCol] = "VerifyNotContains";
       	   					}
       	   					else if (strComponentType == "tree")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "ClickItem";
       	   						xSheetActions.Cells[3,iActionsCol] = "DoubleClickItem";
       	   						xSheetActions.Cells[4,iActionsCol] = "MoveTo";
       	   						xSheetActions.Cells[5,iActionsCol] = "Expand";
       	   						xSheetActions.Cells[6,iActionsCol] = "Collapse";
       	   						xSheetActions.Cells[7,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[8,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[9,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[10,iActionsCol] = "VerifyToolTips";
       	   						xSheetActions.Cells[11,iActionsCol] = "VerifyContains";
       	   						xSheetActions.Cells[12,iActionsCol] = "VerifyNotContains";
       	   					}
       	   					else if (strComponentType == "listitem")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "DoubleClick";
       	   						xSheetActions.Cells[3,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[4,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[6,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "checkbox")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Set";
       	   						xSheetActions.Cells[3,iActionsCol] = "Clear";
       	   						xSheetActions.Cells[4,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[5,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[6,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[7,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "cell")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "DoubleClick";
       	   						xSheetActions.Cells[3,iActionsCol] = "InputKeys";
       	   						xSheetActions.Cells[4,iActionsCol] = "SetTextValue";
       	   						xSheetActions.Cells[5,iActionsCol] = "CellContentClick";
       	   						xSheetActions.Cells[6,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[7,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[8,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[9,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "datetime")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "InputKeys";
       	   						xSheetActions.Cells[3,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[4,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[6,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "radiobutton")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "row")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "picture")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "slider")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[3,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[4,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyToolTips";
       	   					}
       	   					else if (strComponentType == "container")
       	   					{
       	   						xSheetActions.Cells[1,iActionsCol] = "Click";
       	   						xSheetActions.Cells[2,iActionsCol] = "RightClick";
       	   						xSheetActions.Cells[3,iActionsCol] = "Exists";
       	   						xSheetActions.Cells[4,iActionsCol] = "NotExists";
       	   						xSheetActions.Cells[5,iActionsCol] = "VerifyProperty";
       	   						xSheetActions.Cells[6,iActionsCol] = "VerifyToolTips";
       	   					}
       	   						else
       	   						{
									MessageBox.Show(comName.ToString() + "     " + strComponentType);
       	   						}
       	   					Excel.Range rmgForAction = (Excel.Range)xSheetActions.Cells[1,iActionsCol];
       	   					rmgForAction.EntireColumn.Name = piLev2.Name+comName;
       	   					iActionsCol++;
       	   					// MessageBox.Show(comName.ToString() + "     " + strComponentType);
       	   				}
       	   				else
       	   				{
       	   					opXls.writeCell(iComponentRow++,iComponentCol,piCom.Name);
       	   				}
       	   			} 
       	   			
       	   			if(piLev2.Name == "FormManaged_Devices")
       	   			{
       	   				opXls.writeCell(iComponentRow++,iComponentCol,"Add_Device");
       	   				opXls.writeCell(iComponentRow++,iComponentCol,"Del_Device");
       	   			}
       	   				
       	   			iComponentRow = 1;
       	   			iComponentCol++;       	   	
       	   		}      	   		  		
        	} 
       	   	
       	   	opXls.writeCell(iFormRow++,1,"Pause");
       	   	for(int iPuaseNum = 1; iPuaseNum<=60;iPuaseNum++)
       	   	{
       	   		opXls.writeCell(iComponentRow++,iComponentCol,iPuaseNum.ToString());
       	   	}
       	   	opXls.writeCell(iFormRow++,1,"VerifyTxtfileValues");
       	   	opXls.writeCell(iFormRow++,1,"VerifyExcelfileValues");
       	   	opXls.writeCell(iFormRow++,1,"SendCommandToSimulator");
       	   		
       	   	xSheet.Name = "Form DataValDepend";
       	   	xSheetActions.Name = "Action DataValDepend";
       	   	
       	   	Excel.Range rmg = (Excel.Range)xSheet.Cells[1,1];
			rmg.EntireColumn.Name = "Forms";
       	   	
			int countRows = 1;   
			string formName = opXls.readCell(countRows,1);
			while(formName != "")
			{
				rmg = (Excel.Range)xSheet.Cells[1,countRows+1];
				rmg.Name = formName;
				rmg.EntireColumn.Name = formName+"Col";
				
				formName = opXls.readCell(++countRows,1);
			}
			
        	opXls.close();
        }
        
        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            
			
           
            //string viewVer = repo.NFormApp.Help.FormAbout_LiebertR_Nform.ViewerVer.TextValue.ToString();
           // repo.NFormApp.Help.FormAbout_LiebertR_Nform.ViewerVer.Select(0,100);
//            string viewVer = repo.NFormApp.Help.FormAbout_LiebertR_Nform.ViewerVer.TextValue;
//           MessageBox.Show(viewVer);
//           repo.NFormApp.Help.FormAbout_LiebertR_Nform.ViewerVer.TextValue = "abc";
//           
//           viewVer = repo.NFormApp.Help.FormAbout_LiebertR_Nform.ViewerVer.TextValue;
//           MessageBox.Show(viewVer);
//           
//           repo.NFormApp.Help.FormAbout_LiebertR_Nform.
			//repo.NFormApp.NformG2Window.FormAdministrator_LiebertR.TreeItemRackPDU_126_4_100_95.MoveTo();
			//string viewVer = Ranorex.ToolTip.Current.Text;
			//MessageBox.Show(viewVer);
			
			generateFormValDependList("test");
			
			//NformRepository repo = NformRepository.Instance;
			//MessageBox.Show(repo.NFormApp.LogintoLiebertNformWindow.FormLogin_to_LiebertR_Nform.LoginInfo.AbsolutePath.ToString());
			
        	
        }
    }
}
