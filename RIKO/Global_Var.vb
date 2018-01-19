Module Global_Variables
    ''' <summary>
    ''' Headers and application
    ''' </summary>
    ''' <remarks></remarks>
    ''' Versions
    '''  Revision is Auto Incrimented (OnBuild)
    Public Const MajorVersion = "3"
    Public Const MinorVersion = "0"
    Public Const BuildNumber = "0"

    Public Const Revision = "33"
    Public Const Type As VerType = VerType.RC

    '' Applications info
    Public Const APPNAME = "Riko"
    Public Const InnerNAME = "Riko"
    Public Const Maker = "Kallel A.Y"
    Public Const Description = APPNAME & " by " & Maker
    Public Const ACTIVE_YEARS = "2011-2016"
    Public Const COPY = "Copyright © " & APPNAME & " (" & ACTIVE_YEARS & "). Based on RV Car Studio (Car Load 2) and Car lighting..."

    Public Const FULL_INFO = APPNAME & " (" & MajorVersion & "." & MinorVersion & ") by " & Maker & "." & vbNewLine & _
                            "All rights reserved © " & ACTIVE_YEARS & vbNewLine & _
                            "Licensed under GNU GPL." & vbNewLine & _
                            vbNewLine & _
                            "This program uses VoltGL rendering engine based on OpenTK" & vbNewLine & _
                            "VoltGL's full source code is included in " & APPNAME & ". All rights reserved to its maker Kallel A.Y" & vbNewLine & _
                            "VoltGL uses OpenTK: Copyright (c) 2006 - 2012 the Open Toolkit library." & vbNewLine & _
                            vbNewLine & _
                            "This programs uses Car::Load's source code all ported to VoltGL/RvCarStudio. Copyright C::L 2009-2012" & vbNewLine & _
                            vbNewLine & _
                                "Re-Volt is a trademark of Acclaim and IP by We Go Interactive. All rights reserved WeGOi 2013" & vbNewLine & _
                            "We Go Interactive and " & Maker & " are different. None represents the other" & vbNewLine & _
                            ""



    Public PickingPRM As Boolean = False

    Public Active_Car = 0
    Enum VerType
        preAlpha = -1
        alpha = 0
        beta = 1
        gamma = 2
        RC = 3
        release = 4
    End Enum

    Public RVPATH As String



    Public Enum PRMFLAG
        NORMAL = 0
        USEMATRIX = 1

    End Enum


    Public _t As Double = 0


    Public Car_Init = False




End Module
