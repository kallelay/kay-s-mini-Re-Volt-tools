Public Class Form3

    Private Sub Form3_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.Left = Form1.Left + Int(Form1.Width / 2) - Int(Me.Width / 2)
        Me.Top = Form1.Top + Int(Form1.Height / 2) - Int(Me.Height / 2)
        Me.TopMost = True
    End Sub

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each ctr As Control In Me.Controls
            AddHandler ctr.MouseMove, AddressOf MouseMove_
        Next
    End Sub
    Dim Mpos As Point
    Sub MouseMove_(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Location += (MousePosition - Mpos)
            Mpos = MousePosition

        Else
            'capture pointer position
            Mpos = MousePosition


        End If

    End Sub
    Public Sub ShowMessage(ByVal title As String, ByVal message As String)
        Me.Left = Form1.Left + Int(Form1.Width / 2) - Int(Me.Width / 2)
        Me.Top = Form1.Top + Int(Form1.Height / 2) - Int(Me.Height / 2)
        Me.TopMost = True


        Label2.Text = title
        Label1.Text = message
        '  Me.ShowDialog(sender)
        Beep()
        Me.TopMost = True
        Me.StartPosition = FormStartPosition.CenterScreen


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Yes
        Me.Hide()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.No
        Me.Hide()
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

    End Sub
End Class