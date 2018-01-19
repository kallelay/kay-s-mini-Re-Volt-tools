Imports System.Drawing
Imports sunshade.Module1
Public Class gui


    Private Sub gui_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Panel2.BackColor = myColor

    End Sub

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged

        myColor = calculateShade(NumericUpDown1.Value)

        Panel2.BackColor = myColor
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            myColor = ColorDialog1.Color
        End If
        Panel2.BackColor = myColor
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        curWorld = Nothing
        curWorld = New WORLD(FullPath & dirName & "_original.w")


        If RadioButton1.Checked Then
            ShadeWorld(curWorld, myColor, shadingMode.multiply)
        ElseIf RadioButton2.Checked Then
            ShadeWorld(curWorld, myColor, shadingMode.Blank)
        ElseIf RadioButton3.Checked Then
            ShadeWorld(curWorld, myColor, shadingMode.ColorByvertex)
        ElseIf RadioButton4.Checked Then
            ShadeWorld(curWorld, myColor, shadingMode.ColorByPoly)
        ElseIf RadioButton5.Checked Then
            ShadeWorld(curWorld, myColor, shadingMode.AddColorByPoly)
        ElseIf RadioButton6.Checked Then
            ShadeWorld(curWorld, myColor, shadingMode.AddColorByPoly)
        End If

        curWorld.Save()

    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub lightDir_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged, NumericUpDown3.ValueChanged, NumericUpDown4.ValueChanged
        ldir = New Vector3D(NumericUpDown2.Value, NumericUpDown3.Value, NumericUpDown4.Value)
    End Sub

    Private Sub lightOff_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown7.ValueChanged, NumericUpDown6.ValueChanged, NumericUpDown5.ValueChanged
        loff = New Vector3D(NumericUpDown7.Value, NumericUpDown6.Value, NumericUpDown5.Value)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ShadeAllBitmaps(myColor)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        If RadioButton1.Checked Then
            ShadeFIN(myColor, shadingMode.multiply)
        ElseIf RadioButton2.Checked Then
            ShadeFIN(myColor, shadingMode.Blank)
        ElseIf RadioButton3.Checked Then
            ShadeFIN(myColor, shadingMode.ColorByvertex)
        ElseIf RadioButton4.Checked Then
            ShadeFIN(myColor, shadingMode.ColorByPoly)
        ElseIf RadioButton5.Checked Then
            ShadeFIN(myColor, shadingMode.AddColorByPoly)
        ElseIf RadioButton6.Checked Then
            ShadeFIN(myColor, shadingMode.AddColorByPoly)
        End If

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Fob = New FOB_File(FullPath & dirName & ".fob")
        Dim skyboxes = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_SKYBOX)
        For i = 0 To skyboxes.Count - 1
            Fob.ObjList.RemoveAt(skyboxes(i))
        Next

        If CheckBox1.Checked Then
            Fob.ObjList.Add(New FILE_OBJECT(OBJ_ID_TYPE.OBJECT_TYPE_SKYBOX, New Integer() {3, 0, 0, 0}, _
                                            INF.STARTPOS - New Vector3D(0, 40, 0), New Vector3D(0, 1, 0), New Vector3D(0, 0, 1)))


        End If

        Fob.Save()

    End Sub
    Dim beingadded = False
    Private Sub NumericUpDown11_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown11.ValueChanged
        While beingadded
            Threading.Thread.Sleep(1)
        End While

        beingadded = True

        Fob = New FOB_File(FullPath & dirName & ".fob")
        Dim rains = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_RAIN)
        For i = 0 To rains.Count - 1
            Try

                Fob.ObjList.RemoveAt(rains(i))
            Catch ex As Exception

            End Try
        Next

        For i = 0 To NumericUpDown11.Value - 1
            Fob.ObjList.Add(New FILE_OBJECT(OBJ_ID_TYPE.OBJECT_TYPE_RAIN, New Integer() {0, 0, 0, 0}, _
                                                    INF.STARTPOS - New Vector3D(20, 80, 20 * i), New Vector3D(0, 1, 0), New Vector3D(0, 0, 1)))


        Next


        Fob.Save()
        beingadded = False

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        NCP = New ncp_file(FullPath & dirName & "_original.ncp")
        For Each item In NCP.ListOfNCP
            item.Material = MATERIAL_NTYPES.MATERIAL_ICE1
        Next
        NCP.save()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        NCP = New ncp_file(FullPath & dirName & "_original.ncp")

        NCP.save()
    End Sub

    Private Sub Label11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label11.Click

    End Sub
End Class