Imports System.Windows.Forms
Public Class Cleaner
#Region "Helper functions"
    Private Shared Function getNormalFiles(ByVal trackname$) As String()
        Dim temp(130) As String

        Dim i As Integer
        For i = 97 To 106
            temp(i - 97) = trackname & Chr(i) & ".bmp"
            temp(i - 97 + 10) = trackname & Chr(i) & ".bmq"
            temp(i - 97 + 20) = trackname & Chr(i) & ".bmo"
            temp(i - 97 + 30) = trackname & Chr(i) & ".bmr"
            temp(i - 97 + 40) = trackname & Chr(i) & ".bms"
            temp(i - 97 + 50) = trackname & Chr(i) & ".bmm"
            temp(i - 97 + 60) = trackname & Chr(i) & ".bmn"
        Next '30

        Dim j = 81
        temp(j) = trackname & ".fin"
        temp(j + 1) = trackname & ".fob"
        temp(j + 2) = trackname & ".lit"
        temp(j + 3) = trackname & ".cam"
        temp(j + 4) = trackname & ".rim"
        temp(j + 5) = trackname & ".taz"
        temp(j + 6) = trackname & ".pan"
        temp(j + 7) = trackname & ".w"
        temp(j + 8) = trackname & ".ncp"
        temp(j + 9) = trackname & ".tri"
        temp(j + 10) = trackname & ".vis"
        temp(j + 11) = trackname & ".por"
        temp(j + 12) = trackname & ".times"
        temp(j + 13) = "trolley.bmp"
        temp(j + 14) = "trolley.bmq"
        temp(j + 15) = "water.bmp"
        temp(j + 16) = "water.bmq"
        temp(j + 17) = trackname & ".inf"
        temp(j + 18) = trackname & ".fld"
        temp(j + 19) = trackname & ".fan"
        temp(j + 20) = "custom.ini"
        temp(j + 21) = "envstill.bmp"
        temp(j + 22) = "envroll.bmp"
        temp(j + 23) = "sky_bk.bmp"
        temp(j + 24) = "sky_bt.bmp"
        temp(j + 25) = "sky_ft.bmp"
        temp(j + 26) = "sky_tp.bmp"
        temp(j + 27) = "sky_lt.bmp"
        temp(j + 28) = "sky_rt.bmp"
        ' trolley

        'prm ncp times


        'fin fan fob lit cam rim 
        'taz pan 

        'prm

        'w ncp inf

        Return temp

    End Function

    Private Shared Function getNecessaryfiles(ByVal trackname$) As String()
        Dim temp(6) As String



        Dim j = 0
        temp(j) = trackname & ".pan"
        temp(j + 1) = trackname & ".w"
        temp(j + 2) = trackname & ".ncp"
        temp(j + 3) = trackname & ".taz"
        temp(j + 4) = trackname & ".inf"


        Return temp

    End Function
#End Region
    'First Pass: Junk files 
#Region "Main function"
    Public Shared Function getUncessaryFiles() As ListBox
        Dim Dirfiles() As String
        Dim finalFiles As New ListBox()
        Dim mpath = RvDir & "\levels\" & dirName ' If(Main.ActiveList = "RV", RvDir, PathBackUp) & "\levels\" & Main.LatestClickTrackName
        Dim found As Int16 = 0
        Dirfiles = IO.Directory.GetFiles(mpath)
        ' Debug.Print(Dirfiles(0))
        Dim NorFiles() As String = getNormalFiles(dirName)
        'Debug.Print(NorFiles(0))
        Dim i&
        For i = LBound(Dirfiles) To UBound(Dirfiles)
            found = 0
            Dim j As Integer
            For j = LBound(NorFiles) To UBound(NorFiles)

                '   MsgBox(LCase(NorFiles(j)))
                If LCase(NorFiles(j)) = LCase(Dirfiles(i).Split("\").Last) Then
                    found = 1
                End If

                ' Dim i

            Next
            If found = 0 Then
                'C:\Games\pc\revolt\revolt\levels\AMCO_Bitume



                Dim ka = Dirfiles(i).Split("\").Last()
                Try
                    Dim null = ka.Split(".")(1)
                    'MsgBox(null)
                    If null <> "ncp" And null <> "prm" And null <> "ace" And _
                         InStr(Dirfiles(i).Split("\").Last.ToLower(), "readme") = 0 And _
                 InStr(Dirfiles(i).Split("\").Last.ToLower(), "read") = 0 And _
                 InStr(Dirfiles(i).Split("\").Last.ToLower(), "lisez") = 0 And _
                 InStr(Dirfiles(i).Split("\").Last.ToLower(), "lire") = 0 And _
                 InStr(Dirfiles(i).Split("\").Last.ToLower(), "about") = 0 Then

                        finalFiles.Items.Add(Dirfiles(i).Split("\").Last)
                    End If



                Catch ex As Exception
                    finalFiles.Items.Add(Dirfiles(i).Split("\").Last)
                End Try





            End If

nexta:
        Next
        Return finalFiles

    End Function

    Public Shared Function NcpMatchPrm() As ListBox
        '  Dim mpath = If(Main.ActiveList = "RV", RvPath, PathBackUp) & "\levels\" & Main.LatestClickTrackName
        Dim mpath = RvDir & "\levels\" & dirName

        Dim LB As New ListBox
        Dim found = 0
        Dim ncps = IO.Directory.GetFiles(mpath, "*.ncp")
        Dim prms = IO.Directory.GetFiles(mpath, "*.prm")

        For i = LBound(ncps) To UBound(ncps)
            found = 0
            For j = LBound(prms) To UBound(prms)
                If Replace(ncps(i).Split("\").Last.ToLower(), "ncp", "prm") = prms(j).Split("\").Last.ToLower Then
                    found = 1
                End If
                If found = 0 Then found = 0
            Next

            If found = 0 And Replace(ncps(i).Split("\").Last.ToLower, ".ncp", "") <> CurDir.Split("\").Last.ToLower Then LB.Items.Add(ncps(i).Split("\").Last)
        Next

        For i = 0 To LB.Items.Count - 1
            If LCase(LB.Items(i)) = LCase(Replace(dirName, vbTab, "") & ".ncp") Then
                LB.Items.Remove(LB.Items(i))
                Exit For
            End If

        Next


        Return LB
    End Function
    Private Shared Function GetAllInstances() As ListBox
        Dim mpath = RvDir & "\levels\" & dirName
        Dim fullpath As String = mpath & "\" & dirName & ".fin"
        Dim LB As New ListBox()
        Dim instancecount As Int32
        Dim rb As New IO.BinaryReader(IO.File.Open(fullpath, IO.FileMode.Open))

        instancecount = rb.ReadInt32()


        For i = 0 To instancecount - 1
            Dim str As String = rb.ReadChars(8)
            Try
                str = Split(str, Chr(112))(0)

            Catch
            End Try
            LB.Items.Remove(str)
            LB.Items.Add(str)
            '  If  Then
            rb.ReadBytes(64)

        Next
        Return LB

    End Function

    Public Shared Function PRM_Clean() As ListBox
        Dim mpath = RvDir & "\levels\" & dirName
        Dim fullpath As String = mpath & "\" & dirName & ".fin"
        Dim LB_ As New ListBox
        For i = LBound(IO.Directory.GetFiles(mpath, "*.prm")) To UBound(IO.Directory.GetFiles(mpath, "*.prm"))
            Dim n = IO.Directory.GetFiles(mpath, "*.prm")
            Dim k = Mid(n(i).Split("\").Last, 1, 8)
            k = k.ToUpper
            If k.IndexOf(".") <> 0 Then k = k.Split(".")(0)
            LB_.Items.Add(k)
        Next




        Dim LB As New ListBox()
        Dim instancecount As Int32
        If IO.File.Exists(fullpath) = False Then Return New ListBox
        Dim rb As New IO.BinaryReader(IO.File.Open(fullpath, IO.FileMode.Open))

        instancecount = rb.ReadInt32()

        Dim str0 As String = ""
        For i = 0 To instancecount - 1
            str0 = rb.ReadChars(8)
            str0 = str0.ToUpper


            If InStr(str0, Chr(0)) > 0 Then _
            str0 = str0.Split(Chr(0))(0)

            rb.ReadBytes(64)

            LB_.Items.Remove(str0)




        Next
        rb.Close()
        For Each st As Object In LB_.Items
            LB.Items.Add(IO.Directory.GetFiles(mpath, st & "*.prm")(0).Split("\").Last)
            Try
                If LCase(IO.Directory.GetFiles(mpath, st & "*.ncp")(0).Split("\").Last) <> LCase(dirName & ".ncp") Then _
                LB.Items.Add(IO.Directory.GetFiles(mpath, st & "*.ncp")(0).Split("\").Last)
            Catch
            End Try

        Next

        For i = 0 To LB.Items.Count - 1
            If LCase(LB.Items(i)) = LCase(Replace(dirName, vbTab, "") & ".ncp") Then
                LB.Items.Remove(LB.Items(i))
                Exit For
            End If

        Next


        Return LB
    End Function
    Function CleanCurDir()

        Return True
    End Function
#End Region
End Class
