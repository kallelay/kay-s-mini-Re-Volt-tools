Public Class Form1

    Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyValue = 13 Then
            TextBox2.AutoCompleteCustomSource.Add(TextBox2.Text)
            Dim txt = TextBox2.Text
            TextBox2.Text = ""
            Process(txt)
        End If

    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
      
    End Sub
    Public Fv As Boolean = False
    Public Nv As Boolean = False
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
     
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Process("backup")
        RequestTerminate = True
    End Sub

    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If IO.File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\nvolt\nvolt.exe") Then
            DoWrite(">> nVolt is detected, we're going to allow preview mesh possibility :)")
            Nv = True

        End If
        If IO.File.Exists("rv2.exe") Or IO.File.Exists(Environ("windir") & "\rv2.exe") Or IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.System) & "\system32\rv2.exe") Then
            DoWrite(">> FreeVolts is detected, we're going to allow preview mesh possibility :)")
            If Nv = True Then DoWrite(">> However, since nVolt came first, FreeVolts will be disabled")
            Fv = True

        End If
        Dim cmd = Replace(Command, """", "")
        If Command() <> "" Then
            Process("script " & cmd)
        End If
    End Sub
End Class
