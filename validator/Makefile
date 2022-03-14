
# amend to your C# compiler of choice. i use mcs from Mono
CSC=mcs

SRCS=Main.cs AssemblyInfo.cs Validator.cs Util.cs XmlTests.cs \
	 Tests.cs
EXES=validator.exe

.PHONY: all clean tests

all: $(EXES)

clean:
	rm $(EXES)

doap.rng: doap.rnc
	trang -I rnc -O rng doap.rnc doap.rng

validator.exe: $(SRCS) ../schema/doap.rdf doap.rng
	$(CSC) -debug -out:validator.exe $(SRCS) -r:Redland.dll \
		-resource:../schema/doap.rdf,doap.rdf \
		-resource:doap.rng,doap.rng \
		-r:Commons.Xml.Relaxng

tests:
	for x in tests/*.rdf; do \
		echo "Trying: $$x"; \
		mono validator.exe file://`pwd`/$$x; \
	done
