Imports System.IO

Module level_render

    Public Class WorldFile
        Public Directory As String
        Public FileName As String
        Public DirectoryName As String
        Public meshCount As Long
        Public PolyCount&, VexCount&
        Public mMesh(meshCount) As Worldf
        Public Bitmaps(27) As ListBox
        Public polyEleven = 0
        Public ENV() As UInt32
        Public TA As ListBox
        Public allEnv As ListBox


        Sub New(ByVal filepath As String)
            polyEleven = 0
            For i = 0 To 27
                Bitmaps(i) = New ListBox
            Next

            allEnv = New ListBox
            TA = New ListBox

            PolyCount = 0
            VexCount = 0

            CurrentMesh = -1
            CurrentPoly = -1
            Dim J As New BinaryReader(New FileStream(filepath, FileMode.Open))


            meshCount = J.ReadInt32
            ReDim mMesh(meshCount)

            DoWrite("World: " & filepath)
            DoWrite("Mesh count:" & meshCount)


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

            DoWrite("Reading cubes")

            BallC = J.ReadInt32
            DoWrite("Cubes count:" & BallC)

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

            DoWrite("Reading frames")

            AnimC = J.ReadInt32
            DoWrite("Texture Animation Count:" & AnimC)
            ReDim Frames(AnimC)

            For a = 0 To AnimC - 1
                DoWrite("Reading TextureAnimation (" & (a + 1) & ")")
                Frames(a) = New Animation
                Frames(a).FrameCount = J.ReadInt32
                ReDim Frames(a).Frames(Frames(a).FrameCount)

                For b = 0 To Frames(a).FrameCount - 1

                    Frames(a).Frames(b) = New TEX_ANIM_FRAME
                    Frames(a).Frames(b).index = b
                    Frames(a).Frames(b).texture = J.ReadInt32
                    Frames(a).Frames(b).Time = J.ReadSingle

                    Frames(a).Frames(b).UV(0) = New Vector2D(J.ReadSingle, J.ReadSingle)
                    Frames(a).Frames(b).UV(1) = New Vector2D(J.ReadSingle, J.ReadSingle)
                    Frames(a).Frames(b).UV(2) = New Vector2D(J.ReadSingle, J.ReadSingle)
                    Frames(a).Frames(b).UV(3) = New Vector2D(J.ReadSingle, J.ReadSingle)
                Next


            Next

            ReDim ENV(polyEleven)

            For a = 0 To polyEleven - 1
                ENV(a) = J.ReadUInt32
            Next


            J.Close()
            'let's set Directory and also Filename
            Try
                Me.DirectoryName = filepath.Split("\")(UBound(filepath.Split("\")) - 1)
                Me.FileName = filepath.Split("\").Last
                Me.Directory = Replace(filepath, Me.FileName, "", , , CompareMethod.Text)

            Catch ex As Exception
                Me.DirectoryName = CurDir.Split("\").Last
                Me.FileName = filepath
                Me.Directory = CurDir()
            End Try

            Form1.Label1.Text = String.Format("[[ loaded:   {0} ]]", filepath)
            Form1.Label2.Text = "<mesh number>"
            'Form1.Label3.Text = "<poly number>"
        End Sub

        Sub Save(ByVal filepath As String)

            Dim J As New BinaryWriter(New FileStream(filepath, FileMode.Create))
            J.Write(Convert.ToInt32(meshCount))

            DoWrite("Writing in progress...")
            DoWrite("World: " & filepath)
            DoWrite("Mesh count:" & meshCount)


            For k = 0 To meshCount - 1

                'Bounding Sphere...
                J.Write(mMesh(k).BoundingSphere.center.x)
                J.Write(mMesh(k).BoundingSphere.center.y)
                J.Write(mMesh(k).BoundingSphere.center.z)
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
                    J.Write(Convert.TouInt32(mMesh(k).polyl(i).c3))

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

                    J.Write(mMesh(k).vexl(a).Position.x)
                    J.Write(mMesh(k).vexl(a).Position.y)
                    J.Write(mMesh(k).vexl(a).Position.z)

                    J.Write(mMesh(k).vexl(a).normal.x)
                    J.Write(mMesh(k).vexl(a).normal.y)
                    J.Write(mMesh(k).vexl(a).normal.z)



                Next
            Next

            J.Write(Convert.ToInt32(BallC))
            For i = 0 To BallC - 1
                J.Write(Cubes(i).center.x)
                J.Write(Cubes(i).center.y)
                J.Write(Cubes(i).center.z)
                J.Write(Cubes(i).Radius)
                J.Write(CInt(Cubes(i).meshCount))

                For k = 0 To Cubes(i).meshCount - 1
                    J.Write(CInt(Cubes(i).Mesh(k)))
                Next
            Next

            DoWrite("Writing frames")

            J.Write(Convert.ToInt32(AnimC))
            DoWrite("Texture Animation Count:" & AnimC)
            '  Debugger.Break()
            For a = 0 To AnimC - 1
                DoWrite("Writing TextureAnimation (" & (a + 1) & ")")
                J.Write(Convert.ToInt32(Frames(a).FrameCount))
                For b = 0 To Frames(a).FrameCount - 1
                    J.Write(Convert.ToInt32(Frames(a).Frames(b).texture))
                    J.Write(Frames(a).Frames(b).Time)
                    J.Write(Frames(a).Frames(b).UV(0).x)
                    J.Write(Frames(a).Frames(b).UV(0).y)
                    J.Write(Frames(a).Frames(b).UV(1).x)
                    J.Write(Frames(a).Frames(b).UV(1).y)
                    J.Write(Frames(a).Frames(b).UV(2).x)
                    J.Write(Frames(a).Frames(b).UV(2).y)
                    J.Write(Frames(a).Frames(b).UV(3).x)
                    J.Write(Frames(a).Frames(b).UV(3).y)
                    '   Debugger.Break()
                Next


            Next


            For a = 0 To polyEleven - 1
                J.Write(Convert.ToUInt32(ENV(a)))
            Next

            J.Close()


            Form1.Label1.Text = String.Format("[[ Saved:   {0} ]]", filepath)
        End Sub


    End Class

    Public Structure MODEL_POLY_LOAD
        Dim type As Int16
        Dim tpage As Int16
        Dim vi0, vi1, vi2, vi3 As Int16
        Dim c0, c1, c2, c3 As UInt32
        Dim u0, v0, u1, v1, u2, v2, u3, v3 As Single
    End Structure

    Public Structure MODEL_VERTEX_MORPH
        Dim Position As Vector3D
        Dim normal As Vector3D
    End Structure
    Public Class FunnyBall '(meshes)
        Public center As Vector3D
        Public Radius As Single
        Public meshCount As Int32

        Public Mesh() As Int32
    End Class
    Public Class Animation
        Public FrameCount As Int32
        Public Frames() As TEX_ANIM_FRAME
    End Class

    Public Class Worldf
        Public BoundingSphere As Sphere
        Public bbox As BBOX
        Public polynum, vertnum As Int16

        Public polyl() As MODEL_POLY_LOAD
        Public vexl() As MODEL_VERTEX_MORPH



    End Class

    Public BallC As Int32
    Public Cubes() As FunnyBall
    Public AnimC As Int32
    Public Frames() As Animation

End Module
