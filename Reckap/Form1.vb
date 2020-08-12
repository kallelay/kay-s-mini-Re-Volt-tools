Imports System.IO

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Width = 397
        Me.Height = 331
        Panel4.Enabled = False

        Panel8.Left = Panel7.Left + Panel7.Width + 10
        Panel9.Left = Panel8.Left + Panel7.Width + 10
        Panel12.Left = Panel9.Left + Panel7.Width + 10
        Panel10.Left = Panel12.Left + Panel7.Width + 10
        Panel11.Left = Panel10.Left + Panel7.Width + 10
        Panel8.Top = Panel7.Top
        Panel9.Top = Panel7.Top
        Panel10.Top = Panel7.Top
        Panel11.Top = Panel7.Top
        Panel12.Top = Panel7.Top
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ofdw.ShowDialog = Windows.Forms.DialogResult.OK Then
            Panel4.Enabled = True
            ChDir(Mid(ofdw.FileName, 1, Len(ofdw.FileName) - Len(ofdw.FileName.Split("\").Last)))
            Module1.Main()
            If PublicGateCanPass Then Button3.Enabled = True
        End If

        getTheFilesToBeZipped()
    End Sub

    Private Sub Nexte(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Panel3.Left -= 390
        Panel2.Left -= 390
        Panel6.Left -= 390
        Panel7.Left -= 390
        Panel8.Left -= 390
        Panel9.Left -= 390
        Panel10.Left -= 390
        Panel11.Left -= 390
        Panel12.Left -= 390

        getAllFiles()

        If Panel2.Left = 7 Then : Button4.Enabled = False : Else : Button4.Enabled = True : End If
        If Panel11.Left < 20 Then
            Button3.Enabled = False : polish() : Else : Button3.Enabled = True : End If


    End Sub

    Private Sub Prev(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Panel3.Left += 390
        Panel2.Left += 390
        Panel6.Left += 390
        Panel7.Left += 390
        Panel8.Left += 390
        Panel9.Left += 390
        Panel10.Left += 390
        Panel11.Left += 390
        Panel12.Left += 390

        getAllFiles()
        If Panel2.Left = 7 Then : Button4.Enabled = False : Else : Button4.Enabled = True : End If
        If Panel11.Left < 20 Then : Button3.Enabled = False : Else : Button3.Enabled = True : End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

        If TextBox1.Text.Length < 3 Then Label10.Text = "too short!" : Label10.ForeColor = Color.White : Label10.BackColor = Color.Red : Exit Sub
        Try
            ChDir(RvDir)
            IO.Directory.Move(RvDir & "\levels\" & dirName, RvDir & "\levels\" & TextBox1.Text)
            Dim foo = IO.Directory.GetFiles(RvDir & "\levels\" & TextBox1.Text, dirName & "*")
            ChDir(RvDir & "\levels\" & TextBox1.Text)
            For Each item In foo

                Try
                    IO.File.Move(item.Split("\").Last, Replace(item.Split("\").Last, dirName, TextBox1.Text).ToLower)
                Catch ex As Exception

                End Try
            Next

            Try
                IO.File.Move(RvDir & "\gfx\" & dirName & ".bmp", RvDir & "\gfx\" & TextBox1.Text & ".bmp")
            Catch ex As Exception

            End Try
            dirName = TextBox1.Text
            Label10.Text = "renamed successfully!" : Label10.BackColor = Color.ForestGreen : Label10.ForeColor = Color.Honeydew
        Catch ex As Exception
            Label10.Text = "not renamed!" : Label10.ForeColor = Color.White : Label10.BackColor = Color.Red
            Label20.Text = ex.Message
        End Try
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        Return
        Label17.Hide()
        INF.NAME = TextBox2.Text
        INF.Save()
        Label17.Show()

    End Sub
    Sub getAllFiles()
        Me.CheckedListBox1.Items.Clear()
        Me.CheckedListBox1.Items.AddRange(Cleaner.getUncessaryFiles.Items)
        Me.CheckedListBox1.Items.AddRange(Cleaner.NcpMatchPrm.Items)
        Me.CheckedListBox1.Items.AddRange(Cleaner.PRM_Clean.Items)

      

        For i = 0 To Me.CheckedListBox1.Items.Count - 1
            Me.CheckedListBox1.SetItemChecked(i, True)
        Next
    End Sub
    Sub getTheFilesToBeZipped()

        Me.CheckedListBox2.Items.Clear()

        Dim xo = IO.Directory.GetDirectories(RvDir & "\levels\" & dirName)
        For i = 0 To xo.Length - 1
            CheckedListBox2.Items.Add(Split(xo(i), "\").Last & "\", True)
        Next


        xo = IO.Directory.GetFiles(RvDir & "\levels\" & dirName)
        For i = 0 To xo.Length - 1
            CheckedListBox2.Items.Add(Split(xo(i), "\").Last, True)
        Next




    End Sub
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        If MsgBox("Are you positive positive that you want to remove these files??", MsgBoxStyle.YesNoCancel, "Question") = MsgBoxResult.Yes Then
            For i = 0 To Me.CheckedListBox1.CheckedItems.Count - 1
                Try

                    'IO.File.Delete(RvDir & "\levels\" & dirName & "\" & CheckedListBox1.CheckedItems(0))
                    My.Computer.FileSystem.DeleteFile(RvDir & "\levels\" & dirName & "\" & CheckedListBox1.CheckedItems(0), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    CheckedListBox1.Items.Remove(CheckedListBox1.CheckedItems(0))
                Catch ex As Exception

                End Try
            Next
        End If

        getTheFilesToBeZipped()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Dim beingadded = False
    Private Sub NumericUpDown11_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown11.ValueChanged
        While beingadded
            Threading.Thread.Sleep(1)
        End While

        beingadded = True

        Fob = New FOB_File(FullPath & dirName & ".fob")
        Dim rains = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_RAIN)
        For i = 0 To rains.Count - 1
            Try

                Fob.ObjList.RemoveAt(rains(i))
            Catch ex As Exception

            End Try
        Next

        For i = 0 To NumericUpDown11.Value - 1
            Fob.ObjList.Add(New FILE_OBJECT(OBJ_ID_TYPE.OBJECT_TYPE_RAIN, New Integer() {0, 0, 0, 0}, _
                                                    INF.STARTPOS - New Vector3D(20, 80, 20 * i), New Vector3D(0, 1, 0), New Vector3D(0, 0, 1)))


        Next


        Fob.Save()
        beingadded = False

    End Sub

    Private Sub Panel7_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel7.Paint

    End Sub

    Private Sub Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Fob = New FOB_File(FullPath & dirName & ".fob")
        Dim skyboxes = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_SKYBOX)
        For i = 0 To skyboxes.Count - 1
            Fob.ObjList.RemoveAt(skyboxes(i))
        Next

        If ComboBox1.SelectedIndex <> 0 Then
            Fob.ObjList.Add(New FILE_OBJECT(OBJ_ID_TYPE.OBJECT_TYPE_SKYBOX, New Integer() {ComboBox1.SelectedIndex - 1, 0, 0, 0}, _
                                            INF.STARTPOS - New Vector3D(0, 40, 0), New Vector3D(0, 1, 0), New Vector3D(0, 0, 1)))


        End If

        Fob.Save()
    End Sub
    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        While beingadded
            Threading.Thread.Sleep(1)
        End While

        beingadded = True

        Fob = New FOB_File(FullPath & dirName & ".fob")
        Dim rains = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_LIGHTNING)
        For i = 0 To rains.Count - 1
            Try

                Fob.ObjList.RemoveAt(rains(i))
            Catch ex As Exception

            End Try
        Next

        For i = 0 To NumericUpDown1.Value - 1
            Fob.ObjList.Add(New FILE_OBJECT(OBJ_ID_TYPE.OBJECT_TYPE_LIGHTNING, New Integer() {0, 0, 0, 0}, _
                                                    INF.STARTPOS - New Vector3D(200, 160, 20 * i), New Vector3D(0, 1, 0), New Vector3D(0, 0, 1)))


        Next


        Fob.Save()
        beingadded = False

    End Sub



    Public Sub Button5_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        '--------------------------------- score: 40*10


        Dim foo = IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmp")
        Dim fs As FileStream
        Dim bmp As Bitmap
        Dim avgR, avgG, avgB As Double
        Dim stdev As Double = 0

        For i = 0 To foo.Count - 1
            fs = New IO.FileStream(foo(i), FileMode.Open, FileAccess.Read)
            bmp = Bitmap.FromStream(fs)
            avgR = 0 : avgB = 0 : avgG = 0
            stdev = 0
            For j = 0 To bmp.Width - 1
                For k = 0 To bmp.Height - 1
                    Dim fooo = bmp.GetPixel(j, k)
                    avgR += fooo.R / (bmp.Width * bmp.Height)
                    avgG += fooo.G / (bmp.Width * bmp.Height)
                    avgB += fooo.B / (bmp.Width * bmp.Height)


                Next
            Next

            For j = 0 To bmp.Width - 1
                For k = 0 To bmp.Height - 1
                    Dim fooo = bmp.GetPixel(j, k)
                    stdev += (fooo.R - avgR) ^ 2 + (fooo.B - avgB) ^ 2 + (fooo.G - avgG) ^ 2
                Next
            Next
            stdev = (stdev / (bmp.Width * bmp.Height)) ^ 0.5

            Application.DoEvents()

            ' ListBox3.Items.Add("bitmap " & foo(i).Split("\").Last & " ( " & bmp.Width & " x " & bmp.Height & ") , format: " & bmp.PixelFormat.ToString().Replace("Format", ""))

            If stdev > 200 Then
                ListBox3.Items.Add(bmp.Width & "x" & bmp.Height & " " & bmp.PixelFormat.ToString().Replace("Format", "").Replace("Bpp", "").Replace("Rgb", "") & " " & foo(i).Split("\").Last & " has a GREAT texture repartition, superb job!")
                If canscore2 Then score += 45
            ElseIf stdev > 100 Then
                ListBox3.Items.Add(bmp.Width & "x" & bmp.Height & " " & bmp.PixelFormat.ToString().Replace("Format", "").Replace("Bpp", "").Replace("Rgb", "") & " " & foo(i).Split("\").Last & " has a good texture repartition, superb job!")
                If canscore2 Then score += 40
            ElseIf stdev > 50 Then
                ListBox3.Items.Add(bmp.Width & "x" & bmp.Height & " " & bmp.PixelFormat.ToString().Replace("Format", "").Replace("Bpp", "").Replace("Rgb", "") & " " & foo(i).Split("\").Last & " has a fair texture repartition...")
                If canscore2 Then score += 30
            ElseIf stdev > 30 Then
                ListBox3.Items.Add(bmp.Width & "x" & bmp.Height & " " & bmp.PixelFormat.ToString().Replace("Format", "").Replace("Bpp", "").Replace("Rgb", "") & " " & foo(i).Split("\").Last & " has a repartition not that good...")
                If canscore2 Then score += 25
            ElseIf stdev > 20 Then
                ListBox3.Items.Add(bmp.Width & "x" & bmp.Height & " " & bmp.PixelFormat.ToString().Replace("Format", "").Replace("Bpp", "").Replace("Rgb", "") & " " & foo(i).Split("\").Last & " has a repartition that's not good...")
                If canscore2 Then score += 15
            Else

                ListBox3.Items.Add("bitmap " & foo(i).Split("\").Last & " has BAD texture repartition!!! Badly used")
                If canscore2 Then score += 10
            End If

        Next
        canscore2 = False 'no can't score anymore!
        Button5.Hide()
    End Sub

    Private Sub Button6_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If canScore Then canScore = False : score += 5

        If IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmq").Count <> 0 Then
            If MsgBox("We have detected that bmq mipmaps already exist there, delete and recreate?", MsgBoxStyle.YesNoCancel, "BMQ detected") <> MsgBoxResult.Yes Then Exit Sub
        End If

        Dim foo = IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmp")
        Dim fs As FileStream
        Dim bmp As Bitmap


        For i = 0 To foo.Count - 1
            Try
                ListBox5.Items.Add("started -> " & foo(i).Split("\").Last)
                fs = New IO.FileStream(foo(i), FileMode.Open, FileAccess.Read)
                bmp = Bitmap.FromStream(fs)
                ' gfx = Graphics.FromImage(bmp)
                ' gfx.SmoothingMode = Drawing2D.SmoothingMode.HighQuality


                ' ( gfx.ScaleTransform(0.5, 0.5)
                '  gfx.Save()
                Dim bmq = bmp.GetThumbnailImage(bmp.Width / 2, bmp.Height / 2, Nothing, Nothing)
                Try : My.Computer.FileSystem.DeleteFile(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".bmp", ".bmq")) : Catch : End Try
                bmq.Save(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".bmp", ".bmq"), Imaging.ImageFormat.Bmp)
                ListBox5.Items.Add("finished -> " & foo(i).Split("\").Last)
                ListBox5.SelectedIndex = ListBox5.Items.Count - 1
            Catch ex As Exception
                ListBox4.Items.Add(foo(i).Split("\").Last & ":" & ex.Message)
                Label28.Text = "errors (" & ListBox4.Items.Count & "): "
            End Try

        Next
        ListBox5.Items.Add("Done!")
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmr").Count <> 0 Then
            If MsgBox("We have detected that bmr mipmaps already exist there, delete and recreate?", MsgBoxStyle.YesNoCancel, "BMR detected") <> MsgBoxResult.Yes Then Exit Sub
        End If

        Dim foo = IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmp")
        Dim fs As FileStream
        Dim bmp As Bitmap


        For i = 0 To foo.Count - 1
            Try
                ListBox5.Items.Add("started -> " & foo(i).Split("\").Last)
                fs = New IO.FileStream(foo(i), FileMode.Open, FileAccess.Read)
                bmp = Bitmap.FromStream(fs)
                ' gfx = Graphics.FromImage(bmp)
                ' gfx.SmoothingMode = Drawing2D.SmoothingMode.HighQuality


                ' ( gfx.ScaleTransform(0.5, 0.5)
                '  gfx.Save()
                Dim bmq = bmp.GetThumbnailImage(bmp.Width / 4, bmp.Height / 4, Nothing, Nothing)
                Try : My.Computer.FileSystem.DeleteFile(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".bmp", ".bmr")) : Catch : End Try
                bmq.Save(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".bmp", ".bmr"), Imaging.ImageFormat.Bmp)
                ListBox5.Items.Add("finished -> " & foo(i).Split("\").Last)
                ListBox5.SelectedIndex = ListBox5.Items.Count - 1
            Catch ex As Exception
                ListBox4.Items.Add(foo(i).Split("\").Last & ":" & ex.Message)
                Label28.Text = "errors (" & ListBox4.Items.Count & "): "
            End Try

        Next
        ListBox5.Items.Add("Done!")
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
   

        Dim foo = IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmo")
        Dim fs As FileStream
        Dim bmp As Bitmap


        For i = 0 To foo.Count - 1
            Try
                ListBox5.Items.Add("started -> " & foo(i).Split("\").Last)
                fs = New IO.FileStream(foo(i), FileMode.Open, FileAccess.Read)
                bmp = Bitmap.FromStream(fs)
                ' gfx = Graphics.FromImage(bmp)
                ' gfx.SmoothingMode = Drawing2D.SmoothingMode.HighQuality


                ' ( gfx.ScaleTransform(0.5, 0.5)
                '  gfx.Save()
                Dim bmq = bmp.GetThumbnailImage(bmp.Width / 2, bmp.Height / 2, Nothing, Nothing)
                Try : My.Computer.FileSystem.DeleteFile(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".bmo", ".bmp")) : Catch : End Try
                bmq.Save(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".bmo", ".bmp"), Imaging.ImageFormat.Bmp)
                ListBox5.Items.Add("finished -> " & foo(i).Split("\").Last)
                ListBox5.SelectedIndex = ListBox5.Items.Count - 1
            Catch ex As Exception
                ListBox4.Items.Add(foo(i).Split("\").Last & ":" & ex.Message)
                Label28.Text = "errors (" & ListBox4.Items.Count & "): "
            End Try

        Next
        ListBox5.Items.Add("Done!")
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click


        Dim foo = IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmo")
        Dim fs As FileStream
        Dim bmp As Bitmap


        For i = 0 To foo.Count - 1
            Try
                ListBox5.Items.Add("started -> " & foo(i).Split("\").Last)
                fs = New IO.FileStream(foo(i), FileMode.Open, FileAccess.Read)
                bmp = Bitmap.FromStream(fs)
                ' gfx = Graphics.FromImage(bmp)
                ' gfx.SmoothingMode = Drawing2D.SmoothingMode.HighQuality


                ' ( gfx.ScaleTransform(0.5, 0.5)
                '  gfx.Save()
                Dim bmq = bmp.GetThumbnailImage(bmp.Width / 2, bmp.Height / 2, Nothing, Nothing)
                Try : My.Computer.FileSystem.DeleteFile(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".bmo", ".jpg")) : Catch : End Try
                bmq.Save(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".bmo", ".jpg"), Imaging.ImageFormat.Jpeg)
                ListBox5.Items.Add("finished -> " & foo(i).Split("\").Last)
                ListBox5.SelectedIndex = ListBox5.Items.Count - 1
            Catch ex As Exception
                ListBox4.Items.Add(foo(i).Split("\").Last & ":" & ex.Message)
                Label28.Text = "errors (" & ListBox4.Items.Count & "): "
            End Try

        Next
        ListBox5.Items.Add("Done!")
        foo = IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.jpg")
        For i = 0 To foo.Count - 1
            Try
                ListBox5.Items.Add("started -> " & foo(i).Split("\").Last)
                fs = New IO.FileStream(foo(i), FileMode.Open, FileAccess.Read)
                bmp = Bitmap.FromStream(fs)
                ' gfx = Graphics.FromImage(bmp)
                ' gfx.SmoothingMode = Drawing2D.SmoothingMode.HighQuality


                ' ( gfx.ScaleTransform(0.5, 0.5)
                '  gfx.Save()
                Dim bmq = bmp.GetThumbnailImage(bmp.Width, bmp.Height, Nothing, Nothing)
                Try : My.Computer.FileSystem.DeleteFile(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".jpg", ".bmp")) : Catch : End Try
                bmq.Save(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last.Replace(".jpg", ".bmp"), Imaging.ImageFormat.Bmp)
                ListBox5.Items.Add("finished -> " & foo(i).Split("\").Last)
                ListBox5.SelectedIndex = ListBox5.Items.Count - 1
            Catch ex As Exception
                ListBox4.Items.Add(foo(i).Split("\").Last & ":" & ex.Message)
                Label28.Text = "errors (" & ListBox4.Items.Count & "): "
            End Try

        Next

        For i = 0 To foo.Count - 1
            Try : My.Computer.FileSystem.DeleteFile(RvDir & "\levels\" & dirName & "\" & Split(foo(i), "\").Last) : Catch : End Try

        Next
        ListBox5.Items.Add("Done!")
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click

        If IO.Directory.GetFiles(RvDir & "\levels\" & dirName, "readme.txt").Count > 0 Then
            If MsgBox("We have detected a readme file, overwrite?", MsgBoxStyle.YesNoCancel, "Overwrite readme") <> MsgBoxResult.Yes Then Exit Sub
        End If
        readme_generator.UpdateThis()
        readme_generator.Show()



    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        TrackCompress()
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If ofdpic.ShowDialog = Windows.Forms.DialogResult.OK Then
         
            Dim fstr As New IO.FileStream(ofdpic.FileName, FileMode.Open, FileAccess.Read)
            Dim bmp As Bitmap
            bmp = Bitmap.FromStream(fstr)

            'delete old file
            Try
                My.Computer.FileSystem.DeleteFile(RvDir & "\gfx\" & dirName & ".bmp", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                My.Computer.FileSystem.DeleteFile(RvDir & "\gfx\" & dirName & ".bmq", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            Catch ex As Exception

            End Try


            Try
                If RadioButton2.Checked Then

                    Dim bmpp = bmp.GetThumbnailImage(256, 256, Nothing, Nothing)
                    bmpp.Save(RvDir & "\gfx\" & dirName & ".bmp")
                    bmpp = bmp.GetThumbnailImage(128, 128, Nothing, Nothing)
                    bmpp.Save(RvDir & "\gfx\" & dirName & ".bmq", Imaging.ImageFormat.Bmp)
                Else

                    Dim bmpp = bmp.GetThumbnailImage(512, 512, Nothing, Nothing)
                    bmpp.Save(RvDir & "\gfx\" & dirName & ".bmp")
                    bmpp = bmp.GetThumbnailImage(256, 256, Nothing, Nothing)
                    bmpp.Save(RvDir & "\gfx\" & dirName & ".bmq", Imaging.ImageFormat.Bmp)
                End If

                fstr.Close()

                fstr = New IO.FileStream(RvDir & "\gfx\" & dirName & ".bmp", FileMode.Open, FileAccess.Read)
                PictureBox1.Image = Image.FromStream(fstr)
                fstr.Close()



            Catch ex As Exception
                Label30.Text = ex.Message
            End Try
        End If
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

  
    Private Sub Button13_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        Dim foo = IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmr")
        For i = 0 To foo.Count - 1
            Try
                ListBox5.Items.Add("removing " & Split(foo(i), "\").Last)
                My.Computer.FileSystem.DeleteFile(foo(i))
            Catch ex As Exception
                ListBox4.Items.Add(Split(foo(i), "\").Last & ": " & ex.Message)

            End Try



        Next
    End Sub

    Private Sub Label42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Shell("explorer.exe ""http://revoltzone.net/upload""", AppWinStyle.NormalFocus)
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Shell("explorer.exe ""http://z3.invisionfree.com/Revolt_Live/index.php?act=Post&CODE=00&f=8""", AppWinStyle.NormalFocus)

    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        Button5_Click_1(sender, e)
        polish()
    End Sub

    

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            For i = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemCheckState(i, CheckState.Checked)
            Next
        Else
            For i = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
            Next
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        For i = 0 To CheckedListBox2.Items.Count - 1
            CheckedListBox2.SetItemChecked(i, CheckBox3.Checked)
        Next
    End Sub
End Class
