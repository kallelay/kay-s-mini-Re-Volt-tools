Imports System.Drawing

Module kaywords

    Public filePath$
    Sub kayProcess(ByVal fp$)
        'make sure it's initialized
        If Constants.Count = 0 Then
            initDict()
        End If

        'save filepath & load
        filePath = fp
        Dim f As New IO.StreamReader(New IO.FileStream(fp, IO.FileMode.Open, IO.FileAccess.Read))
        Dim cnt = 0

        'start getting into text
        While Not f.EndOfStream
            cnt += 1
            TreatLine(f.ReadLine(), cnt)
        End While

        f.Close() 'finished 

    End Sub


    ' Public keywords As String() = New String() {"apply", "color", "colormode", "clouds", "dir", "mode", "offset", "rain", "skybox", "sunpos", "surf", "surfmul", "level", "ldir", "loff"}
    Public SelectKeyWords = New String() {"bmp", "color", "cube", "mesh", "prop", "surf", "uv"}
    Public InfKeywords = New String() {"NAME", "STARTPOS", "STARTPOSREV", "STARTROT", "STARTROTREV", "STARTGRID", "STARTGRIDREV", _
                                       "ROCK", "ENVROLL", "ENVSTILL", "MP3", "FOGCOLOR", "FARCLIP", "FOGSTART"}

    Public Surfaces As String() = New String() {"surf.default", "surf.marble", "surf.stone", "surf.wood", "surf.sand", "surf.plastic", "surf.carpettile", "surf.carpetshag", "surf.boundary", "surf.glass", "surf.ice1", "surf.metal", "surf.grass", "surf.bumpmetal", "surf.pebbles", "surf.gravel", "surf.conveyor1", "surf.conveyor2", "surf.dirt1", "surf.dirt2", "surf.dirt3", "surf.ice2", "surf.ice3", "surf.wood2", "surf.conveyor_market1", "surf.conveyor_market2", "surf.paving"}
    Public bmpModes As String() = New String() {"mul", "add", "sub", "mix", "reco", "vib"}

    Public Constants As Dictionary(Of String, String) = New Dictionary(Of String, String)





    'Parsing mode
    Enum ParsingMode
        GeneralMode = 0
        SelectMode
        InfMode
        FinishedSelectionMode
    End Enum
    Public ParseMode As ParsingMode = ParsingMode.GeneralMode




    'Dictionaries
    Sub initDict()

        'skycolor, mycolor?

        '      Constants.Add("mycolor", myColor.R & " " & myColor.G & " " & myColor.B)
        '   Constants.Add("suncolor", sunColor.R & " " & sunColor.G & " " & sunColor.B)


        'Skyboxes
        Constants.Add("Toytanic_day", 0)

        Constants.Add("Toytanic_night", 1)

        Constants.Add("Wild West", 2)

        Constants.Add("Neighborhood", 3)

        Constants.Add("sky.ship1", 0)
        Constants.Add("sky.ship2", 1)
        Constants.Add("sky.west", 2)
        Constants.Add("sky.nhood1", 3)
        Constants.Add("sky.nhood", 3)

        'Shade mode
        'vx, addvx,addpoly,poly,add,flat
        Constants.Add("addvx", 4)
        Constants.Add("addpoly", 5)
        Constants.Add("add", 0)
        Constants.Add("flat", 1)
        Constants.Add("vx", 2)
        Constants.Add("poly", 3)

        'general

        Constants.Add("true", 1)
        Constants.Add("false", 0)

        Constants.Add("True", 1)
        Constants.Add("False", 0)


        Constants.Add("TRUE", 1)
        Constants.Add("FALSE", 0)

        Constants.Add("NONE", -1)
        Constants.Add("None", -1)
        Constants.Add("none", -1)


    End Sub




    'get one line
    Sub TreatLine(ByRef str$, ByVal lineNumber%)
        'clean routine
        str = Split(str, "#")(0)
        str = Replace(str, vbTab, Space(1))
        While InStr(str, Space(2)) > 0
            str = Replace(str, Space(2), Space(1))
        End While
        str = Trim(str)



        'If it's empty then exit
        If str = "" Then Exit Sub



        'Constants
        If (ParseMode = ParsingMode.GeneralMode Or ParseMode = ParsingMode.SelectMode) And InStr(str, "level") = 0 Then
            For i = LBound(Constants.Keys.ToArray) To UBound(Constants.Keys.ToArray)
                str = Replace(str, Constants.Keys(i), Constants.Item(Constants.Keys(i)))
            Next
        End If



        'Alright.... keyword recognize *********************** general
        '{"apply", "color", "clouds", "dir", "mode", "offset", "rain", "skybox", "sunpos"}
        If InStr(str, ":") > 0 And ParseMode = ParsingMode.GeneralMode Then
            Dim key$ = LCase(Split(str, ":")(0))
            Dim Value$ = Replace(Replace(Replace(Trim(Mid(str, Len(key) + 2)), ", ", " "), " ,", " "), ",", " ")
            Dim Values$() = Split(Value, " ")
            key = Trim(key)

            'If keywords.Contains(key) Then
            Select Case key




                Case "level", "track" ''''''''''''''''''''''''''''''''''''''''
                    Dim path$ = CurDir()
                    While CurDir().Length > 3 And Not IO.File.Exists(CurDir() & "\revolt.exe")
                        ChDir("..")
                        path = CurDir()
                    End While

                    If path.Length = 3 Then Report("ERROR! revolt.exe not found") : Exit Sub
                    ChDir("levels\" & Value)
                    FullPath = CurDir() & "\"
                    dirName = Value

                    INF = New inf_file(FullPath & dirName & ".inf")

                    'Do we have original?
                    If IO.File.Exists(FullPath & dirName & "_original.w") = False Then
                        IO.File.Copy(FullPath & dirName & ".w", FullPath & dirName & "_original.w")
                    End If
                    If IO.File.Exists(FullPath & dirName & "_original.ncp") = False Then
                        IO.File.Copy(FullPath & dirName & ".ncp", FullPath & dirName & "_original.ncp")
                    End If

                    '------------------------------------------------------------




                Case "apply" ''''''''''''''''''''''''''''''''''''''''
                    For i = LBound(Values) To UBound(Values)
                        Select Case Values(i)
                            Case "w"
                                Dim w As New WORLD(FullPath & dirName & "_original.w")
                                ShadeWorld(w, myColor, ShMode)
                                w.Save()
                            Case "fin"
                                ShadeFIN(myColor, ShMode)
                            Case "ncp"
                                NCP = New ncp_file(FullPath & dirName & "_original.ncp")
                                Dim lastncpflag = 0%

                                For Each item In NCP.ListOfNCP
                                    item.Material = mySurface + item.Material * SurfMultiplier
                                Next
                                NCP.save()
                            Case "bmp"
                                ShadeAllBitmaps(myColor)
                            Case "skybox"
                                ShadeSkybox(myColor)
                        End Select
                    Next



                Case "color" '''''''''''''''''''''''''''''''''''''''
                    If Values.Count <> 3 Then Report("Error! Expected 3 arguments for color in line " & lineNumber) : Exit Sub
                    myColor = Color.FromArgb(Values(0), Values(1), Values(2))

                Case "colormode"
                    For i = 0 To bmpModes.Count - 1
                        If InStr(LCase(Values(0)), bmpModes(i)) Then MyColorMode = i : Exit For
                    Next


                Case "clouds" ''''''''''''''''''''''''''''''''''''''''''''''''
                    If Not IsNumeric(Value) Then Report("Error ! A NaN in line " & lineNumber) : Exit Sub
                    Fob = New FOB_File(FullPath & dirName & ".fob")
                    Dim clouds = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_CLOUDS)
                    For i = clouds.Count - 1 To 0 Step -1
                        Fob.ObjList.RemoveAt(clouds(i))
                    Next

                    If Value <> 0 And Value <> -1 Then
                        Fob.ObjList.Add(New FILE_OBJECT(OBJ_ID_TYPE.OBJECT_TYPE_CLOUDS, New Integer() {Value, 0, 0, 0}, _
                                                                INF.STARTPOS - New Vector3D(200, 150, 0), New Vector3D(0, 1, 0), New Vector3D(0, 0, 1)))
                    End If
                    Fob.Save()


                Case "lighting" ''''''''''''''''''''''''''''''''''''''''''''''''
                    If Not IsNumeric(Value) Then Report("Error ! A NaN in line " & lineNumber) : Exit Sub
                    Fob = New FOB_File(FullPath & dirName & ".fob")
                    Dim clouds = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_LIGHTNING)
                    For i = clouds.Count - 1 To 0 Step -1
                        Fob.ObjList.RemoveAt(clouds(i))
                    Next

                    If Value <> 0 And Value <> -1 Then
                        Fob.ObjList.Add(New FILE_OBJECT(OBJ_ID_TYPE.OBJECT_TYPE_LIGHTNING, New Integer() {Value, 0, 0, 0}, _
                                                                INF.STARTPOS - New Vector3D(250, 150, 0), New Vector3D(0, 1, 0), New Vector3D(0, 0, 1)))
                    End If
                    Fob.Save()


                Case "dir", "ldir", "direction" '''''''''''''''''''''''''''''''''''''''
                    If Values.Count <> 3 Then Report("Error! Expected 3 arguments for dir in line " & lineNumber) : Exit Sub
                    ldir = New Vector3D(Values(0), Values(1), Values(2))



                Case "mode" '''''''''''''''''''''''''''''''''''''
                    If Values.Count > 1 Then Report("Error! Mode can only be one in line " & lineNumber) : Exit Sub
                    ShMode = Values(0)

                Case "offset", "loff" '''''''''''''''''''''''''''''
                    If Values.Count <> 3 Then Report("Error! Expected 3 arguments for offset in line " & lineNumber) : Exit Sub

                    loff = New Vector3D(Values(0), Values(1), Values(2))


                Case "rain" ''''''''''''''''''''''''''''''''''
                    If Not IsNumeric(Value) Then Report("Error ! A NaN in line " & lineNumber) : Exit Sub
                    Fob = New FOB_File(FullPath & dirName & ".fob")
                    Dim rains = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_RAIN)
                    For i = rains.Count - 1 To 0 Step -1
                        Fob.ObjList.RemoveAt(rains(i))
                    Next

                    For i = 1 To Int(Value)
                        Fob.ObjList.Add(New FILE_OBJECT(OBJ_ID_TYPE.OBJECT_TYPE_RAIN, New Integer() {0, 0, 0, 0}, _
                                                                INF.STARTPOS - New Vector3D(200, 160, 20 * i), New Vector3D(0, 1, 0), New Vector3D(0, 0, 1)))
                    Next
                    Fob.Save()


                Case "skybox" ''''''''''''''''''''
                    If Not IsNumeric(Value) Then Report("Error ! A NaN in line " & lineNumber) : Exit Sub
                    Fob = New FOB_File(FullPath & dirName & ".fob")
                    Dim skyboxes = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_SKYBOX)
                    For i = skyboxes.Count - 1 To 0 Step -1
                        Fob.ObjList.RemoveAt(skyboxes(i))
                    Next

                    If Value <> -1 Then
                        Fob.ObjList.Add(New FILE_OBJECT(OBJ_ID_TYPE.OBJECT_TYPE_SKYBOX, New Integer() {Value, 0, 0, 0}, _
                                                                INF.STARTPOS - New Vector3D(0, 150, 0), New Vector3D(0, 1, 0), New Vector3D(0, 0, 1)))
                    End If
                    Fob.Save()

                Case "sunpos" ''''''''''''''''''''''''''''''''
                    If Not IsNumeric(Value) Then Report("Error ! A NaN in line " & lineNumber) : Exit Sub
                    myColor = calculateShade(Value)
                    sunColor = myColor

                Case "surf" ''''''''''''''''''''''''''''''''''''''''
                    mySurface = 0
                    For i = LBound(Values) To UBound(Values)

                        For j = 0 To Surfaces.Count - 1
                            Value = Replace(Value, Surfaces(j), j)
                        Next
                        Values$ = Split(Value, " ")
                        For j = 0 To Values.Count - 1
                            mySurface += Values(j)
                        Next



                    Next
                Case "surfmul"
                    SurfMultiplier = Value


                Case "bmpmode"
                    SurfMultiplier = Value

                Case Else
                    Report("the key " & key & " doesn't exist in line " & lineNumber) : Exit Sub


            End Select


        End If






        'Alright.... keyword recognize *********************** NAME
        '{"apply", "color", "clouds", "dir", "mode", "offset", "rain", "skybox", "sunpos"}
        If InStr(str, ":") > 0 And ParseMode = ParsingMode.InfMode Then
            Dim key$ = UCase(Split(str, ":")(0))
            Dim Value$ = Replace(Replace(Replace(Trim(Mid(str, Len(key) + 2)), ", ", " "), " ,", " "), ",", " ")
            Dim Values$() = Split(Value, " ")
            key = Trim(key)

            'If keywords.Contains(key) Then
            Select Case key




                Case "NAME"
                    INF.NAME = Value
                Case "FOGCOLOR", "SKYCOLOR"
                    Value = Replace(Value, "mycolor", myColor.R & " " & myColor.G & " " & myColor.B)
                    Value = Replace(Value, "suncolor", sunColor.R & " " & sunColor.G & " " & sunColor.B)
                    Values$ = Split(Value, " ")
                    key = Trim(key)


                    INF.FOGCOLOR = Color.FromArgb(Values(0), Values(1), Values(2))

                Case "ENVROLL"
                    INF.ENVROLL = Value
                Case "ENVSTILL"
                    INF.ENVSTILL = Value
                Case "MODELRGBPER"
                    INF.MODELRGBPER = Values(0)
                Case "WORLDRGBPER"
                    INF.ROCK = Value
                Case "MP3"
                    INF.MP3 = Value
                Case "FARCLIP"
                    INF.FARCLIP = Values(0)


                Case Else
                    Report("the key " & key & " doesn't exist in line " & lineNumber) : Exit Sub


            End Select


        End If






        If InStr(str, "{") > 0 Then
            Dim o = Split(str, "{")(0)
            o = Trim(LCase(o))
            If o = "select" Then
                ParseMode = ParsingMode.SelectMode
            ElseIf o = "inf" Then
                ParseMode = ParsingMode.InfMode
            ElseIf o = "" Then
                ParseMode = ParsingMode.GeneralMode
            Else
                Report("Unrecognized Token " & o & " in line " & lineNumber) : Exit Sub
            End If


        End If
        If InStr(str, "}") > 1 Then
            Report("Error! '}' should be in a new line!!! in line " & lineNumber) : Exit Sub
        End If

        If InStr(str, "}") = 1 Then
            If ParseMode = ParsingMode.FinishedSelectionMode Then
                Report("Error! Was finishing selection and I'm finishing it again in line " & lineNumber)
            ElseIf ParseMode = ParsingMode.InfMode Then
                INF.Save()
                ParseMode = ParsingMode.GeneralMode
            ElseIf ParseMode = ParsingMode.SelectMode Then
                ParseMode = ParsingMode.FinishedSelectionMode
                'Here, we have finished the selection
            End If
        End If
    End Sub


End Module

