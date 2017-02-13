<%@ Page Title="Summary of Timesheet Approved" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TimeEntryMissingReport.aspx.cs" Inherits="SAP.TimeEntryMissingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Summary of Timesheet Approved</h2>
                </div>
                <div id="header-form">
                    <div style="width: 490px; height: 59px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 118px">From date:</td>
                                <td style="width: 193px">
                                    <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="True" class="txtDate" ontextchanged="txtDate_TextChanged" Width="120px"></asp:TextBox>
                                </td>
                                <td class="detail_table_td_100" style="width: 128px">To date:</td>
                                <td style="width: 95px">
                                    <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="True" class="txtDate" ontextchanged="txtDate_TextChanged" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 118px">
                                    <asp:Button ID="btnFilterMyTS" runat="server" OnClientClick="Dialog.showLoader();" onclick="btnFilterMyTS_Click" Text="Filter" Width="60px" Height="24px" />
                                </td>
                                <td style="width: 193px">
                                    <asp:Button ID="btnExport2Xlsx" runat="server" Text="Export to Excel" Width="101px" onclick="btnExport2Xlsx_Click" Height="24px" />
                                </td>
                                <td>Company:</td>
                                <td>
                                    <asp:DropDownList ID="ddl_Company" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div ID="tabs-1" style="overflow: auto; height: 497px; margin-top: 0px; margin-right: 2px;">
                    <asp:ListView ID="lvStage" runat="server">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 200px">
                                        <span>Reporting Manager</span>
                                    </th>
                                    <th style="width: 200px">
                                        <span>Consultant Name</span>
                                    </th>
                                    <th style="width: 200px">
                                        <span>Pending for Approval (Hours)</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Approved (Hours)</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Total Hours</span>
                                    </th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="ReportingManager" runat="server" Text='<%# Bind("ReportingManager") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="UserCode" runat="server" Text='<%# Bind("UserCode") %>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="PendingHours" runat="server" Text='<%# Bind("PendingHours") %>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="ApprovedHours" runat="server" Text='<%# Bind("ApprovedHours") %>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="TotalHours" runat="server" Text='<%# Bind("TotalHours") %>'></asp:Label>
                                </td>

                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 200px">
                                        <span>Reporting Manager</span>
                                    </th>
                                    <th style="width: 200px">
                                        <span>Consultant Name</span>
                                    </th>
                                    <th style="width: 200px">
                                        <span>Pending for Approval (Hours)</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Approved (Hours)</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Total Hours</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="5">
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
