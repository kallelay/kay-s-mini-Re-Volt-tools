Public Class Normal
    Dim step_ As Integer = 0
    Private Sub Normal_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        End
    End Sub

    Private Sub Normal_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

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
            TextBox7.Text = GetSetting("LiveInstaller", "settings", "dir", "") & "\levels"
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

    Private Sub ListView1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ListView1.KeyPress
        If e.KeyChar = Chr(13) Then
            ListView1_DoubleClick(sender, e)
        ElseIf e.KeyChar = Chr(8) Then
            TextBox7.Text &= "\..\"
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

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


        End If

       
    End Sub

    Private Sub Normal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        step_ += 1
        If step_ = 1 Then
            ChDir(TextBox7.Text)
            ChDir("..\..\")
            Fname = TextBox7.Text.Split("\").Last
            RvDir = CurDir()
            TextBox4.Text = GetLength()
            TextBox3.Text = CountVer()
            PictureBox2.ImageLocation = RvDir & "\gfx\" & Fname & ".bmp"
            TextBox1.Text = getName()
            Panel2.Location = Panel1.Location
            Button1.Enabled = False
        ElseIf step_ = 2 Then
           
            Application.DoEvents()
            Form2.TopMost = True
            Form2.Show()
            Form2.OK()

        End If



    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If step_ = 0 Then
            Button2.Enabled = False
        Else
            Button2.Enabled = True
        End If

        If step_ = 0 Then
            If IO.File.Exists(TextBox7.Text & "\" & TextBox7.Text.Split("\").Last & ".w") And _
       IO.File.Exists(TextBox7.Text & "\" & TextBox7.Text.Split("\").Last & ".inf") And _
IO.File.Exists(TextBox7.Text & "\" & TextBox7.Text.Split("\").Last & ".ncp") Then

                Button1.Enabled = True
            Else
                Button1.Enabled = False
            End If

        End If
        If step_ = 1 Then
            If TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" And ComboBox1.Text <> "" Then
                Button1.Enabled = True
            Else
                Button1.Enabled = False
            End If
        End If

        If step_ = 2 Then
            Button1.Enabled = False
        Else

        End If


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        If step_ = 1 Then
            Panel2.Left = 50000
        ElseIf step_ = 2 Then
            Panel3.Left = 50000
            Panel2.Left = Panel1.Left

        End If
        step_ -= 1
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        End
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class