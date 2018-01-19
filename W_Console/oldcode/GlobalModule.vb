Module GlobalModule
    '''''''''''''''''''''''''''
    'Global Variables
    '''''''''''''''''''''''
    Public Zoom = 1
    Public CurrentWorld As WorldFile
    Public CurrentMesh As Integer = -1
    Public CurrentPoly As Integer = -1

    Public BatchMode As Boolean = False
    Public RequestTerminate As Boolean = False
    Public SilentMode = False

    Public LatestOutput$

    'Do write
    Sub DoWrite(ByVal Str As String)
        Application.DoEvents()
        If Not SilentMode Then Form1.TextBox1.AppendText("> " & Str & vbNewLine)
        LatestOutput = Str
    End Sub


    'Classes for loading
    Public Class Vector3D
        Public x, y, z As Single
        Sub New(ByVal x As Single, ByVal y As Single, ByVal z As Single)
            Me.x = x
            Me.y = y
            Me.z = z

        End Sub
        Sub New()
            Me.x = 0
            Me.y = 0
            Me.z = 0
        End Sub
    End Class
    Public Class Vector2D
        Public x, y As Single
        Sub New(ByVal x As Single, ByVal y As Single)
            Me.x = x
            Me.y = y
        End Sub
        Sub New()
            Me.x = 0
            Me.y = 0
        End Sub
    End Class
    Public Class Sphere
        Public Radius As Single
        Public center As New Vector3D
        Sub New(ByVal x As Single, ByVal y As Single, ByVal z As Single, ByVal radius As Single)
            center.x = x
            center.y = y
            center.z = z
            Me.Radius = radius
        End Sub
        Sub New(ByVal Center As Vector3D, ByVal radius As Single)
            Me.center = Center
            Me.Radius = radius
        End Sub
    End Class

    Public Class BBOX
        Public minX, minY, minZ, maxX, maxY, maxZ As Single
        Sub New(ByVal minvec As Vector3D, ByVal maxvec As Vector3D)
            minX = minvec.x
            minY = minvec.y
            minZ = minvec.z

            maxX = maxvec.x
            maxY = maxvec.y
            maxZ = maxvec.z

        End Sub
        Sub New()
            minX = 0
            minY = 0
            minZ = 0
            maxX = 0
            maxY = 0
            maxZ = 0
        End Sub

    End Class
    Public Function ColorToUint(ByVal color As Color) As UInt32
        Return Convert.ToUInt32(color.A) << 24 Or Convert.ToUInt32(color.R) << 16 Or Convert.ToUInt32(color.G) << 8 Or Convert.ToUInt32(color.B)
    End Function
    Public Function UintToColor(ByVal clr As UInt32) As Color
        Dim a, r, g, b As Int32
        a = clr >> 24 And &HFF
        r = clr >> 16 And &HFF
        g = clr >> 8 And &HFF
        b = clr >> 0 And &HFF

        Return Color.FromArgb(a, r, g, b)

    End Function
End Module
