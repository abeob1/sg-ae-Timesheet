<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleLogin.aspx.cs" Inherits="SAP.SimpleLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="skin/AdminLogin.css" rel="stylesheet" type="text/css" />
    <style type="text/css">

* { margin: 0; padding: 0; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="login-form">
    <asp:ScriptManager ID="ScriptManagerMain" runat="server">
    </asp:ScriptManager>
        <asp:Login ID="Login1" runat="server" Width="16px" Height="16px" 
            DestinationPageUrl="~/TimeSheet/TimeEntryPost2SAP.aspx">
            <LayoutTemplate>
                <div id="adminlogin1">
                <br />
                    <div class="loginrow">
                        <div class="label4login">
                            <span>Username</span>
                        </div>
                        <asp:TextBox ID="UserName" runat="server" size="15" BackColor="#FFFF99" 
                            Width="170px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="loginrow">
                        <div class="label4login">
                            <span>Password</span>
                        </div>
                        <asp:TextBox ID="Password" runat="server" TextMode="Password" size="15" 
                            BackColor="#FFFF99" Width="169px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                    </div>

                    <div style="font-family: Arial; font-size: x-small; color: #FF0000; padding-left: 10px; " 
                        dir="ltr">
                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        &nbsp;</div>
                    <div style="padding-right: 15px; " dir="rtl">
                        <asp:ImageButton ID="Button1" runat="server" AlternateText="button" 
                            CommandName="Login" Height="18px" ImageUrl="~/skin/images/SAP_OK.png" 
                            OnClientClick="Dialog.showLoader();" Width="65px" />
                    </div>
                </div>
            </LayoutTemplate>
        </asp:Login>
    </div>
    </form>
    
</body>
</html>
