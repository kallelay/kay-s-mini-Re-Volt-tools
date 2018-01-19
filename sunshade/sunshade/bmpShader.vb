Imports System.Drawing

Module bmpShader
    Dim ListOfBitmaps As New List(Of String)
    Sub LoadBitmapList()
        ' Dim path$ = FullPath$
        ListOfBitmaps.Clear()
        ListOfBitmaps.AddRange(IO.Directory.GetFiles(FullPath & "\thor_backup\", "*.bm?"))
    End Sub
    Sub LoadSkyboxList()
        ' Dim path$ = FullPath$
        ListOfBitmaps.Clear()
        ListOfBitmaps.AddRange(IO.Directory.GetFiles(FullPath & "\thor_backup\", "sky*.bm?"))
    End Sub
    Sub BackUpBitmaps()
        Dim npath$ = FullPath & "\thor_backup\"
        If IO.Directory.Exists(npath$) = False Then
            IO.Directory.CreateDirectory(npath$)
            Report("Creating backup folder....")
            Dim v = 0
            For Each item In ListOfBitmaps
                v += 1
                IO.File.Copy(item, npath$ & Split(item, "\").Last)
                Report("Backing up bitmaps .... [" & Str(Math.Round(v * 100.0 / (ListOfBitmaps.Count - 1), 2)) & "%]")
            Next
        End If
      
        npath = Nothing
    End Sub
    Sub ShadeBitmap(ByVal bmppath$, ByVal c As Color)
        Dim stream As New IO.FileStream(bmppath, IO.FileMode.Open, IO.FileAccess.Read)
        Dim b As Bitmap = Bitmap.FromStream(stream)
        Dim tpx As Color
        If b.PixelFormat = Imaging.PixelFormat.Format32bppRgb Then
            'Alpha BMPX
            For i = 0 To b.Height - 1
                For j = 0 To b.Width - 1

                    tpx = b.GetPixel(i, j)
                    ShadeF(tpx, c.R / 255.0, c.G / 255.0, c.B / 255.0)
                    b.SetPixel(i, j, tpx)
                Next
            Next
        Else
            'Assuming it's BMP24 or BMP16

            'Alpha BMPX
            For i = 0 To b.Width - 1
                For j = 0 To b.Height - 1

                    tpx = b.GetPixel(i, j)
                    If Math.Max(tpx.R, Math.Max(tpx.G, tpx.B)) > 2 Then
                        ShadeF(tpx, c.R / 255.0, c.G / 255.0, c.B / 255.0)
                    End If

                    b.SetPixel(i, j, tpx)
                Next
            Next
        End If
        Try
            ' stream.Close()
            b.Save(Replace(bmppath, "\thor_backup\", ""), Imaging.ImageFormat.Bmp)
        Catch ex As Exception
            Report("Error... failed to shade " & bmppath)
        End Try

        stream = Nothing
        b.Dispose()
        tpx = Nothing

    End Sub

    Sub ShadeAllBitmaps(ByVal color As Color)
        LoadBitmapList()
        BackUpBitmaps()

        For i = 0 To ListOfBitmaps.Count - 1
            Dim item = ListOfBitmaps(i)
            Report("shading bitmaps ... [ " & Str(Math.Round(i * 100.0 / (ListOfBitmaps.Count - 1), 2)) & "%] " & item)
            ShadeBitmap(item, color)
            item = Nothing
        Next
    End Sub

    Sub ShadeSkybox(ByVal color As Color)
        LoadSkyboxList()
        BackUpBitmaps()

        For i = 0 To ListOfBitmaps.Count - 1
            Dim item = ListOfBitmaps(i)
            Report("shading bitmaps ... [ " & Str(Math.Round(i * 100.0 / (ListOfBitmaps.Count - 1), 2)) & "%] " & item)
            ShadeBitmap(item, color)
            item = Nothing
        Next
    End Sub
End Module
