Option Explicit On
Option Strict Off

'Imports aFinder
Imports ConnectTo
Imports System
Imports System.Xml
Imports System.Data.SqlClient

Public Class WebService

    Public Function Enviar(ByVal strContrato As String, ByVal intLayout As Short, ByVal intUserID As Integer) As String

        Dim Conn As New ConnectTo.Comando
        Dim strSql As String
        Dim objDR As SqlDataReader = Nothing
        Dim objDEVE As SqlDataReader = Nothing
        Dim objCEL As SqlDataReader = Nothing
        Dim objCELId As SqlDataReader = Nothing
        Dim objCELLayout As SqlDataReader = Nothing
        Dim strURL As String = ""
        Dim strMsg As String = ""
        Dim strFone As String = ""
        Dim strFones As String = ""
        Dim strLayout As String = ""
        Dim x As Short = 0
        Dim intStatus As Short = 2

        Try

            If Trim(strContrato) = "" Then
                Enviar = ""
                Exit Function
            End If

            Conn.Banco = "NEOWEB"
            strSql = "Select TOP 1 A.DEVE_CGCCPF, A.DEVE_ID, A.DEVE_Nome, B.CTRA_ID,C.CART_Descricao as Carteira,D.CONT_Fantasia as Contratante, " & _
                     "(Select TOP 1 K.ESTA_ID FROM Endereco_Devedor K WITH (NOLOCK) Where K.DEVE_ID = A.DEVE_ID AND K.TEND_ID = 1) as UF From " & _
                     "Devedores A WITH(NOLOCK) Join " & _
                     "Contratos B WITH(NOLOCK) On " & _
                     "B.DEVE_ID = A.DEVE_ID Join " & _
                     "Carteiras C WITH(NOLOCK) On " & _
                     "C.CART_ID = B.CART_ID Join " & _
                     "Contratante D WITH(NOLOCK) On " & _
                     "D.CONT_ID = C.CONT_ID Where " & _
                     "B.CTRA_Numero = '" & SoNumeros(strContrato) & "'"
            objDEVE = Conn.ExecuteQuery(strSql)

            If Not objDEVE.Read Then
                Enviar = ""
                Exit Function
            End If

            Conn.Banco = "MIRRORWEB"

            strSql = "Select TOP 1 LAYO_Descricao From tb_FoneSMSLayout " & _
                 "Where LAYO_ID = " & intLayout
            objDR = Conn.ExecuteQuery(strSql)

            If Not objDR.Read Then

                Enviar = ""
                Exit Function

            End If

            strLayout = Trim(objDR("LAYO_Descricao").ToString)

            Conn.Banco = "NEOWEB"

            strSql = "Select TDEV_Telefone From " & _
                     "Telefones_do_Devedor Where " & _
                     "DEVE_ID = " & objDEVE("DEVE_ID").ToString & _
                     " AND (TTEL_ID = 3 OR Substring(TDEV_Telefone,5,1) In('7','8','9')) " & _
                     "AND STEL_ID Not In(4,6)"
            objCEL = Conn.ExecuteQuery(strSql)

            strFones = ""

            Do While objCEL.Read

                strFone = SoNumeros(objCEL("TDEV_Telefone").ToString)

                If Len(strFone) <> 10 Then
                    strFone = ""
                Else
                    If x > 0 Then strFones = strFones & ","
                    strFones = strFones & strFone
                End If

                x = x + 1

            Loop

            Conn.Banco = "MIRRORWEB"

            If Trim(strFones) = "" Then
                intStatus = 1
                strFones = PesquisarFone(Trim(objDEVE("DEVE_CGCCPF").ToString), Trim(objDEVE("UF").ToString), "Movel")
            Else
                intStatus = 2
            End If

            If Trim(strFones) = "" Then
                Enviar = ""
                Exit Function
            End If

            'MANDAR SMS
            strSql = "Select TOP 1 Max(EMIS_ID) From tb_FoneEmissao"
            objCELId = Conn.ExecuteQuery(strSql)
            objCELId.Read()

            strURL = "http://NECTAR/WSMS/inSendSMS.aspx?Id=" & CInt(objCELId(0).ToString) & "&Nome=" & UCase(Trim(Left(objDEVE("DEVE_NOME").ToString, InStr(objDEVE("DEVE_NOME").ToString, " ")))) & "&Fones=" & strFones & "&Status=" & intStatus & "&Msg=" & Trim(strLayout) & "&CTRA_ID=" & Trim(objDEVE("CTRA_ID").ToString) & "&UserID=" & intUserID & "&carteira=" & UpperTrim(objDEVE("Carteira").ToString) & "&contratante=" & UpperTrim(objDEVE("Contratante").ToString)

            Enviar = Trim(strURL)

        Catch ex As Exception

            Enviar = ""
            Exit Function

        End Try

    End Function

    Private Function PesquisarFone(ByVal strCPF As String, ByVal strUF As String, ByVal strTipo As String) As String

        Dim strFones As String = ""
        Dim xmlFinder As New XmlDocument
        Dim WSPesquisa As New PesquisarWS.Pesquisa
        Dim z As Short = 0

        Try

            With WSPesquisa
                '.BuscarCredi = True
                '.UserCredi = "HARGOS"
                '.PassCredi = "HARGOS"
                .BuscarAFinder = True
                .UserFinder = "credicard1"
                .PassFinder = "caneta"
                '.BuscarLocal = True
                '.BuscarInfo = True
                .BuscaCPF = True
                .CPF = strCPF
                .Registros = 5
                .Bases = strTipo
            End With

            xmlFinder.LoadXml(WSPesquisa.Localizar("XML"))

            If Trim(Replace(xmlFinder.InnerXml.ToString, """", "''")) = "<ROOT xmlns=''''></ROOT>" Or Trim(xmlFinder.InnerXml.ToString) = "" Then
                PesquisarFone = ""
                Exit Function
            End If

            strFones = ""

            For z = 0 To xmlFinder.DocumentElement.ChildNodes.Count - 1
                If InStr(Left(Trim(xmlFinder.DocumentElement.ChildNodes(z).Attributes.GetNamedItem("FONE").Value.ToString), 1), "789") > 0 Then

                    If z > 0 Then strFones = strFones & ","

                    strFones = strFones & Trim(xmlFinder.DocumentElement.ChildNodes(z).Attributes.GetNamedItem("DDD").Value.ToString) & Trim(xmlFinder.DocumentElement.ChildNodes(z).Attributes.GetNamedItem("FONE").Value.ToString)
                End If
            Next

            PesquisarFone = Trim(strFones)

        Catch ex As Exception

            PesquisarFone = ""

        End Try

    End Function

End Class
