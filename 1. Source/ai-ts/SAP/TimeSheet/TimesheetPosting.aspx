<%@ Page Title="Timesheet Posting Period" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TimesheetPosting.aspx.cs" Inherits="SAP.TimesheetPosting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
        $(function () {
            $('.datepicker').live('click', function () {
                $(this).datepicker({
                    constrainInput: true, dateFormat: 'mm/dd/yy'
                }).focus();
            });
        });
    </script>

    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">

        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Timesheet Posting Period Status</h2>
                </div>
                <div id="header-form">
                    <div style="width: 418px; height: 28px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">Year:</td>
                                <td style="width: 250px">
                                    <asp:DropDownList ID="ddl_Year" runat="server" >
                                        <asp:ListItem Text="2013" Value="2013"></asp:ListItem>  
                                    </asp:DropDownList>
                                    <asp:Button ID="btnFilter" runat="server" Height="20px" OnClientClick="Dialog.showLoader();"
                                        onclick="btnFilter_Click" Text="Filter" Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tabs-1" style="overflow: auto; height: 497px; margin-top: 0px; margin-right:2px"> 
                    <asp:Button ID="btnAddRecord" Text="Add" runat="server" OnClientClick="javascript:Main.openCustomDialog('../Popup_CreatePeriod.aspx?id=0',250,130); return false;" Height="24px"/>
                    <asp:ListView ID="lvStage" runat="server" OnItemCommand="lvStage_ItemCommand"
                        OnItemEditing="lvStage_ItemEditing" ViewStateMode="Enabled" 
                        onitemdatabound="lvStage_ItemDataBound">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th id="thButtons" runat="server" style="width: 100px"></th>
                                    <th style="width: 100px"><span>Month</span></th>
                                    <th style="width: 100px"><span>Status</span></th>
                                    <th style="display: none"><span>ID</span></th>
                                </tr>
                                <tbody>
                                    <tr ID="itemPlaceholder" runat="server"></tr>
                                </tbody>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td id="tdButtons" style="width: 100px">
                                    <asp:LinkButton ID="imgbEdit" runat="server" CommandName="Edit" Text="Edit" ImageUrl="~/skin/icon/edit_icon_mono.gif" CausesValidation="false" ToolTip="Edit" />
                                    <asp:LinkButton ID="imgbDelete" runat="server" CommandName="Delete" Text="Delete"
                                        ImageUrl="~/skin/icon/delete_icon_mono.gif" OnClientClick="return confirm('Are you sure you want to delete this row?');" ToolTip="Delete" />
                                </td>
                                <td  style="text-align: center">
                                    <asp:Label ID="lblMonth" runat="server"><%#Eval("Month")%></asp:Label>
                                </td>
                                <td  style="text-align: center; width: 100px">
                                    <asp:Label ID="lblStatus" runat="server"><%#Eval("Date_Locked")%></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblID" runat="server"><%#Eval("ID")%></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr>
                                <td  style="width: 100px">
                                    <asp:LinkButton ID="imgbUpdate" runat="server" CommandName="Update" Text="Update"
                                        ImageUrl="~/skin/icon/save_icon_mono.gif" CausesValidation="true" ValidationGroup="vgrpSaveContact" />
                                    <asp:LinkButton ID="imgbCancel" runat="server" CommandName="Cancel" Text="Cancel"
                                        ImageUrl="~/skin/icon/undo_icon_mono.gif" CausesValidation="false" />
                                </td>
                                <td >
                                    <asp:Label ID="lblMonthEdit" runat="server" Text='<%# Bind("Month") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_StatusEdit" runat="server" >
                                        <asp:ListItem Text="Unlocked" Value="0"></asp:ListItem>  
                                        <asp:ListItem Text="Locked" Value="1"></asp:ListItem>                                      
                                    </asp:DropDownList>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblIDEdit" Text='<%# Bind("ID") %>'></asp:Label>
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 100px"><span>Month</span></th>
                                    <th style="width: 100px"><span>Status</span></th>
                                    <th style="display: none"><span>ID</span></th>
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
