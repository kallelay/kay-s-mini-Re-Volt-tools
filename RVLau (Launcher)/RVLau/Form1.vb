Public Class Form1
    Dim Mode As umod = umod._3dsmax
    Enum umod
        _3dsmax
        _blender
    End Enum
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData) = False Then
            MkDir(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData)

        End If
        If IO.File.Exists(TextBox1.Text & "\revolt.exe") Then
            IO.File.WriteAllText(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "rvpath", TextBox1.Text)
            TextBox1.ForeColor = Color.Green
            Button1.Enabled = True
            TextBox2_TextChanged(sender, e)
        Else
            TextBox1.ForeColor = Color.Maroon
            Button1.Enabled = False

        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If IO.Directory.Exists(TextBox1.Text & "\levels\" & TextBox2.Text) And TextBox2.Text <> "" Then
            TextBox2.ForeColor = Color.Green
            Timer1.Start()
            Button1.Text = "Auto Launch once exported"
            Try
                OldAse = IO.File.ReadAllText(TextBox1.Text & "\levels\" & TextBox2.Text & "\" & TextBox2.Text & ".ase")

            Catch ex As Exception
                OldAse = Nothing
            End Try
            Try
                Oldw = IO.File.ReadAllBytes(TextBox1.Text & "\levels\" & TextBox2.Text & "\" & TextBox2.Text & ".w")
            Catch ex As Exception

            End Try

            Button1.Enabled = False
            IO.File.WriteAllText(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "levelname", TextBox2.Text)
        Else
            TextBox2.ForeColor = Color.Maroon
            Button1.Text = "&Launch"
            Timer1.Stop()
            Button1.Enabled = True
        End If
    End Sub
    Dim myProcess As Process
    Dim ioFile As IO.StreamReader

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim rev = Process.GetProcessesByName("revolt")
        For i = 0 To rev.Count - 1
            rev(i).Kill()
        Next
        Try
            IO.File.Delete(Environ("temp") & "\revolt.log")

        Catch ex As Exception

        End Try
        IO.File.WriteAllText(Environ("temp") & "\revolt.log", Nothing)

        ChDir(TextBox1.Text)
        myProcess = New Process()
        myProcess.StartInfo.FileName = "revolt"
        myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal

        Dim args$ = ""
        For i = 1 To 6
            If DirectCast(Me.Controls.Find("checkbox" & i, True)(0), CheckBox).Checked Then
                args += "-" & Me.Controls.Find("checkbox" & i, True)(0).Text & " "
            End If
        Next
        ' Debugger.Break()
        myProcess.StartInfo.Arguments = args
        myProcess.EnableRaisingEvents = True

        myProcess.Start()

        Do Until myProcess.Responding

        Loop

        myProcess.WaitForInputIdle(1000)



        For i = 0 To 500
            Application.DoEvents()
            Threading.Thread.Sleep(100)
            System.Windows.Forms.SendKeys.Send(Chr(13))

            Try
                Dim ioFile As IO.StreamReader = New IO.StreamReader(New IO.FileStream(Environ("temp") & "\revolt.log", IO.FileMode.Open, IO.FileAccess.Read))

                If InStr(ioFile.ReadToEnd.Replace("frontend.w", ""), ".w", CompareMethod.Text) <> 0 Then
                    Exit For

                End If

                If Process.GetProcessesByName("revolt").Length = 0 Then
                    Exit For
                End If

                ioFile.Close()
            Catch ex As Exception

            End Try

        Next



    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            TextBox1.Text = IO.File.ReadAllText(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "rvpath")
            TextBox2.Text = IO.File.ReadAllText(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "levelname")
        Catch ex As Exception

        End Try
    End Sub
    Dim MousePOs As Point

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If MouseButtons = Windows.Forms.MouseButtons.Left Then
            Me.Location += Cursor.Position - MousePOs
        End If
        MousePOs = Cursor.Position
    End Sub
    Dim Ase$, OldAse$
    Dim Oldw() As Byte, W() As Byte
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If OldAse = "" Then Exit Sub
        If TextBox1.ForeColor = Color.Maroon Then Exit Sub
        If TextBox2.ForeColor = Color.Maroon Then Exit Sub
        Try
            Ase = IO.File.ReadAllText(TextBox1.Text & "\levels\" & TextBox2.Text & "\" & TextBox2.Text & ".ase")
            If Ase <> OldAse Then
                ChDir(TextBox1.Text & "\levels\" & TextBox2.Text & "\")
                Dim args = If(CheckBox7.Checked, "-morph", "")
                If args <> "-morph" Then args = If(CheckBox8.Checked, "-ali", "")
                Dim proc = Process.Start("ase2w", args & " " & TextBox2.Text & ".ase")
                proc.WaitForExit()
                OldAse = Ase


            End If
        Catch ex As Exception

        End Try

        If Oldw.Length = 0 Then Exit Sub
        Try
            W = IO.File.ReadAllBytes(TextBox1.Text & "\levels\" & TextBox2.Text & "\" & TextBox2.Text & ".w")
            If Not areThoseBytesSimilar(W, Oldw) Then
                Oldw = W
                Button1_Click(sender, e)
            End If
        Catch ex As Exception
          
        End Try

        '  C:\Games\Re-Volt\levels\destruct
    End Sub
    Function areThoseBytesSimilar(ByVal byte1() As Byte, ByVal byte2() As Byte) As Boolean
        If byte1.Length <> byte2.Length Then Return False
        For i = 0 To byte1.Length 'Step 4
            If byte1(i) <> byte2(i) Then GoTo fail

        Next
        Return True
fail:
        Return False
    End Function

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Shell("explorer """ & TextBox1.Text & "\levels\" & TextBox2.Text)
    End Sub
End Class
