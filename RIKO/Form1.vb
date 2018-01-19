Imports System.Math

Imports Riko.Car_Model
Imports OpenTK.Graphics.OpenGL
Imports OpenTK

Public Class Form1
    Public Const oo = Single.PositiveInfinity

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        End

    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        DoWrite("Seeking Car::Load...")
        If GetSetting("Car Load", "settings", "dir", "") = "" Then
            DoWrite("   Not found")
        Else
            RVPATH = GetSetting("Car Load", "settings", "dir", "")
            DoWrite("   Found: you can use %rvdir%'s method")
        End If




        DoWrite("       version:" & "1.0")


        TextBox2.Text = GetSetting("KDL", "carlight", "latest", If((RVPATH) <> "", "%rvdir%\cars\adeon\body.prm", ""))

        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.DoubleBuffer, True)







    End Sub

    Sub Progress(ByVal Name$)

    End Sub

    'Dim Table() As ArrayList
    Dim maxDist As Single, maxDistOwner = -1
    Dim FinalPointsArray As List(Of Integer)
    Dim FinalPoints() As Riko.Car_Model.Vector2D


    Dim Table As New List(Of Riko.Car_Model.Vector2D)

    Dim minOwner, maxOwner As Integer

    Dim MinTable As New List(Of Riko.Car_Model.Vector2D)
    Dim MaxTable As New List(Of Riko.Car_Model.Vector2D)

    Dim MaxZOwners As New List(Of Integer)

    Public center As New Riko.Car_Model.Vector3D
    Public bbmin As New Riko.Car_Model.Vector3D
    Public bbmax As New Riko.Car_Model.Vector3D

    Public points(), pnts(), renderpnts() As Point
    Dim Angle = PI / 4
    Dim InitColor As Color = Color.White
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click




    End Sub
    Sub permut(ByVal tb As List(Of Riko.Car_Model.Vector2D), ByVal x As Integer, ByVal y As Integer)
        Dim extra = tb(x)
        tb(x) = tb(y)
        tb(y) = extra
    End Sub
    Function Allchecked(ByVal tb As List(Of Riko.Car_Model.Vector2D))
        Dim x = 1
        Do Until x = tb.Count - 1 Or tb(If(x < tb.Count - 1, x, x - 1)).y < tb(If(x < tb.Count - 1, x, x - 1) + 1).y
            x += 1


        Loop

        Return x = tb.Count - 1
    End Function

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Public GL_INITIALIZED = False
    Public Car As Car, mPRM As PRM
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If IO.File.Exists(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text)) Then

            If LCase(Strings.Right(TextBox2.Text, 3)) = "prm" Or LCase(Strings.Right(TextBox2.Text, 2)) = ".m" Then
                TextBox2.BackColor = Color.Lime
                SaveSetting("KDL", "carlight", "latest", TextBox2.Text)
                ' mPRM = New PRM(Replace(TextBox2.Text, "%rvdir%", rvpath, , , CompareMethod.Text))
                Car = New Car(Replace((Replace(TextBox2.Text, "%rvdir%", RVPATH, , , CompareMethod.Text)), _
                                      Split((Replace(TextBox2.Text, "%rvdir%", RVPATH, , , CompareMethod.Text)), "\").Last, _
                ""))

                If (Car.Sing Is Nothing) Then
                    Car.models.BODY = New PRM(TextBox2.Text)



                End If


                CheckBox1_CheckedChanged_1(sender, e)
                If Not GL_INITIALIZED Then
                    GL_INITIALIZED = True
                    Timer2.Start()
                    Show()
                    DoWrite("Initializing OpenGL...")




                    initGL(GlControl1)





                    Progress("starting openGL")




                End If

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
        Timer2.Stop()
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox2.Text = OpenFileDialog1.FileName
        End If
        Timer2.Start()

    End Sub



    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub





    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Timer2.Stop()
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            NumericUpDown5.Value = ColorDialog1.Color.R
            NumericUpDown6.Value = ColorDialog1.Color.G
            NumericUpDown7.Value = ColorDialog1.Color.B
        End If
        Timer2.Start()
    End Sub

    Private Sub NumericUpDown5_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
    NumericUpDown5.ValueChanged, NumericUpDown6.ValueChanged, NumericUpDown7.ValueChanged

        POLY.BakedColor.BaseColor = New OpenTK.Graphics.Color4(NumericUpDown5.Value / 255, _
        NumericUpDown6.Value / 255, _
        NumericUpDown7.Value / 255, 1)

        'update
        NumericUpDown1_ValueChanged_1(sender, e)


    End Sub

    Private Sub NumericUpDown6_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown6.ValueChanged
        InitColor = Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value)

    End Sub

    Private Sub NumericUpDown7_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown7.ValueChanged
        InitColor = Color.FromArgb(NumericUpDown5.Value, NumericUpDown6.Value, NumericUpDown7.Value)

    End Sub
    Public LastUsed$ = ""
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If LastUsed <> "vx" Then DoWrite("Now using VERTEX_SHADING_MODE!")
        LastUsed = "vx"


        Dim prm = Car.models.BODY










        For k = 0 To prm.MyModel.polynum - 1
            prm.MyModel.polyl(k).c(0).Gray = ctr(NumericUpDown4.Value * Sqrt((1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.X * NumericUpDown1.Value) ^ 2 / _x.Value + _
                                           (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.Y * NumericUpDown2.Value) ^ 2 / _y.Value + _
                                           (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.Z * NumericUpDown3.Value) ^ 2 / _z.Value) / Sqrt(3))

            prm.MyModel.polyl(k).c(1).Gray = ctr(NumericUpDown4.Value * Sqrt((1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.X * NumericUpDown1.Value) ^ 2 / _x.Value + _
                                    (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.Y * NumericUpDown2.Value) ^ 2 / _y.Value + _
                                    (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.Z * NumericUpDown3.Value) ^ 2 / _z.Value) / Sqrt(3))

            prm.MyModel.polyl(k).c(2).Gray = ctr(NumericUpDown4.Value * Sqrt((1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.X * NumericUpDown1.Value) ^ 2 / _x.Value + _
                                    (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.Y * NumericUpDown2.Value) ^ 2 / _y.Value + _
                                    (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.Z * NumericUpDown3.Value) ^ 2 / _z.Value) / Sqrt(3))


            prm.MyModel.polyl(k).c(3).Gray = ctr(NumericUpDown4.Value * Sqrt((1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.X * NumericUpDown1.Value) ^ 2 / _x.Value + _
                                    (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.Y * NumericUpDown2.Value) ^ 2 / _y.Value + _
                                    (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.Z * NumericUpDown3.Value) ^ 2 / _z.Value) / Sqrt(3))



        Next



    End Sub
    Function ctr(ByVal s!) As Single
        If CheckBox2.Checked = False Then Return s
        Dim C = NumericUpDown8.Value
        Return (s + 0.5) * C - 0.5
    End Function


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
    Dim kp As Point
    Public Sub GlControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GlControl1.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            'myw.Position += New Vector3((e.Location - kp).X * 0.5, -(e.Location - kp).Y * 0.5, 0)

            PRM.Theta += (e.Location - kp).Y * 0.5
            PRM.Phi -= (e.Location - kp).X * 0.5

            'Invalidate()

        End If

        If PANNING_ALLOWED And e.Button = Windows.Forms.MouseButtons.Right Then
            'myw.Position += New Vector3((e.Location - kp).X * 0.5, -(e.Location - kp).Y * 0.5, 0)

            GlobalPosition.X += (e.Location - kp).X * 0.0025
            GlobalPosition.Y += -(e.Location - kp).Y * 0.0025

            'Invalidate()

        End If


        kp = e.Location
    End Sub
    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        If CarIsLoading = True Then Exit Sub
        StartedRendering = True




        '    If (Not FinishedRendering) and sinceLastFrame < 2  Then Exit Sub
        GL.MatrixMode(MatrixMode.Modelview)
        GL.LoadIdentity()
        GL.Clear(ClearBufferMask.ColorBufferBit Or ClearBufferMask.DepthBufferBit)


        Application.DoEvents()




        GL.Light(LightName.Light0, LightParameter.Diffuse, New Graphics.Color4(1.0F, 1.0F, 1.0F, 1.0F))
        GL.Light(LightName.Light0, LightParameter.Specular, New Graphics.Color4(1.0F, 1.0F, 1.0F, 1.0F))
        GL.Light(LightName.Light0, LightParameter.Ambient, New Graphics.Color4(0.8F, 0.8F, 0.8F, 1.0F))
        GL.Light(LightName.Light0, LightParameter.Position, New Vector4(-10, -10, 0, 0))

        If GlobalPosition.X = Single.NaN Then GlobalPosition.X = -0.85
        If GlobalPosition.Y = Single.NaN Then GlobalPosition.Y = 2
        If GlobalPosition.Z = Single.NaN Then GlobalPosition.Z = 0.25


        Dim Eye = New Vector3(GlobalPosition.Length * Cos(PRM.Theta * PI / 180) * Sin(PRM.Phi * PI / 180), _
                              GlobalPosition.LengthFast * Sin(PRM.Theta * PI / 180) * Sin(PRM.Phi * PI / 180), _
                              GlobalPosition.LengthFast * Cos(PRM.Phi * PI / 180))


        ' GL.Translate(GlobalPosition)
        '
        GL.LoadMatrix(Matrix4.LookAt(Eye, New Vector3, New Vector3(0, 1, 0)))

        GL.Rotate(2 * PRM.Theta, New Vector3(1, 0, 0))







        Car.models.BODY.Render()




        '   GL.Rotate(-PRM.Theta, New Vector3(0, 1, 0))
        '  GL.Rotate(-PRM.Phi, New Vector3(1, 0, 0))
        '
        ' GL.Translate(-GlobalPosition)
        '
        ' GL.Rotate(-PRM.Theta, New Vector3(1, 0, 0))
        '   GL.Rotate(-PRM.Phi, New Vector3(0, 1, 0))
        ' GL.Flush()




        GlControl1.SwapBuffers()


        FinishedRendering = True
        StartedRendering = False
        FrC += 1
    End Sub




    Private Sub NumericUpDown1_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged, _
    NumericUpDown2.ValueChanged, NumericUpDown3.ValueChanged, NumericUpDown4.ValueChanged, _
    _x.ValueChanged, _y.ValueChanged, _z.ValueChanged, _
    NumericUpDown8.ValueChanged, CheckBox2.CheckedChanged
        If LastUsed = "vx" Then Button6_Click(sender, e)
        If LastUsed = "pl" Then Button7_Click(sender, e)
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If LastUsed <> "pl" Then DoWrite("Now using POLYGON_SHADING_MODE!")
        LastUsed = "pl"


        Dim prm = Car.models.BODY










        For k = 0 To prm.MyModel.polynum - 1
            Dim fc0 = NumericUpDown4.Value * Sqrt((1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.X * NumericUpDown1.Value) ^ 2 / _x.Value + _
                                           (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.Y * NumericUpDown2.Value) ^ 2 / _y.Value + _
                                           (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi0).normal.Z * NumericUpDown3.Value) ^ 2 / _z.Value) / Sqrt(3)

            Dim fc1 = NumericUpDown4.Value * Sqrt((1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.X * NumericUpDown1.Value) ^ 2 / _x.Value + _
                                   (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.Y * NumericUpDown2.Value) ^ 2 / _y.Value + _
                                   (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi1).normal.Z * NumericUpDown3.Value) ^ 2 / _z.Value) / Sqrt(3)

            Dim fc2 = NumericUpDown4.Value * Sqrt((1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.X * NumericUpDown1.Value) ^ 2 / _x.Value + _
                                   (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.Y * NumericUpDown2.Value) ^ 2 / _y.Value + _
                                   (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi2).normal.Z * NumericUpDown3.Value) ^ 2 / _z.Value) / Sqrt(3)

            Dim fc3 = NumericUpDown4.Value * Sqrt((1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.X * NumericUpDown1.Value) ^ 2 / _x.Value + _
                                   (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.Y * NumericUpDown2.Value) ^ 2 / _y.Value + _
                                   (1 + prm.MyModel.vexl(prm.MyModel.polyl(k).vi3).normal.Z * NumericUpDown3.Value) ^ 2 / _z.Value) / Sqrt(3)






            prm.MyModel.polyl(k).c(0).Gray = ctr((fc1 + fc2 + fc3) / 3)

            prm.MyModel.polyl(k).c(1).Gray = ctr((fc1 + fc2 + fc3) / 3)

            prm.MyModel.polyl(k).c(2).Gray = ctr((fc1 + fc2 + fc3) / 3)


            prm.MyModel.polyl(k).c(3).Gray = ctr((fc1 + fc2 + fc3) / 3)



        Next

    End Sub

    Private Sub GlControl1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GlControl1.Load

    End Sub
    Dim mtex
    Private Sub CheckBox1_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = False Then
            mtex = Textures(1)
            Textures(1) = Nothing
        Else
            Textures(1) = mtex
        End If
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Randomize()

        Select Case Int(Rnd() * 21)
            Case 0
                Label4.Text = "Riko = Car Load 2 engine + CarLighter"
            Case 1
                Label4.Text = "The name came from RECOlor=>Reco, spelt in french as Riko"
            Case 2
                Label4.Text = "Riko is also a Japanese name 梨子 (Ri as in pear, ko as in Child)"
            Case 3
                Label4.Text = "Riko is more advanced than you think: It can modify values in 'quadratics' or 'normal'"
            Case 4
                Label4.Text = "Multiplier: How much light intensity"
            Case 5
                Label4.Text = "Distance=Distance from point (0,0,0)"
            Case 6
                Label4.Text = "Light Normal: `Inverted` Direction of Light"
            Case 7
                Label4.Text = "Did you know? you can also modify Base color!"
            Case 8
                Label4.Text = "Everything won't be applied unless you press on 'Shade'"
            Case 9
                Label4.Text = "There are two ways of shading: Polygon and vertex"
            Case 10
                Label4.Text = "Dedicated to Skarma! Fully dedicated to him and mladen too"
            Case 11
                Label4.Text = "Re-Volt Live welcomes you... erm... RVL WANTS YOU"
            Case 12
                Label4.Text = "thekdl.wordpress.com <-- Block (g)"
            Case 13
                Label4.Text = "Nihao! erm, Yahia, what should I write here"
            Case 14
                Label4.Text = "Konnichiwa! Thanks for using this program!"
            Case 15
                Label4.Text = "Assalama! Merci ala utilisation mta hetha el programme!"
            Case 16
                Label4.Text = "And then I said, well isn't it Mr Slim! They said yes and we lived happily after!"
            Case 17
                Label4.Text = "Hello! Don't forget me :'("
            Case 18
                Label4.Text = "Textures ruin Re-Volt..."
            Case 19
                Label4.Text = "Erm, what should I write here?"
            Case 20
                Label4.Text = "Here is a tip: Use 0.5 -1.0 0.0 | 1.0 7.5 1 | 1 and base 241 255 255"
        End Select
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Car.models.BODY.Export()
    End Sub
End Class
