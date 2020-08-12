Imports System.Drawing

Module Module1
    Public dirName$ = Split(Environment.CurrentDirectory, "\").Last
    Public FullPath$ = Environment.CurrentDirectory & "\"


    Public myColor As Color = Color.White
    Public ShMode As shadingMode = shadingMode.multiply
    Public MyColorMode = 0%
    Public sunColor As Color = Color.White

    Public mySurface As Long
    Public SurfMultiplier = 0%

    Public INF As inf_file
    Public Fob As FOB_File
    Public NCP As ncp_file


    Public curWorld As WORLD
    Public ldir As Vector3D
    Public loff As Vector3D

    Sub Main()

        If IO.File.Exists(FullPath & dirName & ".inf") = False Then
            Console.WriteLine("ERROR : " & dirName & ".inf doesn't exist!")

            '  If My.Application.CommandLineArgs.Count = 0 Then
            'Console.WriteLine("ERROR: empty argument")
            'ElseIf IO.File.Exists(Command) Then
            '     kayProcess(Command)
            '      Exit Sub
            ' Else
            '     Console.WriteLine("ERROR: " & Command() & " doesn't exist")
            '   End If


        End If

        'param file get
        INF = New inf_file(FullPath & dirName & ".inf")



        'GFX
        If IO.File.Exists(FullPath & "..\..\gfx\" & dirName & ".bmp") = False Then
            gui.PictureBox1.ImageLocation = FullPath & "..\..\gfx\acclaim.bmp"
        Else
            gui.PictureBox1.ImageLocation = FullPath & "..\..\gfx\" & dirName & ".bmp"
        End If


        'get num of polys
        gui.trpoly.Text = CountVer(FullPath & dirName & ".w")

        'get length
        Dim Fast As New IO.BinaryReader(New IO.FileStream(FullPath & dirName & ".pan", IO.FileMode.Open))
        Fast.ReadBytes(8)
        Try
            gui.trlen.Text = Int(Fast.ReadSingle() / 200) & "m"
        Catch ex As Exception
        End Try
        Fast.Close()



        ' paste different inf
        gui.Panel1.BackColor = INF.FOGCOLOR
        gui.trname.Text = INF.NAME
        gui.stpos.Text = INF.STARTPOS.x & " " & INF.STARTPOS.y & " " & INF.STARTPOS.z


        'Do we have original?
        If IO.File.Exists(FullPath & dirName & "_original.w") = False Then
            IO.File.Copy(FullPath & dirName & ".w", FullPath & dirName & "_original.w")
        End If
        If IO.File.Exists(FullPath & dirName & "_original.ncp") = False Then
            IO.File.Copy(FullPath & dirName & ".ncp", FullPath & dirName & "_original.ncp")
        End If

        'Load world
        curWorld = New WORLD(FullPath & dirName & ".w")

        'Load NCP
        NCP = New ncp_file(FullPath & dirName & "_original.ncp")


        'load fob
        Fob = New FOB_File(FullPath & dirName & ".fob")

        'Show fob file
        Dim rainobj = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_RAIN)
        gui.NumericUpDown11.Value = rainobj.Count

        Dim skybox = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_SKYBOX)
        gui.CheckBox1.Checked = skybox.Count > 0


        'Alright....
        gui.ShowDialog()




    End Sub
    Function cclean(ByVal str$)

        Return Split(Replace(Replace(Replace(str, vbTab, " "), Chr(34), ""), "'", ""), ";")(0)
    End Function
    Sub getMeThis(ByVal line$, ByVal key$, ByRef that$)
        If InStr(line, key) = 1 Then
            that = Trim(cclean(Mid(line, Len(key) + 1)))
        End If

    End Sub
End Module
