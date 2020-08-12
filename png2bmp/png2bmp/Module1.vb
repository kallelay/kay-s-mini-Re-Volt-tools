Imports System.Drawing
Module Module1

    Sub Main()
        writeHello()
        For Each x$ In My.Application.CommandLineArgs
            If IO.File.Exists(x) Then
                warn("converting " & x)
                Convert(x)
            Else
                warn("File " & x & " doesn't exist")
            End If
        Next

        Dim dirname = Split(CurDir, "\")(Split(CurDir, "\").Length - 1)
        For i = 0 To 25
            If Not IO.File.Exists((CurDir() & "\" & dirname & Chr(65 + i) & ".png")) Then perror((CurDir() & "\" & dirname & Chr(65 + i) & ".png doesn't exist")) : Continue For
            Console.WriteLine("Converting " & dirname & Chr(65 + i) & ".png")
            Convert(CurDir() & "\" & dirname & Chr(65 + i) & ".png")
        Next
    End Sub

    Sub Convert(ByVal PNGfilename$)
        Dim str = New IO.FileStream(PNGfilename, IO.FileMode.Open, IO.FileAccess.Read)

        Dim bmp = New Bitmap(str)
        Dim x As New Drawing.ImageConverter
        Try

            If IO.File.Exists(setExt(PNGfilename, "png", "bmp")) Then IO.File.Delete(setExt(PNGfilename, "png", "bmp"))
            bmp.Save(setExt(PNGfilename, "png", "bmp"), Imaging.ImageFormat.Bmp)
            str.Close()
        Catch ex As Exception
            perror("Couldn't write " & setExt(PNGfilename, "png", "bmp") & ". " & vbNewLine & "Check whether it's because of permission settings or it's opened somehwere.")
        End Try

       

    End Sub
    Function setExt(ByVal fname$, ByVal oldext$, ByVal newext$) As String
        Return Replace(fname, "." & oldext, "." & newext, , , CompareMethod.Text)
    End Function

    Sub WriteHello()
        Console.WriteLine("--------------------------------------------")
        Console.WriteLine(" PNG2BMP - part of ASE2RV9")
        Console.WriteLine("         by A.Y.K (c) 2013-2014, 2019")
        Console.WriteLine("      licensed under GNU GPL")
        Console.WriteLine("--------------------------------------------")
    End Sub
    Sub perror(ByVal str$)
        Console.ForegroundColor = ConsoleColor.White

        Console.WriteLine(str)
        Console.ResetColor()
    End Sub
    Sub warn(ByVal str$)
        Console.ForegroundColor = ConsoleColor.Gray
        Console.WriteLine(str)
        Console.ResetColor()
    End Sub
End Module
