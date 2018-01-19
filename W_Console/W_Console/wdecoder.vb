Imports System.IO
Imports System.IO.File
Module World_file_decoder
    Dim Ncubes As Int16
    Dim npoly, nver As Int16
    Public Structure CubeX
        Dim CentreX, CentreY, CentreZ, Radius As Single
        Dim Xmin, Xmax, Ymin, Ymax, Zmin, Zmax As Single
        Dim PolyNum, VertNum As Int16
    End Structure
    Public Structure Poly
        Dim Type, Tpage As Int16
        Dim vi0, vi1, vi2, vi3 As Int16
        Dim c0, c1, c2, c3 As Long
        Dim u0, v0, u1, v1, u2, v2, u3, v3 As Single
    End Structure
    Dim MyC() As CubeX
    Dim myP() As Poly
    Dim polyC As Long
    Public Structure VECTOR
        Dim x, y, z As Single
    End Structure
    Public Structure Balls
        Dim vec As VECTOR
        Dim rad As Single

        Dim MeshCount As Long
    End Structure
    Dim MyB As Balls
    Dim ballC As Int32

    Dim MfC As Int32
    Public Structure MMF
        Dim text As Int32

    End Structure
    Dim record$
    Dim myFile As String
    Dim File As IO.StreamWriter
    Dim polyEleven As Int16
    Sub Start(ByVal Filepath$)

        If Filepath = "" Then
            DoWrite("Sorry, no arguments were entered")
            End
        End If
        polyEleven = 0
        myFile$ = ""
        DoWrite("Converting to text in progress...")
        DoWrite("Started " & TimeOfDay.ToLongTimeString)
        ' FileOpen(9, Filepath & ".txt", OpenMode.Output)

        File = IO.File.CreateText(Filepath)

        Dim Y As New BinaryReader(New FileStream(CurrentWorld.Directory & "\" & CurrentWorld.FileName, FileMode.Open))

        Ncubes = Y.ReadInt32

        Writeln("#############################################")
        Writeln("# " & Filepath & " ")
        Writeln("#############################################")
        Writeln("")



        DoWrite("Reading main Infos")
        Writeln("#cubes count")
        Writeln("rvlong " & Ncubes)
        ReDim MyC(Ncubes)
        DoWrite("Reading Cubes")
        For k = 1 To Ncubes
            'Console.Clear()
            DoWrite(" -cube:" & k & "(" & Format((k / Ncubes) * 100, "0#.#0") & "%)")
            Writeln("")

            Writeln("#cube:" & k)

            Writeln("#Bounding sphere (X,Y,Z, radius)")
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)

            Writeln("#Bounding Box (Xmin,Xmax,Ymin,Ymax,Zmin,Zmax)")
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)



            npoly = Y.ReadInt16
            Writeln("#Poly count")
            Writeln("rvshort " & npoly)

            nver = Y.ReadInt16
            Writeln("#Vertex count")
            Writeln("rvshort " & nver)


            polyC += npoly


            For a = 1 To npoly
                Writeln("#Poly n?" & a)


                Dim MyType As Int16
                MyType = Y.ReadInt16
                If MyType And (2 ^ 11) Then
                    polyEleven += 1
                End If
                Writeln("#type")
                Writeln("rvshort " & MyType)
                'Writeln(Convert.ToString(MyType, 2))
                ' If Len(Convert.ToString(MyType, 2)) = 10 Then Writeln("here is it!!!" & "c" & k & "," & a & "/" & npoly)


                Writeln("#texture")
                Writeln("rvshort " & Y.ReadInt16())


                Writeln("#Faces (vertex index which link to a face)")
                Writeln("rvshort " & Y.ReadInt16)
                Writeln("rvshort " & Y.ReadInt16)
                Writeln("rvshort " & Y.ReadInt16)
                Writeln("rvshort " & Y.ReadInt16)


                Writeln("#Vertex Color [3 or 4 depends on the type] (including alpha channel) ")
                Writeln("rvulong " & Y.ReadUInt32)
                Writeln("rvulong " & Y.ReadUInt32)
                Writeln("rvulong " & Y.ReadUInt32)
                Writeln("rvulong " & Y.ReadUInt32)

                Writeln("#UV map")
                For i = 0 To 3
                    Writeln("#(" & (i + 1) & ") UV")
                    Writeln("rvfloat " & Y.ReadSingle)
                    Writeln("rvfloat " & Y.ReadSingle)

                Next




            Next a
            Writeln(" ")
            Writeln(" ")
            For m = 1 To nver
                '  dowrite("     --vertex:" & m)
                Writeln("#vertex n?" & m)
                Writeln("#position (x,y,z)")
                Writeln("rvfloat " & Y.ReadSingle)
                Writeln("rvfloat " & Y.ReadSingle)
                Writeln("rvfloat " & Y.ReadSingle)
                Writeln("#normal (x,y,z)")
                Writeln("rvfloat " & Y.ReadSingle)
                Writeln("rvfloat " & Y.ReadSingle)
                Writeln("rvfloat " & Y.ReadSingle)

                Writeln(" ")

            Next m


        Next k
        '
        DoWrite("Reading Big cubes")
        Writeln("#World's cubes (for FPS I think)")
        Writeln("#Cube count")
        ballC = Y.ReadInt32
        Writeln("rvlong " & ballC)

        For n = 1 To ballC
            DoWrite("     --ball:" & n)
            Writeln("#cube n? " & n)

            Writeln("#Bounding sphere (X,Y,Z, radius)")
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)
            Writeln("rvfloat " & Y.ReadSingle)

            Writeln("#mesh count")
            Dim meshC As Int32 = Y.ReadInt32
            Writeln("rvlong " & meshC)
            Writeln("#mesh indices:")
            For b = 1 To meshC
                Writeln("rvlong " & Y.ReadInt32)
            Next




        Next
        MfC = Y.ReadInt32
        DoWrite("Reading multiframes")
        ' Writeln("rvlong " & Y.ReadInt32)
        Writeln(" ")
        Writeln("# -----------------------------------")

        Writeln("#multiframe count")
        Writeln("rvlong " & MfC)


        For p = 0 To MfC - 1
            If MfC = 0 Then GoTo finish
            Dim MF As Int32 = Y.ReadInt32
            Writeln("#Frame count")
            Writeln("rvlong " & MF)
            For o = 1 To MF

                Writeln("")
                Writeln("#Multiframe(" & (p + 1) & ")(" & o & "):")

                Writeln("#texture")
                Writeln("rvlong " & Y.ReadInt32)

                '  Writeln("rvlong " & Y.ReadInt16)
                Writeln("#time")
                Writeln("rvfloat " & Y.ReadSingle)


                For i = 0 To 3
                    Writeln("#(" & (i + 1) & ") UV")
                    Writeln("rvfloat " & Y.ReadSingle)
                    Writeln("rvfloat " & Y.ReadSingle)

                Next
            Next

            Writeln("")
            Writeln("# ENV COLOR")

            For a = 0 To polyEleven - 1
                Writeln("rvulong " & Y.ReadUInt32)
            Next


finish:
        Next

        Y.Close()
        DoWrite("Finished " & TimeOfDay.ToLongTimeString)
        '  FileClose(9)
        File.Close()

        ' 





    End Sub
    Sub Writeln(ByVal str As String)
        ' Console.WriteLine(str)
        ' myFile &= str & vbNewLine
        'Write(9, CStr(str) & vbCrLf)
        File.WriteLine(str)

    End Sub
End Module
