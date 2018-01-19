Imports System.Drawing

Module Module1
    Public dirName$ = Split(Environment.CurrentDirectory, "\").Last
    Public FullPath$ = Environment.CurrentDirectory & "\"
    Public RvDir$


    Public mySurface As Long
    Public SurfMultiplier = 0%

    Public INF As inf_file
    Public Fob As FOB_File
    Public curWorld As WORLD
    Public NCP As ncp_file

    Public ldir As Vector3D
    Public loff As Vector3D

    Sub Main()
        dirName$ = Split(Environment.CurrentDirectory, "\").Last
        FullPath$ = Environment.CurrentDirectory & "\"

        If IO.File.Exists(FullPath & dirName & ".inf") = False Then
            Console.WriteLine("ERROR : " & dirName & ".inf doesn't exist!")

        End If


        RvDir = Mid(FullPath, 1, Len(FullPath) - Len(Split(FullPath, "\").Last) - 1)
        RvDir = Mid(RvDir, 1, Len(RvDir) - Len(Split(RvDir, "\").Last) - 1)
        RvDir = Mid(RvDir, 1, Len(RvDir) - Len(Split(RvDir, "\").Last) - 1)

        'param file get
        INF = New inf_file(FullPath & dirName & ".inf")



        'GFX
        If IO.File.Exists(FullPath & "..\..\gfx\" & dirName & ".bmp") = False Then
            Form1.PictureBox2.ImageLocation = FullPath & "..\..\gfx\acclaim.bmp"

            Form1.Button12.Enabled = False 'need to pick gfx file first!
        Else

            Form1.PictureBox2.ImageLocation = FullPath & "..\..\gfx\" & dirName & ".bmp"
            Form1.PictureBox1.ImageLocation = FullPath & "..\..\gfx\" & dirName & ".bmp"
        End If


        'get num of polys
        Form1.trpoly.Text = CountVer(FullPath & dirName & ".w")

        'get length
        Try
            Dim Fast As New IO.BinaryReader(New IO.FileStream(FullPath & dirName & ".pan", IO.FileMode.Open))
            Fast.ReadBytes(8)
            Try
                Form1.trlen.Text = Int(Fast.ReadSingle() / 200) & "m"
            Catch ex As Exception
            End Try
            Fast.Close()
        Catch ex As Exception

        End Try
     



        ' paste different inf
        Form1.Panel5.BackColor = INF.FOGCOLOR
        Form1.trname.Text = INF.NAME




        'Load world
        curWorld = New WORLD(FullPath & dirName & ".w")

        'Load NCP
        NCP = New ncp_file(FullPath & dirName & ".ncp")


        'load fob
        Fob = New FOB_File(FullPath & dirName & ".fob")



        Form1.TextBox1.Text = dirName
        Form1.TextBox2.Text = INF.NAME

        'errors
        searchForErrors()

        'skybox
        Dim skb = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_SKYBOX)
        If skb.Count = 0 Then
            Form1.ComboBox1.SelectedIndex = 0
        Else

            Form1.ComboBox1.SelectedIndex = Fob.ObjList(skb.Last()).Flag(0) + 1
        End If

        Form1.NumericUpDown11.Value = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_RAIN).Count
        Form1.NumericUpDown1.Value = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_LIGHTNING).Count

        Form1.Button5.Show()


        'enable bmo to bmp
        If IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmo").Count > 0 And IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmo").Count = 0 Then
            Form1.GroupBox3.Enabled = True
        Else
            Form1.GroupBox3.Enabled = False
        End If

        Form1.getTheFilesToBeZipped()

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
