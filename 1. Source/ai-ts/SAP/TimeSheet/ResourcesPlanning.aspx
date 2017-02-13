<%@ Page Title="Resources Planning" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ResourcesPlanning.aspx.cs" Inherits="SAP.ResourcesPlanning" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">Resources Planning</h2>
                </div>
                <div id="header-form">
                    <style type="text/css">
                    .verticalHeader 
                    {
                        height:100px;
                        width:20px;
                        font-family: Arial, Helvetica, sans-serif;
	                    font-size: 12px;
	                    font-weight: bold;
	                    background-color: #D5E3E3;

                        -webkit-transform: rotate(90deg);
                        -moz-transform: rotate(90deg);
                        -ms-transform: rotate(-90deg);
                        -o-transform: rotate(90deg);
                        transform: rotate(-90deg);

                        -webkit-transform-origin: 50% 50%;
                        -moz-transform-origin: 50% 50%;
                        -ms-transform-origin: 50% 50%;
                        -o-transform-origin: 50% 50%;
                        transform-origin: 50% 50%;

                        filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=1);
                    }
                    .css-vertical-text {
	                    -webkit-transform:rotate(90deg);
	                    -moz-transform:rotate(90deg);
	                    -o-transform: rotate(90deg);
	                    white-space:nowrap;
	                    display:block;
	                    padding-left: 15px;
	                    position:absolute;
	                    right:-5px;
	                    top:0px;
	                    filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=1);
                    } 
                    .GridViewHeader 
                    {
	                    font-family: Arial, Helvetica, sans-serif;
	                    font-size: 12px;
	                    font-weight: bold;
	                    background-color: #D5E3E3;
                    }
                    .GridViewItem {
	                    font-family: Arial, Helvetica, sans-serif;
	                    font-size: 11px;
	                    background-color: #D2E6E6;
                    }
                    .r90 
                    {
                        height:80px;
                     	font-family: Arial, Helvetica, sans-serif;
	                    font-size: 12px;
	                    font-weight: bold;
	                    background-color: #D5E3E3;
                        -webkit-transform: rotate(-90deg);
                        -moz-transform: rotate(90deg);
                        -o-transform: rotate(90deg);
                        -ms-transform: rotate(-90deg);
                        width: 10px;
                        line-height: 1ex; 
                        margin-left:0px;
                        margin-bottom:0px;
                        margin-right:0px;
                        margin-top:0px;
                    }                                                               
                </style>
                    <div style="width: 418px; height: 35px;">
                        <table >
                            <tr>
                                <td class="detail_table_td_100" style="width: 188px">
                                    <span>From date:</span></td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtFromDate" runat="server" class="txtDate" Width="120px"></asp:TextBox>
                                </td>
                                <td class="detail_table_td_100" style="width: 130px">
                                    <span>To date:</span>
                                </td>
                                <td style="width: 246px">
                                    <asp:TextBox ID="txtToDate" runat="server"  class="txtDate" Width="120px" ></asp:TextBox>
                                </td>
                                <td class="detail_table_td_100" style="width: 100px">
                                    <asp:Button ID="btnFilter" runat="server" Height="20px" OnClientClick="Dialog.showLoader();"
                                        onclick="btnFilter_Click" Text="Filter" Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="user-submit">   
                    <form name="form1">
                        <asp:FileUpload id="FileUploadControl" runat="server" />
                        <asp:Button runat="server" id="UploadButton" text="Import" onclick="UploadButton_Click" Height="20px" />
                        <asp:Label runat="server" TextMode="MultiLine" id="StatusLabel" text="Import status: " />
                    </form>
                </div>
                <div id="tabs-1" style="overflow: auto; height: 497px; margin-top: 0px; margin-right:2px"> 
                    <tr>
                        <td>
                            <asp:Table ID="Table_ResourcesPlanningGrid" runat="server" Width="100%" CellPadding="2" CellSpacing="1" CssClass="FormBorder"></asp:Table>
                        </td>
                    </tr>
                </div>
                <div id="footer-form">
                    <div class="left">
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger  ControlID="UploadButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
