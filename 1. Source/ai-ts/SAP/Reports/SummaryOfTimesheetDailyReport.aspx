<%@ Page Title="Timesheet Report" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SummaryOfTimesheetDailyReport.aspx.cs" Inherits="SAP.SummaryOfTimesheetDailyReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2>Summary Of Timesheet Approved - Daily View</h2>
                </div>
                <div class="top">
                    <table class="detail_table">
                            
                            <tr>
                                <td class="detail_table_td_150" style="width: 15%; border-bottom: dotted 1px #808080;">
                                    <span>From Date</span>
                                </td>
                                <td style="width: 15%;">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_150" style="border-bottom: dotted 1px #808080;">
                                    <span>To Date</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="txtDate"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnView" runat="server" Height="22px" onclick="btnView_Click" Text="View" Width="71px" />
                                </td>
                            </tr>
                        </table>
                </div>
                <div>
                    <rsweb:ReportViewer ID="objReportViewer" runat="server" Font-Names="Verdana" 
                        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="97%" 
                        Height="430px" ZoomMode="PageWidth" AsyncRendering="True"
                        ShowParameterPrompts="True">
                    </rsweb:ReportViewer>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
