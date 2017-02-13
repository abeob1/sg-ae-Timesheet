<%@ Page Title="Time Entry" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TimeEntry.aspx.cs" Inherits="SAP.TimeEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Main.myUpdatePanelId = '<%= SalesOpportunityUpdatePanel.ClientID %>';
        function Count(text, long) {
            var maxlength = new Number(long); // Change number to your max length.
            if (document.getElementById('<%=txtDescription.ClientID%>').value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
                alert("Exceeding Only " + long + " characters");
            }
        }
    </script>
    <asp:UpdatePanel ID="SalesOpportunityUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="contentData" style="padding-left: 15px; height: 391px;">
                <div id="title-form" style="border-bottom: 2px solid black;">
                    <h2 style="font-size: small">New Entry</h2>
                </div>
                <div id="header-form">
                    <div style="width: 445px; height: 196px;">
                        <table>
                            <tr>
                                <td class="detail_table_td_100" style="width: 389px">
                                    <span>Date:</span></td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" class="txtDate" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 389px">
                                    <strong>Hour</strong>:</td>
                                <td>
                                    <asp:TextBox ID="txtHour" runat="server"  Width="55px" ></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="detail_table_td_100" style="width: 389px">
                                    <strong>Project</strong>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlPrijCode" runat="server" Height="22px" Width="228px" AutoPostBack="true" 
                                        onselectedindexchanged="ddlPrijCode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtPrjCode" runat="server" Height="19px" 
                                        ReadOnly="True" Width="127px"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="detail_table_td_100" style="width: 389px; font-weight: 700;">
                                    Bill<span style="font-weight: normal">:</span></td>
                                <td>
                                    <asp:RadioButtonList ID="rblBill" runat="server" Height="21px" 
                                        Width="118px">
                                        <asp:ListItem Selected="True" Value="True">Billable</asp:ListItem>
                                        <asp:ListItem Value="False">Non Billable</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 389px; font-weight: 700;" 
                                    align="justify" valign="top">
                                    Description</td>
                                <td>
                                    <asp:TextBox ID="txtDescription" onKeyUp="javascript:Count(this,256);" onChange="javascript:Count(this,256);"
                                     ControlToValidate="textBox" Text="" 
                                        ValidationExpression="^[\s\S]{0,2}$" runat="server" Height="90px" MaxLength="256" 
                                        TextMode="MultiLine" Width="362px"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="detail_table_td_100" style="width: 389px; font-weight: 700;">
                                    &nbsp;</td>
                                <td align="right">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="detail_table_td_100" style="width: 389px; font-weight: 700;">
                                    &nbsp;</td>
                                <td align="right">
                                    <asp:Button ID="btnPostEntry" runat="server" Height="31px" Text="Save" 
                                        Width="60px" onclick="btnPostEntry_Click" Font-Size="12px" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="detail_table_td_100" style="width: 389px; font-weight: 700;">
                                    <asp:Label ID="lblMessage" runat="server" Height="20px" Text="Message:"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblMsgDesc" runat="server" ForeColor="#CC0000" Visible="False"></asp:Label>
                                </td>
                                <asp:Timer id="t1" runat="server" ontick="Timer1_Tick" interval="1000"></asp:Timer>
                            </tr>
                            
                        </table>
                    </div>
                    
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
