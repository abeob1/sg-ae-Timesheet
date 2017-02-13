using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using SAP.Admin.DAO;
using SAP;

namespace SAP.Admin
{
    public partial class SystemUserEdit : System.Web.UI.Page
    {
        private static DataSet dsUserDetails = new DataSet();
        private static DataSet dsDBName = new DataSet();

        #region Properties
        private bool changePassword = false;
        private object reporttoid = null;

        public bool ChangePassword
        {
            get { return changePassword; }
            set { changePassword = value; }
        }

        #region ReportToID
        protected object ReportToID
        {
            get { return reporttoid; }
            set { reporttoid = value; }
        }
        #endregion

        #endregion

        #region Override Methods
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.InitValue();
                this.ShowUserDetail();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) { return; }

                string userName = Request.QueryString["userName"]; 
                string password = passwordTextbox.Text;
                string roleName = rolesDropDownList.SelectedItem.Value.Trim();
                string email = emailTextbox.Text.Trim();
                string currentUser = Context.User.Identity.Name.ToLower();

                //if (userName.ToLower().Trim() == "admin" && currentUser != "admin")
                //{
                //    return;
                //}

                MembershipUser user = Membership.GetUser(userName);
                userName = userNameTextbox.Text.Trim();
                if (user == null && password.Length < 5)
                {
                    return;
                }

                //Add new user
                if (user == null)
                {
                    if (Roles.IsUserInRole(userName, roleName))
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(email))
                    {
                        WebHelper.AddMemberShipUserWithoutEmail(userName, password, roleName);
                    }
                    else
                    {
                        WebHelper.AddMemberShipUser(userName, password, email, roleName);
                    }


                    user = Membership.GetUser(userName);
                    user.IsApproved = chkActive.Checked;
                    Membership.UpdateUser(user);

                    string loggedData = string.Format("{0}|{1}", userName, roleName);
                }
                else //Edit user
                {
                    string[] roles = Roles.GetRolesForUser(user.UserName);
                    if (!CheckPermitRoles(roles))
                    {

                        return;
                    }

                    if (!ChangePassword && roles.Length > 0 && roles[0].ToLower().Trim() != rolesDropDownList.SelectedItem.Value.ToLower().Trim())
                    {
                        Roles.RemoveUserFromRoles(user.UserName, roles);
                        Roles.AddUserToRole(user.UserName, rolesDropDownList.SelectedItem.Value.ToLower().Trim());
                    }

                    if (!string.IsNullOrEmpty(password))
                    {
                        // Unlock user
                        if (user.IsLockedOut) { user.UnlockUser(); }
                        //string currentPassword = user.GetPassword();
                        user.ChangePassword(user.ResetPassword() , passwordTextbox.Text.Trim());
                    }

                    user.Email = email;

                    if (String.Compare(user.UserName, userName) == 0)
                    {
                        user.IsApproved = chkActive.Checked;
                        Membership.UpdateUser(user);

                        // Update Rate hour and DB Name
                        Guid guid = WebHelper.GetUserID(userName);
                        string uIDupdate = guid.ToString();

                    }
                    else
                    {
                        if (Roles.IsUserInRole(userName, roleName))
                        {
                            return;
                        }

                        // todo: update user name.
                        user = Membership.GetUser(userName);
                    }
                }

                StatusLabel.Text = "Updated!";

                // Update Rate hour and DB Name
                string uID = WebHelper.GetUserID(userName).ToString();
                if (dsUserDetails != null && dsUserDetails.Tables.Count > 0 && dsUserDetails.Tables[0].Rows.Count > 0)
                {
                    CultureInfo ivC = new CultureInfo("es-US");
                    DateTime effectiveDate = Convert.ToDateTime("01/01/1900", ivC);

                    string lsRateHour = string.Empty, lsSubcontractor = string.Empty, lsDBName = string.Empty, lsUserType = string.Empty;
                    string lsEffectiveDate = effectiveDate.ToString("MM/dd/yyyy");
                    foreach (DataRow dr in dsUserDetails.Tables[0].Rows)
                    {
                        if (dr.RowState != DataRowState.Added) continue;
                        if (lsDBName.Equals(""))
                        {
                            lsRateHour = dr["rate_hour"].ToString();
                            lsSubcontractor = dr["Subcontractor"].ToString();
                            lsDBName = dr["DBName"].ToString();
                            lsUserType = dr["UserType"].ToString();
                            lsEffectiveDate = Convert.ToDateTime(dr["EffectiveDate"].ToString(), ivC).ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            lsRateHour = lsRateHour + ";" + dr["rate_hour"].ToString();
                            lsSubcontractor = lsSubcontractor + ";" + dr["Subcontractor"].ToString();
                            lsDBName = lsDBName + ";" + dr["DBName"].ToString();
                            lsUserType = lsUserType + ";" + dr["UserType"].ToString();
                            lsEffectiveDate = lsEffectiveDate + ";" + Convert.ToDateTime(dr["EffectiveDate"].ToString(), ivC).ToString("MM/dd/yyyy");
                        }
                    }
                    SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure, "sp_UpdateUserDetails",
                        Data.CreateParameter("@IN_UserID", uID),
                        Data.CreateParameter("@IN_UserName", userNameTextbox.Text.Trim()),
                        Data.CreateParameter("@IN_RateHour", lsRateHour),
                        Data.CreateParameter("@IN_SubCont", lsSubcontractor),
                        Data.CreateParameter("@IN_DBName", lsDBName),
                        Data.CreateParameter("@IN_UserType", lsUserType),
                        Data.CreateParameter("@IN_EffectiveDate", lsEffectiveDate)
                        );
                    dsUserDetails = new DataSet();
                    // Binding User Details
                    LoadUserDetails(WebHelper.GetUserID(user.UserName).ToString());
                    DataView dv = new DataView(dsUserDetails.Tables[0]);
                    this.lvStage.DataSource = dv;
                    this.lvStage.DataBind();
                }
                // Update tbl_ReportTo, tbl_UsersAdd
                SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure, "sp_UpdateReportTo",
                    Data.CreateParameter("@IN_ReportToID", (this.ReportToID == null ? DBNull.Value : this.ReportToID)),
                    Data.CreateParameter("@IN_ReportToName", txtReportTo.Text.Trim()),
                    Data.CreateParameter("@IN_UserID", uID),
                    Data.CreateParameter("@IN_UserName", userNameTextbox.Text.Trim()),
                    Data.CreateParameter("@IN_DateJoined", string.IsNullOrEmpty(DateJoinedTextBox.Text) ? "01/01/1900" : DateJoinedTextBox.Text),
                    Data.CreateParameter("@IN_DateResigned", string.IsNullOrEmpty(DateResignedTextBox.Text) ? "12/31/9999" : DateResignedTextBox.Text)
                    );
            }
            catch (Exception ex)
            {
                StatusLabel.Text = ex.ToString();
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = userNameTextbox.Text.Trim();
                if (userName == "" || userName.ToLower().Trim() == "admin")
                {
                    return;
                }

                MembershipUser deleteUser = Membership.GetUser(userName);
                WebHelper.DeleteMembershipUser(userName);
                this.Redirect();
            }
            catch (Exception ex)
            {
                StatusLabel.Text = ex.ToString();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtRateHour.Text.Trim()))
            {
                StatusLabel.Text = "Please input Rate Hour";
                return;
            }
            if (ddlDBName.SelectedIndex == -1 || string.IsNullOrEmpty(ddlDBName.SelectedValue) )
            {
                StatusLabel.Text = "Please select a DB Name";
                return;
            }

            CultureInfo ivC = new CultureInfo("es-US");
            DateTime effectiveDate = Convert.ToDateTime(string.IsNullOrEmpty(EffectiveDateTextBox.Text) ? "01/01/1900" : EffectiveDateTextBox.Text, ivC);

            DataRow dr = dsUserDetails.Tables[0].NewRow();

            dr["UserID"] = WebHelper.GetUserID(userNameTextbox.Text.Trim()).ToString();
            dr["UserName"] = userNameTextbox.Text.Trim();
            dr["rate_hour"] = txtRateHour.Text.Trim();
            dr["Subcontractor"] = txtSubcontractor.Text.Trim();
            dr["DBName"] = ddlDBName.SelectedValue;
            dr["UserType"] = txtUserType.Text.Trim();
            dr["EffectiveDate"] = effectiveDate.ToString("MM/dd/yyyy");
            dsUserDetails.Tables[0].Rows.Add(dr);
            DataView dv = new DataView(dsUserDetails.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
            txtRateHour.Text = string.Empty;
            txtUserType.Text = string.Empty;
            txtSubcontractor.Text = string.Empty;
        }

        protected void lvStage_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            DataRow dr = dsUserDetails.Tables[0].Rows[e.Item.DataItemIndex];
            ListViewItem lvi = e.Item;
            switch (e.CommandName)
            {
                case "Update":
                    break;

                case "Edit":
                    break;

                case "Delete":
                    int li_ErrorCode = 0;
                    try
                    {
                        string lsIDSin = dr["UserID"].ToString();
                        string lsDBName = dr["DBName"].ToString();
                        string lsEffectiveDate = dr["EffectiveDate"].ToString();
                        SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, "Delete tbl_UsersDetails Where UserID = '" + lsIDSin + "' and DBName = '" + lsDBName
                            + "' and Convert(varchar(8), EffectiveDate, 112) = Convert(varchar(8), cast('" + lsEffectiveDate + "' as date), 112) ");
                    }
                    catch (SqlException sqlEx)
                    {
                        li_ErrorCode = sqlEx.ErrorCode;
                    }

                    if (li_ErrorCode != 0)
                    {
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + li_ErrorCode.ToString() + "','');", true);
                    }
                    else
                    {
                        Session["successMessage"] = "Operation complete sucessful!";
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + "Operation complete sucessful!" + "','');", true);
                        Response.Redirect(Request.RawUrl);
                    } break;

                case "Cancel":
                    Response.Redirect(Request.RawUrl);
                    break;
            }
        }

        protected void lvStage_ItemCreated(object sender, ListViewItemEventArgs e)
        {

        }

        protected void lvStage_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }

        protected void lvStage_ItemEditing(object sender, ListViewEditEventArgs e)
        {

        }

        protected void lvStage_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {

        }

        protected void lvStage_ItemInserting(object sender, ListViewInsertEventArgs e)
        {

        }

        protected void lvStage_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        protected void lvStage_LayoutCreated(object sender, EventArgs e)
        {

        }

        #endregion

        #region Methods
        private void InitValue()
        {
            try
            {
                deleteButton.OnClientClick = "return confirmAction('Do you want do delete this item?')";

                string[] roles = Roles.GetAllRoles();                

                foreach (string role in roles)
                {
                    rolesDropDownList.Items.Add(new ListItem(role, role));
                }

                string userName = string.Empty;
                if (Context.User != null)
                {
                    string currentUser = Request.QueryString["userName"].ToLower().Trim();
                    userName = Context.User.Identity.Name;
                    if (currentUser == userName.ToLower().Trim())
                    {
                        deleteButton.Enabled = false;
                    }

                    if (string.IsNullOrEmpty(currentUser)) 
                    {
                        // if create new user --> set default Consultant role.
                        ListItem roleItem = rolesDropDownList.Items.FindByValue("Consultant");
                        rolesDropDownList.SelectedIndex = rolesDropDownList.Items.IndexOf(roleItem);
                    }
                }

                if (changePassword)
                {
                    string[] rolesOfUser = Roles.GetRolesForUser(userName);
                    ListItem roleItem = rolesDropDownList.Items.FindByValue(rolesOfUser[0]);
                    rolesDropDownList.SelectedIndex = rolesDropDownList.Items.IndexOf(roleItem);
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = ex.ToString();
            }
        }

        private void ShowUserDetail()
        {
            try
            {
                deleteButton.Visible = true;
                string userName = Request.QueryString["UserName"];

                if (changePassword)
                {
                    rolesDropDownList.Enabled = false;
                    deleteButton.Visible = false;
                    userName = HttpContext.Current.User.Identity.Name;
                }

                if (userName == "")
                {
                    userNameTextbox.Text = string.Empty;
                    passwordTextbox.Text = string.Empty;
                    userNameTextbox.Enabled = true;
                    userNameTextbox.Focus();
                    deleteButton.Visible = false;
                    btnAdd.Enabled = false;
                    return;
                }

                System.Web.Security.MembershipUser user = Membership.GetUser(userName);
                if (user == null)
                {
                    return;
                }

                string[] roles = Roles.GetRolesForUser(user.UserName);
                if (roles.Length > 0)
                {
                    rolesDropDownList.SelectedValue = roles[0];
                }

                chkActive.Checked = user.IsApproved;
                lastActivityDateTextBox.Text = user.LastActivityDate.ToString();
                emailTextbox.Text = user.Email;
                userNameTextbox.Text = user.UserName;
                userNameTextbox.Enabled = false;
                DataTable dtUserDefault = new DataTable();
                dtUserDefault = getUserDefaultValue(userName);
                listUserDefault.DataSource = dtUserDefault;
                listUserDefault.DataBind();
                // Binding DB Name
                LoadDBName();
                ddlDBName.DataSource = dsDBName.Tables[0];
                ddlDBName.DataTextField = "DBName";
                ddlDBName.DataValueField = "DBName";
                ddlDBName.DataBind();
                // Binding User Details
                LoadUserDetails(WebHelper.GetUserID(user.UserName).ToString());
                DataView dv = new DataView(dsUserDetails.Tables[0]);
                this.lvStage.DataSource = dv;
                this.lvStage.DataBind();
                txtReportTo.Text = GetReportToName(user.ProviderUserKey.ToString()).ToString();
                SetUsersAdd(user.ProviderUserKey.ToString());
            }
            catch (Exception ex)
            {
                StatusLabel.Text = ex.ToString();
            }
        }

        protected DataTable getUserDefaultValue(String UserName)
        {
            DataTable results = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ToString()))
            {
                SqlCommand command = new SqlCommand("select * from Users_Default where UserId= '" + UserName + "'", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                conn.Open();
                adapter.Fill(results);
            }
            return results;
        }

        protected object GetReportToName(string UserID)
        {
            try
            {
                object obj = SqlHelper.ExecuteScalar(Data.ConnectionString, CommandType.Text, "Select ReportToName From tbl_ReportTo Where UserID= '" + UserID + "'");
                if (obj == null) return "";
                return obj;
            }
            catch (SqlException sqlex)
            {
                return "";
            }
        }

        protected void SetUsersAdd(string UserID)
        {
            try
            {
                DataSet lds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.Text,
                    "Select case Convert(varchar(10), DateJoined, 101) when '01/01/1900' then '' else Convert(varchar(10), DateJoined, 101) end DateJoined, "
                    + " case Convert(varchar(10), DateResigned, 101) when '12/31/9999' then '' else Convert(varchar(10), DateResigned, 101) end DateResigned " 
                    + " From tbl_UsersAdd Where UserId = '" + UserID + "'");
                if (lds != null && lds.Tables[0].Rows.Count > 0)
                {
                    DateJoinedTextBox.Text = lds.Tables[0].Rows[0]["DateJoined"].ToString();
                    DateResignedTextBox.Text = lds.Tables[0].Rows[0]["DateResigned"].ToString();
                }
                else
                {
                    DateJoinedTextBox.Text = string.Empty;
                    DateResignedTextBox.Text = string.Empty;
                }
            }
            catch (SqlException)
            {
            }
        }

        #region OnLoadComplete
        protected override void OnLoadComplete(EventArgs e)
        {
            try
            {
                base.OnLoadComplete(e);
                if (this.Request["__EVENTARGUMENT"] != null && this.Request["__EVENTARGUMENT"].ToString() != "")
                {
                    Int32 itemNo = 0;
                    switch (this.Request["__EVENTARGUMENT"].ToString())
                    {
                        case "ReportingManagerCallBack":
                            UserList chosenItem = Session["chosenUser"] as UserList;
                            itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                            if (chosenItem != null)
                            {
                                ReportToID = chosenItem.UserID;
                                txtReportTo.Text = chosenItem.UserName;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                //                                        "Main.setMasterMessage('" + ex.ToString() + "','');", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading",
                //                                    "Dialog.hideLoader();", true);
            }
        }
        #endregion
       
        private bool CheckPermitRoles(string[] roles)
        {
            try
            {
                if (roles == null || roles.Length == 0)
                {
                    return true;
                }

                foreach (ListItem item in rolesDropDownList.Items)
                {
                    if (item.Enabled && WebHelper.IncludeInStringList(item.Text, roles))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = ex.ToString();
            }
            return false;
        }

        private void Redirect()
        {
            Response.Redirect(changePassword ? "Default.aspx" : "SystemUsers.aspx", false);
        }

        private void LoadDBName()
        {
            dsDBName = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_GetDBList");
        }

        private void LoadUserDetails(string UserID)
        {
            dsUserDetails = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_GetUserDetails", Data.CreateParameter("@IN_UserID", UserID));
        }
        #endregion

        protected void txtReportTo_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Trim().Equals("")) this.ReportToID = null;
        }

    }
}
