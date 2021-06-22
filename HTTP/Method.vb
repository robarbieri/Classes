Option Explicit On
Option Strict Off

Imports System.Net
Imports System.IO

Public Class Method
    Public Enum HTTPMethod As Short
        HTTP_GET = 0
        HTTP_POST = 1
    End Enum

    Public Shared Function Send(ByVal URL As String, _
        Optional ByVal PostData As String = "", _
        Optional ByVal Method As HTTPMethod = HTTPMethod.HTTP_GET, _
        Optional ByVal ContentType As String = "")

        Dim Request As HttpWebRequest = WebRequest.Create(URL)
        Dim Response As HttpWebResponse
        Dim SW As StreamWriter
        Dim SR As StreamReader
        Dim ResponseData As String

        Request.Method = Method.ToString().Substring(5)

        If (Method = HTTPMethod.HTTP_POST AndAlso PostData <> "" AndAlso ContentType = "") Then
            ContentType = "application/x-www-form-urlencoded"
        End If

        If (ContentType <> "") Then
            Request.ContentType = ContentType
            Request.ContentLength = PostData.Length
        End If

        If (Method = HTTPMethod.HTTP_POST) Then
            Try
                SW = New StreamWriter(Request.GetRequestStream())
                SW.Write(PostData)
            Catch Ex As Exception
                Throw Ex
            Finally
                SW.Close()
            End Try
        End If

        Try
            Response = Request.GetResponse()
            SR = New StreamReader(Response.GetResponseStream())
            ResponseData = SR.ReadToEnd()
        Catch Wex As System.Net.WebException
            SR = New StreamReader(Wex.Response.GetResponseStream())
            ResponseData = SR.ReadToEnd()
            Throw New Exception(ResponseData)
        Finally
            SR.Close()
        End Try

        Return ResponseData
    End Function
End Class

