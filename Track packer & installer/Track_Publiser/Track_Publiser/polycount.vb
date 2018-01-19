
Imports System.IO
Imports System.IO.File
Module w_Poly_Ver_counter
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
    Dim polyC, VerC As Long
    Public Structure VECTOR
        Dim x, y, z As Single
    End Structure



    Function CountVer() As String

        Ncubes = 0
        polyC = 0
        VerC = 0
        Dim Y As New BinaryReader(New FileStream(RvDir & "\levels\" & Fname & "\" & Fname & ".w", FileMode.Open))

        Ncubes = Y.ReadInt32
        ReDim MyC(Ncubes)
        For k = 1 To Ncubes
            Y.ReadBytes(4 * 4 + 4 * 6)



            npoly = Y.ReadInt16
            nver = Y.ReadInt16

            polyC += npoly
            VerC += nver


            For a = 1 To npoly

                Dim MyType As Int16
                MyType = Y.ReadInt16

                Y.ReadInt16() 'texture


                'Hurray! it's a quad :)
                Y.ReadBytes(2 * 4)
                Y.ReadBytes(4 * 4)
                Y.ReadBytes(4 * 2 * 4)


            Next a

            For m = 1 To nver
                Y.ReadBytes(4 * 3 * 2)
            Next m

        Next k

        Y.Close()

        Return polyC '& "," '& VerC & "," & Ncubes



    End Function
    Function GetLength() As Long
        Try
            Dim Fast As New IO.BinaryReader(New IO.FileStream(RvDir & "\levels\" & Fname & "\" & Fname & ".pan", FileMode.Open))

            Fast.ReadBytes(8)
            Try
                Return CLng(Fast.ReadSingle() / 200)
            Catch ex As Exception
                Return 0
            End Try

            Fast.Close()


        Catch ex As Exception
            Return 0

        End Try
    End Function
    Function getName() As String
        If IO.Directory.Exists(RvDir & "\levels\" & Fname & "\") Then

            Dim trname As String
            trname = RvDir & "\levels\" & Fname & "\" & Fname & ".inf"
            trname = IO.File.ReadAllText(trname)
            If InStr(trname, "NAME", CompareMethod.Text) > 0 Then
                trname = Split(trname, "NAME", -1, CompareMethod.Text)(1)
                trname = Split(trname, "'")(1)
                Return trname
            End If
        End If
        Return Nothing
    End Function
End Module
