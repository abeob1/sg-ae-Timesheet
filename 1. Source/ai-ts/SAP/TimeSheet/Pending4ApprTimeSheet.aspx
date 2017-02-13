<%@ Page Title="Weekly Entry for PM Approval" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Pending4ApprTimeSheet.aspx.cs" Inherits="SAP.Pending4ApprTimeSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';        
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Weekly Entry for PM Approval</h2>
                </div>
                <div id="header-form">
                    <div style="width: 388px; height: 49px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">From:</td>
                                <td style="width: 170px">
                                    <asp:TextBox ID="txtFromDate" runat="server" class="txtDate" Width="120px" 
                                        ontextchanged="txtFromDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">Consultant:</td>
                                <td style="width: 170px">
                                    <asp:DropDownList ID="ddlUser" runat="server" Width="120px" 
                                        onselectedindexchanged="ddlUser_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td class="detail_table_td_100" style="width: 74px">Status:</td>
                                <td style="width: 170px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="120px" 
                                        AutoPostBack="True" onselectedindexchanged="ddlStatus_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="2">Pending</asp:ListItem>
                                        <asp:ListItem Value="1">Approved</asp:ListItem>
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tabs-1" 
                    style="overflow: auto; height: 497px; margin-top: 8px; margin-right:2px">
                    <asp:Button ID="btnApprove" Text="Approve" runat="server" 
                        onclick="btnApprove_Click" OnClientClick = "javascript:return confirm('Do you want to continue?');"/>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnReject" runat="server" onclick="btnReject_Click" 
                        Text="Reject" Width="53px" OnClientClick = "javascript:return confirm('Do you want to continue?');"/>
                    &nbsp;<asp:ListView ID="lvStage" runat="server" OnItemInserted="lvStage_ItemInserted"
                        OnItemInserting="lvStage_ItemInserting" OnItemCommand="lvStage_ItemCommand"
                        OnItemEditing="lvStage_ItemEditing" onitemcreated="lvStage_ItemCreated" onitemupdating="lvStage_ItemUpdating" 
                        ViewStateMode="Enabled" onitemdatabound="lvStage_ItemDataBound" 
                        onlayoutcreated="lvStage_LayoutCreated">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="text-align: center; width: 70px;">
                                        <asp:CheckBox ID="chkAll" Width="50" runat="server" TextAlign="Left" Checked="True" Text="Status" AutoPostBack="True" oncheckedchanged="chkAll_CheckedChanged"></asp:CheckBox>
                                    </th>
                                    <th style="width: 80px">
                                        <span>Consultant</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Project Name</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Phase</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Task</span>
                                    </th>
                                    <th style="width: 50px">
                                        <span>Billable</span>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblMon" runat="server" Text="Mon"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblTue" runat="server" Text="Tue"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblWed" runat="server" Text="Wed"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblThu" runat="server" Text="Thu"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblFri" runat="server" Text="Fri"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblSat" runat="server" Text="Sat"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblSun" runat="server" Text="Sun"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <span>Total</span>
                                    </th>
                                    <th style="width: 130px">
                                        <span>Comments</span>
                                    </th>
                                    <th>
                                        <span>Project Code</span>
                                    </th>
                                    <th style="display: none">
                                        <span>IDS</span>
                                    </th>
                                    <th style="display: none">
                                        <span>SAPB1DB</span>
                                    </th>
                                    <th style="display: none">
                                        <span>Status</span>
                                    </th>
                                </tr>
                                <tbody>
                                    <tr ID="itemPlaceholder" runat="server"></tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td id="tfEmpty" runat="server">
                                            <asp:Label ID="lblEmpty" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEmpty1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEmpty4" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEmpty3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGrandTotal" Font-Bold="True" runat="server" Text="Grand Total:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEmpty2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMonTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTueTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblWedTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblThuTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFriTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSatTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSunTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblAllTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: center; width: 70px;">
                                    <asp:CheckBox ID="Selection" TextAlign="Left" Font-Bold="true" runat="server" Checked="True"></asp:CheckBox>
                                </td>
                                <td  style="Width: 80;text-align: left">
                                    <asp:Label ID="lblUserName" runat="server"><%#Eval("UserCode")%></asp:Label>
                                </td>
                                <td  style="text-align: left">
                                    <asp:Label ID="lblPrjTask" runat="server"><%#Eval("PrjName")%></asp:Label>
                                </td>
                                <td  style="text-align: left">
                                    <asp:Label ID="lblPhase" runat="server"><%#Eval("Phase")%></asp:Label>
                                </td>
                                <td  style="text-align: left; width: 130px">
                                    <asp:Label ID="lblTask" runat="server"><%#Eval("Task")%></asp:Label>
                                </td>
                                <td style="width: 50px;text-align: center">
                                    <asp:CheckBox ID="chkBillable" Enabled="false" runat="server" Checked='<%# Convert.ToBoolean(Eval("Billable")) %>'></asp:CheckBox> 
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblMon" runat="server"><%#Eval("Mon")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblTue" runat="server"><%#Eval("Tue")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblWed" runat="server"><%#Eval("Wed")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblThu" runat="server"><%#Eval("Thu")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align: right">
                                    <asp:Label ID="lblFri" runat="server"><%#Eval("Fri")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                     <asp:Label ID="lblSat" runat="server"><%#Eval("Sat")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblSun" runat="server"><%#Eval("Sun")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblTotal" style="text-align: right" Font-Bold="True" runat="server"><%#Eval("Total")%></asp:Label>
                                </td>
                                <td style="text-align:left; width: 130px">
                                    <asp:Label ID="lblComments" runat="server"><%#Eval("Comments")%></asp:Label>
                                </td> 
                                <td style="text-align:left">
                                    <asp:Label ID="lblPrjCode" runat="server"><%#Eval("PrjCode")%></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblIDS" runat="server" Text='<%# Bind("IDS") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblSAPB1DB" runat="server"><%#Eval("SAPB1DB")%></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                </td>                                                                 
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtUserNameEdit" style="text-align: left; " runat="server" Text='<%# Bind("UserCode") %>'/>
                                </td>
                                <td  >
                                    <asp:DropDownList ID="ddlPrjTaskEdit" AutoPostBack="true" Width="190px"
                                        onselectedindexchanged="ddlPrj_SelectedIndexChanged" runat="server" >
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTaskEdit" style="text-align: left; " MaxLength="100" runat="server" Text='<%# Bind("Task") %>'/>
                                </td>
                                <td style="width: 50px">
                                    <asp:CheckBox ID="chkBillableEdit" Enabled="true" runat="server" Checked='<%# (int)Eval("Billable") == 1 ? true : false  %>'></asp:CheckBox>
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtMonEdit" style="text-align: right;" ontextchanged="txt_TextChanged" AutoPostBack="True" runat="server" Text='<%# Bind("Mon") %>'/>
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtTueEdit" style="text-align: right;" ontextchanged="txt_TextChanged" AutoPostBack="True" runat="server" Text='<%# Bind("Tue") %>'/>
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtWedEdit" style="text-align: right;" ontextchanged="txt_TextChanged" AutoPostBack="True" runat="server" Text='<%#Bind("Wed")%>' />
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtThuEdit" style="text-align: right;" ontextchanged="txt_TextChanged" AutoPostBack="True" runat="server" Text='<%#Bind("Thu")%>' />
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtFriEdit" style="text-align: right;" ontextchanged="txt_TextChanged" AutoPostBack="True" runat="server" Text='<%#Bind("Fri")%>' />
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtSatEdit" style="text-align: right;" ontextchanged="txt_TextChanged" AutoPostBack="True" runat="server" Text='<%#Bind("Sat")%>' />
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtSunEdit" style="text-align: right;" ontextchanged="txt_TextChanged" AutoPostBack="True" runat="server" Text='<%#Bind("Sun")%>' />
                                </td>
                                <td style="width: 70px;text-align: right">
                                    <asp:Label runat="server" style="text-align: right" ID="lblTotalEdit" Font-Bold="True" Text='<%# Bind("Total") %>'></asp:Label>
                                </td>
                                <td style="width: 130px;text-align: left">
                                    <asp:TextBox ID="txtCommentsEdit" style="text-align: left; " MaxLength="100" runat="server" Text='<%# Bind("Comments") %>'/>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="lblPrjCodeEdit"  Text='<%# Bind("PrjCode") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblIDSEdit"  Text='<%# Bind("IDS") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblSAPB1DBEdit"  Text='<%# Bind("SAPB1DB") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblStatusEdit"  Text='<%# Bind("Status") %>'></asp:Label>
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 70px">
                                        <span>Status</span>
                                    </th>
                                    <th style="width: 80px">
                                        <span>Consultant</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Project Name</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Phase</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Task</span>
                                    </th>
                                    <th style="width: 50px">
                                        <span>Billable</span>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblMon" runat="server" Text="Mon"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblTue" runat="server" Text="Tue"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblWed" runat="server" Text="Wed"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblThu" runat="server" Text="Thu"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblFri" runat="server" Text="Fri"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblSat" runat="server" Text="Sat"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblSun" runat="server" Text="Sun"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <span>Total</span>
                                    </th>
                                    <th style="width: 130px">
                                        <span>Comments</span>
                                    </th>
                                    <th style="display: none">
                                        <span>IDS</span>
                                    </th>
                                    <th style="display: none">
                                        <span>SAPB1DB</span>
                                    </th>
                                    <th>
                                        <span>Project Code</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="16">
                                        <span>No Data</span>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
                <div id="footer-form">
                    <div class="left">
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
