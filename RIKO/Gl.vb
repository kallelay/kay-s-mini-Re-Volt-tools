Imports OpenTK.Graphics
Imports OpenTK.Graphics.OpenGL
Imports OpenTK
Imports System.Threading
Imports QuickFont
Imports System.Math

Module GL_Global
    Public GlobalPosition As New Vector3(100, -100, 100) 'Global Position

    Public Const N_THREADS = 3
    Public Th() As Thread  'Rendering Threads
    Public Sub initGL(ByVal Control As GLControl)

        GL.ClearColor(Color.AliceBlue)


        Dim Width% = Control.Width
        Dim Height% = Control.Height

        '  GL.Viewport(New System.Drawing.Size(Width, Height))
        'glDepthRange(0.0001, 1280)

        GL.LoadIdentity()




        GL.Enable(EnableCap.DepthTest)

        GL.Enable(EnableCap.DepthClamp)
        '  GL.Enable(EnableCap.Histogram)
        GL.Enable(EnableCap.Normalize)
        GL.Enable(EnableCap.ScissorTest)
        GL.Enable(EnableCap.LineSmooth)

        GL.DepthFunc(DepthFunction.Less)
        GL.Enable(EnableCap.Normalize)



        TexLib.TexUtil.InitTexturing()


        'Resizing 
        GL.Viewport(0, 0, Width, Height)
        Perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(PI / 4, Width / Height, 0.001F, 1000)
        GL.MatrixMode(MatrixMode.Projection)
        GL.LoadMatrix(Perspective)

        ' Dim config As New QFontBuilderConfiguration(True)

        ReDim Th(N_THREADS - 1)
        For i = 0 To N_THREADS - 1
            Th(i) = New Threading.Thread(AddressOf Void)

        Next


    End Sub
    Sub VOID()

    End Sub
End Module

Module Axis_Grid_Etc
    Public AXIS, COM, WG As PRM ' New PRM("data\models\axis.prm")

    Sub AllInit()
     
    End Sub



    Sub ShowAxis()
        AXIS.isVisible = True
    End Sub
    Sub ShowWG()
        WG.isVisible = True
    End Sub
    Sub HideWG()
        If Not Initializd Then Exit Sub
        WG.isVisible = False
    End Sub
    Sub HideAxis()
        If Not Initializd Then Exit Sub
        AXIS.isVisible = False
    End Sub

    Sub ShowCOM()
        COM.isVisible = True
    End Sub
    Sub HideCOM()
        If Not Initializd Then Exit Sub
        COM.isVisible = False
    End Sub
    Sub UpdateAxis(ByVal Follower As PRM)

        AXIS.Position = Follower.Position
        AXIS.Rotation = Follower.Rotation
    End Sub
    Sub UpdateCOM(ByVal vec As Vector3)
        'vec = Vector3.Multiply(vec, New Vector3(1, -1, 1))
        COM.Position = vec * Zoom
    End Sub
    Sub UpdateWG(ByVal vec As Vector3)
        ' vec = Vector3.Multiply(vec, New Vector3(1, -1, 1))
        WG.Position = vec * Zoom
    End Sub



    '---------------------- update version 3 -----------------------
    Sub Dot(ByRef vec1 As Vector3, ByRef vec2 As Vector3)
        vec1.X *= vec2.X
        vec1.Y *= vec2.Y
        vec1.Z *= vec2.Z
    End Sub

    Sub Mul(ByRef vec1 As Vector3, ByVal f As Single)
        vec1.X *= f
        vec1.Y *= f
        vec1.Z *= f
    End Sub

End Module


