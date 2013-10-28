# Microsoft Developer Studio Project File - Name="DNPSlave" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Application" 0x0101

CFG=DNPSlave - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "DNPSlave.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "DNPSlave.mak" CFG="DNPSlave - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "DNPSlave - Win32 Release" (based on "Win32 (x86) Application")
!MESSAGE "DNPSlave - Win32 Debug" (based on "Win32 (x86) Application")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "DNPSlave - Win32 Release"

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
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c  /I ".."  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD CPP /nologo  /MD /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c  /I ".."  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409  /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib Winmm.lib /nologo /subsystem:windows /debug /machine:I386 /pdbtype:sept
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib Winmm.lib /nologo /machine:I386  /subsystem:console /incremental:no /pdb:"Release/DNPSlave.pdb" /machine:I386 /nodefaultlib:"libc.lib" /nodefaultlib:"libcmt.lib" /nodefaultlib:"libcd.lib" /nodefaultlib:"libcmtd.lib" /nodefaultlib:"msvcrtd.lib" /out:"../bin/DNPSlave.exe" ..\winiotarg\lib\WinIoTarg.lib 

!ELSEIF  "$(CFG)" == "DNPSlave - Win32 Debug"

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
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ  /c  /I ".."  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD CPP /nologo /MDd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ  /c  /I ".."  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD BASE RSC /l 0x409 /d "_DEBUG"
# ADD RSC /l 0x409  /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib wsock32.lib Winmm.lib /nologo /subsystem:windows /debug /machine:I386 /pdbtype:sept
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib wsock32.lib Winmm.lib /nologo /subsystem:windows /debug /machine:I386 /subsystem:console /incremental:yes /pdb:"Debug/DNPSlave.pdb" /debug /machine:I386 /nodefaultlib:"libc.lib" /nodefaultlib:"libcmt.lib" /nodefaultlib:"msvcrt.lib" /nodefaultlib:"libcd.lib" /nodefaultlib:"libcmtd.lib" /out:"../bin/DNPSlave.exe" /pdbtype:sept ..\winiotarg\lib\WinIoTarg.lib 

!ENDIF

# Begin Target

# Name "DNPSlave - Win32 Release"
# Name "DNPSlave - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=DNPSlave.cpp
# End Source File
# Begin Source File

SOURCE=DNPMultithreadSlave.cpp
# End Source File
# Begin Source File

SOURCE=stdafx.cpp
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=MakefileLinux
# End Source File
# Begin Source File

SOURCE=stdafx.h
# End Source File
# End Group
# End Target
# End Project
