<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="SystemRoles.aspx.cs" Inherits="SAP.Admin.SystemRoles" %>

<asp:Content ID="pageContent" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="../Admin/user_style.css" rel="stylesheet" type="text/css" />
    <div id="user-main">
        <h1 class="user-header">
            Management System Roles
        </h1>
        <div id="user-section">
            <h2>
                System Role List
            </h2>
            <asp:ListView ID="systemRoleListView" runat="server">
                <LayoutTemplate>
                    <table style="width: 50%; text-align: left;">
                        <tr>
                            <th style="width: 50%; text-align: left; padding: 4px 10px;">
                                <span>User Name</span>
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 50%; text-align: left;">
                            <a href="SystemRoleEdit.aspx?RoleName=<%# Eval("RoleName") %>">
                                <%# Eval("RoleName") %></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>

        </div>
         <div id="user-submit">
            <asp:Button ID="addNewButton" OnClientClick="javascript:location.href='SystemRoleEdit.aspx'; return false;" runat="server" Text="Add New" AlternateText="Add New" CssClass="g-button g-button-submit"/>            
        </div>
    </div>
</asp:Content>
