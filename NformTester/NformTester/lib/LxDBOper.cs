/*
 * Created by Ranorex
 * User: x93292
 * Date: 2013/5/9
 * Time: 10:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace NformTester.lib
{
	/// <summary>
	/// Be used to backup database before run scripts. And be used to restore database if scripts are failed.
	/// </summary>
	public class LxDBOper
	{
		/// <summary>
		///DB_DbType=1, Bundled database is used; 
		///DB_DbType=2, SQL Server database is used.
		/// </summary>
		public int DB_DbType;
		
		/// <summary>
		///BackUp Result. 
		/// </summary>
		public bool BackUpResult=false;
		
		/// <summary>
		///Restore Result. 
		/// </summary>
		public bool RestoreResult=false;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public LxDBOper()
		{
		}
		
		/// <summary>
		/// Get database type from device.ini.
		/// Set DB_DbType.
		/// </summary>
		public void SetDbType()
		{
	        string groupName="Database";
	        string key="DB_DbType";
	        string DbType = ParseToValue(groupName,key);
	        DB_DbType = (int.Parse(DbType));
		}
		
		/// <summary>
		/// Get DB_DbType.
		/// </summary>
		public int GetDbType()
		{
			return DB_DbType;
		}
		
		/// <summary>
		/// Get BackUpResult.
		/// </summary>
		public bool GetBackUpResult()
		{
			return BackUpResult;
		}
		
		/// <summary>
		/// Get RestoreResult.
		/// </summary>
		public bool GetRestoreResult()
		{
			return RestoreResult;
		}
		
		/// <summary>
		/// Parse the value from Devices.ini.
		/// </summary>
		public string ParseToValue(string GroupName, string key)
        {
		  LxIniFile confFile = new LxIniFile(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                                                 "Devices.ini"));
          string def = confFile.GetString(GroupName, "Default", "null");
          string result = confFile.GetString(GroupName, key, def);
          return result;
        }
		
		/// <summary>
		/// Copy file.
		/// </summary>
	   public void CopyDir(string srcPath, string aimPath)
      {
        try
        {
            if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                aimPath += System.IO.Path.DirectorySeparatorChar;
            }
            if (!System.IO.Directory.Exists(aimPath))
            {
                System.IO.Directory.CreateDirectory(aimPath);
            }
            string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
            foreach (string file in fileList)
            {
                if (System.IO.Directory.Exists(file))
                {
                    CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                } 
                else
                {
                    System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
                }
            }
        }
        catch (Exception e)
        {
        	throw e;
        }
    }
		
		/// <summary>
		/// Open the SQL server connection.
		/// </summary>
		public void OpenConnection(SqlConnection conn)
		{
		    try
          {
		    	conn.Open();
          }
	        catch(Exception ex)
	        {
	        	string log = ex.ToString();
	        }
       }
		
		/// <summary>
		/// Close the SQL server connection.
		/// </summary>
		public void CloseConnection(SqlConnection conn)
		{
		    try
          {
		    	conn.Close();
          }
		    catch(Exception ex)
	        {
	        	string log = ex.ToString();
	        }
       }
		
		/// <summary>
		/// Get the database connection strin from Devices.ini.
		/// </summary>
		public string GetDBConnString()
		{
		    string groupName="Database";
	        string DataSource="DB_SQL_Server_Name";
			string UserName="DB_SQL_User_Name";
			string Password="SQL_Password";
			string DS = ParseToValue(groupName,DataSource);
	        string UN = ParseToValue(groupName,UserName);
	        string PWD = ParseToValue(groupName,Password);
	        string connString = "Data Source="+DS+";"+"Initial Catalog=master;"+"User ID="+UN+";"+"Password="+PWD+";";
	        return connString;
		}
			
		
		/// <summary>
		/// Back up for bundled database;
		/// </summary>
		public void BackUpBundledDataBase()
		{
			  bool result = false;
			  Console.WriteLine("*****Start to back up bundled Database*****");
			  string groupName="Database";
	          string keyOne="DB_Bundled_Path";
	          string keyTwo="DB_Bundled_Backup_Path";
			  string sourceDBPath = ParseToValue(groupName,keyOne);
	          string targetDBPath = ParseToValue(groupName,keyTwo);
		       //First delete the exsited directory.
			   if (Directory.Exists(targetDBPath))
			   {
			   	Directory.Delete(targetDBPath,true);
		       }
			   //Then create new directory with the same name.
		       Directory.CreateDirectory(targetDBPath);
		       //Restore the files.
		       
		       CopyDir(sourceDBPath,targetDBPath);
		       result = true;
		       Console.WriteLine("*****Finish to back up bundled Database*****");
		       BackUpResult = result;
		}
		
		/// <summary>
		/// Backpup database for SQL Server.
		/// </summary>
		public void ExcuteBakupQuery(SqlConnection conn)
        {	
			bool result = false;
			string backupPath = @"C:\Nform\SqlBackup";
			if (!System.IO.Directory.Exists(backupPath))
            {
				System.IO.Directory.CreateDirectory(backupPath);
            }
			if (conn.State == ConnectionState.Closed)
				conn.Open();
			string backupDB = @"Backup Database Nform To disk= 'C:\Nform\SqlBackup\Nform.bak';";
	        SqlCommand cmd = new SqlCommand(backupDB, conn);
	        cmd.ExecuteNonQuery();
	        backupDB =  @"Backup Database Nform To disk= 'C:\Nform\SqlBackup\NformAlm.bak';";
	        cmd.CommandText = backupDB;
	        cmd.ExecuteNonQuery();
	        backupDB =  @"Backup Database Nform To disk= 'C:\Nform\SqlBackup\NformLog.bak';";
	        cmd.CommandText = backupDB;
	        cmd.ExecuteNonQuery();     
	        result = true;
	        BackUpResult = result;
       }
		
	    /// <summary>
		/// Back up for SQL Server database;
		/// </summary>
		public void BackUpSQLServerDataBase()
		{
			Console.WriteLine("*****Start to back up SQL Server Database*****");
			SqlConnection conn = new SqlConnection();
			conn.ConnectionString = @"Data Source=NFORMTES-6FD309\SQLEXPRESS;Initial Catalog=master;
		    User ID=sa;Password=liebert;"; 
			OpenConnection(conn);
			try
			{
				ExcuteBakupQuery(conn);
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error when excute backup query!"+ex.StackTrace.ToString());
			}
			CloseConnection(conn);		
			Console.WriteLine("****Finish to back up SQL Server Database*****");
		}
		
		/// <summary>
		/// This method is used to back up the database.
		/// Two type of database need to consider to be backup.
		/// </summary>
		public void BackUpDataBase()
		{
			switch(DB_DbType)
        	{
        		case 1:
					BackUpBundledDataBase();
        			break;
        		case 2:
        			BackUpSQLServerDataBase();
        			break;
        		default:
        			break;
        	}		
		}
		
		/// <summary>
		/// Restore bundled database;
		/// </summary>
		public void RestoreBundledDataBase()
		{
		  bool result = false;
		  Console.WriteLine("*****Start to restore bundled Database*****");
		  string groupName="Database";
          string keyOne="DB_Bundled_Backup_Path";
		  string keyTwo="DB_Bundled_Path";
		  string sourceDBPath = ParseToValue(groupName,keyOne);
          string targetDBPath = ParseToValue(groupName,keyTwo);   
          //First delete the exsited directory.
		  if (!Directory.Exists(targetDBPath))
	      {
	       	  //Then create new directory with the same name.
	       	   Directory.CreateDirectory(targetDBPath);
	      }   
	      //Restore the files.
	      CopyDir(sourceDBPath,targetDBPath);
	      result = true;
	       Console.WriteLine("*****Finish to restore bundled Database*****");
	       RestoreResult = result;
		}
		
		/// <summary>
		/// Excute Resotre database query.
		/// </summary>
		public void ExcuteRestoreQuery(SqlConnection conn)
        {	
			bool result = false;
			string backupPath = @"C:\Nform\SqlBackup";
			string Nformbackup = @"C:\Nform\SqlBackup\Nform.bak";
			string NformAlmbackup = @"C:\Nform\SqlBackup\NformAlm.bak";
			string NformLogbackup = @"C:\Nform\SqlBackup\NformLog.bak";
			string movetoPath = @"C:\Nform\Moveto";
			if (!System.IO.Directory.Exists(backupPath))
            {
			//	MessageBox.Show("back up path is not existed!");
				return;
            }
			if(!File.Exists(Nformbackup))
			{
			//	MessageBox.Show("Nform back up file is not existed!");
				return;
			}
			if(!File.Exists(NformAlmbackup))
			{
			//	MessageBox.Show("NformAlm back up file is not existed!");
				return;
			}
			if(!File.Exists(NformLogbackup))
			{
			//	MessageBox.Show("NformLog back up file is not existed!");
				return;
			}
			if (!System.IO.Directory.Exists(movetoPath))
            {
				System.IO.Directory.CreateDirectory(movetoPath);
            }
			if (conn.State == ConnectionState.Closed) 
				conn.Open();
	        // Drop the three databases.
			string dropDB = @"drop database NformAlm;";
	        SqlCommand cmd = new SqlCommand(dropDB, conn);
	        cmd.ExecuteNonQuery();
	            
	        dropDB = @"drop database NformLog;";
	        cmd.CommandText = dropDB;
	        cmd.ExecuteNonQuery();
	                       
	        dropDB = @"drop database Nform;";
	        cmd.CommandText = dropDB;
	        cmd.ExecuteNonQuery();
	           
	        // Create 3 new database: Nform, NformAlm, NformLog.
	        string createDB=@"create database Nform;create database NformAlm;create database NformLog;";
			cmd.CommandText = createDB;
			cmd.ExecuteNonQuery();  
			    
			//Restore the existed database from backup files.
	        string restoreDB = @"restore database Nform from DISK = 'C:\Nform\SqlBackup\Nform.bak' with replace,
            MOVE 'Nform_data' TO 'C:\Nform\Moveto\data1.mdf',
            MOVE 'Nform_log' TO 'C:\Nform\Moveto\log1.ldf';";
		    cmd.CommandText = restoreDB;
		    cmd.ExecuteNonQuery();
          
	        restoreDB =  @"restore database NformLog from DISK = 'C:\Nform\SqlBackup\NformLog.bak' with replace,
            MOVE 'Nform_data' TO 'C:\Nform\Moveto\data2.mdf',
            MOVE 'Nform_log' TO 'C:\Nform\Moveto\log2.ldf';";
	        cmd.CommandText = restoreDB;
            cmd.ExecuteNonQuery();
	          
	        restoreDB =   @"restore database NformAlm from DISK = 'C:\Nform\SqlBackup\NformAlm.bak' with replace,
            MOVE 'Nform_data' TO 'C:\Nform\Moveto\data3.mdf',
            MOVE 'Nform_log' TO 'C:\Nform\Moveto\log3.ldf';";
	        cmd.CommandText = restoreDB; 
	        cmd.ExecuteNonQuery();
	        result = true;
	        
	        RestoreResult = result;
       }
		
		/// <summary>
		/// Restore SQL Server database;
		/// </summary>
		public void RestoreSQLServerDataBase()
		{
			Console.WriteLine("*****Start to restore SQL Server Database*****");	        
	        SqlConnection conn = new SqlConnection();
	        conn.ConnectionString = GetDBConnString();
			OpenConnection(conn);
			try
			{
				ExcuteRestoreQuery(conn);
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error when excute restore query!"+ex.StackTrace.ToString());
			}
			
			CloseConnection(conn);
			Console.WriteLine("*****Finish to restore SQL Server Database*****");
		}
		
		/// <summary>
		/// Restore the database, consider the type of database.
		/// DbType = 1, bundled database;DbType = 2, SQL Server database;
		/// </summary>
		public void RestoreDataBase()
		{
			if(BackUpResult == false)    //If Backup database is failed, restore database can not be excuted.
			{
				Console.WriteLine("Can not restore DB because of failure of backup database.");
				return;
			}
			else
			{
				switch(DB_DbType)
				{
	        		case 1:
						RestoreBundledDataBase();
	        			break;
	        		case 2:
	        			RestoreSQLServerDataBase();
	        			break;
	        		default:
	        		//	MessageBox.Show("Wrong database type!");
	        			break;
				}
			}
		}	
	}
}
