Option Explicit On

Imports ConnectTo
Imports System
Imports System.Data.SqlClient

Partial Class inSendSMS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Mirror As New Comando
        Dim arrFones() As String
        Dim strFones As String = ""
        Dim strFone As String = ""
        Dim strMsg As String = ""
        Dim strSql As String = ""
        Dim strURL As String = ""
        Dim strHTML As String = ""
        Dim strStatus As String = ""
        Dim strNome As String = ""
        Dim x As Short = 0
        Dim y As Short = 0
        Dim intId As Integer = 0
        Dim intCTRA_ID As Integer = 0
        Dim intQtde As Short = 0
        Dim objCELVerify As SqlDataReader = Nothing
        Dim blnOK As Boolean
        Dim intUserID As Integer
        Dim strCarteira As String = ""
        Dim strContratante As String = ""

        On Error Resume Next

        Mirror.Banco = "MIRRORWEB"

        strStatus = Trim(Request.QueryString("Status"))
        strNome = Trim(Request.QueryString("Nome"))
        intId = CInt(Trim(Request.QueryString("ID"))) + 1
        intCTRA_ID = CInt(Trim(Request.QueryString("CTRA_ID")))
        strFones = Request.QueryString("Fones")
        intUserID = Request.QueryString("UserID")
        strCarteira = Request.QueryString("carteira")
        strContratante = Request.QueryString("contratante")

        strHTML = ""

        intQtde = 0

        If InStr(strFones, ",") > 0 Then
            arrFones = Split(Request.QueryString("Fones"), ",")
            intQtde = UBound(arrFones)
        End If

        blnOK = False

        strHTML = ""

        For x = 0 To intQtde

            strMsg = Trim(Request.QueryString("Msg"))
            strMsg = Replace(strMsg, "$NOMECLI$", strNome)
            strMsg = Replace(strMsg, "$TELREC$", "11-2171-0300")

            If intQtde > 0 Then
                strFone = Trim(arrFones(x))
            Else
                strFone = strFones
            End If

            strSql = "prc_SEL_VerificaEmissaoSMS '" & strFone & "', 2"
            objCELVerify = Mirror.ExecuteQuery(strSql)

            If Not objCELVerify.Read Then

                blnOK = True

                If y = 0 Then strHTML = strHTML & "<table><tr><td align=""center"" bgcolor=""#ffffff"" style=""font-size: 8pt; color: #1f3662; font-family: Verdana, Arial width: 100%; height: 100%;"">OK! - SMS Enviado com sucesso.</td></tr></table>"

                strSql = "prc_INS_FoneEmissao 1,3,1,1," & strStatus & ",'" & Left(strFone, 2) & "','" & Mid(strFone, 3, 8) & "'," & intCTRA_ID & "," & intUserID & ",'" & strCarteira & "','" & strContratante & "'"
                'TIPO_EMISSAO,TIPO_FONE,ORIG_EMISSAO,LAYOUT,STATUS,DDD,FONE
                Mirror.Execute(strSql)

                intId = intId + 1
                strURL = "http://200.190.61.201:50127/br/hargos_online?user=hargos_online&pwd=hargos123sms&phone=" & strFone & "&msgtext=" & Trim(strMsg) & "&msgid=" & intId
                strHTML = strHTML & "<div id=" & intId & "><iFrame id=" & intId & " name=" & intId & " src=""" & strURL & """ frameborder=""0"" marginwidth=""0"" marginheight=""0"" scrolling=""no"" style=""width: 240px; height: 20px""></iFrame></div>"

                y = y + 1

            Else

                strHTML = strHTML & "<div id=" & intId & "><table><tr><td align=""center"" bgcolor=""#ffffff"" style=""font-size: 8pt; color: #1f3662; font-family: Verdana, Arial width: 100%; height: 100%;""> Já foi enviado um SMS para este celular nos últimos dias.</td></tr></table></div>"

            End If

        Next x

        If blnOK = True Then ShowMenssage("OK! - " & y & " SMS(s) enviado(s) com sucesso.")

        Response.Write(strHTML)

    End Sub

    Public Sub ShowMenssage(ByVal strMsg As String)

        Dim strScript As String = "<script language=JavaScript>alert('" & strMsg & "');</script>"

        If (Not Page.ClientScript.IsStartupScriptRegistered("clientScript")) Then

            Page.ClientScript.RegisterStartupScript(Me.GetType, "clientScript", strScript)

        End If

    End Sub

End Class
