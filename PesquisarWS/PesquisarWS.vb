Option Explicit On
Option Strict Off

'Importa Referências
Imports System
Imports System.Xml
Imports System.Web.Services.Protocols
Imports ConnectTo
Imports System.Data.SqlClient
Imports HTTP
Imports XMail

Public Class Pesquisa

    Private WSaFinder As New aFinder.WebServices
    Private WSinfo As New InforMarketing.WEBServiceScore
    Private objAuthentication As New aFinder.AuthHeader
    Private Mail As New XMail.SendMail

    Public Function Localizar(Optional ByVal strTipoRetorno As String = "HTML") As String

        Dim ResultLocal As String = ""
        Dim ResultAFinder As String = ""
        Dim ResultInfo As String = ""
        Dim ResultCredi As String = ""
        Dim Mirror As New Comando
        Dim strSql As String = ""
        Dim intStatus As Integer = 0
        Dim objDR As SqlDataReader = Nothing

        Try

            Localizar = ""

            If Trim(strTelefone) <> "" Then
                If Len(Trim(strTelefone)) > 8 Then strTelefone = Left(strTelefone, 8)
                If Trim(strDDD) <> "" Then If Len(Trim(strDDD)) <> 2 Or IsNumeric(Trim(strDDD)) = False Then strDDD = ""
            End If

            Mirror.Banco = "MIRRORWEB"

            If blnProximidades = True Then
                If blnAFinder = True Then
                    ResultAFinder = LocalizarFinder()
                    If UpperTrim(ResultAFinder) <> "ERR" Then
                        If Trim(ResultAFinder) = "<ROOT xmlns=""""></ROOT>" Or Trim(ResultAFinder) = "" Then
                            ResultAFinder = ""
                        Else
                            'CarregarLocal(ResultAFinder, 1)
                        End If
                        Localizar = ResultAFinder
                        If Trim(Localizar) <> "" Then
                            intStatus = 1
                        Else
                            intStatus = 0
                        End If
                        strSql = "prc_INS_PerformancePesquisa " & intStatus & ",1,'WaterFall'"
                        Mirror.Execute(strSql)
                    End If
                End If
                If UpperTrim(Localizar) = "ERR" Then Localizar = ""
                If blnCredi = True And Trim(Localizar) = "" Then
                    strCPF = ""
                    strDDD = ""
                    strTelefone = ""
                    strNome = ""
                    strEndereco = ""
                    strNumero = ""
                    strComplemento = ""
                    strBairro = ""
                    strCidade = ""
                    ResultCredi = LocalizarCredi()
                    If UpperTrim(ResultCredi) <> "ERR" Then
                        If Trim(ResultCredi) <> "" Then Localizar = FormatXMLCredi(ResultCredi)
                        If Trim(Localizar) <> "" Then
                            'CarregarLocal(Localizar, 4)
                            intStatus = 1
                        Else
                            intStatus = 0
                        End If
                        strSql = "prc_INS_PerformancePesquisa " & intStatus & ",4,'WaterFall'"
                        Mirror.Execute(strSql)
                    End If
                End If
                If UpperTrim(Localizar) = "ERR" Then Localizar = ""
                If blnLocal = True And Trim(Localizar) = "" Then
                    strCPF = ""
                    strDDD = ""
                    strTelefone = ""
                    strNome = ""
                    strEndereco = ""
                    strNumero = ""
                    strComplemento = ""
                    strBairro = ""
                    strCidade = ""
                    ResultLocal = LocalizarLocal(0)
                    Localizar = ResultLocal
                    If UpperTrim(ResultLocal) <> "ERR" Then
                        If Trim(Localizar) <> "" Then
                            intStatus = 1
                        Else
                            intStatus = 0
                        End If
                        strSql = "prc_INS_PerformancePesquisa " & intStatus & ",3,'WaterFall'"
                        Mirror.Execute(strSql)
                    End If
                End If
            Else
                If blnLocal = True Then
                    ResultLocal = LocalizarLocal(0)
                    Localizar = ResultLocal
                    If UpperTrim(ResultLocal) <> "ERR" Then
                        If Trim(Localizar) <> "" Then
                            intStatus = 1
                        Else
                            intStatus = 0
                        End If
                        strSql = "prc_INS_PerformancePesquisa " & intStatus & ",3,'WaterFall'"
                        Mirror.Execute(strSql)
                    End If
                End If
                If UpperTrim(Localizar) = "ERR" Then Localizar = ""
                If blnCredi = True And Trim(Localizar) = "" Then
                    ResultCredi = LocalizarCredi()
                    If UpperTrim(ResultCredi) <> "ERR" Then
                        If Trim(ResultCredi) <> "" Then Localizar = FormatXMLCredi(ResultCredi)
                        If Trim(Localizar) <> "" Then
                            'CarregarLocal(Localizar, 4)
                            If Trim(strCEP) <> "" Then CarregarLocal(Localizar, 4)
                            intStatus = 1
                        Else
                            intStatus = 0
                        End If
                        strSql = "prc_INS_PerformancePesquisa " & intStatus & ",4,'WaterFall'"
                        Mirror.Execute(strSql)
                    End If
                End If
                If UpperTrim(Localizar) = "ERR" Then Localizar = ""
                If blnAFinder = True And Trim(Localizar) = "" Then
                    ResultAFinder = LocalizarFinder()
                    If UpperTrim(ResultAFinder) <> "ERR" Then
                        If Trim(ResultAFinder) = "<ROOT xmlns=""""></ROOT>" Or Trim(ResultAFinder) = "" Then
                            ResultAFinder = ""
                        Else
                            'CarregarLocal(ResultAFinder, 1)
                            If Trim(strCEP) <> "" Then CarregarLocal(ResultAFinder, 1)
                        End If
                        Localizar = ResultAFinder
                        If Trim(Localizar) <> "" Then
                            intStatus = 1
                        Else
                            intStatus = 0
                        End If
                        strSql = "prc_INS_PerformancePesquisa " & intStatus & ",1,'WaterFall'"
                        Mirror.Execute(strSql)
                    End If
                End If
                If UpperTrim(Localizar) = "ERR" Then Localizar = ""
                If blnInfo = True And Trim(Localizar) = "" And Trim(strCPF) <> "" Then
                    'strSql = "prc_SEL_Consultas 2,1"
                    'objDR = Mirror.ExecuteQuery(strSql)
                    'objDR.Read()
                    'If objDR.HasRows Or UpperTrim(strUsuario) = "MASTER" Then
                    '    If CLng(objDR("Qtde").ToString) < 3000 Or UpperTrim(strUsuario) = "MASTER" Then
                    '        ResultInfo = LocalizarInfo()
                    '        If UpperTrim(ResultInfo) <> "ERR" Then
                    '            If Trim(ResultInfo) <> "" Then Localizar = FormatXMLInfo(ResultInfo)
                    '            If Trim(Localizar) <> "" Then
                    '                CarregarLocal(Localizar, 2)
                    '                intStatus = 1
                    '            Else
                    '                intStatus = 0
                    '            End If
                    '            strSql = "prc_INS_PerformancePesquisa " & intStatus & ",2,'WaterFall'"
                    '            Mirror.Execute(strSql)
                    '        End If
                    '    End If
                    'End If
                    ResultInfo = LocalizarInfo()
                    If UpperTrim(ResultInfo) <> "ERR" Then
                        If Trim(ResultInfo) <> "" Then Localizar = FormatXMLInfo(ResultInfo)
                        If Trim(Localizar) <> "" Then
                            'CarregarLocal(Localizar, 2)
                            CarregarLocal(Localizar, 2)
                            intStatus = 1
                        Else
                            intStatus = 0
                        End If
                        strSql = "prc_INS_PerformancePesquisa " & intStatus & ",2,'WaterFall'"
                        Mirror.Execute(strSql)
                    End If
                End If
            End If
            If UpperTrim(Localizar) = "ERR" Then Localizar = ""
            If Trim(Localizar) <> "" Then
                If blnSerasa = True Then
                    ResultLocal = LocalizarLocal(2)
                    If Trim(ResultLocal) <> "" Then Localizar = Replace(Localizar, "</ROOT>", "") & ResultLocal
                End If
            Else
                If Trim(Localizar) = "" Then
                    If blnSerasa = True Then
                        ResultLocal = LocalizarLocal(1)
                        If UpperTrim(ResultLocal) <> "ERR" Then
                            Localizar = ResultLocal
                            If Trim(Localizar) <> "" Then
                                intStatus = 1
                                strSql = "prc_INS_PerformancePesquisa " & intStatus & ",3,'WaterFall'"
                                Mirror.Execute(strSql)
                            End If
                        End If
                    End If
                End If
            End If

            If UpperTrim(Localizar) = "ERR" Then Localizar = ""
            If Trim(Localizar) <> "" And UpperTrim(strTipoRetorno) = "HTML" Then Localizar = ReturnHTML(Localizar, "http://DBMIRROR/Intranet/result.xsl")

            intStatus = 0
            If Trim(Localizar) <> "" Then intStatus = 1
            strSql = "prc_INS_PerformancePesquisa " & intStatus & ",88,'" & strUsuario & "'"
            If UpperTrim(Localizar) <> "ERR" Then Mirror.Execute(strSql)
            If UpperTrim(Localizar) = "ERR" Then Localizar = ""

        Catch ex As Exception

            Localizar = ""
            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Pesquisa X"
                .Subject = "Erro na Pesquisa X"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = "Número: " & Err.Number & Chr(13) & "Source: " & Err.Source & Chr(13) & "Descrição: " & Err.Description & Chr(13) & "Help: " & Chr(13) & Err.HelpFile & Chr(13) & Err.HelpContext & Chr(13) & Err.LastDllError & Chr(13)
            End With

            Mail.Send()

        End Try

    End Function

    Private Function LocalizarLocal(Optional ByVal intTipo As Integer = 1) As String

        Dim Mirror As New Comando
        Dim xmlReturn As XmlReader
        Dim strSql As String = ""
        Dim strUFLocal As String = ""

        Try

            Mirror.Banco = "MIRRORWEB"
            strUFLocal = strUF
            If blnBuscaAut = True And blnProximidades = False Then strUFLocal = ""

            Select Case intTipo
                Case 0
                    strSql = "prc_SEL_Pesquisa "

                Case 1
                    strSql = "prc_SEL_PesquisaALL "

                Case 2
                    strSql = "prc_SEL_PesquisaSERASA "

            End Select

            strSql = strSql & "'" & intRegistros & "','" & strUFLocal & "'," & _
                                          "'" & strCPF & "'," & _
                                          "'" & strDDD & "'," & _
                                          "'" & strTelefone & "'," & _
                                          "'" & strNome & "'," & "''," & _
                                          "'" & strEndereco & "'," & _
                                          "'" & strNumero & "'," & _
                                          "'" & strComplemento & "'," & _
                                          "'" & strBairro & "'," & _
                                          "'" & strCidade & "'," & _
                                          "'" & strCEP & "'"
            xmlReturn = Mirror.ExecuteXMLQuery(strSql)

            If Not xmlReturn.Read Then
                xmlReturn = Nothing
                LocalizarLocal = ""
                Exit Function
            End If

            LocalizarLocal = ""

            If intTipo <> 2 Then LocalizarLocal = "<ROOT xmlns="""">"
            Do While xmlReturn.ReadState <> Xml.ReadState.EndOfFile
                LocalizarLocal = LocalizarLocal & UpperTrim(xmlReturn.ReadOuterXml)
            Loop
            LocalizarLocal = LocalizarLocal & "</ROOT>"

        Catch ex As Exception

            LocalizarLocal = "err"
            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Pesquisa X"
                .Subject = "Erro na Pesquisa X - Local"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = "Número: " & Err.Number & Chr(13) & "Source: " & Err.Source & Chr(13) & "Descrição: " & Err.Description & Chr(13) & "Help: " & Chr(13) & Err.HelpFile & Chr(13) & Err.HelpContext & Chr(13) & Err.LastDllError & Chr(13) & "Query: " & strSql
            End With

            Mail.Send()

        End Try

        xmlReturn = Nothing

    End Function

    Private Function LocalizarFinder() As String

        Dim intSentido As Integer
        Dim intErro As Integer = 0

        Try

            If objAuthentication.Username = Nothing Then
                WSaFinder.Credentials = System.Net.CredentialCache.DefaultCredentials
                objAuthentication.Username = strUserFinder
                objAuthentication.Password = strPassFinder
                WSaFinder.AuthHeaderValue = objAuthentication
                If intTimeOut <> 0 Then WSaFinder.Timeout = intTimeOut
            End If
            'Buscar:
            If blnBuscaAut = True Then
                LocalizarFinder = WSaFinder.BuscaAutXml(strUF, _
                                                        strCidade, _
                                                        strBairro, _
                                                        strEndereco, _
                                                        strNumero, _
                                                        strComplemento, _
                                                        strCEP, _
                                                        strNome, _
                                                        strCPF, _
                                                        strDDD, _
                                                        strTelefone, _
                                                        intRegistros, _
                                                        blnHigienizar, _
                                                        strBases).OuterXml

            ElseIf blnBuscaCPF = True Then

                LocalizarFinder = WSaFinder.BuscaCPFXml("", _
                                                        strCPF, _
                                                        intRegistros, _
                                                        strBases).OuterXml

            ElseIf blnProximidades = True Then

                intSentido = 2
                If Trim(strComplemento) <> "" Then intSentido = 3

                LocalizarFinder = WSaFinder.BuscaProximidadesXml(strUF, _
                                                                 strCidade, _
                                                                 strBairro, _
                                                                 strEndereco, _
                                                                 strNumero, _
                                                                 strComplemento, _
                                                                 strCEP, _
                                                                 intRegistros, _
                                                                 intSentido).OuterXml

            Else

                LocalizarFinder = WSaFinder.BuscaXml(strUF, _
                                                     strCidade, _
                                                     strBairro, _
                                                     strEndereco, _
                                                     strNumero, _
                                                     strComplemento, _
                                                     strCEP, _
                                                     strNome, _
                                                     strCPF, _
                                                     strDDD, _
                                                     strTelefone, _
                                                     intRegistros, _
                                                     strBases).OuterXml

            End If

        Catch ex As Exception

            '    'intErro = intErro + 1
            '    'If intErro < 2 And InStr(LCase(Err.Description), "tempo limite") = 0 And InStr(LCase(Err.Description), "timeout") = 0 And InStr(LCase(Err.Description), "timed out") = 0 Then
            '    '    GoTo Buscar
            '    'End If

            '    'LocalizarFinder = "Retorno do Servidor @finder: " & Err.Description
            LocalizarFinder = "err"
            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Pesquisa X"
                .Subject = "Erro na Pesquisa X - @finder"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = "Número: " & Err.Number & Chr(13) & "Source: " & Err.Source & Chr(13) & "Descrição: " & Err.Description & Chr(13) & "Help: " & Chr(13) & Err.HelpFile & Chr(13) & Err.HelpContext & Chr(13) & Err.LastDllError & Chr(13)
            End With

            Mail.Send()

        End Try

        If InStr(UCase(LocalizarFinder), "<ERROR>") > 0 Then
            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Pesquisa X"
                .Subject = "Erro na Pesquisa X - @finder"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = LocalizarFinder
            End With

            Mail.Send()

            LocalizarFinder = "err"
        End If

        WSaFinder.Dispose()
        WSaFinder = Nothing

    End Function

    Private Function LocalizarInfo() As String

        Try

            If Trim(strAuthInfo) = "" Then strAuthInfo = WSinfo.getLogin("credicard1", "caneta", "hargos")
            'If intTimeOut <> 0 Then WSinfo.Timeout = intTimeOut
            LocalizarInfo = WSinfo.getLista_Dados_String_XML("credicard1", strAuthInfo, strCPF)

        Catch ex As Exception

            LocalizarInfo = "err"
            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Pesquisa X"
                .Subject = "Erro na Pesquisa X - Informarketing"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = "Número: " & Err.Number & Chr(13) & "Source: " & Err.Source & Chr(13) & "Descrição: " & Err.Description & Chr(13) & "Help: " & Chr(13) & Err.HelpFile & Chr(13) & Err.HelpContext & Chr(13) & Err.LastDllError & Chr(13)
            End With

            Mail.Send()

        End Try

        WSinfo.Dispose()
        WSinfo = Nothing

    End Function

    Private Function LocalizarCredi() As String

        Dim strURL As String = ""

        Try

            'CC > Variável  CPF/CNPJ
            'TE >Variável telefone
            'NO > Variável nome
            'LO > Variável logradouro
            'CE > Variável CEP
            'NU > Variável numero
            'CO > Variável complemento
            'BA > Variável bairro
            'CI > Variável cidade
            'UF > Variável UF


            'OP = 1 : CPF/CNPJ (*CC, UF)
            'OP = 2 : TELEFONE (*TE)
            'OP = 3 : CEP (*CE, NU, CO, NO, CC)
            'OP = 4 : ENDERECO (*LO, NU, CO, BA, CI, UF, NO, CC)
            'OP = 5 : NOME (*NO, *UF, BA, CI)


            If Trim(strCPF) <> "" Then

                strURL = "US=" & strUserCredi & _
                         "&PS=" & strPassCredi & _
                         "&SG=HARGO&" & _
                         "OP=1&CC=" & strCPF

            ElseIf Trim(strTelefone) <> "" Then

                If Trim(strDDD) = "" Then strDDD = "11"

                strURL = "US=" & strUserCredi & _
                         "&PS=" & strPassCredi & _
                         "&SG=HARGO&" & _
                         "OP=2&TE=" & strDDD & strTelefone

            ElseIf Trim(strNome) <> "" Then

                strURL = "US=" & strUserCredi & _
                         "&PS=" & strPassCredi & _
                         "&SG=HARGO&" & _
                         "OP=5&NO=" & strNome & _
                         "&UF=" & strUF
                If Trim(strBairro) <> "" Then strURL = strURL & "&BA=" & strBairro
                If Trim(strCidade) <> "" Then strURL = strURL & "&CI=" & strCidade

            ElseIf Trim(strEndereco) <> "" Then

                strURL = "US=" & strUserCredi & _
                         "&PS=" & strPassCredi & _
                         "&SG=HARGO&" & _
                         "OP=4&LO=" & strEndereco
                If Trim(strNumero) <> "" Then strURL = strURL & "&NU=" & strNumero
                If Trim(strComplemento) <> "" Then strURL = strURL & "&CO=" & strComplemento
                If Trim(strBairro) <> "" Then strURL = strURL & "&BA=" & strBairro
                If Trim(strCidade) <> "" Then strURL = strURL & "&CI=" & strCidade
                If Trim(strUF) <> "" Then strURL = strURL & "&UF=" & strUF
                If Trim(strNome) <> "" Then strURL = strURL & "&NO=" & strNome
                If Trim(strCPF) <> "" Then strURL = strURL & "&CC=" & strCPF

            ElseIf Trim(strCEP) <> "" Then

                strURL = "US=" & strUserCredi & _
                         "&PS=" & strPassCredi & _
                         "&SG=HARGO&" & _
                         "OP=3&CE=" & strCEP
                If Trim(strNumero) <> "" Then strURL = strURL & "&NU=" & strNumero
                If Trim(strComplemento) <> "" Then strURL = strURL & "&CO=" & strComplemento
                If Trim(strNome) <> "" Then strURL = strURL & "&NO=" & strNome
                If Trim(strCPF) <> "" Then strURL = strURL & "&CC=" & strCPF

            End If

            LocalizarCredi = Method.Send("http://www.credilink.com.br/Integracao/index.jsp", _
                             strURL, _
                             Method.HTTPMethod.HTTP_POST)

        Catch ex As Exception

            LocalizarCredi = "err"
            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Pesquisa X"
                .Subject = "Erro na Pesquisa X - Credilink"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = "URL: " & "http://www.credilink.com.br/Integracao/index.jsp?" & strURL & Chr(13) & "Número: " & Err.Number & Chr(13) & "Source: " & Err.Source & Chr(13) & "Descrição: " & Err.Description & Chr(13) & "Help: " & Chr(13) & Err.HelpFile & Chr(13) & Err.HelpContext & Chr(13) & Err.LastDllError & Chr(13)
            End With

            Mail.Send()

        End Try

        If InStr(UCase(UpperTrim(LocalizarCredi)), "NAO LOCALIZADO") > 0 Then LocalizarCredi = ""
        If InStr(UCase(UpperTrim(LocalizarCredi)), "INEXISTENTE") > 0 Then LocalizarCredi = ""
        If InStr(UCase(UpperTrim(LocalizarCredi)), "INVALIDO") > 0 Then LocalizarCredi = ""
        If InStr(UCase(UpperTrim(LocalizarCredi)), "USUARIO DESATIVADO") > 0 Then LocalizarCredi = ""

    End Function

    Public Sub New()

        strUserFinder = ""
        strPassFinder = ""
        strAuthInfo = ""
        strUF = ""
        strCidade = ""
        strBairro = ""
        strEndereco = ""
        strNumero = ""
        strComplemento = ""
        strCEP = ""
        strNome = ""
        strCPF = ""
        strDDD = ""
        strTelefone = ""
        intRegistros = 0
        strBases = ""
        blnHigienizar = False
        blnBuscaAut = False
        blnProximidades = False
        blnAFinder = False
        blnLocal = False
        blnInfo = False

    End Sub

    'Propriedades
    Public Property BuscarAFinder() As Boolean
        Get
            Return blnAFinder
        End Get
        Set(ByVal Value As Boolean)
            If blnAFinder <> Value Then blnAFinder = Value
        End Set
    End Property

    Public Property BuscarLocal() As Boolean
        Get
            Return blnLocal
        End Get
        Set(ByVal Value As Boolean)
            If blnLocal <> Value Then blnLocal = Value
        End Set
    End Property

    Public Property BuscarSerasa() As Boolean
        Get
            Return blnSerasa
        End Get
        Set(ByVal Value As Boolean)
            If blnSerasa <> Value Then blnSerasa = Value
        End Set
    End Property

    Public Property BuscarInfo() As Boolean
        Get
            Return (blnInfo)
        End Get
        Set(ByVal Value As Boolean)
            If blnInfo <> Value Then blnInfo = Value
        End Set
    End Property

    Public Property BuscarCredi() As Boolean
        Get
            Return (blnCredi)
        End Get
        Set(ByVal Value As Boolean)
            If blnCredi <> Value Then blnCredi = Value
        End Set
    End Property

    Public Property UserFinder() As String
        Get
            Return strUserFinder
        End Get
        Set(ByVal Value As String)
            If strUserFinder <> Value Then strUserFinder = Value
        End Set
    End Property

    Public Property PassFinder() As String
        Get
            Return strPassFinder
        End Get
        Set(ByVal Value As String)
            If strPassFinder <> Value Then strPassFinder = Value
        End Set
    End Property

    Public Property TimeOut() As Integer
        Get
            Return intTimeOut
        End Get
        Set(ByVal Value As Integer)
            If intTimeOut <> Value Then intTimeOut = Value
        End Set
    End Property

    Public Property UserCredi() As String
        Get
            Return strUserCredi
        End Get
        Set(ByVal Value As String)
            If strUserCredi <> Value Then strUserCredi = Value
        End Set
    End Property

    Public Property PassCredi() As String
        Get
            Return strPassCredi
        End Get
        Set(ByVal Value As String)
            If strPassCredi <> Value Then strPassCredi = Value
        End Set
    End Property

    Public Property AuthInfo() As String
        Get
            Return strAuthInfo
        End Get
        Set(ByVal Value As String)
            If strAuthInfo <> Value Then strAuthInfo = Value
        End Set
    End Property

    Public Property BuscaAut() As Boolean
        Get
            Return blnBuscaAut
        End Get
        Set(ByVal Value As Boolean)
            If blnBuscaAut <> Value Then blnBuscaAut = Value
        End Set
    End Property

    Public Property BuscaCPF() As Boolean
        Get
            Return blnBuscaCPF
        End Get
        Set(ByVal Value As Boolean)
            If blnBuscaCPF <> Value Then blnBuscaCPF = Value
        End Set
    End Property

    Public Property Proximidades() As Boolean
        Get
            Return blnProximidades
        End Get
        Set(ByVal Value As Boolean)
            If blnProximidades <> Value Then blnProximidades = Value
        End Set
    End Property

    Public Property UF() As String
        Get
            Return strUF
        End Get
        Set(ByVal Value As String)
            If strUF <> Value Then strUF = UpperTrim(Value)
        End Set
    End Property

    Public Property Cidade() As String
        Get
            Return strCidade
        End Get
        Set(ByVal Value As String)
            If strCidade <> Value Then strCidade = UpperTrim(Value)
        End Set
    End Property

    Public Property Bairro() As String
        Get
            Return strBairro
        End Get
        Set(ByVal Value As String)
            If strBairro <> Value Then strBairro = UpperTrim(Value)
        End Set
    End Property

    Public Property Endereco() As String
        Get
            Return strEndereco
        End Get
        Set(ByVal Value As String)
            If strEndereco <> Value Then strEndereco = UpperTrim(Value)
        End Set
    End Property

    Public Property Numero() As String
        Get
            Return strNumero
        End Get
        Set(ByVal Value As String)
            If strNumero <> Value Then strNumero = UpperTrim(Value)
        End Set
    End Property

    Public Property Complemento() As String
        Get
            Return strComplemento
        End Get
        Set(ByVal Value As String)
            If strComplemento <> Value Then strComplemento = UpperTrim(Value)
        End Set
    End Property

    Public Property CEP() As String
        Get
            Return strCEP
        End Get
        Set(ByVal Value As String)
            If strCEP <> Value Then strCEP = UpperTrim(SoNumeros(Value))
        End Set
    End Property

    Public Property Nome() As String
        Get
            Return strNome
        End Get
        Set(ByVal Value As String)
            If strNome <> Value Then strNome = UpperTrim(Value)
        End Set
    End Property

    Public Property CPF() As String
        Get
            Return strCPF
        End Get
        Set(ByVal Value As String)
            If strCPF <> Value Then strCPF = UpperTrim(SoNumeros(Value))
        End Set
    End Property

    Public Property DDD() As String
        Get
            Return strDDD
        End Get
        Set(ByVal Value As String)
            If strDDD <> Value Then strDDD = UpperTrim(SoNumeros(Value))
        End Set
    End Property

    Public Property Telefone() As String
        Get
            Return strTelefone
        End Get
        Set(ByVal Value As String)
            If strTelefone <> Value Then strTelefone = UpperTrim(SoNumeros(Value))
        End Set
    End Property

    Public Property Registros() As String
        Get
            Return intRegistros
        End Get
        Set(ByVal Value As String)
            If intRegistros <> Value Then intRegistros = Value
        End Set
    End Property

    Public Property Higienizar() As String
        Get
            Return blnHigienizar
        End Get
        Set(ByVal Value As String)
            If blnHigienizar <> Value Then blnHigienizar = Value
        End Set
    End Property

    Public Property Bases() As String
        Get
            Return strBases
        End Get
        Set(ByVal Value As String)
            If strBases <> Value Then strBases = UpperTrim(Value)
        End Set
    End Property

    Public Property Usuario() As String
        Get
            Return strUsuario
        End Get
        Set(ByVal Value As String)
            If strUsuario <> Value Then strUsuario = UpperTrim(Value)
        End Set
    End Property

End Class