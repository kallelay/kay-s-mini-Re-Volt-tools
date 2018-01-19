''-----------------------------------------------------------------------------------------
'' This document is a part of CAR_WINDOW " Special Shade " Program by Kallel Ahmed Yahia
''         All rights reserved (c) Kallel Ahmed Yahia 2012-2014
''   LIcensed under GNU GPL v3
''------------------------------------------------------------------------------------------
Public Class Form1
    Dim bmp = ""

    'Load texture
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then 'alright then everything is ok
            bmp = ofd.FileName 'get filename
            Panel1.BackgroundImage = Image.FromFile(bmp) 'and load it

        End If
    End Sub
    Dim clicked = False 'clicked
    Dim started = False 'started dragging
    Dim approX, approY As Integer 'The positions 

    Private Sub Panel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel1.Click, Panel4.Click
        'just a click then
        If clicked Then Exit Sub
        Panel4.Size = New Size 'otherwise, clear the panel4
        Panel4.Location = New Point
    End Sub
    'Activate Clicked
    Private Sub Panel1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseDown, Panel4.MouseDown, Panel22.MouseDown
        clicked = True
        If sender Is Panel4 Then Panel4.Size = New Size
    End Sub
    'Activate Panel4's resizing to get the texture UV bounds
    Private Sub Panel1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel22.MouseMove, Panel1.MouseMove

        approX = e.X - If(sender Is Panel22, 11, 0)
        approY = e.Y - If(sender Is Panel22, 9, 0)
        If clicked Then
            If Not started Then
                'If e.X - 2 < 0 Then
                'approX = 0
                ' End If
                'If e.Y - 2 < 0 Then
                'approY = 0
                'End If


                Panel4.Location = New Point(approX, approY)
                started = True
            Else
                Panel4.Size = e.Location - Panel4.Location


            End If
        End If
    End Sub
    'Dragging finished
    Private Sub Panel1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseUp, Panel4.MouseUp, Panel22.MouseUp
        clicked = False
        started = False
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click


        '  For i = 0 To Panel2.Width \ 2 + 2
        ' Application.DoEvents()
        '  Threading.Thread.Sleep(1)
        Panel2.Left -= Panel2.Width + 6 'next menu
        Panel3.Left -= Panel2.Width + 6
        Panel5.Left -= Panel2.Width + 6
        '   Next

        Panel13.BackgroundImageLayout = ImageLayout.Stretch 'make sure it's stretched
        Panel13.BackgroundImage = Panel1.BackgroundImage 'load picture to

        'Now obscure all except the new region selected in the previous mode
        Panel9.Location = New Point(0, 0)
        Panel9.Size = New Point(Panel1.Width, Panel4.Top)

        Panel6.Location = New Point(0, 0)
        Panel6.Size = New Point(Panel4.Left, Panel1.Height)

        Panel7.Location = New Point(0, Panel4.Top + Panel4.Height)
        Panel7.Size = New Point(Panel1.Width, Panel1.Height - Panel4.Height)

        Panel8.Location = New Point(Panel4.Left + Panel4.Width, 0)
        Panel8.Size = New Point(Panel1.Width - Panel4.Height, Panel1.Height)



    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        ' For i = 0 To Panel2.Width \ 2 + 2
        'Application.DoEvents()
        '  Threading.Thread.Sleep(1)
        Panel2.Left += Panel2.Width + 6
        Panel3.Left += Panel2.Width + 6
        Panel5.Left += Panel2.Width + 6
        '  Next
    End Sub
    Dim prm = ""
    Dim myprm As Car_Model
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If prm_ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            prm = prm_ofd.FileName 'open file
            myprm = New Car_Model(prm) 'load prm file

        End If
    End Sub

    Dim minU, minV, maxU, maxV As Single
    Dim uv_loc As vector
    Dim uv_Finish As vector

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'For i = 0 To Panel2.Width \ 2 + 2
        'Application.DoEvents()
        ' Threading.Thread.Sleep(1)
        Panel2.Left -= Panel2.Width + 6
        Panel3.Left -= Panel2.Width + 6
        Panel5.Left -= Panel2.Width + 6
        '  Next

        log("starting...")
        log("Calculating UV:")
        uv_loc = New vector(Panel4.Left / 256, Panel4.Top / 256)
        uv_Finish = New vector((Panel4.Width + Panel4.Left) / 256, (Panel4.Height + Panel4.Top) / 256)

        log("UV 1:" & uv_loc.ToString)
        log("UV 2:" & uv_Finish.ToString)

        log("Searching ...")
        Polies = New List(Of Integer)
        If myprm Is Nothing Then GoTo SKIP
        If myprm.MyModel Is Nothing Then GoTo SKIP


        BackgroundWorker1.RunWorkerAsync() 'triple thread
        BackgroundWorker2.RunWorkerAsync()
        For i = 0 To myprm.MyModel.polynum - 1 Step 3
            'First, calculating the box!
            DA_WORK(i)

        Next
SKIP:
        If Polies.Count = 0 Then log("AAAArgh!!!! Can't find any!!!!!!!!!!!!!1")
        log("finished!")
    End Sub
    Sub DA_WORK(ByVal i%)
        Dim minU, minV, maxU, maxV

        If myprm.MyModel.polyl(i).type And 1 Then 'it's quad
            minU = Math.Min(Math.Min(myprm.MyModel.polyl(i).u0, myprm.MyModel.polyl(i).u1), Math.Min(myprm.MyModel.polyl(i).u2, myprm.MyModel.polyl(i).u3))
            minV = Math.Min(Math.Min(myprm.MyModel.polyl(i).v0, myprm.MyModel.polyl(i).v1), Math.Min(myprm.MyModel.polyl(i).v2, myprm.MyModel.polyl(i).v3))
            maxU = Math.Max(Math.Max(myprm.MyModel.polyl(i).u0, myprm.MyModel.polyl(i).u1), Math.Max(myprm.MyModel.polyl(i).u2, myprm.MyModel.polyl(i).u3))
            maxV = Math.Max(Math.Max(myprm.MyModel.polyl(i).v0, myprm.MyModel.polyl(i).v1), Math.Max(myprm.MyModel.polyl(i).v2, myprm.MyModel.polyl(i).v3))
        Else
            minU = Math.Min(Math.Min(myprm.MyModel.polyl(i).u0, myprm.MyModel.polyl(i).u1), myprm.MyModel.polyl(i).u2)
            minV = Math.Min(Math.Min(myprm.MyModel.polyl(i).v0, myprm.MyModel.polyl(i).v1), myprm.MyModel.polyl(i).v2)
            maxU = Math.Max(Math.Max(myprm.MyModel.polyl(i).u0, myprm.MyModel.polyl(i).u1), myprm.MyModel.polyl(i).u2)
            maxV = Math.Max(Math.Max(myprm.MyModel.polyl(i).v0, myprm.MyModel.polyl(i).v1), myprm.MyModel.polyl(i).v2)

        End If

        'Second, check if it's in the box
        If minU >= uv_loc.x And minV >= uv_loc.y Then
            If maxU <= uv_Finish.x And maxV <= uv_Finish.y Then
                log("found :" & i)
                Polies.Add(i)
            End If
        End If
        minU = Nothing : minV = Nothing : maxU = Nothing : maxV = Nothing

    End Sub
    Dim Polies As New List(Of Integer)
    Class vector 'vector  = pointf
        Public x, y As Single
        Sub New()
            x = 0
            y = 0
        End Sub
        Sub New(ByVal x_!, ByVal y_!)
            x = x_
            y = y_
        End Sub
        Overrides Function ToString() As String
            Return Strings.Format(x, "0.#0") & "x" & Strings.Format(y, "0.#0")

        End Function
    End Class

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        Label6.Text = "opacity is " & TrackBar1.Value & "%"
    End Sub
    Dim mcolor As Color = Color.White 'init to white 
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If clr.ShowDialog = Windows.Forms.DialogResult.OK Then
            mcolor = Color.FromArgb(TrackBar1.Value * 255 / 100, clr.Color)
            log("chosen color : " & mcolor.ToString)
        End If
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        log("I'll be coloring them then!")
        For i = 0 To Polies.Count - 1
            'translucent (obligatory)

            myprm.MyModel.polyl(Polies.Item(i)).type = myprm.MyModel.polyl(Polies.Item(i)).type Or 4
            If CheckBox2.Checked Then
                If Not (myprm.MyModel.polyl(Polies.Item(i)).type And 2) Then
                    'double sided (optional)
                    myprm.MyModel.polyl(Polies.Item(i)).type = myprm.MyModel.polyl(Polies.Item(i)).type Or 2
                End If
            End If
            mcolor = Color.FromArgb(TrackBar1.Value * 255 / 100, mcolor) '(force) Set alpha from trackbar

            myprm.MyModel.polyl(Polies.Item(i)).c0 = RGBToLong(mcolor)
            myprm.MyModel.polyl(Polies.Item(i)).c1 = RGBToLong(mcolor)
            myprm.MyModel.polyl(Polies.Item(i)).c2 = RGBToLong(mcolor)
            myprm.MyModel.polyl(Polies.Item(i)).c3 = RGBToLong(mcolor)
            '  MsgBox(ColorsToRGB(myprm.MyModel.polyl(Polies.Item(i)).c2).ToString)
        Next
        log("And finished!")
        log("so, first I'll back up the old file")
        My.Computer.FileSystem.CopyFile(myprm.FileName, myprm.FileName & ".bck", True)
        log("exporting!")
        myprm.Export(myprm.FileName)
        log("DONE!")
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        'For i = 0 To Panel2.Width \ 2 + 2
        ' Application.DoEvents()
        ' Threading.Thread.Sleep(1)
        Panel2.Left += Panel2.Width + 6
        Panel3.Left += Panel2.Width + 6
        Panel5.Left += Panel2.Width + 6
        '  Next
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'For i = 0 To Panel2.Width \ 2 + 2
        Application.DoEvents()
        '  Threading.Thread.Sleep(1)
        Panel2.Left -= Panel2.Width + 6
        Panel3.Left -= Panel2.Width + 6
        Panel5.Left -= Panel2.Width + 6
        ' Next
    End Sub

    Private Sub Panel22_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel22.MouseDown

    End Sub



    Private Sub Panel22_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel22.Paint

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Width = 314
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        For i = 1 To myprm.MyModel.polynum - 1 Step 3
            DA_WORK(i)
        Next
    End Sub

    Private Sub BackgroundWorker2_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        For i = 2 To myprm.MyModel.polynum - 1 Step 3
            DA_WORK(i)
        Next
    End Sub

    Private Sub Panel3_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel3.Paint

    End Sub
End Class
