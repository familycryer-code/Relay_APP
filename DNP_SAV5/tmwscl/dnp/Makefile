# This is an example makefile for the Triangle MicroWorks, Inc.
# Source Code Library. It will attempt to build a library
#
# This file is a simple example designed to be as generic as possible.
# It is highly likely that it will have to be modified to run with
# the target make system

CC = cl -nologo -c
CFLAGS =  /I "../../"
SRC =	\
	sdnpauth.c	\
	sdnpdata.c	\
	sdnpdiag.c	\
	sdnpevnt.c	\
	sdnpfsim.c	\
	sdnpmem.c	\
	sdnpo000.c	\
	sdnpo001.c	\
	sdnpo002.c	\
	sdnpo003.c	\
	sdnpo004.c	\
	sdnpo010.c	\
	sdnpo011.c	\
	sdnpo012.c	\
	sdnpo013.c	\
	sdnpo020.c	\
	sdnpo021.c	\
	sdnpo022.c	\
	sdnpo023.c	\
	sdnpo030.c	\
	sdnpo032.c	\
	sdnpo034.c	\
	sdnpo040.c	\
	sdnpo041.c	\
	sdnpo042.c	\
	sdnpo043.c	\
	sdnpo050.c	\
	sdnpo051.c	\
	sdnpo060.c	\
	sdnpo070.c	\
	sdnpo080.c	\
	sdnpo085.c	\
	sdnpo086.c	\
	sdnpo087.c	\
	sdnpo088.c	\
	sdnpo110.c	\
	sdnpo111.c	\
	sdnpo112.c	\
	sdnpo113.c	\
	sdnpo120.c	\
	sdnpo121.c	\
	sdnpo122.c	\
	sdnprbe.c	\
	sdnpsesn.c	\
	sdnpsa.c	\
	sdnpsav2.c	\
	sdnpsim.c	\
	sdnpunsl.c	\
	sdnputil.c	\
	sdnpxml.c	\
	sdnpxml2.c	\
	dnpauth.c	\
	dnpchnl.c	\
	dnpdiag.c	\
	dnpdtime.c	\
	dnplink.c	\
	dnpmem.c	\
	dnpsesn.c	\
	dnpstat.c	\
	dnptprt.c	\
	dnputil.c
OBJ = $(SRC,.c=.o)

.c.o:
	$(CC) $(CFLAGS) $(.SOURCE)

all: $(OBJ)

