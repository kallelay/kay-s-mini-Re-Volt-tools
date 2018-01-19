Module ncp_file_module
    Class ncp_file
        Public FullPath$ = ""

        Public ListOfNCP As New List(Of NEWCOLLPOLY)

        Sub New(ByVal fp$)
            On Error Resume Next
            FullPath = fp
            Dim br As New IO.BinaryReader(New IO.FileStream(fp, IO.FileMode.Open, IO.FileAccess.Read))
            For i = 0 To br.ReadUInt16() - 1
                Dim n As New NEWCOLLPOLY With {.Type = br.ReadInt32, .Material = br.ReadInt32, _
                                               .plane = New PLANE(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle())}
                n.edgePlane(0) = New PLANE(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
                n.edgePlane(1) = New PLANE(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
                n.edgePlane(2) = New PLANE(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
                n.edgePlane(3) = New PLANE(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle())

                n.bbox = New BBOX(New Vector3D(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), New Vector3D(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()))

                ListOfNCP.Add(n)
                n = Nothing



            Next
            br.Close()
        End Sub
        Sub save()
            save(Replace(FullPath, "_original", ""))
        End Sub
        Sub Save(ByVal fp$)

            Dim bw As New IO.BinaryWriter(New IO.FileStream(fp, IO.FileMode.Open, IO.FileAccess.Write))
            bw.Write(Convert.ToUInt16(ListOfNCP.Count))
            For i = 0 To ListOfNCP.Count - 1

                bw.Write(Convert.ToInt32(ListOfNCP(i).Type))
                bw.Write(Convert.ToInt32(ListOfNCP(i).Material))
                For j = 0 To 3 : bw.Write(Convert.ToSingle(ListOfNCP(i).plane.v(j))) : Next
                For k = 0 To 3 : For j = 0 To 3 : bw.Write(Convert.ToSingle(ListOfNCP(i).edgePlane(k).v(j))) : Next : Next
                bw.Write(Convert.ToSingle(ListOfNCP(i).bbox.minX))
                bw.Write(Convert.ToSingle(ListOfNCP(i).bbox.minY))
                bw.Write(Convert.ToSingle(ListOfNCP(i).bbox.minZ))

                bw.Write(Convert.ToSingle(ListOfNCP(i).bbox.maxX))
                bw.Write(Convert.ToSingle(ListOfNCP(i).bbox.maxY))
                bw.Write(Convert.ToSingle(ListOfNCP(i).bbox.maxZ))




            Next
            bw.Flush()
            bw.Close()

        End Sub

    End Class


    Class NEWCOLLPOLY
        Public Type As Int32
        Public Material As MATERIAL_NTYPES
        Public plane As PLANE
        Public edgePlane(3) As PLANE
        Public bbox As BBOX


    End Class
    Enum MATERIAL_NTYPES As Long
        MATERIAL_NONE = -1
        MATERIAL_DEFAULT = 0
        MATERIAL_MARBLE
        MATERIAL_STONE
        MATERIAL_WOOD
        MATERIAL_SAND
        MATERIAL_PLASTIC
        MATERIAL_CARPETTILE
        MATERIAL_CARPETSHAG
        MATERIAL_BOUNDARY
        MATERIAL_GLASS
        MATERIAL_ICE1
        MATERIAL_METAL
        MATERIAL_GRASS
        MATERIAL_BUMPMETAL
        MATERIAL_PEBBLES
        MATERIAL_GRAVEL
        MATERIAL_CONVEYOR1
        MATERIAL_CONVEYOR2
        MATERIAL_DIRT1
        MATERIAL_DIRT2
        MATERIAL_DIRT3
        MATERIAL_ICE2
        MATERIAL_ICE3
        MATERIAL_WOOD2
        MATERIAL_CONVEYOR_MARKET1
        MATERIAL_CONVEYOR_MARKET2
        MATERIAL_PAVING
    End Enum
    Class PLANE
        Public v(3) As Single
        Sub New(ByVal v0!, ByVal v1!, ByVal v2!, ByVal v3!)
            v = New Single() {v0, v1, v2, v3}
        End Sub
    End Class
End Module
