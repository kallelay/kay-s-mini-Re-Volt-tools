Module GlobalModule


    Sub DoWrite(ByVal Str As String)
        Form1.TextBox3.AppendText(">> " & Str & vbNewLine)
    End Sub
    Function tex(ByVal path$) As Integer
        Dim model As New Car_Model(path)
        Dim n As Integer
        Dim cnt(31) As String
        For n = 0 To model.MyModel.polynum - 1
            cnt(model.MyModel.polyl(n).Tpage + 1) += 1
        Next
        Dim Max As Integer, TfP As Integer
        max = 0
        For n = 1 To 31
            If max < cnt(n) Then
                max = cnt(n)
                tfp = n - 1
            End If
        Next
        Return TfP

    End Function

End Module
