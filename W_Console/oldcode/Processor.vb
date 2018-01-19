Module Processor
    Sub Process(ByVal str$)

        If str.Length = 0 Then Exit Sub 'not valid

        If InStr(str, "{", CompareMethod.Text) Then str = Split(str, "{")(0)

        Try
            Do Until (LCase(str(0)) >= "a" And LCase(str(0)) <= "z") Or Len(str) = 0 Or str(0) = "#"  'clean
                str = str.Substring(1)
                'Application.DoEvents()
            Loop
        Catch
            Exit Sub
        End Try


        If str.Length = 0 Then Exit Sub 'not valid

        'variables and constants
        Dim last_file = GetSetting("Wconsole", "General", "Last_File", "")

        Dim cmd = getCommand(str)
        Dim vars = getAllVars(str)

        vars = Replace(vars, "last file", last_file, , , CompareMethod.Text)
        vars = Replace(vars, "lastfile", last_file, , , CompareMethod.Text)

        If cmd(0) = "#" Then Exit Sub

        Select Case LCase(cmd)
            Case "help"
                DoWrite(Replace("you'll have to load one world mesh and then go forward (that would mean to load one world, then to select the mesh)" & vbNewLine & _
                                vbTab & "load <path to world file> : allows to load the world file" & vbNewLine & _
                                vbTab & "count meshes: all meshes count, indexed from 0 to meshcount - 1" & vbNewLine & _
                                vbTab & "count polys: get polys count in the current mesh, you'll have to select a mesh before" & vbNewLine & _
                                vbTab & "select mesh <number>: select mesh" & vbNewLine & _
                                    "", vbNewLine, vbNewLine & "> "))


            Case "load", "open"
                If vars = "" Or vars = " " Then vars = last_file
                If IO.File.Exists(vars) Then
                    CurrentWorld = New WorldFile(vars)
                    If InStr(last_file, ":\", CompareMethod.Text) = 0 Then
                        last_file = CurDir() & last_file
                    End If
                    SaveSetting("Wconsole", "General", "Last_File", vars)

                Else
                    DoWrite("Error: File (" & vars & ") doesn't exist!")
                End If

            Case "script"
                If IO.File.Exists(vars) Then
                    Dim allCmd = IO.File.ReadAllLines(vars)
                    For i = 0 To allCmd.Length - 1
                        Process(allCmd(i))
                    Next
                Else
                    DoWrite("error, script is not found!")
                    Exit Sub
                End If

            Case "save"
                If CurrentWorld Is Nothing Then
                    DoWrite("Why not try to load the world first...")
                    Exit Sub
                End If
                If vars = " " Or vars = "" Then
                    vars = last_file
                End If

                CurrentWorld.Save(vars)

            Case "backup"
                If CurrentWorld Is Nothing Then
                    DoWrite("Error! please select a world file")
                    Exit Sub
                End If
                DoWrite("Saving a backup in progress....")
                Dim name$ = CurrentWorld.Directory & CurrentWorld.DirectoryName & "_" & IO.Directory.GetFiles(CurrentWorld.Directory, CurrentWorld.DirectoryName & "_*.w").Count & ".w"
                'Debugger.Break()
                My.Computer.FileSystem.CopyFile(last_file, name, True)
                DoWrite("saved:" & name)

            Case "restore"
                If (Not LCase(vars) = "first" Or IsNumeric(vars)) And vars IsNot Nothing Then
                    vars = Replace(vars, "first", "0", , , CompareMethod.Text)

                    DoWrite("Sending " & last_file & " to recycle bin")

                    My.Computer.FileSystem.DeleteFile(last_file, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    IO.File.Copy(CurrentWorld.Directory & CurrentWorld.DirectoryName & "_" & vars & ".w", last_file)

                    DoWrite("Restored")
                    DoWrite("Reloading file...")

                    Process("load")
                    Exit Sub

                End If
                DoWrite("Restoring a backup in progress....")
                Dim i = 0
                Do Until IO.File.Exists(last_file & "_" & IO.Directory.GetFiles(Left(last_file, last_file.LastIndexOf("\")) & "\", last_file.Split("\").Last & "_*.w").Count - i & ".w") Or i = -1
                    i -= 1
                Loop

                DoWrite("Sending " & last_file & " to recycle bin")

                If IO.File.Exists(last_file & "_" & IO.Directory.GetFiles(Left(last_file, last_file.LastIndexOf("\")) & "\", last_file.Split("\").Last & "_*.w").Count - i & ".w") Then
                    My.Computer.FileSystem.DeleteFile(last_file, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    IO.File.Copy(last_file & "_" & IO.Directory.GetFiles(Left(last_file, last_file.LastIndexOf("\")) & "\", last_file.Split("\").Last & "_*.w").Count - i & ".w", last_file)
                End If
                DoWrite("Restored the latest backup (" & IO.Directory.GetFiles(Left(last_file, last_file.LastIndexOf("\")) & "\", last_file.Split("\").Last & "_*.w").Count - i & ")")
                DoWrite("Reloading file...")

                Process("load")





            Case "count"
                If CurrentWorld Is Nothing Then
                    DoWrite("Error, no world is loaded")
                    Exit Sub
                End If
                Select Case LCase(vars)
                    Case "meshes", "mesh"
                        DoWrite("Mesh Count: " & CurrentWorld.meshCount)
                    Case "poly", "polys"
                        If CurrentMesh <> -1 Then
                            DoWrite(CurrentWorld.mMesh(CurrentMesh).polynum)
                        Else
                            DoWrite("All meshes' poly count:" & CurrentWorld.PolyCount)
                        End If
                    Case "ver", "vex", "vx", "vertex", "vertices"
                        If CurrentMesh <> -1 Then
                            DoWrite(CurrentWorld.mMesh(CurrentMesh).vertnum)
                        Else
                            DoWrite("All meshes' poly count:" & CurrentWorld.VexCount)
                        End If
                    Case Else
                        DoWrite("precise count <what> : meshes, polys or vertices")
                End Select

            Case "getbytex"
                If LCase(vars) >= "a" And LCase(vars) <= "z" Then
                    vars = Asc(CChar(UCase(vars))) - Asc("A")
                End If
                Try
                    For c = 0 To CurrentWorld.Bitmaps(Int(vars) + 1).Items.Count - 1
                        Dim inf$ = CurrentWorld.Bitmaps(Int(vars) + 1).Items(c)
                        Dim cpoly = CurrentWorld.mMesh(Split(inf, ",")(0)).polyl(Split(inf, ",")(1))

                        Dim Type$ = ""
                        If cpoly.type And 2 ^ 0 Then
                            Type += " [QUAD] "
                        End If
                        If cpoly.type And 2 ^ 1 Then
                            Type += " [DBLSD] "
                        End If
                        If cpoly.type And 2 ^ 2 Then
                            Type += " [TRANS] "
                        End If
                        If cpoly.type And 2 ^ 8 Then
                            Type += " [TTYPE] "
                        End If
                        If cpoly.type And 2 ^ 9 Then
                            Type += " [TEXANIM] "
                        End If
                        If cpoly.type And 2 ^ 11 Then
                            Type += " [ENV] "
                        End If

                        DoWrite("mesh: " & Split(inf, ",")(0) & " | Poly:" & Split(inf, ",")(1) & String.Format(" | Type: {0} | ", Type) & String.Format("Tex:{0} , uv: {1}x{2} {3}x{4} {5}x{6} {7}x{8}", Chr(cpoly.tpage + 65), Format(cpoly.u0, "0.#0"), Format(cpoly.v0, "0.#0"), Format(cpoly.u1, "0.#0"), Format(cpoly.v1, "0.#0"), Format(cpoly.u2, "0.#0"), Format(cpoly.v2, "0.#0"), Format(cpoly.u3, "0.#0"), Format(cpoly.v3, "0.#0")))
                    Next
                Catch
                    DoWrite("Error happened, maybe Out of range? Re-Volt only support 26 bitmaps + empty channel")
                End Try


            Case "select"
                Select Case LCase(Split(vars, " ")(0))
                    Case "mesh"

                        If vars = "mesh " Then
                            DoWrite("error, end of bounds")
                            Exit Sub
                        End If


                        CurrentMesh = Split(vars, " ")(1)
                        Form1.Label2.Text = "[[ Current Mesh : " & CurrentMesh & " ]]"
                        DoWrite("Selected mesh:" & CurrentMesh)
                    Case Else
                        If IsNumeric(vars) Then
                            CurrentMesh = vars
                            Form1.Label2.Text = "[[ Current Mesh : " & CurrentMesh & " ]]"
                            DoWrite("Selected mesh:" & CurrentMesh)
                        Else

                            DoWrite("select mesh <meshnumber>")
                        End If
                End Select



            Case "getinfo"
                If CurrentMesh = -1 Then
                    DoWrite("please select a mesh before; COMMAND: select mesh <mesh number>")
                    Exit Sub
                Else
                    If CurrentWorld Is Nothing Then
                        DoWrite("Please load a world file first")
                        Exit Sub
                    End If
                    DoWrite("poly count:" & CurrentWorld.mMesh(CurrentMesh).polynum)
                    DoWrite("Vx count:" & CurrentWorld.mMesh(CurrentMesh).vertnum)
                    DoWrite("First Poly is Quad?:" & CBool(CurrentWorld.mMesh(CurrentMesh).polyl(0).type And 1))
                    DoWrite("First Poly has TEXANIM?:" & CBool(CurrentWorld.mMesh(CurrentMesh).polyl(0).type And 512))

                    'most used Bitmap (needed for preview in Freevolts)
                    Dim bmps(28) As ListBox
                    For k = 0 To 27
                        bmps(k) = New ListBox
                    Next
                    For k = 0 To CurrentWorld.mMesh(CurrentMesh).polynum - 1
                        bmps(CurrentWorld.mMesh(CurrentMesh).polyl(k).tpage + 1).Items.Add("X")
                    Next
                    Dim Max = 0
                    Dim Container$ = ""
                    For k = 0 To 27
                        If Max < bmps(k).Items.Count Then
                            Max = bmps(k).Items.Count
                            Container = k - 1
                        End If
                    Next

                    DoWrite("Most used bitmap: (" & Chr(Container + 65) & ") [" & Container & "]")


                End If

            Case "listall"
                If CurrentWorld Is Nothing Then
                    DoWrite("Please load a world file first")
                    Exit Sub
                End If
                Form1.Button1.Visible = True
                For k = 0 To CurrentWorld.meshCount - 1
                    DoWrite("MESH : " & k)
                    For a = 0 To CurrentWorld.mMesh(k).polynum - 1
                        Dim cpoly = CurrentWorld.mMesh(k).polyl(a)
                        Dim Type$ = ""
                        If cpoly.type And 2 ^ 0 Then
                            Type += " [QUAD] "
                        End If
                        If cpoly.type And 2 ^ 1 Then
                            Type += " [DBLSD] "
                        End If
                        If cpoly.type And 2 ^ 2 Then
                            Type += " [TRANS] "
                        End If
                        If cpoly.type And 2 ^ 8 Then
                            Type += " [TTYPE] "
                        End If
                        If cpoly.type And 2 ^ 9 Then
                            Type += " [TEXANIM] "
                        End If
                        If cpoly.type And 2 ^ 11 Then
                            Type += " [ENV] "
                        End If
                        DoWrite(String.Format(vbTab & "poly:{0} | Type:{1} | ", a, Type) & String.Format("Tex:{0} , uv: {1}x{2} {3}x{4} {5}x{6} {7}x{8}", Chr(cpoly.tpage + 65), Format(cpoly.u0, "0.#0"), Format(cpoly.v0, "0.#0"), Format(cpoly.u1, "0.#0"), Format(cpoly.v1, "0.#0"), Format(cpoly.u2, "0.#0"), Format(cpoly.v2, "0.#0"), Format(cpoly.u3, "0.#0"), Format(cpoly.v3, "0.#0")))

                        If RequestTerminate Then
                            RequestTerminate = False
                            DoWrite(vbTab & "Canceled by user.")
                            Form1.Button1.Hide()
                            Exit Sub
                        End If
                    Next
                Next

            Case "list"
                If CurrentMesh = -1 Then
                    DoWrite("please select a mesh before; COMMAND: select mesh <mesh number>")
                    Exit Sub
                End If
                Form1.Button1.Show()

                For a = 0 To CurrentWorld.mMesh(CurrentMesh).polynum - 1
                    Dim cpoly = CurrentWorld.mMesh(CurrentMesh).polyl(a)
                    Dim Type$ = ""
                    If cpoly.type And 2 ^ 0 Then
                        Type += " [QUAD] "
                    End If
                    If cpoly.type And 2 ^ 1 Then
                        Type += " [DBLSD] "
                    End If
                    If cpoly.type And 2 ^ 2 Then
                        Type += " [MIR] "
                    End If
                    If cpoly.type And 2 ^ 8 Then
                        Type += " [TTYPE] "
                    End If
                    If cpoly.type And 2 ^ 9 Then
                        Type += " [TEXANIM] "
                    End If
                    If cpoly.type And 2 ^ 11 Then
                        Type += " [ENV] "
                    End If
                    DoWrite(String.Format("poly:{0} | Type:{1} | ", a, Type) & String.Format("Tex:{0} , uv: {1}x{2} {3}x{4} {5}x{6} {7}x{8}", Chr(cpoly.tpage + 65), Format(cpoly.u0, "0.#0"), Format(cpoly.v0, "0.#0"), Format(cpoly.u1, "0.#0"), Format(cpoly.v1, "0.#0"), Format(cpoly.u2, "0.#0"), Format(cpoly.v2, "0.#0"), Format(cpoly.u3, "0.#0"), Format(cpoly.v3, "0.#0")))

                    If RequestTerminate Then
                        RequestTerminate = False
                        DoWrite(vbTab & "Canceled by user.")
                        Form1.Button1.Hide()
                        Exit Sub
                    End If
                Next

            Case "convert"
                If CurrentMesh = -1 Then
                    DoWrite("Please select a mesh first")
                    Exit Sub
                End If
                Select Case LCase(vars)
                    Case "quads"
                        If CurrentWorld Is Nothing Then
                            DoWrite("Please select a world file first")
                            Exit Sub
                        End If
                        Dim Curmesh = CurrentWorld.mMesh(CurrentMesh)
                        Dim iMesh As New Worldf
                        iMesh.bbox = Curmesh.bbox
                        iMesh.BoundingSphere = Curmesh.BoundingSphere
                        iMesh.vexl = Curmesh.vexl
                        iMesh.vertnum = Curmesh.vertnum

                        DoWrite("Searching Quads...")
                        Dim found = False
                        For i = 0 To Curmesh.polynum - 1
                            found = found Or (Curmesh.polyl(i).type And 1)
                        Next

                        If found Then
                            DoWrite("Aborting, Quad meshes were found")
                            Exit Sub
                        End If

                        If Curmesh.polynum \ 2 <> Curmesh.polynum / 2 Then
                            DoWrite("Aborting, Meshes aren't even (they're odd)")
                            Exit Sub
                        End If

                        iMesh.polynum = Curmesh.polynum \ 2
                        ReDim iMesh.polyl(iMesh.polynum)
                        For i = 0 To (Curmesh.polynum - 1) \ 2
                            If Not (iMesh.polyl(i).type And 1) Then iMesh.polyl(i).type = iMesh.polyl(i).type Or 1
                            iMesh.polyl(i).tpage = Curmesh.polyl(i * 2).tpage
                            iMesh.polyl(i).vi0 = Curmesh.polyl(i * 2).vi0
                            iMesh.polyl(i).vi1 = Curmesh.polyl(i * 2).vi1
                            iMesh.polyl(i).vi2 = Curmesh.polyl(i * 2).vi2
                            iMesh.polyl(i).vi3 = Curmesh.polyl(i * 2 + 1).vi1

                            iMesh.polyl(i).c0 = Curmesh.polyl(i * 2).c0
                            iMesh.polyl(i).c1 = Curmesh.polyl(i * 2).c1
                            iMesh.polyl(i).c2 = Curmesh.polyl(i * 2).c2
                            iMesh.polyl(i).c3 = Curmesh.polyl(i * 2 + 1).c1

                            iMesh.polyl(i).u0 = Curmesh.polyl(i * 2).u0
                            iMesh.polyl(i).v0 = Curmesh.polyl(i * 2).v0
                            iMesh.polyl(i).u1 = Curmesh.polyl(i * 2).u1
                            iMesh.polyl(i).v1 = Curmesh.polyl(i * 2).v1
                            iMesh.polyl(i).u2 = Curmesh.polyl(i * 2).u2
                            iMesh.polyl(i).v2 = Curmesh.polyl(i * 2).v2
                            iMesh.polyl(i).u3 = Curmesh.polyl(i * 2 + 1).u1
                            iMesh.polyl(i).v3 = Curmesh.polyl(i * 2 + 1).v1

                        Next

                        CurrentWorld.mMesh(CurrentMesh) = iMesh
                        If Not BatchMode Then Process("backup")
                        Process("save")

                End Select

            Case "shade"
                If vars = "" Or vars = " " Then Exit Sub
                If CurrentMesh = -1 Then
                    DoWrite("please select a mesh before")
                    Exit Sub
                End If
                vars = Replace(vars, "  ", " ")

                If Split(vars, " ", , CompareMethod.Text).Count = 3 Then
                    Dim mColor As UInteger = ColorToUInt(Color.FromArgb(255, Split(vars, " ")(0), Split(vars, " ")(1), Split(vars, " ")(2)))
                    DoWrite("Shading to :" & UIntToColor(mColor).ToString)

                    For k = 0 To CurrentWorld.mMesh(CurrentMesh).polynum - 1
                        If CurrentWorld.mMesh(CurrentMesh).polyl(k).type And (2 ^ 2) Then _
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).type = CurrentWorld.mMesh(CurrentMesh).polyl(k).type Or Not (2 ^ 2) ' NO translucent
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).c0 = mColor
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).c1 = mColor
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).c2 = mColor
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).c3 = mColor

                    Next
                ElseIf Split(vars, " ", , CompareMethod.Text).Count = 4 Then

                    Dim mColor As UInteger = ColorToUInt(Color.FromArgb(Split(vars, " ")(0), Split(vars, " ")(1), Split(vars, " ")(2), Split(vars, " ")(3)))
                    DoWrite("Shading to :" & UIntToColor(mColor).ToString)
                    For k = 0 To CurrentWorld.mMesh(CurrentMesh).polynum - 1
                        If Not (CurrentWorld.mMesh(CurrentMesh).polyl(k).type And (2 ^ 2)) Then _
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).type = CurrentWorld.mMesh(CurrentMesh).polyl(k).type Or 2 ^ 2 'translucent
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).c0 = mColor
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).c1 = mColor
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).c2 = mColor
                        CurrentWorld.mMesh(CurrentMesh).polyl(k).c3 = mColor
                    Next
                End If
                Process("save")
                DoWrite("Mesh shaded!")
                If Not BatchMode Then Form1.Button1.Hide()

            Case "dir"
                DoWrite("Current files in this directory:" & CurDir())
                For i = 0 To IO.Directory.GetFiles(CurDir).Length - 1
                    DoWrite(IO.Directory.GetFiles(CurDir)(i))
                Next

            Case "generate"
                If LCase(vars) = "envlist" Or LCase(vars) = "env" Then
                    If CurrentWorld Is Nothing Then
                        DoWrite("please load a world file before!") 'BTW, wouldn't a "check procedure/sub" be enough....
                    End If
                    If CurrentWorld.ENV.Count = CurrentWorld.polyEleven Then
                        DoWrite(String.Format("No need to generate, since all the {0} ENV (list) is there ", CurrentWorld.polyEleven))
                    ElseIf CurrentWorld.ENV.Count < CurrentWorld.polyEleven Then
                        ReDim CurrentWorld.ENV(CurrentWorld.polyEleven)
                        For i = 0 To CurrentWorld.polyEleven - 1
                            If CurrentWorld.ENV(i) = 0 Then CurrentWorld.ENV(i) = ColorToUInt(Color.FromArgb(200, 200, 200))
                        Next
                        DoWrite("ENV list is generated")
                        Process("save")
                    Else
                        DoWrite("What's going on? ENV list is less than CurrentWorld's ENV_enabled polies")

                    End If
                Else
                    DoWrite("sorry, only ENV list is supported so far")
                End If



            Case "envshade"

                Select Case LCase(vars)
                    Case "list", ""
                        If CurrentWorld Is Nothing Then
                            DoWrite("please load a world file first")
                            Exit Sub
                        End If
                        If CurrentWorld.allEnv.Items.Count = 0 Then
                            DoWrite("Empty list!")
                        End If
                        For j = 0 To CurrentWorld.allEnv.Items.Count - 1

                            Dim meshN& = Split(CurrentWorld.allEnv.Items(j), ",")(0)
                            Dim polyN& = Split(CurrentWorld.allEnv.Items(j), ",")(1)
                            Dim cpoly = CurrentWorld.mMesh(meshN).polyl(polyN) 'shortening a little


                            'READABLE TYPE
                            Dim Type$ = ""
                            If cpoly.type And 2 ^ 0 Then
                                Type += " [QUAD] "
                            End If
                            If cpoly.type And 2 ^ 1 Then
                                Type += " [DBLSD] "
                            End If
                            If cpoly.type And 2 ^ 2 Then
                                Type += " [TRANS] "
                            End If
                            If cpoly.type And 2 ^ 8 Then
                                Type += " [TTYPE] "
                            End If
                            If cpoly.type And 2 ^ 9 Then
                                Type += " [TEXANIM] "
                            End If
                            If cpoly.type And 2 ^ 11 Then
                                Type += " [ENV] "
                            End If

                            DoWrite("__________________________________")
                            DoWrite(String.Format("|index: {0} | mesh: {1} | poly: {2} |", j, meshN, polyN))
                            DoWrite("----------------------------------")
                            DoWrite(vbTab & String.Format(" ~ Type:{0}  ", Type))
                            DoWrite(vbTab & String.Format(" ~ Tex:{0} , uv: {1}x{2} {3}x{4} {5}x{6} {7}x{8}", Chr(cpoly.tpage + 65), Format(cpoly.u0, "0.#0"), Format(cpoly.v0, "0.#0"), Format(cpoly.u1, "0.#0"), Format(cpoly.v1, "0.#0"), Format(cpoly.u2, "0.#0"), Format(cpoly.v2, "0.#0"), Format(cpoly.u3, "0.#0"), Format(cpoly.v3, "0.#0")))
                            DoWrite(vbTab & String.Format(" ~ Color: {0}", UIntToColor(CurrentWorld.ENV(j)).ToString))
                            DoWrite(vbNewLine)
                        Next
                        DoWrite("=============== Listing is done ===============")
                        Exit Sub

                    Case Else
                        vars = Replace(vars, "  ", " ")
                        Form1.Button1.Show()



                        If Split(vars, " ", , CompareMethod.Text).Count = 4 Then
                            Dim mColor As UInteger = ColorToUInt(Color.FromArgb(128, Split(vars, " ")(1), Split(vars, " ")(2), Split(vars, " ")(3)))
                            DoWrite("ENVShading (" & vars & ") to :" & UIntToColor(mColor).ToString)

                            If IsNumeric(Split(vars, " ")(0)) Then
                                CurrentWorld.ENV(Split(vars, " ")(0)) = mColor

                            ElseIf LCase(Split(vars, " ")(0)) = "i" Then
                                DoWrite("=============== Starting ENV ===============")

                                For n = 0 To CurrentWorld.ENV.Length - 1
                                    Process("envshade " & Replace(vars, "i", n, , , CompareMethod.Text))
                                    ' Debugger.Break()
                                Next

                                DoWrite("=============== Ending ENV ===============")
                            End If

                        ElseIf Split(vars, " ", , CompareMethod.Text).Count = 5 Then
                            Dim mColor As UInteger = ColorToUint(Color.FromArgb(Split(vars, " ")(1), Split(vars, " ")(2), Split(vars, " ")(3), Split(vars, " ")(4)))
                            DoWrite("ENVShading (" & vars & ") to :" & UintToColor(mColor).ToString)

                            If IsNumeric(Split(vars, " ")(0)) Then
                                CurrentWorld.ENV(Split(vars, " ")(0)) = mColor

                            ElseIf LCase(Split(vars, " ")(0)) = "i" Then
                                DoWrite("=============== Starting ENV ===============")

                                For n = 0 To CurrentWorld.ENV.Length - 1
                                    Process("envshade " & Replace(vars, "i", n, , , CompareMethod.Text))
                                    ' Debugger.Break()
                                Next

                                DoWrite("=============== Ending ENV ===============")
                            End If


                        Else
                            DoWrite("not accepted, only ""envshade INDEX R G B"" and ""envshade INDEX A R G B are accepted")
                            DoWrite("if it's tiring to env a whole track, use index as 'i' ""envshade i R G B""")
                            Exit Sub
                        End If
                        Process("save")
                        DoWrite("ENV Mesh shaded!")
                        If Not BatchMode Then Form1.Button1.Hide()

                        Exit Sub
                End Select

            Case "frames"




            Case "texanim", "multiframe", "mf"
                If IO.File.Exists(vars) Then
                    DoWrite("Loading TexAnim List:" & vars)
                    getFileForTexAnim(vars)

                    Form1.Label3.Text = "[[ loaded TEXANIM:" & vars & " ]]"

                    DoWrite("TexAnim List is loaded, please select a mesh and write 'animate'")

                    If LCase(vars) = "mf" Then DoWrite("mf mean multiframe btw...")
                Else

                    DoWrite("Error, Texture Animation's frames list isn't there")
                    DoWrite("Syntax: ""texanim <path to tex_anim_list""")
                End If

            Case "animate"
                DoWrite("PLEASE MAKE SURE IT'S ANIMATED ONCE!!!!")

                If mFrames Is Nothing Then
                    DoWrite("Error, please load a (texanim) frame list first")
                    Exit Sub
                End If
                If CurrentMesh = -1 Then
                    DoWrite("Please select a mesh before")
                    Exit Sub
                End If

                Process("flag texanim") 'patch to become TEX_ANIM
                '  Debugger.Break()
                CurrentWorld.TA.Items.Add(CurrentMesh)
                CurrentWorld.TA.Sorted = True
                Dim found = False 'search in the list
                Dim j = 0
                Do Until found = True
                    found = CurrentWorld.TA.Items(j) = CurrentMesh
                    j += 1
                Loop
                j -= 1


                AnimC += 1
                Dim t_f() As Animation
                t_f = Frames

                ReDim Frames(AnimC - 1)

                If Frames.Length = 16 Then
                    DoWrite("Error: MAX_TEX_ANIM = 16")
                    Exit Sub
                End If

                For alpha = 0 To t_f.Count - 1
                    Frames(alpha) = t_f(alpha)
                Next

                '  Debugger.Break()
                Frames(AnimC - 1) = New Animation
                'copy paste mf
                Frames(AnimC - 1).FrameCount = mFrames.Count - 1
                Frames(AnimC - 1).Frames = mFrames

                If j <> 0 Then


                    For k = AnimC - 1 To j + 1 Step -1
                        PermutTwoAnim(Frames(k), Frames(k - 1))
                    Next

                End If

                Process("save")




            Case "flag", "type"
                If LCase(vars) = "" Or LCase(vars) = "list" Then
                    Process("list")
                    Exit Sub
                End If
                '  Debugger.Break()
                If CurrentWorld Is Nothing Or CurrentMesh = -1 Then
                    DoWrite("Please select mesh/load a world file first")
                    Exit Sub
                End If
                DoWrite("Applying flags to selected mesh")

                For a = 0 To CurrentWorld.mMesh(CurrentMesh).polynum - 1

                    If InStr(vars, "dblsd", CompareMethod.Text) > 0 Then
                        If Not (CurrentWorld.mMesh(CurrentMesh).polyl(a).type And 2 ^ 1) Then
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Or 2 ^ 1
                        Else
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Xor 2 ^ 1
                        End If

                    End If
                    If InStr(vars, "trans", CompareMethod.Text) > 0 Then
                        If Not (CurrentWorld.mMesh(CurrentMesh).polyl(a).type And 2 ^ 2) Then
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Or 2 ^ 2
                        Else
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Xor 2 ^ 2
                        End If

                    End If

                    If InStr(vars, "ttype", CompareMethod.Text) > 0 Then
                        If Not (CurrentWorld.mMesh(CurrentMesh).polyl(a).type And 2 ^ 8) Then
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Or 2 ^ 8
                        Else
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Xor 2 ^ 8
                        End If

                    End If


                    If InStr(vars, "texanim", CompareMethod.Text) > 0 Then
                        If Not (CurrentWorld.mMesh(CurrentMesh).polyl(a).type And 2 ^ 9) Then
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Or 2 ^ 9
                        Else
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Xor 2 ^ 9
                        End If


                    End If

                    If InStr(vars, "env", CompareMethod.Text) > 0 Then
                        If Not (CurrentWorld.mMesh(CurrentMesh).polyl(a).type And 2 ^ 11) Then
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Or 2 ^ 11
                        Else
                            CurrentWorld.mMesh(CurrentMesh).polyl(a).type = CurrentWorld.mMesh(CurrentMesh).polyl(a).type Xor 2 ^ 11
                        End If

                    End If
                    ' Debugger.Break()

                Next a
                If Not BatchMode Then Process("backup")
                Process("save")







            Case "explore"
                DoWrite("Exploring in progress...")
                System.Diagnostics.Process.Start("explorer", """" & Left(last_file, last_file.LastIndexOf("\")) & """")

            Case "revolt"
                If CurrentWorld Is Nothing Then
                    DoWrite("please select a world file inside levels\track, first")
                    Exit Sub
                End If
                If IO.File.Exists(CurrentWorld.Directory & "\..\..\revolt.exe") Then
                    DoWrite("Launching Re-Volt...")
                    Dim p = CurDir()
                    ChDir(CurrentWorld.Directory & "\..\..")
                    Try
                        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Acclaim\Re-Volt\1.0").SetValue("DirPath", CurrentWorld.DirectoryName)

                    Catch ex As Exception

                    End Try
                    Try
                        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Acclaim\Re-Volt\1.0").SetValue("LevelDir", CurrentWorld.DirectoryName)

                    Catch ex As Exception

                    End Try
                    Shell("revolt.exe -window -dev", AppWinStyle.NormalFocus)

                    ChDir(p)
                End If



            Case "export"
                If vars = "" Or vars = " " Then
                    vars = last_file & "_" & CurrentMesh & ".prm"
                End If


                If LCase(vars) <> "each" Then

                    If CurrentMesh = -1 Then
                        DoWrite("Please select world, mesh before")
                        Exit Sub
                    End If


                    Dim Curmesh = CurrentWorld.mMesh(CurrentMesh)

                    DoWrite("Exporting in progress")
                    DoWrite("getting boundingbox")
                    Dim min As New Vector3D(0, 0, 0), max As New Vector3D(0, 0, 0)

                    For i = 0 To Curmesh.vertnum - 1
                        If max.x < Curmesh.vexl(i).Position.x Then max.x = Curmesh.vexl(i).Position.x
                        If max.y < Curmesh.vexl(i).Position.y Then max.y = Curmesh.vexl(i).Position.y
                        If max.z < Curmesh.vexl(i).Position.z Then max.z = Curmesh.vexl(i).Position.z

                        If min.x > Curmesh.vexl(i).Position.x Then min.x = Curmesh.vexl(i).Position.x
                        If min.y > Curmesh.vexl(i).Position.y Then min.y = Curmesh.vexl(i).Position.y
                        If min.z > Curmesh.vexl(i).Position.z Then min.z = Curmesh.vexl(i).Position.z
                    Next

                    'center/trans
                    Dim Trans As New Vector3D(-(min.x + max.x) / 2, -(min.y + max.y) / 2, -(min.z + max.z) / 2)





                    Dim N As New IO.BinaryWriter(New IO.FileStream(vars, IO.FileMode.OpenOrCreate))
                    N.Write(Convert.ToInt16(Curmesh.polynum))
                    N.Write(Convert.ToInt16(Curmesh.vertnum))

                    For i = 0 To Curmesh.polynum - 1
                        N.Write(Convert.ToInt16(Curmesh.polyl(i).type))
                        N.Write(Convert.ToInt16(Curmesh.polyl(i).tpage))
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

                        N.Write(Curmesh.vexl(i).Position.x + Trans.x)
                        N.Write(Curmesh.vexl(i).Position.y + Trans.y)
                        N.Write(Curmesh.vexl(i).Position.z + Trans.z)
                        N.Write(Curmesh.vexl(i).normal.x)
                        N.Write(Curmesh.vexl(i).normal.y)
                        N.Write(Curmesh.vexl(i).normal.z)

                    Next
                    N.Close()
                    DoWrite("Exported mesh:" & CurrentMesh)
                ElseIf LCase(vars) = "each" Then
                    For k = 0 To CurrentWorld.meshCount - 1

                        If RequestTerminate Then
                            RequestTerminate = False
                            DoWrite(vbTab & "Canceled by user.")
                            Form1.Button1.Hide()
                            Exit Sub
                        End If


                        DoWrite("Exporting mesh: " & k)
                        Dim Curmesh = CurrentWorld.mMesh(k)
                        Dim N As New IO.BinaryWriter(New IO.FileStream(last_file & "_" & k & ".prm", IO.FileMode.OpenOrCreate))
                        N.Write(Convert.ToInt32(Curmesh.polynum))
                        N.Write(Convert.ToInt32(Curmesh.vertnum))

                        For i = 0 To Curmesh.polynum - 1
                            N.Write(Convert.ToInt16(Curmesh.polyl(i).type))
                            N.Write(Convert.ToInt16(Curmesh.polyl(i).tpage))
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

                            N.Write(Curmesh.vexl(i).Position.x)
                            N.Write(Curmesh.vexl(i).Position.y)
                            N.Write(Curmesh.vexl(i).Position.z)
                            N.Write(Curmesh.vexl(i).normal.x)
                            N.Write(Curmesh.vexl(i).normal.y)
                            N.Write(Curmesh.vexl(i).normal.z)

                        Next
                        N.Close()
                        DoWrite("Exported mesh:" & k)
                    Next

                End If

            Case "silent"
                DoWrite("Silent mode is on, to remove it write 'verbose'")
                SilentMode = True

            Case "dowrite", "echo"
                DoWrite(vars)

            Case "verbose"
                SilentMode = False
                DoWrite("verbose mode is on, to remove it write 'silent'")

            Case "if"
                Dim Stat$ = Split(vars, "then", , CompareMethod.Text)(0)
                Dim Inst$ = Mid(vars, Len(Stat) - 1)

                'ok now, replace
                DoWrite("disabled/not implemented")

            Case "for"
                Form1.Button1.Show()
                BatchMode = True
                Dim secLoops = 0
                If CurrentWorld Is Nothing Then
                    DoWrite("please load a world file first")
                    Exit Sub
                End If
                DoWrite("============= Starting For =============")
                vars = Replace(vars, "each", "0->" & CurrentWorld.meshCount - 1, , , CompareMethod.Text) 'for each
                vars = Replace(Replace(vars, "meshes", "", , , CompareMethod.Text), "mesh", "", , , CompareMethod.Text) 'for meshes i->j
                'search ->
                vars = Replace(Replace(vars, " ->", "->"), "-> ", "->")

                If InStr(vars, "->", CompareMethod.Text) > 0 Then
                    Dim Arrays() As String = Split(vars, "->")
                    For i = 0 To Arrays.Length - 2


                        If RequestTerminate Then
                            RequestTerminate = False
                            DoWrite(vbTab & "Canceled by user.")
                            Form1.Button1.Hide()
                            Exit Sub
                        End If


                        '  Do Until Arrays(i)(Len(Arrays(i))) >= "0" And Arrays(i)(Len(Arrays(i))) <= "9"
                        'Arrays(i) = Mid(Arrays(i), Len(Arrays(i) - 1))
                        '  secLoops += 1
                        '  If secLoops > 20 Then
                        'DoWrite("Infinity loop!")
                        '   Exit Sub
                        '  End If
                        '
                        '   Loop

                        '  Do Until Arrays(i + 1)(0) >= "0" And Arrays(i + 1)(0) <= "9"
                        'Arrays(i) = Mid(Arrays(i), 1)
                        '  secLoops += 1
                        '   If secLoops > 20 Then
                        'DoWrite("Infinity loop!")
                        '   Exit Sub
                        '   End If

                        '  Loop
                        Dim firstMember$
                        Try
                            firstMember = Mid(Arrays(i), Arrays(i).LastIndexOf(" "))
                        Catch ex As Exception
                            firstMember = Arrays(i)
                        End Try
                        Dim secondMember$ = Split(Arrays(i + 1), " ")(0)
                        If InStr(secondMember, ":") > 0 Then secondMember = Split(secondMember, ":")(0)

                        If firstMember > secondMember Then
                            secondMember = secondMember + 100000 * firstMember

                            firstMember = secondMember Mod 100000
                            secondMember = secondMember \ 1000000
                        End If

                        Dim dump As String = ""
                        For ii = Int(firstMember) To Int(secondMember)
                            dump &= ii & Space(1)
                        Next
                        vars = Replace(vars, firstMember & "->" & secondMember, dump, , , CompareMethod.Text)
                    Next


                End If
                vars = Replace(Replace(vars, "for ", "", , , CompareMethod.Text), "  ", " ")

                For i = 0 To Split(Split(vars, ":")(0), " ").Count - 2

                    If RequestTerminate Then
                        RequestTerminate = False
                        DoWrite(vbTab & "Canceled by user.")
                        Form1.Button1.Hide()
                        Exit Sub
                    End If

                    DoWrite(vbNewLine)
                    Process("select mesh " & Split(Split(vars, ":")(0), " ")(i))
                    Process(Split(vars, ":")(1))
                Next
                BatchMode = False
                Form1.Button1.Hide()
                DoWrite("============= End For =============")


            Case "cls"
                Form1.TextBox1.Text = ""

            Case "log"
                IO.File.AppendAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\W_Console.txt", Now.ToShortDateString & " " & Now.ToShortTimeString & vbNewLine & Form1.TextBox1.Text & vbNewLine & "------------------------------------------------")
                DoWrite("Logged to :" & My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\W_Console.txt")

            Case "run"
                Try
                    Shell(vars, AppWinStyle.NormalFocus)
                Catch
                    DoWrite("Please select an executable file to run")
                End Try

            Case "cd"
                If IO.Directory.Exists(vars) Then
                    ChDir(vars)
                    DoWrite("Current Directory: " & CurDir())
                Else
                    DoWrite("Directory not found (" & vars & ")")
                End If

            Case "decode"
                If CurrentWorld Is Nothing Then
                    DoWrite("Why not try to load the world first...")
                    Exit Sub
                End If
                If vars = " " Or vars = "" Then
                    vars = last_file & ".txt"
                End If
                World_file_decoder.Start(vars)

            Case "encode"
                If vars = " " Or vars = "" Then
                    vars = last_file & ".txt"
                End If
                Encoder(last_file)

            Case "exit"
                End

            Case "nvfv", "fvnv"
                If IO.File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\nvolt\nvolt.exe") Then
                    DoWrite(">> nVolt is detected, we're going to allow preview mesh possibility :)")
                    Form1.Nv = True
                    Form1.TextBox2.AutoCompleteCustomSource.Add("preview              {allows you to preview current mesh}")
                End If
                If IO.File.Exists("rv2.exe") Or IO.File.Exists(Environ("windir") & "\rv2.exe") Or IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.System) & "\system32\rv2.exe") Then
                    DoWrite(">> FreeVolts is detected, we're going to allow preview mesh possibility :)")
                    If Form1.Nv = True Then DoWrite(">> However, since nVolt came first, FreeVolts will be disabled")
                    Form1.Fv = True
                    Form1.TextBox2.AutoCompleteCustomSource.Add("preview              {allows you to preview current mesh}")
                End If
                'fun part :P
            Case "fuck", "shit", "ass", "damn"
                DoWrite("Hey, what's going????...")
            Case "about"
                DoWrite("Application by theKDL" & vbNewLine & _
                        "> version 1.0.0.1" & vbNewLine & _
                        "> for updates and later versions, please check Re-Volt Live (http://z3.invisionfree.com/Revolt_Live)")

                'others: extras

            Case "fvpreview"
                If CurrentMesh = -1 Then
                    DoWrite("please make sure you selected a world file and a mesh")
                    Exit Sub
                End If



                Process("export")
                Dim FileName$ = last_file & "_" & Split(LatestOutput, ":")(1) & ".prm"
                Process("getinfo")
                Dim myTex$ = CurrentWorld.Directory & "\" & CurrentWorld.DirectoryName & Split(Split(LatestOutput, "(")(1), ")")(0) & ".bmp"



                If Form1.Fv Then
                    Environment.SetEnvironmentVariable("IRR_DRIVER", "DX9")
                    Environment.SetEnvironmentVariable("IRR_MODE", "1024x768")
                    Shell(String.Format("rv2 ""{0}"" ""{1}""", FileName, myTex), AppWinStyle.NormalFocus)
                Else
                    DoWrite("freevolts isn't found!")
                    DoWrite("write 'fvnv' to reload")
                End If



            Case "fvwpreview"
                If CurrentWorld Is Nothing Then
                    DoWrite("please select a world file first")
                    Exit Sub
                End If

            
                If Form1.Fv Then 'freeVolts

                    Dim p = CurDir()
                    ChDir(CurrentWorld.Directory & "\..")
                    Environment.SetEnvironmentVariable("IRR_DRIVER", "DX9")
                    Environment.SetEnvironmentVariable("IRR_MODE", "1024x768")
                    Shell(String.Format("rv2 ""{0}""", CurrentWorld.DirectoryName), AppWinStyle.NormalFocus)
                    ChDir(p)

                Else
                    DoWrite("freevolts isn't found!")
                    DoWrite("write 'fnvn' to reload")
                End If




            Case "preview"
                If CurrentWorld Is Nothing Then
                    DoWrite("please select a world file first")
                    Exit Sub
                End If

                Select Case LCase(vars)
                    Case "world", "level", "*"
                        If Form1.Nv Then 'nVolt
                            Shell(String.Format(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\nvolt\nvolt {0}", last_file), AppWinStyle.NormalFocus)
                        Else
                            If Form1.Fv Then 'freeVolts

                                Dim p = CurDir()
                                ChDir(CurrentWorld.Directory & "\..")
                                Environment.SetEnvironmentVariable("IRR_DRIVER", "DX9")
                                Environment.SetEnvironmentVariable("IRR_MODE", "1024x768")
                                Shell(String.Format("rv2 ""{0}""", CurrentWorld.DirectoryName), AppWinStyle.NormalFocus)
                                ChDir(p)

                            End If
                        End If



                    Case "mesh", "this", Nothing
                        If CurrentMesh = -1 Then
                            DoWrite("please make sure you selected a world file and a mesh")
                            Exit Sub
                        End If



                        Process("export")
                        Dim FileName$ = last_file & "_" & Split(LatestOutput, ":")(1) & ".prm"
                        Process("getinfo")
                        Dim myTex$ = CurrentWorld.Directory & "\" & CurrentWorld.DirectoryName & Split(Split(LatestOutput, "(")(1), ")")(0) & ".bmp"


                        If Form1.Nv Then 'Freevolts

                            Shell(String.Format(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\nvolt\nvolt {0}", FileName), AppWinStyle.NormalFocus)


                        Else
                            If Form1.Fv Then
                                Environment.SetEnvironmentVariable("IRR_DRIVER", "DX9")
                                Environment.SetEnvironmentVariable("IRR_MODE", "1024x768")
                                Shell(String.Format("rv2 ""{0}"" ""{1}""", FileName, myTex), AppWinStyle.NormalFocus)
                            Else
                                DoWrite("No previewers (nvolt or freevolts) were found!")
                            End If
                        End If


                    Case Else
                        DoWrite("unknown...")




                End Select






            Case Else

                DoWrite("Unknown Command :" & cmd)
        End Select



    End Sub


    Public Function getAllVars(ByVal buffer$)
        Return Mid(buffer, Len(getCommand(buffer)) + 2)
    End Function

    Public Function getCommand(ByVal buffer$)
        buffer = Replace(buffer, vbTab, "")
        Return Split(buffer, " ")(0)
    End Function
End Module
