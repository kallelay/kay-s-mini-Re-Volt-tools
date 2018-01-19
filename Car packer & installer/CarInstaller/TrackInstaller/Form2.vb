Public Class Form2
    Dim J As New ProgressBar

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '226; 20

        '33; 14
        J.Location = Label2.Location
        J.Size = Label2.Size
        J.Style = ProgressBarStyle.Marquee
        J.MarqueeAnimationSpeed = 5
        Me.Controls.Add(J)

    End Sub
  

End Class