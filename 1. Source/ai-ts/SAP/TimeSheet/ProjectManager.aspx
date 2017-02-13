<%@ Page Title="Project Manager" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ProjectManager.aspx.cs" Inherits="SAP.ProjectManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
        function ScrollToBottom() {
            var divTabs = document.getElementById('tabs-1');
            divTabs.scrollTop = divTabs.scrollHeight;
        }
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Project Manager</h2>
                </div>
                <div id="tabs-1" style="overflow: auto; height: 497px; margin-top: 0px;">
                    <asp:ListView ID="lvStage" runat="server" OnItemInserted="lvStage_ItemInserted"
                        OnItemInserting="lvStage_ItemInserting" OnItemCommand="lvStage_ItemCommand"
                        OnItemEditing="lvStage_ItemEditing" onitemcreated="lvStage_ItemCreated" onitemupdating="lvStage_ItemUpdating" 
                        ViewStateMode="Enabled" onitemdatabound="lvStage_ItemDataBound" 
                        onlayoutcreated="lvStage_LayoutCreated">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th id="thButtons" runat="server" style="width: 90px">
                                    </th>
                                    <th style="width: 200px">
                                        <span>Project Name</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>User Name</span>
                                    </th>
                                    <th style="display: none">
                                        <span>Status</span>
                                    </th>
                                    <th style="display: none">
                                        <span>ID</span>
                                    </th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                <asp:LinkButton ID="imgbEdit" runat="server" CommandName="Edit" Text="Edit" ImageUrl="~/skin/icon/edit_icon_mono.gif" />
                                <asp:LinkButton ID="imgbDelete" runat="server" CommandName="Delete" Text="Delete"
                                    ImageUrl="~/skin/icon/delete_icon_mono.gif" OnClientClick="return confirm('Are you sure you want to delete this row?');"
                                    ToolTip="Delete" />
                                </td>
                                <td  style="width: 500px; text-align: left">
                                    <asp:Label ID="lblPrjName" style="width: 98%;" runat="server"><%#Eval("PrjName")%></asp:Label>
                                </td>
                                <td  style="width: 150px; text-align: left">
                                    <asp:Label ID="lblUserName" runat="server"><%#Eval("UserName")%></asp:Label>
                                </td>                                
                                <td style="display: none">
                                    <asp:Label ID="lblStatus" runat="server"><%#Eval("StatusText")%></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblID" runat="server"><%#Eval("ID")%></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="imgbUpdate" runat="server" CommandName="Update" Text="Update"
                                        ImageUrl="~/skin/icon/save_icon_mono.gif" CausesValidation="true" ValidationGroup="vgrpSaveContact" />
                                    <asp:LinkButton ID="imgbCancel" runat="server" CommandName="Cancel" Text="Cancel"
                                        ImageUrl="~/skin/icon/undo_icon_mono.gif" CausesValidation="false" />
                                </td>
                                <td style="width: 200px; text-align:left">
                                    <asp:DropDownList ID="ddlPrjNameEdit" AutoPostBack="true" style="width: 98%;"
                                        onselectedindexchanged="ddlPrjName_SelectedIndexChanged" runat="server" >
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 150px; text-align:left">
                                    <asp:DropDownList ID="ddlUserNameEdit" AutoPostBack="true" width="140" 
                                        onselectedindexchanged="ddlUserName_SelectedIndexChanged" runat="server" >
                                    </asp:DropDownList>
                                </td>                                
                                <td style="display: none">
                                    <asp:DropDownList ID="ddlStatusEdit" runat="server">
                                        <asp:ListItem Text="Active" Value="1" />
                                        <asp:ListItem Text="Deactive" Value="0" />
                                    </asp:DropDownList>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblIDEdit"  Text='<%# Bind("ID") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblPrjCodeEdit"  Text='<%# Bind("PrjCode") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblUserIDEdit"  Text='<%# Bind("UserID") %>'></asp:Label>
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="imgbUpdate" runat="server" CommandName="Insert" Text="Update"
                                        ImageUrl="~/skin/icon/save_icon_mono.gif" CausesValidation="true" ValidationGroup="vgrpSaveContact" />
                                    <asp:LinkButton ID="imgbCancel" runat="server" CommandName="Cancel" Text="Cancel"
                                        ImageUrl="~/skin/icon/undo_icon_mono.gif" CausesValidation="false" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlUserNameInsert" runat="server" >
                                    </asp:DropDownList>   
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPrjNameInsert" runat="server" >
                                    </asp:DropDownList>   
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatusInsert" runat="server">
                                        <asp:ListItem Text="Active" Value="1" />
                                        <asp:ListItem Text="Deactive" Value="0" />
                                    </asp:DropDownList>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblIDInsert"  Text='<%# Bind("ID") %>'></asp:Label>
                                </td>
                            </tr>
                        </InsertItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 200px">
                                        <span>Project Name</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>User Name</span>
                                    </th>
                                    <th style="display: none">
                                        <span>Status</span>
                                    </th>
                                    <th style="display: none">
                                        <span>ID</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <span>No Data</span>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <asp:Button ID="btnAddRecord" Text="Add" runat="server" onclick="btnAddRecord_Click"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblMsgDesc" runat="server" ForeColor="#000099" Font-Bold="True" Visible="False"></asp:Label>
                </div>
                <div id="footer-form">
                    <div class="clear">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
