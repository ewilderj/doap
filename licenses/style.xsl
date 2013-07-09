<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:cc="http://web.resource.org/cc/"
				xmlns:dc="http://purl.org/dc/elements/1.1/"
				xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
				xmlns:rdfs="http://www.w3.org/2000/01/rdf-schema#"
				xmlns="http://www.w3.org/1999/xhtml"
				exclude-result-prefixes="rdfs rdf cc dc"
                version="1.0">

<xsl:output indent="yes" encoding="utf-8" method="xml"
			doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
			doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"
			/>

<xsl:template match="cc:License">
<html>
  <head>
	<title>DOAP License Description: <xsl:value-of select="rdfs:label" /></title>
  </head>
  <body>
	<h1>License: <xsl:value-of select="rdfs:label" /></h1>
	<h2><xsl:value-of select="dc:title" /></h2>
	<h3>URI for use in DOAP</h3>
	<p>To use this as a license in your DOAP file, add the following:</p>
	<p><tt>&lt;license rdf:resource="<xsl:value-of select="@rdf:about" />" /&gt;</tt></p>
	<h3>See Also</h3>
	<xsl:for-each select="rdfs:seeAlso">
	  <p><a href="{@rdf:resource}"><xsl:value-of select="@rdf:resource" /></a></p>
	</xsl:for-each>
  </body>
</html>
</xsl:template>

<xsl:template match="/">
  <xsl:apply-templates select="//cc:License" />
</xsl:template>

</xsl:stylesheet>