# Microsoft Developer Studio Project File - Name="utils" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Static Library" 0x0104

CFG=utils - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "utils.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "utils.mak" CFG="utils - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "utils - Win32 Release" (based on "Win32 (x86) Static Library")
!MESSAGE "utils - Win32 Debug" (based on "Win32 (x86) Static Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "utils - Win32 Release"

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
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c  /I "..\.." /I "..\..\thirdPartyCode\openssl" /I "..\..\thirdPartyCode\openssl\inc32"  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD CPP /nologo  /MD /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c  /I "..\.." /I "..\..\thirdPartyCode\openssl" /I "..\..\thirdPartyCode\openssl\inc32"  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409  /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ELSEIF  "$(CFG)" == "utils - Win32 Debug"

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
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ  /c  /I "..\.." /I "..\..\thirdPartyCode\openssl" /I "..\..\thirdPartyCode\openssl\inc32"  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
# ADD CPP /nologo /MDd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ  /c  /I "..\.." /I "..\..\thirdPartyCode\openssl" /I "..\..\thirdPartyCode\openssl\inc32"  /D "_CRT_SECURE_NO_WARNINGS" /D "TMWCNFG_INCLUDE_ASSERTS" /D "TMW_WTK_TARGET" /D "TMW_PRIVATE"
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

# Name "utils - Win32 Release"
# Name "utils - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=tmwappl.c
# End Source File
# Begin Source File

SOURCE=tmwdb.c
# End Source File
# Begin Source File

SOURCE=tmwchnl.c
# End Source File
# Begin Source File

SOURCE=tmwcrypto.c
# End Source File
# Begin Source File

SOURCE=tmwdiag.c
# End Source File
# Begin Source File

SOURCE=tmwdlist.c
# End Source File
# Begin Source File

SOURCE=tmwdtime.c
# End Source File
# Begin Source File

SOURCE=tmwlink.c
# End Source File
# Begin Source File

SOURCE=tmwmem.c
# End Source File
# Begin Source File

SOURCE=tmwmsim.c
# End Source File
# Begin Source File

SOURCE=tmwphys.c
# End Source File
# Begin Source File

SOURCE=tmwphysd.c
# End Source File
# Begin Source File

SOURCE=tmwpltmr.c
# End Source File
# Begin Source File

SOURCE=tmwsctr.c
# End Source File
# Begin Source File

SOURCE=tmwsesn.c
# End Source File
# Begin Source File

SOURCE=tmwsim.c
# End Source File
# Begin Source File

SOURCE=tmwtarg.c
# End Source File
# Begin Source File

SOURCE=tmwtargp.c
# End Source File
# Begin Source File

SOURCE=tmwtimer.c
# End Source File
# Begin Source File

SOURCE=tmwtprt.c
# End Source File
# Begin Source File

SOURCE=tmwvrsn.c
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=tmwappl.h
# End Source File
# Begin Source File

SOURCE=tmwdb.h
# End Source File
# Begin Source File

SOURCE=tmwchnl.h
# End Source File
# Begin Source File

SOURCE=tmwcnfg.h
# End Source File
# Begin Source File

SOURCE=tmwcrypto.h
# End Source File
# Begin Source File

SOURCE=tmwdefs.h
# End Source File
# Begin Source File

SOURCE=tmwtypes.h
# End Source File
# Begin Source File

SOURCE=tmwdiag.h
# End Source File
# Begin Source File

SOURCE=tmwdlist.h
# End Source File
# Begin Source File

SOURCE=tmwdtime.h
# End Source File
# Begin Source File

SOURCE=tmwlink.h
# End Source File
# Begin Source File

SOURCE=tmwmem.h
# End Source File
# Begin Source File

SOURCE=tmwmsim.h
# End Source File
# Begin Source File

SOURCE=tmwphys.h
# End Source File
# Begin Source File

SOURCE=tmwphysd.h
# End Source File
# Begin Source File

SOURCE=tmwpltmr.h
# End Source File
# Begin Source File

SOURCE=tmwscl.h
# End Source File
# Begin Source File

SOURCE=tmwsctr.h
# End Source File
# Begin Source File

SOURCE=tmwsesn.h
# End Source File
# Begin Source File

SOURCE=tmwsim.h
# End Source File
# Begin Source File

SOURCE=tmwtarg.h
# End Source File
# Begin Source File

SOURCE=tmwtargp.h
# End Source File
# Begin Source File

SOURCE=tmwtimer.h
# End Source File
# Begin Source File

SOURCE=tmwtprt.h
# End Source File
# Begin Source File

SOURCE=tmwvrsn.h
# End Source File
# End Group
# End Target
# End Project
