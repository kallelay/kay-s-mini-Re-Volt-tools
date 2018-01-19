Imports System.IO
Imports OpenTK
Imports OpenTK.Graphics
Imports OpenTK.Graphics.OpenGL



' ///////////////////////////////////
' //        File structure         //
' ///////////////////////////////////
' // Last modification August'8th
' // By theKDL

Public Class W
    Public Directory As String
    Public FileName As String
    Public DirectoryName As String

    Public Pos As New Vector3
    Public Rotation As Quaternion
    Property Position() As Vector3
        Get
            Return Pos
        End Get
        Set(ByVal value As Vector3)
            Pos = value
        End Set
    End Property

    Public meshCount As Long
    Public PolyCount&, VexCount&
    Public mMesh(meshCount) As Worldf
    Public Bitmaps(27) As ListBox
    Public polyEleven = 0
    Public Cubes() As FunnyBall
    Public ENV() As UInt32
    Public TA As ListBox
    Public allEnv As ListBox
    'Public AllFrames As List(Of Frame)
    Public AllFrames As New List(Of List(Of Frame))
    Dim BallC, AnimC As Long

    Sub New(ByVal filepath As String)
        polyEleven = 0
        For i = 0 To 27
            Bitmaps(i) = New ListBox
        Next

        allEnv = New ListBox
        TA = New ListBox

        PolyCount = 0
        VexCount = 0
        AllFrames.Clear()


    
        Dim J As New BinaryReader(New FileStream(filepath, FileMode.Open))


        meshCount = J.ReadInt32
        ReDim mMesh(meshCount)



        For k = 0 To meshCount - 1
            mMesh(k) = New Worldf

            'Bounding Sphere...
            mMesh(k).BoundingSphere = New Sphere(J.ReadSingle, J.ReadSingle, J.ReadSingle, J.ReadSingle)


            'BBOX as well.....
            mMesh(k).bbox = New BBOX()
            mMesh(k).bbox.minX = J.ReadSingle '  = New BBOX(New Vector3D(J.ReadSingle, J.ReadSingle, J.ReadSingle), New Vector3D(J.ReadSingle, J.ReadSingle, J.ReadSingle))
            mMesh(k).bbox.maxX = J.ReadSingle
            mMesh(k).bbox.minY = J.ReadSingle
            mMesh(k).bbox.maxY = J.ReadSingle
            mMesh(k).bbox.minZ = J.ReadSingle
            mMesh(k).bbox.maxZ = J.ReadSingle



            'Vert/Poly count
            mMesh(k).polynum = J.ReadInt16()
            mMesh(k).vertnum = J.ReadInt16()
            'Debugger.Break()
            ' DoWrite("Poly count:" & Chr(9) & mMesh(k).polynum)
            ' DoWrite("Vert count:" & Chr(9) & mMesh(k).vertnum)
            '(  

            ReDim mMesh(k).polyl(mMesh(k).polynum)
            For i = 0 To mMesh(k).polynum - 1



                mMesh(k).polyl(i).type = J.ReadInt16
                If mMesh(k).polyl(i).type And (2 ^ 11) Then
                    allEnv.Items.Add(k & "," & i)
                    polyEleven += 1
                End If

                If mMesh(k).polyl(i).type And (2 ^ 9) Then
                    TA.Items.Add(k)
                    'TA.Sorted = true
                End If

                '  doWrite("TYPE:" & Hex(mmesh(k).polyl(i).type))
                mMesh(k).polyl(i).tpage = J.ReadInt16

                Bitmaps(mMesh(k).polyl(i).tpage + 1).Items.Add(k & "," & i)

                '  If mMesh(k).polyl(i).Tpage = -1 Then mMesh(k).polyl(i).Tpage = 26

                mMesh(k).polyl(i).vi0 = J.ReadInt16
                mMesh(k).polyl(i).vi1 = J.ReadInt16
                mMesh(k).polyl(i).vi2 = J.ReadInt16
                mMesh(k).polyl(i).vi3 = J.ReadInt16



                mMesh(k).polyl(i).c0 = J.ReadUInt32
                mMesh(k).polyl(i).c1 = J.ReadUInt32
                mMesh(k).polyl(i).c2 = J.ReadUInt32
                mMesh(k).polyl(i).c3 = J.ReadUInt32

                mMesh(k).polyl(i).u0 = J.ReadSingle
                mMesh(k).polyl(i).v0 = J.ReadSingle
                mMesh(k).polyl(i).u1 = J.ReadSingle
                mMesh(k).polyl(i).v1 = J.ReadSingle
                mMesh(k).polyl(i).u2 = J.ReadSingle
                mMesh(k).polyl(i).v2 = J.ReadSingle
                mMesh(k).polyl(i).u3 = J.ReadSingle
                mMesh(k).polyl(i).v3 = J.ReadSingle
            Next

            ReDim mMesh(k).vexl(mMesh(k).vertnum)

            PolyCount += mMesh(k).polynum
            VexCount += mMesh(k).vertnum
            For a = 0 To mMesh(k).vertnum - 1


                mMesh(k).vexl(a).Position = New Vector3D(J.ReadSingle, J.ReadSingle, J.ReadSingle)

                mMesh(k).vexl(a).normal = New Vector3D(J.ReadSingle, J.ReadSingle, J.ReadSingle)


            Next
        Next


        BallC = J.ReadInt32


        ReDim Cubes(BallC)
        For i = 0 To BallC - 1
            Cubes(i) = New FunnyBall
            Cubes(i).center = New Vector3D(J.ReadSingle, J.ReadSingle, J.ReadSingle)
            Cubes(i).Radius = J.ReadSingle
            Cubes(i).meshCount = J.ReadInt32
            ReDim Cubes(i).Mesh(Cubes(i).meshCount)
            For k = 0 To Cubes(i).meshCount - 1
                Cubes(i).Mesh(k) = J.ReadInt32
            Next
        Next


        AnimC = J.ReadInt32


        For a = 0 To AnimC - 1

            AllFrames.Add(New List(Of Frame))
            For b = 0 To J.ReadInt32 - 1
                AllFrames.Item(a).Add(New Frame)
            Next



            For c = 0 To AllFrames.Item(a).Count - 1
                AllFrames(a)(c).Tex = J.ReadInt32
                AllFrames(a)(c).Delay = J.ReadSingle * 1000
                AllFrames(a)(c).UV(0) = New Vector2D(J.ReadSingle, J.ReadSingle)
                AllFrames(a)(c).UV(1) = New Vector2D(J.ReadSingle, J.ReadSingle)
                AllFrames(a)(c).UV(2) = New Vector2D(J.ReadSingle, J.ReadSingle)
                AllFrames(a)(c).UV(3) = New Vector2D(J.ReadSingle, J.ReadSingle)
            Next


        Next

        ReDim ENV(polyEleven)

        For a = 0 To polyEleven - 1
            ENV(a) = J.ReadUInt32
        Next


        J.Close()
        'let's set Directory and also Filename

        Dim Splitter$ = "\"


        Try
            Me.DirectoryName = filepath.Split(Splitter)(UBound(filepath.Split(Splitter)) - 1)
            Me.FileName = filepath.Split(Splitter)(UBound(filepath.Split(Splitter)))
            Me.Directory = Replace(filepath, Me.FileName, "", , , CompareMethod.Text)

        Catch ex As Exception
            Me.DirectoryName = CurDir.Split(Splitter)(UBound(CurDir.Split(Splitter)))
            Me.FileName = filepath
            Me.Directory = CurDir()
        End Try


    End Sub

    Sub Save(ByVal filepath As String)

        Dim J As New BinaryWriter(New FileStream(filepath, FileMode.Create))
        J.Write(Convert.ToInt32(meshCount))



        For k = 0 To meshCount - 1

            'Bounding Sphere...
            J.Write(mMesh(k).BoundingSphere.center.X)
            J.Write(mMesh(k).BoundingSphere.center.Y)
            J.Write(mMesh(k).BoundingSphere.center.Z)
            J.Write(mMesh(k).BoundingSphere.Radius)


            'BBOX as well.....
            J.Write(mMesh(k).bbox.minX)
            J.Write(mMesh(k).bbox.maxX)
            J.Write(mMesh(k).bbox.minY)
            J.Write(mMesh(k).bbox.maxY)
            J.Write(mMesh(k).bbox.minZ)
            J.Write(mMesh(k).bbox.maxZ)


            J.Write(CShort(mMesh(k).polynum))
            J.Write(CShort(mMesh(k).vertnum))


            For i = 0 To mMesh(k).polynum - 1

                J.Write(Convert.ToInt16(mMesh(k).polyl(i).type))
                J.Write(Convert.ToInt16(mMesh(k).polyl(i).tpage))


                J.Write(Convert.ToInt16(mMesh(k).polyl(i).vi0))
                J.Write(Convert.ToInt16(mMesh(k).polyl(i).vi1))
                J.Write(Convert.ToInt16(mMesh(k).polyl(i).vi2))
                J.Write(Convert.ToInt16(mMesh(k).polyl(i).vi3))



                J.Write(Convert.ToUInt32(mMesh(k).polyl(i).c0))
                J.Write(Convert.ToUInt32(mMesh(k).polyl(i).c1))
                J.Write(Convert.ToUInt32(mMesh(k).polyl(i).c2))
                J.Write(Convert.ToUInt32(mMesh(k).polyl(i).c3))

                J.Write(mMesh(k).polyl(i).u0)
                J.Write(mMesh(k).polyl(i).v0)
                J.Write(mMesh(k).polyl(i).u1)
                J.Write(mMesh(k).polyl(i).v1)
                J.Write(mMesh(k).polyl(i).u2)
                J.Write(mMesh(k).polyl(i).v2)
                J.Write(mMesh(k).polyl(i).u3)
                J.Write(mMesh(k).polyl(i).v3)


            Next



            For a = 0 To mMesh(k).vertnum - 1

                J.Write(mMesh(k).vexl(a).Position.X)
                J.Write(mMesh(k).vexl(a).Position.Y)
                J.Write(mMesh(k).vexl(a).Position.Z)

                J.Write(mMesh(k).vexl(a).normal.X)
                J.Write(mMesh(k).vexl(a).normal.Y)
                J.Write(mMesh(k).vexl(a).normal.Z)



            Next
        Next

        J.Write(Convert.ToInt32(BallC))
        For i = 0 To BallC - 1
            J.Write(Cubes(i).center.X)
            J.Write(Cubes(i).center.Y)
            J.Write(Cubes(i).center.Z)
            J.Write(Cubes(i).Radius)
            J.Write(CInt(Cubes(i).meshCount))

            For k = 0 To Cubes(i).meshCount - 1
                J.Write(CInt(Cubes(i).Mesh(k)))
            Next
        Next



        J.Write(Convert.ToInt32(AllFrames.Count))

        '  Debugger.Break()
        For a = 0 To AllFrames.Count - 1

            J.Write(Convert.ToInt32(AllFrames.Item(a).Count))


            For c = 0 To AllFrames.Item(a).Count - 1
                J.Write(Convert.ToInt32(AllFrames(a)(c).Tex))
                J.Write(Convert.ToSingle(AllFrames(a)(c).Delay / 1000))
                For i = 0 To 3
                    J.Write(Convert.ToSingle(AllFrames(a)(c).UV(i).X))
                    J.Write(Convert.ToSingle(AllFrames(a)(c).UV(i).Y))
                Next


            Next

        Next


        For a = 0 To polyEleven - 1
            J.Write(Convert.ToUInt32(ENV(a)))
        Next

        J.Close()



    End Sub

    Sub ExportMesh(ByVal Currentmesh%, ByVal Path$)
        Dim min As New Vector3d(900000, 900000, 900000), max As New Vector3d(-900000, -900000, -9000000)
        Dim Curmesh = Me.mMesh(Currentmesh)
        For i = 0 To Curmesh.vertnum - 1
            If max.X < Curmesh.vexl(i).Position.X Then max.X = Curmesh.vexl(i).Position.X
            If max.Y < Curmesh.vexl(i).Position.Y Then max.Y = Curmesh.vexl(i).Position.Y
            If max.Z < Curmesh.vexl(i).Position.Z Then max.Z = Curmesh.vexl(i).Position.Z

            If min.X > Curmesh.vexl(i).Position.X Then min.X = Curmesh.vexl(i).Position.X
            If min.Y > Curmesh.vexl(i).Position.Y Then min.Y = Curmesh.vexl(i).Position.Y
            If min.Z > Curmesh.vexl(i).Position.Z Then min.Z = Curmesh.vexl(i).Position.Z
        Next

        'center/trans
        Dim Trans As New Vector3d(-(min.X + max.X) / 2, -(min.Y + max.Y) / 2, -(min.Z + max.Z) / 2)





        Dim N As New IO.BinaryWriter(New IO.FileStream(Path, IO.FileMode.OpenOrCreate))
        N.Write(Convert.ToInt16(Curmesh.polynum))
        N.Write(Convert.ToInt16(Curmesh.vertnum))

        For i = 0 To Curmesh.polynum - 1
            N.Write(Convert.ToInt16(Curmesh.polyl(i).type))
            N.Write(Convert.ToInt16(Curmesh.polyl(i).Tpage))
            N.Write(Convert.ToInt16(Curmesh.polyl(i).vi0))
            N.Write(Convert.ToInt16(Curmesh.polyl(i).vi1))
            N.Write(Convert.ToInt16(Curmesh.polyl(i).vi2))
            N.Write(Convert.ToInt16(Curmesh.polyl(i).vi3))

            N.Write(Convert.ToUInt32(Curmesh.polyl(i).c0))
            N.Write(Convert.ToUInt32(Curmesh.polyl(i).c1))
            N.Write(Convert.ToUInt32(Curmesh.polyl(i).c2))
            N.Write(Convert.ToUInt32(Curmesh.polyl(i).c3))

            N.Write(Curmesh.polyl(i).u0)
            N.Write(Curmesh.polyl(i).v0)
            N.Write(Curmesh.polyl(i).u1)
            N.Write(Curmesh.polyl(i).v1)
            N.Write(Curmesh.polyl(i).u2)
            N.Write(Curmesh.polyl(i).v2)
            N.Write(Curmesh.polyl(i).u3)
            N.Write(Curmesh.polyl(i).v3)

        Next

        For i = 0 To Curmesh.vertnum - 1

            N.Write(Curmesh.vexl(i).Position.X + Trans.X)
            N.Write(Curmesh.vexl(i).Position.Y + Trans.Y)
            N.Write(Curmesh.vexl(i).Position.Z + Trans.Z)
            N.Write(Curmesh.vexl(i).normal.X)
            N.Write(Curmesh.vexl(i).normal.Y)
            N.Write(Curmesh.vexl(i).normal.Z)

        Next
        N.Close()
    End Sub


    Sub Render()
        For j = 0 To Me.meshCount - 1
            For i = 0 To Me.mMesh(j).polynum - 1
                GL.Translate(Pos)

                If Not (mMesh(j).polyl(i).type And PolyType.QUAD) Then

                    GL.Begin(BeginMode.Points)


                    GL.BindTexture(TextureTarget.Texture2D, Textures(1 + mMesh(j).polyl(i).Tpage))
                    If mMesh(j).polyl(i).type And PolyType.SEMI_TRANS Then
                        GL.Color4(UintToColor(mMesh(j).polyl(i).c0))
                    Else
                        GL.Color3(UintToColor(mMesh(j).polyl(i).c0))
                    End If
                    GL.TexCoord2(mMesh(j).polyl(i).u0, mMesh(j).polyl(i).v0)
                    GL.Normal3(mMesh(j).vexl(mMesh(j).polyl(i).vi0).normal.X, mMesh(j).vexl(mMesh(j).polyl(i).vi0).normal.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi0).normal.Z)
                    GL.Vertex3(mMesh(j).vexl(mMesh(j).polyl(i).vi0).Position.X, mMesh(j).vexl(mMesh(j).polyl(i).vi0).Position.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi0).Position.Z)


                    GL.BindTexture(TextureTarget.Texture2D, Textures(1 + mMesh(j).polyl(i).Tpage))
                    GL.TexCoord2(mMesh(j).polyl(i).u1, mMesh(j).polyl(i).v1)
                    If mMesh(j).polyl(i).type And PolyType.SEMI_TRANS Then
                        GL.Color4(UintToColor(mMesh(j).polyl(i).c1))
                    Else
                        GL.Color3(UintToColor(mMesh(j).polyl(i).c1))
                    End If

                    GL.Normal3(mMesh(j).vexl(mMesh(j).polyl(i).vi1).normal.X, mMesh(j).vexl(mMesh(j).polyl(i).vi1).normal.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi1).normal.Z)
                    GL.Vertex3(mMesh(j).vexl(mMesh(j).polyl(i).vi1).Position.X, mMesh(j).vexl(mMesh(j).polyl(i).vi1).Position.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi1).Position.Z)


                    GL.BindTexture(TextureTarget.Texture2D, Textures(1 + mMesh(j).polyl(i).Tpage))
                    If mMesh(j).polyl(i).type And PolyType.SEMI_TRANS Then
                        GL.Color4(UintToColor(mMesh(j).polyl(i).c2))
                    Else
                        GL.Color3(UintToColor(mMesh(j).polyl(i).c2))
                    End If


                    GL.TexCoord2(mMesh(j).polyl(i).u2, mMesh(j).polyl(i).v2)
                    GL.Normal3(mMesh(j).vexl(mMesh(j).polyl(i).vi2).normal.X, mMesh(j).vexl(mMesh(j).polyl(i).vi2).normal.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi2).normal.Z)
                    GL.Vertex3(mMesh(j).vexl(mMesh(j).polyl(i).vi2).Position.X, mMesh(j).vexl(mMesh(j).polyl(i).vi2).Position.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi2).Position.Z)



                    GL.End()
                Else

                    GL.Begin(BeginMode.Points)

                    GL.BindTexture(TextureTarget.Texture2D, Textures(1 + mMesh(j).polyl(i).Tpage))
                    GL.Normal3(mMesh(j).vexl(mMesh(j).polyl(i).vi0).normal.X, mMesh(j).vexl(mMesh(j).polyl(i).vi0).normal.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi0).normal.Z)
                    If mMesh(j).polyl(i).type And PolyType.SEMI_TRANS Then
                        GL.Color4(UintToColor(mMesh(j).polyl(i).c0))
                    Else
                        GL.Color3(UintToColor(mMesh(j).polyl(i).c0))
                    End If
                    GL.TexCoord2(mMesh(j).polyl(i).u0, mMesh(j).polyl(i).v0)
                    GL.Vertex3(mMesh(j).vexl(mMesh(j).polyl(i).vi0).Position.X, mMesh(j).vexl(mMesh(j).polyl(i).vi0).Position.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi0).Position.Z)


                    GL.Normal3(mMesh(j).vexl(mMesh(j).polyl(i).vi1).normal.X, mMesh(j).vexl(mMesh(j).polyl(i).vi1).normal.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi1).normal.Z)
                    GL.BindTexture(TextureTarget.Texture2D, Textures(1 + mMesh(j).polyl(i).Tpage))
                    If mMesh(j).polyl(i).type And PolyType.SEMI_TRANS Then
                        GL.Color4(UintToColor(mMesh(j).polyl(i).c1))
                    Else
                        GL.Color3(UintToColor(mMesh(j).polyl(i).c1))
                    End If


                    GL.TexCoord2(mMesh(j).polyl(i).u1, mMesh(j).polyl(i).v1)
                    GL.Vertex3(mMesh(j).vexl(mMesh(j).polyl(i).vi1).Position.X, mMesh(j).vexl(mMesh(j).polyl(i).vi1).Position.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi1).Position.Z)
                    GL.Normal3(mMesh(j).vexl(mMesh(j).polyl(i).vi2).normal.X, mMesh(j).vexl(mMesh(j).polyl(i).vi2).normal.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi2).normal.Z)
                    GL.BindTexture(TextureTarget.Texture2D, Textures(1 + mMesh(j).polyl(i).Tpage))
                    If mMesh(j).polyl(i).type And PolyType.SEMI_TRANS Then
                        GL.Color4(UintToColor(mMesh(j).polyl(i).c2))
                    Else
                        GL.Color3(UintToColor(mMesh(j).polyl(i).c2))
                    End If

                    GL.TexCoord2(mMesh(j).polyl(i).u2, mMesh(j).polyl(i).v2)
                    GL.Vertex3(mMesh(j).vexl(mMesh(j).polyl(i).vi2).Position.X, mMesh(j).vexl(mMesh(j).polyl(i).vi2).Position.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi2).Position.Z)


                    GL.Normal3(mMesh(j).vexl(mMesh(j).polyl(i).vi3).normal.X, mMesh(j).vexl(mMesh(j).polyl(i).vi3).normal.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi3).normal.Z)
                    GL.BindTexture(TextureTarget.Texture2D, Textures(1 + mMesh(j).polyl(i).Tpage))
                    If mMesh(j).polyl(i).type And PolyType.SEMI_TRANS Then
                        GL.Color4(UintToColor(mMesh(j).polyl(i).c3))
                    Else
                        GL.Color3(UintToColor(mMesh(j).polyl(i).c3))
                    End If

                    GL.TexCoord2(mMesh(j).polyl(i).u3, mMesh(j).polyl(i).v3)
                    GL.Vertex3(mMesh(j).vexl(mMesh(j).polyl(i).vi3).Position.X, mMesh(j).vexl(mMesh(j).polyl(i).vi3).Position.Y, mMesh(j).vexl(mMesh(j).polyl(i).vi3).Position.Z)

                    GL.End()
                End If



            Next
        Next

        GL.Translate(-Pos)

    End Sub


End Class

'Structure
Public Class FunnyBall '(meshes)
    Public center As Vector3D
    Public Radius As Single
    Public meshCount As Int32

    Public Mesh() As Int32
End Class
Public Class Worldf
    Public BoundingSphere As Sphere
    Public bbox As BBOX
    Public polynum, vertnum As Int16

    Public ListOfThatTexture As New List(Of Integer)

    Public polyl() As MODEL_POLY_LOAD
    Public vexl() As MODEL_VERTEX_MORPH



End Class
