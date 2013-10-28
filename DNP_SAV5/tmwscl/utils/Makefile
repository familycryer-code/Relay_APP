# This is an example makefile for the Triangle MicroWorks, Inc.
# Source Code Library. It will attempt to build a library
#
# This file is a simple example designed to be as generic as possible.
# It is highly likely that it will have to be modified to run with
# the target make system

CC = cl -nologo -c
CFLAGS =  /I "../../" /I "../../thirdPartyCode/openssl" /I "../../thirdPartyCode/openssl/inc32"
SRC =	\
	tmwappl.c	\
	tmwdb.c	\
	tmwchnl.c	\
	tmwcrypto.c	\
	tmwdiag.c	\
	tmwdlist.c	\
	tmwdtime.c	\
	tmwlink.c	\
	tmwmem.c	\
	tmwmsim.c	\
	tmwphys.c	\
	tmwphysd.c	\
	tmwpltmr.c	\
	tmwsctr.c	\
	tmwsesn.c	\
	tmwsim.c	\
	tmwtarg.c	\
	tmwtargp.c	\
	tmwtimer.c	\
	tmwtprt.c
OBJ = $(SRC,.c=.o)

.c.o:
	$(CC) $(CFLAGS) $(.SOURCE)

all: $(OBJ)

