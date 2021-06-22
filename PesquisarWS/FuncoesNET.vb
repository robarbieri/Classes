Imports System.IO
Imports System.Xml
Imports System.Xml.Xsl
Imports System.Text
Imports XMail

Module FuncoesNET

    Public Mail As New XMail.SendMail

    Public Function UpperTrim(ByVal strText As String) As String

        UpperTrim = Trim(UCase(strText))

    End Function

    Public Function SoNumeros(ByVal strDado As String) As String

        Dim x As Short = 0

        For x = 1 To Len(UpperTrim(strDado))
            If IsNumeric(Mid(UpperTrim(strDado), x, 1)) = False Then
                strDado = Trim(Replace(UpperTrim(strDado), Mid(UpperTrim(strDado), x, 1), ""))
            End If
        Next x

        SoNumeros = strDado

    End Function

    Public Function ReturnHTML(ByVal strXML As String, ByVal strPathXSL As String) As String

        Dim objXML As New XmlDocument
        Dim objXSL As New XslTransform
        Dim sb As New StringBuilder
        Dim sw As New StringWriter(sb)

        Try

            objXML.LoadXml(strXML)
            objXSL.Load(strPathXSL)

            objXSL.Transform(objXML, Nothing, sw, Nothing)

            ReturnHTML = sb.ToString

        Catch ex As Exception

            ReturnHTML = ""

            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Funções .NET - ReturnHTML"
                .Subject = "Erro na rotina ReturnHTML"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = "Número: " & Err.Number & Chr(13) & "Source: " & Err.Source & Chr(13) & "Descrição: " & Err.Description & Chr(13) & "Help: " & Chr(13) & Err.HelpFile & Chr(13) & Err.HelpContext & Chr(13) & Err.LastDllError & Chr(13)
            End With

            Mail.Send()

        End Try

    End Function

    Public Function FormatXMLInfo(ByVal strXML As String) As String

        Dim strFones As String = ""
        Dim strEndereco As String = ""
        Dim arrEndereco() As String
        Dim strCPF As String = ""
        Dim strTipoEnd As String = ""
        Dim strEnd As String = ""
        Dim strNum As String = ""
        Dim strCompl As String = ""
        Dim strCEP As String = ""
        Dim strBairro As String = ""
        Dim strCidade As String = ""
        Dim strUF As String = ""
        Dim strNome As String = ""
        Dim arrDDD(50) As String
        Dim arrFones() As String
        Dim xmlDoc As New XmlDocument
        Dim x As Short = 0
        Dim y As Short = 0

        Try

            xmlDoc.LoadXml(strXML)

            y = xmlDoc.ChildNodes(0).ChildNodes.Count - 1

            For x = 0 To y

                'Select Case UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                '    Case "CPF"
                '        strCPF = Mid("00000000000000" & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml), Len("00000000000000" & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml)) - 13)

                '    Case "NOME"
                '        strNome = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml)

                '    Case "ENDERECO RESIDENCIAL - A"
                '        strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml)

                '    Case "ENDERECO RESIDENCIAL - V"
                '        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml)

                '    Case "ENDERECO RESIDENCIAL - G"
                '        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml)

                '    Case "ENDERECO COMERCIAL - R"
                '        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml)

                '    Case "ENDERECO COMERCIAL - R1"
                '        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml)

                '    Case "TELEFONE RESIDENCIAL - A"
                '        If Trim(strFones) <> "" Then strFones = strFones & " - "
                '        strFones = strFones & UpperTrim(Replace(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml, "-", ""))

                '    Case "TELEFONE RESIDENCIAL - V"
                '        If Trim(strFones) <> "" Then strFones = strFones & " - "
                '        strFones = strFones & UpperTrim(Replace(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml, "-", ""))

                '    Case "TELEFONE RESIDENCIAL - G"
                '        If Trim(strFones) <> "" Then strFones = strFones & " - "
                '        strFones = strFones & UpperTrim(Replace(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml, "-", ""))

                '    Case "TELEFONE COMERCIAL - R"
                '        If Trim(strFones) <> "" Then strFones = strFones & " - "
                '        strFones = strFones & UpperTrim(Replace(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml, "-", ""))

                '    Case "TELEFONE COMERCIAL - R1"
                '        If Trim(strFones) <> "" Then strFones = strFones & " - "
                '        strFones = strFones & UpperTrim(Replace(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml, "-", ""))

                '    Case "TELEFONE CELULAR - 1"
                '        If Trim(strFones) <> "" Then strFones = strFones & " - "
                '        strFones = strFones & UpperTrim(Replace(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(3).InnerXml, "-", ""))

                'End Select

                Select Case UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(1).InnerXml)

                    Case "CPF"
                        strCPF = Mid("00000000000000" & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml), Len("00000000000000" & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)) - 13)

                    Case "NOME"
                        strNome = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "ENDERECO"
                        strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "1-ENDERECO"
                        strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "2-ENDERECO"
                        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "3-ENDERECO"
                        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "ENDERECO COMERCIAL"
                        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "1-ENDERECO COMERCIAL"
                        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "2-ENDERECO COMERCIAL"
                        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "3-ENDERECO COMERCIAL"
                        If Trim(strEndereco) = "" Then strEndereco = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "TELEFONE"
                        If Trim(strFones) <> "" Then strFones = strFones & " - "
                        strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "1-TELEFONE"
                        If Trim(strFones) <> "" Then strFones = strFones & " - "
                        strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "2-TELEFONE"
                        If Trim(strFones) <> "" Then strFones = strFones & " - "
                        strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "3-TELEFONE"
                        If Trim(strFones) <> "" Then strFones = strFones & " - "
                        strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "TELEFONE COMERCIAL"
                        If Trim(strFones) <> "" Then strFones = strFones & " - "
                        strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "1-TELEFONE COMERCIAL"
                        'If Trim(strFones) <> "" Then strFones = strFones & " - "
                        'strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)
                        If Trim(strFones) = "" Then strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "2-TELEFONE COMERCIAL"
                        'If Trim(strFones) <> "" Then strFones = strFones & " - "
                        'strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)
                        If Trim(strFones) = "" Then strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "3-TELEFONE COMERCIAL"
                        'If Trim(strFones) <> "" Then strFones = strFones & " - "
                        'strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)
                        If Trim(strFones) = "" Then strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "TELEFONE CELULAR"
                        If Trim(strFones) <> "" Then strFones = strFones & " - "
                        strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "1-TELEFONE CELULAR"
                        If Trim(strFones) <> "" Then strFones = strFones & " - "
                        strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "2-TELEFONE CELULAR"
                        If Trim(strFones) <> "" Then strFones = strFones & " - "
                        strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                    Case "3-TELEFONE CELULAR"
                        If Trim(strFones) <> "" Then strFones = strFones & " - "
                        strFones = strFones & UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).ChildNodes(2).InnerXml)

                End Select

            Next

            strFones = Replace(strFones, " ", "")
            arrFones = Split(Trim(strFones), "-")
            arrEndereco = Split(strEndereco, "-")
            strTipoEnd = Trim(arrEndereco(0))
            strEnd = Trim(arrEndereco(1)) & " " & Trim(arrEndereco(2))
            strNum = Trim(arrEndereco(3))
            strCompl = Trim(arrEndereco(4))
            strBairro = Trim(arrEndereco(5))
            strCidade = Trim(arrEndereco(6))
            strUF = Trim(arrEndereco(7))
            strCEP = Trim(arrEndereco(8))

            strXML = "<ROOT xmlns="""">"

            For x = 0 To UBound(arrFones)

                arrDDD(x) = Mid(Trim(arrFones(x)), 1, 2)
                arrFones(x) = Trim(Mid(arrFones(x), 3))

                strXML = strXML & "<XML ID=""" & Format(Now, "ddMMyyyhhmmss") & x & """"
                strXML = strXML & " DDD=""" & arrDDD(x) & """"
                strXML = strXML & " FONE=""" & arrFones(x) & """"
                strXML = strXML & " NOME=""" & strNome & """"
                strXML = strXML & " TIPO=""" & strTipoEnd & """"
                strXML = strXML & " ENDERECO=""" & strEnd & """"
                strXML = strXML & " NUMERO=""" & strNum & """"
                strXML = strXML & " COMPLEMENTO=""" & strCompl & """"
                strXML = strXML & " BAIRRO=""" & strBairro & """"
                strXML = strXML & " CIDADE=""" & strCidade & """"
                strXML = strXML & " UF=""" & strUF & """"
                strXML = strXML & " CEP=""" & strCEP & """"
                strXML = strXML & " CPF=""" & strCPF & """"
                strXML = strXML & " ACAO=""1"" TABLE=""TELEFONES"" />"

            Next x

            strXML = strXML & "</ROOT>"

            If Trim(strXML) = "<ROOT xmlns=""""></ROOT>" Then strXML = ""

            FormatXMLInfo = strXML

        Catch ex As Exception

            FormatXMLInfo = ""
            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Funções .NET - FormatXMLInfo"
                .Subject = "Erro na rotina FormatXMLInfo"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = "Número: " & Err.Number & Chr(13) & "Source: " & Err.Source & Chr(13) & "Descrição: " & Err.Description & Chr(13) & "Help: " & Chr(13) & Err.HelpFile & Chr(13) & Err.HelpContext & Chr(13) & Err.LastDllError & Chr(13)
            End With

            Mail.Send()

        End Try

    End Function

    Public Function FormatXMLCredi(ByVal strXML As String) As String

        Dim strEndereco As String = ""
        Dim strCPF As String = ""
        Dim strDDD1 As String = ""
        Dim strFone As String = ""
        Dim strTipoEnd As String = ""
        Dim strNumero As String = ""
        Dim strCompl As String = ""
        Dim strCEP As String = ""
        Dim strBairro As String = ""
        Dim strCidade As String = ""
        Dim strUF As String = ""
        Dim strNome As String = ""
        Dim xmlDoc As New XmlDocument
        Dim x As Short = 0
        Dim y As Short = 0

        Try

            xmlDoc.LoadXml(strXML)

            strXML = "<ROOT xmlns="""">"

            For y = 0 To xmlDoc.ChildNodes(1).ChildNodes.Count - 1

                strDDD1 = ""
                strFone = ""
                strNome = ""
                strTipoEnd = ""
                strEndereco = ""
                strNumero = ""
                strCompl = ""
                strBairro = ""
                strCidade = ""
                strUF = ""
                strCEP = ""
                strCPF = ""

                For x = 0 To xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes.Count - 1

                    Select Case UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).Name)

                        Case "CPFCNPJ"
                            strCPF = Mid("00000000000000" & UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString), Len("00000000000000" & UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString)) - 13)

                        Case "NOME"
                            strNome = UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString)

                        Case "TELEFONE"
                            strDDD1 = Left(UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString), 2)
                            strFone = Mid(UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString), 3, 8)

                        Case "ENDERECO"
                            strTipoEnd = UpperTrim(Left(UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString), InStr(UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString), " ")))
                            strEndereco = UpperTrim(Replace(UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString), strTipoEnd, ""))

                        Case "NUMERO"
                            strNumero = UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString)

                        Case "COMPLEMENTO"
                            strCompl = UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString)

                        Case "BAIRRO"
                            strBairro = UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString)

                        Case "CEP"
                            strCEP = UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString)

                        Case "CIDADE"
                            strCidade = UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString)

                        Case "UF"
                            strUF = UpperTrim(xmlDoc.ChildNodes(1).ChildNodes(y).ChildNodes(x).InnerXml.ToString)

                    End Select

                Next x

                strXML = strXML & "<XML ID=""" & Format(Now, "ddMMyyyhhmmss") & x & """"
                strXML = strXML & " DDD=""" & Replace(strDDD1, "NULL", "") & """"
                strXML = strXML & " FONE=""" & Replace(strFone, "NULL", "") & """"
                strXML = strXML & " NOME=""" & Replace(strNome, "NULL", "") & """"
                strXML = strXML & " TIPO=""" & Replace(strTipoEnd, "NULL", "") & """"
                strXML = strXML & " ENDERECO=""" & Replace(strEndereco, "NULL", "") & """"
                strXML = strXML & " NUMERO=""" & Replace(strNumero, "NULL", "") & """"
                strXML = strXML & " COMPLEMENTO=""" & Replace(strCompl, "NULL", "") & """"
                strXML = strXML & " BAIRRO=""" & Replace(strBairro, "NULL", "") & """"
                strXML = strXML & " CIDADE=""" & Replace(strCidade, "NULL", "") & """"
                strXML = strXML & " UF=""" & Replace(strUF, "NULL", "") & """"
                strXML = strXML & " CEP=""" & Replace(strCEP, "NULL", "") & """"
                strXML = strXML & " CPF=""" & Replace(strCPF, "NULL", "") & """"
                strXML = strXML & " ACAO=""1"" TABLE=""TELEFONES"" />"

            Next y

            strXML = strXML & "</ROOT>"

            If Trim(strXML) = "<ROOT xmlns=""""></ROOT>" Then strXML = ""

            FormatXMLCredi = strXML

        Catch ex As Exception

            FormatXMLCredi = ""

            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Funções .NET - FormatXMLCredi"
                .Subject = "Erro na rotina FormatXMLCredi"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = "Número: " & Err.Number & Chr(13) & "Source: " & Err.Source & Chr(13) & "Descrição: " & Err.Description & Chr(13) & "Help: " & Chr(13) & Err.HelpFile & Chr(13) & Err.HelpContext & Chr(13) & Err.LastDllError & Chr(13)
            End With

            Mail.Send()

        End Try

    End Function

    Public Sub CarregarLocal(ByVal strXML As String, ByVal intOrigem As Integer)

        Dim arrDados(13) As String
        Dim xmlDoc As New XmlDocument
        Dim x As Short = 0
        Dim y As Short = 0
        Dim Mirror As New ConnectTo.Comando
        Dim strSql As String = ""
        Dim objDR As System.Data.SqlClient.SqlDataReader = Nothing
        Dim blnInsert As Boolean = False

        Try

            Mirror.Banco = "MIRRORWEB"

            xmlDoc.LoadXml(strXML)

            For x = 0 To xmlDoc.ChildNodes(0).ChildNodes.Count - 1

                blnInsert = False

                For y = 0 To xmlDoc.ChildNodes(0).ChildNodes(x).Attributes.Count - 4

                    arrDados(y) = UpperTrim(xmlDoc.ChildNodes(0).ChildNodes(x).Attributes(y + 1).Value.ToString)

                Next y

                strSql = "Select TOP 1 PESQ_ID From tb_Pesquisa (NOLOCK) Where PESQ_UF = '" & arrDados(9) & "' AND " & _
                                          "PESQ_CGCCPF = '" & arrDados(11) & "' AND " & _
                                          "PESQ_DDD = '" & arrDados(0) & "' AND " & _
                                          "PESQ_FONE = '" & arrDados(1) & "' AND PEOR_ID In(4,5,9)"
                objDR = Mirror.ExecuteQuery(strSql)

                If objDR.Read Then
                    Try
                        strSql = "Delete tb_Pesquisa Where PESQ_ID = " & Trim(objDR("PESQ_ID").ToString)
                        Mirror.Execute(strSql)
                        If Len(Trim(arrDados(0))) = 2 And Len(Trim(arrDados(1))) = 8 Then
                            blnInsert = True
                        End If
                    Catch ex As Exception
                    End Try
                Else
                    objDR.Close()
                    strSql = "Select TOP 1 PESQ_ID From tb_Pesquisa (NOLOCK) Where PESQ_UF = '" & arrDados(9) & "' AND " & _
                           "PESQ_CGCCPF = '" & arrDados(11) & "' AND " & _
                           "PESQ_DDD = '" & arrDados(0) & "' AND " & _
                           "PESQ_FONE = '" & arrDados(1) & "'"
                    objDR = Mirror.ExecuteQuery(strSql)

                    If Not objDR.HasRows Then
                        If Len(Trim(arrDados(0))) = 2 And Len(Trim(arrDados(1))) = 8 Then
                            blnInsert = True
                        End If
                    End If
                End If

                If blnInsert = True Then
                    Try
                        strSql = "prc_INS_Pesquisa "

                        For y = 0 To xmlDoc.ChildNodes(0).ChildNodes(x).Attributes.Count - 4
                            If y > 0 Then strSql = strSql & ","
                            strSql = strSql & "'" & arrDados(y) & "'"
                        Next y

                        strSql = strSql & "," & intOrigem
                        Mirror.Execute(strSql)

                    Catch ex As Exception
                    End Try
                End If

                objDR.Close()

                'strSql = "prc_SEL_Pesquisa '1','" & arrDados(9) & "'," & _
                '                  "'" & arrDados(11) & "'," & _
                '                  "'" & arrDados(0) & "'," & _
                '                  "'" & arrDados(1) & "'," & _
                '                  "''," & "''," & _
                '                  "''," & _
                '                  "''," & _
                '                  "''," & _
                '                  "''," & _
                '                  "''," & _
                '                  "''"
                'objDR = Mirror.ExecuteQuery(strSql)

                'If Not objDR.Read Then
                '    Try
                '        strSql = "prc_INS_Pesquisa "

                '        For y = 0 To xmlDoc.ChildNodes(0).ChildNodes(x).Attributes.Count - 4

                '            If y > 0 Then strSql = strSql & ","
                '            strSql = strSql & "'" & arrDados(y) & "'"

                '        Next y

                '        strSql = strSql & "," & intOrigem

                '        Mirror.Execute(strSql)
                '    Catch ex As Exception

                '    End Try
                'End If

                'objDR.Close()

            Next x

        Catch ex As Exception

            With Mail
                .From = "x_mail@hargos.com.br"
                .Sender = "Funções .NET - Carregar Local"
                .Subject = "Erro na rotina Carregar Local"
                .ToAddress = "rodrigo.barbieri@hargos.com.br"
                .ToName = "Desenvolvimento"
                .IsBodyHTML = False
                .Body = "Número: " & Err.Number & Chr(13) & "Source: " & Err.Source & Chr(13) & "Descrição: " & Err.Description & Chr(13) & "Help: " & Chr(13) & Err.HelpFile & Chr(13) & Err.HelpContext & Chr(13) & Err.LastDllError & Chr(13)
            End With

            Mail.Send()

        End Try

    End Sub

End Module
