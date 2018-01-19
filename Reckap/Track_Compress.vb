Imports Ionic.Zip

Module Track_Compress
    Public Dirt = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Re-Volt Track archives\"
    Public FileSize As Long
    Sub TrackCompress()

        If IO.Directory.Exists(Dirt) = False Then My.Computer.FileSystem.CreateDirectory(Dirt)

        Randomize()


        Dim gen As Integer
        gen = Int(Rnd() * 2 ^ 30)
        Dim tempdir = Environ("temp") & "\" & gen & "\levels\" & dirName & "\"

        Dim mpath = RvDir & "\levels\" & dirName & "\"


        'MkDir(Environ("tmp") & "\" & gen & "\")
        IO.Directory.CreateDirectory(tempdir)


        Dim allDirlevels As New List(Of String)
        Dim allFileslevels As New List(Of String)
        For j = 0 To Form1.CheckedListBox2.CheckedItems.Count - 1
            If Right(Form1.CheckedListBox2.CheckedItems(j), 1) = "\" Then allDirlevels.Add(Form1.CheckedListBox2.CheckedItems(j)) Else _
            allFileslevels.Add(mpath & Form1.CheckedListBox2.CheckedItems(j))
        Next


        'copy directories
        Dim i = 0
        For Each folder In allDirlevels
            i += 1
            Application.DoEvents()
            Form1.Label38.Text = "Copying" & readme_generator.chRp(".", 12 - Len("Copying")) & Int(i / allDirlevels.Count)
            IO.Directory.CreateDirectory(tempdir & folder)
            Dim allfiles() As String = IO.Directory.GetFiles(mpath & folder)
               For j = LBound(allfiles) To UBound(allfiles)
                IO.File.Copy(allfiles(j), tempdir & allfiles(j).Split("\")(UBound(allfiles(j).Split("\")) - 1) & "\" & allfiles(j).Split("\")(UBound(allfiles(j).Split("\"))))
            Next
   
        Next

        'copy gfx
        Try
            MkDir(tempdir & "..\..\gfx\")
        Catch ex As Exception

        End Try
        IO.File.Copy(mpath & "\..\..\gfx\" & dirName & ".bmp", tempdir & "\..\..\gfx\" & dirName & ".bmp")
        Try : IO.File.Copy(mpath & "\..\..\gfx\" & dirName & ".bmq", tempdir & "\..\..\gfx\" & dirName & ".bmq") : Catch : End Try


        Dim allfiles_ = allFileslevels.ToArray
        For j = LBound(allfiles_) To UBound(allfiles_)
            Application.DoEvents()
            Form1.Label38.Text = "Copying" & readme_generator.chRp(".", 12 - Len("Copying")) & Int(j / UBound(allfiles_))


            IO.File.Copy(allfiles_(j), tempdir & allfiles_(j).Split("\")(UBound(allfiles_(j).Split("\"))))

        Next

        Dim y As New ZipFile()
        y.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression
        y.AddDirectory(Environ("temp") & "\" & gen)

        AddHandler y.SaveProgress, AddressOf updateZipPg
        kso = -1
        Try : My.Computer.FileSystem.DeleteFile(Dirt & dirName & ".zip") : Catch : End Try
        y.Save(Dirt & dirName & ".zip")

        Kill(tempdir & "*.*")

        Try
            IO.Directory.Delete(tempdir, True)
            IO.Directory.Delete(Environ("temp") & "\" & gen, True)
        Catch
        End Try

        If Form1.CheckBox1.Checked Then Shell("explorer """ & Dirt & """", AppWinStyle.NormalFocus)
        Form1.Label38.Text = "Finished!..... 100%"

        Dim tcs, tps As ULong
        Dim crat! = 0
        For i = 0 To y.Entries.Count - 1
            tcs += y.Entries(i).CompressedSize
            tps += y.Entries(i).UncompressedSize
            crat += y.Entries(i).CompressionRatio / y.Entries.Count
        Next
        FileSize = tcs
        Form1.Label40.Text = "Compression Ratio: " & Math.Round(crat, 2) & "%"
        Form1.Label39.Text = "Final size : " & Math.Round(tcs / (1048576), 2) & "MB" & vbNewLine & "[Uncompressed : " & Math.Round(tps / (1048576), 2) & "MB]"

    End Sub
    Dim kso%
    Dim lpr As Ionic.Zip.ZipEntry
    Sub updateZipPg(ByVal sender As Ionic.Zip.ZipFile, ByVal e As Ionic.Zip.SaveProgressEventArgs)
        Application.DoEvents()
        If lpr IsNot e.CurrentEntry Then kso += 1
        lpr = e.CurrentEntry

        Form1.Label38.Text = "Compressing" & readme_generator.chRp(".", 12 - Len("Copying")) & Math.Round(kso / sender.Count * 100) & "%"
        If e.CurrentEntry IsNot Nothing Then Form1.Label40.Text = e.CurrentEntry.FileName.Split("\").Last '& "........." & 
        Dim poo$ = Math.Round(e.BytesTransferred / e.TotalBytesToTransfer * 100, 2)
        Try : If isNumber(CInt(poo)) = False Then : poo = "0" : End If : Catch : poo = "0" : End Try
        Form1.Label39.Text = Format(CSng(poo), "00.#0") & "% " & readme_generator.chRp(".", 6) & Format(Math.Round(e.TotalBytesToTransfer / (1024 * 1024), 2), "0.00") & "MB"

    End Sub
End Module
