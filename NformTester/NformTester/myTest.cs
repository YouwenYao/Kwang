/*
 * Created by Ranorex
 * User: y93248
 * Date: 2012-5-23
 * Time: 14:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using Ranorex.Core.Repository;

namespace NformTester
{
    /// <summary>
    /// Description of myTest.
    /// </summary>
    [TestModule("A201CDD0-CCDA-48AD-B091-2EB9C38E066E", ModuleType.UserCode, 1)]
    public class myTest : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public myTest()
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
            
           // NformRepository repo = NformRepository.Instance;
            //repo.NFormApp.AddDeviceWizard.FormAdd_Device.Row0_CellStart_IP_address. = "abceee";
            
//            repo.NFormApp.UsersandGroupsWindow.FormUsers_and_Groups.UsersList;
//            repo.NFormApp.ManagedDevicesWindow.FormManaged_Devices.Managed_device_table.Rows[1].Cells[]
//            repo.NFormApp.UsersandGroupsWindow.FormUsers_and_Groups.UsersList.Items[1].Click();
//            repo.NFormApp.UsersandGroupsWindow.FormUsers_and_Groups.UsersEdit.Click();

			
//			RepoItemInfo myTestListCellInfo = new RepoItemInfo(repo.NFormApp.UsersandGroupsWindow.FormUsers_and_Groups, "TestListCell", 
//                                               "container[@controlname='m_usersGrp']/table/list/listitem[@accessiblename='Nform']", 10000, null, "e7cbd45f-c792-4c61-aef3-9b44e41fe99f");
//                         
//            Ranorex.ListItem myItem = myTestListCellInfo.CreateAdapter<Ranorex.ListItem>(true);
//            myItem.Click();
//            repo.NFormApp.UsersandGroupsWindow.FormUsers_and_Groups.UsersEdit.Click();
            //repo.NFormApp.UsersandGroupsWindow.FormUsers_and_Groups.u
           // repo.NFormApp.NavigationViews.FormNavigationViewProperties.Name.PressKeys("abc");
//           repo.NFormApp.NformG2Window.FormMain.Status_NoComm.MoveTo();
//           Delay.Milliseconds(2000);
//           repo.NFormApp.NformG2Window.FormMain.Status_Normal.MoveTo();
//           Delay.Milliseconds(2000);
//           repo.NFormApp.NformG2Window.FormMain.Status_Alarm.MoveTo();
           // repo.NFormApp.NavigationViews.FormNavigationViewProperties.View_tree.MoveTo
            //repo.NFormApp.NavigationViews.FormNavigationViewProperties.View_tree.CollapseAll();
//            repo.NFormApp.DevicePropertiesWindow.FormDevice_Data_Logging.Data_logging_table.Rows[2].Cells[1].Click();
//            
//            repo.NFormApp.DevicePropertiesWindow.FormDevice_Data_Logging.Data_logging_table.Rows[2].Cells[0].Click();
//            repo.NFormApp.DevicePropertiesWindow.FormDevice_Data_Logging.Data_logging_table.Rows[3].Cells[0].Click();
//            repo.NFormApp.DevicePropertiesWindow.FormDevice_Data_Logging.Data_logging_table.Rows[4].Cells[0].Click();
//            repo.NFormApp.DevicePropertiesWindow.FormDevice_Data_Logging.Data_logging_table.Rows[5].Cells[0].Click();
//            repo.NFormApp.DevicePropertiesWindow.FormDevice_Data_Logging.Data_logging_table.Rows[6].Cells[0].Click();
				//repo.NFormApp.NformG2Window.FormMain.FromDate.
           
        }
    }
}
