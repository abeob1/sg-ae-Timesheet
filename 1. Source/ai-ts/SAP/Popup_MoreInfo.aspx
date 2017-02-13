<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Popup_MoreInfo.aspx.cs" Inherits="SAP.Popup_MoreInfo" %>

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
            <table>
                <tr>
                    <td style="width:120px;">
                        <asp:Label ID="lblCountry" runat="server" Text="Country:" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCountry" Width="200px" Text="" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td style="width:120px;">
                        <asp:Label ID="lblEBMName" runat="server" Text="Name of the EBM:" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEBMName" Width="350px" Text="" MaxLength="250" />
                    </td>
                </tr>
                <tr>
                    <td style="width:120px;">
                        <asp:Label ID="lblProspectName" runat="server" Text="Prospect Name:" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtProspectName" Width="350px" Text="" MaxLength="250" />
                    </td>
                </tr>
            </table>
            
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="action-form">
        <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/skin/images/SAP_choose.png" OnClick="btnAdd_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/skin/images/SAP_cancel.png" OnClientClick="return Main.cancelDialogClick()" />
    </div>
    </form>
</body>
</html>