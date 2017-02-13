<%@ Page Title="My Pending for Approval Report" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TimeMyEntryReport.aspx.cs" Inherits="SAP.TimeMyEntryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small"><span>My Pending for Approval Report</span></h2>
                </div>
                <div id="header-form">
                    <div style="width: 681px; height: 55px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">
                                    <span>From date:</span></td>
                                <td style="width: 170px">
                                    <asp:TextBox ID="txtFromDate" runat="server" class="txtDate" Width="120px"></asp:TextBox>
                                </td>
                                <td class="detail_table_td_100" style="width: 79px">
                                    <span>To date:</span></td>
                                <td style="width: 246px">
                                    <asp:TextBox ID="txtToDate" runat="server"  class="txtDate" Width="120px" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnFilterMyTS" runat="server" Height="24px" 
                                        OnClientClick="Dialog.showLoader();" onclick="Button1_Click" Text="Filter" 
                                        Width="60px" />
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
                                    <th style="width: 120px">
                                        <span>Consultant Name</span>
                                    </th>
                                    <th style="width: 90px">
                                        <span>Date</span>
                                    </th>
                                    <th style="width: 90px">
                                        <span>Hour</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Project Code</span>
                                    </th>
                                    <th style="width: 250px">
                                        <span>Project Name</span>
                                    </th>
                                    <th>
                                        <span>Task</span>
                                    </th>
                                    <th>
                                        <span>Comments</span>
                                    </th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="UserCode" runat="server" Text='<%# Bind("UserCode") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Date" runat="server" Text='<%# Bind("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Hour" runat="server" Text='<%# Bind("Hour") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjCode" runat="server" Text='<%# Bind("PrjCode") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjName" runat="server" Text='<%# Bind("PrjName") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="Task" runat="server" Text='<%# Bind("Task") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="Comments" runat="server" Text='<%# Bind("Comments") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 120px">
                                        <span>Consultant Name</span>
                                    </th>
                                    <th style="width: 90px">
                                        <span>Date</span>
                                    </th>
                                    <th style="width: 90px">
                                        <span>Hour</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Project Code</span>
                                    </th>
                                    <th style="width: 250px">
                                        <span>Project Name</span>
                                    </th>
                                    <th>
                                        <span>Task</span>
                                    </th>
                                    <th>
                                        <span>Comments</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="7">
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
