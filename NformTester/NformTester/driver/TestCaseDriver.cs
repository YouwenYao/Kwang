/*
 * Created by Ranorex
 * User: y93248
 * Date: 2012-3-12
 * Time: 13:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using System.Collections;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using NformTester.lib;

namespace NformTester.driver
{
	
	
    /// <summary>
    /// Description of TestCaseDriver.
    /// </summary>
    [TestModule("78CE2FBF-D204-4698-98BF-68118D374E9F", ModuleType.UserCode, 1)]
    public class TestCaseDriver : ITestModule
    {
    	/// <summary>
		/// Script path
		/// </summary>
    	public string excelPath = "";
    	
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public TestCaseDriver()
        {
            // Do not delete - a parameterless constructor is required!
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
            
            LxSetup mainOp = LxSetup.getInstance();                                             
            string tsName = mainOp.getTestCaseName();
            string excelPath = "keywordscripts/" + tsName + ".xlsx";                                           
            Report.Info("INfo",excelPath);
            	
            mainOp.StrExcelDirve = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                                                 excelPath);
            mainOp.runApp();							//  ********* 1. run Application *********
            
            LxParse stepsRepository = mainOp.getSteps();
            // stepsRepository.doValidate();  				//  ********* 2. check scripts  syntax *********
            
            ArrayList stepList = stepsRepository.getStepsList();
            bool result = LxGenericAction.performScripts(stepList);	//  ********* 3. run scripts with reflection *********
            mainOp.setResult();            
            
            mainOp.runOverOneCase(tsName);
            mainOp.opXls.close();
            LxTearDown.closeApp(mainOp.ProcessId);		//  ********* 4. clean up for next running *********
          	
            
        }
    }
}
