# This is an example top level makefile for Triangle MicroWorks, Inc.
# Source Code Library. This makefile recurses into the required
# subdirectories and performs a make. This will typically build the
# appropriate library.
#
# This file is a simple example designed to be as generic as possible.
# It is highly likely that it will have to be modified to run with
# the target make system

CD = cd
MAKE = make

all:
	$(CD) ./dnp; $(MAKE);
	$(CD) ./utils; $(MAKE);
	-rm *.lib;
