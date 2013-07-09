## DOAP licenses

.PHONY: live all

LICENSES=gpl lgpl bsd mit artistic mpl w3c asl10 asl11 asl20

all:
	@echo "Type 'make live' to transfer the schema to the live site."

live:
	lftp -e "open -u edd http://usefulinc.com/doap/licenses; put style.xsl ${LICENSES};  quit"
