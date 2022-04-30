Imports MySql.Data.MySqlClient
'Imports System.Threading
Imports System
Imports System.Collections
Imports System.Globalization
Imports Microsoft.VisualBasic

Imports System.Configuration
Imports System.Data

Imports System.Resources
Imports System.Reflection
Public Class Form1
    Public date5 As String
    Dim mysqlcon As MySqlConnection
    Dim command As MySqlCommand
    Dim reader As MySqlDataReader
    Dim query As String
    Protected ci As CultureInfo
    Private Sub runquery(ByVal query As String)
        mysqlcon = New MySqlConnection
        mysqlcon.ConnectionString = "server=localhost;userid=signet;password=enapass;port=33953;database=dben"
        'mysqlcon.ConnectionString = "server=192.168.1.10;userid=signet;password=enapass;database=dbzorb;port=33953;"
        Try
            mysqlcon.Open()
            command = New MySqlCommand(query, mysqlcon)
            reader = command.ExecuteReader

            mysqlcon.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Sub
        End Try
        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TODATE()
        If returnsinglevaluequery("SELECT id FROM pk_vehicles WHERE code='" & TextBox1.Text & "'") = 0 Then
            MessageBox.Show("Δε βρέθηκε")
            Exit Sub
        End If
        runquery("DELETE FROM pk_movetracks WHERE vehicle in (SELECT id FROM pk_vehicles WHERE code='" & TextBox1.Text & "') AND entrydatetime>'" & date5 & "'")
        'runquery("DELETE FROM pk_vehicles WHERE code='" & TextBox1.Text & "'")
        runquery("DELETE FROM pk_log WHERE vcode='" & TextBox1.Text & "' AND dateentered>'" & date5 & "'")
        TextBox1.Clear()

    End Sub
    Private Sub TODATE()
        Dim date1 As String = Date.Today.ToString

        Dim date3 As String() = date1.Split(" ")

        Dim date4 As String() = date3(0).Split("/")
        date5 = date4(2) + "-" + date4(1) + "-" + date4(0)
    End Sub
    Private Sub TextBox1_Click(sender As Object, e As System.EventArgs) Handles TextBox1.Click
        Dim TypeOfLanguage = New System.Globalization.CultureInfo("en")
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(TypeOfLanguage)
    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged
        Dim TypeOfLanguage = New System.Globalization.CultureInfo("en")
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(TypeOfLanguage)
        TextBox1.Text = TextBox1.Text.ToUpper
        TextBox1.SelectionStart = TextBox1.Text.Length
        Dim j As Integer = 0
        Dim C As Char() = TextBox1.Text
        For i = 0 To C.Length - 1
            If (((65 <= AscW(C(i)) And AscW(C(i)) <= 90)) Or (48 <= AscW(C(i)) And AscW(C(i)) <= 57)) = False Then

                TextBox1.Text.Remove(i, 1)
                TextBox1.Text = TextBox1.Text.Remove(i - j, 1)
                j = j + 1
                TextBox1.SelectionStart = TextBox1.Text.Length
            Else

            End If
            'i = i + 1
        Next
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim TypeOfLanguage = New System.Globalization.CultureInfo("en")
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(TypeOfLanguage)
        Me.Show()
        TextBox1.Focus()
    End Sub
    Public Function returnsinglevaluequery(ByVal query As String) As Object
        Dim item As Object

        mysqlcon = New MySqlConnection
        'mysqlcon.ConnectionString = "server=192.168.1.10;userid=signet;password=enapass;database=dbzorb;port=33953;"
        mysqlcon.ConnectionString = "server=localhost;userid=signet;password=enapass;port=33953;database=dben"

        Try
            mysqlcon.Open()

            command = New MySqlCommand(query, mysqlcon)
            item = command.ExecuteScalar()

            mysqlcon.Close()
        Catch ex As Exception

            MessageBox.Show(ex.Message)

            Exit Function
        End Try
        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
        Return item
    End Function
End Class
