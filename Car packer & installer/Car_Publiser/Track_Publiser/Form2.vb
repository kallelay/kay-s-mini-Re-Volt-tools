﻿Imports SevenZip

Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        
    End Sub

    Private Sub Form2_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Sub OK()
        Dim gen As Integer = Int(Rnd() * 5000)
        Dim Fpath As String = Environ("temp") & "\CAR" & gen '& "\TRACK"
        Randomize()
        Try
            MkDir(Fpath)
        Catch
            Fpath &= Int(5000 * Rnd())
            MkDir(Fpath)
        End Try

        CarCompress(RvDir & "\cars\" & Fname & "\", Fpath & "\CAR")
        Dim generated As String = ""
        With Normal
            generated = .TextBox1.Text & ","
            generated += .TextBox2.Text & ","
            If .ComboBox1.Text.Split(" ")(0) <> "00" Then
                generated += Int(.ComboBox1.Text.Split(" ")(0).Replace("0", "")) & ","
            Else
                generated += "0,"
            End If
            generated += .TextBox4.Text & ","
            generated += Fname & ","

        End With


        IO.File.WriteAllText(Fpath & "\INFO", generated)
        Try
            Normal.PictureBox2.Image.Save(Fpath & "\GFX.png", Imaging.ImageFormat.Png)
        Catch
            MsgBox("No GFX file!!!!????")
        End Try

        Try
            MkDir(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Rv Cars")
        Catch ex As Exception

        End Try
        Dim K As New SevenZipCompressor()
        K.CompressionLevel = CompressionLevel.Ultra
        K.ArchiveFormat = OutArchiveFormat.SevenZip
        K.CompressDirectory(Fpath, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Rv Cars\" & Fname & ".rvc")

        If MsgBox("Do you want to browse Rvc output folder?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Shell("explorer.exe " & Chr(34) & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Rv Cars\" & Chr(34), AppWinStyle.NormalFocus)
        End If
        Me.Hide()
        Normal.panel3.location = Normal.Panel1.Location
    End Sub
End Class