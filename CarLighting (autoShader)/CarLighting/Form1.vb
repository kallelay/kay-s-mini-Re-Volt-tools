Imports System.Math

Imports CarLighting.Car_Model

Public Class Form1
    Public Const oo = Single.PositiveInfinity
    Dim rvpath$ = ""
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DoWrite("Seeking Car::Load...")
        If GetSetting("Car Load", "settings", "dir", "") = "" Then
            DoWrite("   Not found")
        Else
            rvpath = GetSetting("Car Load", "settings", "dir", "")
            DoWrite("   Found: you can use %rvdir%'s method")
        End If


        DoWrite("       version:" & "1.0")


        TextBox2.Text = GetSetting("KDL", "carlight", "latest", If((rvpath) <> "", "%rvdir%\cars\adeon\body.prm", ""))

        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.DoubleBuffer, True)

    End Sub
    'Dim Table() As ArrayList
    Dim maxDist As Single, maxDistOwner = -1
    Dim FinalPointsArray As List(Of Integer)
    Dim FinalPoints() As Vector2D


    Dim Table As New List(Of Vector2D)

    Dim minOwner, maxOwner As Integer

    Dim MinTable As New List(Of Vector2D)
    Dim MaxTable As New List(Of Vector2D)

    Dim MaxZOwners As New List(Of Integer)

    Public center As New Vector3D
    Public bbmin As New Vector3D
    Public bbmax As New Vector3D

    Public points(), pnts(), renderpnts() As Point
    Dim Angle = PI / 4
    Dim InitColor As Color = Color.White
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
       



    End Sub
    Sub permut(ByVal tb As List(Of Vector2D), ByVal x As Integer, ByVal y As Integer)
        Dim extra = tb(x)
        tb(x) = tb(y)
        tb(y) = extra
    End Sub
    Function Allchecked(ByVal tb As List(Of Vector2D))
        Dim x = 1
        Do Until x = tb.Count - 1 Or tb(If(x < tb.Count - 1, x, x - 1)).y < tb(If(x < tb.Count - 1, x, x - 1) + 1).y
            x += 1


        Loop

        Return x = tb.Count - 1
    End Function

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If IO.File.Exists(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text)) Then

            If LCase(Strings.Right(TextBox2.Text, 3)) = "prm" Or LCase(Strings.Right(TextBox2.Text, 2)) = ".m" Then
                TextBox2.BackColor = Color.Lime
                SaveSetting("KDL", "carlight", "latest", TextBox2.Text)
            Else
                TextBox2.BackColor = Color.LightPink

            End If

        Else
            TextBox2.BackColor = Color.LightPink
        End If
    End Sub
    Dim MAXSIZE = 512
    Public COLORx As Color = Color.White



    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If TextBox2.Text = "" Then Exit Sub
        Button1_Click(sender, e)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox2.Text = OpenFileDialog1.FileName
        End If
    End Sub

   

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

            If TextBox2.BackColor <> Color.Lime Then
            DoWrite("Sorry, not valid")
        End If
        Dim prm As New Car_Model(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text))


        Dim zoom As Single = 2.5
        Dim Pan = 180




        Dim n, ts As New ListBox
        DoWrite("Init GDI+")



        Dim g = Me.CreateGraphics

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




                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)



                Dim Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y) + 1
                prm.MyModel.polyl(k).c0 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.y) + 1
                prm.MyModel.polyl(k).c1 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.y) + 1
                prm.MyModel.polyl(k).c2 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.y) + 1
                prm.MyModel.polyl(k).c3 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))


                g.FillPolygon(New SolidBrush(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2)), pnts)



            Else
                Dim pnts(2) As Point
                pnts(0) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(1) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(2) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)

                Dim Normal = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal) / 4
                Dim Center = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position) / 4


                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)



                Dim Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y) + 1
                If Inte = Single.PositiveInfinity Or Inte = Single.NegativeInfinity Or Inte = Single.NaN Or Single.IsNaN(Inte) Then Inte = 1

                prm.MyModel.polyl(k).c0 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))

                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.y) + 1
                If Inte = Single.PositiveInfinity Or Inte = Single.NegativeInfinity Or Inte = Single.NaN Or Single.IsNaN(Inte) Then Inte = 1

                prm.MyModel.polyl(k).c1 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.y) + 1
                If Single.IsInfinity(Inte) Then Inte = 1
                If Inte = Single.PositiveInfinity Or Inte = Single.NegativeInfinity Or Inte = Single.NaN Or Single.IsNaN(Inte) Then Inte = 1

                prm.MyModel.polyl(k).c2 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))

                If Inte = Single.PositiveInfinity Or Inte = Single.NegativeInfinity Or Inte = Single.NaN Or Single.IsNaN(Inte) Then Inte = 1


                If Not CheckBox5.Checked Then g.FillPolygon(New SolidBrush(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2)), pnts)


            End If

        Next

        DoWrite("Creating a backup")
        FileCopy(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text), Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text) & ".bak")


        DoWrite("Exporting .prm (using PRM)")
        prm.Export(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text))

        DoWrite("Ending scene")
        g.Dispose()


    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        If TextBox2.BackColor <> Color.Lime Then
            DoWrite("Sorry, not valid")
        End If
        Dim prm As New Car_Model(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text))


        Dim zoom As Single = 2.5
        Dim Pan = 180




        Dim n, ts As New ListBox
        DoWrite("Init GDI+")



        Dim g = Me.CreateGraphics

        g.Clear(Color.AliceBlue)

        DoWrite("Fixing Positions")

        'For q = 0 To prm.MyModel.vertnum - 1
        'prm.MyModel.vexl(q).Position *= zoom
        ' Next


        DoWrite("Calculating BBOX")


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




                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)

                Dim Normal = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal) / 4
                Dim Center = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).Position) / 4

                Dim Inte = Normal.y + 1 ' / (LightPos - Center).getDist + 1
                If CheckBox4.Checked Then Inte = -Normal.y + 1


                ' Dim Inte = ((Normal.x * LightNor.x + Normal.y * LightNor.y + Normal.z * LightNor.z) + 1.5) / 3 ' * 2 / (LightPos - Center).y) + 20
                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)




                prm.MyModel.polyl(k).c0 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                prm.MyModel.polyl(k).c1 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                prm.MyModel.polyl(k).c2 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                prm.MyModel.polyl(k).c3 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))


                g.FillPolygon(New SolidBrush(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2)), pnts)



            Else
                Dim pnts(2) As Point
                pnts(0) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(1) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(2) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)

                Dim Normal = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal) / 4
                Dim Center = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position) / 4



                Dim Inte = Normal.y + 1 ' / (LightPos - Center).getDist + 1
                If CheckBox4.Checked Then Inte = -Normal.y + 1

                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)




                prm.MyModel.polyl(k).c0 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                prm.MyModel.polyl(k).c1 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                prm.MyModel.polyl(k).c2 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                ' prm.MyModel.polyl(k).c3 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))


                If Not CheckBox5.Checked Then g.FillPolygon(New SolidBrush(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2)), pnts)

            End If

        Next

        DoWrite("Creating a backup")
        FileCopy(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text), Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text) & ".bak")


        DoWrite("Exporting .prm (using PRM)")
        prm.Export(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text))

        DoWrite("Ending scene")
        g.Dispose()

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            NumericUpDown5.Value = ColorDialog1.Color.R
            NumericUpDown6.Value = ColorDialog1.Color.G
            NumericUpDown7.Value = ColorDialog1.Color.B
        End If
    End Sub

    Private Sub NumericUpDown5_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown5.ValueChanged
        InitColor = Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value)
    End Sub

    Private Sub NumericUpDown6_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown6.ValueChanged
        InitColor = Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value)

    End Sub

    Private Sub NumericUpDown7_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown7.ValueChanged
        InitColor = Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value)

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        If TextBox2.BackColor <> Color.Lime Then
            DoWrite("Sorry, not valid")
        End If
        Dim prm As New Car_Model(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text))


        Dim zoom As Single = 2.5
        Dim Pan = 180




        Dim n, ts As New ListBox
        DoWrite("Init GDI+")



        Dim g = Me.CreateGraphics

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




                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)



                Dim Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y) + 1
                ' prm.MyModel.polyl(k).c0 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.y) + 1
                'prm.MyModel.polyl(k).c1 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.y) + 1
                ' prm.MyModel.polyl(k).c2 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.y) + 1
                ' prm.MyModel.polyl(k).c3 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))


                g.FillPolygon(New SolidBrush(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2)), pnts)



            Else
                Dim pnts(2) As Point
                pnts(0) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(1) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(2) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)

                Dim Normal = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal) / 4
                Dim Center = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position) / 4


                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)




                Dim Inte As Single = prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.y) + 1
                ' prm.MyModel.polyl(k).c0 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.y) + 1
                '  prm.MyModel.polyl(k).c1 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))
                Inte = prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.y + 1
                If CheckBox4.Checked Then Inte = -(prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.y) + 1
                'prm.MyModel.polyl(k).c2 = RGBToLong(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2))

                g.DrawString("PLEASE LOAD AT CAR.LOAD() TO GET THE REAL PREVIEW", New Drawing.Font(Drawing.FontFamily.GenericMonospace, 8, FontStyle.Bold, GraphicsUnit.Point), Brushes.Red, 10, 40)

                If Inte = Single.PositiveInfinity Or Inte = Single.NegativeInfinity Or Inte = Single.NaN Or Single.IsNaN(Inte) Then Inte = 1

                g.FillPolygon(New SolidBrush(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2)), pnts)

                Application.DoEvents()


            End If

        Next

        g.Save()


        DoWrite("Ending scene")
        g.Dispose()

    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If TextBox2.BackColor <> Color.Lime Then
            DoWrite("Sorry, not valid")
        End If
        Dim prm As New Car_Model(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text))


        Dim zoom As Single = 3
        Dim Pan = 200




        Dim n, ts As New ListBox
        DoWrite("Init GDI+")



        Dim g = Me.CreateGraphics

        g.Clear(Color.AliceBlue)

        DoWrite("Fixing Positions")

        'For q = 0 To prm.MyModel.vertnum - 1
        'prm.MyModel.vexl(q).Position *= zoom
        ' Next


        DoWrite("Calculating BBOX")


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




                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)

                Dim Normal = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal) / 4
                Dim Center = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).Position) / 4

                Dim Inte = Normal.y + 1 ' / (LightPos - Center).getDist + 1
                If CheckBox4.Checked Then Inte = -(Normal.y) + 1

                ' Dim Inte = ((Normal.x * LightNor.x + Normal.y * LightNor.y + Normal.z * LightNor.z) + 1.5) / 3 ' * 2 / (LightPos - Center).y) + 20
                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)



                If Inte = Single.PositiveInfinity Or Inte = Single.NegativeInfinity Or Inte = Single.NaN Or Single.IsNaN(Inte) Then Inte = 1



                g.FillPolygon(New SolidBrush(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2)), pnts)



            Else
                Dim pnts(2) As Point
                pnts(0) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(1) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)
                pnts(2) = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position.ProjectAlongTheta(Angle) * zoom).getPoint + New Point(Pan, Pan)

                Dim Normal = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal) / 4
                Dim Center = (prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).Position + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).Position) / 4



                Dim Inte = Normal.y + 1 ' / (LightPos - Center).getDist + 1
                If CheckBox4.Checked Then Inte = -(Normal.y) + 1

                InitColor = If(CheckBox2.Checked, Color.Green, Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value))
                If CheckBox1.Checked Then g.DrawLines(Pens.Black, pnts)


                If Inte = Single.PositiveInfinity Or Inte = Single.NegativeInfinity Or Inte = Single.NaN Or Single.IsNaN(Inte) Then Inte = 1




                g.FillPolygon(New SolidBrush(Color.FromArgb(Inte * InitColor.R / 2, Inte * InitColor.G / 2, Inte * InitColor.B / 2)), pnts)

            End If

        Next

        g.Save()




        DoWrite("Ending scene")
        g.Dispose()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Shell("explorer ""http://thekdl.wordpress.com/""", AppWinStyle.NormalFocus)

    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Shell("explorer ""http://z3.invisionfree.com/Revolt_Live/""", AppWinStyle.NormalFocus)

    End Sub

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Shell("explorer ""http://z3.invisionfree.com/Our_Revolt_Pub/""", AppWinStyle.NormalFocus)

    End Sub

    Private Sub LinkLabel4_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Shell("explorer ""http://revolt.wikia.org/""", AppWinStyle.NormalFocus)

    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
End Class
