<%@ Page Title="Timesheet Summary by Project" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TimeEntrySummaryReportByProject.aspx.cs" Inherits="SAP.TimeEntrySummaryReportByProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Timesheet Summary by Project</h2>
                </div>
                <div id="header-form">
                    <div style="width: 579px; height: 111px;">
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

                                    Company:</td>
                                <td>

                                    <asp:DropDownList ID="ddl_Company" runat="server">
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100">
                                    Employment type:</td>
                                <td>
                                    <asp:DropDownList ID="ddl_EmpType" runat="server">
                                        <asp:ListItem Value="0">Staff</asp:ListItem>
                                        <asp:ListItem Value="1">Sub-contractor</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnFilterMyTS0" runat="server" Height="24px" 
                                        onclick="btnFilterMyTS_Click" OnClientClick="Dialog.showLoader();" 
                                        Text="Filter" Width="60px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnExport2Xlsx0" runat="server" Height="24px" 
                                        onclick="btnExport2Xlsx_Click" Text="Export to Excel" Width="101px" />
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
                                    <th style="width: 100px"><span>Project Code</span></th>
                                    <th style="width: 150px"><span>Project Type</span></th>
                                    <th ><span>Project Name</span></th>
                                    <th style="width: 100px"><span>Total Hrs.</span></th>
                                    <th style="width: 100px"><span>Total Cost</span></th>
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
                                    <asp:Label ID="PrjCode" runat="server" Text='<%# Bind("PrjCode") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjType" runat="server" Text='<%# Bind("PrjType") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjName" runat="server" Text='<%# Bind("PrjName") %>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Hour" runat="server" Text='<%# Bind("Hour") %>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="TotalCost" runat="server" Text='<%# Bind("TotalCost") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 120px"><span>Reporting Manager</span></th>
                                    <th style="width: 120px"><span>Consultant</span></th>
                                    <th style="width: 100px"><span>Project Code</span></th>
                                    <th style="width: 150px"><span>Project Type</span></th>
                                    <th ><span>Project Name</span></th>
                                    <th style="width: 100px"><span>Total Hrs.</span></th>
                                    <th style="width: 100px"><span>Total Cost</span></th>
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
