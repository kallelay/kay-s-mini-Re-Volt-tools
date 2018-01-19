Imports System.Drawing
Module Shaders
    'imported from rvtmod's re-volt gl lib
    'HELPERS
    Sub ShadeF(ByRef col As Color, ByVal sr!, ByVal sg!, ByVal sb!)
        If MyColorMode = 0 Then
            col = Color.FromArgb(col.A, col.R * sr, col.G * sg, col.B * sb)
        ElseIf MyColorMode = 1 Then
            col = Color.FromArgb(col.A, tt(255 * sr + col.R), tt(255 * sg + col.G), tt(255 * sb + col.G))
        ElseIf MyColorMode = 2 Then
            col = Color.FromArgb(col.A, tt(-255 * sr + col.R), tt(-255 * sg + col.G), tt(-255 * sb + col.G))
        ElseIf MyColorMode = 3 Then
            col = Color.FromArgb(col.A, tt(128 * sr + col.R / 2), tt(128 * sg + col.G / 2), tt(128 * sb + col.G / 2))

        ElseIf MyColorMode = 4 Then
            Dim g As Integer = (col.R / 3.0 + col.G / 3.0 + col.B / 3.0)
            col = Color.FromArgb(col.A, tt(g * sr), tt(g * sg), tt(g * sb))

        ElseIf MyColorMode = 5 Then
            Dim g As Integer = (col.R / 3.0 + col.G / 3.0 + col.B / 3.0) / 2.0
            col = Color.FromArgb(col.A, tt(g * sr + 128 * sr), tt(g * sg + 128 * sg), tt(g * sb + 128 * sb))
        End If

    End Sub
    Function tt(ByVal inte%)
        Return If(inte > 255, 255, If(inte < 0, 0, inte))
    End Function

    Sub ShadeAdd(ByRef col As Color, ByVal r%, ByVal g%, ByVal b%)
        col = Color.FromArgb(col.A, col.R + r, col.G + g, col.B + b)
    End Sub



    'Shade by multiplication
    Sub ShadeMulForbakedRV(ByRef col As UInt32, ByRef ShadeColor As Color)
        Dim c As Color = UintToColor(col)
        ShadeF(c, ShadeColor.R / 255.0, ShadeColor.G / 255.0, ShadeColor.B / 255.0)
        col = ColorToUint(c)
        c = Nothing

    End Sub

    'Flat shade
    Sub ShadeReplaceForbakedRV(ByRef col As UInt32, ByRef ShadeColor As Color)
        Dim c As Color = Color.White
        ShadeF(c, ShadeColor.R / 255.0, ShadeColor.G / 255.0, ShadeColor.B / 255.0)
        col = ColorToUint(c)
        c = Nothing

    End Sub

    'Shade from Normal vertex
    Sub ShadeFromNormalForBakedRV(ByRef col As UInt32, ByRef vertex As MODEL_VERTEX_MORPH, ByRef ShadeColor As Color, ByRef lightDir As Vector3D, ByRef lightOffset As Vector3D)
        Dim gray = ((lightOffset.y - vertex.normal.y * lightDir.y) / 2.0) ^ 2
        gray += ((lightOffset.x - vertex.normal.x * lightDir.x) / 2.0) ^ 2
        gray += ((lightOffset.z - vertex.normal.z * lightDir.z) / 2.0) ^ 2
        gray = Math.Sqrt(gray) * 255
        gray = Math.Max(Math.Min(gray, 255), 0)

        If Double.IsInfinity(gray) Or Double.IsNaN(gray) Then Return
        Dim c As Color = Color.FromArgb(UintToColor(col).A, gray, gray, gray)
        ShadeF(c, ShadeColor.R / 255.0, ShadeColor.G / 255.0, ShadeColor.B / 255.0)
        col = ColorToUint(c)
        c = Nothing
        gray = Nothing
    End Sub

    'Shade from Normal vertex
    Sub ShadeFromNormalForBakedRVAdd(ByRef col As UInt32, ByRef vertex As MODEL_VERTEX_MORPH, ByRef ShadeColor As Color, ByRef lightDir As Vector3D, ByRef lightOffset As Vector3D)
        Dim gray = ((lightOffset.y - vertex.normal.y * lightDir.y) / 2.0) ^ 2
        gray += ((lightOffset.x - vertex.normal.x * lightDir.x) / 2.0) ^ 2
        gray += ((lightOffset.z - vertex.normal.z * lightDir.z) / 2.0) ^ 2
        gray = Math.Sqrt(gray) * 255
        gray = Math.Max(Math.Min(gray, 255), 0)


        Dim c As Color = UintToColor(col)
        ShadeF(c, ShadeColor.R / 255.0, ShadeColor.G / 255.0, ShadeColor.B / 255.0)
        col = ColorToUint(c)
        c = Nothing
        gray = Nothing
    End Sub


    Enum shadingMode
        multiply = 0
        Blank = 1
        ColorByvertex = 2
        ColorByPoly = 3
        AddColorByvertex = 4
        AddColorByPoly = 5
    End Enum

    '-------------- functions ------------------------
    Sub ShadeWorld(ByRef w As WORLD, ByRef color As Color, ByVal Mode As shadingMode)

        Select Case Mode
            Case shadingMode.multiply 'Shade by multiply
                For i = 0 To w.meshCount - 1
                    For j = 0 To w.mMesh(i).polynum - 1
                        ShadeMulForbakedRV(w.mMesh(i).polyl(j).c0, color)
                        ShadeMulForbakedRV(w.mMesh(i).polyl(j).c1, color)
                        ShadeMulForbakedRV(w.mMesh(i).polyl(j).c2, color)
                        ShadeMulForbakedRV(w.mMesh(i).polyl(j).c3, color)
                    Next
                Next


            Case shadingMode.Blank 'Shade Flat
                For i = 0 To w.meshCount - 1
                    For j = 0 To w.mMesh(i).polynum - 1

                        ShadeReplaceForbakedRV(w.mMesh(i).polyl(j).c0, color)
                        ShadeReplaceForbakedRV(w.mMesh(i).polyl(j).c1, color)
                        ShadeReplaceForbakedRV(w.mMesh(i).polyl(j).c2, color)
                        ShadeReplaceForbakedRV(w.mMesh(i).polyl(j).c3, color)

                    Next

                Next


            Case shadingMode.ColorByvertex 'Shade assign vertexNormal
                For i = 0 To w.meshCount - 1
                    For j = 0 To w.mMesh(i).polynum - 1

                        ShadeFromNormalForBakedRV(w.mMesh(i).polyl(j).c0, w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi0), color, ldir, loff)
                        ShadeFromNormalForBakedRV(w.mMesh(i).polyl(j).c1, w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi1), color, ldir, loff)
                        ShadeFromNormalForBakedRV(w.mMesh(i).polyl(j).c2, w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi2), color, ldir, loff)
                        If w.mMesh(i).polyl(j).type And 1 Then
                            ShadeFromNormalForBakedRV(w.mMesh(i).polyl(j).c3, w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi3), color, ldir, loff)
                        End If


                    Next

                Next


            Case shadingMode.ColorByPoly 'Shade assign vertexNormal
                Dim VirtualVertex As New MODEL_VERTEX_MORPH
                For i = 0 To w.meshCount - 1
                    For j = 0 To w.mMesh(i).polynum - 1
                        VirtualVertex = w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi0)
                        VirtualVertex.normal += w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi1).normal
                        VirtualVertex.normal += w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi2).normal
                        If w.mMesh(i).polyl(j).vi3 < w.mMesh(i).vertnum And w.mMesh(i).polyl(j).vi3 > 0 Then
                            VirtualVertex.normal += w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi3).normal
                            VirtualVertex.normal /= 4.0
                        Else

                            VirtualVertex.normal /= 3.0
                        End If




                        ShadeFromNormalForBakedRV(w.mMesh(i).polyl(j).c0, VirtualVertex, color, ldir, loff)
                        ShadeFromNormalForBakedRV(w.mMesh(i).polyl(j).c1, VirtualVertex, color, ldir, loff)
                        ShadeFromNormalForBakedRV(w.mMesh(i).polyl(j).c2, VirtualVertex, color, ldir, loff)
                        Try

                            ShadeFromNormalForBakedRV(w.mMesh(i).polyl(j).c3, VirtualVertex, color, ldir, loff)
                        Finally

                        End Try
                    Next

                Next
            Case shadingMode.AddColorByvertex 'Shade assign vertexNormal
                For i = 0 To w.meshCount - 1
                    For j = 0 To w.mMesh(i).polynum - 1

                        ShadeFromNormalForBakedRVAdd(w.mMesh(i).polyl(j).c0, w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi0), color, ldir, loff)
                        ShadeFromNormalForBakedRVAdd(w.mMesh(i).polyl(j).c1, w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi1), color, ldir, loff)
                        ShadeFromNormalForBakedRVAdd(w.mMesh(i).polyl(j).c2, w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi2), color, ldir, loff)
                        If w.mMesh(i).polyl(j).type And 1 Then _
                               ShadeFromNormalForBakedRVAdd(w.mMesh(i).polyl(j).c3, w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi3), color, ldir, loff)


                    Next

                Next


            Case shadingMode.AddColorByPoly 'Shade assign vertexNormal
                Dim VirtualVertex As New MODEL_VERTEX_MORPH
                For i = 0 To w.meshCount - 1
                    For j = 0 To w.mMesh(i).polynum - 1
                        VirtualVertex = w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi0)
                        VirtualVertex.normal += w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi1).normal
                        VirtualVertex.normal += w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi2).normal
                        If w.mMesh(i).polyl(j).vi3 < w.mMesh(i).vertnum And w.mMesh(i).polyl(j).vi3 > 0 Then
                            VirtualVertex.normal += w.mMesh(i).vexl(w.mMesh(i).polyl(j).vi3).normal
                            VirtualVertex.normal /= 4.0
                        Else

                            VirtualVertex.normal /= 3.0
                        End If




                        ShadeFromNormalForBakedRVAdd(w.mMesh(i).polyl(j).c0, VirtualVertex, color, ldir, loff)
                        ShadeFromNormalForBakedRVAdd(w.mMesh(i).polyl(j).c1, VirtualVertex, color, ldir, loff)
                        ShadeFromNormalForBakedRVAdd(w.mMesh(i).polyl(j).c2, VirtualVertex, color, ldir, loff)
                        Try

                            ShadeFromNormalForBakedRVAdd(w.mMesh(i).polyl(j).c3, VirtualVertex, color, ldir, loff)
                        Finally

                        End Try
                    Next

                Next
        End Select

    End Sub

    Sub ShadeFIN(ByRef color As Color, ByVal Mode As shadingMode)

        fin_file.Load_FIN()
        Dim model As PRM
        For k = 0 To Instances.Count - 1
            model = New PRM(Instances(k).fullPath)
            Report("Shading instances .... [" & k & "/" & (Instances.Count - 1) & "] .... " & Split(Instances(k).fullPath, "\").Last)

            If NotShadedInstances.Contains(Split(Instances(k).fullPath, "\").Last) Then
                NotShadedInstances.Remove(Split(Instances(k).fullPath, "\").Last)
            Else
                Continue For
            End If

            Select Case Mode
                Case shadingMode.multiply 'Shade by multiply
                    For j = 0 To model.MyModel.polynum - 1

                        ShadeMulForbakedRV(model.MyModel.polyl(j).c0, color)
                        ShadeMulForbakedRV(model.MyModel.polyl(j).c1, color)
                        ShadeMulForbakedRV(model.MyModel.polyl(j).c2, color)
                        ShadeMulForbakedRV(model.MyModel.polyl(j).c3, color)

                    Next


                Case shadingMode.Blank 'Shade Flat
                    For j = 0 To model.MyModel.polynum - 1

                        ShadeReplaceForbakedRV(model.MyModel.polyl(j).c0, color)
                        ShadeReplaceForbakedRV(model.MyModel.polyl(j).c1, color)
                        ShadeReplaceForbakedRV(model.MyModel.polyl(j).c2, color)
                        ShadeReplaceForbakedRV(model.MyModel.polyl(j).c3, color)


                    Next


                Case shadingMode.ColorByvertex 'Shade assign vertexNormal

                    For j = 0 To model.MyModel.polynum - 1

                        ShadeFromNormalForBakedRV(model.MyModel.polyl(j).c0, model.MyModel.vexl(model.MyModel.polyl(j).vi0), color, ldir, loff)
                        ShadeFromNormalForBakedRV(model.MyModel.polyl(j).c1, model.MyModel.vexl(model.MyModel.polyl(j).vi1), color, ldir, loff)
                        ShadeFromNormalForBakedRV(model.MyModel.polyl(j).c2, model.MyModel.vexl(model.MyModel.polyl(j).vi2), color, ldir, loff)
                        If model.MyModel.polyl(j).type And 1 Then
                            ShadeFromNormalForBakedRV(model.MyModel.polyl(j).c3, model.MyModel.vexl(model.MyModel.polyl(j).vi3), color, ldir, loff)
                        End If



                    Next


                Case shadingMode.ColorByPoly 'Shade assign vertexNormal
                    Dim VirtualVertex As New MODEL_VERTEX_MORPH
                    For j = 0 To model.MyModel.polynum - 1
                        VirtualVertex = model.MyModel.vexl(model.MyModel.polyl(j).vi0)
                        VirtualVertex.normal += model.MyModel.vexl(model.MyModel.polyl(j).vi1).normal
                        VirtualVertex.normal += model.MyModel.vexl(model.MyModel.polyl(j).vi2).normal
                        If model.MyModel.polyl(j).vi3 < model.MyModel.vertnum And model.MyModel.polyl(j).vi3 > 0 Then
                            VirtualVertex.normal += model.MyModel.vexl(model.MyModel.polyl(j).vi3).normal
                            VirtualVertex.normal /= 4.0
                        Else

                            VirtualVertex.normal /= 3.0
                        End If




                        ShadeFromNormalForBakedRV(model.MyModel.polyl(j).c0, VirtualVertex, color, ldir, loff)
                        ShadeFromNormalForBakedRV(model.MyModel.polyl(j).c1, VirtualVertex, color, ldir, loff)
                        ShadeFromNormalForBakedRV(model.MyModel.polyl(j).c2, VirtualVertex, color, ldir, loff)
                        Try

                            ShadeFromNormalForBakedRV(model.MyModel.polyl(j).c3, VirtualVertex, color, ldir, loff)
                        Finally

                        End Try

                    Next
                Case shadingMode.AddColorByvertex 'Shade assign vertexNormal
                    For j = 0 To model.MyModel.polynum - 1

                        ShadeFromNormalForBakedRVAdd(model.MyModel.polyl(j).c0, model.MyModel.vexl(model.MyModel.polyl(j).vi0), color, ldir, loff)
                        ShadeFromNormalForBakedRVAdd(model.MyModel.polyl(j).c1, model.MyModel.vexl(model.MyModel.polyl(j).vi1), color, ldir, loff)
                        ShadeFromNormalForBakedRVAdd(model.MyModel.polyl(j).c2, model.MyModel.vexl(model.MyModel.polyl(j).vi2), color, ldir, loff)
                        If model.MyModel.polyl(j).type And 1 Then _
                               ShadeFromNormalForBakedRVAdd(model.MyModel.polyl(j).c3, model.MyModel.vexl(model.MyModel.polyl(j).vi3), color, ldir, loff)


                    Next



                Case shadingMode.AddColorByPoly 'Shade assign vertexNormal
                    Dim VirtualVertex As New MODEL_VERTEX_MORPH
                    For j = 0 To model.MyModel.polynum - 1
                        VirtualVertex = model.MyModel.vexl(model.MyModel.polyl(j).vi0)
                        VirtualVertex.normal += model.MyModel.vexl(model.MyModel.polyl(j).vi1).normal
                        VirtualVertex.normal += model.MyModel.vexl(model.MyModel.polyl(j).vi2).normal
                        If model.MyModel.polyl(j).vi3 < model.MyModel.vertnum And model.MyModel.polyl(j).vi3 > 0 Then
                            VirtualVertex.normal += model.MyModel.vexl(model.MyModel.polyl(j).vi3).normal
                            VirtualVertex.normal /= 4.0
                        Else

                            VirtualVertex.normal /= 3.0
                        End If




                        ShadeFromNormalForBakedRVAdd(model.MyModel.polyl(j).c0, VirtualVertex, color, ldir, loff)
                        ShadeFromNormalForBakedRVAdd(model.MyModel.polyl(j).c1, VirtualVertex, color, ldir, loff)
                        ShadeFromNormalForBakedRVAdd(model.MyModel.polyl(j).c2, VirtualVertex, color, ldir, loff)
                        Try

                            ShadeFromNormalForBakedRVAdd(model.MyModel.polyl(j).c3, VirtualVertex, color, ldir, loff)
                        Finally

                        End Try
                    Next

            End Select

            model.Save()

        Next

    End Sub

  
End Module
