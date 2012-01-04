## DOAP Schema makefile

.PHONY: live all

all:
	@echo "Type 'make live' to transfer the schema to the live site."

live:
	@cp doap.rdf doap
	lftp -e "open -u edd http://usefulinc.com/ns/; put doap;  quit"
	@rm doap
