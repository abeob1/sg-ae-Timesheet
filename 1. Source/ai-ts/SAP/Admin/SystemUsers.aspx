<%@ Page Title="Management Users" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="SystemUsers.aspx.cs" Inherits="SAP.Admin.SystemUsers" %>

<asp:content id="pageContent" runat="Server" contentplaceholderid="ContentPlaceHolder1">
   <link href="../Admin/user_style.css" rel="stylesheet" type="text/css" />
     <div id="user-main">
        <h1 class="user-header">Management System Users</h1>
         <div id="user-filter">
            <h4> Filter</h4>
            <asp:label id="userNameLabel" runat="server" associatedcontrolid="userNameTextBox"
                cssclass="FilterMessageTitle" meta:resourcekey="LabelUserName"></asp:label>
            <asp:textbox id="userNameTextBox" runat="server" cssclass="FilterInput">
            </asp:textbox>
            <asp:Button ID="filterButton" OnClick="FilterButton_Click" runat="server" Text="Filter" AlternateText="Filter" CssClass="g-button g-button-submit"/>            
        </div>
        <div id="user-section">
            <h4>System User List</h4>
            <asp:ListView ID="systemUsersListView" runat="server">
                <LayoutTemplate>
                    <table style="width: 50%; text-align: left;">
                        <tr>
                            <th class="PermissionHeaderColumn">
                                <span>&nbsp;&nbsp;User Name</span>
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 50%; text-align: left;">
                            <a href="<%# GetSystemUserEditUrl(Convert.ToString(Eval("UserName"))) %>"><%# HttpUtility.HtmlEncode((string)Eval("UserName")) %></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div> 

        <div id="user-submit">
            <asp:Button ID="addNewButton" OnClick="AddNewButton_Click" runat="server" Text="New" AlternateText="Save" CssClass="g-button g-button-submit"/>            
            <form name="form1">
                <!--input type="file" name="filediag" style="display:none" />&nbsp;
                <input type="button" value="browse..." 
                        onclick="Button1_Click()" /!-->
                <asp:FileUpload id="FileUploadControl" runat="server" />
                <asp:Button runat="server" id="UploadButton" text="Import" 
                    onclick="UploadButton_Click" Height="20px" />
                <br /><br />
                <asp:Label runat="server" id="StatusLabel" text="Import status: " />
            </form>
        </div>
</asp:content>
