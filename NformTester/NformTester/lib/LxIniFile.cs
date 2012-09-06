/*
 * Created by Ranorex
 * User: y93248
 * Date: 2012-6-27
 * Time: 16:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Runtime.InteropServices; 

namespace NformTester.lib
{
	/// <summary>
	/// Description of LxIniFile.
	/// </summary>
	public class LxIniFile
	{
		private string fileName;
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(
           string lpAppName,// 指向包含 Section 名称的字符串地址 
           string lpKeyName,// 指向包含 Key 名称的字符串地址 
           int nDefault,// 如果 Key 值没有找到，则返回缺省的值是多少 
           string lpFileName
           );
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
           string lpAppName,// 指向包含 Section 名称的字符串地址 
           string lpKeyName,// 指向包含 Key 名称的字符串地址 
           string lpDefault,// 如果 Key 值没有找到，则返回缺省的字符串的地址 
           StringBuilder lpReturnedString,// 返回字符串的缓冲区地址 
           int nSize,// 缓冲区的长度 
           string lpFileName
           );
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(
           string lpAppName,// 指向包含 Section 名称的字符串地址 
           string lpKeyName,// 指向包含 Key 名称的字符串地址 
           string lpString,// 要写的字符串地址 
           string lpFileName
           );

        //**********************************************************************
		/// <summary>
		/// Pass arg file path.
		/// </summary>
        public LxIniFile(string filename)
        {
            fileName = filename;
        }
        
        //**********************************************************************
		/// <summary>
		/// Read int by using section and key.
		/// </summary>
        public int GetInt(string section, string key, int def)
        {
            return GetPrivateProfileInt(section, key, def, fileName);
        }
        
        //**********************************************************************
		/// <summary>
		/// Read string by using section and key.
		/// </summary>
        public string GetString(string section, string key, string def)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, temp, 1024, fileName);
            return temp.ToString();
        }
        
        //**********************************************************************
		/// <summary>
		/// Write int by using section and key.
		/// </summary>
        public void WriteInt(string section, string key, int iVal)
        {
            WritePrivateProfileString(section, key, iVal.ToString(), fileName);
        }
        
        //**********************************************************************
		/// <summary>
		/// Write string by using section and key.
		/// </summary>
        public void WriteString(string section, string key, string strVal)
        {
            WritePrivateProfileString(section, key, strVal, fileName);
        }
        
        //**********************************************************************
		/// <summary>
		/// Delete key.
		/// </summary>
        public void DelKey(string section, string key)
        {
            WritePrivateProfileString(section, key, null, fileName);
        }
        
        //**********************************************************************
		/// <summary>
		/// Delete section.
		/// </summary>
        public void DelSection(string section)
        {
            WritePrivateProfileString(section, null, null, fileName);
        }
	}
}
