# This is an example makefile for the Triangle MicroWorks, Inc.
# Source Code Library. It will attempt to build a library
#
# This file is a simple example designed to be as generic as possible.
# It is highly likely that it will have to be modified to run with
# the target make system

CC = cl -nologo -c
CFLAGS =  /I "../"
SRC =	\
	lin232.c	\
	liniodiag.c	\
	liniotarg.c	\
	lintcp.c
OBJ = $(SRC,.c=.o)

.c.o:
	$(CC) $(CFLAGS) $(.SOURCE)

all: $(OBJ)

