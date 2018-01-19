'
' Crée par SharpDevelop.
' Utilisateur: Admin
' Date: 07/05/2011
' Heure: 18:20
'
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 
'''''''''                  CAR LOAD's loading engine optimized          '''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports System.IO
imports system.Drawing
Module Global_
	Public zoomFactor As Single = 1
	Sub LogError()
		console.ForegroundColor = consolecolor.Red
		Console.WriteLine("Error!")
		End Sub
End Module
Public Class Car_Model

    Public Directory As String
    Public FileName As String
    Public DirectoryName As String
    Public MyModel As New MODEL
    Public PolysReadingProgress, VertexReadingProgress As Double
    Public Texture_ As String = ""
    Public VxCount As Single
    Public isMirror As Boolean = False
    Sub New(ByVal filepath As String)

        filepath = Replace(filepath, ",", ".")

        If IO.File.Exists(filepath) = False Then
           Err("Error, file ( " & strings.right(filepath,20) & ") not found")
            endit()
            
        End If

        Dim old As Double

      

        Dim J As New BinaryReader(New FileStream(Replace(filepath, ",", "."), FileMode.Open))
        If J Is Nothing Then Exit Sub


        'Vert/Poly count
        MyModel.polynum = J.ReadInt16()
        MyModel.vertnum = J.ReadInt16()



        ReDim MyModel.polyl(MyModel.polynum)
        Dim i as integer
        For i = 0 To MyModel.polynum - 1

            If old <> Int(100 * i / (MyModel.polynum)) Then
                PolysReadingProgress = Int(100 * i / (MyModel.polynum))
            End If

            old = Int(100 * i / (MyModel.polynum))


            MyModel.polyl(i).type = J.ReadInt16
            MyModel.polyl(i).Tpage = J.ReadInt16


            MyModel.polyl(i).vi0 = J.ReadInt16
            MyModel.polyl(i).vi1 = J.ReadInt16
            MyModel.polyl(i).vi2 = J.ReadInt16
            MyModel.polyl(i).vi3 = J.ReadInt16



            MyModel.polyl(i).c0 = J.ReadInt32
            MyModel.polyl(i).c1 = J.ReadInt32
            MyModel.polyl(i).c2 = J.ReadInt32
            MyModel.polyl(i).c3 = J.ReadInt32

            MyModel.polyl(i).u0 = J.ReadSingle
            MyModel.polyl(i).v0 = J.ReadSingle
            MyModel.polyl(i).u1 = J.ReadSingle
            MyModel.polyl(i).v1 = J.ReadSingle
            MyModel.polyl(i).u2 = J.ReadSingle
            MyModel.polyl(i).v2 = J.ReadSingle
            MyModel.polyl(i).u3 = J.ReadSingle
            MyModel.polyl(i).v3 = J.ReadSingle
        Next

        ReDim MyModel.vexl(MyModel.vertnum)
dim a as integer
        For a = 0 To MyModel.vertnum - 1
            If old <> Int(a * 100 / (MyModel.vertnum)) Then
                VertexReadingProgress = Int(a * 100 / (MyModel.vertnum))
            End If

            old = Int(a * 100 / (MyModel.vertnum))
            Dim x, y, z As Single

            x = J.ReadSingle '* ZoomFactor
            y = J.ReadSingle ' * -1 * ZoomFactor
            z = J.ReadSingle '* ZoomFactor


            MyModel.vexl(a).Position = New Vector3D(x, y, z)

            x = J.ReadSingle '* ZoomFactor
            y = J.ReadSingle '* ZoomFactor * -1
            z = J.ReadSingle '* ZoomFactor
            MyModel.vexl(a).normal = New Vector3D(x, y, z)


        Next

        J.Close()
        'let's set Directory and also Filename

       
       


    End Sub
    
    
     Sub Export(ByVal filepath As String)

        filepath = Replace(filepath, ",", ".")

       


      
If io.File.Exists(Replace(filepath, ",", ".")) Then
	filecopy (Replace(filepath, ",", "."),Replace(filepath, ",", ".") & ".bak" & int(rnd * 500))
	kill( Replace(filepath, ",", "."))
End If
        Dim J As New BinaryWriter(New FileStream(Replace(filepath, ",", "."), FileMode.CreateNew))
        If J Is Nothing Then Exit Sub


        'Vert/Poly count
        J.Write( Convert.ToInt16(  MyModel.polynum))
        J.Write( Convert.ToInt16( MyModel.vertnum))

dim i as integer

       	For i = 0 To MyModel.polynum - 1

           


'
J.Write( Convert.ToInt16( MyModel.polyl(i).type))
J.Write( Convert.ToInt16( MyModel.polyl(i).tpage))

J.Write(Convert.ToInt16( MyModel.polyl(i).vi0))
J.Write(Convert.ToInt16( MyModel.polyl(i).vi1))
J.Write(Convert.ToInt16( MyModel.polyl(i).vi2))
J.Write(Convert.ToInt16( MyModel.polyl(i).vi3))

J.Write(Convert.ToInt32(MyModel.polyl(i).c0))
J.Write(Convert.ToInt32(MyModel.polyl(i).c1))
J.Write(Convert.ToInt32(MyModel.polyl(i).c2))
J.Write(Convert.ToInt32(MyModel.polyl(i).c3))

J.Write(csng(MyModel.polyl(i).u0))
J.Write(csng(MyModel.polyl(i).v0))
J.Write(csng(MyModel.polyl(i).u1))
J.Write(csng(MyModel.polyl(i).v1))
J.Write(csng(MyModel.polyl(i).u2))
J.Write(csng(MyModel.polyl(i).v2))
J.Write(csng(MyModel.polyl(i).u3))
J.Write(csng(MyModel.polyl(i).v3))
        

        Next

     
dim a as integer
        For a = 0 To MyModel.vertnum - 1
           
          
J.Write(csng(MyModel.vexl(a).Position.x))
J.Write(csng(MyModel.vexl(a).Position.y))
J.Write(csng(MyModel.vexl(a).Position.z))

J.Write(csng(MyModel.vexl(a).normal.x))
J.Write(csng(MyModel.vexl(a).normal.y))
J.Write(csng(MyModel.vexl(a).normal.z))
           
        

        Next

        J.Close()
      
    End Sub
   

    


    'Structure

    Public Structure MODEL_POLY_LOAD
        Dim type, Tpage As Int16
        Dim vi0, vi1, vi2, vi3 As Int16
        Dim c0, c1, c2, c3 As Long
        Dim u0, v0, u1, v1, u2, v2, u3, v3 As Single
    End Structure

    Public Structure MODEL_VERTEX_MORPH
        Dim Position As Vector3D
        Dim normal As Vector3D
    End Structure
    Public Structure Sphere
        Dim Center As Vector3D
        Dim radius As Single
    End Structure
    Public Structure BBOX
        Dim minX, maxX As Single
        Dim minY, maxY As Single
        Dim minZ, maxZ As Single
    End Structure

Public Class Vector3D
	Public x,y,z As Single
	Sub New(_x_ As Single,_y_ As Single,_z_ As Single)
		x = _x_
		y = _y_
		z = _z_
	End Sub
End Class



   
    Public Class MODEL
        Public polynum, vertnum As Short
        Public polyl() As MODEL_POLY_LOAD
        Public vexl() As MODEL_VERTEX_MORPH
    End Class
    
    
End Class