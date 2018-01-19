Module reportandlogs
    Dim pth$ = My.Computer.FileSystem.SpecialDirectories.Temp & "\"
    Dim _temp_$ = pth & "sunshadelog.txt"
    Dim fs As IO.FileStream = New IO.FileStream(_temp_, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
    Dim fo As IO.StreamWriter
    Sub Report(ByVal str$)
        If fo Is Nothing Then fo = New IO.StreamWriter(fs)
        fo.Flush()

        fo.WriteLine(str)




    End Sub
End Module
