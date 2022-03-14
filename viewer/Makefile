
# amend to your C# compiler of choice. i use mcs from Mono
CSC=mcs

SRCS=Viewer.cs Main.cs ../validator/Util.cs Constants.cs
EXES=viewer.exe
TESTFILE=$(shell pwd)/doap.rdf

.PHONY: all clean tests

all: $(EXES)

clean:
	rm $(EXES)

viewer.exe: $(SRCS) ../schema/doap.rdf style.xsl
	$(CSC) -debug -out:viewer.exe $(SRCS) -r:Redland.dll \
		-resource:../schema/doap.rdf,doap.rdf \
		-resource:style.xsl,style.xsl


check:
	@./viewer.exe file://${TESTFILE}
