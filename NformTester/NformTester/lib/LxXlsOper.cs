/*
 * Created by Ranorex
 * User: y93248
 * Date: 2011-11-16
 * Time: 13:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Reflection;
using System.IO;

using System.Windows.Forms;
namespace NformTester.lib
{
	//**************************************************************************
	/// <summary>
	/// Excel file operator
	/// </summary>
	/// <para> Author: Peter Yao</para>
	public class LxXlsOper
	{
		/// <summary>
		/// Excel global process
		/// </summary>
		public Excel._Application excelMain = null;
		
		/// <summary>
		/// Excel book
		/// </summary>
    	public Excel.Workbook xBook = null;
    	
    	/// <summary>
		/// Excel sheet
		/// </summary>
    	public Excel.Worksheet xSheet = null;
    	
    	//**********************************************************************
		/// <summary>
		/// Constructer. new excel application process
		/// </summary>
		public LxXlsOper()
		{
			excelMain = new Excel.ApplicationClass();
		}
		
		//**********************************************************************
		/// <summary>
		/// Open excel file from the giving path
		/// </summary>
		public void open(string strPath)
        {
        	xBook = excelMain.Workbooks.Open(strPath,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value);
            xSheet = (Excel.Worksheet)xBook.Sheets[1];
        }
        
		//**********************************************************************
		/// <summary>
		/// New excel file from the giving path
		/// </summary>
		public void create(string strPath)
        {
			if(File.Exists(strPath))
			{
				File.Delete(strPath);
			}
			object Nothing = Missing.Value;
			object format = Excel.XlFileFormat.xlWorkbookNormal;
			xBook = excelMain.Workbooks.Add(Nothing);
			xSheet = (Excel.Worksheet)xBook.Sheets[1];
			//MessageBox.Show(strPath);
			xBook.SaveAs(strPath,Nothing,Nothing,Nothing,Nothing,Nothing,
				Excel.XlSaveAsAccessMode.xlExclusive,Nothing,Nothing,Nothing,Nothing);
        }
		
		//**********************************************************************
		/// <summary>
		/// Read multi-lines data from excel, begin iColBegin and iColEnd
		/// </summary>
        public ArrayList getData(int iRow, int iColBegin, int iColEnd)
        {
        	ArrayList al = new ArrayList();
        	for(int i=iColBegin; i<= iColEnd; i++)
        	{
        		try {
        			Excel.Range rmg2 = (Excel.Range)xSheet.Cells[iRow,i];
        			al.Add(rmg2.Value2.ToString());
        		}
        		catch(Exception e) 
        		{	
        			e.Message.ToString();
        			al.Add("");
        		}
        		
        	}
        	return al;
        }
        
        //**********************************************************************
		/// <summary>
		/// Write multi-lines data to excel, begin iColBegin and iColEnd
		/// </summary>
        public void setData(int iRow, int iColBegin, int iColEnd, ArrayList alData)
        {
        	int i = iColBegin;
        	foreach(string item in alData)
        	{
        		xSheet.Cells[iRow,i++] = item;
        	}
        }
		
        //**********************************************************************
        /// Read one cell
        /// <summary>
		/// </summary>
        public string readCell(int iRow, int iCol)
        {
        	Excel.Range rmg2 = (Excel.Range)xSheet.Cells[iRow,iCol];
        	return rmg2.Value2 != null ? rmg2.Value2.ToString() : "";
        }
        
        //**********************************************************************
		/// <summary>
		/// write one cell
		/// </summary>
        public void writeCell(int iRow, int iCol, string sData)
        {
        	xSheet.Cells[iRow,iCol] = sData;
        }
        
        //**********************************************************************
		/// <summary>
		/// Close the excel
		/// </summary>
        public void close()
        {
        	xBook.Save();
        	excelMain.Quit();
        	excelMain = null;
        }
	}
}
