<%@ Page Title="Time Entry for PM Approval by Period" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Pending4ApprTimeSheetByPeriod.aspx.cs" Inherits="SAP.Pending4ApprTimeSheetByPeriod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Time Entry for PM Approval by Period</h2>
                </div>
                <div id="header-form">
                    <div style="width: 511px; height: 74px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">From date:</td>
                                <td style="width: 170px">
                                    <asp:TextBox ID="txtFromDate" runat="server" class="txtDate" Width="120px"></asp:TextBox>
                                </td>
                                <td class="detail_table_td_100" style="width: 74px">To date:</td>
                                <td style="width: 170px">
                                    <asp:TextBox ID="txtToDate" runat="server" class="txtDate" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">Consultant:</td>
                                <td style="width: 170px">
                                    <asp:DropDownList ID="ddlUser" runat="server" Width="120px"></asp:DropDownList>
                                </td>
                                <td class="detail_table_td_100" style="width: 74px">Status:</td>
                                <td style="width: 170px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="120px">
                                        <asp:ListItem Selected="True" Value="2">Pending</asp:ListItem>
                                        <asp:ListItem Value="1">Approved</asp:ListItem>
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 130px">
                                    <asp:Button ID="btnFilter" runat="server" OnClientClick="Dialog.showLoader();" onclick="btnFilter_Click" 
                                        Text="Filter" Width="60px" Height="24px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tabs-1" 
                    style="overflow: auto; height: 497px; margin-top: 4px; margin-right:2px">
                    <asp:Button ID="btnApprove" Text="Approve" runat="server" 
                        onclick="btnApprove_Click" OnClientClick = "javascript:return confirm('Do you want to continue?');"/>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnReject" runat="server" onclick="btnReject_Click" 
                        Text="Reject" Width="53px" OnClientClick = "javascript:return confirm('Do you want to continue?');"/>
                    &nbsp;<asp:ListView ID="lvStage" runat="server"
                        OnItemEditing="lvStage_ItemEditing" 
                        ViewStateMode="Enabled" onitemdatabound="lvStage_ItemDataBound">
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
                                    <th style="width: 100">
                                        <span>Phase</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Task</span>
                                    </th>
                                    <th style="width: 50px">
                                        <span>Billable</span>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblHour" runat="server" Text="Hour"></asp:Label>
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
                                            <asp:Label ID="lblEmpty3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEmpty4" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGrandTotal" Font-Bold="True" runat="server" Text="Grand Total:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEmpty2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblEmpty5" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblHourTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
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
                                <td style="width: 70px; text-align:left">
                                    <asp:Label ID="lblDate" runat="server"><%#Eval("Date", "{0:dd/MM/yyyy}")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblTue" runat="server"><%#Eval("Hour")%></asp:Label>
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
                                    <asp:DropDownList ID="ddlPrjTaskEdit" AutoPostBack="true" Width="190px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTaskEdit" style="text-align: left; " MaxLength="100" runat="server" Text='<%# Bind("Task") %>'/>
                                </td>
                                <td style="width: 50px">
                                    <asp:CheckBox ID="chkBillableEdit" Enabled="true" runat="server" Checked='<%# (int)Eval("Billable") == 1 ? true : false  %>'></asp:CheckBox>
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtDateEdit" style="text-align: right;" runat="server" Text='<%# Bind("Date") %>'/>
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtHourEdit" style="text-align: right;" runat="server" Text='<%# Bind("Hour") %>'/>
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
                                        <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblHour" runat="server" Text="Hour"></asp:Label>
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
            <div id="progressloader">
                <div id="progressloader_dialog" class="progressloader_window">
                    <asp:Image ID="Image1" runat="server" Height="42" Width="42" ImageUrl="~/skin/images/ajax-loader.gif" />
                </div>
                    <div id="progressloader_mask"></div>
             </div>        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
