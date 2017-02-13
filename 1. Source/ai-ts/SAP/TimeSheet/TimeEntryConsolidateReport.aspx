<%@ Page Title="Timesheet Consolidate Report" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TimeEntryConsolidateReport.aspx.cs" Inherits="SAP.TimeEntryConsolidateReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Consolidate Report</h2>
                </div>
                <div id="header-form">
                    <div style="width: 374px; height: 59px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">
                                    Month:</td>
                                <td style="width: 113px">
                                    <asp:DropDownList ID="ddl_Month" runat="server">
                                        <asp:ListItem Value="1">01</asp:ListItem>
                                        <asp:ListItem Value="2">02</asp:ListItem>
                                        <asp:ListItem Value="3">03</asp:ListItem>
                                        <asp:ListItem Value="4">04</asp:ListItem>
                                        <asp:ListItem Value="5">05</asp:ListItem>
                                        <asp:ListItem Value="6">06</asp:ListItem>
                                        <asp:ListItem Value="7">07</asp:ListItem>
                                        <asp:ListItem Value="8">08</asp:ListItem>
                                        <asp:ListItem Value="9">09</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="detail_table_td_100" style="width: 79px">
                                    Year:</td>
                                <td style="width: 95px">
                                    <asp:DropDownList ID="ddl_Year" runat="server">
                                        <asp:ListItem>2012</asp:ListItem>
                                        <asp:ListItem>2013</asp:ListItem>
                                        <asp:ListItem>2014</asp:ListItem>
                                        <asp:ListItem>2015</asp:ListItem>
                                        <asp:ListItem>2016</asp:ListItem>
                                        <asp:ListItem>2017</asp:ListItem>
                                        <asp:ListItem>2018</asp:ListItem>
                                        <asp:ListItem>2019</asp:ListItem>
                                        <asp:ListItem>2020</asp:ListItem>
                                        <asp:ListItem>2021</asp:ListItem>
                                        <asp:ListItem>2022</asp:ListItem>
                                        <asp:ListItem>2023</asp:ListItem>
                                        <asp:ListItem>2024</asp:ListItem>
                                        <asp:ListItem>2025</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnFilterMyTS" runat="server" Height="24px" 
                                        onclick="btnFilterMyTS_Click" Text="Filter" Width="60px" />
                                </td>
                                <td style="width: 113px">
                                    <asp:Button ID="btnExport2Xlsx" runat="server" Height="24px" 
                                        Text="Export to Excel" Width="101px" onclick="btnExport2Xlsx_Click" />
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
                                        <span>Manager</span>
                                    </th>
                                    <th style="width: 200px">
                                        <span>Consultant Name</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Hours</span>
                                    </th>

                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="Manager" runat="server" Text='<%# Bind("ReportToName") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="UserCode" runat="server" Text='<%# Bind("UserCode") %>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Hours" runat="server" Text='<%# Bind("Hours") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th>
                                        <span>Manager</span>
                                    </th>
                                    <th>
                                        <span>Consultant Name</span>
                                    </th>
                                    <th>
                                        <span>Hours</span>
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
