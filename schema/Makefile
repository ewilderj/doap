## DOAP Schema makefile
# 
# this is run on a post-receive hook when the deploy
# remote origin is pushed to

# deploy won't work if there's spaces in the path... sorry

DIR:=$(dir $(abspath $(lastword $(MAKEFILE_LIST))))

.PHONY: live all

all:
	@echo "Type 'make live' to transfer the schema to the live site."

live:
	cp $(DIR)doap.rdf /home/edd/public_html/static/usefulinc.com/ns/doap
