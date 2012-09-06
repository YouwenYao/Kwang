/*
 * Created by Ranorex
 * User: y93248
 * Date: 2011-11-16
 * Time: 13:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Diagnostics;
using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using System.Xml;

using System.Windows.Forms;

namespace NformTester.lib
{
	//**************************************************************************
	/// <summary>
	/// Setup the environment for scripts running
	/// </summary>
	/// <para> Author: Peter Yao</para>
	public class LxSetup
	{
		/// <summary>
		/// The only one instance of this class
		/// </summary>
		public static LxSetup m_instance;
		
		/// <summary>
		/// The test object application file path
		/// </summary>
		public string m_strApplicationName = "";
		
		/// <summary>
		/// The path of scripts file
		/// </summary>
		public string m_strExcelDirve = "";
		
		/// <summary>
		/// The process id of application
		/// </summary>
		public int m_iProcessId = 0;
		
		/// <summary>
		/// The instance to operate Xls
		/// </summary>
		public LxXlsOper opXls = null;
		
		/// <summary>
		/// The testcase runlist from rxtst file
		/// </summary>
		public ArrayList m_Runlist = new ArrayList();
		
		/// <summary>
		/// Numbers of commands
		/// </summary>
		public int m_iRowNum = 0;
		
		/// <summary>
		/// Get method for ProcessId
		/// </summary>
		public int ProcessId {
			get { return m_iProcessId; }
		}
		
		/// <summary>
		/// Get and Set method for ApplicationName
		/// </summary>
		public string StrApplicationName {
			get { return m_strApplicationName; }
			set { m_strApplicationName = value; }
		}

		/// <summary>
		/// Get and Set method for ExcelDirve
		/// </summary>
		public string StrExcelDirve {
			get { return m_strExcelDirve; }
			set { m_strExcelDirve = value; }
		}
		
		//**********************************************************************
		/// <summary>
		/// Private Constructer, get the testcase runlist from rxtst file
		/// </summary>
		private LxSetup()
		{
			m_Runlist.Clear();
			m_Runlist = getRunlist();
		}
		
		//**********************************************************************
		/// <summary>
		/// Return the only one instance of this class
		/// </summary>
		public static LxSetup getInstance() 
		{
			if ( m_instance == null )
			{
				m_instance = new LxSetup();
			} // end if
			return m_instance;
		} // end getInstance
		
		//**********************************************************************
		/// <summary>
		/// Read the application path from scripts and run
		/// </summary>
		public void runApp()
		{
			opXls = new LxXlsOper();
        	opXls.open(m_strExcelDirve);
        	m_strApplicationName = opXls.readCell(2,2);
        	Console.WriteLine(m_strApplicationName);
			m_iRowNum = Convert.ToInt16(opXls.readCell(7,2));
        	m_iProcessId = Host.Local.RunApplication(m_strApplicationName);

		}
		
		//**********************************************************************
		/// <summary>
		/// Read the command from scripts
		/// </summary>
		public LxParse getSteps()
		{
			LxParse parse = new LxParse();
        	for( int i = 1; i <= m_iRowNum; i++)
        	{
        		LxScriptItem item = new LxScriptItem();
//        		item.m_Index = opXls.readCell(i+1,3).Trim();
//        		item.m_Type = opXls.readCell(i+1,4).Trim();
//        		item.m_WindowName = opXls.readCell(i+1,5).Trim();
//        		item.m_Component = opXls.readCell(i+1,6).Trim();
//        		item.m_Action = opXls.readCell(i+1,7).Trim();
//        		item.m_Arg1 = opXls.readCell(i+1,8).Trim();
//        		item.m_Arg2 = opXls.readCell(i+1,9).Trim();
//        		item.m_Arg3 = opXls.readCell(i+1,10).Trim();
//        		item.m_Arg4 = opXls.readCell(i+1,11).Trim();
//        		item.m_Arg5 = opXls.readCell(i+1,12).Trim();
//        		item.m_Arg6 = opXls.readCell(i+1,13).Trim();
				item.m_Index = opXls.readCell(i+1,3);
        		item.m_Type = opXls.readCell(i+1,4);
        		item.m_WindowName = opXls.readCell(i+1,5);
        		item.m_Component = opXls.readCell(i+1,6);
        		item.m_Action = opXls.readCell(i+1,7);
        		item.m_Arg1 = opXls.readCell(i+1,8);
        		item.m_Arg2 = opXls.readCell(i+1,9);
        		item.m_Arg3 = opXls.readCell(i+1,10);
        		item.m_Arg4 = opXls.readCell(i+1,11);
        		item.m_Arg5 = opXls.readCell(i+1,12);
        		item.m_Arg6 = opXls.readCell(i+1,13);
        		parse.m_Scripts.Add(item);        		
        	}		
			return parse;
		}
		
		//**********************************************************************
		/// <summary>
		/// After one case running, set the result(pass or fail) to the scripts file
		/// </summary>
		public void setResult()
		{
			int i = 1;
			for( i = 1; i <= m_iRowNum; i++)
        	{
        		string commandResult = opXls.readCell(i+1,14).Trim();
        		if(commandResult == "Fail")
        		{
        			break;
        		}	
        	}		
			//writeInfo();
			opXls.writeCell(8,2,i <= m_iRowNum?"Fail":"Pass");
		}
		
		//**********************************************************************
		/// <summary>
		/// After one case running, write additional info to the scripts file
		/// </summary>
		public void writeInfo()
		{			
			opXls.writeCell(11,2,Ranorex.Host.Local.RanorexVersion);
			opXls.writeCell(20,2,Ranorex.Host.Local.OSEdition + "  " + Ranorex.Host.Local.OSVersion);
			opXls.writeCell(21,2,Ranorex.Host.Local.RuntimeVersion);
			
			
			NformRepository repo = NformRepository.Instance;
			
			//Delay.Milliseconds(2000);
			if(!repo.NFormApp.NformG2Window.FormMain.HelpInfo.Exists())
				return;
			//Delay.Milliseconds(2000);
						
			repo.NFormApp.NformG2Window.FormMain.Help.Click();
			repo.NFormApp.NformG2Window.FormMain.About_Liebert_Nform.Click();
			string viewVer = repo.NFormApp.Help.FormAbout_LiebertR_Nform.ViewerVer.TextValue;
			repo.NFormApp.Help.FormAbout_LiebertR_Nform.TabServer.Click();
			string severVer = repo.NFormApp.Help.FormAbout_LiebertR_Nform.ServerVer.TextValue;
			repo.NFormApp.Help.FormAbout_LiebertR_Nform.TabDatabase.Click();
			string dbVer = repo.NFormApp.Help.FormAbout_LiebertR_Nform.DbVersion.TextValue;
			string dbEdition = repo.NFormApp.Help.FormAbout_LiebertR_Nform.DbEdition.TextValue;
			repo.NFormApp.Help.FormAbout_LiebertR_Nform.TabLicense.Click();
			string licenseDetail = repo.NFormApp.Help.FormAbout_LiebertR_Nform.LicenseDetail.TextValue;
			repo.NFormApp.Help.FormAbout_LiebertR_Nform.TabRegistration.Click();
			string regist = repo.NFormApp.Help.FormAbout_LiebertR_Nform.RegistInfoDscr.TextValue;
			repo.NFormApp.Help.FormAbout_LiebertR_Nform.OK.Click();
			
			opXls.writeCell(12,2,viewVer);
			opXls.writeCell(13,2,severVer);
			opXls.writeCell(14,2,dbEdition);
			opXls.writeCell(15,2,dbVer);
			opXls.writeCell(22,2,regist);
		}
		
		//**********************************************************************
		/// <summary>
		/// Return the testcase name from runlist according to the order in rxtst
		/// </summary>
		public string getTestCaseName()
		{
			if(m_Runlist.Count != 0) 
			{
				object[] arg = (object[])m_Runlist[0];
				return (string)arg[0];
			}
			
			return "";
		}
		
		//**********************************************************************
		/// <summary>
		/// After running one case, remove it from runlist
		/// </summary>
		public void runOverOneCase(string testcaseName)
		{
			m_Runlist.RemoveAt(0);
		}
		
		//**************************************************************************
		/// <summary>
		/// Comparer of the runlist, make sure the order in runlist is the order in rxtst
		/// </summary>
		/// <para> Author: Peter Yao</para>
		public class myRunlistCompareClass : IComparer
		{
			//**********************************************************************
			/// <summary>
			/// The order is accordint to the int value y, from small to large
			/// </summary>
     		int IComparer.Compare( Object x, Object y )  
      		{	
     			object[] argX = (object[])x;
     			object[] argY = (object[])y;
     			int iX = (int)argX[1];
     			int iY = (int)argY[1];
     			return( iX.CompareTo(iY) );
      		}
   		}

		//**********************************************************************
		/// <summary>
		/// Load the rxtst file, and get the runlist of testcase name
		/// </summary>
		public ArrayList getRunlist()
		{
			ArrayList runlist = new ArrayList();
			
			string xmlpath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                                                 "NformTester.rxtst");
        	XmlDocument xmldoc = new XmlDocument();
        	xmldoc.Load(xmlpath);
        	XmlNodeList nodes ;
        	XmlElement xe;
        	
        	Hashtable htAllCaseName = new Hashtable();
        	nodes = xmldoc.SelectNodes("/testsuite/content/testcase");
        	int iNum = 0;
        	foreach(XmlNode xnf in nodes)
        	{
        		xe=(XmlElement)xnf;
        		htAllCaseName.Add(xe.GetAttribute("id"),
        		                  new object[] {xe.GetAttribute("name"), iNum} );
        		iNum ++;			
        	}
        	
        	nodes = xmldoc.SelectNodes("/testsuite/testconfigurations/testconfiguration/testcase");
        	for(int i=0; i< nodes.Count; i++)
        	{
        		xe=(XmlElement)nodes[i];
        		object[] arg = (object[])htAllCaseName[xe.GetAttribute("id")];
        		if(arg == null)
        			continue;
        		runlist.Add(arg);
        		
        	}
        	        	
        	IComparer myComparer = new myRunlistCompareClass();
        	runlist.Sort(myComparer);
        	return runlist;

		}
		
		//**********************************************************************
        /// <summary>
        /// Load the languagefile
        /// </summary>
		public Hashtable LoadLanguageInfo()
		{
			string currentLanguage = System.Globalization.CultureInfo.InstalledUICulture.Name.ToString();

            Hashtable m_LanguageFileMap = new Hashtable();
            m_LanguageFileMap.Add("zh-CN","CN");
            m_LanguageFileMap.Add("en","EN");

            string xmlpath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
            	                      "AppResource_"+m_LanguageFileMap[currentLanguage]+".xml");
            
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlpath);
            XmlNodeList nodes;
            XmlElement xe;

            Hashtable htName = new Hashtable();
            nodes = xmldoc.SelectNodes("/Resource/Form/Controls/Control");
            int iNum = 0;
            foreach (XmlNode xnf in nodes)
            {
                xe = (XmlElement)xnf;
                htName.Add(xe.GetAttribute("name"),xe.GetAttribute("text"));
                iNum++;
            }
            
            return htName;
		}
		
		//**********************************************************************
        /// <summary>
        /// Return the string text according to LanguageID
        /// </summary>
        public string GetLanguageString(string LanguageID)
        {
        	// return htLanguageInfo[LanguageID];
        	return "";
	    }
		
	}
}
