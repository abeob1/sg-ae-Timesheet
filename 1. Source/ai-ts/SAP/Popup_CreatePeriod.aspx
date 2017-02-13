<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Popup_CreatePeriod.aspx.cs" Inherits="SAP.Popup_CreatePeriod" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="/js/jquery-1.4.3.min.js"></script>
    <script type="text/javascript" src="/js/main.js"></script>
    <link href="skin/skin.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form2" runat="server">
    <asp:ScriptManager ID="ScriptManagerEditWareHouse" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="editWareHouseUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">       
        <ContentTemplate>
            <div id="header-form">
                <div >
                    <table >
                        <tr>
                            <td class="detail_table_td_100" style="width: 100px">Month:</td>
                            <td style="width: 60px">
                                <asp:DropDownList ID="ddl_Month" runat="server" Width="70px">
                                    <asp:ListItem>01</asp:ListItem>
                                    <asp:ListItem>02</asp:ListItem>
                                    <asp:ListItem>03</asp:ListItem>
                                    <asp:ListItem>04</asp:ListItem>
                                    <asp:ListItem>05</asp:ListItem>
                                    <asp:ListItem>06</asp:ListItem>
                                    <asp:ListItem>07</asp:ListItem>
                                    <asp:ListItem>08</asp:ListItem>
                                    <asp:ListItem>09</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>11</asp:ListItem>
                                    <asp:ListItem>12</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="detail_table_td_101" style="width: 40px">Year:</td>
                            <td style="width: 60px">
                                <asp:DropDownList ID="ddl_Year" runat="server" Width="60px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="detail_table_td_100" style="width: 100px">Number of Period:</td>
                            <td style="width: 60px">
                                <asp:TextBox ID="txtNumberOfPeriod" runat="server"  Width="50px" 
                                    MaxLength="2" ></asp:TextBox>
                            </td>                            
                        </tr>
                    </table>
                </div>
          </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="action-form">
        <asp:ImageButton ID="btnAdd" runat="server" 
            ImageUrl="~/skin/images/SAP_Add.png" OnClick="btnAdd_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/skin/images/SAP_cancel.png" OnClientClick="return Main.cancelDialogClick()" />
    </div>
    </form>
</body>
</html>