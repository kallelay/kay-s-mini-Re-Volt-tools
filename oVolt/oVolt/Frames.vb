Imports OpenTK
Public Module Frames
    Public Class Frame
        Public UV(3) As Vector2
        Public Tex As Integer
        Public delay As Single
        Sub New()
            For i = 0 To 3
                UV(i) = New Vector2
            Next
        End Sub
    End Class
End Module
