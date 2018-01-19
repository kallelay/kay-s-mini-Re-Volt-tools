Imports OpenTK.Graphics.OpenGL
Imports OpenTK
Imports System.Math
Imports QuickFont
Imports OpenTK.Graphics

Public Module Helpers
    ''' <remarks></remarks>
    Enum PolyType
        QUAD = 1
        DOUBLE_SIDED = 2
        SEMI_TRANS = 4
        ADDITIVE_TRANS = 128
        'SEMI_TRANS_ONE = 256
        TEXANIM = 512
        NOENV = 1024
        ENV = 2048

    End Enum

    Public Enum PRMFLAG
        NORMAL = 0
        USEMATRIX = 1

    End Enum

    Public _t As Double = 0


    Public Car_Init = False



    Public PermaModels As New List(Of PRM)

    Public Structure VERTEX
        Dim Position As Vector3
        Dim normal As Vector3
    End Structure
    Public Class Sphere
        Public Radius As Single
        Public center As New Vector3d
        Sub New(ByVal x As Single, ByVal y As Single, ByVal z As Single, ByVal radius As Single)
            center.X = x
            center.Y = y
            center.Z = z
            Me.Radius = radius
        End Sub
        Sub New(ByVal Center As Vector3d, ByVal radius As Single)
            Me.center = Center
            Me.Radius = radius
        End Sub
    End Class

End Module

Module RVCARSTUDIO_SHARED
    Public PolyCount = 0
    Public carList As New List(Of String)


    Public CarIsLoading = False
    Sub ReLoadOneCar(ByVal car As Car, Optional ByVal KeepTextures As Boolean = True, Optional ByVal ReloadParams As Boolean = False)
        PolyCount = 0





        CarIsLoading = True

        ' For Each m In Models
        'm.MyModel.vertnum = Nothing
        '' m.MyModel.vexl = Nothing
        ' m.MyModel.polynum = Nothing
        '  m.MyModel.polyl = Nothing
        ' m.MyModel = Nothing
        ' Next
        ' Models.Clear()

        Application.DoEvents()







        Dim res = car.Load()

        If Not res Then

            Exit Sub

        End If




            Dim ftex = (Replace(RVPATH & "\" & car.Theory.MainInfos.Tpage, ",", "."))
            InitAllTextures(ftex)




        '  On Error Resume Next
        If (car.Theory.Body.modelNumber) <> -1 Then
            If IO.File.Exists(Replace(Replace(RVPATH & "\" & car.Theory.MainInfos.Model(car.Theory.Body.modelNumber), Chr(34), ""), ",", ".")) = True Then
                car.models.BODY = New PRM(Replace(RVPATH & "\" & car.Theory.MainInfos.Model(car.Theory.Body.modelNumber), Chr(34), ""))
                car.models.BODY.TextureI = 1
                Try
                    car.models.BODY.Position = car.Theory.Body.Offset * Zoom  ' -car.Theory.RealInfos.COM / 2
                Catch
                End Try



            End If
        Else

        End If

        Application.DoEvents()


        For i = 0 To 3
            If car.Theory.wheel(i).modelNumber <> -1 Then
                If IO.File.Exists(Replace(RVPATH & "\" & Replace(car.Theory.MainInfos.Model(car.Theory.wheel(i).modelNumber), Chr(34), ""), ",", ".")) = True Then
                    car.models.Wheel(i) = New PRM(RVPATH & "\" & Replace(car.Theory.MainInfos.Model(car.Theory.wheel(i).modelNumber), Chr(34), ""))
                    If car.models.Wheel(i) IsNot Nothing Then
                        car.models.Wheel(i).TextureI = 1
                        car.models.Wheel(i).Position = car.Theory.wheel(i).Offset(1) * Zoom  '+car.Theory.RealInfos  '+car.Theory.wheel(i).offset  * ZOOM  (2) '-car.Theory.Body.offset  * ZOOM  





                    End If
                Else
                    'Tip.fShow("~~Error: MODEL(" &car.Theory.wheel(i).modelNumber & ") doesn't exist" & vbNewLine)
                End If

            End If

        Next





        For i = 0 To 3
            'TODO : springs
            If car.Theory.Spring(i).modelNumber <> -1 Then

                car.models.Spring(i) = New PRM(RVPATH & "\" & car.Theory.MainInfos.Model(car.Theory.Spring(i).modelNumber).Replace(Chr(34), ""))
                car.models.Spring(i).TextureI = 1


                'scale
                Dim Scale! = (car.Theory.Spring(i).Offset - car.Theory.wheel(i).Offset(1)).Length / car.Theory.Spring(i).Length


                car.models.Spring(i).MATRIX = Matrix4.Scale(1, Scale, 1)

                car.models.Spring(i).MATRIX *= BuildLookMatrixDown( _
                            car.Theory.wheel(i).Offset(1) * Zoom, _
                            car.Theory.Spring(i).Offset * Zoom)


                car.models.Spring(i).MATRIX *= Matrix4.CreateTranslation(car.Theory.Spring(i).Offset * Zoom)



            End If


        Next



        For i = 0 To 3
            If car.Theory.Axle(i).modelNumber <> -1 Then


                car.models.axle(i) = New PRM(RVPATH & "\" & car.Theory.MainInfos.Model(car.Theory.Axle(i).modelNumber).Replace(Chr(34), ""))
                car.models.axle(i).TextureI = 1

                Dim Scale! = (car.Theory.Axle(i).offSet - car.Theory.wheel(i).Offset(1)).LengthFast / car.Theory.Axle(i).Length


                car.models.axle(i).MATRIX = Matrix4.Scale(1, 1, Scale)

                car.models.axle(i).MATRIX *= BuildLookMatrixForward( _
                                                    car.Theory.Axle(i).offSet * Zoom, _
                                                      car.Theory.wheel(i).Offset(1) * Zoom)


                car.models.axle(i).MATRIX *= Matrix4.CreateTranslation(car.Theory.Axle(i).offSet * Zoom)




            End If
        Next



        For i = 0 To 3
            If car.Theory.PIN(i).modelNumber <> -1 Then
                'If _Pin(i) IsNot Nothing Then
                car.models.Pin(i) = New PRM(RVPATH & "\" & car.Theory.MainInfos.Model(car.Theory.PIN(i).modelNumber).Replace(Chr(34), ""))
                car.models.Pin(i).TextureI = 1


                'Dim Scale! = (car.Theory.Spring(i).offset  * ZOOM   -car.Theory.wheel(i).offset  * ZOOM (1) ).Length /car.Theory.PIN(i).Length

                car.models.Pin(i).MATRIX = Matrix4.Scale(1, -car.Theory.PIN(i).Length, 1)

                car.models.Pin(i).MATRIX *= BuildLookMatrixDown( _
                         car.Theory.wheel(i).Offset(1) * Zoom, _
                         car.Theory.PIN(i).offSet * Zoom + car.Theory.Spring(i).Offset * Zoom)

                car.models.Pin(i).MATRIX *= Matrix4.CreateTranslation(car.Theory.PIN(i).offSet * Zoom + car.Theory.Spring(i).Offset * Zoom / 2)



                ' _Pin(i).Scale *=car.Theory.PIN(i).Length



                'End If
            End If
        Next



        If car.Theory.Spinner.modelNumber <> -1 Then
            car.models.Spinner = New PRM(RVPATH & "\" & car.Theory.MainInfos.Model(car.Theory.Spinner.modelNumber).Replace(Chr(34), ""))

            car.models.Spinner.TextureI = 1
            'car.models.spinner.Render()
            '  MsgBox(_Spinner.PolysReadingProgress)
            car.models.Spinner.Position = car.Theory.Spinner.offSet * Zoom

            'car.Theory.Spinner.Axis()
            '_Spinner.ScnNode.Scale =car.Theory.Spinner.Axis 

        End If



        If car.Theory.Aerial.ModelNumber <> -1 Then
            If IO.File.Exists(RVPATH & "\" & car.Theory.MainInfos.Model(car.Theory.Aerial.ModelNumber).Replace(Chr(34), "")) = False Then
                ' Tip.fShow("~~Error: MODEL(" &car.Theory.Aerial.ModelNumber & ") doesn't exist" & vbNewLine)
            End If
            car.models.Aerial = New PRM(RVPATH & "\" & car.Theory.MainInfos.Model(car.Theory.Aerial.ModelNumber).Replace(Chr(34), ""))
            If car.models.Aerial IsNot Nothing Then

                car.models.Aerial.TextureI = 2 'RVPATH & "\gfx\fxpage1.bmp"
                ' car.models.aerial.Render()

                car.models.Aerial.MATRIX = Matrix4.Scale(1, car.Theory.Aerial.length * 3 / 2, 1)
                'car.models.aerial.MATRIX *= Matrix4.Scale(car.Theory.Aerial.Direction)
                car.models.Aerial.RenderBBOX = True

                car.models.Aerial.Position = car.Theory.Aerial.offset * Zoom  '+ New Vector3D(0, car.models.aerial.BoundingBox.maxY *car.Theory.Aerial.length, 0)
                'car.models.aerial.MATRIX *= Matrix4.Scale(New Vector3d(1,car.Theory.Aerial.length, 1))
                '  car.models.aerial.Rotation =car.Theory.Aerial.Direction
                'car.models.aerial.ScnNode.Scale.SetLength(car.Theory.Aerial.length)


            End If
        End If



        If car.Theory.Aerial.TopModelNumber <> -1 Then
            Dim aerialtop As New PRM(RVPATH & "\" & car.Theory.MainInfos.Model(car.Theory.Aerial.TopModelNumber).Replace(Chr(34), ""))
            If aerialtop IsNot Nothing Then
                aerialtop.TextureI = 2 ' RVPATH & "\gfx\fxpage1.bmp"
                ' aerialtop.Render()
                '   aerialtop.ScnNode.Scale *= 5
                ' aerialtop.ScnNode.Position *=car.Theory.Aerial.Direction.Y
                aerialtop.MATRIX = Matrix4.Scale(1, -car.Theory.Aerial.length / 3, 1)
                aerialtop.Position = car.Theory.Aerial.offset * Zoom + New Vector3(0, car.Theory.Aerial.length * 3 / 2, 0) ' *car.Theory.Aerial.length * 2

                '  aerialtop.Scale = New Vector3d(1, -5, 1)
                '  aerialtop.ScnNode.Position +=car.Theory.Aerial.Direction
                '  aerialtop.ScnNode.Position +=car.Theory.Aerial.offset  * ZOOM   '+  ' ) '+' Aerial.ScnNode.BoundingBox.MaxEdge



            End If
        End If



        'saving car as default

        CarIsLoading = False

    End Sub

    Dim delta As Integer
    Public PANNING_ALLOWED = False
   
    Public StartedRendering = False : Public FinishedRendering = True
    Public FrC As Single = 0.0F
    Dim sinceLastFrame = 0
    Dim th() As Threading.Thread

End Module
