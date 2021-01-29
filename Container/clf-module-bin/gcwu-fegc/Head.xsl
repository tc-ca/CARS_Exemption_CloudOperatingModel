<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="utf-8" indent="yes" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:value-of select="/info/data[@name='header']/dataBefore" disable-output-escaping="yes"/>

		<xsl:for-each select="/info/data[@name='metadata']/metadata">
			<xsl:text disable-output-escaping="yes">&lt;meta</xsl:text>
				<xsl:text disable-output-escaping="yes"> name="</xsl:text>
					<xsl:value-of select="name" disable-output-escaping="yes"/>
				<xsl:text disable-output-escaping="yes">"</xsl:text>

				<xsl:if test="scheme">
					<xsl:text disable-output-escaping="yes"> scheme="</xsl:text>
						<xsl:value-of select="scheme" disable-output-escaping="yes"/>
					<xsl:text disable-output-escaping="yes">"</xsl:text>
				</xsl:if>

				<xsl:text disable-output-escaping="yes"> content="</xsl:text>
					<xsl:value-of select="content" disable-output-escaping="yes"/>
				<xsl:text disable-output-escaping="yes">"</xsl:text>
			<xsl:text disable-output-escaping="yes"> /&gt;</xsl:text>
		</xsl:for-each>

		<xsl:value-of select="/info/data[@name='header']/dataInside" disable-output-escaping="yes"/>
	</xsl:template>
</xsl:stylesheet>