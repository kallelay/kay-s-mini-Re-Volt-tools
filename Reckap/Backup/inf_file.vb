Imports System.Drawing

Module INF_FILE_MODULE
    Public PublicGateCanPass = False
    Dim paramfile$()
    Class inf_file
        Dim fpath$
        Public NAME$
        Public STARTPOS As Vector3D = New Vector3D
        Public STARTPOSREV As Vector3D = New Vector3D
        Public STARTROT As Single = 0.0F
        Public STARTROTREV As Single = 0.0F
        Public STARTGRID As Integer
        Public STARTGRIDREV As Integer
        Public FARCLIP As Single = 0.0F
        Public FOGSTART As Long
        Public FOGCOLOR As Color
        Public VERTFOGSTART As Long
        Public VERTFOGEND As Long
        Public WORLDRGBPER As Long
        Public MODELRGBPER As Long
        Public MP3 As String
        Public REDBOOK As String
        Public SHADOWBOX As Integer
        Public ENVSTILL As String
        Public ENVROLL As String
        Public ROCK As String

        Sub New(ByVal path$)
            fpath = path
            Try
                paramfile = IO.File.ReadAllLines(path$)
                For i = 0 To paramfile.Count - 1
                    paramfile(i) = Trim(paramfile(i))
                    getString(paramfile(i), "NAME", NAME)
                    get3D(paramfile(i), "STARTPOS", STARTPOS)
                    get3D(paramfile(i), "STARTPOSREV", STARTPOSREV)
                    getString(paramfile(i), "STARTROT", STARTROT)
                    getString(paramfile(i), "STARTROTREV", STARTROTREV)
                    getString(paramfile(i), "STARTGRID", STARTGRID)
                    getString(paramfile(i), "STARTGRIDREV", STARTGRIDREV)
                    getString(paramfile(i), "FOGSTART", FOGSTART)
                    getColor(paramfile(i), "FOGCOLOR", FOGCOLOR)
                    getString(paramfile(i), "FARCLIP", FARCLIP)
                    getString(paramfile(i), "WORLDRGBPER", WORLDRGBPER)
                    getString(paramfile(i), "MODELRGBPER", MODELRGBPER)
                    'missing
                    getString(paramfile(i), "MP3", MP3)
                    getString(paramfile(i), "ENVSTILL", ENVSTILL)
                    getString(paramfile(i), "ENVROLL", ENVROLL)
                    getString(paramfile(i), "ROCK", ROCK)
                    PublicGateCanPass = True
                Next
            Catch ex As Exception
                MsgBox(ex.Message & "!!" & vbNewLine & "Rename folder back!!! ")
                PublicGateCanPass = False
            End Try

        End Sub
        Sub Save()
            For i = 0 To paramfile.Count - 1
                paramfile(i) = Trim(paramfile(i))
                setString(paramfile(i), "NAME", NAME)
                set3D(paramfile(i), "STARTPOS", STARTPOS)
                set3D(paramfile(i), "STARTPOSREV", STARTPOSREV)
                setNumber(paramfile(i), "STARTROT", STARTROT)
                setNumber(paramfile(i), "STARTROTREV", STARTROTREV)
                setNumber(paramfile(i), "STARTGRID", STARTGRID)
                setNumber(paramfile(i), "STARTGRIDREV", STARTGRIDREV)
                setNumber(paramfile(i), "FOGSTART", FOGSTART)
                setColor(paramfile(i), "FOGCOLOR", FOGCOLOR)
                setNumber(paramfile(i), "FARCLIP", FARCLIP)
                setNumber(paramfile(i), "WORLDRGBPER", WORLDRGBPER)
                setNumber(paramfile(i), "MODELRGBPER", MODELRGBPER)
                'missing
                setString(paramfile(i), "MP3", MP3)
                setString(paramfile(i), "ENVSTILL", ENVSTILL)
                setString(paramfile(i), "ENVROLL", ENVROLL)
                setString(paramfile(i), "ROCK", ROCK)
            Next

            IO.File.WriteAllLines(RvDir & "\levels\" & dirName & "\" & dirName & ".inf", paramfile)


        End Sub


    End Class
    Sub getString(ByVal line$, ByVal key$, ByRef that$)
        If InStr(line, key & vbTab) + InStr(line, key & " ") = 1 Then
            that = Trim(cclean(Mid(line, Len(key) + 1)))
        End If
    End Sub
    Sub getColor(ByVal line$, ByVal key$, ByRef that As Color)
        Dim tmp = ""
        If InStr(line, key & vbTab) + InStr(line, key & " ") = 1 Then
            tmp = Trim(cclean(Mid(line, Len(key) + 1)))
            that = Color.FromArgb(Split(tmp, " ")(0), Split(tmp, " ")(1), Split(tmp, " ")(2))
            tmp = Nothing
        End If

    End Sub
    Sub get3D(ByVal line$, ByVal key$, ByRef that As Vector3D)
        Dim tmp = ""
        If InStr(line, key & vbTab) + InStr(line, key & " ") = 1 Then
            tmp = Trim(cclean(Mid(line, Len(key) + 1)))
            that = New Vector3D(Split(tmp, " ")(0), Split(tmp, " ")(1), Split(tmp, " ")(2))
            tmp = Nothing
        End If
    End Sub
    Sub setString(ByRef line$, ByVal key$, ByRef that$)
        If InStr(line, key & vbTab) + InStr(line, key & " ") = 1 Then
            line = key & vbTab & vbTab & If(key.Length > 8, vbTab, "") & "'" & that & "'"
        End If
    End Sub
    Sub setNumber(ByRef line$, ByVal key$, ByRef that$)
        If InStr(line, key & vbTab) + InStr(line, key & " ") = 1 Then
            line = key & vbTab & If(key.Length > 8, vbTab, "") & vbTab & that
        End If
    End Sub
    Sub setColor(ByRef line$, ByVal key$, ByRef that As Color)

        If InStr(line, key & vbTab) + InStr(line, key & " ") = 1 Then
            line = key & vbTab & vbTab & If(key.Length > 8, vbTab, "") & that.R & vbTab & that.G & vbTab & that.B
        End If


    End Sub
    Sub set3D(ByRef line$, ByVal key$, ByRef that As Vector3D)

        If InStr(line, key & vbTab) + InStr(line, key & " ") = 1 Then
            line = key & vbTab & vbTab & If(key.Length > 8, vbTab, "") & that.x & vbTab & that.y & vbTab & that.z
        End If


    End Sub
End Module
