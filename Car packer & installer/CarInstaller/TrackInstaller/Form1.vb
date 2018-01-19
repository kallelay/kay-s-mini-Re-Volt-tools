Imports SevenZip

Public Class Form1
    Private Mpos As Point 'mouse position
    Dim mouseon As Boolean = False
    Dim Xon As Boolean = False
    Dim _On As Boolean = False
    Dim Xnonred As Integer = 85
    Dim _nonblue As Integer = 64
    Dim xon1 = -50, t As Integer
    Dim _on1 = 0, t1 As Integer
    Public Fname As String
    Public started As Boolean
    Dim File2Ext As String
    Public Ended As Boolean = False
    Enum KeyAv
        Name = 0
        creator = 1
        type = 2
        length = 3
        picture = 4
    End Enum
    Sub RequestrvDir()
        If GetSetting("LiveInstaller", "settings", "dir", "") = "" Then

            Form2.Label1.Text = "Please Run 'Configure Installers' before"
            _3sEnder.Start()
            Me.Left = -5000
            Exit Sub
        End If

        RvDir = GetSetting("LiveInstaller", "settings", "dir", "")
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Form2.Show()
        If Command.Length < 3 Then

            Form2.Label1.Text = "No file is specified!" & vbCrLf & "exiting..."
            _3sEnder.Start()
            Me.Left = -5000
            Exit Sub
        End If

        Me.Cursor = Nothing

        'load
        Label2.Top = -32
        Label3.Top = -32

        RequestrvDir()

        For Each ctr As Control In Me.Controls
            AddHandler ctr.MouseMove, AddressOf mouseOver
        Next


        Application.DoEvents()
        Te = TrackInfoFile(Command)
        SetLabel(KeyAv.creator, Te.Creator)
        SetLabel(KeyAv.Name, Te.Name)
        SetLabel(KeyAv.length, Te.Length)
        SetLabel(KeyAv.type, Te.Crassu)
        Fname = Te.foldername
        Try
            SetLabel(KeyAv.picture, Te.pic)
        Catch ex As Exception
        End Try
        Form2.Hide()
        Ended = True
        started = True
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
            Me.Cursor = Cursors.Arrow

        End If
        mouseon = True 'activate fade



    End Sub

    Private Sub SmthFade_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmthFade.Tick

        If started = True And IO.Directory.Exists(RvDir & "\levels\" & Fname) Then
            If Button2.Left > 192 Then Button2.Left -= 1
        End If

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
        mouseon = False
    End Sub

    Private Sub Label2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Label2.MouseMove
        Xon = True

    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        End
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Label3_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label3.MouseLeave
        _On = False
        mouseon = False
    End Sub

    Private Sub Label3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Label3.MouseMove
        ' mouseon = True
        _On = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        End
    End Sub
    Function SetLabel(ByVal key As KeyAv, ByVal value As String)
        Select Case key
            Case KeyAv.Name
                Label14.Text = value
            Case KeyAv.creator
                Label13.Text = value
            Case KeyAv.type

                Label12.Text = value
            Case KeyAv.length
                Label11.Text = value & "m"
            Case KeyAv.picture
                PictureBox1.ImageLocation = value
        End Select

        Return True
    End Function

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Button2.Left <> 308 Then
            If MsgBox("This will overwrite the track without backup, wanna continue?", MsgBoxStyle.YesNo, "Warning") = MsgBoxResult.No Then Exit Sub
        End If
        Form2.Label1.Text = "Please wait until the track is extracted"
        Form2.TopMost = True
        Form2.Show()
        ExtractAll()
        Form2.Hide()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim n As New SevenZip.SevenZipCompressor
        n.ArchiveFormat = SevenZip.OutArchiveFormat.Zip
        n.CompressionLevel = CompressionLevel.Ultra
        Try
            IO.Directory.CreateDirectory(RvDir & "\levels\backups\")
        Catch
        End Try

        Try
            IO.File.Create(RvDir & "\levels\" & Fname & "\empty")
        Catch ex As Exception
        End Try


        Try
            n.CompressDirectory(RvDir & "\levels\" & Fname, RvDir & "\levels\backups\" & Fname & Now.TimeOfDay.ToString.Replace(":", "_") & ".zip")
            ExtractAll()
            End
        Catch

            Form2.Label1.Text = "Failed to create backup fail, sorry :("
            _3sNonEnder.Start()
            Exit Sub
        End Try


        '  IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Re-Volt tracks backup\")
        ' For Each Str As String In IO.Directory.GetFiles(RvDir & "\levels\" & Fname & "\")
        'FileCopy(Str, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Re-Volt tracks backup\" & Fname & "\" & Str.Split("\")(UBound(Str.Split("\"))))
        'Next




        ExtractAll()

    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _3sEnder.Tick
        End

    End Sub

    Private Sub _3sNonEnder_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _3sNonEnder.Tick
        Form2.Hide()
    End Sub
End Class