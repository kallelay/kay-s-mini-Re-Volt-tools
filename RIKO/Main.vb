Public Class Main_


    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If GetSetting("Car Load", "settings", "dir_Riko", "") <> "" Then
            Form1.Show()
            Me.Close()

        End If
    End Sub
    Dim Phase% = 0

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Select Case Phase
            Case 0
                Label1.Text = "Thank you! I'm currently searching for Re-Volt directories and once done I'll get you to the next page!"
                Phase = 1
                Label2.Visible = True
                SearchForDir()


        End Select
    End Sub
    Sub SearchForDir()
        If GetSetting("Car Load", "settings", "dir", "") <> "" Then
            ListBox1.Items.Add("Use Car Load's directory (currently set at " & GetSetting("Car Load", "settings", "dir", ""))
        End If








    End Sub
  
End Class