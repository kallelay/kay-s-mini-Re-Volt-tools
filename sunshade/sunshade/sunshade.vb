Imports System.Drawing
Module sunshade



    Public Function calculateShade(ByVal degree!) As Color
        Dim tmp As New Color
        Dim sv! = 0.0F
        '	RVColor tmp = RVColor(); 
        'float sv; //grad?
        degree = degree Mod 180

        If (degree <= -30) Then
            tmp = Color.FromArgb(20, 23, 40)
        ElseIf (degree > -30 And degree <= -15) Then
            sv = (30 + degree) / 15.0
            tmp = Color.FromArgb(20, 23, 40)

        ElseIf (degree > -15 And degree <= 0) Then

            sv = (15 + degree) / 15.0
            tmp = Color.FromArgb(152 + 23.0 * sv, 160 - 60 * sv, 181 - 181.0 * sv)
        ElseIf (degree > 0 And degree <= 10) Then
            sv = (degree / 10)
            tmp = Color.FromArgb(175 + 80.0 * sv, 136 - 136 * sv, 0 + 142.0 * sv)
        ElseIf (degree > 10 And degree <= 15) Then
            sv = ((degree - 10) / 5)
            tmp = Color.FromArgb(255 - 63 * sv, 206 - 83 * sv, 142 + 48 * sv)
        ElseIf (degree > 15 And degree <= 20) Then
            sv = ((degree - 15) / 5)
            tmp = Color.FromArgb((192 - 38 * sv), (153 + 77 * sv), (190 + 65 * sv))
        Else
            sv = ((degree - 20) / 70)
            tmp = Color.FromArgb((230 + 25 * sv), (230 + 25 * sv), 255)
        End If

        Return tmp

    End Function


	

End Module
