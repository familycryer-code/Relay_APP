# Microsoft Developer Studio Project File - Name="dnp" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Static Library" 0x0104

CFG=dnp - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "dnp.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "dnp.mak" CFG="dnp - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "dnp - Win32 Release" (based on "Win32 (x86) Static Library")
!MESSAGE "dnp - Win32 Debug" (based on "Win32 (x86) Static Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "dnp - Win32 Release"

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
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c  /I "..\.."  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD CPP /nologo  /MD /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c  /I "..\.."  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409  /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ELSEIF  "$(CFG)" == "dnp - Win32 Debug"

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
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ  /c  /I "..\.."  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD CPP /nologo /MDd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ  /c  /I "..\.."  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD BASE RSC /l 0x409 /d "_DEBUG"
# ADD RSC /l 0x409  /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ENDIF

# Begin Target

# Name "dnp - Win32 Release"
# Name "dnp - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=sdnpauth.c
# End Source File
# Begin Source File

SOURCE=sdnpdata.c
# End Source File
# Begin Source File

SOURCE=sdnpdiag.c
# End Source File
# Begin Source File

SOURCE=sdnpevnt.c
# End Source File
# Begin Source File

SOURCE=sdnpfsim.c
# End Source File
# Begin Source File

SOURCE=sdnpmem.c
# End Source File
# Begin Source File

SOURCE=sdnpo000.c
# End Source File
# Begin Source File

SOURCE=sdnpo001.c
# End Source File
# Begin Source File

SOURCE=sdnpo002.c
# End Source File
# Begin Source File

SOURCE=sdnpo003.c
# End Source File
# Begin Source File

SOURCE=sdnpo004.c
# End Source File
# Begin Source File

SOURCE=sdnpo010.c
# End Source File
# Begin Source File

SOURCE=sdnpo011.c
# End Source File
# Begin Source File

SOURCE=sdnpo012.c
# End Source File
# Begin Source File

SOURCE=sdnpo013.c
# End Source File
# Begin Source File

SOURCE=sdnpo020.c
# End Source File
# Begin Source File

SOURCE=sdnpo021.c
# End Source File
# Begin Source File

SOURCE=sdnpo022.c
# End Source File
# Begin Source File

SOURCE=sdnpo023.c
# End Source File
# Begin Source File

SOURCE=sdnpo030.c
# End Source File
# Begin Source File

SOURCE=sdnpo032.c
# End Source File
# Begin Source File

SOURCE=sdnpo034.c
# End Source File
# Begin Source File

SOURCE=sdnpo040.c
# End Source File
# Begin Source File

SOURCE=sdnpo041.c
# End Source File
# Begin Source File

SOURCE=sdnpo042.c
# End Source File
# Begin Source File

SOURCE=sdnpo043.c
# End Source File
# Begin Source File

SOURCE=sdnpo050.c
# End Source File
# Begin Source File

SOURCE=sdnpo051.c
# End Source File
# Begin Source File

SOURCE=sdnpo060.c
# End Source File
# Begin Source File

SOURCE=sdnpo070.c
# End Source File
# Begin Source File

SOURCE=sdnpo080.c
# End Source File
# Begin Source File

SOURCE=sdnpo085.c
# End Source File
# Begin Source File

SOURCE=sdnpo086.c
# End Source File
# Begin Source File

SOURCE=sdnpo087.c
# End Source File
# Begin Source File

SOURCE=sdnpo088.c
# End Source File
# Begin Source File

SOURCE=sdnpo110.c
# End Source File
# Begin Source File

SOURCE=sdnpo111.c
# End Source File
# Begin Source File

SOURCE=sdnpo112.c
# End Source File
# Begin Source File

SOURCE=sdnpo113.c
# End Source File
# Begin Source File

SOURCE=sdnpo120.c
# End Source File
# Begin Source File

SOURCE=sdnpo121.c
# End Source File
# Begin Source File

SOURCE=sdnpo122.c
# End Source File
# Begin Source File

SOURCE=sdnprbe.c
# End Source File
# Begin Source File

SOURCE=sdnpsesn.c
# End Source File
# Begin Source File

SOURCE=sdnpsa.c
# End Source File
# Begin Source File

SOURCE=sdnpsav2.c
# End Source File
# Begin Source File

SOURCE=sdnpsim.c
# End Source File
# Begin Source File

SOURCE=sdnpunsl.c
# End Source File
# Begin Source File

SOURCE=sdnputil.c
# End Source File
# Begin Source File

SOURCE=sdnpxml.c
# End Source File
# Begin Source File

SOURCE=sdnpxml2.c
# End Source File
# Begin Source File

SOURCE=dnpauth.c
# End Source File
# Begin Source File

SOURCE=dnpchnl.c
# End Source File
# Begin Source File

SOURCE=dnpdiag.c
# End Source File
# Begin Source File

SOURCE=dnpdtime.c
# End Source File
# Begin Source File

SOURCE=dnplink.c
# End Source File
# Begin Source File

SOURCE=dnpmem.c
# End Source File
# Begin Source File

SOURCE=dnpsesn.c
# End Source File
# Begin Source File

SOURCE=dnpstat.c
# End Source File
# Begin Source File

SOURCE=dnptprt.c
# End Source File
# Begin Source File

SOURCE=dnputil.c
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=sdnpauth.h
# End Source File
# Begin Source File

SOURCE=sdnpcnfg.h
# End Source File
# Begin Source File

SOURCE=sdnpdata.h
# End Source File
# Begin Source File

SOURCE=sdnpdiag.h
# End Source File
# Begin Source File

SOURCE=sdnpevnt.h
# End Source File
# Begin Source File

SOURCE=sdnpfsim.h
# End Source File
# Begin Source File

SOURCE=sdnpmem.h
# End Source File
# Begin Source File

SOURCE=sdnpo000.h
# End Source File
# Begin Source File

SOURCE=sdnpo001.h
# End Source File
# Begin Source File

SOURCE=sdnpo002.h
# End Source File
# Begin Source File

SOURCE=sdnpo003.h
# End Source File
# Begin Source File

SOURCE=sdnpo004.h
# End Source File
# Begin Source File

SOURCE=sdnpo010.h
# End Source File
# Begin Source File

SOURCE=sdnpo011.h
# End Source File
# Begin Source File

SOURCE=sdnpo012.h
# End Source File
# Begin Source File

SOURCE=sdnpo013.h
# End Source File
# Begin Source File

SOURCE=sdnpo020.h
# End Source File
# Begin Source File

SOURCE=sdnpo021.h
# End Source File
# Begin Source File

SOURCE=sdnpo022.h
# End Source File
# Begin Source File

SOURCE=sdnpo023.h
# End Source File
# Begin Source File

SOURCE=sdnpo030.h
# End Source File
# Begin Source File

SOURCE=sdnpo032.h
# End Source File
# Begin Source File

SOURCE=sdnpo034.h
# End Source File
# Begin Source File

SOURCE=sdnpo040.h
# End Source File
# Begin Source File

SOURCE=sdnpo041.h
# End Source File
# Begin Source File

SOURCE=sdnpo042.h
# End Source File
# Begin Source File

SOURCE=sdnpo043.h
# End Source File
# Begin Source File

SOURCE=sdnpo050.h
# End Source File
# Begin Source File

SOURCE=sdnpo051.h
# End Source File
# Begin Source File

SOURCE=sdnpo060.h
# End Source File
# Begin Source File

SOURCE=sdnpo070.h
# End Source File
# Begin Source File

SOURCE=sdnpo080.h
# End Source File
# Begin Source File

SOURCE=sdnpo085.h
# End Source File
# Begin Source File

SOURCE=sdnpo086.h
# End Source File
# Begin Source File

SOURCE=sdnpo087.h
# End Source File
# Begin Source File

SOURCE=sdnpo088.h
# End Source File
# Begin Source File

SOURCE=sdnpo110.h
# End Source File
# Begin Source File

SOURCE=sdnpo111.h
# End Source File
# Begin Source File

SOURCE=sdnpo112.h
# End Source File
# Begin Source File

SOURCE=sdnpo113.h
# End Source File
# Begin Source File

SOURCE=sdnpo120.h
# End Source File
# Begin Source File

SOURCE=sdnpo121.h
# End Source File
# Begin Source File

SOURCE=sdnpo122.h
# End Source File
# Begin Source File

SOURCE=sdnprbe.h
# End Source File
# Begin Source File

SOURCE=sdnpsav2.h
# End Source File
# Begin Source File

SOURCE=sdnpsa.h
# End Source File
# Begin Source File

SOURCE=sdnpsesn.h
# End Source File
# Begin Source File

SOURCE=sdnpsesp.h
# End Source File
# Begin Source File

SOURCE=sdnpsim.h
# End Source File
# Begin Source File

SOURCE=sdnpunsl.h
# End Source File
# Begin Source File

SOURCE=sdnputil.h
# End Source File
# Begin Source File

SOURCE=sdnpxml.h
# End Source File
# Begin Source File

SOURCE=sdnpxml2.h
# End Source File
# Begin Source File

SOURCE=dnpauth.h
# End Source File
# Begin Source File

SOURCE=dnpchnl.h
# End Source File
# Begin Source File

SOURCE=dnpcnfg.h
# End Source File
# Begin Source File

SOURCE=dnpdata.h
# End Source File
# Begin Source File

SOURCE=dnpdefs.h
# End Source File
# Begin Source File

SOURCE=dnpdiag.h
# End Source File
# Begin Source File

SOURCE=dnpdtime.h
# End Source File
# Begin Source File

SOURCE=dnplink.h
# End Source File
# Begin Source File

SOURCE=dnpmem.h
# End Source File
# Begin Source File

SOURCE=dnpsesn.h
# End Source File
# Begin Source File

SOURCE=dnpstat.h
# End Source File
# Begin Source File

SOURCE=dnptprt.h
# End Source File
# Begin Source File

SOURCE=dnputil.h
# End Source File
# End Group
# End Target
# End Project
