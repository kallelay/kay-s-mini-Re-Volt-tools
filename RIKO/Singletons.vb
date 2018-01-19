Public Class Singletons
    Private Shared newStr As String
    Sub New(ByVal FilePath As String)
        If Not IO.File.Exists(FilePath) Then
            MsgBox("Error:" & FilePath & " couldn't be found")
            Me.Finalize()


        End If
        newStr = IO.File.ReadAllText(FilePath)
        Clean()
    End Sub
    Sub Clean()
        Dim temp = Split(newStr, vbNewLine)
        Dim CleanStr As String = ""
        For i = 0 To UBound(temp)
            If InStr(temp(i), ";") > 0 Then
                If temp(i).Length > 1 Then
                    If temp(i)(1) = ")" Then
                        CleanStr &= temp(i) & vbNewLine
                        Continue For
                    End If
                End If


                CleanStr &= Split(temp(i), ";")(0) & vbNewLine
            Else

                CleanStr &= temp(i) & vbNewLine
            End If
        Next

        Do Until InStr(CleanStr, vbNewLine & vbNewLine) < 1
            CleanStr = Replace(CleanStr, vbNewLine & vbNewLine, vbNewLine)
        Loop

        newStr = CleanStr
    End Sub
    Public Function getAllSingletons() As String()
        Dim temp() = newStr.Split(vbNewLine)
        Dim header As String = ""
        For i = LBound(temp) To UBound(temp)
            If InStr(temp(i), "{") > 0 Then
                header &= Replace(Replace(Split(temp(i), "{")(0), " " & vbTab, ""), vbTab, "") & ","
            End If
        Next

        Return header.Split(",")
    End Function
    Sub writeHeader(ByRef w As IO.StreamWriter, ByVal Name$)
        w.WriteLine()
        w.WriteLine(";====================")
        w.WriteLine("; {0}", Name)
        w.WriteLine(";====================")
        w.WriteLine()
    End Sub

    Public Function getSingleton(ByVal header) As SingletonItem
        If InStr(newStr, header) < 1 Then
            Return SingletonItem.Null
        End If


        Dim temp As String = ""
        If header = "" Or header = " " Then
            temp = Split(newStr, "{")(1)

        Else
            temp = Split(Split(newStr, header)(1), "{")(1)
        End If

        If InStr(Split(temp, "}")(0), "{") < 1 Then
            'lucky us!
            Dim l = Split(temp, "}")(0)
            Dim torep = Split(l, vbNewLine).Last

            Return SingletonItem.ToSingletonItem(Replace(l, torep, ""))


        Else
            'how unlucky...
            Dim tmp As String = temp
            Do Until InStr(tmp, "{") = 0
                Dim splt = Split(Split(Split(tmp, "{")(0), vbNewLine)(1), "}")(0)
                tmp = Replace(tmp, splt, "")
            Loop
            Return SingletonItem.ToSingletonItem(tmp)
        End If
        Return SingletonItem.Null
    End Function

End Class
Public Class SingletonItem
    Private Shared items() As String
    Public Shared Null = Nothing

    Public Shared Function ToSingletonItem(ByVal str As String) As SingletonItem
        Dim nSing As New SingletonItem
        Dim splitted() = Split(str, vbNewLine)
        ReDim items(splitted.Length)
        SingletonItem.items = splitted
        SingletonItem.items = splitted
        Return nSing
    End Function

    Public Function getValue(ByVal key)



        For i = LBound(items) To UBound(items)

            If InStr(items(i), key) > 0 Then
                Dim tmp = Replace(Split(items(i), key)(1), " " & vbTab, "") ', ".", ",")


                If IO.File.Exists(RvPath & "\" & Replace(tmp, """", "")) Then Return Replace(tmp, """", "")

                If InStr(CSng(2.15), ",") <> 0 Then
                    Dim cnt As Integer, sp As Integer
                    For j = 0 To tmp.Length - 1
                        If CChar(tmp(j)) >= "0" And CChar(tmp(j)) <= "9" Then cnt += 1
                        If CChar(tmp(j)) = " " Or CChar(tmp(j)) = vbTab Then sp += 1
                    Next

                    If cnt / (tmp.Length - sp) * 100 > 40 Then
                        tmp = Replace(tmp, ".", ",")
                    End If

                    Return tmp

                End If

                Return tmp

            End If
        Next
        Return Nothing
    End Function
    Public Function get3LinesValue(ByVal key)

        For i = LBound(items) To UBound(items)

            If InStr(items(i), key) > 0 Then
                Dim tmp = Replace(Split(items(i), key)(1), " " & vbTab, "") & vbNewLine  ', ".", ",")

                tmp &= Replace(items(i + 1), " " & vbTab, "") & vbNewLine  ', ".", ",")
                tmp &= Replace(items(i + 2), " " & vbTab, "") ', ".", ",")

                If InStr(CSng(2.15), ",") <> 0 Then
                    tmp = Replace(tmp, ".", ",")
                End If

                Return tmp

            End If
        Next
        Return Nothing
    End Function
    Public Function get2LinesValue(ByVal key)

        For i = LBound(items) To UBound(items)

            If InStr(items(i), key) > 0 Then
                Dim tmp = Replace(Split(items(i), key)(1), " " & vbTab, "") & vbNewLine  ', ".", ",") & 

                tmp &= Replace(items(i + 1), " " & vbTab, "") ', ".", ",")

                If InStr(CSng(2.15), ",") <> 0 Then
                    tmp = Replace(tmp, ".", ",")
                End If

                Return tmp

            End If
        Next
        Return Nothing
    End Function
    Public Function getAllKeys()
        Dim allVal(items.Length) As String
        For i = LBound(items) To UBound(items)
            If InStr(items(i), " " & vbTab) > 0 Then
                allVal(i) = Split(items(i), " " & vbTab)(0)
            Else
                allVal(i) = Split(items(i), vbTab)(0)
            End If



        Next
        Return allVal
    End Function
    Public Sub setValue(ByVal key As String, ByVal value As String)
        For i = LBound(items) To UBound(items)

            If InStr(items(i), key) > 0 Then

                items(i) = Replace(items(i), Split(items(i), key)(1), value)

                Exit Sub
            End If
        Next
        Dim nItems(items.Length + 1)

        For i = LBound(items) To UBound(items)
            nItems(i) = items(i)
        Next

        ReDim items(items.Length + 1)

        For i = LBound(items) To UBound(items) - 1
            items(i) = nItems(i)
        Next
        items(UBound(items)) = key & " " & vbTab & value

    End Sub
    Public Function GetEverything() As String()
        Return items
    End Function


End Class