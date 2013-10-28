// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#pragma once

#define _BIND_TO_CURRENT_VCLIBS_VERSION 1

#define __STR2__(x) #x
#define __STR1__(x) __STR2__(x)
#define __LOC__ __FILE__ "("__STR1__(__LINE__)") : Note: "

#if _MSC_VER >= 1300
#define WINVER 0x0501
#endif

// Insert your headers here
#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
#define _WIN32_DCOM

#ifndef _DEBUG
#include <stdlib.h>
#endif

#include <windows.h>
#include <ObjBase.h>

#include <process.h>

#include <time.h>
#include <winsock2.h>  // socket API

#include <sys\timeb.h>

// TODO: reference additional headers your program requires here

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

