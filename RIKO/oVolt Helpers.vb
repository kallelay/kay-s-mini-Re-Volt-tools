Imports OpenTK.Graphics.OpenGL
Imports System.Drawing.Imaging
Imports Riko.TexLib
Imports System.Math
Imports OpenTK
Imports QuickFont


Module oVolt_helpers
    Public Perspective As Matrix4
    Public Initializd = False


    Public Function ColorToUint(ByVal color As Color) As UInt32
        Return Convert.ToUInt32(color.A) << 24 Or Convert.ToUInt32(color.R) << 16 Or Convert.ToUInt32(color.G) << 8 Or Convert.ToUInt32(color.B)
    End Function
    Public Function UintToColor(ByVal clr As Int32, ByVal UseAlpha As Boolean) As OpenTK.Graphics.Color4



        Dim a, r, g, b As Int32
        a = clr >> 24 And &HFF
        r = clr >> 16 And &HFF
        g = clr >> 8 And &HFF
        b = clr >> 0 And &HFF

        If Not UseAlpha Then
            a = 255
        End If

        '  r = clr >> 16 And &HFF
        '  g = clr >> 8 And &HFF
        ' b = clr >> 0 And &HFF
        ' End If
        '   Return Color.FromArgb(255, 128, 128, 128)
        Return New OpenTK.Graphics.Color4(r / 255.0F, g / 255.0F, b / 255.0F, a / 255.0F)

    End Function

    Public Textures(2) As Integer

    Public Sub InitAllTextures(ByVal texture$)
        'TODO: CLEANUP textures

        Textures(0) = -1 'Empty
        Textures(1) = TexLib.TexUtil.CreateTextureFromFile(texture)
        Textures(2) = TexLib.TexUtil.CreateTextureFromFile(RVPATH & "\gfx\fxpage1.bmp")
        'Textures(i + 1) = LoadTexture(model.Directory & model.DirectoryName & Chr(i + 65) & ".bmp")


    End Sub
    'Idea from MS, code from Internet

    '' The idea of Look matrix is simple:

    '               [ROT]
    '   Right.x     Up.x        Look.x      0
    '   Right.y     Up.y        Look.y      0
    '   Right.z     Up.z        Look.z      0
    '     0           0           0         1

    '               [TRANS]
    '   1           0           0           0
    '   0           1           0           0
    '   0           0           1           0
    ' -eye.x       -eye.y    -eye.z         1


    'LookVector = Normal(target-eye)
    'RightVector= Normal( up x LookVector)
    'UpVector   = LookVector x RightVector 


    'Translated from Re-Volt
    Function BuildLookMatrixDown(ByVal pos As Vector3, ByVal look As Vector3) As Matrix4
        Dim M As Matrix4 = Matrix4.Identity



        'UP VECTOR
        Dim V = (look - pos)
        V.Normalize()

        M.M12 = V.X
        M.M22 = V.Y
        M.M32 = V.Z

        'RIGHT VECTOR
        M.M11 = -M.M32
        M.M21 = 0
        M.M31 = M.M12


        V = Vector3.Normalize(M.Column0.Xyz)


        M.M11 = V.X
        M.M21 = V.Y
        M.M31 = V.Z


        'LOOKAT VECTOR
        V = Vector3.Cross(M.Column0.Xyz, M.Column1.Xyz)




        M.M13 = V.X
        M.M23 = V.Y
        M.M33 = V.Z

        M.Invert()

        Return M

    End Function
    Function BuildLookMatrixForward(ByVal pos As Vector3, ByVal look As Vector3) As Matrix4
        Dim M As Matrix4 = Matrix4.Identity


        'UP VECTOR     
        Dim V = (look - pos)

        V.Normalize()

        M.M13 = V.X
        M.M23 = V.Y
        M.M33 = V.Z

        'RIGHT VECTOR
        M.M11 = M.M33
        M.M21 = 0
        M.M31 = -M.M13


        V = Vector3.Normalize(M.Column0.Xyz)


        M.M11 = V.X
        M.M21 = V.Y
        M.M31 = V.Z


        'LOOKAT VECTOR
        V = Vector3.Cross(M.Column2.Xyz, M.Column0.Xyz)


        M.M12 = V.X
        M.M22 = V.Y
        M.M32 = V.Z

        M.Invert()

        Return M


    End Function
    Function ProjectAlongTheta(ByVal Vec As Vector3, ByVal Theta As Single) As Vector2d
        Dim PHi = PI / 4
        Theta = 1.45 * PI
        Return New Vector2d(Vec.X * Cos(PHi) * Sin(Theta) + Vec.Z * Sin(Theta) * Cos(PHi) - Cos(Theta) * Vec.Y, Vec.X * Cos(Theta) * Sin(PHi) - Vec.Z * Cos(Theta) * Sin(PHi) + Sin(Theta) * Vec.Y)

    End Function
    Function getPositionBlock(ByVal camerapos As Vector3, ByVal center As Vector3) As Integer


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



    Sub PrintVecToObjectsT(ByVal vec As Vector3, ByRef object1 As Object, ByRef object2 As Object, ByRef object3 As Object)
        object1.text = vec.X
        object2.text = vec.Y
        object3.text = vec.Z
    End Sub
    Sub PrintVecToObjectsV(ByVal vec As Vector3, ByRef object1 As Object, ByRef object2 As Object, ByRef object3 As Object)
        object1.value = vec.X
        object2.value = vec.Y
        object3.value = vec.Z
    End Sub



    'Releasing
    '-readme
    Sub GenerateReadMe()
        Dim X As New IO.StreamWriter(Application.StartupPath & "\readme.txt")
        X.WriteLine("________________________________________" & vbNewLine)
        X.WriteLine("   R e - V o l t  C a r  S t u d i o" & vbNewLine)
        X.WriteLine("________________________________________")
        X.WriteLine()
        X.WriteLine("Name: " & APPNAME & vbNewLine & _
                      "Made by: " & Maker & vbNewLine & _
                      "Version: " & MajorVersion & "." & MinorVersion & "." & BuildNumber & "." & Revision & "[" & Type.ToString & "]" & vbNewLine & _
               "________________________________________" & vbNewLine & _
                      "COPYRIGHTS:" & vbNewLine & COPY)

        X.WriteLine("________________________________________")
        X.WriteLine(FULL_INFO)
        X.WriteLine("Generated " & Now.ToLongDateString & ", " & Now.ToLongTimeString)
        X.Flush()
        X.Close()


    End Sub
    '-build
    Sub MakeBuild()

        Dim lit = "F:\"
        If IO.Directory.Exists(lit) Then
            If IO.Directory.Exists(lit & "Car Studio") = False Then MkDir(lit & "Car Studio")
            If IO.Directory.Exists(lit & "Car Studio\data") = False Then MkDir(lit & "Car Studio\data")
            If IO.Directory.Exists(lit & "Car Studio\data\lang") = False Then MkDir(lit & "Car Studio\data\lang")
            If IO.Directory.Exists(lit & "Car Studio\data\models") = False Then MkDir(lit & "Car Studio\data\models")
            If IO.Directory.Exists(lit & "Car Studio\data\tips") = False Then MkDir(lit & "Car Studio\data\tips")
            If IO.Directory.Exists(lit & "Car Studio\data\gfx") = False Then MkDir(lit & "Car Studio\data\gfx")
        End If

        If IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio") = False Then MkDir(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio")
        If IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data") = False Then MkDir(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data")
        If IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data\lang") = False Then MkDir(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data\lang")
        If IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data\models") = False Then MkDir(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data\models")
        If IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data\tips") = False Then MkDir(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data\tips")
        If IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data\gfx") = False Then MkDir(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "Car Studio\data\gfx")





        Dim All = IO.Directory.GetFiles(Application.StartupPath, "*", IO.SearchOption.AllDirectories)
        For i = 0 To All.Count - 1
            Select Case LCase(Replace(All(i), Application.StartupPath & "\", ""))
                Case "fmodex.dll", "data\lang\arabic.txt"
                    Continue For

                Case Else
                    If LCase(Split(All(i), ".")(1)) = "pdb" Then Continue For
            End Select

            IO.File.Copy(All(i), My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Car Studio" & Replace(All(i), Application.StartupPath, ""), True)
            If IO.Directory.Exists(lit & "Car Studio\data\tips") Then IO.File.Copy(All(i), lit & "Car Studio" & Replace(All(i), Application.StartupPath, ""), True)


        Next


    End Sub
End Module
