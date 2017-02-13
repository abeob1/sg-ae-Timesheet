    <%@ Page Title="Time Entry Post to SAP" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TimeEntryPost2SAP.aspx.cs" Inherits="SAP.TimeEntryPost2SAP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server" >
        <ContentTemplate>                
        <div id="overall" style="height:650px; overflow:auto">
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Time Entry Post to SAP</h2>                  
                </div>
                <div id="header-form">
                    <div style="width: 677px; height: 49px;">
                        <table style="height: 20px; width: 673px;">
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">
                                    <span>From date:</span>
                                </td>
                                <td style="width: 170px">
                                    <asp:TextBox ID="txtFromDate" runat="server" class="txtDate" Width="120px"></asp:TextBox>
                                </td>
                                <td class="detail_table_td_100" style="width: 72px">
                                    <span>To date:</span>
                                </td>
                                <td style="width: 132px">
                                    <asp:TextBox ID="txtToDate" runat="server" class="txtDate" Width="120px"></asp:TextBox>
                                </td>
                                <td style="width: 79px">
                                    
                                    Company:</td>
                                <td>
                                    <asp:DropDownList ID="ddlCompany" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">
                                    <asp:Button ID="btnFilterMyTS" runat="server" Height="24px" OnClientClick="Dialog.showLoader();" onclick="Filter_Click" Text="Filter" Width="60px" />
                                </td>
                                <td colspan="5">
                                    <asp:Button ID="btnExport" runat="server" Height="24px" AutoPostBack="true"
                                        Text="Export to Excel" Width="100px" onclick="btnExport_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div ID="tabs-1" style="overflow: auto; margin-top: 0px; margin-right: 2px;">
                    <asp:ListView ID="lvStage" runat="server">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>

                                    <th style="width: 90px">
                                        <span>Project Code</span>
                                    </th>
                                    <th style="width: 256px">
                                        <span>Project Name</span>
                                    </th>
                                    <th style="width: 90px">
                                        <span>Date</span>
                                    </th>
                                    <th style="width: 90px">
                                        <span>Consultant</span>
                                    </th>
                                    <th style="width: 60px">
                                        <span>Hours</span>
                                    </th>
                                    <th style="width: 60px">
                                        <span>Rate</span>
                                    </th>
                                     <th style="width: 90px">
                                        <span>Amount</span>
                                    </th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>

                                <td style="text-align: left">
                                    <asp:Label ID="PrjCode" runat="server" Text='<%#Bind("PrjCode")%>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="PrjName" runat="server" Text='<%#Bind("PrjName")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Date" runat="server" Text='<%#Bind("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="Consultant" runat="server" Text='<%#Bind("U_UserID")%>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Hour" runat="server" Text='<%#Bind("Hour")%>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Rate" runat="server" Text='<%#Bind("u_ai_ratehour")%>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Amount" runat="server" Text='<%#Bind("Amount")%>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 90px">
                                        <span>Project Code</span>
                                    </th>
                                    <th style="width: 256px">
                                        <span>Project Name</span>
                                    </th>
                                    <th style="width: 90px">
                                        <span>Date</span>
                                    </th>
                                    <th style="width: 90px">
                                        <span>Consultant</span>
                                    </th>
                                    <th style="width: 60px">
                                        <span>Hours</span>
                                    </th>
                                    <th style="width: 60px">
                                        <span>Rate</span>
                                    </th>
                                    <th>
                                        <span>Amount</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <span>No Data</span>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>

                    <div id="tabs-2" style="overflow: auto; margin-top: 0px; margin-right: 2px;">
<%--                      <td>   <asp:Button ID="btn_Sumary" runat="server" Text="Sumarize"  OnClientClick="Dialog.showLoader();" 
                              onclick="btn_Sumary_Click" />   
                    </td>--%>
                    <td style="text-align: center">                                                   
                        <asp:Label ID="lblSummary" runat="server" Font-Underline="True" Text="Summary Data:" 
                            Font-Bold="True"></asp:Label>  
                    </td>
                    <asp:ListView ID="lvSummary" style="text-align: center" runat="server" >
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 150px">
                                        <span>Company</span>
                                    </th>
                                    <th  style="width: 150px">
                                        <span>Project Code</span>
                                    </th>
                                    <th  style="width: 100px">
                                        <span>Amount</span>
                                    </th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="Company" Font-Bold="True" runat="server" Text='<%#Bind("Company")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="ProjCode" Font-Bold="True" runat="server" Text='<%#Bind("PrjCode")%>'></asp:Label>
                                </td>
                                <td style="text-align:right">
                                    <asp:Label ID="Amount" Font-Bold="True" runat="server" Text='<%#Bind("Amount")%>'></asp:Label>
                                </td>                    
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th >
                                        <span>Company</span>
                                    </th>
                                    <th >
                                        <span>Project Code</span>
                                    </th>
                                    <th >
                                        <span>Amount</span>
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
                <div style="width: 791px;">
                        <table>
                            <tr>
                                <td>
                                   <%-- <asp:Button ID="Button1" runat="server" Height="31px" 
                                        onclick="Simulate_Click" Text="Simulate" Width="60px" Visible="False" />--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                <div id="tabs-3" style="overflow:auto; margin-top: 0px;">
                  <td style="text-align: center">                                                   
                        <asp:Label ID="Label1" runat="server" Font-Underline="True" Text="Data Simulated:" 
                            Font-Bold="True"></asp:Label>  
                    </td>

                    <div id="fl" style="overflow:scroll">
                    <table>
                    <tr>
                    
                        <td>
                       
                          <asp:ListView ID="lvSimulate" style="text-align: center" runat="server">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 100px">
                                        <span>Company</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Account Code</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Account Name</span>
                                    </th>
                                    <th  style="width: 70px">
                                        <span>Debit</span>
                                    </th>
                                    <th  style="width: 70px">
                                        <span>Credit</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Project</span>
                                    </th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="siCompany" Font-Bold="True" runat="server"><%#Eval("Company")%></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="siAccount" Font-Bold="True" runat="server"><%#Eval("Account")%></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" Font-Bold="True" runat="server"><%#Eval("AccountName")%></asp:Label>
                                </td>
                                <td style="text-align:right">
                                    <asp:Label ID="siDebit" Font-Bold="True" runat="server"><%#Eval("Debit")%></asp:Label>
                                </td>
                                <td style="text-align:right">
                                    <asp:Label ID="siCredit" Font-Bold="True" runat="server"><%#Eval("Credit")%></asp:Label>
                                </td> 
                                <td>
                                    <asp:Label ID="siProject" Font-Bold="True" runat="server"><%#Eval("Project")%></asp:Label>
                                </td>                    
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th >
                                        <span>Company</span>
                                    </th>
                                    <th >
                                        <span>Account Code</span>
                                    </th>
                                    <th >
                                        <span>Account Name</span>
                                    </th>
                                    <th >
                                        <span>Debit</span>
                                    </th>
                                    <th >
                                        <span>Credit</span>
                                    </th>
                                    <th >
                                        <span>Project</span>
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

                        </td>
                        <td style="width:40px"></td>
                        <td>
                             <asp:ListView ID="lvSimulateAcc" style="text-align: center" runat="server">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 100px">
                                        <span>Company</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Account Code</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Account Name</span>
                                    </th>
                                    <th  style="width: 70px">
                                        <span>Debit</span>
                                    </th>
                                    <th  style="width: 70px">
                                        <span>Credit</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Project</span>
                                    </th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="siCompany" Font-Bold="True" runat="server"><%#Eval("Company")%></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="siAccount" Font-Bold="True" runat="server"><%#Eval("Account")%></asp:Label>
                                </td>

                                <td>
                                    <asp:Label ID="Label2" Font-Bold="True" runat="server"><%#Eval("AccountName")%></asp:Label>
                                </td>

                                <td style="text-align:right">
                                    <asp:Label ID="siDebit" Font-Bold="True" runat="server"><%#Eval("Debit")%></asp:Label>
                                </td>
                                <td style="text-align:right">
                                    <asp:Label ID="siCredit" Font-Bold="True" runat="server"><%#Eval("Credit")%></asp:Label>
                                </td> 
                                <td>
                                    <asp:Label ID="siProject" Font-Bold="True" runat="server"><%#Eval("Project")%></asp:Label>
                                </td>                    
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th >
                                        <span>Company</span>
                                    </th>
                                    <th >
                                        <span>Account Code</span>
                                    </th>
                                    <th >
                                        <span>Account Name</span>
                                    </th>
                                    <th >
                                        <span>Debit</span>
                                    </th>
                                    <th >
                                        <span>Credit</span>
                                    </th>
                                    <th >
                                        <span>Project</span>
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
                       
                        </td>
                    </tr>
                    </table>
                  </div>
                   

                </div>
                <div style="width: 791px; height: 41px;">
                        <table style="height: 20px; width: 564px;">
                            <tr>
                                <td>
                                    <asp:Button ID="btnPost" runat="server" Height="31px" onclick="Post_Click"  OnClientClick="Dialog.showLoader();" 
                                        Text="Post" Width="60px" />
                                        <asp:Label ID="lbError" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>  
                </div>                
                             
            </div>

            <div id="progressloader">
                <div id="progressloader_dialog" class="progressloader_window">
                    <asp:Image ID="Image1" runat="server" Height="42" Width="42" ImageUrl="~/skin/images/ajax-loader.gif" />
                </div>
                    <div id="progressloader_mask"></div>
                </div>
            </div>
 </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
