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

    Public started As Boolean
    Dim File2Ext As String
    Public Ended As Boolean = False
    Enum KeyAv
        Name = 0
        creator = 1
        type = 2
        length = 3
        picture = 4
        Polies = 5
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
        Me.Opacity = 0
        Form2.Show()
        If Command.Length < 3 Then

            Form2.Label1.Text = "No file is specified!" & vbCrLf & "exiting..."
            _3sEnder.Start()
            Me.Left = -5000
            Exit Sub
        End If

        Me.Cursor = Nothing

        'load
        label2.Top = -20
        label3.Top = -20

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
        Try
            SetLabel(KeyAv.Polies, Te.Polies)
        Catch ex As Exception

        End Try

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
        'MsgBox(Fname)
        If started = True And IO.Directory.Exists(RvDir & "\levels\" & Fname) Then
            If Button2.Left > 192 Then Button2.Left -= 1
            If Button4.Left > 85 Then Button4.Left -= 4
            Button1.Text = "Re&install"
        Else
            If Button2.Left < 308 Then Button2.Left += 1
            If Button4.Left < Me.Width Then Button4.Left += 4
            Button1.Text = "Install"
        End If

        'form fades
        If mouseon = True And Me.Opacity < 1 Then
            Me.Opacity += 0.01
        ElseIf mouseon = False And Me.Opacity > 0.9 Then
            Me.Opacity -= 0.01
        End If

        'X
        If Xon = True And label2.Top < 0 Then
            Xnonred -= 1
            label2.Top += 1
            ' Label2.BackColor = Color.FromArgb(255, Xnonred, Xnonred)
        ElseIf Xon = False And label2.Top > -20 Then
            Xnonred += 1
            label2.Top -= 1
            ' Label2.BackColor = Color.FromArgb(255, Xnonred, Xnonred)
        ElseIf Xon = True And label2.Top = -2 Then

            If xon1 >= 50 Then
                t = -1.5
            ElseIf xon1 <= -50 Then
                t = 1.5
            End If

            xon1 += t


            ' Label2.BackColor = Color.FromArgb(255, Xnonred + xon1, Xnonred + xon1)
        End If

        '_
        If _On = True And Label3.Top < -2 Then '0; 64; 255 -> 0; 128; 255 //48
            _nonblue -= 2
            Label3.Top += 1
            '   Label3.BackColor = Color.FromArgb(0, _nonblue + 32, 255)
        ElseIf _On = False And label3.Top > -20 Then
            _nonblue += 2
            label3.Top -= 1
            '   Label3.BackColor = Color.FromArgb(0, _nonblue + 32, 255)

        ElseIf _On = True And Label3.Top = -2 Then

            If _on1 >= 100 Then
                t1 = -1
            ElseIf _on1 <= 0 Then
                t1 = 1
            End If
            '
            _on1 += t1


            '  Label3.BackColor = Color.FromArgb(0, _nonblue + _on1, 255)

        End If
        PictureBox2.Left = Button1.Left + 2
        PictureBox3.Left = Button2.Left + 2
        PictureBox4.Left = Button3.Left + 2
        PictureBox5.Left = Button4.Left + 2

        PictureBox2.Width = Button1.Width - 4
        PictureBox3.Width = Button2.Width - 4
        PictureBox4.Width = Button3.Width - 4
        PictureBox5.Width = Button4.Width - 4

        PictureBox6.Width = label2.Width
        PictureBox6.Location = label2.Location + New Point(0, label2.Height - 3)

        PictureBox7.Width = label3.Width
        PictureBox7.Location = label3.Location + New Point(0, label3.Height - 3)



    End Sub

    Private Sub Label2_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles label2.MouseLeave
        Xon = False
        mouseon = False
    End Sub

    Private Sub Label2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles label2.MouseMove
        Xon = True

    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles label2.Click
        Timer2.Start()


    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles label3.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Label3_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles label3.MouseLeave
        _On = False
        mouseon = False
    End Sub

    Private Sub Label3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles label3.MouseMove
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
            Case KeyAv.Polies
                Label9.Text = value
        End Select

        Return True
    End Function

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Button2.Left <> 308 Then
            Form3.ShowMessage("Warning", "This will overwrite the track without backup, wanna continue?")
            If Form3.ShowDialog() = Windows.Forms.DialogResult.No Then Exit Sub
            'If MsgBox("This will overwrite the track without backup, wanna continue?", MsgBoxStyle.YesNo, "Warning") = MsgBoxResult.No Then Exit Sub
        End If
        Form2.Label1.Text = "Installing..."
        Form2.TopMost = True
        Form2.Show()
        ExtractAll()
        Form2.Hide()

        Form2.Show()
        Form2.TopMost = True
        Form2.Label1.Text = "Installed!" & vbNewLine & "exiting in 3 seconds..."
        _3sEnder.Start()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Form2.Label1.Text = "Back up in progress :)"
        Form2.TopMost = True
        Form2.Show()
        Application.DoEvents()
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


        ' Try
        Application.DoEvents()
        n.CompressDirectory(RvDir & "\levels\" & Fname & "\", RvDir & "\levels\backups\" & Fname & " " & Today.ToLongDateString.Replace("/", "'") & " _ " & Now.ToLongTimeString.Replace(":", "'") & ".zip")
        Application.DoEvents()
        Form2.Label1.Text = "Installing..."
        ExtractAll()

        ' Catch


        'Exit Sub
        ' End Try


        '  IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Re-Volt tracks backup\")
        ' For Each Str As String In IO.Directory.GetFiles(RvDir & "\levels\" & Fname & "\")
        'FileCopy(Str, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Re-Volt tracks backup\" & Fname & "\" & Str.Split("\")(UBound(Str.Split("\"))))
        'Next

        Form2.Hide()


        ExtractAll()
        Form2.Label1.Text = "Installed! exiting in 3 seconds!"
        Form2.TopMost = True
        _3sEnder.Start()
        Form2.Show()
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _3sEnder.Tick
        End

    End Sub

    Private Sub _3sNonEnder_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _3sNonEnder.Tick
        Form2.Hide()
    End Sub
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Shell("explorer http://z3.invisionfree.com/Revolt_Live", AppWinStyle.NormalFocus)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Form3.ShowMessage("Uninstall?", "Are you sure about uninstalling this track, wasn't it fun?")
        If Form3.ShowDialog() = Windows.Forms.DialogResult.No Then Exit Sub

        Try
            Form2.Label1.Text = "Uninstalling in progress :( ..."
            Form2.Show()
            Form2.TopMost = True
            For Each Str As String In IO.Directory.GetDirectories(RvDir & "\levels\" & Fname)
                For Each str2 As String In IO.Directory.GetFiles(Str)
                    IO.File.Delete(str2)
                Next
                'IO.File.Delete(Str)
            Next

            For Each Str As String In IO.Directory.GetFiles(RvDir & "\levels\" & Fname)
                IO.File.Delete(Str)
            Next

            For Each Dir As String In IO.Directory.GetDirectories(RvDir & "\levels\" & Fname)
                IO.Directory.Delete(Dir)
            Next
            IO.Directory.Delete(RvDir & "\levels\" & Fname)
            Form2.Hide()
        Catch ex As Exception

            Try
                Form2.Label1.Text = "Uninstallation sadly failed :( due to " & vbNewLine & ex.ToString.Split(":")(0).Split(".")(1)

        Catch
                Form2.Label1.Text = "Uninstallation sadly failed :( due to " & vbNewLine & ex.ToString
                Form2.TopMost = True
                _3sNonEnder.Start()
            End Try



       
        End Try

    End Sub

    Private Sub Label9_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label9.TextChanged
        If Label9.Text <> "Poly Count" Then Label9.Text = Format(CLng(Label9.Text), "#,###")
    End Sub

    Private Sub Timer1_Tick_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Opacity += 0.01
        If Me.Opacity > 0.95 Then Timer1.Stop()
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        mouseon = False
        Me.Opacity -= 0.03
        Application.DoEvents()
        If Me.Opacity < 0.1 Then End
    End Sub

    Private Sub label3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles label3.Click

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'config:
        Dim BrowserPath = "C:\Program Files\Opera 10 Beta\opera.exe"
        Dim WorkingPath = "C:\Users\kallel\Documents\"
        Dim FilePath = "poster.html"
        Dim AfterCreatingLaunchBrowser = True
        Dim WheretosavePictures = "poster/"

        'new folder?
        ChDir(WorkingPath)
        Try
            MkDir("poster")
        Catch
        End Try

        'to get it working/ don't touch
        Dim HTMLheader = "<html><header><title>" & Me.Text & "</title> <style language=" & Chr(34) & "text/css" & Chr(34) & ">" & vbNewLine & "/*generated using Kallel's VB2HTML*/" & vbNewLine
        Dim HTMLMedium = "</style> </header>" & vbNewLine & vbNewLine & "<body>" & vbNewLine
        Dim HTMLbottom = "</body>" & vbNewLine & "</html>"
        Dim x As New String("") 'style
        Dim y As New String("") 'form

        'First: the form itself
        If Me.BackgroundImage IsNot Nothing Then
            Dim bck As Image = Me.BackgroundImage.Clone()
            Me.BackgroundImage.Save(WheretosavePictures & "background.png", Imaging.ImageFormat.Png)

            y &= "<img src='" & WheretosavePictures & "background.png' width='" & Me.Width & "px' height='" & Me.Height & "px' style='position:absolute;'></img>" & vbNewLine
        End If

        'styles
        For Each ct As Control In Me.Controls

            x &= ".style_" & ct.Name & "{" & vbNewLine
            x &= " position: absolute;" & vbNewLine
            x &= "color: RGB(" & ct.ForeColor.R & "," & ct.ForeColor.G & "," & ct.ForeColor.B & ");" & vbNewLine
            x &= "font-family: " & ct.Font.Name & ";" & vbNewLine
            x &= "font: " & ct.Font.Name & ";" & vbNewLine
            x &= "font-size: " & Int(ct.Font.Size) + 3 & "px;" & vbNewLine
            x &= "left: " & ct.Left + 3 & "px;" & vbNewLine
            x &= "top: " & ct.Top + 5 & "px;" & vbNewLine
            x &= "width: " & ct.Width & "px;" & vbNewLine
            x &= "height: " & ct.Height & "px;" & vbNewLine
          


            Try
                Dim null = DirectCast(ct, Button)

                Try
                    Dim thumb As New Bitmap(Width, Height)
                    Dim g As Graphics = Graphics.FromImage(thumb)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

                    Dim bm As New Bitmap(null.BackgroundImage)
                    g.DrawImage(bm, New Rectangle(0, 0, null.Width, null.Height), New Rectangle(0, 0, bm.Width, _
bm.Height), GraphicsUnit.Pixel)

                    g.Dispose()



                    thumb.Save(WheretosavePictures & ct.Name & ".png", Imaging.ImageFormat.Png)
                    x &= "background-image: url(" & WheretosavePictures & ct.Name & ".png" & ");" & vbNewLine
                Catch
                End Try

                x &= "border: " & null.FlatAppearance.BorderSize & "px " & Mid(Hex(null.FlatAppearance.BorderColor.ToArgb), 3, 6) & " solid;" & vbNewLine
                x &= "border-radius: 1px;" & vbNewLine
                x &= "z-index: 1;" & vbNewLine
            Catch

            End Try


            x &= " }" & vbNewLine & vbNewLine

            'Forms


            Try
                Dim null = DirectCast(ct, Button)
                y += "<button type='button' class='style_" & ct.Name & "' >" & ct.Text & "</button>" & vbNewLine

            Catch
            End Try

            Try
                Dim bg = "", ed = ""

                If ct.Font.Underline Then
                    bg += "<u>"
                    ed += "</u>"
                End If

                If ct.Font.Bold Then
                    bg += "<b>"
                    ed += "</b>"
                End If

                If ct.Font.Italic Then
                    bg += "<i>"
                    ed += "</i>"
                End If




                Dim null = DirectCast(ct, Label)
                y += "<div class='style_" & ct.Name & "'>" & ct.Text & "</div>" & vbNewLine
            Catch
            End Try

            Try
                Dim null = DirectCast(ct, PictureBox)
                null.Image.Save(WheretosavePictures & ct.Name & ".png")
                y += "<img class='style_" & ct.Name & "' src='" & WheretosavePictures & ct.Name & ".png" & "'></img>" & vbNewLine
            Catch
            End Try

        Next

        IO.File.WriteAllText(FilePath, HTMLheader & x & HTMLMedium & y & HTMLbottom)
        If AfterCreatingLaunchBrowser = True Then
            Shell(BrowserPath & " " & Chr(34) & CurDir() & "\" & FilePath & Chr(34), AppWinStyle.NormalFocus)
        End If


    End Sub
End Class