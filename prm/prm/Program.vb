'
' Crée par SharpDevelop.
' Utilisateur: Admin
' Date: 07/05/2011
' Heure: 17:32
'
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'

Imports System.Math
imports System.Drawing

Module Program
    Sub Main()

        'Get the trouble problems

        Dim CommandLineArgs(My.Application.CommandLineArgs.Count - 1) As String

        For i = 0 To My.Application.CommandLineArgs.Count - 1
            CommandLineArgs(i) = My.Application.CommandLineArgs(i)
        Next

        'HACK: problems of numerics
        If CStr(CSng(1.23)).IndexOf(".") = 0 Then
            'mode is float, sep. dec by comma
            For i = 0 To CommandLineArgs.Length - 1
                CommandLineArgs(i) = Replace(CommandLineArgs(i), ".", ",")
            Next
        Else
            For i = 0 To CommandLineArgs.Length - 1
                CommandLineArgs(i) = Replace(CommandLineArgs(i), ",", ".")
            Next
        End If

        If Command() = "" Then
            Logo()
            showHelp()
            EndIt()
        End If

        If Command(0) <> "*" Then Logo()





        If My.Application.CommandLineArgs.Count < 2 Then
            If Not LCase(My.Application.CommandLineArgs(0)) = "help" Then

                showHelp()
                EndIt()


            End If
        End If

        Select Case LCase(CommandLineArgs(0).Replace("_", ""))

            Case "help"
                Output("count file.prm: Counting PRM's polygons")
                Output("info file.prm: getting PRM's infos")
                Output("center file.prm: Center a PRM (XYZ)")
                Output2("center file.prm XY: Center a PRM (X and Y only)")
                Output2("center file.prm XZ: Center a PRM (X and Z only)")
                Output("double file.prm: make PRM double sided")
                Output("single file.prm: make PRM single sided")
                Output("trans file.prm %: set opacity of PRM")
                Output2("trans file.prm 25% : Opacity = 25%")
                Output2("opacity file.prm 25% is another input")

                Output("remap file.prm FROM TO: remap from FROM to TO ")
                Output2("possible input: A~Z and 1~30, ex: remap file.prm A B")
                Output("resize file.prm ScaleRatio: Resize PRM 'ScaleRatio' time ")
                Output2("Example: Resize file.prm 0.5: resize to half")
                Output2("possible scaleration: Float numbers, -nfs4 (0.8), -double, -half,")
                Output2("                                     -zmod (3.33) and -tiny")
                Output2("Scale file.prm ScaleRatio is another syntax")
                EndIt()

                Console.ReadKey()


            Case "autoshade"
                If CommandLineArgs.Length > 2 Then
                    If Trim(LCase(CommandLineArgs(2))) = "vertex" Then
                        autoShade(CommandLineArgs(1), 0)
                    ElseIf Trim(LCase(CommandLineArgs(2))) = "polygon" Then
                        autoShade(CommandLineArgs(1), 1)

                    End If
                Else 'shade by vertex
                    autoShade(CommandLineArgs(1), 0)
                End If
            Case "autoshadeinv"
                If CommandLineArgs.Length > 2 Then
                    If Trim(LCase(CommandLineArgs(2))) = "vertex" Then
                        autoShade(CommandLineArgs(1), 0, True)
                    ElseIf Trim(LCase(CommandLineArgs(2))) = "polygon" Then
                        autoShade(CommandLineArgs(1), 1, True)

                    End If
                Else 'shade by vertex
                    autoShade(CommandLineArgs(1), 0, True)
                End If

            Case "rvshade", "shade"
                'shade file R G B [A]
                If CommandLineArgs.Length = 5 Then
                    rvShade(CommandLineArgs(1), _
                            CommandLineArgs(2), _
                             CommandLineArgs(3), _
                              CommandLineArgs(4))
                ElseIf CommandLineArgs.Length = 6 Then
                    rvShade(CommandLineArgs(1), _
                            CommandLineArgs(2), _
                             CommandLineArgs(3), _
                              CommandLineArgs(4), _
                              CommandLineArgs(5))
                ElseIf CommandLineArgs.Length = 3 Then
                    rvShade(CommandLineArgs(1), _
                            CommandLineArgs(2), _
                             CommandLineArgs(2), _
                              CommandLineArgs(2))

                Else
                    Output2("Error: prm shade file.prm R G B A ")
                    Output2("Error: prm shade file.prm R G B ")
                    Output2("Error: prm shade file.prm Gray ")
                End If

            Case "count"
                CountVerticesAndShow(CommandLineArgs(1))

            Case "info"
                CountVerticesAndShow(CommandLineArgs(1))

            Case "translate"
                'trans file X Y Z
                If CommandLineArgs.Length = 5 Then
                    Try
                        Dim x = CSng(CommandLineArgs(2))
                        Dim y = CSng(CommandLineArgs(3))
                        Dim z = CSng(CommandLineArgs(4))
                        Translate(CommandLineArgs(1), x, y, z)

                    Catch ex As Exception
                        Output2("Couldn't perform action, make sure it is Translate file.prm X Y Z")
                        Output2("Also make sure to see the decimal seperator in Control Panel\regions  (comma , or point . )")
                    End Try

                Else
                    Output2("Error: Translate file.prm X Y Z")

                End If

                '---------------- normal manipulation ------------------
            Case "normal"
                'normal file X Y Z
                If CommandLineArgs.Length = 5 Then
                    Try
                        Dim x = CSng(CommandLineArgs(2))
                        Dim y = CSng(CommandLineArgs(3))
                        Dim z = CSng(CommandLineArgs(4))
                        Normalize(CommandLineArgs(1), x, y, z)

                    Catch ex As Exception
                        Output2("Couldn't perform action, make sure it is Translate file.prm X Y Z")
                        Output2("Also make sure to see the decimal seperator in Control Panel\regions  (comma , or point . )")
                    End Try

                Else
                    Output2("Error: Normal file.prm X Y Z")

                End If

            Case "setnormal" 'HACK
                Dim x = CSng(CommandLineArgs(2))
                Dim y = CSng(CommandLineArgs(3))
                Dim z = CSng(CommandLineArgs(4))
                Normalize(CommandLineArgs(1), x, y, z, 0)

            Case "normalize"
                BreakNormal(CommandLineArgs(1), 0)
            Case "breaknormals"
                BreakNormal(CommandLineArgs(1), 1)



            Case "rotate"
                'rotate file origin X Y Z
                'rotate file center X Y Z
                'rotate file centeroid X Y Z
                'rotate file centerX centerY centerZ X Y Z
                If CommandLineArgs.Length = 6 Then
                    Try
                        Dim x! = CSng(CommandLineArgs(3))
                        Dim y! = CSng(CommandLineArgs(4))
                        Dim z! = CSng(CommandLineArgs(5))

                        If Trim(LCase(CommandLineArgs(2))) = "center" Then
                            Rotate(CommandLineArgs(1), 1, x, y, z)
                        ElseIf Trim(LCase(CommandLineArgs(2))) = "centeroid" Then
                            Rotate(CommandLineArgs(1), 2, x, y, z)
                        ElseIf Trim(LCase(CommandLineArgs(2))) = "origin" Then
                            Rotate(CommandLineArgs(1), 0, x, y, z)

                        End If



                    Catch ex As Exception
                        Output2("Bad numerics")
                        Return
                    End Try

                ElseIf CommandLineArgs.Length = 8 Then
                    Try
                        Dim cx! = CSng(CommandLineArgs(2))
                        Dim cy! = CSng(CommandLineArgs(3))
                        Dim cz! = CSng(CommandLineArgs(4))
                        Dim x! = CSng(CommandLineArgs(5))
                        Dim y! = CSng(CommandLineArgs(6))
                        Dim z! = CSng(CommandLineArgs(7))


                        Rotate(CommandLineArgs(1), 3, x, y, z, cx, cy, cz)

                    Catch ex As Exception
                        Output2("Bad numerics")
                        Return
                    End Try
                Else
                    Output2("Wrong usage:")
                    Output2("prm rotate file.prm origin angleX angleY angleZ (rotate along origin)")
                    Output2("prm rotate file.prm center angleX angleY angleZ (rotate along bbox center)")
                    Output2("prm rotate file.prm centroid angleX angleY angleZ (rotate along centroid center)")

                    Output2("prm rotate file.prm centerRotationX centerRotationY centerRotationZ angleX angleY angleZ (rotate along a specified axis point)")
                End If




            Case "texmap", "rvtexmap"
                'prm texmap body.prm h:0,0:256x256=a:0,0:128x128
                'prm rvtexmap file.prm h:0,0:256x256=a:0,0:512x512
                Try
                    Dim args = Split(Command(), ".prm", , CompareMethod.Text)(1)
                    args = Replace(Replace(LCase(args), Chr(34), ""), " ", "")
                    args = Trim(args)
                    args = Replace(args, ",", "x")


                    Dim srcTex$ = Split(args, ":")(0)
                    Dim srcPos$ = Split(args, ":")(1)
                    Dim srcWH$ = Split(Split(args, ":")(2), "=")(0)

                    Dim dstTex$ = Split(Split(args, "=")(1), ":")(0)
                    Dim dstPos$ = Split(Split(args, "=")(1), ":")(1)
                    Dim dstWH$ = Split(Split(args, "=")(1), ":")(2)


                    Dim srcTexN% = If(srcTex = "*", -1, Asc(srcTex) - 97)
                    Dim dstTexN% = If(dstTex = "*", -1, Asc(dstTex) - 97)

                    Dim ltx! = Split(srcPos, "x")(0) / 256.0
                    Dim lty! = Split(srcPos, "x")(1) / 256.0
                    Dim rbx! = Split(srcWH$, "x")(0) / 256.0 + ltx!
                    Dim rby! = Split(srcWH$, "x")(1) / 256.0 + lty!


                    Dim dltx! = Split(dstPos, "x")(0) / 256.0
                    Dim dlty! = Split(dstPos, "x")(1) / 256.0
                    Dim drbx! = Split(dstWH$, "x")(0) / 256.0 + ltx!
                    Dim drby! = Split(dstWH$, "x")(1) / 256.0 + lty!

                    remapUV(CommandLineArgs(1), srcTexN, ltx, lty, rbx, rby, _
                            dstTexN, dltx, dlty, drbx, drby)

                Catch
                    Output2("Bad syntax, try follow the syntax below")
                    Output2("prm texmap body.prm h:0,0:256x256=a:0,0:128x128")
                    Output2("prm texmap body.prm *:0,0:256x256=a:0,0:128x128")
                    Output2("Contact Manual asap")
                End Try





            Case "uvremap"
                'prm uvremap file.prm "A 0.01x0.03 0.2x0.5" "B 0.3x0.5 1x1"
                'prm uvremap body.prm "* 0.01x0.03 0.2x0.5" "B 0.3x0.5 1x1"
                Try

                    CommandLineArgs(2) = Replace(CommandLineArgs(2), Space(2), Space(1))
                    CommandLineArgs(3) = Replace(CommandLineArgs(3), Space(2), Space(1))

                    Dim srcTexN% = If(CommandLineArgs(2)(0) = "*", -1, Asc(CommandLineArgs(2)(0)) - 97)
                    Dim dstTexN% = If(CommandLineArgs(3)(0) = "*", -1, Asc(CommandLineArgs(3)(0)) - 97)

                    Dim ltx! = Split(Split(CommandLineArgs(2), " ")(1), "x")(0)
                    Dim lty! = Split(Split(CommandLineArgs(2), " ")(1), "x")(1)

                    Dim rbx! = Split(Split(CommandLineArgs(2), " ")(2), "x")(0)
                    Dim rby! = Split(Split(CommandLineArgs(2), " ")(2), "x")(1)


                    Dim dltx! = Split(Split(CommandLineArgs(3), " ")(1), "x")(0)
                    Dim dlty! = Split(Split(CommandLineArgs(3), " ")(1), "x")(1)

                    Dim drbx! = Split(Split(CommandLineArgs(3), " ")(2), "x")(0)
                    Dim drby! = Split(Split(CommandLineArgs(3), " ")(2), "x")(1)

                    remapUV(CommandLineArgs(1), srcTexN, ltx, lty, rbx, rby, _
                               dstTexN, dltx, dlty, drbx, drby)
                Catch

                    Output2("Bad syntax, try to stick to this")
                    Output2("prm uvremap file.prm ""A 0.01x0.03 0.2x0.5"" ""B 0.3x0.5 1x1""")
                    Output2("prm uvremap body.prm ""* 0.01x0.03 0.2x0.5"" ""B 0.3x0.5 1x1""")
                End Try


            Case "texgen"
                TexGen(CommandLineArgs(1), 0)

            Case "texgenwire"
                TexGen(CommandLineArgs(1), 1)

            Case "texgenmix"
                TexGen(CommandLineArgs(1), 2)

            Case "flip"
                Mirror(CommandLineArgs(1))


            Case "vflip"
                If CommandLineArgs.Length > 1 Then
                    Resize(CommandLineArgs(1), 1, -1, 1)
                Else
                    Output2("needs prm/m file path as second parameter")

                End If

            Case "hflip"
                If CommandLineArgs.Length > 1 Then
                    Resize(CommandLineArgs(1), -1, 1, 1)
                Else
                    Output2("needs prm/m file path as second parameter")

                End If


            Case "center"
                If CommandLineArgs.Length > 2 Then
                    Dim x, y, z As Boolean
                    x = False
                    y = False
                    z = False

                    If IO.File.Exists(CommandLineArgs(1)) Then

                        If InStr(CommandLineArgs(2), "x", CompareMethod.Text) > 0 Then
                            x = True
                        End If
                        If InStr(CommandLineArgs(2), "y", CompareMethod.Text) > 0 Then
                            y = True
                        End If
                        If InStr(CommandLineArgs(2), "z", CompareMethod.Text) > 0 Then
                            z = True
                        End If

                        CenterIt(CommandLineArgs(1), x, y, z)
                    Else

                        If IO.File.Exists(CommandLineArgs(2)) = False Then
                            Output2("ERROR: " & CommandLineArgs(2) & " not found")
                            Exit Sub
                        End If

                        If InStr(CommandLineArgs(1), "x", CompareMethod.Text) > 0 Then
                            x = True
                        End If
                        If InStr(CommandLineArgs(1), "y", CompareMethod.Text) > 0 Then
                            y = True
                        End If
                        If InStr(CommandLineArgs(1), "z", CompareMethod.Text) > 0 Then
                            z = True
                        End If

                        CenterIt(CommandLineArgs(2), x, y, z)

                    End If



                Else
                    CenterIt(CommandLineArgs(1), True, True, True)

                End If

            Case "centeroid", "centroid"
                If CommandLineArgs.Length > 2 Then
                    Dim x, y, z As Boolean
                    x = False
                    y = False
                    z = False

                    If IO.File.Exists(CommandLineArgs(1)) Then

                        If InStr(CommandLineArgs(2), "x", CompareMethod.Text) > 0 Then
                            x = True
                        End If
                        If InStr(CommandLineArgs(2), "y", CompareMethod.Text) > 0 Then
                            y = True
                        End If
                        If InStr(CommandLineArgs(2), "z", CompareMethod.Text) > 0 Then
                            z = True
                        End If

                        CenterItCentroid(CommandLineArgs(1), x, y, z)
                    Else

                        If IO.File.Exists(CommandLineArgs(2)) = False Then
                            Output2("ERROR: " & CommandLineArgs(2) & " not found")
                            Exit Sub
                        End If

                        If InStr(CommandLineArgs(1), "x", CompareMethod.Text) > 0 Then
                            x = True
                        End If
                        If InStr(CommandLineArgs(1), "y", CompareMethod.Text) > 0 Then
                            y = True
                        End If
                        If InStr(CommandLineArgs(1), "z", CompareMethod.Text) > 0 Then
                            z = True
                        End If

                        CenterItCentroid(CommandLineArgs(2), x, y, z)

                    End If



                Else
                    CenterItCentroid(CommandLineArgs(1), True, True, True)

                End If

            Case "double", "dblsd", "doubleside"
                MakeDoubleSide(CommandLineArgs(1))
            Case "single"
                MakeOneSide(CommandLineArgs(1))

            Case "opacity"
                setAlpha(CommandLineArgs(1), CommandLineArgs(2))


            Case "trans"
                setAlpha(CommandLineArgs(1), 1 - CommandLineArgs(2))

            Case "uv"
                UV(CommandLineArgs(1))

            Case "remap"
                Dim from As Integer, _To As Integer

                If Asc(UCase(CommandLineArgs(2))) >= 65 Then
                    from = Asc(UCase(CommandLineArgs(2))) - 65
                Else
                    from = CommandLineArgs(2)
                End If

                If Asc(UCase(CommandLineArgs(3))) >= 65 Then
                    _To = Asc(UCase(CommandLineArgs(3))) - 65
                Else
                    _To = CommandLineArgs(3)
                End If
                Remap(CommandLineArgs(1), from, _To)

            Case "resize", "scale"

                If CommandLineArgs.Length < 4 Then

                    If IsNumeric(CommandLineArgs(2)) Then
                        Resize(CommandLineArgs(1), CommandLineArgs(2), CommandLineArgs(2), CommandLineArgs(2))
                    Else
                        Dim SizeF As Single = 1.0
                        Select Case LCase(CommandLineArgs(2))
                            Case "-nfs4"
                                SizeF = 0.8
                            Case "-double"
                                SizeF = 2
                            Case "-half"
                                SizeF = 0.5
                            Case "-tiny"
                                SizeF = 0.001
                            Case "-zmod"
                                SizeF = 3.33
                        End Select
                        Resize(CommandLineArgs(1), SizeF, SizeF, SizeF)

                    End If
                Else
                    Resize(CommandLineArgs(1), CommandLineArgs(2), CommandLineArgs(3), CommandLineArgs(4))
                End If


            Case "*tex1"
                tex(CommandLineArgs(1))


            Case "load"
                load(CommandLineArgs(1))


            Case "envon"
                envOn(CommandLineArgs(1))
            Case "envoff"
                envOFF(CommandLineArgs(1))





            Case "restore"
                Try
                    FileCopy(CommandLineArgs(1) & ".bck", CommandLineArgs(1))
                    Output("Restored file")
                Catch ex As IO.IOException
                    Output2("ERROR: either file is in use or doesn't exist" & CommandLineArgs(1) & ".bck")
                Catch ex As Exception

                    Output2("Incorrect syntax: restore file.prm")
                End Try
            Case Else
                Output2("ERROR: Unknown command " & CommandLineArgs(0))

        End Select

        If Command(0) <> "*" Then EndIt()

    End Sub

    Sub Output(ByVal mstr As String)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("+ " & mstr & Space(76 - Len(mstr)) + "+")
        '	console.ForegroundColor = consolecolor.Gray

    End Sub

    Sub Output2(ByVal mstr As String)
        If Len(mstr) > 76 Then
            Output2(Mid(mstr, 1, 76))
            Output2(Mid(mstr, 77))
            Exit Sub
        End If
        Console.ForegroundColor = ConsoleColor.Black
        Console.BackgroundColor = ConsoleColor.White
        Console.WriteLine("+ " & mstr & Space(76 - Len(mstr)) + "+")
        Console.ForegroundColor = ConsoleColor.Gray
        Console.BackgroundColor = ConsoleColor.Black
    End Sub

    Sub Err(ByVal mstr As String)
        Console.ForegroundColor = ConsoleColor.Red
        Console.WriteLine("+ " & mstr & Space(76 - Len(mstr)) + "+")
        Console.ForegroundColor = ConsoleColor.Gray
    End Sub

    Sub Line()
        Console.WriteLine("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+")
    End Sub

    Sub EndIt()
        Console.ForegroundColor = ConsoleColor.Gray
        Console.WriteLine("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+")
        '   Console.WriteLine("                                                   Press any key to continue...")
        '  Console.ReadKey()
        End
    End Sub

    Sub Logo()
        Console.WriteLine("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+")
        Console.WriteLine("+    PRM Tools                                                        v0.02   +")
        Console.WriteLine("+                                        The Remake of Chaos tools, rvminis   +")
        Console.WriteLine("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+")
    End Sub

    Sub showHelp()

        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("+ wrong usage                                                                 +")
        Console.ForegroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("+ prm filename command (command arguments)                                    +")
        Console.WriteLine("+ Write 'prm help' to get the full documentation                              +")

    End Sub

    Sub CountVerticesAndShow(ByVal path$)
        Dim model As New prm.Car_Model(path)

        Output("   Vertex count: " & model.MyModel.vertnum)
        Output("   Polies count: " & model.MyModel.polynum)

    End Sub

    Sub MakeDoubleSide(ByVal path$)
        Output("making selected prm double sided")
        Dim model As New prm.Car_Model(path)
        Dim i As Integer
        For i = 0 To model.MyModel.polynum - 1
            If Not (model.MyModel.polyl(i).type And &H2) Then
                model.MyModel.polyl(i).type += 2
            End If
        Next
        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub
    Sub CenterIt(ByVal path$, ByVal X As Boolean, ByVal Y As Boolean, ByVal z As Boolean)
        Output("Calculating BBOX")
        Dim model As New prm.Car_Model(path)
        Dim i As Integer

        Dim xmin, xmax, ymin, ymax, zmin, zmax As Single

        xmin = +Single.PositiveInfinity
        ymin = +Single.PositiveInfinity
        zmin = +Single.PositiveInfinity

        xmax = Single.NegativeInfinity
        ymax = Single.NegativeInfinity
        zmax = Single.NegativeInfinity

        For i = 0 To model.MyModel.vertnum - 1

            xmin = If(model.MyModel.vexl(i).Position.x < xmin, model.MyModel.vexl(i).Position.x, xmin)
            ymin = If(model.MyModel.vexl(i).Position.y < ymin, model.MyModel.vexl(i).Position.y, ymin)
            zmin = If(model.MyModel.vexl(i).Position.z < zmin, model.MyModel.vexl(i).Position.z, zmin)

            xmax = If(model.MyModel.vexl(i).Position.x > xmax, model.MyModel.vexl(i).Position.x, xmax)
            ymax = If(model.MyModel.vexl(i).Position.y > ymax, model.MyModel.vexl(i).Position.y, ymax)
            zmax = If(model.MyModel.vexl(i).Position.z > zmax, model.MyModel.vexl(i).Position.z, zmax)




        Next


        Output("Calculating Center of BBOX")
        Output("    Center: (" & (xmin + xmax) / 2 & "," & (ymax + ymin) / 2 & "," & (zmax + zmin) / 2 & ")")

        Dim VectorPos As New prm.Car_Model.Vector3D(-(xmin + xmax) / 2, -(ymin + ymax) / 2, -(zmin + zmax) / 2)


        Output("Centering...")

        For i = 0 To model.MyModel.vertnum - 1
            model.MyModel.vexl(i).Position.x += VectorPos.x
            model.MyModel.vexl(i).Position.y += VectorPos.y
            model.MyModel.vexl(i).Position.z += VectorPos.z

        Next





        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub


    Sub CenterItCentroid(ByVal path$, ByVal X As Boolean, ByVal Y As Boolean, ByVal z As Boolean)
        Output("Calculating BBOX")
        Dim model As New prm.Car_Model(path)
        Dim i As Integer


        Dim centX As Double = 0, centY As Double = 0, centZ As Double = 0

        Output("Calculating Centeroid")
        For i = 0 To model.MyModel.vertnum - 1
            centX += model.MyModel.vexl(i).Position.x
            centY += model.MyModel.vexl(i).Position.y
            centZ += model.MyModel.vexl(i).Position.z
        Next
        centX /= model.MyModel.vertnum
        centY /= model.MyModel.vertnum
        centZ /= model.MyModel.vertnum

        Output("    Center: (" & centX & "," & centY & "," & centZ & ")")

        Dim VectorPos As New prm.Car_Model.Vector3D(-centX, -centY, -centZ)


        Output("Centering...")

        For i = 0 To model.MyModel.vertnum - 1
            model.MyModel.vexl(i).Position.x += VectorPos.x
            model.MyModel.vexl(i).Position.y += VectorPos.y
            model.MyModel.vexl(i).Position.z += VectorPos.z

        Next





        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub
    Sub MakeOneSide(ByVal path$)
        Output("making selected prm one sided")
        Dim model As New prm.Car_Model(path)
        Dim i As Integer
        For i = 0 To model.MyModel.polynum - 1
            If (model.MyModel.polyl(i).type And &H2) Then
                model.MyModel.polyl(i).type -= &H2
            End If
        Next
        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub

    Sub tex(ByVal path$)
        Dim model As New prm.Car_Model(path)
        Dim n As Integer
        Dim cnt(31) As String
        For n = 0 To model.MyModel.polynum - 1
            cnt(model.MyModel.polyl(n).Tpage + 1) += 1
        Next
        Dim Max As Integer, TfP As Integer
        Max = 0
        For n = 1 To 31
            If Max < cnt(n) Then
                Max = cnt(n)
                TfP = n - 1
            End If
        Next

        Console.WriteLine(TfP)

    End Sub

    Sub Resize(ByVal path$, ByVal timeX As Single, ByVal timeY As Single, ByVal timeZ As Single)
        Output("Resizing to x:" & timeX & ", y:" & timeY & ", z: " & timeZ)
        Dim model As New prm.Car_Model(path)
        Dim i As Integer

        Dim normLen#

        For i = 0 To model.MyModel.vertnum - 1

            model.MyModel.vexl(i).Position.x *= timeX
            model.MyModel.vexl(i).Position.y *= timeY
            model.MyModel.vexl(i).Position.z *= timeZ


            ' Normalise()
            model.MyModel.vexl(i).normal.x *= -timeX
            model.MyModel.vexl(i).normal.y *= timeY
            model.MyModel.vexl(i).normal.z *= timeZ

            normLen# = Sqrt(model.MyModel.vexl(i).normal.x ^ 2 + model.MyModel.vexl(i).normal.y ^ 2 + model.MyModel.vexl(i).normal.z ^ 2)

            model.MyModel.vexl(i).normal.x /= normLen#
            model.MyModel.vexl(i).normal.y /= normLen#
            model.MyModel.vexl(i).normal.z /= normLen#

        Next


        'FIX: resize the normals too

        For i = 0 To model.MyModel.vertnum - 1

        Next
       

        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub


    Sub load(ByVal path$)
        Output("making selected prm one sided")
        Dim model As New prm.Car_Model(path)
        MsgBox(Color.FromArgb(model.MyModel.polyl(0).c0).A)
    End Sub

    Sub setAlpha(ByVal path$, ByVal alpha$)
        Output("Setting alpha to " & alpha & "%")
        Dim model As New prm.Car_Model(path)
        Dim i As Integer
        For i = 0 To model.MyModel.polynum - 1
            Dim mycolor As Color

            If LCase(My.Application.CommandLineArgs(2)) = "add" Then

                If Not (model.MyModel.polyl(i).type And &H4) Then
                    model.MyModel.polyl(i).type += &H4
                End If

            ElseIf LCase(My.Application.CommandLineArgs(2)) = "noadd" Then
                If (model.MyModel.polyl(i).type And &H4) Then
                    model.MyModel.polyl(i).type -= &H4
                End If
            Else
                Dim a As Single = CSng(alpha)
                '	If Not (model.MyModel.polyl(i).type And &H0004) Then
                '		model.MyModel.polyl(i).type &= &H0004
                '	End If
                mycolor = Color.FromArgb((a / 100) * 255, Color.FromArgb(model.MyModel.polyl(i).c0))
                model.MyModel.polyl(i).c0 = mycolor.ToArgb

                mycolor = Color.FromArgb((a / 100) * 255, Color.FromArgb(model.MyModel.polyl(i).c1))
                model.MyModel.polyl(i).c1 = mycolor.ToArgb

                mycolor = Color.FromArgb((a / 100) * 255, Color.FromArgb(model.MyModel.polyl(i).c2))
                model.MyModel.polyl(i).c2 = mycolor.ToArgb

                mycolor = Color.FromArgb((a / 100) * 255, Color.FromArgb(model.MyModel.polyl(i).c3))
                model.MyModel.polyl(i).c3 = mycolor.ToArgb

                FileCopy(path, path & ".bck")
                Kill(path)
                model.Export(path)
            End If
        Next

    End Sub

    Sub Remap(ByVal path$, ByVal from As Integer, ByVal _To As Integer)

        Output("Remapping")
        Dim model As New prm.Car_Model(path)
        Dim i As Integer
        For i = 0 To model.MyModel.polynum - 1
            If (model.MyModel.polyl(i).Tpage = from) Then
                model.MyModel.polyl(i).Tpage = _To
            End If
        Next
        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub


    Sub UV(ByVal path$)
        Output("generating UV coordinates file")
        Dim model As New prm.Car_Model(path)
        Dim i As Integer
        Dim FileUV As String = ""
        For i = 0 To model.MyModel.polynum - 1
            FileUV &= model.MyModel.polyl(i).u0 & "_" & model.MyModel.polyl(i).v0 & "_" & _
              model.MyModel.polyl(i).u1 & "_" & model.MyModel.polyl(i).v1 & "_" & _
              model.MyModel.polyl(i).u2 & "_" & model.MyModel.polyl(i).v2 & "_" & _
              model.MyModel.polyl(i).u3 & "_" & model.MyModel.polyl(i).v3 & "\"
        Next
        IO.File.WriteAllText(Environ("temp") & "\fileUV.txt", FileUV)


        Output2("Done!")
    End Sub




    Sub envOn(ByVal path$)
        Output("making selected prm ENV on")
        Dim model As New prm.Car_Model(path)
        Dim i As Integer
        For i = 0 To model.MyModel.polynum - 1
            If (model.MyModel.polyl(i).type And 1024) Then
                model.MyModel.polyl(i).type -= 1024
            End If
        Next
        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub

    Sub envOFF(ByVal path$)
        Output("making selected prm ENV off")
        Dim model As New prm.Car_Model(path)
        Dim i As Integer
        For i = 0 To model.MyModel.polynum - 1
            If Not (model.MyModel.polyl(i).type And 1024) Then
                model.MyModel.polyl(i).type += 1024
            End If
        Next
        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")

        ''
    End Sub
    Sub Translate(ByVal path$, ByVal X As Integer, ByVal Y As Integer, ByVal z As Integer)


        Dim model As New prm.Car_Model(path)


        Output("translating...")

        For i = 0 To model.MyModel.vertnum - 1
            model.MyModel.vexl(i).Position.x += X
            model.MyModel.vexl(i).Position.y += Y
            model.MyModel.vexl(i).Position.z += z

        Next


        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub
    Sub Rotate(ByVal path$, ByVal Method As Integer, ByVal x!, ByVal y!, ByVal z!, Optional ByVal cx! = 0, Optional ByVal cy! = 0, Optional ByVal cz! = 0)

        Dim model As New prm.Car_Model(path)
        Dim i As Integer

        Select Case Method
            Case 0 'origin
                'Do nothing, cx=cy=cz=0
            Case 1 'bbox
                Dim xmin, xmax, ymin, ymax, zmin, zmax As Single

                xmin = +Single.PositiveInfinity
                ymin = +Single.PositiveInfinity
                zmin = +Single.PositiveInfinity

                xmax = Single.NegativeInfinity
                ymax = Single.NegativeInfinity
                zmax = Single.NegativeInfinity

                For i = 0 To model.MyModel.vertnum - 1

                    xmin = If(model.MyModel.vexl(i).Position.x < xmin, model.MyModel.vexl(i).Position.x, xmin)
                    ymin = If(model.MyModel.vexl(i).Position.y < ymin, model.MyModel.vexl(i).Position.y, ymin)
                    zmin = If(model.MyModel.vexl(i).Position.z < zmin, model.MyModel.vexl(i).Position.z, zmin)

                    xmax = If(model.MyModel.vexl(i).Position.x > xmax, model.MyModel.vexl(i).Position.x, xmax)
                    ymax = If(model.MyModel.vexl(i).Position.y > ymax, model.MyModel.vexl(i).Position.y, ymax)
                    zmax = If(model.MyModel.vexl(i).Position.z > zmax, model.MyModel.vexl(i).Position.z, zmax)




                Next
                cx = (xmin + xmax) / 2
                cy = (ymin + ymax) / 2
                cz = (zmin + zmax) / 2



            Case 2 'Centroid
                Output("Calculating Centeroid")
                For i = 0 To model.MyModel.vertnum - 1
                    cx += model.MyModel.vexl(i).Position.x
                    cy += model.MyModel.vexl(i).Position.y
                    cz += model.MyModel.vexl(i).Position.z
                Next
                cx /= model.MyModel.vertnum
                cy /= model.MyModel.vertnum
                cz /= model.MyModel.vertnum
            Case 3 'precised
                'Also Do nothing
        End Select


        Output2("need to check?: have rotation code")

        'for viewability sake, keep the notations as they are
        Dim Phi!, Gamma!, varPhi!
        Phi = x * PI / 180
        Gamma = y * PI / 180
        varPhi = z * PI / 180


        'We can complicate it more than we want, but let's make it easy ok?
        For i = 0 To model.MyModel.vertnum - 1
            model.MyModel.vexl(i).Position.x -= cx
            model.MyModel.vexl(i).Position.y -= cy
            model.MyModel.vexl(i).Position.z -= cz
        Next

        'Then rotational feelingz
        Dim newX#, newY#, newZ#
        For i = 0 To model.MyModel.vertnum - 1

            vecRot(model.MyModel.vexl(i).Position.x, model.MyModel.vexl(i).Position.y, model.MyModel.vexl(i).Position.z, _
                   Phi, Gamma, varPhi, _
                   newX, newY, newZ)
            model.MyModel.vexl(i).Position.x = newX
            model.MyModel.vexl(i).Position.y = newY
            model.MyModel.vexl(i).Position.z = newZ


            vecRot(model.MyModel.vexl(i).normal.x, model.MyModel.vexl(i).normal.y, model.MyModel.vexl(i).normal.z, _
                   Phi, Gamma, varPhi, _
                   newX, newY, newZ)
            model.MyModel.vexl(i).normal.x = newX
            model.MyModel.vexl(i).normal.y = newY
            model.MyModel.vexl(i).normal.z = newZ
        

        Next


        'We can complicate it more than we want, but let's make it easy ok?
        For i = 0 To model.MyModel.vertnum - 1
            model.MyModel.vexl(i).Position.x += cx
            model.MyModel.vexl(i).Position.y += cy
            model.MyModel.vexl(i).Position.z += cz
        Next


        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")


    End Sub

    Sub vecRot(ByVal x#, ByVal y#, ByVal z#, _
                    ByVal phi#, ByVal Gamma#, ByVal varPhi#, _
                    ByRef newX#, ByRef newY#, ByRef newZ#)
        newX = x * (Cos(Phi) * Cos(varPhi) - Sin(Phi) * Sin(varPhi) * Cos(Gamma)) + _
               y * (-Cos(Phi) * Sin(varPhi) - Sin(Phi) * Cos(varPhi) * Cos(Gamma)) + _
               z * Sin(varPhi) * Sin(Gamma)
        newY = x * (Sin(Phi) * Cos(varPhi) + Cos(Phi) * Sin(varPhi) * Cos(Gamma)) + _
               y * (-Sin(Phi) * Sin(varPhi) + Cos(Phi) * Cos(varPhi) * Cos(Gamma)) + _
               z * -Cos(Phi) * Sin(Gamma)
        newZ = x * Sin(phi) * Sin(Gamma) + _
                y * Cos(phi) * Sin(Gamma) + _
                z * Cos(Gamma)
    End Sub
    Sub Normalize(ByVal path$, ByVal X As Single, ByVal Y As Single, ByVal z As Single, Optional ByVal weMultiply As Boolean = True)


        Dim model As New prm.Car_Model(path)


        Output("Multiplying normals... by " & X & ", " & Y & ", " & z)
        Dim normLen#

        If weMultiply Then 'We multiply

            For i = 0 To model.MyModel.vertnum - 1
                model.MyModel.vexl(i).normal.x *= X
                model.MyModel.vexl(i).normal.y *= Y
                model.MyModel.vexl(i).normal.z *= z

                'normalize
                normLen# = Sqrt(model.MyModel.vexl(i).normal.x ^ 2 + model.MyModel.vexl(i).normal.y ^ 2 + model.MyModel.vexl(i).normal.z ^ 2)

                model.MyModel.vexl(i).normal.x /= normLen#
                model.MyModel.vexl(i).normal.y /= normLen#
                model.MyModel.vexl(i).normal.z /= normLen#

                '  Output(model.MyModel.vexl(i).normal.x & "," & model.MyModel.vexl(i).normal.y & "," & model.MyModel.vexl(i).normal.z)

            Next
        Else 'we overwrite (setNormal)

            For i = 0 To model.MyModel.vertnum - 1
                model.MyModel.vexl(i).normal.x = X
                model.MyModel.vexl(i).normal.y = Y
                model.MyModel.vexl(i).normal.z = z

                'normalize
                normLen# = Sqrt(model.MyModel.vexl(i).normal.x ^ 2 + model.MyModel.vexl(i).normal.y ^ 2 + model.MyModel.vexl(i).normal.z ^ 2)

                model.MyModel.vexl(i).normal.x /= normLen#
                model.MyModel.vexl(i).normal.y /= normLen#
                model.MyModel.vexl(i).normal.z /= normLen#

                '   Output(model.MyModel.vexl(i).normal.x & "," & model.MyModel.vexl(i).normal.y & "," & model.MyModel.vexl(i).normal.z)

            Next


        End If


        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub
    Sub Mirror(ByVal path$)

        Dim model As New prm.Car_Model(path)


        Output("Flipping normals...")


        For i = 0 To model.MyModel.polynum - 1
            If Not (model.MyModel.polyl(i).type And 1) Then 'tri

                swap(model.MyModel.polyl(i).vi0, _
                 model.MyModel.polyl(i).vi2)
                swap(model.MyModel.polyl(i).u0, _
                 model.MyModel.polyl(i).u2)
                swap(model.MyModel.polyl(i).v0, _
                model.MyModel.polyl(i).v2)
                swap(model.MyModel.polyl(i).c0, _
                model.MyModel.polyl(i).c2)




            Else
                swap(model.MyModel.polyl(i).vi1, _
                model.MyModel.polyl(i).vi2)
                swap(model.MyModel.polyl(i).u1, _
                 model.MyModel.polyl(i).u2)
                swap(model.MyModel.polyl(i).v1, _
                model.MyModel.polyl(i).v2)
                swap(model.MyModel.polyl(i).c1, _
                model.MyModel.polyl(i).c2)


                swap(model.MyModel.polyl(i).vi0, _
                model.MyModel.polyl(i).vi3)
                swap(model.MyModel.polyl(i).u0, _
                 model.MyModel.polyl(i).u3)
                swap(model.MyModel.polyl(i).v0, _
                model.MyModel.polyl(i).v3)
                swap(model.MyModel.polyl(i).c0, _
                model.MyModel.polyl(i).c3)



            End If



        Next


        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub
    Sub BreakNormal(ByVal path$, ByVal modePlanar As Boolean)

        Dim model As New prm.Car_Model(path)

        Output("Breaking normals....")

        Dim normLen#

        If Not modePlanar Then 'Vertex 
            For i = 0 To model.MyModel.vertnum - 1
                normLen = Sqrt( _
                model.MyModel.vexl(i).Position.x ^ 2 + _
                model.MyModel.vexl(i).Position.y ^ 2 + _
                model.MyModel.vexl(i).Position.z ^ 2)



                model.MyModel.vexl(i).normal.x = model.MyModel.vexl(i).Position.x / normLen#
                model.MyModel.vexl(i).normal.y = model.MyModel.vexl(i).Position.y / normLen#
                model.MyModel.vexl(i).normal.z = model.MyModel.vexl(i).Position.z / normLen#
            Next
        Else 'Normal by plane
            Dim nx#, ny#, nz#
            For i = 0 To model.MyModel.polynum - 1

                nx = model.MyModel.vexl(model.MyModel.polyl(i).vi0).Position.x + _
                    model.MyModel.vexl(model.MyModel.polyl(i).vi1).Position.x + _
                    model.MyModel.vexl(model.MyModel.polyl(i).vi2).Position.x

                ny = model.MyModel.vexl(model.MyModel.polyl(i).vi0).Position.y + _
                  model.MyModel.vexl(model.MyModel.polyl(i).vi1).Position.y + _
                  model.MyModel.vexl(model.MyModel.polyl(i).vi2).Position.y

                nz = model.MyModel.vexl(model.MyModel.polyl(i).vi0).Position.z + _
                  model.MyModel.vexl(model.MyModel.polyl(i).vi1).Position.z + _
                  model.MyModel.vexl(model.MyModel.polyl(i).vi2).Position.z

                nx /= 3 : ny /= 3 : nz /= 3

                normLen# = Sqrt(nx ^ 2 + ny ^ 2 + nz ^ 2)

                model.MyModel.vexl(model.MyModel.polyl(i).vi0).normal.x = nx / normLen
                model.MyModel.vexl(model.MyModel.polyl(i).vi0).normal.y = ny / normLen
                model.MyModel.vexl(model.MyModel.polyl(i).vi0).normal.z = nz / normLen


                model.MyModel.vexl(model.MyModel.polyl(i).vi1).normal.x = nx / normLen
                model.MyModel.vexl(model.MyModel.polyl(i).vi1).normal.y = ny / normLen
                model.MyModel.vexl(model.MyModel.polyl(i).vi1).normal.z = nz / normLen


                model.MyModel.vexl(model.MyModel.polyl(i).vi2).normal.x = nx / normLen
                model.MyModel.vexl(model.MyModel.polyl(i).vi2).normal.y = ny / normLen
                model.MyModel.vexl(model.MyModel.polyl(i).vi2).normal.z = nz / normLen

                If model.MyModel.polyl(i).type And 1 Then


                    model.MyModel.vexl(model.MyModel.polyl(i).vi3).normal.x = nx / normLen
                    model.MyModel.vexl(model.MyModel.polyl(i).vi3).normal.y = ny / normLen
                    model.MyModel.vexl(model.MyModel.polyl(i).vi3).normal.z = nz / normLen
                End If

            Next


        End If




        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub
    Sub swap(ByRef X As Object, ByRef Y As Object)
        Dim v = X
        X = Y
        Y = v
        v = Nothing

    End Sub

    Sub autoShade(ByVal path$, ByVal ByPolgon As Boolean, Optional ByVal invert As Integer = 0)


        Dim model As New prm.Car_Model(path)

        Output("Shading ....")

        If invert Then GoTo invertedShade


        If Not ByPolgon Then 'Vertex 
            For i = 0 To model.MyModel.polynum - 1

                model.MyModel.polyl(i).c0 = RGBToLong(Int( _
                Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi0).normal.y) * 255))
                model.MyModel.polyl(i).c1 = RGBToLong(Int( _
                 Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi1).normal.y) * 255))
                model.MyModel.polyl(i).c2 = RGBToLong(Int( _
                Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi2).normal.y) * 255))

                If model.MyModel.polyl(i).type And 1 Then _
                model.MyModel.polyl(i).c2 = RGBToLong(Int( _
               Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi2).normal.y) * 255))


            Next



        Else 'Normal by plane

            For i = 0 To model.MyModel.polynum - 1

                Dim avgNormal# = _
                    Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi0).normal.y) + _
                   Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi1).normal.y) + _
                    Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi2).normal.y)

                avgNormal# /= 3
                model.MyModel.polyl(i).c0 = RGBToLong(Int(avgNormal# * 255))
                model.MyModel.polyl(i).c1 = RGBToLong(Int(avgNormal# * 255))
                model.MyModel.polyl(i).c2 = RGBToLong(Int(avgNormal# * 255))

                If model.MyModel.polyl(i).type And 1 Then
                    model.MyModel.polyl(i).c3 = RGBToLong(Int(avgNormal# * 255))
                End If




            Next


        End If




        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")


        Exit Sub 'Non inverted stops here, after this shade 



invertedShade:
        If Not ByPolgon Then 'Vertex 
            For i = 0 To model.MyModel.polynum - 1

                model.MyModel.polyl(i).c0 = RGBToLong(255 - Int( _
                Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi0).normal.y) * 255))
                model.MyModel.polyl(i).c1 = RGBToLong(255 - Int( _
                 Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi1).normal.y) * 255))
                model.MyModel.polyl(i).c2 = RGBToLong(255 - Int( _
                Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi2).normal.y) * 255))

                If model.MyModel.polyl(i).type And 1 Then _
                model.MyModel.polyl(i).c2 = RGBToLong(255 - Int( _
               Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi2).normal.y) * 255))


            Next



        Else 'Normal by plane

            For i = 0 To model.MyModel.polynum - 1

                Dim avgNormal# = _
                    Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi0).normal.y) + _
                   Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi1).normal.y) + _
                    Abs(model.MyModel.vexl(model.MyModel.polyl(i).vi2).normal.y)

                avgNormal# /= 3
                avgNormal# = 1 - avgNormal#
                model.MyModel.polyl(i).c0 = RGBToLong(Int(avgNormal# * 255))
                model.MyModel.polyl(i).c1 = RGBToLong(Int(avgNormal# * 255))
                model.MyModel.polyl(i).c2 = RGBToLong(Int(avgNormal# * 255))

                If model.MyModel.polyl(i).type And 1 Then
                    model.MyModel.polyl(i).c3 = RGBToLong(Int(avgNormal# * 255))
                End If




            Next


        End If





        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub
    Sub rvShade(ByVal path$, ByVal R%, ByVal G%, ByVal B%, Optional ByVal A% = 255)

        Dim model As New prm.Car_Model(path)

        Output("Shading ....")


        If A = 255 Then
            For i = 0 To model.MyModel.polynum - 1
                model.MyModel.polyl(i).c0 = RGBToLong(255, R, G, B)
                model.MyModel.polyl(i).c1 = RGBToLong(255, R, G, B)
                model.MyModel.polyl(i).c2 = RGBToLong(255, R, G, B)
                model.MyModel.polyl(i).c3 = RGBToLong(255, R, G, B)
                ' model.MyModel.polyl(i).type = model.MyModel.polyl(i).type Xor 4
            Next
        Else
            For i = 0 To model.MyModel.polynum - 1
                model.MyModel.polyl(i).c0 = RGBToLong(A, R, G, B)
                model.MyModel.polyl(i).c1 = RGBToLong(A, R, G, B)
                model.MyModel.polyl(i).c2 = RGBToLong(A, R, G, B)
                model.MyModel.polyl(i).c3 = RGBToLong(A, R, G, B)
                model.MyModel.polyl(i).type = model.MyModel.polyl(i).type Or 4
            Next

        End If

        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")
    End Sub
    Sub remapUV(ByVal path$, ByVal srcTex%, ByVal srcLTx#, ByVal srcLTy#, ByVal srcRBx#, ByVal srcRBy#, _
                ByVal dstTex%, ByVal dstLTx#, ByVal dstLTy#, ByVal dstRBx#, ByVal dstRBy#)
        Dim Sx#, Tx#, Sy#, Ty#
        'srcLTx - srcRBx error?
        Try
            Sx = (dstLTx - dstRBx) / (srcLTx - srcRBx)
            Tx = dstLTx - srcLTx
            Sy = (dstLTy - dstRBy) / (srcLTy - srcRBy)
            Ty = dstLTy - srcLTy

        Catch ex As Exception

            Output2("ERROR: Something went wrong and I'm not going to do anything to PRM")
            End
        End Try

        Dim model As New prm.Car_Model(path)

        Output("Remapping ....")



        For i = 0 To model.MyModel.polynum - 1

            If model.MyModel.polyl(i).Tpage <> srcTex% And srcTex <> -1 Then Continue For

            If Not isInRange(model.MyModel.polyl(i).u0, srcLTx#, srcRBx) Then Continue For
            If Not isInRange(model.MyModel.polyl(i).u1, srcLTx#, srcRBx) Then Continue For
            If Not isInRange(model.MyModel.polyl(i).u2, srcLTx#, srcRBx) Then Continue For
            If model.MyModel.polyl(i).type And 1 Then If Not isInRange(model.MyModel.polyl(i).u3, srcLTx#, srcRBx) Then Continue For


            If Not isInRange(model.MyModel.polyl(i).v0, srcLTy#, srcRBy) Then Continue For
            If Not isInRange(model.MyModel.polyl(i).v1, srcLTy#, srcRBy) Then Continue For
            If Not isInRange(model.MyModel.polyl(i).v2, srcLTy#, srcRBy) Then Continue For
            If model.MyModel.polyl(i).type And 1 Then If Not isInRange(model.MyModel.polyl(i).v3, srcLTy#, srcRBy) Then Continue For


            model.MyModel.polyl(i).u0 = Sx * (model.MyModel.polyl(i).u0 - srcLTx) + Tx
            model.MyModel.polyl(i).u1 = Sx * (model.MyModel.polyl(i).u1 - srcLTx) + Tx
            model.MyModel.polyl(i).u2 = Sx * (model.MyModel.polyl(i).u2 - srcLTx) + Tx
            model.MyModel.polyl(i).u3 = Sx * (model.MyModel.polyl(i).u3 - srcLTx) + Tx


            model.MyModel.polyl(i).v0 = Sy * (model.MyModel.polyl(i).v0 - srcLTy) + Ty
            model.MyModel.polyl(i).v1 = Sy * (model.MyModel.polyl(i).v1 - srcLTy) + Ty
            model.MyModel.polyl(i).v2 = Sy * (model.MyModel.polyl(i).v2 - srcLTy) + Ty
            model.MyModel.polyl(i).v3 = Sy * (model.MyModel.polyl(i).v3 - srcLTy) + Ty

            If dstTex <> -1 Then model.MyModel.polyl(i).Tpage = dstTex
        Next



        FileCopy(path, path & ".bck")
        Kill(path)
        model.Export(path)
        Output2("Done!")


    End Sub
    Sub TexGen(ByVal path$, ByVal Mode%)



        Dim model As New prm.Car_Model(path)

        Output("Generating Textures ....")

        Dim res = 8192 * 2

res:
        res /= 2
        Output("Resolution : " & res & " ....")
        Dim K As New Bitmap(res, res, Imaging.PixelFormat.Format32bppPArgb)
        Dim g As Graphics = Graphics.FromImage(K)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        Dim b As Brush = New SolidBrush(Color.Transparent)
        Dim r As Rectangle = New Rectangle(0, 0, 8192, 8192)
        Dim p As Pen = New Pen(Color.Gray)
        Dim bp As Brush = New SolidBrush(Color.Gray)



        g.FillRectangle(b, r)

        Dim ptf_tri(2) As PointF
        Dim ptf_qd(3) As PointF


        For i = 0 To model.MyModel.polynum - 1

            If model.MyModel.polyl(i).type And 1 Then
                ptf_qd(0) = New PointF(model.MyModel.polyl(i).u0 * res, model.MyModel.polyl(i).v0 * res)
                ptf_qd(1) = New PointF(model.MyModel.polyl(i).u1 * res, model.MyModel.polyl(i).v1 * res)
                ptf_qd(2) = New PointF(model.MyModel.polyl(i).u2 * res, model.MyModel.polyl(i).v2 * res)
                ptf_qd(3) = New PointF(model.MyModel.polyl(i).u3 * res, model.MyModel.polyl(i).v3 * res)

                If Mode% = 0 Then g.DrawPolygon(p, ptf_qd) Else g.FillPolygon(bp, ptf_qd)
                If Mode = 2 Then g.DrawPolygon(p, ptf_qd)
            Else
                ptf_tri(0) = New PointF(model.MyModel.polyl(i).u0 * res, model.MyModel.polyl(i).v0 * res)
                ptf_tri(1) = New PointF(model.MyModel.polyl(i).u1 * res, model.MyModel.polyl(i).v1 * res)
                ptf_tri(2) = New PointF(model.MyModel.polyl(i).u2 * res, model.MyModel.polyl(i).v2 * res)

                If Mode% = 0 Then g.DrawPolygon(p, ptf_tri) Else g.FillPolygon(bp, ptf_tri)
                If Mode = 2 Then g.DrawPolygon(p, ptf_tri)
            End If


        Next

        g.Flush()
        K.Save(path & "_" & res & ".png")


        If res >= 128 Then GoTo res




        Output2("Done!")


    End Sub
    Function isQuad(ByVal ptype%)
        Return ptype And 1
    End Function

    Function isInRange(ByVal x!, ByVal a!, ByVal b!)
        If x < a Or x > b Then Return False
        Return True
    End Function
End Module


Module GlobalModule

    '''''''''''''''''''''''''''
    'Global Variables
    '''''''''''''''''''''''
    Public Zoom = 1
    'Public CurrentWorld As WorldFile

    Public Function RGBToLong(ByVal color As Color) As Int32
        Return Convert.ToInt32(color.A) << 24 Or CUInt(color.R) << 16 Or CUInt(color.G) << 8 Or CUInt(color.B) << 0
    End Function
    Public Function RGBToLong(ByVal A%, ByVal R%, ByVal G%, ByVal B%) As Int32
        Return Convert.ToInt32(A) << 24 Or CUInt(R) << 16 Or CUInt(G) << 8 Or CUInt(B) << 0
    End Function
    Public Function RGBToLong(ByVal Gray%) As Int32
        Return Convert.ToInt32(255) << 24 Or CUInt(Gray) << 16 Or CUInt(Gray) << 8 Or CUInt(Gray) << 0
    End Function
    Public Function ColorsToRGB(ByVal cl As UInt32) As Color
        'long rgb value, is composed from 0~255 R, G, B
        'according to net: (2^8)^cn
        ' cn: R = 0 , G = 1, B = 2


        'simple...
        Dim a = cl >> 24

        If a = 0 Then a = 251


        ' 
        ' If a = 0 Then a = 255
        Dim r = cl >> 16 And &HFF

        Dim g = cl >> 8 And &HFF
        Dim b = cl >> 0 And &HFF


        Return Color.FromArgb(a, r, g, b)


    End Function

End Module