Imports System.ComponentModel
Imports System.Web.UI

Public Class MessageBox
    Inherits System.Web.UI.WebControls.WebControl
    Implements IPostBackEventHandler
    Private _Message As String
    Private _Key As String
    Private _PostBackOnYes As Boolean
    Private _PostBackOnNo As Boolean

    Public Event YesChoosed(ByVal sender As Object, ByVal Key As String)
    Public Event NoChoosed(ByVal sender As Object, ByVal Key As String)

    Public Sub ShowConfirmation(ByVal Message As String, _
      ByVal Key As String, _
         ByVal PostBackOnYes As Boolean, _
              ByVal PostBackOnNo As Boolean)
        _Message = "Conf" & Message
        _Key = Key
        _PostBackOnYes = PostBackOnYes
        _PostBackOnNo = PostBackOnNo
    End Sub

    Public Sub ShowMessage(ByVal Message As String)
        _Message = Message
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
        If Not MyBase.Page.IsClientScriptBlockRegistered( _
                                             "MessageBox") Then
            Page.RegisterClientScriptBlock("MessageBox", FunctionJava1())
        End If
    End Sub

    Private Function FunctionJava1() As String
        Dim meuPostBackOnYes As String = _
                               "MessageBoxTextoMensagem="""";"
        Dim meuPostBackOnNo As String = _
                               "MessageBoxTextoMensagem="""";"

        If _PostBackOnYes Then
            meuPostBackOnYes = _
               Page.GetPostBackEventReference(Me, "Yes" & _Key)
        End If
        If _PostBackOnNo Then
            meuPostBackOnNo = _
           Page.GetPostBackEventReference(Me, "No_" & _Key)
        End If

        Return "<script language=""javascript""> " & _
           "var MessageBoxTipoMensagem; " & _
           "var MessageBoxTextoMensagem; " & _
           "if (document.all&&window.attachEvent) { " & _
"window.attachEvent(""onfocus"", MessageBoxMostrarMensagem); " & _
           "} else if (window.addEventListener) {  " & _
           "window.addEventListener(""load""," & _
           "MessageBoxMostrarMensagem,false); }" & _
           "function MessageBoxMostrarMensagem() { " & _
           "if (MessageBoxTextoMensagem) { " & _
           "if (MessageBoxTextoMensagem != """") { " & _
           "if (MessageBoxTipoMensagem==2) {" & _
           " alert(MessageBoxTextoMensagem); " & _
           "} else {" & _
           "if (confirm(MessageBoxTextoMensagem)) { " & _
           meuPostBackOnYes & _
           "} else { " & _
           meuPostBackOnNo & _
           "}} MessageBoxTextoMensagem=""""; " & _
           " }}} </script>"
    End Function

    Protected Overrides Sub Render( _
        ByVal writer As System.Web.UI.HtmlTextWriter)
        If ModoDesign(Me) Then
            writer.Write(Me.ID)
        Else

            If _Message <> String.Empty Then
                Dim miSB As System.Text.StringBuilder = _
                        New System.Text.StringBuilder(_Message)
                miSB.Replace("""", "'"c)

                If miSB.ToString.StartsWith("Conf") Then
                    Me.Page.Response.Write( _
"<script>MessageBoxTipoMensagem=1; MessageBoxTextoMensagem=""" + _
          miSB.ToString.Substring(4) + """;</script>")
                Else
                    Me.Page.Response.Write( _
"<script>MessageBoxTipoMensagem=2; MessageBoxTextoMensagem=""" + _
         miSB.ToString + """;</script>")
                End If
                miSB = Nothing
            End If
        End If
    End Sub

    Private Shared Function ModoDesign(ByVal QueControl As _
    System.Web.UI.WebControls.WebControl) As Boolean
        Dim DesignMode As Boolean = False
        Try
            DesignMode = QueControl.Site.DesignMode
        Catch : End Try
        Return DesignMode
    End Function

    Public Sub RaisePostBackEvent(ByVal eventArgument As String) _
    Implements IPostBackEventHandler.RaisePostBackEvent
        Select Case eventArgument.Substring(0, 3)
            Case "Yes"
                RaiseEvent YesChoosed(Me, eventArgument.Substring(3))
            Case "No_"
                RaiseEvent NoChoosed(Me, eventArgument.Substring(3))
        End Select
    End Sub

End Class



