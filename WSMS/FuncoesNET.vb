Module FuncoesNET

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

End Module
