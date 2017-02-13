<%@ Page Title="My Timesheet" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="MyTimeSheet.aspx.cs" Inherits="SAP.MyTimeSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">My Entry</h2>
                </div>
                <div id="header-form">
                    <div style="width: 681px; height: 69px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">
                                    <span>From date:</span>
                                </td>
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
                                <td class="detail_table_td_100" style="width: 74px">
                                    <span>Type:</span></td>
                                <td style="width: 170px">
                                    <asp:DropDownList ID="ddlType" runat="server" Width="120px">
                                        <asp:ListItem Text="New" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Pending for Approval" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Posted to SAP" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    
                                </td>
                                <td style="width: 79px">
                                    Project name:</td>
                                <td style="width: 246px">
                                    
                                    <asp:DropDownList ID="ddl_Project" runat="server" Width="239px">
                                    </asp:DropDownList>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnFilterMyTS" runat="server" CssClass="ButtonLarge" 
                                        Height="24px" OnClientClick="Dialog.showLoader();" onclick="Button1_Click" 
                                        Text="Filter" Width="60px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div ID="tabs-1" style="overflow: auto; height: 497px; margin-top: 0px; margin-right: 2px;">
                    <asp:ListView ID="lvStage" runat="server" onitemcommand="lvStage_ItemCommand" 
                        OnItemEditing="lvStage_ItemEditing">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
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
                                    <th style="width: 150px">
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
                                <td>
                                    <asp:Label ID="Date" runat="server"><%#Eval("Date", "{0:dd/MM/yyyy}")%></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Hour" runat="server"><%#Eval("Hour")%></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjCode" runat="server"><%#Eval("PrjCode")%></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjName" runat="server"><%#Eval("PrjName")%></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="Task" runat="server"><%#Eval("Task")%></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="Comments" runat="server"><%#Eval("Comments")%></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th>
                                        <span>Date</span>
                                    </th>
                                    <th>
                                        <span>Hour</span>
                                    </th>
                                    <th>
                                        <span>Project Code</span>
                                    </th>
                                    <th>
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
                                    <td colspan="6">
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
