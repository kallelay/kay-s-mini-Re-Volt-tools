Module ruless
    Dim errorFlag As Long
    Public score = 0
    Sub searchForErrors()
        On Error Resume Next
        '---------------------------For score     20
        score = 20

        errorFlag = 0
        Dim lb = Form1.ListBox1
        Dim lb2 = Form1.ListBox2
        Dim lb3 = Form1.ListBox3

        lb.Items.Clear()
        lb2.Items.Clear()
        lb3.Items.Clear()

        'Lighting_Rain_X
        Dim rainc = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_RAIN).Count
        Dim lightnc = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_LIGHTNING).Count
        Dim pickups = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_PICKUP).Count
        Dim gstars = Fob.getIDsFor(OBJ_ID_TYPE.OBJECT_TYPE_STAR).Count

        lb.Items.Add("Pickups   : " & pickups)
        lb.Items.Add("Stars     : " & gstars)
        lb.Items.Add("Rain      : " & rainc)
        lb.Items.Add("Lightings : " & lightnc)
        lb.Items.Add("")
        lb.Items.Add("Pickup/m  : " & Math.Round(pickups / Form1.trlen.Text.Replace("m", "") * 100, 2) & "%")


        '---------------------------For score     20
        Dim pickperm = Math.Round(pickups / Form1.trlen.Text.Replace("m", "") * 100, 2)
        If pickperm = 0 Then
            lb2.Items.Add("Your track has NO PICKUPS!!")
            lb3.Items.Add("Your track has NO PICKUPS!!")
            'For score
            score += 5

        ElseIf pickperm < 5 Then
            lb2.Items.Add("Your track is a little deprived of pickups")
            If Form1.trlen.Text.Replace("m", "") < 400 Then
                lb2.Items.Add("But I guess it's okay as it's shorter than 400m...")
                lb2.Items.Add("It's going to feel like Supermarket 2...")
                score += 10

            Else
                lb2.Items.Add("Do you actually hate pickups? if not consider adding more!")
                score += 10
            End If
        ElseIf pickperm < 10 Then
            lb2.Items.Add("Good amount of pickups! Did you know? Most of stock only have less 35 pickups for track length < 800m?")
            score += 20
        Else
            lb2.Items.Add("You sir.... have put tooo much pickups!!")
            score += 5

        End If

        If rainc > 0 And lightnc = 0 Then
            lb2.Items.Add("There is rain, but no lighting...")
            score -= 5
        End If
        If rainc = 0 And lightnc > 0 Then
            lb2.Items.Add("There is no rain but lighting?...")
            score -= 5
        End If

        algo = 0
        cr = 0
        For i = 0 To UBound(curWorld.mMesh)
            algo = 0
            For j = LBound(curWorld.mMesh(i).polyl) To UBound(curWorld.mMesh(i).polyl)
                algo += (curWorld.mMesh(i).polyl(j).c0 / 3.0 + curWorld.mMesh(i).polyl(j).c1 / 3.0 + curWorld.mMesh(i).polyl(j).c2 / 3.0) / curWorld.mMesh(i).polynum

            Next
            cr += algo / curWorld.meshCount
        Next


        '---------------------------For score     20
        Dim c = UintToColor(cr)
        cr = (c.R / 3.0 + c.G / 3.0 + c.B / 3.0)
        lb3.Items.Add("The average color in this world is (" & c.R & ", " & c.B & ", " & c.G & ")  [Gray:" & Math.Round(cr, 2) & "]")
        If cr > 250 Then
            lb3.Items.Add("Your track is too bright!!!! consider shading it!")
            score += 10
        ElseIf cr > 220 Then
            lb3.Items.Add("Your track is a little bright.... Sunny?")
            score += 15
        ElseIf cr > 180 Then
            lb3.Items.Add("Perfect lighting for a sunny layout")
            score += 20
        ElseIf cr > 120 Then
            lb3.Items.Add("hmm... good settings for afternoon or morning")
            score += 20
        ElseIf cr > 80 Then
            lb3.Items.Add("Your track is a little dark")
            score += 12
        Else
            lb3.Items.Add("Dark track!! Make sure you add a lot of lights!")
            score += 5
        End If


        '---------------------------For score     20
        If curWorld.meshCount < 1000 Then
            lb3.Items.Add("Your cubes aren't too much, they are " & Cubes.Count)
            lb3.Items.Add("Consider using WORLD_CUT to cut your world!")
            score += 14
        ElseIf curWorld.meshCount < 1500 Then
            lb3.Items.Add("Good amount of cubes,.... it would be nice if you add more!")
            score += 17
        Else
            lb3.Items.Add("Cube count is fine! (" & Cubes.Count & ")")
            score += 20
        End If


        '---------------------------For score     20
        lb3.Items.Add("Total Visual Polygons count in .w file is : " & curWorld.PolyCount)
        If curWorld.PolyCount < 10000 Then
            lb3.Items.Add("Track doesn't have much polygons... you can add more!")
            score += 14
        ElseIf curWorld.PolyCount < 30000 Then
            lb3.Items.Add("Track polygons count is good, but make sure that your world is well reparted.")
            score += 20
        ElseIf curWorld.PolyCount < 50000 Then
            lb3.Items.Add("Track polygons count is a little high. ")
            lb3.Items.Add("You may want to use WORLD_CUT and visiboxes if you haven't.")
            score += 17
        ElseIf curWorld.PolyCount < 80000 Then
            lb3.Items.Add("Make sure to use visiboxes and world_cut")
            score += 15

        ElseIf curWorld.PolyCount < 100000 Then
            lb3.Items.Add("You have too much polygons!!! polish your track!")
            score += 13
        Else
            lb3.Items.Add("We advise to revise your track once more. Too much polygons!!")
            score += 10
        End If
   

        '---------------------------For score     20
        lb3.Items.Add("Physics (NCP) polygons count :" & NCP.ListOfNCP.Count)

        If NCP.ListOfNCP.Count < 16000 Then
            lb3.Items.Add("NCP polygons amount is fair and is 1.1-compatible")
            score += 20
        ElseIf NCP.ListOfNCP.Count < 32000 Then
            lb3.Items.Add("NCP polygons amount is a little high, 1.2a-exclusive")
            score += 18
        Else
            lb3.Items.Add("NCP polygons amount is a little too much, 1.2a-exclusive")
            score += 14
        End If


        '---------------------------For score     20
        Dim trlen = Form1.trlen.Text.Replace("m", "")
        lb3.Items.Add("Track length :" & trlen)
        If trlen = 0 Then
            lb3.Items.Add("NOT A RACEABLE TRACK!")
            score += 10
        ElseIf trlen < 300 Then
            lb3.Items.Add("track is too short!")
            score += 16
        ElseIf trlen < 500 Then
            lb3.Items.Add("Track length is okay, a little short...")
            score += 18
        ElseIf trlen < 800 Then
            lb3.Items.Add("track's length is okay!")
            score += 20
        ElseIf trlen < 1000 Then
            lb3.Items.Add("track is a bit long!")
            score += 17
        ElseIf trlen < 1500 Then
            lb3.Items.Add("track is long!")
            score += 13
        Else
            lb3.Items.Add("Annoyingly LONG track")
            score += 10
        End If


        If IO.Directory.GetFiles(RvDir & "\levels\" & dirName, "*.bmq").Count = 0 Then
            lb3.Items.Add("Track doesn't have mipmaps!!")
            score -= 5

        End If
        canScore = True
        canscore2 = True

    End Sub
    Dim algo, cr As New Double
    Public canScore = False, canscore2 = False
    Enum Errors
        no_pickups
        no_globalstars
        Lighting_Rain_x
        no_skymap

        cubes_not_many
        dark_but_not_dark
        no_visiboxes


        missing_w
        missing_ncp
        missing_inf

        ReadMe_doesnt_exist
        bmq_files_dont_exist
        no_mp3file
        track_incomplete
        track_not_shaded


    End Enum

    Dim fscore = 0
    Sub polish()
        Dim lb = Form1.ListBox6
        lb.Items.Clear()
        Dim bmpCnt = IO.Directory.GetFiles(RvDir & "\levels\" & dirName, dirName & "?.bmp").Count * 40

        If canscore2 = True Then
            Form1.Label42.Show()
            Form1.Button14.Show()
        Else
            Form1.Label42.Hide()
            Form1.Button14.Hide()
        End If

        lb.Items.Add(If(canscore2 = False, "Bitmap test was run.....", "WARNING: Bitmap testing did not run! A low score is expected!"))


        If canscore2 = True Then : lb.Items.Add("I have calculated Machine score : " & Math.Round(score / (140 + bmpCnt) * 20, 2) & "/5.71*")
        Else : lb.Items.Add("I have calculated Machine score : " & Math.Round(score / (140 + bmpCnt) * 20, 2) & "/20")

        End If


        If canscore2 = True Then lb.Items.Add("(*) please run bitmap test!! .... so far you have got " & Math.Round(score / 140 * 20, 2) & "/20")
        If (score / (140 + bmpCnt) > 0.75) Then : lb.Items.Add("You should be proud! This is an excellent score!")
        ElseIf (score / (140 + bmpCnt) > 0.5) Then : lb.Items.Add("I hope it has turned it out to be well! It's very strict so consider 10/20 a super result!")
        Else : lb.Items.Add("This score is very strict! If it's around 10/20 then your track is just fine!") : End If

        Form1.Label41.Text = Math.Round(score / (140 + bmpCnt) * 20, 2) & vbNewLine & "        /20"


        lb.Items.Add("")
        Dim fs! = FileSize / (1024 * 1024)
        lb.Items.Add("File size (" & Math.Round(fs, 2) & "MB)")
        If fs < 15 Then
            lb.Items.Add("This should make it to Re-Volt Zone...")
        Else
            lb.Items.Add("!!!!! This may not make it to Re-Volt zone!!" & vbNewLine & "Reduce it to less than 15MB!")
        End If



    End Sub

End Module
