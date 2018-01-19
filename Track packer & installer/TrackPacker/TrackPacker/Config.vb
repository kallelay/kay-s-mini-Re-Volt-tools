Public Class Config
    Private Mpos As Point 'mouse position
    Dim mouseon As Boolean = False
    Dim Xon As Boolean = False
    Dim _On As Boolean = False
    Dim Xnonred As Integer = 85
    Dim _nonblue As Integer = 64
    Dim xon1 = -50, t As Integer
    Dim _on1 = 0, t1 As Integer
    Dim RvDir As String
    Dim File2Ext As String


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load
        Label2.Top = -32
        Label3.Top = -32



        For Each ctr As Control In Me.Controls
            AddHandler ctr.MouseMove, AddressOf mouseOver
        Next
    End Sub
    Sub mouseOver(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        mouseon = True
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Location += (MousePosition - Mpos)
            Mpos = MousePosition

        Else
            'capture pointer position
            Mpos = MousePosition


        End If
        mouseon = True 'activate fade
    End Sub
    Private Sub Form1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave

        mouseon = False 'activate face
    End Sub


    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Location += (MousePosition - Mpos)
            Mpos = MousePosition
            Cursor = Cursors.SizeAll
        Else
            'capture pointer position
            Mpos = MousePosition
            Cursor = Cursors.Default

        End If
        mouseon = True 'activate fade



    End Sub

    Private Sub SmthFade_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmthFade.Tick
        'form fades
        If mouseon = True And Me.Opacity < 1 Then
            Me.Opacity += 0.01
        ElseIf mouseon = False And Me.Opacity > 0.9 Then
            Me.Opacity -= 0.01
        End If

        'X
        If Xon = True And Label2.Top < -2 Then
            Xnonred -= 1
            Label2.Top += 1
            Label2.BackColor = Color.FromArgb(255, Xnonred, Xnonred)
        ElseIf Xon = False And Label2.Top > -32 Then
            Xnonred += 1
            Label2.Top -= 1
            Label2.BackColor = Color.FromArgb(255, Xnonred, Xnonred)
        ElseIf Xon = True And Label2.Top = -2 Then

            If xon1 >= 50 Then
                t = -1.5
            ElseIf xon1 <= -50 Then
                t = 1.5
            End If

            xon1 += t


            Label2.BackColor = Color.FromArgb(255, Xnonred + xon1, Xnonred + xon1)
        End If

        '_
        If _On = True And Label3.Top < -2 Then '0; 64; 255 -> 0; 128; 255 //48
            _nonblue -= 2
            Label3.Top += 1
            Label3.BackColor = Color.FromArgb(0, _nonblue + 32, 255)
        ElseIf _On = False And Label3.Top > -32 Then
            _nonblue += 2
            Label3.Top -= 1
            Label3.BackColor = Color.FromArgb(0, _nonblue + 32, 255)

        ElseIf _On = True And Label3.Top = -2 Then

            If _on1 >= 100 Then
                t1 = -1
            ElseIf _on1 <= 0 Then
                t1 = 1
            End If

            _on1 += t1


            Label3.BackColor = Color.FromArgb(0, _nonblue + _on1, 255)

        End If

    End Sub

    Private Sub Label2_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label2.MouseLeave
        Xon = False
    End Sub

    Private Sub Label2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Label2.MouseMove
        Xon = True
        ' mouseon = True
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        End
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Label3_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label3.MouseLeave
        _On = False
    End Sub

    Private Sub Label3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Label3.MouseMove
        ' mouseon = True
        _On = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub


    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Shell("explorer http://z3.invisionfree.com/Revolt_Live", AppWinStyle.NormalFocus)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If MsgBox("Confirm Exit?", MsgBoxStyle.YesNo, "?") = MsgBoxResult.Yes Then
            End
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectPlay\Applications\Revolt", "Path", "") <> "" Then
            TextBox7.Text = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectPlay\Applications\Revolt", "Path", "")
        Else
            If Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectPlay\Applications\Revolt", "CurrentDirectory", "") <> "" Then
                TextBox7.Text = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectPlay\Applications\Revolt", "CurrentDirectory", "")
            Else
                MsgBox("failed to detect Re-Volt directory...", MsgBoxStyle.Critical, "Failed...")
            End If
        End If
    End Sub
    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged
        If IO.Directory.Exists(TextBox7.Text) = True Then
            ListView1.Items.Clear()
            ChDir(TextBox7.Text)
            Dim Dirs() As String
            Dirs = IO.Directory.GetDirectories(CurDir)

            ListView1.Items.Add("..", 0)
            For i = LBound(Dirs) To UBound(Dirs)
                Dim LV As New ListViewItem
                LV.ImageIndex = 0
                LV.Text = Dirs(i).Split("\")(UBound(Dirs(i).Split("\")))
                ListView1.Items.Add(LV)


            Next

            If IO.File.Exists("revolt.exe") Then
                Button7.Enabled = True
                Button7.Text = "Set this as Re-Volt directory"
            Else
                Button7.Enabled = False
                Button7.Text = "::"
            End If
        End If
    End Sub


    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        If ListView1.SelectedItems(0).Text <> "" Or ListView1.SelectedItems(0).Text <> Nothing Then
            ChDir(ListView1.SelectedItems(0).Text)
            ListView1.Items.Clear()
            TextBox7.Text = CurDir()
        End If
    End Sub

    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        ChDir(Environ("programfiles") & "\..\")
        TextBox7.Text = CurDir()
        Dim Dirs() As String
        Dirs = IO.Directory.GetDirectories(CurDir)

        ListView1.Items.Add("..", 0)




        For i = LBound(Dirs) To UBound(Dirs)
            Dim LV2 As New ListViewItem
            LV2.ImageIndex = 0
            LV2.Text = Dirs(i).Split("\")(UBound(Dirs(i).Split("\")))
            ListView1.Items.Add(LV2)
        Next

        If GetSetting("LiveInstaller", "settings", "dir", "") <> "" Then
            TextBox7.Text = GetSetting("LiveInstaller", "settings", "dir", "")
        End If
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        SaveSetting("LiveInstaller", "settings", "dir", TextBox7.Text)
        Label5.Text = "Performing a small check..."
        Try
            IO.File.WriteAllText(TextBox7.Text & "\TEMPFILE", "directory is permissive")
        Catch ex As Exception
            MsgBox("Actually... This Path is a little problematic if you understand what do I mean" & Chr(13) & "Windows Vista/7 fails in permissions...")
            Exit Sub
        End Try

        End

    End Sub
End Class