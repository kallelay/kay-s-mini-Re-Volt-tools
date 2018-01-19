Public Class readme_generator

    Private Sub readme_generator_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If MsgBox("Sure?", MsgBoxStyle.YesNo, "Close this?") = MsgBoxResult.Yes Then e.Cancel = False Else e.Cancel = True

    End Sub

    Private Sub readme_generator_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Sub UpdateThis()

        'now read me...
        tname.Text = INF.NAME
        TextBox1.Text = Form1.trlen.Text
        TextBox16.Text = Today.ToShortDateString

        TextBox5.Text = dirName
        Dim pics = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_PICKUP).Count
        TextBox6.Text = If(pics = 0, "No", "Yes (" & pics & " pickups)")

        pics = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_SKYBOX).Count
        TextBox7.Text = If(pics = 0, "No", "Yes")

        TextBox11.Text = Format(curWorld.PolyCount, "# ###")
        TextBox9.Text = Format(NCP.ListOfNCP.Count, "# ###")
        TextBox8.Text = Format(curWorld.meshCount, "# ###")

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Panel1.Enabled = CheckBox1.Checked
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub ListBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.DoubleClick, Button5.Click
        If ListBox1.SelectedIndex = -1 Then Exit Sub
        ListBox2.Items.Add(ListBox1.SelectedItem)
        ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub

    Private Sub ListBox2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox2.DoubleClick, Button6.Click
        If ListBox2.SelectedIndex = -1 Then Exit Sub
        ListBox1.Items.Add(ListBox2.SelectedItem)
        ListBox2.Items.RemoveAt(ListBox2.SelectedIndex)
    End Sub



    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        ListBox2.Items.Add(TextBox13.Text)
    End Sub

    Function GenerateOneLine(ByVal Key$, ByVal value$) As String
        If RadioButton1.Checked Then
            '----------------------- car::load-----------------
            Dim n$ = "+ "
            n$ &= Key$

            n &= Space(16 - n.Length)

            n &= ": " & value$
            If n.Length > 80 Then n = n.Substring(0, 80)

            n &= Space(79 - n.Length) & "+"
            Return n & vbNewLine

        ElseIf RadioButton2.Checked Then
            '-----------------------std----------------------
            Dim n$ = " "
            n$ &= Key$

            n &= Space(16 - n.Length)

            n &= ": " & value$
            If n.Length > 80 Then n = n.Substring(0, 80)

            n &= Space(79 - n.Length) & " "
            Return n & vbNewLine
        ElseIf RadioButton3.Checked Then
            '-----------------------kay----------------------
            Dim n$ = "   "
            n$ &= Key$ & ":"

            n &= Space(19 - n.Length)

            n &= Space(1) & value$
            If n.Length > 80 Then n = n.Substring(0, 80)

            n &= Space(79 - n.Length) & " " & vbNewLine '& Space(5) & chRp("¨", Len(Key))
            Return n & vbNewLine

        ElseIf RadioButton4.Checked Then
            '-----------------------rvl----------------------
            Dim n$ = ""
            n$ &= Space(5) & Key$ & ":"

            n &= Space(21 - n.Length)

            n &= value$
            If n.Length > 80 Then n = n.Substring(0, 80)

            n &= Space(79 - n.Length) & " " & vbNewLine & Space(5) & chRp("¨", Len(Key))
            Return n & vbNewLine
        ElseIf RadioButton5.Checked Then
            '-----------------------dyspro----------------------
            Dim n$ = ""
            n$ &= Space(5) & Key$ & ":"

            n &= Space(21 - n.Length)

            n &= value$
            If n.Length > 80 Then n = n.Substring(0, 80)

            n &= Space(79 - n.Length) & " " ' & Space(5) & chRp("¨", Len(Key))
            Return n & vbNewLine

        ElseIf RadioButton6.Checked Then
            '-----------------------std----------------------
            Dim n$ = ""
            n$ &= Space(3) & Key$ & ":"

            n &= Space(21 - n.Length)

            n &= value$
            If n.Length > 80 Then n = n.Substring(0, 80)

            n &= Space(79 - n.Length) & " " ' & Space(5) & chRp("¨", Len(Key))
            Return n & vbNewLine

        End If

    End Function
    Public Function chRp(ByVal ch$, ByVal len%)
        chRp = ""
        For i = 0 To len - 1
            chRp &= ch
        Next
    End Function

    Function GenerateSpecial(ByVal T As typu, ByVal value$) As String
        If RadioButton1.Checked Then
            '----------------------- car::load-----------------
            If T = typu.blank Then Return vbNewLine
            If T = typu.line Then Return "+-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-+" & vbNewLine
            If T = typu.empty Then Return "+" & Space(78) & "+" & vbNewLine
            Dim tit = value
            If tit.Length > 20 Then tit = tit.Substring(0, 20)
            tit = " " & tit
            tit = tit & Space(20 - tit.Length + 1)
            If T = typu.title Then Return "+   ______________________                                                     +" & vbNewLine & _
                                            "+  [" & tit & ":]                                                    +" & vbNewLine & _
                                            "+   ¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨                                                     +" & vbNewLine
            If T = typu.comment Then
                Return "+ " & value & Space(82 - 6 - value.Length) & " +" & vbNewLine
            End If
            Return ""
        ElseIf RadioButton2.Checked Then
            '-----------------------   std -----------------
            If T = typu.blank Then Return vbNewLine
            If T = typu.line Then Return vbNewLine
            If T = typu.empty Then Return vbNewLine
            Dim tit = value
            If tit.Length > 20 Then tit = tit.Substring(0, 20)
            tit = "" & tit
            tit = tit & Space(20 - tit.Length + 1)
            If T = typu.title Then Return _
                                            "" & tit & "                                                     " & vbNewLine & _
                                           chRp("=", 80) & vbNewLine
            If T = typu.comment Then
                Return " " & value & Space(82 - 6 - value.Length) & "  " & vbNewLine
            End If
            Return ""
        ElseIf RadioButton3.Checked Then
            '-----------------------   std -----------------
            If T = typu.blank Then Return vbNewLine
            If T = typu.line Then Return ""
            If T = typu.empty Then Return vbNewLine
            Dim tit = value
            If tit.Length > 20 Then tit = tit.Substring(0, 20)
            tit = " " & tit
            tit = tit & Space(20 - tit.Length + 1)
            If T = typu.title Then Return "______________________                                                      " & vbNewLine & _
                                            "" & tit & "                                                       " & vbNewLine & _
                                            "¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨                                                      " & vbNewLine
            If T = typu.comment Then
                Return "  " & value & Space(82 - 6 - value.Length) & "  " & vbNewLine
            End If
            Return ""

        ElseIf RadioButton4.Checked Then
            '-----------------------   rvl -----------------
            If T = typu.blank Then Return vbNewLine
            If T = typu.line Then Return vbNewLine
            If T = typu.empty Then Return vbNewLine
            Dim tit = value
            ' If tit.Length > 20 Then tit = tit.Substring(0, 20)
            ' tit = " " & tit
            ' tit = tit & Space(20 - tit.Length + 1)
            If T = typu.title Then Return " " & chRp("_", Len(tit)) & vbNewLine & _
                                            "(" & tit & ")                                                       " & vbNewLine & _
                                            " " & chRp("¨", Len(tit)) & vbNewLine
            If T = typu.comment Then
                Return "  " & value & Space(82 - 6 - value.Length) & "  " & vbNewLine
            End If
            Return ""

        ElseIf RadioButton5.Checked Then
            '-----------------------   dyspro -----------------
            If T = typu.blank Then Return vbNewLine
            If T = typu.line Then Return vbNewLine
            If T = typu.empty Then Return vbNewLine
            Dim tit = value
            ' If tit.Length > 20 Then tit = tit.Substring(0, 20)
            ' tit = " " & tit
            ' tit = tit & Space(20 - tit.Length + 1)
            If T = typu.title Then Return " " & vbNewLine & _
                                            " " & tit & "                                                        " & vbNewLine & _
                                            " " & chRp("-", Len(tit)) & vbNewLine & vbNewLine
            If T = typu.comment Then
                Return "  " & value & Space(82 - 6 - value.Length) & "  " & vbNewLine
            End If
            Return ""
        ElseIf RadioButton6.Checked Then
            '-----------------------   dyspro -----------------
            If T = typu.blank Then Return vbNewLine
            If T = typu.line Then Return vbNewLine
            If T = typu.empty Then Return vbNewLine
            Dim tit = value
            ' If tit.Length > 20 Then tit = tit.Substring(0, 20)
            ' tit = " " & tit
            ' tit = tit & Space(20 - tit.Length + 1)
            If T = typu.title Then Return "" & chRp("=", Len(tit) + 2) & vbNewLine & _
                                            " " & tit & "                                                        " & vbNewLine & _
                                            "" & chRp("=", Len(tit) + 2) & vbNewLine & vbNewLine
            If T = typu.comment Then
                Return " " & value & Space(82 - 6 - value.Length) & "  " & vbNewLine
            End If
            Return ""
        End If

    End Function
    Function TreatTextBox(ByVal textbox As TextBox) As String
        Dim c = textbox.Text
        Dim buffer$, processed$, final$
        final = ""
        processed = ""
        For i = 0 To c.Split(CChar(vbNewLine)).Length - 1

            buffer = " " & c.Split(CChar(vbNewLine))(i)


            Do While buffer.Length > 78
                processed &= buffer.Substring(0, buffer.Substring(0, 78).LastIndexOf(" ")) & Space(78 - buffer.Substring(0, 78).LastIndexOf(" ")) & vbNewLine
                buffer = buffer.Substring(buffer.Substring(0, 78).LastIndexOf(" "))
                Application.DoEvents()
            Loop
            processed &= buffer & Space(78 - Len(buffer)) & vbNewLine


        Next
        If RadioButton1.Checked Then : processed = "+" & Replace(processed, vbNewLine, "+ " & vbNewLine & "+") & vbNewLine
        Else : processed = " " & Replace(processed, vbNewLine, "  " & vbNewLine & " ") & vbNewLine
        End If

        processed = Mid(processed, 1, Len(processed) - 3)
        Return processed

    End Function
    Function GenerateSpecial(ByVal T As typu) As String
        Return GenerateSpecial(T, "")
    End Function

    Function GenerateCopy() As String
        Return "._____________________________________________________." & vbNewLine & _
                "|                           R E K C A P               |" & vbNewLine & _
                "¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨" & vbNewLine & _
        Now.ToLongDateString & ", " & Now.ToLongTimeString

    End Function
    Enum typu
        line
        empty
        blank
        title
        comment
    End Enum
    Function Generate_Readme() As String
        Dim FULL$ = GenerateSpecial(typu.line)
        Dim Capname$ = tname.Text.ToUpper
        For i = 0 To 82 - 6 - Len(tname.Text)
            Capname = " " & Capname
        Next
        If RadioButton1.Checked Then
            Capname = "+" & Capname
            Capname &= " +" & vbNewLine
        ElseIf RadioButton2.Checked Or RadioButton3.Checked Then
            Capname = " " & Capname
            Capname &= "  " & vbNewLine

        End If

        FULL &= Capname
        FULL &= GenerateSpecial(typu.line)

        FULL &= GenerateSpecial(typu.title, "Main Infos")
        FULL &= GenerateOneLine("Name", tname.Text)
        FULL &= GenerateOneLine("made by", TextBox4.Text)
        FULL &= GenerateOneLine("Type", ComboBox1.Text)
        FULL &= GenerateOneLine("Length", TextBox1.Text)
        FULL &= GenerateOneLine("Date finished", TextBox16.Text)
        FULL &= GenerateSpecial(typu.empty)
        FULL &= GenerateSpecial(typu.line)

        If TextBox3.Text <> "" Or CheckBox1.Checked Then
            FULL &= GenerateSpecial(typu.title, "Theme")
            If TextBox3.Text <> "" Then FULL &= GenerateOneLine("Main Theme", TextBox3.Text)
            If CheckBox1.Checked Then FULL &= GenerateOneLine("Weather", ComboBox2.Text & " " & ComboBox3.Text)
            FULL &= GenerateSpecial(typu.empty)
            FULL &= GenerateSpecial(typu.line)
        End If

        FULL &= GenerateSpecial(typu.title, "Technicals")
        FULL &= GenerateOneLine("Folder", TextBox5.Text)
        FULL &= GenerateOneLine("Pickups", TextBox6.Text)
        FULL &= GenerateOneLine("Skybox", TextBox7.Text)
        FULL &= GenerateOneLine("W Poly count", TextBox11.Text)
        FULL &= GenerateOneLine("NCP Poly count", TextBox9.Text)
        FULL &= GenerateOneLine("Mesh count", TextBox8.Text)
        FULL &= GenerateSpecial(typu.comment, Space(3) & "~polys: polygons")
        FULL &= GenerateSpecial(typu.empty)
        FULL &= GenerateSpecial(typu.line)

        If ListBox2.Items.Count > 0 Then
            FULL &= GenerateSpecial(typu.title, "Tools")
            For Each item In ListBox2.Items
                FULL &= GenerateSpecial(typu.comment, Space(1) & "- " & item)
            Next
            FULL &= GenerateSpecial(typu.empty)
            FULL &= GenerateSpecial(typu.line)
        End If

        If TextBox2.Text.Length > 2 Then
            FULL &= GenerateSpecial(typu.title, "Description")
            FULL &= TreatTextBox(TextBox2)
            FULL &= GenerateSpecial(typu.empty)
            FULL &= GenerateSpecial(typu.line)
        End If

        If TextBox10.Text.Length > 2 Then
            FULL &= GenerateSpecial(typu.title, "Notes")
            FULL &= TreatTextBox(TextBox10)
            FULL &= GenerateSpecial(typu.empty)
            FULL &= GenerateSpecial(typu.line)
        End If

        If TextBox12.Text.Length > 2 Then
            FULL &= GenerateSpecial(typu.title, "Credits")
            FULL &= TreatTextBox(TextBox12)
            FULL &= GenerateSpecial(typu.empty)
            FULL &= GenerateSpecial(typu.line)
        End If

        If TextBox17.Text.Length > 2 Then
            FULL &= GenerateSpecial(typu.title, "Thanks")
            FULL &= TreatTextBox(TextBox17)
            FULL &= GenerateSpecial(typu.empty)
            FULL &= GenerateSpecial(typu.line)
        End If


        If TextBox18.Text.Length > 2 Then
            FULL &= GenerateSpecial(typu.title, "Copyrights")
            FULL &= TreatTextBox(TextBox18)
            FULL &= GenerateSpecial(typu.empty)
            FULL &= GenerateSpecial(typu.line)
        End If

        FULL &= GenerateSpecial(typu.title, "Contact")
        FULL &= GenerateOneLine("E-mail", TextBox15.Text)
        FULL &= GenerateOneLine("Site", TextBox14.Text)
        FULL &= GenerateSpecial(typu.empty)
        FULL &= GenerateSpecial(typu.line)


        FULL &= vbNewLine
        If CheckBox2.Checked Then FULL &= GenerateCopy()
        Return FULL
    End Function

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        textviewer.TextBox1.Text = Generate_Readme()
        textviewer.Show()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try : My.Computer.FileSystem.DeleteFile(RvDir & "\levels\" & dirName & "\readme.txt", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin) : Catch : End Try
        IO.File.WriteAllText(RvDir & "\levels\" & dirName & "\readme.txt", Generate_Readme())

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        My.Computer.Clipboard.SetText(Generate_Readme())
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If MsgBox("Sure?", MsgBoxStyle.YesNo, "Close this?") = MsgBoxResult.Yes Then Me.Close()

    End Sub
End Class