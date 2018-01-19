Module fob_file_module
    Class FOB_File
        Dim fpath$ = ""
        Dim cnt As Int32
        Public ObjList As New List(Of FILE_OBJECT)
        Sub New(ByVal filepath$)
            fpath = filepath
            Dim br As New IO.BinaryReader(New IO.FileStream(filepath, IO.FileMode.Open, IO.FileAccess.Read))
            cnt = br.ReadInt32()
            ObjList.Clear()
            For i = 0 To cnt - 1
                Dim n As New FILE_OBJECT
                n.ID = br.ReadInt32()
                n.Flag(0) = br.ReadInt32()
                n.Flag(1) = br.ReadInt32()
                n.Flag(2) = br.ReadInt32()
                n.Flag(3) = br.ReadInt32()

                n.pos = New Vector3D(br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
                n.up = New Vector3D(br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
                n.look = New Vector3D(br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
                ObjList.Add(n)
                n = Nothing
            Next
            br.Close()

        End Sub

        Function getIDsFor(ByVal objtype As OBJ_ID_TYPE) As List(Of Integer)
            getIDsFor = New List(Of Integer)
            For i = 0 To ObjList.Count - 1
                If ObjList(i).ID = objtype Then getIDsFor.Add(i)
            Next
        End Function

        Sub Save()
            Dim bw As New IO.BinaryWriter(New IO.FileStream(fpath, IO.FileMode.Open, IO.FileAccess.Write))

            bw.Write(Convert.ToInt32(ObjList.Count))
            For i = 0 To ObjList.Count - 1
                Dim n = ObjList(i)
                bw.Write(Convert.ToInt32(n.ID))
                bw.Write(Convert.ToInt32(n.Flag(0)))
                bw.Write(Convert.ToInt32(n.Flag(1)))
                bw.Write(Convert.ToInt32(n.Flag(2)))
                bw.Write(Convert.ToInt32(n.Flag(3)))

                bw.Write(Convert.ToSingle(n.pos.x))
                bw.Write(Convert.ToSingle(n.pos.y))
                bw.Write(Convert.ToSingle(n.pos.z))

                bw.Write(Convert.ToSingle(n.up.x))
                bw.Write(Convert.ToSingle(n.up.y))
                bw.Write(Convert.ToSingle(n.up.z))

                bw.Write(Convert.ToSingle(n.look.x))
                bw.Write(Convert.ToSingle(n.look.y))
                bw.Write(Convert.ToSingle(n.look.z))


                n = Nothing
            Next
            bw.Flush()
            bw.Close()

        End Sub

    End Class

    Class FILE_OBJECT
        Public ID As OBJ_ID_TYPE
        Public Flag(3) As Int32
        Public pos As Vector3D
        Public up As Vector3D
        Public look As Vector3D
        Sub New()

        End Sub
        Sub New(ByVal id As OBJ_ID_TYPE, ByVal flag() As Int32, ByVal pos As Vector3D, ByVal up As Vector3D, ByVal look As Vector3D)
            Me.ID = id
            For i = 0 To 3 : Me.Flag(i) = flag(i) : Next
            Me.pos = pos
            Me.up = up
            Me.look = look
        End Sub


    End Class
    Enum OBJ_ID_TYPE As Long
        OBJECT_TYPE_BARREL
        OBJECT_TYPE_BEACHBALL
        OBJECT_TYPE_PLANET
        OBJECT_TYPE_PLANE
        OBJECT_TYPE_COPTER
        OBJECT_TYPE_DRAGON
        OBJECT_TYPE_WATER
        OBJECT_TYPE_TROLLEY
        OBJECT_TYPE_BOAT
        OBJECT_TYPE_SPEEDUP
        OBJECT_TYPE_RADAR
        OBJECT_TYPE_BALLOON
        OBJECT_TYPE_HORSE
        OBJECT_TYPE_TRAIN
        OBJECT_TYPE_STROBE
        OBJECT_TYPE_FOOTBALL
        OBJECT_TYPE_SPARKGEN
        OBJECT_TYPE_SPACEMAN

        OBJECT_TYPE_SHOCKWAVE
        OBJECT_TYPE_FIREWORK
        OBJECT_TYPE_PUTTYBOMB
        OBJECT_TYPE_WATERBOMB
        OBJECT_TYPE_ELECTROPULSE
        OBJECT_TYPE_OILSLICK
        OBJECT_TYPE_OILSLICK_DROPPER
        OBJECT_TYPE_CHROMEBALL
        OBJECT_TYPE_CLONE
        OBJECT_TYPE_TURBO
        OBJECT_TYPE_ELECTROZAPPED
        OBJECT_TYPE_SPRING
        OBJECT_TYPE_PICKUP
        OBJECT_TYPE_DISSOLVEMODEL
        OBJECT_TYPE_FLAP
        OBJECT_TYPE_LASER
        OBJECT_TYPE_SPLASH
        OBJECT_TYPE_BOMBGLOW
        OBJECT_TYPE_WEEBEL
        OBJECT_TYPE_PROBELOGO
        OBJECT_TYPE_CLOUDS
        OBJECT_TYPE_NAMEWHEEL
        OBJECT_TYPE_SPRINKLER
        OBJECT_TYPE_SPRINKLER_HOSE
        OBJECT_TYPE_OBJECT_THROWER
        OBJECT_TYPE_BASKETBALL
        OBJECT_TYPE_TRACKSCREEN
        OBJECT_TYPE_CLOCK
        OBJECT_TYPE_CARBOX
        OBJECT_TYPE_STREAM
        OBJECT_TYPE_CUP
        OBJECT_TYPE_3DSOUND
        OBJECT_TYPE_STAR
        OBJECT_TYPE_FOX
        OBJECT_TYPE_TUMBLEWEED
        OBJECT_TYPE_SMALLSCREEN
        OBJECT_TYPE_LANTERN
        OBJECT_TYPE_SKYBOX
        OBJECT_TYPE_SLIDER
        OBJECT_TYPE_BOTTLE
        OBJECT_TYPE_BUCKET
        OBJECT_TYPE_CONE
        OBJECT_TYPE_CAN
        OBJECT_TYPE_LILO
        OBJECT_TYPE_GLOBAL
        OBJECT_TYPE_RAIN
        OBJECT_TYPE_LIGHTNING
        OBJECT_TYPE_SHIPLIGHT
        OBJECT_TYPE_PACKET
        OBJECT_TYPE_ABC
        OBJECT_TYPE_WATERBOX
        OBJECT_TYPE_RIPPLE
        OBJECT_TYPE_FLAG
        OBJECT_TYPE_DOLPHIN
        OBJECT_TYPE_GARDEN_FOG
        OBJECT_TYPE_FOGBOX
    End Enum
End Module