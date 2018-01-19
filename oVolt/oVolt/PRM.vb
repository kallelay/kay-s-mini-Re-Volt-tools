Imports System.IO
Imports OpenTK
Imports OpenTK.Graphics
Imports OpenTK.Graphics.OpenGL
Imports System.Math 


' ///////////////////////////////////
' //        File structure         //
' ///////////////////////////////////
' // Last modification August'8th
' // By theKDL

Public Class PRM
    Public Directory As String
    Public FileName As String
    Public DirectoryName As String
    Public MyModel As New MODEL
    Public Pos As New Vector3
    ' Public Rotation As Quaternion
    Public Shared GlobalPosition As New Vector3(0, 0, -2)
    Public Shared Zoom As Single = 0.001

    'Testing
    '  Public Vex() As OpenTK.Vector3

    Public Theta As Single = 20
    Public Phi As Single = 20

    Property Position() As Vector3
        Get
            Return Pos
        End Get
        Set(ByVal value As Vector3)
            Pos = value
        End Set
    End Property


    Sub New(ByVal filepath As String)
        If IO.File.Exists(filepath) = False Then
            MsgBox("File doesn't exist")
            Exit Sub
        End If


        Dim J As New BinaryReader(New FileStream(Replace(filepath, Chr(34), ""), FileMode.Open))

        'Vert/Poly count
        MyModel.polynum = J.ReadInt16()
        MyModel.vertnum = J.ReadInt16()

        ReDim MyModel.polyl(MyModel.polynum)
        For i = 0 To MyModel.polynum - 1




            '

            MyModel.polyl(i).type = J.ReadInt16
            MyModel.polyl(i).Tpage = J.ReadInt16


            MyModel.polyl(i).vi0 = J.ReadInt16
            MyModel.polyl(i).vi1 = J.ReadInt16
            MyModel.polyl(i).vi2 = J.ReadInt16
            MyModel.polyl(i).vi3 = J.ReadInt16



            MyModel.polyl(i).c0 = J.ReadUInt32
            MyModel.polyl(i).c1 = J.ReadUInt32
            MyModel.polyl(i).c2 = J.ReadUInt32
            MyModel.polyl(i).c3 = J.ReadUInt32

            MyModel.polyl(i).u0 = J.ReadSingle
            MyModel.polyl(i).v0 = J.ReadSingle
            MyModel.polyl(i).u1 = J.ReadSingle
            MyModel.polyl(i).v1 = J.ReadSingle
            MyModel.polyl(i).u2 = J.ReadSingle
            MyModel.polyl(i).v2 = J.ReadSingle
            MyModel.polyl(i).u3 = J.ReadSingle
            MyModel.polyl(i).v3 = J.ReadSingle
        Next

        ReDim MyModel.vexl(MyModel.vertnum)

        For a = 0 To MyModel.vertnum - 1


            Dim x, y, z As Single

            x = J.ReadSingle
            y = J.ReadSingle
            z = J.ReadSingle


            MyModel.vexl(a).Position = New Vector3(x, y, z)

            x = J.ReadSingle '* Zoom
            y = J.ReadSingle ' -1 * Zoom
            z = J.ReadSingle '* Zoom
            MyModel.vexl(a).normal = New Vector3(x, y, z)


        Next

        'let's set Directory and also Filename

        Me.FileName = filepath.Split("\")(UBound(filepath.Split("\")))
        Me.Directory = Replace(filepath, Me.FileName, "", , , CompareMethod.Text)
        Me.DirectoryName = filepath.Split("\")(UBound(filepath.Split("\")) - 1)

        J.Close()
    End Sub

    Sub Render()
        GL.Translate(Pos + GlobalPosition)
        GL.Scale(Zoom * New Vector3(1, 1, 1))
        GL.Rotate(Theta, New Vector3(1, 0, 0))
        GL.Rotate(Phi, New Vector3(0, 1, 0))
        '



        For i = 0 To Me.MyModel.polynum - 1
            UseAlpha = MyModel.polyl(i).type And PolyType.SEMI_TRANS
            GL.BindTexture(TextureTarget.Texture2D, Textures(1 + MyModel.polyl(i).Tpage))


            GL.Begin(BeginMode.Triangles)
            GL.Color4(UintToColor(MyModel.polyl(i).c0))
            GL.TexCoord2(MyModel.polyl(i).u0, MyModel.polyl(i).v0)
            GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi0).normal.X, MyModel.vexl(MyModel.polyl(i).vi0).normal.Y, MyModel.vexl(MyModel.polyl(i).vi0).normal.Z)
            GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi0).Position.X, -MyModel.vexl(MyModel.polyl(i).vi0).Position.Y, MyModel.vexl(MyModel.polyl(i).vi0).Position.Z)



            GL.Color4(UintToColor(MyModel.polyl(i).c1))
            GL.TexCoord2(New Vector2d(MyModel.polyl(i).u1, MyModel.polyl(i).v1))
            GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi1).normal.X, MyModel.vexl(MyModel.polyl(i).vi1).normal.Y, MyModel.vexl(MyModel.polyl(i).vi1).normal.Z)
            GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi1).Position.X, -MyModel.vexl(MyModel.polyl(i).vi1).Position.Y, MyModel.vexl(MyModel.polyl(i).vi1).Position.Z)


            GL.Color4(UintToColor(MyModel.polyl(i).c2))
            GL.TexCoord2(New Vector2d(MyModel.polyl(i).u2, MyModel.polyl(i).v2))
            GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi2).normal.X, MyModel.vexl(MyModel.polyl(i).vi2).normal.Y, MyModel.vexl(MyModel.polyl(i).vi2).normal.Z)
            GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi2).Position.X, -MyModel.vexl(MyModel.polyl(i).vi2).Position.Y, MyModel.vexl(MyModel.polyl(i).vi2).Position.Z)


            '   GoTo SkipDBLSD
            If (MyModel.polyl(i).type And PolyType.DOUBLE_SIDED) Then



                GL.Color4(UintToColor(MyModel.polyl(i).c0))
                GL.TexCoord2(MyModel.polyl(i).u0, MyModel.polyl(i).v0)
                GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi0).normal.X, MyModel.vexl(MyModel.polyl(i).vi0).normal.Y, MyModel.vexl(MyModel.polyl(i).vi0).normal.Z)
                GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi0).Position.X, -MyModel.vexl(MyModel.polyl(i).vi0).Position.Y, MyModel.vexl(MyModel.polyl(i).vi0).Position.Z)



                GL.Color4(UintToColor(MyModel.polyl(i).c2))
                GL.TexCoord2(New Vector2d(MyModel.polyl(i).u2, MyModel.polyl(i).v2))
                GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi2).normal.X, MyModel.vexl(MyModel.polyl(i).vi2).normal.Y, MyModel.vexl(MyModel.polyl(i).vi2).normal.Z)
                GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi2).Position.X, -MyModel.vexl(MyModel.polyl(i).vi2).Position.Y, MyModel.vexl(MyModel.polyl(i).vi2).Position.Z)

                GL.Color4(UintToColor(MyModel.polyl(i).c1))
                GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi1).normal.X, MyModel.vexl(MyModel.polyl(i).vi1).normal.Y, MyModel.vexl(MyModel.polyl(i).vi1).normal.Z)
                GL.TexCoord2(New Vector2d(MyModel.polyl(i).u1, MyModel.polyl(i).v1))
                GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi1).Position.X, -MyModel.vexl(MyModel.polyl(i).vi1).Position.Y, MyModel.vexl(MyModel.polyl(i).vi1).Position.Z)



            End If

SkipDBLSD:

            If (MyModel.polyl(i).type And PolyType.QUAD) Then


                GL.Color4(UintToColor(MyModel.polyl(i).c0))
                GL.TexCoord2(MyModel.polyl(i).u0, MyModel.polyl(i).v0)
                GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi0).normal.X, MyModel.vexl(MyModel.polyl(i).vi0).normal.Y, MyModel.vexl(MyModel.polyl(i).vi0).normal.Z)
                GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi0).Position.X, -MyModel.vexl(MyModel.polyl(i).vi0).Position.Y, MyModel.vexl(MyModel.polyl(i).vi0).Position.Z)



                GL.Color4(UintToColor(MyModel.polyl(i).c2))
                GL.TexCoord2(MyModel.polyl(i).u2, MyModel.polyl(i).v2)
                GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi2).normal.X, MyModel.vexl(MyModel.polyl(i).vi2).normal.Y, MyModel.vexl(MyModel.polyl(i).vi2).normal.Z)
                GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi2).Position.X, -MyModel.vexl(MyModel.polyl(i).vi2).Position.Y, MyModel.vexl(MyModel.polyl(i).vi2).Position.Z)



                GL.Color4(UintToColor(MyModel.polyl(i).c3))
                GL.TexCoord2(MyModel.polyl(i).u3, MyModel.polyl(i).v3)
                GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi3).normal.X, MyModel.vexl(MyModel.polyl(i).vi3).normal.Y, MyModel.vexl(MyModel.polyl(i).vi3).normal.Z)
                GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi3).Position.X, -MyModel.vexl(MyModel.polyl(i).vi3).Position.Y, MyModel.vexl(MyModel.polyl(i).vi3).Position.Z)

                '  GoTo SkipDBLSD_Q
                If MyModel.polyl(i).type And PolyType.DOUBLE_SIDED Then

                    GL.Color4(UintToColor(MyModel.polyl(i).c0))
                    GL.TexCoord2(MyModel.polyl(i).u0, MyModel.polyl(i).v0)
                    GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi0).normal.X, MyModel.vexl(MyModel.polyl(i).vi0).normal.Y, MyModel.vexl(MyModel.polyl(i).vi0).normal.Z)
                    GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi0).Position.X, -MyModel.vexl(MyModel.polyl(i).vi0).Position.Y, MyModel.vexl(MyModel.polyl(i).vi0).Position.Z)


                    GL.Color4(UintToColor(MyModel.polyl(i).c3))
                    GL.TexCoord2(MyModel.polyl(i).u3, MyModel.polyl(i).v3)
                    GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi3).normal.X, MyModel.vexl(MyModel.polyl(i).vi3).normal.Y, MyModel.vexl(MyModel.polyl(i).vi3).normal.Z)
                    GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi3).Position.X, -MyModel.vexl(MyModel.polyl(i).vi3).Position.Y, MyModel.vexl(MyModel.polyl(i).vi3).Position.Z)




                    GL.Color4(UintToColor(MyModel.polyl(i).c2))
                    GL.TexCoord2(MyModel.polyl(i).u2, MyModel.polyl(i).v2)
                    GL.Normal3(MyModel.vexl(MyModel.polyl(i).vi2).normal.X, MyModel.vexl(MyModel.polyl(i).vi2).normal.Y, MyModel.vexl(MyModel.polyl(i).vi2).normal.Z)
                    GL.Vertex3(MyModel.vexl(MyModel.polyl(i).vi2).Position.X, -MyModel.vexl(MyModel.polyl(i).vi2).Position.Y, MyModel.vexl(MyModel.polyl(i).vi2).Position.Z)


                End If
SkipDBLSD_Q:
            End If


            GL.End()
        Next


        GL.Rotate(-Theta, New Vector3(0, 1, 0))
        GL.Rotate(-Phi, New Vector3(1, 0, 0))
        GL.Scale(1 / Zoom * New Vector3(1, 1, 1))
        GL.Translate(-Pos - GlobalPosition)
    End Sub


End Class

'Structure

Public Structure MODEL_POLY_LOAD
    Dim type, Tpage As Int16
    Dim vi0, vi1, vi2, vi3 As Int16
    Dim c0, c1, c2, c3 As UInt32
    Dim u0, v0, u1, v1, u2, v2, u3, v3 As Single
End Structure

Public Structure MODEL_VERTEX_MORPH
    Dim Position As Vector3d
    Dim normal As Vector3d
End Structure
Public Class Sphere
    Public Radius As Single
    Public center As New Vector3D
    Sub New(ByVal x As Single, ByVal y As Single, ByVal z As Single, ByVal radius As Single)
        center.X = x
        center.Y = y
        center.Z = z
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
        minX = minvec.X
        minY = minvec.Y
        minZ = minvec.Z

        maxX = maxvec.X
        maxY = maxvec.Y
        maxZ = maxvec.Z

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


Public Class MODEL
    Public polynum, vertnum As Short
    Public polyl() As MODEL_POLY_LOAD
    Public vexl() As MODEL_VERTEX_MORPH


End Class


