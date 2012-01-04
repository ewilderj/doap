CSC=mcs /debug

CONFIG=Debug

DLLSRC= \
	Doap/AssemblyInfo.cs \
	Doap/ProjectProperties.cs \
	Doap/Resource.cs \
	Doap/Release.cs \
	Doap/Attributes/BugDatabaseAttribute.cs \
	Doap/Attributes/CreatedAttribute.cs \
	Doap/Attributes/HomepageAttribute.cs \
	Doap/Attributes/LicenseAttribute.cs \
	Doap/Attributes/MailingListAttribute.cs \
	Doap/Attributes/MaintainerAttribute.cs \
	Doap/Attributes/ReleaseAttribute.cs \
	Doap/Attributes/ScreenshotsAttribute.cs \
	Doap/Attributes/ShortDescriptionAttribute.cs

EXESRC= \
    DoapWriter/AssemblyInfo.cs \
    DoapWriter/DoapSharp.cs 

DLL=Doap/bin/$(CONFIG)/Doap.dll
DLLPDB=Doap/bin/$(CONFIG)/Doap.pdb
EXE=DoapWriter/bin/$(CONFIG)/DoapWriter.exe

all: DoapWriter

clean:
	rm -rf Doap/bin
	rm -rf DoapWriter/bin

DoapWriter: $(EXE)

DoapLib: $(DLL)

$(EXE): $(EXESRC) DoapWriter/bin/$(CONFIG)/Doap.dll
	[ -d DoapWriter/bin/$(CONFIG) ] || mkdir -p DoapWriter/bin/$(CONFIG)
	$(CSC) /target:exe /out:$@ /lib:Doap/bin/$(CONFIG) /r:Doap $(EXESRC)

$(DLL): $(DLLSRC)
	[ -d Doap/bin/$(CONFIG) ] || mkdir -p Doap/bin/$(CONFIG)
	$(CSC) /target:library /out:$@ $(DLLSRC)

DoapWriter/bin/$(CONFIG)/Doap.dll: $(DLL)
	[ -d DoapWriter/bin/$(CONFIG) ] || mkdir -p DoapWriter/bin/$(CONFIG)
	cp $(DLL) $@
