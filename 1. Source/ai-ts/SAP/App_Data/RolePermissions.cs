using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace SAP.Admin.DAO
{
	public class RolePermissions
	{
		#region ***** Fields & Properties ***** 
		private int _RolePermissionID;
		public int RolePermissionID
		{ 
			get 
			{ 
				return _RolePermissionID;
			}
			set 
			{ 
				_RolePermissionID = value;
			}
		}
		private string _RoleName;
		public string RoleName
		{ 
			get 
			{ 
				return _RoleName;
			}
			set 
			{ 
				_RoleName = value;
			}
		}
		private string _PageName;
		public string PageName
		{ 
			get 
			{ 
				return _PageName;
			}
			set 
			{ 
				_PageName = value;
			}
		}
		private bool _Accessable;
		public bool Accessable
		{ 
			get 
			{ 
				return _Accessable;
			}
			set 
			{ 
				_Accessable = value;
			}
		}
		#endregion

		#region ***** Init Methods ***** 
		public RolePermissions()
		{
		}
		public RolePermissions(int rolepermissionid)
		{
			this.RolePermissionID = rolepermissionid;
		}
		public RolePermissions(int rolepermissionid, string rolename, string pagename, bool accessable)
		{
			this.RolePermissionID = rolepermissionid;
			this.RoleName = rolename;
			this.PageName = pagename;
			this.Accessable = accessable;
		}
		#endregion

		#region ***** Get Methods ***** 
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public RolePermissions Populate(IDataReader myReader)
		{
			RolePermissions obj = new RolePermissions();
			obj.RolePermissionID = (int) myReader["RolePermissionID"];
			obj.RoleName = (string) myReader["RoleName"];
			obj.PageName = (string) myReader["PageName"];
			obj.Accessable = (bool) myReader["Accessable"];
			return obj;
		}

		/// <summary>
		/// Get RolePermissions by rolepermissionid
		/// </summary>
		/// <param name="rolepermissionid">RolePermissionID</param>
		/// <returns>RolePermissions</returns>
		public RolePermissions GetByRolePermissionID(int rolepermissionid)
		{
			using (IDataReader reader = SqlHelper.ExecuteReader(Data.ConnectionString, CommandType.StoredProcedure, "sproc_RolePermissions_GetByRolePermissionID", Data.CreateParameter("RolePermissionID", rolepermissionid)))			{
				if (reader.Read())
				{
					return Populate(reader);
				}
				return null;
			}
		}

        /// <summary>
        /// Get RolePermissions by rolepermissionid
        /// </summary>
        /// <param name="rolepermissionid">RolePermissionID</param>
        /// <returns>RolePermissions</returns>
        public List<RolePermissions> GetByRolePermissionName(String rolepermissionname)
        {
            using (IDataReader reader = SqlHelper.ExecuteReader(Data.ConnectionString, CommandType.StoredProcedure, "sproc_RolePermissions_GetByRolePermissionName", Data.CreateParameter("RoleName", rolepermissionname)))
            {
                List<RolePermissions> list = new List<RolePermissions>();
                while (reader.Read())
                {
                    list.Add(Populate(reader));
                }
                return list;
            }
        }

		/// <summary>
		/// Get all of RolePermissions
		/// </summary>
		/// <returns>List<<RolePermissions>></returns>
		public List<RolePermissions> GetList()
		{
			using (IDataReader reader = SqlHelper.ExecuteReader(Data.ConnectionString, CommandType.StoredProcedure, "sproc_RolePermissions_Get"))
			{
				List<RolePermissions> list = new List<RolePermissions>();
				while (reader.Read())
				{
				list.Add(Populate(reader));
				}
				return list;
			}
		}

		/// <summary>
		/// Get DataSet of RolePermissions
		/// </summary>
		/// <returns>DataSet</returns>
		public DataSet GetDataSet()
		{
			return SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure,"sproc_RolePermissions_Get");
		}


		/// <summary>
		/// Get all of RolePermissions paged
		/// </summary>
		/// <param name="recperpage">record per page</param>
		/// <param name="pageindex">page index</param>
		/// <returns>List<<RolePermissions>></returns>
		public List<RolePermissions> GetListPaged(int recperpage, int pageindex)
		{
			using (IDataReader reader = SqlHelper.ExecuteReader(Data.ConnectionString, CommandType.StoredProcedure, "sproc_RolePermissions_GetPaged"
							,Data.CreateParameter("recperpage", recperpage)
							,Data.CreateParameter("pageindex", pageindex)))
			{
				List<RolePermissions> list = new List<RolePermissions>();
				while (reader.Read())
				{
				list.Add(Populate(reader));
				}
				return list;
			}
		}

		/// <summary>
		/// Get DataSet of RolePermissions paged
		/// </summary>
		/// <param name="recperpage">record per page</param>
		/// <param name="pageindex">page index</param>
		/// <returns>DataSet</returns>
		public DataSet GetDataSetPaged(int recperpage, int pageindex)
		{
			return SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure,"sproc_RolePermissions_GetPaged"
							,Data.CreateParameter("recperpage", recperpage)
							,Data.CreateParameter("pageindex", pageindex));
		}





		#endregion

		#region ***** Add Update Delete Methods ***** 
		/// <summary>
		/// Add a new RolePermissions within RolePermissions database table
		/// </summary>
		/// <param name="obj">RolePermissions</param>
		/// <returns>key of table</returns>
		public int Add(RolePermissions obj)
		{
			DbParameter parameterItemID = Data.CreateParameter("RolePermissionID", obj.RolePermissionID);
			parameterItemID.Direction = ParameterDirection.Output;
			SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure,"sproc_RolePermissions_Add"
							,parameterItemID
							,Data.CreateParameter("RoleName", obj.RoleName)
							,Data.CreateParameter("PageName", obj.PageName)
							,Data.CreateParameter("Accessable", obj.Accessable)
			);
			return (int)parameterItemID.Value;
		}

		/// <summary>
		/// updates the specified RolePermissions
		/// </summary>
		/// <param name="obj">RolePermissions</param>
		/// <returns></returns>
		public void Update(RolePermissions obj)
		{
			SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure,"sproc_RolePermissions_Update"
							,Data.CreateParameter("RolePermissionID", obj.RolePermissionID)
							,Data.CreateParameter("RoleName", obj.RoleName)
							,Data.CreateParameter("PageName", obj.PageName)
							,Data.CreateParameter("Accessable", obj.Accessable)
			);
		}

		/// <summary>
		/// Delete the specified RolePermissions
		/// </summary>
		/// <param name="rolepermissionid">RolePermissionID</param>
		/// <returns></returns>
		public void Delete(int rolepermissionid)
		{
			SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure,"sproc_RolePermissions_Delete", Data.CreateParameter("RolePermissionID", rolepermissionid));
		}
		#endregion
	}
}
