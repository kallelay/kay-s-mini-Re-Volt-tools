Imports System.Math
Imports getUV.Car_Model

Public Class Form1

    Public Const oo = Single.PositiveInfinity
    Dim rvpath$ = ""
    Dim Angle = PI / 4
    'Easter egg
    Dim Sprites As New List(Of Point)
    Dim iSprites As New List(Of Point)
    Sub InitSprites()

        Randomize()
        For i = 0 To 100
            Dim randY = Rnd() * 200 - 200
            Dim randX = Rnd() * Me.Width
            Sprites.Add(New Point(randX, randY))
            iSprites.Add(New Point(randX, randY))
        Next
    End Sub
    Dim t = 0
    Sub UpdateSprites()
        t += 1
        For i = 0 To Sprites.Count - 1
            If Sprites(i).Y = Me.Height - 40 Then
                Sprites(i) = New Point(Sprites(i).X, Sprites(i).Y + 1)
                Sprites.Add(New Point(Sprites(i).X, 0))
            ElseIf Sprites(i).Y > Me.Height - 40 Then
            Else

                Sprites(i) = New Point(Sprites(i).X + 2 * Cos(t), +Sprites(i).Y + 1)
            End If


        Next
    End Sub
    Sub DrawSprites(ByVal e As Graphics)

        Me.CreateGraphics.Clear(Color.AliceBlue)
        For i = 0 To Sprites.Count - 1
            e.DrawRectangle(Pens.Black, New Rectangle(Sprites(i), New Size(2, 2)))
        Next
        e.Dispose()
    End Sub

    Private Sub Form1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick

        If Not (Now.Month = 12 Or Now.Month = 1 Or Now.Month = 2) Then
            Exit Sub
        End If

        If Not Timer1.Enabled Then
            Timer2.Start()
        Else
            Timer1.Stop()
        End If

    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DoWrite("Seeking Car::Load...")
        If GetSetting("Car Load", "settings", "dir", "") = "" Then
            DoWrite("   Not found")
        Else
            rvpath = GetSetting("Car Load", "settings", "dir", "")
            DoWrite("   Found: you can use %rvdir%'s method")
        End If

        DoWrite("Searching Colors")
        Panel2.BackColor = Color.FromArgb(GetSetting("KDL", "UV", "line.R", 0), GetSetting("KDL", "UV", "line.G", 0), GetSetting("KDL", "UV", "line.B", 0))
        LINECOLOR = Panel2.BackColor
        Panel3.BackColor = Color.FromArgb(GetSetting("KDL", "UV", "back.R", 255), GetSetting("KDL", "UV", "back.G", 255), GetSetting("KDL", "UV", "back.B", 255))
        Panel1.BackColor = Panel3.BackColor



        DoWrite("       version:" & "1.0")


        TextBox2.Text = GetSetting("KDL", "UV", "latest", If((rvpath) <> "", "%rvdir%\cars\adeon\body.prm", ""))
        DoWrite("Advanced mode?")
        CheckBox2.Checked = CBool(GetSetting("KDL", "UV", "advanced", "false"))
        CheckBox2_CheckedChanged(sender, e)

        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.DoubleBuffer, True)


        'Bonus
        '   FullLoaded = True
     
    End Sub
    Dim fullLoaded = False
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If IO.File.Exists(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text)) Then

            If LCase(Strings.Right(TextBox2.Text, 3)) = "prm" Or LCase(Strings.Right(TextBox2.Text, 2)) = ".m" Then
                TextBox2.BackColor = Color.Lime
                SaveSetting("KDL", "UV", "latest", TextBox2.Text)
            Else
                TextBox2.BackColor = Color.LightPink

            End If

        Else
            TextBox2.BackColor = Color.LightPink
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox2.Text = OpenFileDialog1.FileName
            Button1_Click(sender, e)
        End If
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        Application.DoEvents()
        TextBox3.Visible = True
        If TextBox2.BackColor <> Color.Lime Then
            DoWrite("Sorry, not valid")
            TextBox3.Visible = False
            Exit Sub
        End If
        Dim prm As New Car_Model(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text))


        Dim zoom As Single = 2.5
        Dim Pan = 180




        Dim n, ts As New ListBox
        DoWrite("Init GDI+")

        Form2.Show()

        Dim g = Form2.CreateGraphics

        g.Clear(Color.AliceBlue)

        DoWrite("Fixing Positions")

        'For q = 0 To prm.MyModel.vertnum - 1
        'prm.MyModel.vexl(q).Position *= zoom
        ' Next



        DoWrite("Yessir! Rendering in GDI+!" & vbNewLine)

        '  DoWrite("Calculating Light Position and Normal")

        '  Dim LightPos = New Vector3D(NumericUpDown2.Value, NumericUpDown3.Value, NumericUpDown4.Value)
        '  Dim LightNormalAbs = Sqrt(LightPos.x ^ 2 + LightPos.y ^ 2 + LightPos.z)
        '  Dim LightNor = New Vector3D(LightPos.x / LightNormalAbs, LightPos.y / LightNormalAbs, LightPos.z / LightNormalAbs)

        '  DoWrite("  Light Position:" & LightPos.ToString & vbNewLine)



        DoWrite("Calculating Lighting for each Vertex (Each alone)")
        For k = 0 To prm.MyModel.polynum - 1
            If prm.MyModel.polyl(k).type And 1 Then
                Dim pnts(3) As Point
                pnts(0) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(1) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(2) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(3) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)






                Dim Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y + 1


                g.DrawLines(Pens.Black, pnts)



            Else
                Dim pnts(2) As Point
                pnts(0) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(1) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(2) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)

                Dim Normal = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal) / 4
                Dim Center = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position) / 4






                Dim Inte As Single = prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y + 1

                g.DrawPolygon(Pens.Black, pnts)


            End If

        Next

        g.Save()


        DoWrite("Ending scene")
        TextBox3.Visible = False
        g.Dispose()

    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub
    Dim UV As New List(Of Vector2D)
    Dim RenderUV As New List(Of Point)
    Dim V As Car_Model
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        TextBox3.Visible = True
        If TextBox2.BackColor <> Color.Lime Then
            Console.Beep()
            TextBox3.Visible = False
            Exit Sub
        End If

        UV.Clear()
        RenderUV.Clear()

        DoWrite("Loading prm")

        V = New Car_Model(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text))

        DoWrite("Extracting UV")
        For i = 0 To V.MyModel.polynum - 1
            RenderUV.Add(New Point(V.MyModel.polyl(i).u0 * 256, V.MyModel.polyl(i).v0 * 256))
            RenderUV.Add(New Point(V.MyModel.polyl(i).u1 * 256, V.MyModel.polyl(i).v1 * 256))
            RenderUV.Add(New Point(V.MyModel.polyl(i).u1 * 256, V.MyModel.polyl(i).v1 * 256))
            RenderUV.Add(New Point(V.MyModel.polyl(i).u2 * 256, V.MyModel.polyl(i).v2 * 256))
            RenderUV.Add(New Point(V.MyModel.polyl(i).u2 * 256, V.MyModel.polyl(i).v2 * 256))


            '   UV.Add(New Vector2D(V.MyModel.polyl(i).u0, V.MyModel.polyl(i).v0))
            '  UV.Add(New Vector2D(V.MyModel.polyl(i).u1, V.MyModel.polyl(i).v1))
            ' UV.Add(New Vector2D(V.MyModel.polyl(i).u2, V.MyModel.polyl(i).v2))

            If V.MyModel.polyl(i).type And 1 Then
                '   UV.Add(New Vector2D(V.MyModel.polyl(i).u3, V.MyModel.polyl(i).v3))
                RenderUV.Add(New Point(V.MyModel.polyl(i).u3 * 256, V.MyModel.polyl(i).v3 * 256))
                RenderUV.Add(New Point(V.MyModel.polyl(i).u3 * 256, V.MyModel.polyl(i).v3 * 256))

            End If
            RenderUV.Add(New Point(V.MyModel.polyl(i).u0 * 256, V.MyModel.polyl(i).v0 * 256))

            '    UV.Add(New Vector2D(V.MyModel.polyl(i).u0, V.MyModel.polyl(i).v0))
            '    If Not V.MyModel.polyl(i).type And 1 Then
            '   UV.Add(New Vector2D(-50, -50)) 'the limiter

            '    End If
        Next

        DoWrite("Saving UV coordinates")

        DoWrite("Rendering UV coordinates")

        DoWrite("   Translating to GDI+")
        ' TranslateToGDIp()
        TextBox3.Visible = False
        DoWrite("   Rendering...")
        Draw()




    End Sub


    Sub TranslateToGDIp()
        Dim i As Int16 = 1
        Do Until i >= UV.Count
            If UV(i).x + UV(i).y <> -100 And UV(i - 1).x + UV(i - 1).y <> -100 Then
                '  RenderUV.Add(New Point(Int(UV(i - 1).x * 256), Int(UV(i - 1).y * 256)))
                RenderUV.Add(New Point(Int(UV(i).x * 256), Int(UV(i).y * 256)))
                i += 1
            Else
                i += 3

            End If


        Loop
    End Sub
    Dim LINECOLOR As Color
    Dim BCKCOLOR As Color
    Sub Draw()
        Dim g = Panel1.CreateGraphics
        For i = 0 To RenderUV.Count - 2 Step 2
            g.DrawLine(New Pen(LINECOLOR), RenderUV(i), RenderUV(i + 1))
        Next
        g.Dispose()
    End Sub

    Sub Draw(ByVal g As Graphics)
        'Dim g = Panel1.CreateGraphics
        For i = 0 To RenderUV.Count - 2 Step 2
            g.DrawLine(New Pen(LINECOLOR), RenderUV(i), RenderUV(i + 1))
        Next
        g.Dispose()
    End Sub
    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint
        If RenderUV.Count = 0 Then Exit Sub
        Draw(e.Graphics)

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        clr.Color = Panel2.BackColor
        If clr.ShowDialog = Windows.Forms.DialogResult.OK Then
            Panel2.BackColor = clr.Color
            SaveSetting("KDL", "UV", "line.R", clr.Color.R)
            SaveSetting("KDL", "UV", "line.G", clr.Color.G)
            SaveSetting("KDL", "UV", "line.B", clr.Color.B)
            LINECOLOR = Panel2.BackColor


        End If
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        clr.Color = Panel3.BackColor
        If clr.ShowDialog = Windows.Forms.DialogResult.OK Then
            Panel3.BackColor = clr.Color
            SaveSetting("KDL", "UV", "back.R", clr.Color.R)
            SaveSetting("KDL", "UV", "back.G", clr.Color.G)
            SaveSetting("KDL", "UV", "back.B", clr.Color.B)
            Panel1.BackColor = Panel3.BackColor

        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If TextBox1.BackColor <> Color.Lime Then
            Console.Beep()
            Exit Sub
        End If

        If CheckBox1.Checked = False Then Exit Sub
        Try
            Panel1.BackgroundImage = Image.FromFile(Replace(TextBox1.Text, "%rvdir%", rvpath, , , CompareMethod.Text))
        Catch ex As Exception

        End Try


    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If BMPPICK.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = BMPPICK.FileName
            Button3_Click(sender, e)
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If IO.File.Exists(Replace(TextBox1.Text, "%rvdir%", rvpath, , , CompareMethod.Text)) Then
            TextBox1.BackColor = Color.Lime
        Else
            TextBox1.BackColor = Color.LightPink
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = False Then
            Panel1.BackgroundImage = Nothing
        End If
        Button3_Click(sender, e)

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        TextBox1.Text = Replace(Replace(TextBox2.Text, "%rvdir%", rvpath), Split(TextBox2.Text, "\")(UBound(Split(TextBox2.Text, "\"))), "", , , CompareMethod.Text) & "\" & Chr(65 + tex(TextBox2.Text)) & ".bmp"
        If IO.File.Exists(TextBox1.Text) = False Then
            TextBox1.Text = IO.Directory.GetFiles(Replace(Replace(TextBox2.Text, Split(TextBox2.Text, "\")(UBound(Split(TextBox2.Text, "\"))), "", , , CompareMethod.Text), "%rvdir%", rvpath), "*.bmp")(0)
        End If
        Button3_Click(sender, e)
    End Sub
    Dim jRenderUV As New List(Of Point)
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click

        If save_file.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub


        Dim bmp = New Bitmap(256, 256)
        Dim X = Graphics.FromImage(bmp)

        Save_Bitmap(X, 256)

        X.Dispose()

        bmp.Save(save_file.FileName)


    End Sub
    Sub Save_Bitmap(ByVal X As Graphics, ByVal SizeA As Integer)

        jRenderUV = New List(Of Point)

        For i = 0 To RenderUV.Count - 1
            jRenderUV.Add(New Point(SizeA / 256 * RenderUV(i).X, SizeA / 256 * RenderUV(i).Y))
        Next



        If Panel1.BackColor <> Color.White Then X.FillRectangle(New SolidBrush(Panel1.BackColor), New Rectangle(0, 0, SizeA, SizeA))

        If CheckBox1.Checked And TextBox1.BackColor = Color.Lime Then
            X.DrawImage(Panel1.BackgroundImage, 0, 0, SizeA, SizeA)
        End If

        For i = 0 To RenderUV.Count - 2 Step 2
            X.DrawLine(New Pen(LINECOLOR), jRenderUV(i), jRenderUV(i + 1))
        Next
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        If save_file.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub


        Dim bmp = New Bitmap(512, 512)
        Dim X = Graphics.FromImage(bmp)

        Save_Bitmap(X, 512)

        X.Dispose()

        bmp.Save(save_file.FileName)

    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        If save_file.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub


        Dim bmp = New Bitmap(1024, 1024)
        Dim X = Graphics.FromImage(bmp)

        Save_Bitmap(X, 1024)

        X.Dispose()

        bmp.Save(save_file.FileName)

    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        If save_file.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub


        Dim bmp = New Bitmap(2048, 2048)
        Dim X = Graphics.FromImage(bmp)

        Save_Bitmap(X, 2048)

        X.Dispose()

        bmp.Save(save_file.FileName)

    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        If save_file.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub


        Dim bmp = New Bitmap(4096, 4096)
        Dim X = Graphics.FromImage(bmp)

        Save_Bitmap(X, 4096)

        X.Dispose()

        bmp.Save(save_file.FileName)

    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        If save_file.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub


        Dim bmp = New Bitmap(8192, 8192)
        Dim X = Graphics.FromImage(bmp)

        Save_Bitmap(X, 8192)

        X.Dispose()

        bmp.Save(save_file.FileName)

    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            Button1.Show()
            TextBox2.Show()
            TextBox1.Show()
            Button3.Show()
            Button15.Hide()

        Else
            Button1.Hide()
            TextBox2.Hide()
            TextBox1.Hide()
            Button3.Hide()
            Button15.Show()

        End If
        SaveSetting("KDL", "UV", "advanced", CheckBox2.Checked.ToString)
    End Sub



    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Not fullLoaded Then Exit Sub
        Me.Refresh()
        UpdateSprites()

        DrawSprites(Me.CreateGraphics())
    End Sub

    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Not fullLoaded Then Exit Sub
        '   InitSprites()

    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        fullLoaded = True
        InitSprites()
        Timer2.Stop()
        Timer1.Start()
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        Button1_Click(sender, e)
    End Sub
End Class
