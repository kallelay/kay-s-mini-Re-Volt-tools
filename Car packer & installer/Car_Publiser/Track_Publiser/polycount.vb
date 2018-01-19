
Imports System.IO
'Imports System.IO.File
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



    Function CountSpeed() As String

        Dim k = IO.File.ReadAllText(RvDir & "\cars\" & Fname & "\parameters.txt")

        k = Split(Split(k, "TopSpeed", -1)(1), vbNewLine)(0)
        If InStr(k, ";") > 0 Then k = Split(k, ";")(0)
        k = Replace(Replace(k, vbTab, ""), " ", "")

        If InStr(Str(CSng(0.12)), ",") > 0 Then
            k = k.Replace(".", ",")
        End If

        If InStr(CStr(2.15), ",") > 0 Then
            k = Replace(k, ".", ",")
        End If
        Dim KL = CSng(k)

        Dim nkl = KL * 0.01
        nkl = KL - nkl

        Return Int(nkl) & "~" & Math.Ceiling(KL) & " MPH"


       
    End Function
   
    Function getName() As String

        Dim k = IO.File.ReadAllText(RvDir & "\cars\" & Fname & "\parameters.txt")

        k = Split(Split(k, "Name", -1)(1), vbNewLine)(0)
        If InStr(k, ";") > 0 Then k = Split(k, ";")(0)
        k = Replace(k, vbTab, "")
        Do Until k(0) = Chr(34)
            k = Mid(k, 2)
        Loop

        k = Replace(k, Chr(34), "")

        Return k



    End Function
End Module
