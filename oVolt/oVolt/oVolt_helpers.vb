Imports OpenTK.Graphics.OpenGL
Imports System.Drawing.Imaging
Imports GLVolt.TexLib
Imports System.Math
Imports OpenTK


Module oVolt_helpers
    Public Perspective As Matrix4
    Public Initializd = False
    Public Sub initGL()

        GL.ClearColor(Color.AliceBlue)


        Dim Width% = Form1.GlControl1.Width
        Dim Height% = Form1.GlControl1.Height

        '  GL.Viewport(New System.Drawing.Size(Width, Height))
        'glDepthRange(0.0001, 1280)

        GL.LoadIdentity()




        GL.Enable(EnableCap.DepthTest)





        TexLib.TexUtil.InitTexturing()


        'Resizing 
        GL.Viewport(New Size(Width, Height))
        Perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(PI / 4, Width / Height, 0.001F, 1000)
        GL.MatrixMode(MatrixMode.Projection)
        GL.LoadMatrix(Perspective)


        Initializd = True
    End Sub
    Declare Auto Sub glDepthRange Lib "Opengl32.dll" Alias "glDepthRange" (ByVal Near As Double, ByVal far As Double)

    Public Function ColorToUint(ByVal color As Color) As UInt32
        Return Convert.ToUInt32(color.A) << 24 Or Convert.ToUInt32(color.R) << 16 Or Convert.ToUInt32(color.G) << 8 Or Convert.ToUInt32(color.B)
    End Function
    Public UseAlpha = True
    Public Function UintToColor(ByVal clr As UInt32) As Color
        Dim a, r, g, b As Int32
        a = clr >> 24 And &HFF
        r = clr >> 16 And &HFF
        g = clr >> 8 And &HFF
        b = clr >> 0 And &HFF

        'If UseAlpha Then _
        a = 255
        '   Return Color.FromArgb(255, 128, 128, 128)
        Return Color.FromArgb(a, r, g, b)

    End Function

    Public Textures(27) As Integer
    Public Sub InitAllTextures(ByVal model As PRM)
        Textures(0) = -1 'Empty

        For i = 1 To 10
            If IO.File.Exists(model.Directory & model.DirectoryName & Chr(i + 64) & ".bmp") Then
                Textures(i) = TexLib.TexUtil.CreateTextureFromFile(model.Directory & model.DirectoryName & Chr(i + 64) & ".bmp")
                'Textures(i + 1) = LoadTexture(model.Directory & model.DirectoryName & Chr(i + 65) & ".bmp")
            Else
                Textures(i) = -1
            End If
        Next



    End Sub
    Public Sub InitAllTextures(ByVal model As W)
        Textures(0) = -1 'Empty

        For i = 1 To 10
            If IO.File.Exists(model.Directory & model.DirectoryName & Chr(i + 64) & ".bmp") Then
                Textures(i) = TexLib.TexUtil.CreateTextureFromFile(model.Directory & model.DirectoryName & Chr(i + 64) & ".bmp")
                'Textures(i + 1) = LoadTexture(model.Directory & model.DirectoryName & Chr(i + 65) & ".bmp")
            Else
                Textures(i) = -1
            End If
        Next



    End Sub
End Module
