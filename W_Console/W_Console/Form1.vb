Imports System.Runtime.InteropServices

Public Class Form1
    Implements System.Windows.Forms.IMessageFilter
    Dim capsLock = False
    Dim Locked = False

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown

        '        If TextBox1.Text(InStr(Mid(TextBox1.Text, 1, TextBox1.SelectionStart), vbNewLine)) = ">" Then
        'Locked = False
        'Else
        'Locked = True
        'End If

        ' 'If TextBox1.Text(TextBox1.SelectionStart + 1) = ">" Then
        'If (TextBox1.SelectionStart = 0 Or TextBox1.Text(Math.Min(TextBox1.SelectionStart, TextBox1.Text.Length - 1)) = vbNewLine) Then

        '        Locked = True
        '       End If

        If InStr(TextBox1.SelectionStart + 1, TextBox1.Text, vbNewLine, CompareMethod.Binary) = 0 Then
            Locked = False
        Else
            Locked = True
        End If


        Select Case e.KeyCode
            Case Keys.Tab


                If ListBox1.Visible Then
                    Application.DoEvents()
                    ListBox1.Dock = DockStyle.None
                    For i = 0 To 255 - 224 Step 5
                        Application.DoEvents()
                        ListBox1.Left += 10
                        ' ListBox1.BackColor = Color.FromArgb(224 + i, 224 + i, 224 + i)
                        Threading.Thread.Sleep(10)
                    Next
                    ListBox1.BackColor = Color.White
                    TextBox1.SelectionStart = Len(TextBox1.Text)
                    ListBox1.Hide()
                    TextBox1.ScrollToCaret()
                    Exit Sub
                End If



                ListBox1.Dock = DockStyle.None
                ListBox1.Top = 0
                ListBox1.Height = Me.Height
                ListBox1.Left = Me.Width

                ListBox1.Show()





                Application.DoEvents()
                For i = 0 To 158 \ 10
                    ListBox1.Left -= 10
                    Threading.Thread.Sleep(10)
                Next
                ListBox1.Dock = DockStyle.Right


            Case Keys.Escape
                RequestTerminate = True

            Case Keys.Z And e.Control
                TextBox1.Undo()

            Case Keys.C And e.Control
                Clipboard.SetText(TextBox1.SelectedText)

            Case Keys.X And e.Control
                Dim v = TextBox1.SelectionStart
                Clipboard.SetText(TextBox1.SelectedText)
                If Not Locked Then TextBox1.Text = TextBox1.Text.Remove(TextBox1.SelectionStart, TextBox1.SelectionLength)
                TextBox1.SelectionStart = v

            Case Keys.V And e.Control
                Dim v = TextBox1.SelectionStart
                If Locked Then Exit Sub
                TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart, Clipboard.GetText)
                TextBox1.SelectionStart = v + Len(Clipboard.GetText)
                'Debugger.Break()

            Case Keys.CapsLock
                '  capsLock = Not capsLock


            Case Keys.Down, Keys.Up, Keys.Right, Keys.Left
            Case Keys.ControlKey
                '   TextBox1.SelectionStart += Math.Max(0, InStr(Mid(TextBox1.Text, TextBox1.SelectionStart - 1), " ") + TextBox1.SelectionStart)

            Case Keys.Space
                '      If Locked Then Exit Sub
                '     Dim v = TextBox1.SelectionStart '  = Len(TextBox1.Text)
                '
                'If Not (e.Shift Or capsLock) Then
                'TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart, " ")
                'Else
                'TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart, vbTab)
                'End If
                'TextBox1.SelectionStart = v + 1


            Case Keys.Enter
                If Locked Then Exit Sub
                Dim v = TextBox1.SelectionStart '  = Len(TextBox1.Text)

                TextBox1.Text &= vbNewLine
                Process(Split(TextBox1.Text, vbNewLine)(Split(TextBox1.Text, vbNewLine).Length - 2))
                'If Not (e.Shift Or capsLock) Then

                '  Else
                '     TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart, vbNewLine)
                'End If
                'TextBox1.SelectionStart = v + 4

                TextBox1.SelectionStart = Len(TextBox1.Text)


            Case Keys.ShiftKey  'speical


            Case Keys.Back
                If Locked Then Exit Sub

                Dim v = TextBox1.SelectionStart


                If TextBox1.SelectionLength = 0 Then



                    If (TextBox1.Text(Math.Max(v - 1, 0)) = ">" Or TextBox1.Text(Math.Max(0, v - 1)) = "?") And (v = 1 Or TextBox1.Text(Math.Max(v - 2, 0)) = Chr(10)) Then Exit Sub
                    TextBox1.Text = TextBox1.Text.Remove(TextBox1.SelectionStart - 1, 1)
                    TextBox1.SelectionStart = v - 1
                Else
                    TextBox1.Text = TextBox1.Text.Remove(v, TextBox1.SelectionLength)
                    TextBox1.SelectionStart = v
                End If


            Case Keys.Delete
                If Locked Then Exit Sub
                Dim v = TextBox1.SelectionStart

                Try
                    If (TextBox1.Text(v) = ">" Or TextBox1.Text(v) = "?") And TextBox1.Text(Math.Max(0, v - 2)) = Chr(13) Then Exit Sub
                    Debug.WriteLine("v:" & v & ":" & Asc(TextBox1.Text(Math.Max(0, v - 2))))
                    If v = 0 Then Exit Sub
                    TextBox1.Text = TextBox1.Text.Remove(TextBox1.SelectionStart, Math.Max(1, TextBox1.SelectionLength))
                    TextBox1.SelectionStart = v

                Catch ex As Exception

                End Try


            Case Keys.Apps
                'Case Else


                If Not (e.KeyValue >= 65 And e.KeyValue <= 65 + 26) Then
                    TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart, Chr(e.KeyCode))


                    Exit Sub
                End If

                If Not (e.Shift Or capsLock) Then
                Else
                    TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart, Chr(e.KeyCode + 32))
                End If

        End Select
        TextBox1.ScrollToCaret()
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress

        Select Case Asc(e.KeyChar)
            Case Keys.Back, Keys.ControlKey, Keys.Control, Keys.Alt, Keys.Capital, Is <= 22
                Exit Sub

        End Select





        If Locked Then Exit Sub

        Dim v = TextBox1.SelectionStart '  = Len(TextBox1.Text)


        If TextBox1.SelectionLength > 1 Then

            TextBox1.Text = TextBox1.Text.Remove(v, TextBox1.SelectionLength)
            TextBox1.SelectionStart = v

        End If



        Try
            If TextBox1.Text(Math.Max(v - 1, 0)) = Chr(10) Or v = 0 Then _
                    If (TextBox1.Text(v) = ">" Or TextBox1.Text(v) = "?") Then Exit Sub

        Catch ex As Exception

        End Try

        TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart, e.KeyChar)
        TextBox1.SelectionStart = v + 1
        TextBox1.ScrollToCaret()
    End Sub








    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub
    Public Fv As Boolean = False
    Public Nv As Boolean = False
    Private WithEvents BS As New BuildString
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

        Select Case m.Msg
            Case &H400
                BS.BuildString(m.LParam)
            Case Else
                MyBase.WndProc(m)
        End Select
        ' MyBase.WndProc(m)
    End Sub
    Private Sub SB_StringOK(ByVal Result As String) Handles BS.StringOK

        Process("texanim " & Result)
    End Sub
    Public Function PreFilterMessage(ByRef m As System.Windows.Forms.Message) As Boolean Implements System.Windows.Forms.IMessageFilter.PreFilterMessage
        BS.BuildString(m.Result)


    End Function
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DoWrite("W_Console v3.0")
        DoWrite("Operating System: " & Environment.OSVersion.ToString)
        If InStr(Environment.OSVersion.ToString, "Unix", CompareMethod.Text) > 0 Then
            Unix = True
        End If
        DoWrite("is Unix?:" & CStr(Unix))


        DoWrite(".NET CLR: " & Environment.Version.ToString)






    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Process("backup")
        RequestTerminate = True
    End Sub



    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        '  DoWrite(My.Computer.Info.OSFullName)

        If Unix Then
            'UNIX
            If IO.File.Exists("/nvolt/nvolt.exe") Then
                DoWrite("nVolt: Found")
                Nv = True
            Else
                DoWrite("nVolt: not Found")
            End If
            If IO.File.Exists("freevolts/rv2.exe") Then
                DoWrite(" FreeVolts: Found")
                If Nv = True Then DoWrite(" FreeVolts:Disabled")
                If Nv = True Then DoWrite("     Hint: use fvpreview to call Freevolts")
                Fv = True

            End If


        Else
            'WINDOWS

            If IO.File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\nvolt\nvolt.exe") Then
                DoWrite(" nVolt: Found")
                Nv = True

            End If
            If IO.File.Exists("rv2.exe") Or IO.File.Exists(Environ("windir") & "\rv2.exe") Or IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.System) & "\system32\rv2.exe") Then
                DoWrite(" FreeVolts: Found")
                If Nv = True Then DoWrite(" FreeVolts:Disabled")
                If Nv = True Then DoWrite("         Hint: use fvpreview to call Freevolts")
                Fv = True

            End If

        End If

        DoWrite("! session started")




        TextBox1.Text = TextBox1.Text & "> "
        TextBox1.Focus()
        TextBox1.SelectionStart = Len(TextBox1.Text)
        TextBox1.ScrollToCaret()
        Dim cmd = Replace(Command, """", "")
        cmd = Replace(cmd, Application.ExecutablePath, "")
        If Command() <> "" Then
            Process("script " & cmd)
        End If
    End Sub




    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged







        TextBox1.Text &= ListBox1.SelectedItem
        Application.DoEvents()
        ListBox1.Dock = DockStyle.None
        For i = 0 To 255 - 224 Step 5
            Application.DoEvents()
            ListBox1.Left += 10
            ' ListBox1.BackColor = Color.FromArgb(224 + i, 224 + i, 224 + i)
            Threading.Thread.Sleep(10)
        Next
        ListBox1.BackColor = Color.White
        TextBox1.SelectionStart = Len(TextBox1.Text)
        ListBox1.Hide()
        TextBox1.Focus()
        TextBox1.SelectionStart = Len(TextBox1.Text)
        TextBox1.ScrollToCaret()
    End Sub

End Class
