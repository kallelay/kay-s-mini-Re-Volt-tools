Module math_class
    Public Class Vector3D
        Public x! = 0.0F, y! = 0.0F, z! = 0.0F
        Public Sub New()
            Me.x = 0
            Me.y = 0
            Me.z = 0
        End Sub
        Public Sub New(ByVal x!, ByVal y!, ByVal z!)
            Me.x = x
            Me.y = y
            Me.z = z
        End Sub
        Shared Operator +(ByVal vec1 As Vector3D, ByVal vec2 As Vector3D) As Vector3D
            Return New Vector3D(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z)
        End Operator
        Shared Operator -(ByVal vec1 As Vector3D, ByVal vec2 As Vector3D) As Vector3D
            Return New Vector3D(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z)
        End Operator
        Shared Operator /(ByVal vec1 As Vector3D, ByVal scalar!) As Vector3D
            If scalar = 0 Then Throw New Exception("Scalar shouldn't be 0")

            Return New Vector3D(vec1.x / scalar, vec1.y / scalar, vec1.z / scalar)
        End Operator

        Shared Operator *(ByVal vec1 As Vector3D, ByVal scalar!) As Vector3D

            Return New Vector3D(vec1.x * scalar, vec1.y * scalar, vec1.z * scalar)
        End Operator
    End Class
    Public Class Vector2D
        Public x! = 0.0F, y! = 0.0F
        Public Sub New()
            Me.x = 0
            Me.y = 0
        End Sub
        Public Sub New(ByVal x!, ByVal y!)
            Me.x = x
            Me.y = y
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


    Public Const Epsilon = Single.Epsilon
    '4 positions
    Public Function getCenter(ByVal BB As BBOX) As Vector3D
        Return New Vector3D(BB.minX / 2 + BB.maxX / 2, BB.minY / 2 + BB.maxY / 2, BB.maxZ / 2 + BB.minZ / 2)
    End Function
    Public Function getBBoxVectors(ByVal BB As BBOX) As Vector3D
        Return New Vector3D(BB.maxX - BB.minX, BB.maxY - BB.minY, BB.maxZ - BB.minZ)
    End Function
    Public Function getVecLength(ByVal v As Vector3D) As Single
        Return Math.Sqrt(v.x ^ 2 + v.y ^ 2 + v.z ^ 2)
    End Function

    Public Function isNumber(ByVal S As Single) As Boolean
        Return S <> Single.NaN
    End Function
    Public Function isNumber(ByVal D As Double) As Boolean
        Return Not (Double.IsNaN(D))
    End Function
    Public Function isNumber(ByVal I As Integer) As Boolean
        Return I <> Single.NaN
    End Function
    Function getPositionBlock(ByVal camerapos As Vector3D, ByVal center As Vector3D) As Integer


        '[  \1/0
        '[ 2/3\
        'setting X,Z
        Dim x, z As Single
        x = camerapos.X - center.X + 1.0E-66 'to avoid /0
        z = camerapos.Z - center.Z

        If x < 0 Then
            If z / x < -1 Then
                Return 3
            ElseIf z / x >= -1 And z / x <= 1 Then
                Return 2
            Else 'z >1
                Return 1
            End If


        ElseIf x >= 0 Then
            If z / x < -1 Then
                Return 1
            ElseIf z / x >= -1 And z / x <= 1 Then
                '  If x >= 0 Then Return 2
                Return 0
            Else 'z >1
                Return 3
            End If

        End If


    End Function
End Module


