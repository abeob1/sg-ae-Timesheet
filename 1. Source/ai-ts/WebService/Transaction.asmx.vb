Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Transaction
    Inherits System.Web.Services.WebService

    Dim lRetCode As Integer
    Dim lErrCode As Integer
    Dim sErrMsg As String
    Dim connect As New Connection()
    <WebMethod()> _
    Public Function CreateMarketingDocument(ByVal strXml As String, UserID As String, DocType As String, Key As String, IsUpdate As Boolean) As DataSet
        Dim b As New SAP_Functions
        Try
            Dim sStr As String = "Operation Completed Successfully!"
                Dim oDocment
                Select Case DocType
                    Case "30"
                        oDocment = DirectCast(oDocment, SAPbobsCOM.JournalEntries)
                    Case "97"
                        oDocment = DirectCast(oDocment, SAPbobsCOM.SalesOpportunities)
                    Case "191"
                        oDocment = DirectCast(oDocment, SAPbobsCOM.ServiceCalls)
                    Case "33"
                        oDocment = DirectCast(oDocment, SAPbobsCOM.Contacts)
                    Case "221"
                        oDocment = DirectCast(oDocment, SAPbobsCOM.Attachments2)
                    Case "2"
                        oDocment = DirectCast(oDocment, SAPbobsCOM.BusinessPartners)
                    Case "28"
                        oDocment = DirectCast(oDocment, SAPbobsCOM.IJournalVouchers)
                    Case Else
                        oDocment = DirectCast(oDocment, SAPbobsCOM.Documents)
                End Select

                Dim constr As String
                constr = connect.connectDB(UserID)
                If constr <> "" Then
                    Return b.ReturnMessage(-1, constr)
                End If

                PublicVariable.oCompany.XMLAsString = True
                oDocment = PublicVariable.oCompany.GetBusinessObjectFromXML(strXml, 0)
                If IsUpdate Then
                    If oDocment.GetByKey(Key) Then
                        oDocment.Browser.ReadXML(strXml, 0)
                        lErrCode = oDocment.Update()
                    Else
                        Return b.ReturnMessage(-1, "Record not found!")
                    End If
                Else
                    lErrCode = oDocment.Add()
                End If

                If lErrCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(lErrCode, sErrMsg)
                    Return b.ReturnMessage(lErrCode, sErrMsg)
                Else
                    Return b.ReturnMessage(lErrCode, "Operation Sucessful!")
                End If
        Catch ex As Exception
            Return b.ReturnMessage(-1, ex.ToString)
        End Try
    End Function
   
End Class