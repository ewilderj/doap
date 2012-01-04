<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns="http://www.w3.org/1999/xhtml"
				exclude-result-prefixes=""
                version="1.0">

<xsl:output indent="yes" encoding="utf-8" method="xml"
			/>

<xsl:template match="shortdesc">
  <p class="shortdesc"><xsl:value-of select="." /></p>
</xsl:template>

<xsl:template match="project">
  <div class="project">
	<h1><xsl:value-of select="name" /></h1>
	<xsl:apply-templates select="shortdesc" />
	<h3>Description</h3>
	<p><xsl:value-of select="description" /></p>
	<p>Project created on <xsl:value-of select="created" />.</p>
	<h3>Links</h3>
	<div class="urls">

	  <div class="link">
		<div class="label">Home page</div>
		<div class="resource"><a href="{homepage/@href}"><xsl:value-of select="homepage/@href" /></a></div>
	  </div>

	  <xsl:for-each select="./mailing-list">
		<div class="link">
		  <div class="label">Mailing list</div>
		  <div class="resource"><a href="{@href}"><xsl:value-of select="@href" /></a></div>
		</div>
	  </xsl:for-each>

	  <xsl:for-each select="wiki">
		<div class="link">
		  <div class="label">Wiki</div>
		  <div class="resource"><a href="{@href}"><xsl:value-of select="@href" /></a></div>
		</div>
	  </xsl:for-each>

	  <xsl:for-each select="wiki">
		<div class="link">
		  <div class="label">Wiki</div>
		  <div class="resource"><a href="{@href}"><xsl:value-of select="@href" /></a></div>
		</div>
	  </xsl:for-each>

	  <xsl:for-each select="download-page">
		<div class="link">
		  <div class="label">Download</div>
		  <div class="resource"><a href="{@href}"><xsl:value-of select="@href" /></a></div>
		</div>
	  </xsl:for-each>

	  <xsl:for-each select="screenshots">
		<div class="link">
		  <div class="label">Screenshots</div>
		  <div class="resource"><a href="{@href}"><xsl:value-of select="@href" /></a></div>
		</div>
	  </xsl:for-each>
	  
	  
	  <xsl:for-each select="bug-database">
		<div class="link">
		  <div class="label">Bug database</div>
		  <div class="resource"><a href="{@href}"><xsl:value-of select="@href" /></a></div>
		</div>
	  </xsl:for-each>
	  
	</div>

	<h3>People</h3>

	<xsl:for-each select="person">
	  <div class="person">
		<div class="role">
		  <xsl:value-of select="@role" />
		</div>

		<div class="pname">
		  <xsl:choose>
			<xsl:when test="@homepage">
			  <a href="{@homepage}"><xsl:value-of select="@name" /></a>
			</xsl:when>
			<xsl:otherwise>
			  <xsl:value-of select="@name" />
			</xsl:otherwise>
		  </xsl:choose>
		</div>

	  </div>
	</xsl:for-each>
	
  </div>
</xsl:template>

<xsl:template match="/">
  <xsl:apply-templates select="//project" />
</xsl:template>

</xsl:stylesheet>