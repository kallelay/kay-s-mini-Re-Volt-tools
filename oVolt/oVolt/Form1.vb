Imports OpenTK.Graphics.OpenGL
Imports OpenTK

Public Class Form1

    Private Sub Form1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress, GlControl1.KeyPress
        
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initGL()
        '   GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, OpenTK.Graphics.OpenGL.All.Modulate)

    End Sub

    Private Sub GlControl1_Invalidated(ByVal sender As Object, ByVal e As System.Windows.Forms.InvalidateEventArgs) Handles GlControl1.Invalidated

    End Sub

    Private Sub GlControl1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GlControl1.KeyDown, Me.KeyDown
        MsgBox(e.KeyData)



    End Sub




    Private Sub GlControl1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GlControl1.MouseClick
        '   myw.Position -= Vector3
    End Sub

    Private Sub GlControl1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles GlControl1.Paint
        '  initGL()


    End Sub
    Dim Done = False
    Dim myw As PRM
    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        If Not Initializd Then initGL()
        'myw = New PRM("C:\Games\Acclaim Entertainment\Re-Volt\cars\toyeca\body.prm")
        myw = New PRM("C:\Games\Acclaim Entertainment\Re-Volt\levels\nhood1\bmw.prm")

        InitAllTextures(myw)
        Done = True
        GlControl1.Focus()


    End Sub
    Dim StartedRendering = False : Dim FinishedRendering = True
    Public FrC As Single = 0.0F
    Dim sinceLastFrame = 0
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        StartedRendering = True

        '    If (Not FinishedRendering) and sinceLastFrame < 2  Then Exit Sub

        GL.Clear(ClearBufferMask.ColorBufferBit Or ClearBufferMask.DepthBufferBit)





        GL.CullFace(CullFaceMode.Back)
        GL.MatrixMode(MatrixMode.Modelview)
        GL.LoadIdentity()


        If Done Then myw.Render()
        ' GL.Flush()


        sinceLastFrame = 0

        GlControl1.SwapBuffers()

        If Not StartedRendering Then Timer1_Tick(sender, e)


        FinishedRendering = True
        StartedRendering = False
        FrC += 1
    End Sub

    Private Sub GlControl1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GlControl1.Load

    End Sub
    Dim kp As Point
    Dim delta As Integer
    Public PANNING_ALLOWED = True
    Private Sub GlControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GlControl1.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            'myw.Position += New Vector3((e.Location - kp).X * 0.5, -(e.Location - kp).Y * 0.5, 0)

            myw.Theta += (e.Location - kp).Y * 0.5
            myw.Phi += (e.Location - kp).X * 0.5

            'Invalidate()

        End If

        If PANNING_ALLOWED And e.Button = Windows.Forms.MouseButtons.Right Then
            'myw.Position += New Vector3((e.Location - kp).X * 0.5, -(e.Location - kp).Y * 0.5, 0)

            PRM.GlobalPosition.X += (e.Location - kp).X * 0.0025
            PRM.GlobalPosition.Y += -(e.Location - kp).Y * 0.0025

            'Invalidate()

        End If


        kp = e.Location
    End Sub

    Private Sub Form1_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel

        '   If e.Delta > 0 Then PRM.Zoom *= Math.Abs(e.Delta) * 0.01 Else PRM.Zoom /= Math.Abs(e.Delta) * 0.01
        PRM.GlobalPosition.Z += 0.5 * (e.Delta) / Math.Abs(e.Delta)

        '    Invalidate()


    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Not (Initializd) Then Exit Sub
        GL.Viewport(Me.Size)
        Invalidate()

    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Me.Text = "Re-Volt vs OpenGL (" & FrC & " fps)"
        FrC = 0
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class
