<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="SystemRoleEdit.aspx.cs" Inherits="SAP.Admin.SystemRoleEdit" %>

<asp:Content ID="pageContent" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="../Admin/user_style.css" rel="stylesheet" type="text/css" />
    <link href="/skin/Admin/edit.css" rel="stylesheet" type="text/css" />
    <div id="user-main">
        <h1 class="user-header">
            Role Information</h1>
        <div id="user-role-edit">
            <div>
                <h4 style="width: 20%;display: inline;">
                    <asp:Localize ID="roleNameLocalize" runat="server" meta:resourcekey="LabelRoleName">
                    </asp:Localize>
                </h4>
                <span id="requriedSymbolSpan" runat="server" class="RequiredField">*</span>
                <asp:TextBox ID="roleNameTextBox" runat="server" CssClass="InputBox">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="roleNameRequiredFieldValidator" runat="server" CssClass="RequiredField"
                    ControlToValidate="roleNameTextBox" Display="Dynamic" ErrorMessage="Required" />
            </div>
            <br/>
            <div id="user-role-list">
                <h4>
                    <asp:Localize ID="Localize1" runat="server" meta:resourcekey="LabelPermissions">
                    </asp:Localize>
                </h4>
                <asp:GridView ID="permissionGridView" runat="server" AutoGenerateColumns="False"
                    SkinID="rolePermissionGridViewSkin">
                    <Columns>
                        <asp:BoundField DataField="PageName" HeaderStyle-CssClass="PermissionHeaderColumn"
                            HeaderText="&nbsp;&nbsp;Page Name" />
                        <asp:TemplateField HeaderStyle-CssClass="PermissionHeaderColumn" ItemStyle-CssClass="AccessCheckBoxColumn">
                            <HeaderTemplate>
                                &nbsp;<input type="checkbox" value="false" id="chkCheckAll" onclick="ChangeAllCheckBoxStates();"
                                    title="Check/uncheck all items" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="accessableCheckBox" runat="server" ToolTip="check/uncheck for accessing this page">
                                </asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="user-submit">
                 <asp:Button ID="saveButton"  OnClick="SaveButton_Click" runat="server" Text="Save" AlternateText="Save" CssClass="g-button g-button-submit"/>            
                 <asp:Button ID="deleteButton" OnClick="DeleteButton_Click" runat="server" Text="Delete" AlternateText="Delete" CssClass="g-button g-button-submit"/>            
                 <asp:Button ID="cancelButton" OnClientClick="javascript:location.href='SystemRoles.aspx'; return false;" runat="server" Text="Cancel" AlternateText="Cancel" CssClass="g-button g-button-submit"/>            
            </div>
        </div>
    </div>
    <script type="text/javascript" language="javascript">

        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null) {
                cb.checked = checkState;
            }
            else {
                alert('id: ' + id + ' was not found');
            }
        }

        function ChangeAllCheckBoxStates() {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array
            // and updates their value to the checkState input parameter
            var chkCheckAll = document.getElementById("chkCheckAll");
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++) {
                    //alert("do ChangeAllCheckBoxState " + chkCheckAll.value);
                    ChangeCheckBoxState(CheckBoxIDs[i], chkCheckAll.checked);
                }
            }
        }
 

    </script>
</asp:Content>
