
Imports System.IO
Imports System.Math
Module PRM_File


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
        Public Pos As New Vector3D
        ' Public Rotation As Quaternion
        Public Shared GlobalPosition As New Vector3D
        Public Shared Zoom As Single = 0.001

        'Testing
        Public Vex() As Vector3D

        Public Theta As Single = -20
        Public Phi As Single = 165


        Sub New(ByVal filepath As String)
            If IO.File.Exists(filepath) = False Then
                MsgBox("File doesn't exist")
                Exit Sub
            End If
            Dim J As New BinaryReader(New FileStream(Replace(filepath, Chr(34), ""), FileMode.Open))


            'Vert/Poly count
            MyModel.polynum = J.ReadInt16()
            MyModel.vertnum = J.ReadInt16()

            ReDim MyModel.polyl(MyModel.polynum - 1)
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

            ReDim MyModel.vexl(MyModel.vertnum - 1)

            For a = 0 To MyModel.vertnum - 1


                Dim x, y, z As Single

                x = J.ReadSingle
                y = J.ReadSingle
                z = J.ReadSingle


                MyModel.vexl(a).Position = New Vector3D(x, y, z)

                x = J.ReadSingle '* Zoom
                y = J.ReadSingle ' -1 * Zoom
                z = J.ReadSingle '* Zoom
                MyModel.vexl(a).normal = New Vector3D(x, y, z)


            Next

            'let's set Directory and also Filename

            Me.FileName = filepath.Split("\")(UBound(filepath.Split("\")))
            Me.Directory = Replace(filepath, Me.FileName, "", , , CompareMethod.Text)
            Me.DirectoryName = filepath.Split("\")(UBound(filepath.Split("\")) - 1)

            J.Close()
        End Sub
        Sub Save()
            Save(FullPath & Me.FileName)
        End Sub

        Sub Save(ByVal filepath$)
            If IO.File.Exists(filepath) = False Then
                MsgBox("File doesn't exist")
                Exit Sub
            End If
            Dim J As New BinaryWriter(New FileStream(Replace(filepath, Chr(34), ""), FileMode.Open))


            'Vert/Poly count
            J.Write(Convert.ToInt16(MyModel.polynum))
            J.Write(Convert.ToInt16(MyModel.vertnum))
        


            For i = 0 To MyModel.polynum - 1




                J.Write(Convert.ToInt16(MyModel.polyl(i).type))
                J.Write(Convert.ToInt16(MyModel.polyl(i).tpage))


                J.Write(Convert.ToInt16(MyModel.polyl(i).vi0))
                J.Write(Convert.ToInt16(MyModel.polyl(i).vi1))
                J.Write(Convert.ToInt16(MyModel.polyl(i).vi2))
                J.Write(Convert.ToInt16(MyModel.polyl(i).vi3))




                J.Write(Convert.ToUInt32(MyModel.polyl(i).c0))
                J.Write(Convert.ToUInt32(MyModel.polyl(i).c1))
                J.Write(Convert.ToUInt32(MyModel.polyl(i).c2))
                J.Write(Convert.ToUInt32(MyModel.polyl(i).c3))



                J.Write(Convert.ToSingle(MyModel.polyl(i).u0))
                J.Write(Convert.ToSingle(MyModel.polyl(i).v0))
                J.Write(Convert.ToSingle(MyModel.polyl(i).u1))
                J.Write(Convert.ToSingle(MyModel.polyl(i).v1))
                J.Write(Convert.ToSingle(MyModel.polyl(i).u2))
                J.Write(Convert.ToSingle(MyModel.polyl(i).v2))
                J.Write(Convert.ToSingle(MyModel.polyl(i).u3))
                J.Write(Convert.ToSingle(MyModel.polyl(i).v3))





            Next



            For a = 0 To MyModel.vertnum - 1



                J.Write(Convert.ToSingle(MyModel.vexl(a).Position.x))
                J.Write(Convert.ToSingle(MyModel.vexl(a).Position.y))
                J.Write(Convert.ToSingle(MyModel.vexl(a).Position.z))



                J.Write(Convert.ToSingle(MyModel.vexl(a).normal.x))
                J.Write(Convert.ToSingle(MyModel.vexl(a).normal.y))
                J.Write(Convert.ToSingle(MyModel.vexl(a).normal.z))


            Next

         
            J.Close()
        End Sub

    End Class




    Public Class MODEL
        Public polynum, vertnum As Short
        Public polyl() As MODEL_POLY_LOAD
        Public vexl() As MODEL_VERTEX_MORPH


    End Class



End Module
