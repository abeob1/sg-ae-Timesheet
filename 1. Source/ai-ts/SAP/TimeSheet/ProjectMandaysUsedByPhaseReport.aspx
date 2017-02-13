<%@ Page Title="Project Man-days Used Report" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ProjectMandaysUsedByPhaseReport.aspx.cs" Inherits="SAP.ProjectMandaysUsedByPhaseReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Project Man-days Used Report</h2>
                </div>
                <div id="header-form">
                    <div style="width: 749px; height: 80px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 100px">
                                    To<span> date</span></td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtToDate" runat="server" class="txtDate" Width="120px" 
                                        AutoPostBack="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 100px">
                                    <span>Project code:</span>
                                </td>
                                <td class="detail_table_td_100" style="width: 250px">
                                    <asp:TextBox ID="txtProjectCode" runat="server" Width="180px"></asp:TextBox>
                                    <asp:Button ID="btnProjectCode" runat="server" CssClass="ButtonLarge" Height="24px" 
                                        OnClientClick="javascript:Main.openCustomDialog('../Popup_EditProject.aspx?id=0',600,610); return false;" 
                                        Text="..." Width="20px" />
                                </td>
                                <td class="detail_table_td_100" style="width: 100px">
                                    <span>Company:</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCompany" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 100px">
                                    <asp:Button ID="btnFilterMyTS" runat="server" OnClientClick="Dialog.showLoader();" onclick="btnFilterMyTS_Click" Text="Filter" Width="60px" Height="24px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnExport2Xlsx" runat="server" Text="Export to Excel" Width="101px" onclick="btnExport2Xlsx_Click" Height="24px" />
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
                                    <th style="width: 150px"><span>Project Code</span></th>
                                    <th ><span>Project Name</span></th>
                                    <th style="width: 150px"><span>Phase</span></th>
                                    <th style="width: 100px"><span>Man-days Utilized</span></th>
                                    <th style="display: none"><span>Total</span></th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjCode" runat="server" Text='<%# Bind("PrjCode") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjName" runat="server" Text='<%# Bind("PrjName") %>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="Phase" runat="server" Text='<%# Bind("Phase") %>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="MandaysUtilized" runat="server" Text='<%# Bind("MandaysUtilized") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 150px"><span>Project Code</span></th>
                                    <th ><span>Project Name</span></th>
                                    <th style="width: 120px"><span>Phase</span></th>
                                    <th style="width: 100px"><span>Man-days Utilized</span></th>
                                </tr>
                                <tr>
                                    <td colspan="4">
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
