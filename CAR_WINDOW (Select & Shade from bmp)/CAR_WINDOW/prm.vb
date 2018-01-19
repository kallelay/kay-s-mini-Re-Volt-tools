''--------------------------------------------------------------------
'' This document is a part of Car Load , Car_WINDOW
'' All rights reserved (c) Kallel Ahmed Yahia 2009-2014.
'' Licensed under GNU GPL v3
''--------------------------------------------------------------------
Imports System.IO
Imports System.Math

''' <summary>
''' Model is PRM's loading engine
''' </summary>
''' <remarks>@Car Load Loading engine</remarks>
Public Class Car_Model

    Public zoomFactor = 1 'current zoom factor
    Public Directory As String 'directory of the prm
    Public FileName As String 'filename of the prm file
    Public DirectoryName As String 'directory's name only
    ''' <summary>
    ''' The current model
    ''' </summary>
    ''' <remarks></remarks>
    Public MyModel As New MODEL 'current model'''
    Public PolysReadingProgress, VertexReadingProgress As Double 'reading %?

    Public Texture_ As String = "" 'texture string file
    Public VxCount As Single  'vertex count
    Public isMirror As Boolean = False 'haz mirror?
    ''' <summary>
    ''' Constructor of PRM entity
    ''' </summary>
    ''' <param name="filepath">The path to PRM file</param>
    ''' <remarks></remarks>
    Sub New(ByVal filepath As String)

        filepath = Replace(filepath, ",", ".") 'filepath? old fix not neede

        If IO.File.Exists(filepath) = False Then
            Console.Beep(500, 100) 'ok doesn't exist, beep and end it
            Exit Sub
        End If

        DoWrite("loading " & filepath)
        Dim old&


        'open file for reading 
        Dim J As New BinaryReader(New FileStream(Replace(filepath, ",", "."), FileMode.Open))
        If J Is Nothing Then Exit Sub 'if it's null then exit 


        'Vert/Poly count
        MyModel.polynum = J.ReadInt16()
        MyModel.vertnum = J.ReadInt16()

        DoWrite("POLIES: " & MyModel.polynum)
        DoWrite("VEX: " & MyModel.vertnum)

        'For each polygon of those you counted
        ReDim MyModel.polyl(MyModel.polynum)
        For i = 0 To MyModel.polynum - 1

            If old <> Int(100 * i / (MyModel.polynum)) Then
                PolysReadingProgress = Int(100 * i / (MyModel.polynum)) 'progress
            End If

            old = Int(100 * i / (MyModel.polynum)) 'progress


            'get type, tpage, vertex index, colors and uv

            MyModel.polyl(i).type = J.ReadInt16
            MyModel.polyl(i).Tpage = J.ReadInt16


            MyModel.polyl(i).vi0 = J.ReadInt16
            MyModel.polyl(i).vi1 = J.ReadInt16
            MyModel.polyl(i).vi2 = J.ReadInt16
            MyModel.polyl(i).vi3 = J.ReadInt16



            MyModel.polyl(i).c0 = J.ReadInt32
            MyModel.polyl(i).c1 = J.ReadInt32
            MyModel.polyl(i).c2 = J.ReadInt32
            MyModel.polyl(i).c3 = J.ReadInt32

            MyModel.polyl(i).u0 = J.ReadSingle
            MyModel.polyl(i).v0 = J.ReadSingle
            MyModel.polyl(i).u1 = J.ReadSingle
            MyModel.polyl(i).v1 = J.ReadSingle
            MyModel.polyl(i).u2 = J.ReadSingle
            MyModel.polyl(i).v2 = J.ReadSingle
            MyModel.polyl(i).u3 = J.ReadSingle
            MyModel.polyl(i).v3 = J.ReadSingle
        Next

        'For each vertex you mentioned
        ReDim MyModel.vexl(MyModel.vertnum)

        For a = 0 To MyModel.vertnum - 1
            If old <> Int(a * 100 / (MyModel.vertnum)) Then
                VertexReadingProgress = Int(a * 100 / (MyModel.vertnum))
            End If
            MyModel.vexl(a) = New MODEL_VERTEX_MORPH 'get progress
            old = Int(a * 100 / (MyModel.vertnum))
            Dim x, y, z As Single

            'position
            x = J.ReadSingle * zoomFactor
            y = J.ReadSingle * -1 * zoomFactor 'upside down
            z = J.ReadSingle * zoomFactor


            MyModel.vexl(a).Position = New Vector3D(x, y, z)

            'normal
            x = J.ReadSingle * zoomFactor
            y = J.ReadSingle * zoomFactor
            z = J.ReadSingle * zoomFactor
            MyModel.vexl(a).normal = New Vector3D(x, y, z)


        Next
        'close 
        J.Close()

        'let's set Directory and also Filename

        Me.FileName = filepath
        Me.Directory = Replace(filepath, Me.FileName, "", , , CompareMethod.Text)
        Me.DirectoryName = filepath.Split("\")(UBound(filepath.Split("\")) - 1)

        DoWrite("Done!")


    End Sub


    ''' <summary>
    ''' Exports current PRM 
    ''' </summary>
    ''' <param name="filepath">The path of the new PRM file</param>
    ''' <remarks></remarks>
    Sub Export(ByVal filepath As String)

        filepath = Replace(filepath, ",", ".")





        If IO.File.Exists(Replace(filepath, ",", ".")) Then
            '    filecopy(Replace(filepath, ",", "."), Replace(filepath, ",", ".") & ".bak" & int(rnd * 500))
            Kill(Replace(filepath, ",", ".")) 'remove it
        End If
        Dim J As New BinaryWriter(New FileStream(Replace(filepath, ",", "."), FileMode.CreateNew)) 'add new, overwrite
        If J Is Nothing Then Exit Sub


        'Vert/Poly count
        J.Write(Convert.ToInt16(MyModel.polynum))
        J.Write(Convert.ToInt16(MyModel.vertnum))

        Dim i As Integer

        For i = 0 To MyModel.polynum - 1




            'write type, tpage, vi, c and uv for each poly
            J.Write(Convert.ToInt16(MyModel.polyl(i).type))
            J.Write(Convert.ToInt16(MyModel.polyl(i).Tpage))

            J.Write(Convert.ToInt16(MyModel.polyl(i).vi0))
            J.Write(Convert.ToInt16(MyModel.polyl(i).vi1))
            J.Write(Convert.ToInt16(MyModel.polyl(i).vi2))
            J.Write(Convert.ToInt16(MyModel.polyl(i).vi3))

            J.Write(Convert.ToInt32(MyModel.polyl(i).c0))
            J.Write(Convert.ToInt32(MyModel.polyl(i).c1))
            J.Write(Convert.ToInt32(MyModel.polyl(i).c2))
            J.Write(Convert.ToInt32(MyModel.polyl(i).c3))

            J.Write(CSng(MyModel.polyl(i).u0))
            J.Write(CSng(MyModel.polyl(i).v0))
            J.Write(CSng(MyModel.polyl(i).u1))
            J.Write(CSng(MyModel.polyl(i).v1))
            J.Write(CSng(MyModel.polyl(i).u2))
            J.Write(CSng(MyModel.polyl(i).v2))
            J.Write(CSng(MyModel.polyl(i).u3))
            J.Write(CSng(MyModel.polyl(i).v3))


        Next


        Dim a As Integer
        For a = 0 To MyModel.vertnum - 1

            'write postion and normal for each vx
            J.Write(CSng(MyModel.vexl(a).Position.x) / zoomFactor)
            J.Write(CSng(-MyModel.vexl(a).Position.y) / zoomFactor)
            J.Write(CSng(MyModel.vexl(a).Position.z) / zoomFactor)

            J.Write(CSng(MyModel.vexl(a).normal.x))
            J.Write(CSng(MyModel.vexl(a).normal.y))
            J.Write(CSng(MyModel.vexl(a).normal.z))



        Next

        J.Close()

    End Sub








    'Structures
    Public Structure MODEL_POLY_LOAD
        Public type, Tpage As Int16
        Public vi0, vi1, vi2, vi3 As Int16
        Public c0, c1, c2, c3 As Long
        Public u0, v0, u1, v1, u2, v2, u3, v3 As Single
    End Structure

    Public Class MODEL_VERTEX_MORPH
        Public Position As Vector3D
        Public normal As Vector3D
    End Class
    Public Class Sphere
        Public Center As Vector3D
        Public radius As Single
    End Class
    Public Class BBOX
        Public minX, maxX As Single
        Public minY, maxY As Single
        Public minZ, maxZ As Single
    End Class


    'Math class 
    'Classes for loading
    Public Class Vector3D
        Public x, y, z As Single
        Function ProjectXZ() As Vector2D
            Return New Vector2D(x, z)
        End Function
        Function projectXY() As Vector2D
            Return New Vector2D(x, y)

        End Function
        Function getDist() As Single
            Return Sqrt(x ^ 2 + y ^ 2 + z ^ 2)
        End Function
        Function ProjectAlongTheta(ByVal Theta As Single) As Vector2D
            Dim PHi = PI / 4
            Theta = 1.45 * PI
            Return New Vector2D(x * Cos(PHi) * Sin(Theta) + z * Sin(Theta) * Cos(PHi) - Cos(Theta) * y, x * Cos(Theta) * Sin(PHi) - z * Cos(Theta) * Sin(PHi) + Sin(Theta) * y)

        End Function
        Public Overrides Function ToString() As String
            Return "(" & x & "," & y & "," & z & ")"
        End Function
        Function projectYZ() As Vector2D
            Return New Vector2D(y, z)
        End Function
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

        Public Shared Operator *(ByVal v1 As Vector3D, ByVal s As Single) As Vector3D
            Return New Vector3D(v1.x * s, v1.y * s, v1.z * s)
        End Operator
        Public Shared Operator +(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Vector3D
            Return New Vector3D(V1.x + V2.x, V1.y + V2.y, V1.z + V2.z)
        End Operator
        Public Shared Operator -(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Vector3D
            Return New Vector3D(V1.x - V2.x, V1.y - V2.y, V1.z - V2.z)
        End Operator

        Public Shared Operator /(ByVal v1 As Vector3D, ByVal val As Single) As Vector3D
            Return New Vector3D(v1.x / val, v1.y / val, v1.z / val)
        End Operator

    End Class

    Public Class Vector2D
        Public x, y As Single
        Public Shared Operator +(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Vector2D
            Return New Vector2D(V1.x + V2.x, V1.y + V2.y)
        End Operator
        Public Shared Operator +(ByVal V1 As Vector2D, ByVal V2 As Point) As Point
            Return New Point(V1.x + V2.X, V1.y + V2.Y)
        End Operator
        Public Function getPoint() As Point
            Return New Point(Int(x), Int(y))
        End Function



        Public Shared Operator -(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Vector2D
            Return New Vector2D(V1.x - V2.x, V1.y - V2.y)
        End Operator

        Public Shared Operator -(ByVal V1 As Vector2D) As Vector2D
            Return New Vector2D(-V1.x, -V1.y)
        End Operator
        Public Shared Operator *(ByVal v1 As Vector2D, ByVal s As Single) As Vector2D
            Return New Vector2D(v1.x * s, v1.y * s)
        End Operator
        Public Shared Operator *(ByVal v1 As Vector2D, ByVal s As Integer) As Vector2D
            Return New Vector2D(v1.x * s, v1.y * s)
        End Operator
        Public Overrides Function ToString() As String
            Return "(" & x & "," & y & ")"

        End Function
        Sub New(ByVal x As Single, ByVal y As Single)
            Me.x = x
            Me.y = y
        End Sub
        Sub New()
            Me.x = 0
            Me.y = 0
        End Sub
    End Class


    'Model has the following items
    Public Class MODEL
        Public polynum, vertnum As Short
        Public polyl() As MODEL_POLY_LOAD
        Public vexl() As MODEL_VERTEX_MORPH
    End Class
End Class



Module Public_Models
    Public Models As New List(Of Car_Model)
    Public Models2 As New List(Of Car_Model)
End Module
