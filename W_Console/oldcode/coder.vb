Imports System.IO

Module FileEncoder



    Sub Encoder(ByVal filepath As String)
        Randomize()

        DoWrite("Encoding file in progress...")
        Dim X As New BinaryWriter(New FileStream(Left(filepath, Len(filepath) - 4), FileMode.OpenOrCreate))
        Dim textbox1 As IO.StreamReader
        textbox1 = IO.File.OpenText(filepath)


        ' Dim Arr = Split(textbox1, vbNewLine)
        'Dim Len = Arr.Length - 1
        Do Until textbox1.EndOfStream
            Dim Mystr As String = textbox1.ReadLine()


            If Mystr.Length < 3 Then GoTo nexty
            If Mystr(0) = "#" Then GoTo nexty


            If InStr(Mystr, "rvlong", CompareMethod.Text) Then
                X.Write(Convert.ToInt32(Replace(Mystr, "rvlong ", "", 1, -1, CompareMethod.Text)))
            ElseIf InStr(Mystr, "rvulong", CompareMethod.Text) Then
                X.Write(Convert.ToUInt32(Replace(Mystr, "rvulong ", "", 1, -1, CompareMethod.Text)))
            ElseIf InStr(Mystr, "rvshort", CompareMethod.Text) Then
                X.Write(Convert.ToInt16(Replace(Mystr, "rvshort ", "", 1, -1, CompareMethod.Text)))
            ElseIf InStr(Mystr, "rvushort", CompareMethod.Text) Then
                X.Write(Convert.ToUInt16(Replace(Mystr, "rvushort ", "", 1, -1, CompareMethod.Text)))
            ElseIf InStr(Mystr, "rvfloat", CompareMethod.Text) Then
                X.Write(Convert.ToSingle(Replace(Mystr, "rvfloat ", "", 1, -1, CompareMethod.Text)))
            ElseIf InStr(Mystr, "rvstring", CompareMethod.Text) Then
                X.Write(Convert.ToString(Replace(Mystr, "rvstring ", "", 1, -1, CompareMethod.Text)))
            ElseIf InStr(Mystr, "rvchar", CompareMethod.Text) Then
                X.Write(Convert.ToChar(Replace(Mystr, "rvchar ", "", 1, -1, CompareMethod.Text)))
            ElseIf InStr(Mystr, "rvbyte", CompareMethod.Text) Then
                X.Write(Convert.ToByte(Replace(Mystr, "rvbyte ", "", 1, -1, CompareMethod.Text)))
            ElseIf InStr(Mystr, "rvdec", CompareMethod.Text) Then
                X.Write(Convert.ToDecimal(Replace(Mystr, "rvdec ", "", 1, -1, CompareMethod.Text)))
            ElseIf InStr(Mystr, "rvdword", CompareMethod.Text) Then
                X.Write(Convert.ToDouble(Replace(Mystr, "rvdword ", "", 1, -1, CompareMethod.Text)))
            End If
nexty:

        Loop
        DoWrite("finished!")
        X.Close()
    End Sub

End Module
