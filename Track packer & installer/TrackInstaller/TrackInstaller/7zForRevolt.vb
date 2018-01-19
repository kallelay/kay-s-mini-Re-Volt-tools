Imports SevenZip

Module _7zForRevolt
    Public RvDir As String
    Public Te As TrackInfo
    Public Fname As String
    Structure TrackInfo

        Dim Name As String
        Dim Creator As String
        Dim Classu As TrackClass
        Dim Crassu As String
        Dim Length As Integer
        Dim pic As String
        Dim foldername As String
        Dim Polies As Long
    End Structure
    Enum TrackClass
        Unknown = 0
        Race__Lego_ = 1
        Race__Lego_Extreme = 2
        Race__Extreme = 3
        Race__Lego_FullCustom = 11
        Race__Lego_Extreme_FullCustom = 21
        Race__Extreme_FullCustom = 31
        Battle_Tag = 4
        Stunt_Arena = 5
        PRM_Kit = 6
        Frontend = 7

    End Enum
    Sub LevelCompress(ByVal level As String, ByVal archive As String)
        Randomize()
        Dim levelname = level.Split("\")(UBound(level.Split("\")))
        Dim gen As Integer
        gen = Int(Rnd() * 2 ^ 30)
        Dim tempdir = Environ("temp") & "\" & gen & "\levels\" & levelname & "\"
        'MkDir(Environ("tmp") & "\" & gen & "\")
        IO.Directory.CreateDirectory(tempdir)


        Dim alllevels() As String = IO.Directory.GetDirectories(level)
        For i = LBound(alllevels) To UBound(alllevels)
            IO.Directory.CreateDirectory(tempdir & alllevels(i).Split("\")(UBound(alllevels(i).Split("\"))))
            Dim allfiles() As String = IO.Directory.GetFiles(alllevels(i))
            For j = LBound(allfiles) To UBound(allfiles)

                IO.File.Copy(allfiles(j), tempdir & allfiles(j).Split("\")(UBound(allfiles(j).Split("\")) - 1) & "\" & allfiles(j).Split("\")(UBound(allfiles(j).Split("\"))))


            Next



        Next

        Dim allfiles_ = IO.Directory.GetFiles(level)
        For j = LBound(allfiles_) To UBound(allfiles_)


            IO.File.Copy(allfiles_(j), tempdir & allfiles_(j).Split("\")(UBound(allfiles_(j).Split("\"))))

        Next

        Dim y As New SevenZipCompressor()
        y.CompressionLevel = CompressionLevel.Ultra
        y.ArchiveFormat = OutArchiveFormat.SevenZip
        y.CompressDirectory(tempdir, archive)

        Kill(tempdir & "*.*")

        alllevels = IO.Directory.GetDirectories(level)
        For i = LBound(alllevels) To UBound(alllevels)
            Kill(alllevels(i) & "\*.*")
            For Each Str As String In IO.Directory.GetFiles(alllevels(i))
                Kill(Str)
            Next

            IO.Directory.Delete(alllevels(i))
        Next

        IO.Directory.Delete(tempdir)
        IO.Directory.Delete(Environ("temp") & "\" & gen & "\levels\")
        IO.Directory.Delete(Environ("temp") & "\" & gen)
    End Sub
    Function TrackInfoFile(ByVal archivePath As String) As TrackInfo
        'Dim n As New IO.FileStream(Environ("temp") & "\info.inf", IO.FileMode.OpenOrCreate)
        Dim n As New IO.FileStream(Environ("temp") & "\temp_00", IO.FileMode.OpenOrCreate)


        'garden1,kallel,extreme,768

        Dim x As New SevenZipExtractor(archivePath.Replace(Chr(34), ""))
        x.ExtractFile("INFO", n)
        Dim V As New IO.StreamReader(n)

        Dim na As New IO.FileStream(Environ("temp") & "\temp_01.bmp", IO.FileMode.OpenOrCreate)
        Dim nb As New IO.FileStream(Environ("temp") & "\track", IO.FileMode.OpenOrCreate)





        Dim xa As New SevenZipExtractor(archivePath.Replace(Chr(34), ""))
        Try
            xa.ExtractFile("GFX.bmp", na)
            Dim Va As New IO.StreamReader(na)
            Va.ReadToEnd()
            Va.Close()
            na.Close()



        Catch
        End Try

        xa.ExtractFile("TRACK", nb)
        Dim Vb As New IO.StreamReader(nb)
        Vb.ReadToEnd()
        Vb.Close()



        V.ReadToEnd()
        V.Close()
        n.Close()

        Dim SR = IO.File.ReadAllText(Environ("temp") & "\temp_00")

        Dim Temp As New TrackInfo()

        Temp.Name = Split(SR, ",")(0)
        
        Temp.Creator = Split(SR, ",")(1)
        Temp.Classu = Split(SR, ",")(2)
        Temp.Length = Split(SR, ",")(3)
        Temp.foldername = Split(SR, ",")(4)
        Fname = Temp.foldername
        If Fname(Len(Fname) - 1) = " " Then Fname = Mid(Fname, 1, Len(Fname) - 1)
        Try
            Temp.Polies = CLng(Split(SR, ",")(5))
        Catch
            Temp.Polies = 0
        End Try
        Temp.pic = Environ("temp") & "\temp_01.bmp"
        Try
            IO.File.Delete(Environ("temp") & "\temp_02.bmp")
        Catch ex As Exception

        End Try


        IO.File.Copy(Environ("temp") & "\temp_01.bmp", Environ("temp") & "\temp_02.bmp")
        Temp.Crassu = Replace(Replace(Temp.Classu.ToString.Replace("_", " "), "Full Custom", "(Full Custom)", 1, -1, 1), "  ", ": ")



        Dim o As New SevenZipExtractor(Environ("temp") & "\track")
        Dim fsize As Long = 0
        For po = 0 To o.ArchiveFileData.Count - 1
            fsize += o.ArchiveFileData(po).Size
        Next
        Form1.Label15.Text = Mid(fsize / (1024 * 1024), 1, 4) & "MB"




        Return Temp


    End Function

    Sub ExtractAll()
        '  Form1.PictureBox1.Image.Save(Chr(34) & RvDir & "\gfx\" & Te.foldername & ".bmp" & Chr(34), Drawing.Imaging.ImageFormat.Bmp)
        'InputBox("", "", RvDir & "\gfx\" & Te.foldername & ".bmp")
        'ChDir(RvDir)
        Try
            IO.File.Delete(RvDir & "\gfx\" & Fname & ".bmp")
        Catch ex As Exception

        End Try

        IO.File.Copy(Environ("temp") & "\temp_02.bmp", RvDir & "\gfx\" & Fname & ".bmp")

        '  Catch ex As Exception
        'IO.File.Replace(Environ("temp") & "\temp_02.bmp", RvDir & "\gfx\" & Fname & ".bmp", RvDir & "\gfx\" & Fname & "_BACKUP.bmp")

        '  End Try

        '  FileCopy(Environ("temp") & "\temp_01.bmp", RvDir & "\gfx\" & Te.foldername & ".bmp")

        '

        Dim x As New SevenZipExtractor(Environ("temp") & "\track")
        x.PreserveDirectoryStructure = True

        x.ExtractArchive(RvDir)





    End Sub
End Module
