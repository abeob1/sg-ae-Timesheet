<%@ Page Title="Timesheet Report" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TimeEntryReport.aspx.cs" Inherits="SAP.TimeEntryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Timesheet Report</h2>
                </div>
                <div id="header-form">
                    <div style="width: 579px; height: 80px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 188px">
                                    <span>From date:</span></td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtFromDate" runat="server" class="txtDate" Width="120px" 
                                        AutoPostBack="True" ontextchanged="txtDate_TextChanged"></asp:TextBox>
                                </td>
                                <td class="detail_table_td_100" style="width: 130px">
                                    <span>To date:</span>
                                </td>
                                <td style="width: 246px">
                                    <asp:TextBox ID="txtToDate" runat="server"  class="txtDate" Width="120px" 
                                        AutoPostBack="True" ontextchanged="txtDate_TextChanged" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 188px">
                                    <span>Reporting manager:</span>
                                </td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtReportTo" runat="server" Width="120px"> </asp:TextBox>
                                    <asp:Button ID="btnReportTo" CssClass="ButtonLarge" runat="server" Text="..." Width="20px" 
                                        OnClientClick="javascript:Main.openCustomDialog('../Popup_UserList.aspx?id=0',400,600); return false;" 
                                        Height="24px" />
                                </td>
                                <td class="detail_table_td_100" style="width: 130px">
                                    <span>Project code:</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProjectCode" runat="server" Width="180px"></asp:TextBox>
                                    <asp:Button ID="btnProjectCode" runat="server" CssClass="ButtonLarge" Height="24px" 
                                        OnClientClick="javascript:Main.openCustomDialog('../Popup_EditProject.aspx?id=0',600,610); return false;" 
                                        Text="..." Width="20px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 188px">
                                    <span>Consultant name:</span></td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtConsultant" runat="server" Width="120px"> </asp:TextBox>
                                    <asp:Button ID="btnConsultant" CssClass="ButtonLarge" runat="server" Text="..." 
                                        Width="20px" 
                                        OnClientClick="javascript:Main.openCustomDialog('../Popup_UserList.aspx?id=1',400,600); return false;" 
                                        Height="24px" />
                                </td>
                                <td class="detail_table_td_100" style="width: 130px">
                                    <asp:Button ID="btnFilterMyTS" runat="server" OnClientClick="Dialog.showLoader();" onclick="btnFilterMyTS_Click" 
                                        Text="Filter" Width="60px" Height="24px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnExport2Xlsx" runat="server" Text="Export to Excel" 
                                        Width="101px" onclick="btnExport2Xlsx_Click" Height="24px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div ID="tabs-1" 
                    style="overflow: auto; height: 497px; margin-top: 8px; margin-right: 2px;">
                    <asp:ListView ID="lvStage" runat="server">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 120px"><span>Reporting Manager</span></th>
                                    <th style="width: 120px"><span>Consultant</span></th>
                                    <th style="width: 100px"><span>Project Type</span></th>
                                    <th style="width: 100px"><span>Project Manager</span></th>
                                    <th style="width: 100px"><span>Project Code</span></th>
                                    <th ><span>Project Name</span></th>
                                    <th ><span>Customer</span></th>
                                    <th ><span>Task</span></th>
                                    <th ><span>Comments</span></th>
                                    <th style="width: 100"><span>Date</span></th>
                                    <th style="width: 90px"><span>Hour</span></th>
                                    <th style="width: 40px"><span>Rate per Hour</span></th>
                                    <th style="width: 40px"><span>Total Cost</span></th>
                                    <th style="width: 100px"><span>Status</span></th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="ReportingManager" runat="server" Text='<%# Bind("Manager") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="UserCode" runat="server" Text='<%# Bind("UserCode") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjType" runat="server" Text='<%# Bind("PrjType") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjManager" runat="server" Text='<%# Bind("PrjManager") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjCode" runat="server" Text='<%# Bind("PrjCode") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjName" runat="server" Text='<%# Bind("PrjName") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="CustName" runat="server" Text='<%# Bind("CustName") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="Task" runat="server" Text='<%# Bind("Task") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="Comments" runat="server" Text='<%# Bind("Comments") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Date" runat="server" Text='<%# Bind("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Hour" runat="server" Text='<%# Bind("Hour") %>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="rate_hour" runat="server" Text='<%# Bind("rate_hour") %>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="TotalCost" runat="server" Text='<%# Bind("TotalCost") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="Status" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 120px"><span>Reporting Manager</span></th>
                                    <th style="width: 120px"><span>Consultant</span></th>
                                    <th style="width: 100px"><span>Project Type</span></th>
                                    <th style="width: 100px"><span>Project Manager</span></th>
                                    <th style="width: 100px"><span>Project Code</span></th>
                                    <th ><span>Project Name</span></th>
                                    <th ><span>Customer</span></th>
                                    <th ><span>Task</span></th>
                                    <th ><span>Comments</span></th>
                                    <th style="width: 100px"><span>Date</span></th>
                                    <th style="width: 80px"><span>Hour</span></th>
                                    <th style="width: 40px"><span>Rate per Hour</span></th>
                                    <th style="width: 40px"><span>Total Cost</span></th>
                                    <th style="width: 100px"><span>Status</span></th>
                                </tr>
                                <tr>
                                    <td colspan="14">
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
