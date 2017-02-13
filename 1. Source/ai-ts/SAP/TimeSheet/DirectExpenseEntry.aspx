<%@ Page Title="Direct Expense Entry" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DirectExpenseEntry.aspx.cs" Inherits="SAP.DirectExpenseEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';

        function isNumberKey(sender, evt) {
            var txt = sender.value;
            var dotcontainer = txt.split('.');
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (!(dotcontainer.length == 1 && charCode == 46) && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function Populate(eventType) {
            switch (eventType) {
                case "add":
                    Main.openCustomDialog('../Popup_EditProject.aspx?id=0', 600, 610); return false;
                    break;

                case "submit":
                case "delete":
                    return confirm('Do you want to continue?');
                    break;
            }
        }
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Direct Expense Entry</h2>
                </div>
                <div id="header-form">
                    <div style="width: 885px; height: 60px;">
                        <table style=" width: 880px">
                            <tr>
                                <td style="width: 10%">From Date:</td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtFromDate" runat="server" class="txtDate" Width="120px"></asp:TextBox>
                                </td>
                                <td style="width: 10%">To Date:</td>
                                <td style="width: 65%">
                                    <asp:TextBox ID="txtToDate" runat="server" class="txtDate" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Button ID="btnFilter" runat="server" Height="24px" OnClientClick="Dialog.showLoader();"
                                        onclick="btnFilter_Click" Text="Filter" Width="50px" />
                                </td>
                                <td colspan="3">
                                    <asp:FileUpload id="FileUploadControl" runat="server" />
                                    <asp:Button runat="server" id="UploadButton" text="Import" 
                                        onclick="UploadButton_Click" Height="24px" />
                                    <asp:Label runat="server" TextMode="MultiLine" id="StatusLabel" text="Import status: " />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tabs-1" style="overflow: auto; margin-top: 0px;">
                    <asp:Button ID="btnAddRecord" Text="Add" runat="server" OnClientClick="Populate('add');" Height="24px"/>

                    <asp:ListView ID="lvStage" runat="server" OnItemInserted="lvStage_ItemInserted"
                        OnItemInserting="lvStage_ItemInserting" OnItemCommand="lvStage_ItemCommand"
                        OnItemEditing="lvStage_ItemEditing" onitemcreated="lvStage_ItemCreated" onitemupdating="lvStage_ItemUpdating" 
                        ViewStateMode="Enabled" onitemdatabound="lvStage_ItemDataBound" 
                        onlayoutcreated="lvStage_LayoutCreated">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th id="thButtons" runat="server" style="width: 90px">
                                    </th>
                                    <th style="width: 100px">
                                        <span>Date</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Project Code</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Amount</span>
                                    </th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="imgbEdit" runat="server" OnClientClick="Populate('edit');" CommandName="Edit" Text="Edit" ImageUrl="~/skin/icon/edit_icon_mono.gif" />
                                    <asp:LinkButton ID="imgbDelete" runat="server" CommandName="Delete" Text="Delete"
                                        ImageUrl="~/skin/icon/delete_icon_mono.gif" OnClientClick="Populate('delete');"
                                        ToolTip="Delete" />
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="lblDate" runat="server"><%#Eval("Date", "{0:MM/dd/yyyy}")%></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="lblPrjCode" runat="server"><%#Eval("PrjCode")%></asp:Label>
                                </td>                                
                                <td style="text-align: right">
                                    <asp:Label ID="lblAmount" runat="server"><%#Eval("Amount")%></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblSAPB1DB" style="text-align:center;" Text='<%# Bind("SAPB1DB") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblinternal_id" style="text-align:center;" Text='<%# Bind("internal_id") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr>
                                <td style="width: 100px">
                                    <asp:LinkButton ID="imgbUpdate" runat="server" CommandName="Update" Text="Update"
                                        ImageUrl="~/skin/icon/save_icon_mono.gif" CausesValidation="true" ValidationGroup="vgrpSaveContact" />
                                    <asp:LinkButton ID="imgbCancel" runat="server" CommandName="Cancel" Text="Cancel"
                                        ImageUrl="~/skin/icon/undo_icon_mono.gif" CausesValidation="false" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDateEdit" class="txtDate" Text='<%# Bind("Date", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                </td>
                                <td >
                                    <asp:HyperLink ID="linkItems" ToolTip="Select Account" NavigateUrl='<%# String.Format("javascript:Main.openCustomDialog(\"../Popup_EditProject.aspx?id={0},600,610)",  Eval("No").ToString() + "\"")%>'
                                                runat="server">
                                        <asp:Image ID="imgItems" runat="server" ImageUrl="~/skin/images/item-pointer.gif" />
                                    </asp:HyperLink>
                                    <asp:Label runat="server" ID="lblPrjCodeEdit"  Text='<%# Bind("PrjCode") %>'></asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox runat="server" ID="txtAmountEdit" OnKeyPress="return isNumberKey(this, event);" style="text-align: right;" Text='<%# Bind("Amount") %>'></asp:TextBox>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblSAPB1DBEdit" style="text-align:center;" Text='<%# Bind("SAPB1DB") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblinternal_idEdit" style="text-align:center;" Text='<%# Bind("internal_id") %>'></asp:Label>
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 100px">
                                        <span>Date</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Project Code</span>
                                    </th>
                                    <th style="width: 100px">
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
                <div id="footer-form">
                    <div class="clear">
                    </div>
                </div>
            </div>
            <div id="user-submit">   
                <form name="form1">

                </form>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger  ControlID="UploadButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
