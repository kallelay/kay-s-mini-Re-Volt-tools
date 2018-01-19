Public Class Form1
    Dim Done = False, started = False
 
    Public Class Forum
    

        Public Name As String
        Public members As Integer
        Public posts As Integer
        Public str$
        Public Sub New(ByVal n$)
            Name = n$
            X.Add(Me)
        End Sub
    End Class



    Public Shared X As New List(Of Forum)
    'MICROSOFT YOU STUPID, CAN'T DO A GOOD AND FAST SORT ALGORITHM!!!!!
    Public Sub DoSort(ByVal l As List(Of Forum))
        Dim max = -1000, id = 0
        For i = 0 To l.Count - 1
            max = l(i).posts
            id = i
            For j = i To l.Count - 1
                If max < l(j).posts Then
                    max = l(j).posts
                    id = j
                End If
            Next
            Dim aux As Forum
            aux = l(id)
            l(id) = l(i)
            l(i) = aux
        Next

    End Sub

    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Done Or started Then Exit Sub

        started = True
        Dim n As Net.WebRequest, a As IO.StreamReader, s$

        Application.DoEvents()



        Application.DoEvents()

        'ARM
        Label1.Text = "Fetching ARM"
        Application.DoEvents()
        n = Net.WebRequest.Create("http://www.aliasrvm.altervista.org/phpBB3/index.php")
        a = New IO.StreamReader(n.GetResponse.GetResponseStream)
        Label1.Text = "getting response from ARM"
        s = a.ReadToEnd()
        Label1.Text = "Reading results"
        Dim ARM As New Forum("ARM")
        ARM.str = "# [[aliasrevoltmaster|Alias Re-Volt Master]] (Italian)"
        ARM.posts = fine(Split(Split(s, "Totale messaggi: <strong>")(1), "</strong>")(0))
        ARM.members = Split(Split(s, "Totale iscritti: <strong>")(1), "</strong>")(0)
        Label1.Text = "ARM, handshaking"

        Application.DoEvents()
        s = ""
        a = Nothing

        'RVTT
        Label1.Text = "Fetching RVTT"
        Application.DoEvents()
        n = Net.WebRequest.Create("http://forum.rvtt.com/index.php")
        a = New IO.StreamReader(n.GetResponse.GetResponseStream)
        Label1.Text = "getting response from RVTT"
        s = a.ReadToEnd()
        s = Split(s, "<h3>Statistiques</h3>")(1)
        Label1.Text = "Reading results"
        Dim RVTT As New Forum("RVTT")
        RVTT.str = "# [[RVTT]] (French)"
        RVTT.posts = fine(Split(Split(s, "<p><strong>")(1), "</strong>")(0))
        RVTT.members = Split(Split(s, "<strong>")(3), "</strong>")(0)
        Label1.Text = "RVTT, handshaking"
        Application.DoEvents()

        Application.DoEvents()
        s = ""
        a = Nothing

        'RVL
        Label1.Text = "Fetching RVL"
        Application.DoEvents()
        n = Net.WebRequest.Create("http://z3.invisionfree.com/Revolt_Live/index.php?")
        a = New IO.StreamReader(n.GetResponse.GetResponseStream)
        Label1.Text = "getting response from RVL"
        s = a.ReadToEnd()
        Label1.Text = "Reading results"
        Dim RVL As New Forum("RVL")
        RVL.str = "# [[Re-Volt_Live|Re-Volt Live]]"
        RVL.posts = fine(Split(Split(s, "align='left'>Our members have made a total of <b>")(1), "</b>")(0))
        RVL.members = Split(Split(s, "posts<br />We have <b>")(1), "</b>")(0)
        Label1.Text = "RVL, handshaking"
        Application.DoEvents()


        Application.DoEvents()
        s = ""
        a = Nothing

        'RRR
        Label1.Text = "Fetching RRR"
        Application.DoEvents()
        n = Net.WebRequest.Create("http://z8.invisionfree.com/RRR_Racing_Forum/index.php?")
        a = New IO.StreamReader(n.GetResponse.GetResponseStream)
        Label1.Text = "getting response from RRR"
        s = a.ReadToEnd()
        Label1.Text = "Reading results"
        Dim RRR As New Forum("RRR")
        RRR.str = "# [[RRR_Racing_Forum|RRR Racing Forum]]"
        RRR.posts = fine(Split(Split(s, "align='left'>Our members have made a total of <b>")(1), "</b>")(0))
        RRR.members = Split(Split(s, "posts<br />We have <b>")(1), "</b>")(0)
        Label1.Text = "RRR, handshaking"
        Application.DoEvents()


        Application.DoEvents()
        s = ""
        a = Nothing



        'ORP
        Label1.Text = "Fetching ORP"
        Application.DoEvents()
        n = Net.WebRequest.Create("http://z3.invisionfree.com/Our_Revolt_Pub/index.php?")
        a = New IO.StreamReader(n.GetResponse.GetResponseStream)
        Label1.Text = "getting response from ORP"
        s = a.ReadToEnd()
        Label1.Text = "Reading results"
        Dim ORP As New Forum("ORP")
        ORP.str = "# [[ORP|Our Re-Volt Pub]]"
        ORP.posts = fine(Split(Split(s, "align='left'>Our members have made a total of <b>")(1), "</b>")(0))
        ORP.members = Split(Split(s, "posts<br />We have <b>")(1), "</b>")(0)
        Label1.Text = "ORP, handshaking"
        Application.DoEvents()


        Application.DoEvents()
        s = ""
        a = Nothing

        'RVF
        Label1.Text = "Fetching RVF"
        Application.DoEvents()
        n = Net.WebRequest.Create("http://z4.invisionfree.com/re_volt_forum")
        a = New IO.StreamReader(n.GetResponse.GetResponseStream)
        Label1.Text = "getting response from RVF"
        s = a.ReadToEnd()
        Label1.Text = "Reading results"
        Dim RVF As New Forum("RVF")
        RVF.str = "# [[Re-Volt Forum]]"
        RVF.posts = fine(Split(Split(s, "align='left'>Our members have made a total of <b>")(1), "</b>")(0))
        RVF.members = Split(Split(s, "posts<br />We have <b>")(1), "</b>")(0)
        Label1.Text = "RVF, handshaking"
        Application.DoEvents()


        Application.DoEvents()
        s = ""
        a = Nothing


        'RVFE
        'Label1.Text = "Fetching RVFE"
        ' Application.DoEvents()
        ' n = Net.WebRequest.Create("http://s13.zetaboards.com/RV_Frontend/index/")
        '  a = New IO.StreamReader(n.GetResponse.GetResponseStream)
        '   Label1.Text = "getting response from RVFE"
        '  s = a.ReadToEnd()
        '   Label1.Text = "Reading results"
        '   Dim RVFE As New Forum("RVFE")
        '  RVFE.str = "# [[RVFrontend]] (German)"
        '  RVFE.posts = fine(Split(Split(s, "align='left'>Our members have made a total of <b>")(1), "</b>")(0))
        '  RVFE.members = Split(Split(s, "posts<br />We have <b>")(1), "</b>")(0)
        ' Label1.Text = "RVFE, handshaking"
        '  Application.DoEvents()

        '  Application.DoEvents()
        '  s = ""
        '  a = Nothing

        'RVFE
        Label1.Text = "Fetching RVFE"
        Application.DoEvents()
        n = Net.WebRequest.Create("http://s13.zetaboards.com/RV_Frontend/index/")
        Label1.Text = "Reading results"
        Dim RVFE As New Forum("RVFE")
        RVFE.str = "# [[RVFrontend]] (German)"
        RVFE.posts = "0"
        RVFE.members = "0"
        Label1.Text = "RVFE, handshaking"
        Application.DoEvents()

        Application.DoEvents()
        s = ""
        a = Nothing



        'RVdev
        Label1.Text = "Fetching RVDEV"
        Application.DoEvents()
        n = Net.WebRequest.Create("http://z3.invisionfree.com/rvdev/index.php?")
        a = New IO.StreamReader(n.GetResponse.GetResponseStream)
        Label1.Text = "getting response from RVdev"
        s = a.ReadToEnd()
        Label1.Text = "Reading results"
        Dim RVdev As New Forum("RVdev")
        RVdev.str = "# [[RVDev|Re-Volt Developers]]"
        RVdev.posts = fine(Split(Split(s, "align='left'>Our members have made a total of <b>")(1), "</b>")(0))
        RVdev.members = Split(Split(s, "posts<br />We have <b>")(1), "</b>")(0)
        Label1.Text = "RVdev, handshaking"
        Application.DoEvents()

        Label1.Text = "Sorting!"
        Application.DoEvents()
        s = ""
        a = Nothing

        'Custom Sorting...
        DoSort(X)
        Application.DoEvents()
        Dim nstr$ = "Here is a comparison of board statistics as of " & Now.ToShortDateString & vbNewLine
        For i = 0 To X.Count - 1
            nstr &= X.Item(i).str & vbNewLine
            nstr &= Tidy(X.Item(i).members, X.Item(i).posts) & vbNewLine
        Next


        Result.TextBox1.Text = nstr
        Result.Show()
        Me.Hide()


        Done = True

    End Sub

    Function fine(ByVal s$) As String
        Return Replace(Replace(Replace(Replace(s, ",", ""), ".", ""), " ", ""), vbTab, "")
    End Function
    Function Tidy(ByVal members%, ByVal posts%)
        Return "#: '''" & Refine(posts) & "''' posts | '''" & Refine(members) & "''' members [" & Strings.Format(posts / members, "0#.#0") & " posts/user]"
    End Function
    Function Refine(ByVal s%) As String
        If Len(CStr(s)) <= 3 Then
            Return s
        Else
            Return CStr(s \ 1000) & "." & Strings.Right(s, 3)
        End If
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
