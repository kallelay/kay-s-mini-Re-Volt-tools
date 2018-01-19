Imports System.IO

Module fin_file
    Public Class FILE_INSTANCE
        Public Name(9) As Char
        Public R, G, B As Char
        Public EnvRGB As Int32
        Public Priority, Flag, Pad(2) As Int16
        Public LodBias As Single
        Public WorldPos As Vector3D

        '  Public WorldMatrix As Matrix4
        Public center As Vector3D

        Public fullPath As String



    End Class

    Public Instances() As FILE_INSTANCE
    Public NotShadedInstances As New List(Of String)
    Sub Load_FIN()

        If Not IO.Directory.Exists(FullPath & "\thor_fin_backup") Then
            IO.Directory.CreateDirectory(FullPath & "\thor_fin_backup")
        End If

        NotShadedInstances.Clear()


        Dim J As New FileStream(FullPath & dirName & ".fin", FileMode.Open)
        Dim X As New BinaryReader(J)

        Dim FinCount = X.ReadInt32


        ReDim Instances(FinCount - 1)
        Dim i&, k&
        For i = 0 To FinCount - 1

            Instances(i) = New FILE_INSTANCE



            Instances(i).Name = X.ReadChars(9)

            Instances(i).Name(8) = Chr(0) 'Force EOS




            Instances(i).R = ChrW(X.ReadByte())
            Instances(i).G = ChrW(X.ReadByte)
            Instances(i).B = ChrW(X.ReadByte)

            Instances(i).EnvRGB = X.ReadInt32
            Instances(i).Priority = X.ReadBoolean
            Instances(i).Flag = X.ReadByte
            Instances(i).Pad(0) = X.ReadByte
            Instances(i).Pad(1) = X.ReadByte
            Instances(i).LodBias = X.ReadSingle
            Instances(i).WorldPos = New Vector3D(X.ReadSingle(), X.ReadSingle(), X.ReadSingle())

            ' Instances(i).WorldMatrix = New Matrix4
            Dim m(8) As Single
            For k = 0 To 8

                m(k) = X.ReadSingle

                ''If k Mod 3 = 1 Then m(k) *= -1
                '  If k = 2 Then m(k) *= -1
                '  If k = 6 Then m(k) *= -1
                ' If k = 8 Then m(k) *= -1



            Next
            '  Instances(i).WorldMatrix.RotationDegrees += New Vector3D(180, 0, 0)

            'and loading fullpath of prm
            For p = 0 To 8
                If Instances(i).Name(p) = Chr(0) Then Instances(i).Name(p) = "*"

            Next

            'get fullpath
            Instances(i).fullPath = IO.Directory.GetFiles(FullPath, "*.prm").Intersect(IO.Directory.GetFiles(FullPath, Instances(i).Name))(0)


            'backup first
            If IO.File.Exists(FullPath & "\thor_fin_backup\" & Split(Instances(i).fullPath, "\").Last) = False Then
                IO.File.Copy(Instances(i).fullPath, FullPath & "\thor_fin_backup\" & Split(Instances(i).fullPath, "\").Last)
            End If

            'get fullpath (in backup)
            Instances(i).fullPath = IO.Directory.GetFiles(FullPath & "\thor_fin_backup", "*.prm").Intersect(IO.Directory.GetFiles(FullPath & "\thor_fin_backup", Instances(i).Name))(0)

            'add to non-shaded instances
            If Not NotShadedInstances.Contains(Split(Instances(i).fullPath, "\").Last) Then
                NotShadedInstances.Add(Split(Instances(i).fullPath, "\").Last)
            End If


        Next


        J.Close()
        J = Nothing






    End Sub
End Module
