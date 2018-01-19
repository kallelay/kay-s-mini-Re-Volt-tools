Imports SevenZip

Module _7zForRevolt
    Public RvDir As String
    Public Te As TrackInfo
    Public Fname As String
    Structure TrackInfo

        Dim Name As String
        Dim Creator As String
        Dim Classu As CarClass
        Dim Crassu As String
        Dim Length As String
        Dim pic As String
        Dim foldername As String
        Dim Polies As Long
    End Structure

    Enum CarClass
        Unknown = 0
        Original = 1
        Conversion = 2
        Remake = 3
        Repaint = 4
    End Enum

    Sub CarCompress(ByVal car As String, ByVal archive As String)
        Randomize()
        If car(Len(car) - 1) = "\" Then car = Mid(car, 1, Len(car) - 1)
        Dim levelname = car.Split("\")(UBound(car.Split("\")))
        Dim gen As Integer
        gen = Int(Rnd() * 2 ^ 30)
        Dim tempdir = Environ("temp") & "\" & gen & "\cars\" & levelname & "\"


        'MkDir(Environ("tmp") & "\" & gen & "\")
        IO.Directory.CreateDirectory(tempdir)


        Dim alllevels() As String = IO.Directory.GetDirectories(car)
        For i = LBound(alllevels) To UBound(alllevels)
            IO.Directory.CreateDirectory(tempdir & alllevels(i).Split("\")(UBound(alllevels(i).Split("\"))))
            Dim allfiles() As String = IO.Directory.GetFiles(alllevels(i))
            For j = LBound(allfiles) To UBound(allfiles)

                IO.File.Copy(allfiles(j), tempdir & allfiles(j).Split("\")(UBound(allfiles(j).Split("\")) - 1) & "\" & allfiles(j).Split("\")(UBound(allfiles(j).Split("\"))))


            Next



        Next

        Dim allfiles_ = IO.Directory.GetFiles(car)
        For j = LBound(allfiles_) To UBound(allfiles_)


            IO.File.Copy(allfiles_(j), tempdir & allfiles_(j).Split("\")(UBound(allfiles_(j).Split("\"))))

        Next

        Dim y As New SevenZipCompressor()
        y.CompressionLevel = CompressionLevel.Ultra
        y.ArchiveFormat = OutArchiveFormat.SevenZip
        y.DirectoryStructure = True
        y.CompressDirectory(tempdir, archive)

        Kill(tempdir & "*.*")

        alllevels = IO.Directory.GetDirectories(car)
        For i = LBound(alllevels) To UBound(alllevels)
            Kill(alllevels(i) & "\*.*")
            For Each Str As String In IO.Directory.GetFiles(alllevels(i))
                Kill(Str)
            Next
            Try
                IO.Directory.Delete(alllevels(i))
            Catch
            End Try
        Next
        Try
            IO.Directory.Delete(tempdir)
            IO.Directory.Delete(Environ("temp") & "\" & gen & "\cars\")
            IO.Directory.Delete(Environ("temp") & "\" & gen)
        Catch
        End Try
    End Sub
    Function CarInfoFile(ByVal archivePath As String) As TrackInfo
        'Dim n As New IO.FileStream(Environ("temp") & "\info.inf", IO.FileMode.OpenOrCreate)
        Dim n As New IO.FileStream(Environ("temp") & "\temp_00", IO.FileMode.OpenOrCreate)


        'garden1,kallel,extreme,768

        Dim x As New SevenZipExtractor(archivePath.Replace(Chr(34), ""))
        x.ExtractFile("INFO", n)
        Dim V As New IO.StreamReader(n)

        Dim na As New IO.FileStream(Environ("temp") & "\temp_01.png", IO.FileMode.OpenOrCreate)
        Dim nb As New IO.FileStream(Environ("temp") & "\CAR", IO.FileMode.OpenOrCreate)





        Dim xa As New SevenZipExtractor(archivePath.Replace(Chr(34), ""))
        Try
            xa.ExtractFile("GFX.png", na)
            Dim Va As New IO.StreamReader(na)
            Va.ReadToEnd()
            Va.Close()
            na.Close()



        Catch
        End Try

        xa.ExtractFile("CAR", nb)
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
        Temp.pic = Environ("temp") & "\temp_01.png"
        Try
            IO.File.Delete(Environ("temp") & "\temp_02.png")
        Catch ex As Exception

        End Try


        IO.File.Copy(Environ("temp") & "\temp_01.png", Environ("temp") & "\temp_02.png")
        Temp.Crassu = Replace(Temp.Classu.ToString.Replace("_", " "), "  ", ": ")



        Dim o As New SevenZipExtractor(Environ("temp") & "\CAR")
        Dim fsize As Long = 0
        For po = 0 To o.ArchiveFileData.Count - 1
            fsize += o.ArchiveFileData(po).Size
        Next





        Return Temp


    End Function

    Sub ExtractAll()



        Dim x As New SevenZipExtractor(Environ("temp") & "\CAR")
        x.PreserveDirectoryStructure = True

        x.ExtractArchive(RvDir)





    End Sub
End Module
