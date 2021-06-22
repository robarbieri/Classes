Option Explicit On
Option Strict Off

Imports System.Net.Mail
Imports System.Text
Imports System.ServiceProcess

Public Class SendMail

    Public Function Send() As Boolean

        Try

            Dim mailFrom As New MailAddress(strFrom, strSender)
            Dim mailTo As New MailAddress(strTo, strToName)
            If Trim(strCC) <> "" Then blnCC = True
            If Trim(strAttachments) <> "" Then blnAttachments = True
            Dim Services() As ServiceController = ServiceController.GetServices
            Dim Service As ServiceController = Nothing
            Dim blnHasSmtpService As Boolean = False

            For Each Service In Services
                If Service.ServiceName.ToLower = "smtpsvc" Then
                    blnHasSmtpService = True
                    Exit For
                End If
            Next

            If Not blnHasSmtpService Then
                Send = ""
                Exit Function
            End If

            Dim xMail As New MailMessage

            With xMail
                .IsBodyHtml = blnBodyHTML
                .From = mailFrom
                .To.Add(mailTo)
                .Subject = strSubject
                .Body = strBody
                .Priority = MailPriority.Normal
                If blnPriority Then .Priority = MailPriority.High
                If blnCC Then
                    Dim mailCC As New MailAddress(strCC, strCCName)
                    .CC.Add(mailCC)
                End If
                If blnAttachments Then
                    Dim strFile As String
                    Dim strAttach() As String = Split(strAttachments, ";")
                    For Each strFile In strAttach
                        Dim mailAttachment As New Attachment(strFile)
                        .Attachments.Add(mailAttachment)
                    Next
                End If
            End With

            Try
                'Dim client As New SmtpClient("smarthost")
                Dim client As New SmtpClient("dbmirror")
                client.Send(xMail)
                Send = True '"X-Mail Enviado com Sucesso!"
            Catch err As Exception
                Send = False
                Exit Function
            End Try

        Catch err As Exception
            Send = False
            Exit Function
        End Try

    End Function

    Public Sub New()

        strFrom = ""
        strSender = ""
        strTo = ""
        strToName = ""
        blnCC = False
        strCC = ""
        strCCName = ""
        blnAttachments = False
        strAttachments = ""
        blnBodyHTML = False
        blnPriority = False
        strSubject = ""
        strBody = ""
        blnUseLayout = False

    End Sub

    Public Property From() As String
        Get
            Return strFrom
        End Get
        Set(ByVal Value As String)
            If strFrom <> Value Then strFrom = Value
        End Set
    End Property

    Public Property Sender() As String
        Get
            Return strSender
        End Get
        Set(ByVal Value As String)
            If strSender <> Value Then strSender = Value
        End Set
    End Property

    Public Property ToAddress() As String
        Get
            Return strTo
        End Get
        Set(ByVal Value As String)
            If strTo <> Value Then strTo = Value
        End Set
    End Property

    Public Property ToName() As String
        Get
            Return strToName
        End Get
        Set(ByVal Value As String)
            If strToName <> Value Then strToName = Value
        End Set
    End Property

    Public Property CC() As String
        Get
            Return strCC
        End Get
        Set(ByVal Value As String)
            If strCC <> Value Then strCC = Value
        End Set
    End Property

    Public Property CCName() As String
        Get
            Return strCCName
        End Get
        Set(ByVal Value As String)
            If strCCName <> Value Then strCCName = Value
        End Set
    End Property

    Public Property Attachments() As String
        Get
            Return strAttachments
        End Get
        Set(ByVal Value As String)
            If strAttachments <> Value Then strAttachments = Value
        End Set
    End Property

    Public Property IsBodyHTML() As Boolean
        Get
            Return blnBodyHTML
        End Get
        Set(ByVal Value As Boolean)
            If blnBodyHTML <> Value Then blnBodyHTML = Value
        End Set
    End Property

    Public Property HighPriority() As Boolean
        Get
            Return blnPriority
        End Get
        Set(ByVal Value As Boolean)
            If blnPriority <> Value Then blnPriority = Value
        End Set
    End Property

    Public Property Subject() As String
        Get
            Return strSubject
        End Get
        Set(ByVal Value As String)
            If strSubject <> Value Then strSubject = Value
        End Set
    End Property

    Public Property Body() As String
        Get
            Return strBody
        End Get
        Set(ByVal Value As String)
            If strBody <> Value Then strBody = Value
        End Set
    End Property

    Public Property UsePreDefinedLayout() As Boolean
        Get
            Return blnUseLayout
        End Get
        Set(ByVal Value As Boolean)
            If blnUseLayout <> Value Then blnUseLayout = Value
        End Set
    End Property

End Class
