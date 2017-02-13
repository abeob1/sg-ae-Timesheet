<%@ Page Title="Weekly Timesheet" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="WeekendTimeSheet.aspx.cs" Inherits="SAP.WeekendTimeSheet" %>

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
        function Populate(eventType) {
            //warning message box when Period had locked
            if (PeriodStatus == 1) return alert('Timesheet Posting Period had locked.');
            //onclientclick event
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
        function txtOnKeyPress(txtDate, ServiceType, DayOfWeek, TimeEndDate) {
            var curHrs = document.getElementById(txtDate);
            var hlpdsk = document.getElementById(ServiceType);
            //alert(DayOfWeek)
            //alert(TimeEndDate)
            if (DayOfWeek > TimeEndDate) {
                alert("The Time End Date of this project less than this date. Cannot key-in hour in this date.");
                curHrs.value = 0;
                return;
            }
            if (ServiceType != 'HLPDSK' && curHrs.value > 8) {
                curHrs.value = 0;
                alert("Cannot key-in more than 8 hours per day.");
            }
        }
        function OpenMoreInfo(moreinfo, status) {
            var url = "../Popup_MoreInfo.aspx?moreinfo=" + moreinfo + "&status=" + status
            Main.openCustomDialog(url, 500, 200);
        }

    </script>

    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnCountry" runat="server" />
            <asp:HiddenField ID="hdnEBMName" runat="server" />
            <asp:HiddenField ID="hdnProspectName" runat="server" />
            <asp:HiddenField ID="hdnStatus" runat="server" />
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Weekly Entry</h2>
                </div>
                <div id="header-form">
                    <div style="width: 418px; height: 49px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">Name:</td>
                                <td style="width: 250px"><asp:TextBox ID="txtUserCode" runat="server" Width="121px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 74px">From:</td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtFromDate" runat="server" 
                                       CssClass="datepicker" Width="120px"></asp:TextBox>
                                    <asp:Button ID="btnFilter" runat="server" Height="20px" OnClientClick="Dialog.showLoader();"
                                        onclick="btnFilter_Click" Text="Filter" Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tabs-1" style="overflow: auto; height: 497px; margin-top: 0px; margin-right:2px"> 
                    <asp:Button ID="btnAddRecord" Text="Add" runat="server"
                        OnClientClick="Populate('add');" 
                        Height="24px"/>
                    <asp:Button ID="btnSubmit" runat="server" onclick="btnSubmit_Click" 
                        Text="Submit" Width="68px" OnClientClick = "Populate('submit');" 
                        Height="24px" />
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                    <asp:ListView ID="lvStage" runat="server" OnItemCommand="lvStage_ItemCommand"
                        OnItemEditing="lvStage_ItemEditing" 
                        ViewStateMode="Enabled" onitemdatabound="lvStage_ItemDataBound" 
                        onlayoutcreated="lvStage_LayoutCreated" 
                        onitemdeleting="lvStage_ItemDeleting" 
                        onitemupdating="lvStage_ItemUpdating">
                        <LayoutTemplate>
                            <table class="data_table">
                                <tr>
                                    <th id="thButtons" runat="server" style="width: 70px">
                                    </th>
                                    <th style="width: 150px">
                                        <span>Project Name</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Phase</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Task</span>
                                    </th>
                                    <th style="width: 50px">
                                        <span>Billable</span>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblMon" runat="server" Text="Mon"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblTue" runat="server" Text="Tue"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblWed" runat="server" Text="Wed"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblThu" runat="server" Text="Thu"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblFri" runat="server" Text="Fri"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblSat" runat="server" Text="Sat"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblSun" runat="server" Text="Sun"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <span>Total</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Comments</span>
                                    </th>
                                    <th>
                                        <span>Project Code</span>
                                    </th>
                                    <th style="display: none">
                                        <span>IDS</span>
                                    </th>
                                    <th style="display: none">
                                        <span>SAPB1DB</span>
                                    </th>
                                    <th style="display: none">
                                        <span>Status</span>
                                    </th>
                                    <th >
                                        <span>More Info.</span>
                                    </th>

                                </tr>
                                <tbody>
                                    <tr ID="itemPlaceholder" runat="server"></tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td id="tfEmpty" runat="server">
                                            <asp:Label ID="lblEmpty" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEmpty1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEmpty2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGrandTotal" Font-Bold="True" runat="server" Text="Grand Total:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEmpty3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMonTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTueTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblWedTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblThuTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFriTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSatTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSunTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblAllTotal" Font-Bold="True" runat="server" Text="0"></asp:Label>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 50px; " id="tdButtons">
                                    <asp:LinkButton ID="imgbEdit" runat="server" OnClientClick="Populate('edit');" CommandName="Edit" Text="Edit" ImageUrl="~/skin/icon/edit_icon_mono.gif" />
                                    <asp:LinkButton ID="imgbDelete" runat="server" CommandName="Delete" Text="Delete"
                                        ImageUrl="~/skin/icon/delete_icon_mono.gif" OnClientClick="Populate('delete');"
                                        ToolTip="Delete" />
                                </td>
                                <td  style="text-align: left">
                                    <asp:Label ID="lblPrjTask" runat="server"><%#Eval("PrjName")%></asp:Label>
                                </td>
                                <td  style="text-align: left">
                                    <asp:Label ID="lblPhase" runat="server" Text='<%# Bind("Phase") %>'></asp:Label>
                                </td>
                                <td  style="text-align: left; width: 150px">
                                    <asp:Label ID="lblTask" runat="server"><%#Eval("Task")%></asp:Label>
                                </td>
                                <td style="width: 50px;text-align: center">
                                    <asp:CheckBox ID="chkBillable" Enabled="false" runat="server" Checked='<%# Convert.ToBoolean(Eval("Billable")) %>'></asp:CheckBox> 
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblMon" runat="server"><%#Eval("Mon")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblTue" runat="server"><%#Eval("Tue")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblWed" runat="server"><%#Eval("Wed")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblThu" runat="server"><%#Eval("Thu")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align: right">
                                    <asp:Label ID="lblFri" runat="server"><%#Eval("Fri")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                     <asp:Label ID="lblSat" runat="server"><%#Eval("Sat")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblSun" runat="server"><%#Eval("Sun")%></asp:Label>
                                </td>
                                <td style="width: 70px; text-align:right">
                                    <asp:Label ID="lblTotal" style="text-align: right" Font-Bold="True" runat="server"><%#Eval("Total")%></asp:Label>
                                </td>
                                <td style="text-align:left; width: 150px">
                                    <asp:Label ID="lblComments" runat="server"><%#Eval("Comments")%></asp:Label>
                                </td> 
                                <td style="text-align:left">
                                    <asp:Label ID="lblPrjCode" runat="server"><%#Eval("PrjCode")%></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblIDS" runat="server"><%#Eval("IDS")%></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblSAPB1DB" runat="server"><%#Eval("SAPB1DB")%></asp:Label>
                                </td> 
                                <td style="display: none">
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblSrvType" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblTimeEndDate" runat="server" Text='<%# Bind("U_AI_TimeEndDate") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label ID="lblMoreInfo" runat="server" Text='<%# Bind("MoreInfo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkMoreInfoTemplate" OnClick="lnkMoreInfo_Click" Text="More..." ToolTip="More Information" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr>
                                <td style="width: 50px">
                                    <asp:LinkButton ID="imgbUpdate" runat="server" CommandName="Update" Text="Update"
                                        ImageUrl="~/skin/icon/save_icon_mono.gif" CausesValidation="true" ValidationGroup="vgrpSaveContact" />
                                    <asp:LinkButton ID="imgbCancel" runat="server" CommandName="Cancel" Text="Cancel"
                                        ImageUrl="~/skin/icon/undo_icon_mono.gif" CausesValidation="false" />
                                </td>
                                <td  >
                                    <%--<asp:DropDownList ID="ddlPrjTaskEdit" AutoPostBack="true" Width="190px"
                                        onselectedindexchanged="ddlPrj_SelectedIndexChanged" runat="server" >
                                    </asp:DropDownList>--%>

                                    <asp:HyperLink ID="linkItems" ToolTip="Select Account" NavigateUrl='<%# String.Format("javascript:Main.openCustomDialog(\"../Popup_EditProject.aspx?id={0},600,610)",  Eval("No").ToString() + "\"")%>'
                                                runat="server">
                                        <asp:Image ID="imgItems" runat="server" ImageUrl="~/skin/images/item-pointer.gif" />
                                    </asp:HyperLink>
                                    <asp:Label ID="lblPrjNameEdit" runat="server" Text='<%# Bind("PrjName") %>'></asp:Label>
                                </td>
                                <td style=" width: 100px; ">
                                    <asp:DropDownList ID="ddlPhaseEdit" runat="server" >
                                        <asp:ListItem Value="Project Preparation">Project Preparation</asp:ListItem>
                                        <asp:ListItem Value="System Administration/Installation">System Administration/Installation</asp:ListItem> 
                                        <asp:ListItem Value="Design">Design</asp:ListItem>
                                        <asp:ListItem Value="Build">Build</asp:ListItem>
                                        <asp:ListItem Value="Realization">Realization</asp:ListItem>
                                        <asp:ListItem Value="Go-Live Support">Go-Live Support</asp:ListItem>
                                        <asp:ListItem Value="HelpDesk - Incident Management">HelpDesk - Incident Management</asp:ListItem>
                                        <asp:ListItem Value="HelpDesk - Enhancement">HelpDesk - Enhancement</asp:ListItem>
                                        <asp:ListItem Value="Others">Others</asp:ListItem>
                                    </asp:DropDownList>                                
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTaskEdit" style="text-align: left; " MaxLength="100" runat="server" Text='<%# Bind("Task") %>'/>
                                </td>
                                <td style="width: 50px">
                                    <asp:CheckBox ID="chkBillableEdit" Enabled="true" runat="server" Checked='<%# (int)Eval("Billable") == 1 ? true : false  %>'></asp:CheckBox>
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtMonEdit" style="text-align: right;" runat="server" Text='<%# Bind("Mon") %>'/>
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtTueEdit" style="text-align: right;" runat="server" Text='<%# Bind("Tue") %>'/>
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtWedEdit" style="text-align: right;" runat="server" Text='<%#Bind("Wed")%>' />
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtThuEdit" style="text-align: right;" runat="server" Text='<%#Bind("Thu")%>' />
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtFriEdit" style="text-align: right;" runat="server" Text='<%#Bind("Fri")%>' />
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtSatEdit" style="text-align: right;" runat="server" Text='<%#Bind("Sat")%>' />
                                </td>
                                <td style="width: 70px">
                                    <asp:TextBox ID="txtSunEdit" style="text-align: right;" runat="server" Text='<%#Bind("Sun")%>' />
                                </td>
                                <td style="width: 70px;text-align: right">
                                    <asp:Label runat="server" style="text-align: right" ID="lblTotalEdit" Font-Bold="True" Text='<%# Bind("Total") %>'></asp:Label>
                                </td>
                                <td style="width: 150px;text-align: left">
                                    <asp:TextBox ID="txtCommentsEdit" style="text-align: left; " MaxLength="100" runat="server" Text='<%# Bind("Comments") %>'/>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="lblPrjCodeEdit" Text='<%# Bind("PrjCode") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblIDSEdit" Text='<%# Bind("IDS") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblSAPB1DBEdit" Text='<%# Bind("SAPB1DB") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblStatusEdit" Text='<%# Bind("Status") %>'></asp:Label>
                                </td>
                                <td style="display: none">
                                    <asp:TextBox runat="server" ID="txtSrvTypeEdit" Text='<%# Bind("ServiceType") %>'></asp:TextBox>
                                </td>
                                <td style="display: none">
                                    <asp:TextBox runat="server" ID="txtTimeEndDateEdit" Text='<%# Bind("U_AI_TimeEndDate") %>'></asp:TextBox>
                                </td>
                                <td style="display: none">
                                    <asp:Label runat="server" ID="lblMoreInfoEdit" Text='<%# Bind("MoreInfo") %>' />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkMoreInfo" OnClick="lnkMoreInfo_Click" Text="More..." ToolTip="More Information" runat="server" />
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <EmptyDataTemplate>
                            <table class="data_table">
                                <tr>
                                    <th style="width: 150px">
                                        <span>Project Name</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Phase</span>
                                    </th>
                                    <th style="width: 100px">
                                        <span>Task</span>
                                    </th>
                                    <th style="width: 50px">
                                        <span>Billable</span>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblMon" runat="server" Text="Mon"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblTue" runat="server" Text="Tue"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblWed" runat="server" Text="Wed"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblThu" runat="server" Text="Thu"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblFri" runat="server" Text="Fri"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblSat" runat="server" Text="Sat"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <asp:Label ID="lblSun" runat="server" Text="Sun"></asp:Label>
                                    </th>
                                    <th style="width: 70px">
                                        <span>Total</span>
                                    </th>
                                    <th style="width: 150px">
                                        <span>Comments</span>
                                    </th>
                                    <th>
                                        <span>Project Code</span>
                                    </th>
                                    <th style="display: none">
                                        <span>IDS</span>
                                    </th>
                                    <th style="display: none">
                                        <span>SAPB1DB</span>
                                    </th>
                                    <th style="display: none">
                                        <span>Status</span>
                                    </th>
                                    <th>
                                        <span>More Info.</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="15">
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
