# Microsoft Developer Studio Project File - Name="winiotarg" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Dynamic-Link Library" 0x0102

CFG=winiotarg - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "winiotarg.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "winiotarg.mak" CFG="winiotarg - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "winiotarg - Win32 Release" (based on "Win32 (x86) Dynamic-Link Library")
!MESSAGE "winiotarg - Win32 Debug" (based on "Win32 (x86) Dynamic-Link Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "winiotarg - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c  /I ".."  /D "_CRT_SECURE_NO_WARNINGS" /D "WINIOTARG_EXPORTS" /D "_USRDLL" /D "_WINDOWS" /D "TMW_PRIVATE" /D "TMW_WTK_TARGET"
# ADD CPP /nologo  /MD /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c  /I ".."  /D "_CRT_SECURE_NO_WARNINGS" /D "WINIOTARG_EXPORTS" /D "_USRDLL" /D "_WINDOWS" /D "TMW_PRIVATE" /D "TMW_WTK_TARGET"
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409  /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /dll /machine:I386
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /dll /machine:I386 ..\tmwscl\utils\release\utils.lib /incremental:no /out:"../bin/WinIoTarg.dll" /implib:"lib/WinIoTarg.lib" /libpath:"\mbpxxsupp" 

!ELSEIF  "$(CFG)" == "winiotarg - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ  /c  /I ".."  /D "_CRT_SECURE_NO_WARNINGS" /D "WINIOTARG_EXPORTS" /D "_USRDLL" /D "_WINDOWS" /D "TMW_PRIVATE" /D "TMW_WTK_TARGET"
# ADD CPP /nologo /MDd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ  /c  /I ".."  /D "_CRT_SECURE_NO_WARNINGS" /D "WINIOTARG_EXPORTS" /D "_USRDLL" /D "_WINDOWS" /D "TMW_PRIVATE" /D "TMW_WTK_TARGET"
# ADD BASE RSC /l 0x409 /d "_DEBUG"
# ADD RSC /l 0x409  /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /dll /debug /machine:I386 /pdbtype:sept
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /dll /debug /machine:I386 ..\tmwscl\utils\debug\utils.lib /incremental:yes /pdb:"Debug/WinIoTarg.pdb" /debug /out:"../bin/WinIoTarg.dll" /implib:"lib/WinIoTarg.lib" /pdbtype:sept /libpath:"\code\thirdPartyCode\mbpsupp"  

!ENDIF

# Begin Target

# Name "winiotarg - Win32 Release"
# Name "winiotarg - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=StdAfx.cpp
# End Source File
# Begin Source File

SOURCE=Win232Channel.cpp
# End Source File
# Begin Source File

SOURCE=WinIoConnector.cpp
# End Source File
# Begin Source File

SOURCE=WinIoInterface.cpp
# End Source File
# Begin Source File

SOURCE=WinIoTarg.cpp
# End Source File
# Begin Source File

SOURCE=WinIoTargTimer.cpp
# End Source File
# Begin Source File

SOURCE=WinMBPChannel.cpp
# End Source File
# Begin Source File

SOURCE=WinTCPChannel.cpp
# End Source File
# Begin Source File

SOURCE=WinTCPListener.cpp
# End Source File
# Begin Source File

SOURCE=WinIoBaseTime.cpp
# End Source File
# Begin Source File

SOURCE=WinIoSimulatedTime.cpp
# End Source File
# Begin Source File

SOURCE=WinIoSystemTime.cpp
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=StdAfx.h
# End Source File
# Begin Source File

SOURCE=Win232Channel.h
# End Source File
# Begin Source File

SOURCE=WinIoConnector.h
# End Source File
# Begin Source File

SOURCE=WinIoInterface.h
# End Source File
# Begin Source File

SOURCE=include/WinIoTarg.h
# End Source File
# Begin Source File

SOURCE=include/WinIoTargDefs.h
# End Source File
# Begin Source File

SOURCE=include/WinIoTargEnums.h
# End Source File
# Begin Source File

SOURCE=WinIoTargTimer.h
# End Source File
# Begin Source File

SOURCE=WinMBPChannel.h
# End Source File
# Begin Source File

SOURCE=WinTCPChannel.h
# End Source File
# Begin Source File

SOURCE=WinTCPListener.h
# End Source File
# Begin Source File

SOURCE=WinThreading.h
# End Source File
# Begin Source File

SOURCE=WinIoBaseTime.h
# End Source File
# Begin Source File

SOURCE=WinIoSimulatedTime.h
# End Source File
# Begin Source File

SOURCE=WinIoSystemTime.h
# End Source File
# End Group
# End Target
# End Project
