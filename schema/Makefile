## DOAP Schema makefile
# 
# this is run on a post-receive hook when the deploy
# remote origin is pushed to

DIR:=$(PWD)/$({0%/*})

.PHONY: live all

all:
	@echo "Type 'make live' to transfer the schema to the live site."

live:
	cp $(DIR)doap.rdf /home/edd/public_html/static/usefulinc.com/ns/doap
