
Module GlobalModule
    '''''''''''''''''''''''''''
    'Global Variables
    '''''''''''''''''''''''
    Public Zoom = 1
    'Public CurrentWorld As WorldFile

    'Do write
    Sub DoWrite(ByVal Str As String)
        Form1.TextBox1.AppendText(">> " & Str & vbNewLine)
    End Sub
    Sub log(ByVal Str As String)
        Form1.TextBox2.AppendText(">> " & Str & vbNewLine)
    End Sub
    Public Function RGBToLong(ByVal color As Color) As Int32
        Return Convert.ToInt32(color.A) << 24 Or CUInt(color.R) << 16 Or CUInt(color.G) << 8 Or CUInt(color.B) << 0
    End Function

    Public Function ColorsToRGB(ByVal cl As UInt32) As Color
        'long rgb value, is composed from 0~255 R, G, B
        'according to net: (2^8)^cn
        ' cn: R = 0 , G = 1, B = 2


        'simple...
        Dim a = cl >> 24

        If a = 0 Then a = 251


        ' 
        ' If a = 0 Then a = 255
        Dim r = cl >> 16 And &HFF

        Dim g = cl >> 8 And &HFF
        Dim b = cl >> 0 And &HFF


        Return Color.FromArgb(a, r, g, b)


    End Function

End Module
