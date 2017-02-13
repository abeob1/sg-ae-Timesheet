using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Security;
using System.Data;
using System.IO;
using SAP.Admin.DAO;


namespace SAP.Admin
{
    public partial class SystemRoleEdit : System.Web.UI.Page
    {
        Hashtable pageList = new Hashtable();

        #region Override Methods
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            SetClientCheckBoxIds();
            if (IsPostBack) { return; }
            InitValue();
            ShowRoleDetail();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                if (ViewState["RoleName"] == null || ViewState["RoleName"].ToString() == "")
                {
                    if (WebHelper.IsExistSpecialCharacter(roleNameTextBox.Text))
                    {
                        return;
                    }
                    if (Roles.RoleExists(roleNameTextBox.Text))
                    {
                        return;
                    }

                    Roles.CreateRole(roleNameTextBox.Text);
                }

                SavePermissions(roleNameTextBox.Text.Trim());

                this.Redirect();
            }
            catch (Exception ex)
            {
 
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            
            try
            {
                string roleName = ViewState["RoleName"].ToString();
                if (roleName == "")
                {
                    return;
                }

                string[] userNames = Roles.GetUsersInRole(roleName);
                if (userNames.Length > 0)
                {
                    return;
                }

                //Roles.RemoveUsersFromRole(userNames, roleName);   

                Roles.DeleteRole(roleName);

                RolePermissions roleController = new RolePermissions();
                List<RolePermissions> list = roleController.GetByRolePermissionName(roleName);
                
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        RolePermissions role = list[i];
                        roleController.Delete(role.RolePermissionID);
                    }                    
                }

                //WebHelper.LogAction("dbo.RolePermissions", ViewState["RoleName"].ToString(), ActionLog.LogAction.Delete);

                this.Redirect();
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion

        #region Methods
        private void InitValue()
        {
            try
            {
                saveButton.Focus();
                ViewState["RoleName"] = Request.QueryString["RoleName"];

                deleteButton.OnClientClick = "return confirmAction('Do you want to delete this item')";

                ShowGridData();
            }
            catch (Exception ex)
            {
                
            }
        }
        private void ShowGridData()
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("PageName");

                string currentPath = Server.MapPath("~");
                string[] fileNames = Directory.GetFiles(currentPath, "*.aspx", SearchOption.AllDirectories);
                string pageName = "";
                for (int i = 0; i < fileNames.Length; i++)
                {
                    pageName = fileNames[i].Replace(currentPath, "");
                    DataRow row = table.NewRow();
                    row["PageName"] = pageName;
                    table.Rows.Add(row);
                    pageList.Add(pageName, i);
                }

                permissionGridView.DataSource = table;
                permissionGridView.DataBind();

                SetClientCheckBoxIds();
            }
            catch (Exception ex)
            {
                

            }
        }
        private void SetClientCheckBoxIds()
        {
            try
            {
                if (permissionGridView.Rows.Count == 0)
                {
                    return;
                }

                foreach (GridViewRow row in permissionGridView.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("accessableCheckBox");
                    if (chk == null) continue;
                    ClientScript.RegisterArrayDeclaration("CheckBoxIDs", String.Concat("'", chk.ClientID, "'"));
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        private void ShowRoleDetail()
        {
            try
            {
                string userName = WebHelper.GetCurrentUserName().ToLower();
                if (userName == "admin" && ViewState["RoleName"].ToString() == "")
                {
                    deleteButton.Visible = false;
                    return;
                }
                else if (ViewState["RoleName"].ToString() == "")
                {
                    deleteButton.Visible = false;
                    saveButton.Enabled = false;
                    roleNameTextBox.Enabled = false;
                    roleNameTextBox.Focus();

                    return;
                }

                if (WebHelper.GetCurrentUserName().ToLower() == "admin")
                {
                    deleteButton.Visible = true;
                }
                else
                {
                    deleteButton.Visible = false;
                }
                roleNameTextBox.Text = ViewState["RoleName"].ToString();
                roleNameTextBox.Enabled = false;

                if (!Roles.RoleExists(roleNameTextBox.Text))
                {
                    deleteButton.Visible = false;
                    saveButton.Enabled = false;
                    return;
                }
                
                RolePermissions roleController = new RolePermissions();
                List<RolePermissions> list = roleController.GetByRolePermissionName(roleNameTextBox.Text.Trim() );
                if (list == null)
                {
                    return;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    if (pageList[list[i].PageName] == null)
                    {
                        continue;
                    }

                    int pageIndex = int.Parse(pageList[list[i].PageName].ToString());

                    GridViewRow row = permissionGridView.Rows[pageIndex];
                    CheckBox accessableCheckBox = (CheckBox)row.FindControl("accessableCheckBox");

                    if (accessableCheckBox == null) continue;
                    accessableCheckBox.Checked = (bool)list[i].Accessable;
                }
            }
            catch (Exception ex)
            {
                
            }

        }
        private void SavePermissions(string roleName)
        {
            
            RolePermissions roleController = new RolePermissions();
            List<RolePermissions> list = roleController.GetByRolePermissionName(roleName);
                
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    RolePermissions role = list[i];
                    roleController.Delete(role.RolePermissionID);
                }
                list.Clear();
            }
            else
            {
                list = new List<RolePermissions>();
            }

            foreach (GridViewRow row in permissionGridView.Rows)
            {
                CheckBox accessableCheckBox = (CheckBox)row.FindControl("accessableCheckBox");
                if (accessableCheckBox == null || !accessableCheckBox.Checked) continue;

                RolePermissions item = new RolePermissions();
                item.RoleName = roleName;
                item.PageName = row.Cells[0].Text;
                item.Accessable = accessableCheckBox.Checked;

                list.Add(item);
                roleController.Add(item);
            }

            UpdateAllowedPagesInSession(list, roleName);
             
        }
        
        private void UpdateAllowedPagesInSession(List<RolePermissions> list, string roleName)
        {
            try
            {
                string userName = "";
                if (Context.User != null)
                {
                    userName = Context.User.Identity.Name.ToLower();
                }

                string[] roles = Roles.GetRolesForUser(userName);
                if (roles.Length == 0)
                {
                    return;
                }

                for (int i = 0; i < roles.Length; i++)
                {
                    if (roles[i].Trim().ToLower() == roleName.ToLower().Trim())
                    {
                        HttpContext.Current.Session["AllowedPages"] = list;
                        return;
                    }
                }
            }
            catch
            {
            }
        }

        private void Redirect()
        {
            Response.Redirect("SystemRoles.aspx", false);
        }
        #endregion
    }
}