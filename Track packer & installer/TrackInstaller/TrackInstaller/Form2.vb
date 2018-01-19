Public Class Form2
    Dim J As New ProgressBar

    Private Sub Form2_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.Left = Form1.Left + Int(Form1.Width / 2) - Int(Me.Width / 2)
        Me.Top = Form1.Top + Int(Form1.Height / 2) - Int(Me.Height / 2)
        Me.TopMost = True
    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      

        '226; 20

        '33; 14
        J.Location = Label2.Location
        J.Size = Label2.Size
        J.Style = ProgressBarStyle.Marquee
        J.MarqueeAnimationSpeed = 5
        Me.Controls.Add(J)

    End Sub
  

    Private Sub Form2_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        
    End Sub
End Class