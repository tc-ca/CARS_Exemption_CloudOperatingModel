<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="utf-8" indent="yes" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="not(info/data[@name='leftMenuItems']/Item)">
				<xsl:value-of select="/info/data[@name='col']/oneCol" disable-output-escaping="yes"/>
			</xsl:when>

			<xsl:otherwise>
				<xsl:value-of select="/info/data[@name='col']/twoCol" disable-output-escaping="yes"/>
			</xsl:otherwise>
		</xsl:choose>

		<xsl:value-of select="/info/data[@name='navaid']" disable-output-escaping="yes"/>

		<xsl:for-each select="/info/data[@name='breadcrumbsItems']/Item">
			<li>
				<xsl:choose>
					<xsl:when test="not(position()=last())">
						<xsl:text disable-output-escaping="yes">&lt;a</xsl:text>
							<xsl:text disable-output-escaping="yes"> href="</xsl:text>
								<xsl:value-of select="itemLink" disable-output-escaping="yes"/>
							<xsl:text disable-output-escaping="yes">"</xsl:text>

							<xsl:text disable-output-escaping="yes"> title="</xsl:text>
								<xsl:value-of select="itemTitle" disable-output-escaping="yes"/>
							<xsl:text disable-output-escaping="yes">"</xsl:text>

							<xsl:text disable-output-escaping="yes">&gt;</xsl:text>

							<xsl:value-of select="itemTitle" disable-output-escaping="yes"/>
						<xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
					</xsl:when >

					<xsl:otherwise >
						<xsl:value-of select="itemTitle" disable-output-escaping="yes"/>
					</xsl:otherwise>
				</xsl:choose>
			</li>
		</xsl:for-each>

		<xsl:value-of select="/info/data[@name='breadEnd']" disable-output-escaping="yes"/>

		<xsl:if test="info/data[@name='rightMenuItems']/Item" >
			<xsl:value-of select="/info/layoutData/openContentWithRightSideMenu" disable-output-escaping="yes"/>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>