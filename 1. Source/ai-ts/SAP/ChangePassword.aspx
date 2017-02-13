<%@ Page Language="C#" AutoEventWireup="true" Title="Change Password"  MasterPageFile="~/Main.Master"  CodeBehind="ChangePassword.aspx.cs" Inherits="SAP.ChangePassword" %>



<asp:content id="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="login-form" style="position:fixed; top:30%; left:50%">

            <LayoutTemplate>
                <div id="adminlogin1">                    
                    <div class="loginrow">
                        <div class="label4login">
                            <span>Old Password</span>
                        </div>
                        <asp:TextBox ID="Password" runat="server" TextMode="Password" size="15" 
                            BackColor="#FFFF99" Width="169px" autocomplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                    </div>

                    <div class="loginrow">
                        <div class="label4login">
                            <span>New Password</span>
                        </div>
                        <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" size="15" 
                            BackColor="#FFFF99" Width="169px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="NewPassword"
                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                    </div>

                    <div class="loginrow">
                        <div class="label4login">
                            <span>Confirm</span>
                        </div>
                        <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" size="15" 
                            BackColor="#FFFF99" Width="169px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ConfirmPassword"
                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                    </div>

                    <div style="font-family: Arial; font-size: medium; color: #FF0000; padding-left: 10px; " 
                        dir="ltr">
                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        &nbsp;</div>
                    <div>
                        <asp:ImageButton ID="btn_Ok" runat="server" AlternateText="button" 
                            Height="18px" ImageUrl="~/skin/images/SAP_OK.png" 
                            Width="65px" onclick="btn_Ok_Click" />

                    </div>
                </div>
            </LayoutTemplate>

    </div>
</asp:content>

<%--
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="skin/AdminChangePass.css" rel="stylesheet" type="text/css" />
    <style type="text/css">

* { margin: 0; padding: 0; }
    </style>
</head>
<body>
  <form id="form1" runat="server">
    <div id="login-form">
    <asp:ScriptManager ID="ScriptManagerMain" runat="server">
    </asp:ScriptManager>

            <LayoutTemplate>
                <div id="adminlogin1">                    
                    <div class="loginrow">
                        <div class="label4login">
                            <span>Old Password</span>
                        </div>
                        <asp:TextBox ID="Password" runat="server" TextMode="Password" size="15" 
                            BackColor="#FFFF99" Width="169px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                    </div>

                    <div class="loginrow">
                        <div class="label4login">
                            <span>New Password</span>
                        </div>
                        <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" size="15" 
                            BackColor="#FFFF99" Width="169px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="NewPassword"
                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                    </div>

                    <div class="loginrow">
                        <div class="label4login">
                            <span>Confirm</span>
                        </div>
                        <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" size="15" 
                            BackColor="#FFFF99" Width="169px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ConfirmPassword"
                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                    </div>

                    <div style="font-family: Arial; font-size: x-small; color: #FF0000; padding-left: 10px; " 
                        dir="ltr">
                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        &nbsp;</div>
                    <div style="padding-right: 15px; " dir="rtl">
                        <asp:ImageButton ID="btn_Ok" runat="server" AlternateText="button" 
                            Height="18px" ImageUrl="~/skin/images/SAP_OK.png" 
                            Width="65px" onclick="btn_Ok_Click" />
                             <asp:ImageButton ID="btn_Back" runat="server" AlternateText="button" 
                            Height="18px" ImageUrl="~/skin/images/SAP_Back.png" 
                             Width="65px" onclick="btn_Back_Click" />
                    </div>
                </div>
            </LayoutTemplate>

    </div>
  </form>
    
</body>
</html>
--%>