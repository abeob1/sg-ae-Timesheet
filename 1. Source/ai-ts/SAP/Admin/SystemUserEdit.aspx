<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    Codebehind="SystemUserEdit.aspx.cs" Inherits="SAP.Admin.SystemUserEdit" %>

    

<asp:Content ID="pageContentContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Admin/user_style.css" rel="stylesheet" type="text/css" />
    <link href="/skin/Admin/edit.css" rel="stylesheet" type="text/css" />
    <div id="user-main">
        <h2 class="user-header">User Information</h2>
        <div class="EditRegion">       
            <div id="roleNameDiv" runat="server">
                <label>
                    <asp:Localize ID="roleNameLocalize" runat="server" meta:resourcekey="LabelRoleName"></asp:Localize>
                    <span id="requriedSymbolSpan" runat="server" class="RequiredField">*</span>
                </label>
                <asp:DropDownList ID="rolesDropDownList" runat="server"></asp:DropDownList>
            </div>
            <div id="userNameDiv" runat="server">
            <label>
                <asp:Localize ID="userNameLocalize" runat="server" meta:resourcekey="LabelUserName">
                </asp:Localize>
                <span id="Span1" runat="server" class="RequiredField">*</span>
            </label>
            <asp:TextBox ID="userNameTextbox" runat="server" CssClass="InputBox" MaxLength="200" />
            <asp:RequiredFieldValidator ID="rqfUserName" runat="server" Display="Dynamic"
                ErrorMessage="Required" ControlToValidate="userNameTextbox" 
                CssClass="RequiredField" SetFocusOnError="True"></asp:RequiredFieldValidator>
        </div>
            <div id="passwordDiv" runat="server">
            <label>
                <asp:Localize ID="passwordLocalize" runat="server" meta:resourcekey="LabelPassword">
                </asp:Localize>
                <span id="Span2" runat="server" class="RequiredField">*</span>
            </label>
            <asp:TextBox ID="passwordTextbox" runat="server" CssClass="InputBox" TextMode="Password"></asp:TextBox>
            <asp:Label ID="passwordRequiredLabel" runat="server" ForeColor="Red" 
                Text="Required" Visible="False"></asp:Label>
            <asp:RegularExpressionValidator ID="passwordRegularExpressionValidator" CssClass="RequiredField" runat="server"
                ControlToValidate="passwordTextbox" ErrorMessage="Password Length must be greater than 5 !!!"
                ValidationExpression="^.{5,20}$" SetFocusOnError="true">
            </asp:RegularExpressionValidator>
        </div>
            <div id="confirmPasswordDiv" runat="server">
                <label>
                    <asp:Localize ID="confirmPasswordLocalize" runat="server" meta:resourcekey="LabelConfirmPassword"></asp:Localize>
                    <span id="Span3" runat="server" class="RequiredField">*</span>
                </label>
                <asp:TextBox ID="confirmPasswordTextbox" runat="server" CssClass="InputBox" TextMode="Password"></asp:TextBox>
                <asp:CompareValidator ID="cpvRePassword" runat="server" ErrorMessage="Password confirm is not match." Display="Dynamic"
                    Text="Password confirm is not match." ControlToCompare="passwordTextbox" ControlToValidate="confirmPasswordTextbox" 
                    CssClass="RequiredField" SetFocusOnError="True">
                </asp:CompareValidator>
            </div>
            <div id="emailDiv" runat="server">
                <label>
                    <asp:Localize ID="emailLocalize" runat="server" meta:resourcekey="LabelEmail"></asp:Localize>
                </label>
                <asp:TextBox ID="emailTextbox" runat="server" CssClass="InputBox"></asp:TextBox>
                <asp:RegularExpressionValidator ID="emailRegularExpressionValidator" CssClass="RequiredField" runat="server"
                    ControlToValidate="emailTextbox" ErrorMessage="Email is not invalid"
                    ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$" SetFocusOnError="true">
                </asp:RegularExpressionValidator>
            </div>
            <div id="lastActivityDateDiv" runat="server">
                <label style="width : 460px; text-align: left">
                    <asp:Localize ID="lastActivityDateLocalize" runat="server" meta:resourcekey="LabelLastActivityDate"></asp:Localize>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="lastActivityDateTextBox" Width="150" runat="server" CssClass="InputBox" ReadOnly="true"></asp:TextBox>
                </label>
                <label>
                    <asp:Localize ID="activeLocalize" runat="server" meta:resourcekey="LabelActive"></asp:Localize>
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID='chkActive' runat="server" />
                </label>
            </div>
            <div id="DateDiv" runat="server">
                <label style="width : 460px; text-align: left">
                    <asp:Localize ID="DateJoinedLocalize" runat="server" meta:resourcekey="LabelDateJoined"></asp:Localize>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="DateJoinedTextBox" class="txtDate" Width="150" runat="server"></asp:TextBox>
                </label>
                <label style="width : 460px; text-align: left">
                    <asp:Localize ID="DateResignedLocalize" runat="server" meta:resourcekey="LabelDateResigned"></asp:Localize>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="DateResignedTextBox" class="txtDate" Width="150" runat="server"></asp:TextBox>
                </label>
            </div>
            <div id="userReportToDiv" runat="server">
                <label style="width : 460px; text-align: left">
                    <asp:Localize ID="ReportToLocalize" runat="server" meta:resourcekey="LabelReportTo"></asp:Localize>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtReportTo" Width="150" runat="server" CssClass="InputBox" MaxLength="256" ontextchanged="txtReportTo_TextChanged" />
                    <asp:Button ID="btnReportTo" width="20" Height="20" runat="server" OnClientClick="javascript:Main.openCustomDialog('../Popup_UserList.aspx?id=0',400,600); return false;" meta:resourcekey="btnReportTo" CssClass="ButtonLarge" />
                </label>
            </div>
            <div>
                <h4 class="user-default-header">User Details Value</h4>
                <div id="userInforAddDiv" runat="server">
                    <label style="width : 120px; text-align: left">
                        <asp:Localize ID="rateHourLocalize" runat="server" meta:resourcekey="LabelRateHour"></asp:Localize>
                        <asp:TextBox ID="txtRateHour" style="text-align: right;" runat="server" CssClass="InputBox" Width="50px"></asp:TextBox>   
                    </label> 
                    <label style="width : 270px; text-align: right">
                        <asp:Localize ID="subcontractRateLocalize" runat="server" meta:resourcekey="LabelSubcontractRate"></asp:Localize>
                        <asp:TextBox ID="txtSubcontractor" style="text-align: left;" runat="server" CssClass="InputBox" Width="150px"></asp:TextBox>   
                    </label>
                    <label style="width : 180px; text-align: right">
                        <asp:Localize ID="EffectiveDateLocalize" runat="server" meta:resourcekey="LabelEffectiveDate"></asp:Localize>
                        <asp:TextBox ID="EffectiveDateTextBox" class="txtDate" runat="server" Width="80px"></asp:TextBox> 
                    </label>
                    <label style="width : 160px; text-align: right">
                        <asp:Localize ID="dbNameLocalize" runat="server" meta:resourcekey="LabelDBName"></asp:Localize>
                        <asp:DropDownList ID="ddlDBName" Width="100px" runat="server"></asp:DropDownList>
                    </label>
                    <label style="width : 180px; text-align: right">
                        <asp:Localize ID="UserTypeLocalize" runat="server" meta:resourcekey="LabelUserType"></asp:Localize>
                        <asp:TextBox ID="txtUserType" runat="server" CssClass="InputBox" Width="100px"></asp:TextBox> 
                        
                    </label>
                    <label>
                        <asp:Button ID="btnAdd" runat="server" Text="Add User Detail" onclick="btnAdd_Click" />
                    </label>
                </div>
                <div id="tabs-1" style="overflow: auto; height: 80px; margin-top: 0px; margin-right:2px">
                    <asp:ListView ID="lvStage" runat="server" OnItemInserted="lvStage_ItemInserted"
                        OnItemInserting="lvStage_ItemInserting" OnItemCommand="lvStage_ItemCommand"
                        OnItemEditing="lvStage_ItemEditing" onitemcreated="lvStage_ItemCreated" onitemupdating="lvStage_ItemUpdating" 
                        ViewStateMode="Enabled" onitemdatabound="lvStage_ItemDataBound" onlayoutcreated="lvStage_LayoutCreated">
                        <LayoutTemplate>
                            <table class="data_table" style="height: 14px;">
                            <tr style="height : 14px">
                                <th id="thButtons" style="height: 14px; width: 100px" runat="server"></th>
                                <th style="height: 14px"><span>Rate Hour</span></th>
                                <th style="height: 14px"><span>Sub-contractor Rate</span></th>
                                <th style="height: 14px"><span>Effective Date</span></th>
                                <th style="height: 14px"><span>DB Name</span></th>
                                <th style="height: 14px"><span>User Type</span></th>
                                <th style="display: none; height: 14px"><span>UserID</span></th>
                            </tr>
                            <tbody>
                                <tr ID="itemPlaceholder" runat="server"></tr>
                            </tbody>
                        </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr style="height : 14px">
                                <td style="height: 14px; width: 80px">
                                    <asp:LinkButton ID="imgbDelete" runat="server" CommandName="Delete" Height="14px" Text="Delete"
                                        ImageUrl="~/skin/icon/delete_icon_mono.gif" OnClientClick="return confirm('Are you sure you want to delete this row?');" ToolTip="Delete" />
                                </td>
                                <td  style="text-align: right; height: 14px">
                                    <asp:Label ID="lblRateHour" Height="14" runat="server"><%#Eval("rate_hour")%></asp:Label>
                                </td>
                                <td  style="text-align: left; height: 14px">
                                    <asp:Label ID="lblSubcontractor" Height="14" runat="server"><%#Eval("Subcontractor")%></asp:Label>
                                </td>
                                <td style="text-align: left; height: 14px">
                                    <asp:Label ID="Date" runat="server"><%#Eval("EffectiveDate", "{0:MM/dd/yyyy}")%></asp:Label>
                                </td>
                                <td  style="text-align: left; height: 14px">
                                    <asp:Label ID="lblDBName" Height="14" runat="server"><%#Eval("DBName")%></asp:Label>
                                </td>
                                <td style="text-align: left; height: 14px">
                                    <asp:Label ID="lblUserType" Height="14" runat="server"><%#Eval("UserType")%></asp:Label>
                                </td>
                                <td style="display: none; height: 14px">
                                    <asp:Label ID="lblUserID" Height="14" runat="server"><%#Eval("UserID")%></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr style="height : 14px">
                                <td style="height : 14px">
                                    <asp:LinkButton ID="imgbUpdate" runat="server" CommandName="Update" Height="14px" Text="Update"
                                        ImageUrl="~/skin/icon/save_icon_mono.gif" CausesValidation="true" ValidationGroup="vgrpSaveContact" />
                                    <asp:LinkButton ID="imgbCancel" runat="server" CommandName="Cancel" Height="14px" Text="Cancel"
                                        ImageUrl="~/skin/icon/undo_icon_mono.gif" CausesValidation="false" />
                                </td>
                                <td style="height : 14px">
                                    <asp:TextBox ID="txtRateHourEdit" style="text-align: right; " MaxLength="5" runat="server" Text='<%# Bind("rate_hour") %>'/>
                                </td>
                                <td style="height : 14px">
                                    <asp:TextBox ID="txtSubcontractor" style="text-align: right; " MaxLength="250" runat="server" Text='<%# Bind("Subcontractor") %>'/>
                                </td>
                                <td style="height : 14px">
                                    <asp:DropDownList ID="ddlDBName" runat="server"></asp:DropDownList>
                                </td>
                                <td style="height : 14px">
                                    <asp:TextBox ID="txtUserType" style="text-align: left; " MaxLength="100" runat="server" Text='<%# Bind("UserType") %>'/>
                                </td>
                                <td style="display: none; height : 14px">
                                    <asp:Label runat="server" ID="lblUserIDEdit" Text='<%# Bind("UserID") %>'></asp:Label>
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table" style="height : 14px">
                                <tr style="height : 14px">
                                    <th style="height : 14px">
                                        <span>Rate Hour</span>
                                    </th>
                                    <th style="height : 14px">
                                        <span>Sub-contractor Rate</span>
                                    </th>
                                    <th style="height : 14px">
                                        <span>Effective Date</span>
                                    </th>
                                    <th style="height : 14px">
                                        <span>DB Name</span>
                                    </th>
                                    <th style="height : 14px">
                                        <span>UserType</span>
                                    </th>
                                    <th style="display: none; height : 14px">
                                        <span>UserID</span>
                                    </th>
                                </tr>
                                <tr style="height : 14px">
                                    <td colspan="5"  style="height : 14px">
                                        <span>No Data</span>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
                <!--
        <div visible="false">
            <h4 class="user-default-header">User Default Value</h4>
        </div>
        <div id="user-default">
            <asp:ListView ID="listUserDefault" runat="server" Visible="False">
                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <th style="width: 100px">
                                <span>&nbsp;Default Code</span>
                            </th>
                            <th style="width: 100px">
                                <span>&nbsp;Default Value</span>
                            </th>
                            <th style="width: 200px">
                                <span>&nbsp;Default By Query</span>
                            </th>                            
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblNo" Text='<%#Eval("DefaultCode")%>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelRoleName" Text='<%#Eval("DefaultValue")%>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelRoleDescription" Text='<%#Eval("DefaultType")%>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        -->
                <div id="user-submit">
                    <asp:Button ID="saveButton" OnClick="SaveButton_Click" runat="server" Text="Save" AlternateText="Save" CssClass="g-button g-button-submit"/>            
                    <asp:Button ID="deleteButton" OnClick="DeleteButton_Click" runat="server" Text="Delete" AlternateText="Delete" CssClass="g-button g-button-submit"/>            
                    <asp:Button ID="cancelButton" OnClientClick="javascript:location.href='SystemUsers.aspx'; return false;" runat="server" Text="Cancel" AlternateText="Cancel" CssClass="g-button g-button-submit"/>            
                    <br />
                    <asp:Label runat="server" id="StatusLabel" text="Msg:" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
