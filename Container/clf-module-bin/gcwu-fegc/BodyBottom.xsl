<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="xml" encoding="utf-8" indent="yes" omit-xml-declaration="yes" />
	<xsl:template match="/">

		<!-- Render Right Side Menu -->
		<xsl:if test="info/data[@name='rightMenuItems']/Item" >
			<xsl:value-of select="/info/layoutData/closeContentWithRightSideMenu" disable-output-escaping="yes"/>
			<xsl:value-of select="/info/layoutData/openRightSideMenu" disable-output-escaping="yes"/>

			<xsl:for-each select="/info/data[@name='rightMenuItems']/Item">

				<!-- Open Menu Item -->
				<xsl:if test="itemType = 'OpenItem'">
					<xsl:value-of select="/info/layoutData/openLeftList" disable-output-escaping="yes"/>

					<!-- Render Individual item's <a> element -->
					<xsl:call-template name="menuitem">
						<xsl:with-param name="item" select="current()"/>
					</xsl:call-template>

					<xsl:value-of select="/info/layoutData/closeLeftList" disable-output-escaping="yes"/>
				</xsl:if>

				<!-- Close Menu Item -->
				<xsl:if test="itemType = 'CloseItem'">
				</xsl:if>

				<!-- Open Menu -->
				<xsl:if test="itemType = 'OpenMenu'">
					<xsl:value-of select="/info/layoutData/openRightSideMenuBlock" disable-output-escaping="yes"/>
					<xsl:value-of select="/info/layoutData/openH2" disable-output-escaping="yes"/>
					<xsl:value-of select="itemTitle" disable-output-escaping="yes"/>
					<xsl:value-of select="/info/layoutData/closeH2" disable-output-escaping="yes"/>

					<xsl:if test="itemHasSubItems = 'true'">
						<xsl:value-of select="/info/layoutData/openLeft" disable-output-escaping="yes"/>
					</xsl:if>
				</xsl:if>

				<!-- Close Menu -->
				<xsl:if test="itemType = 'CloseMenu'">
					<xsl:if test="itemHasSubItems = 'true'">
						<xsl:value-of select="/info/layoutData/closeLeft" disable-output-escaping="yes"/>
					</xsl:if>

					<xsl:value-of select="/info/layoutData/closeRightSideMenuBlock" disable-output-escaping="yes"/>
				</xsl:if>

			</xsl:for-each>

			<xsl:value-of select="/info/layoutData/closeRightSideMenu" disable-output-escaping="yes"/>
		</xsl:if>

		<xsl:value-of select="/info/data[@name='footer']/topfooter" disable-output-escaping="yes"/>

		<!-- Render Left Side Menu -->
		<xsl:if test="/info/data[@name='leftMenuItems']/Item" >
			<xsl:value-of select="/info/data[@name='footer']/leftMenu/top" disable-output-escaping="yes"/>

			<xsl:for-each select="/info/data[@name='leftMenuItems']/Item">

				<!-- Open Menu Item -->
				<xsl:if test="itemType = 'OpenItem'">
					<xsl:value-of select="/info/layoutData/openLeftList" disable-output-escaping="yes"/>

					<!-- Render Individual item's <a> element -->
					<xsl:call-template name="menuitem">
						<xsl:with-param name="item" select="current()"/>
					</xsl:call-template>

					<xsl:if test="itemHasSubItems = 'true'">
						<xsl:value-of select="/info/layoutData/openLeft" disable-output-escaping="yes"/>
					</xsl:if>
				</xsl:if>

				<!-- Close Menu Item -->
				<xsl:if test="itemType = 'CloseItem'">
					<xsl:if test="itemHasSubItems = 'true'">
						<xsl:value-of select="/info/layoutData/closeLeft" disable-output-escaping="yes"/>
					</xsl:if>

					<xsl:value-of select="/info/layoutData/closeLeftList" disable-output-escaping="yes"/>
				</xsl:if>

				<!-- Open Menu -->
				<xsl:if test="itemType = 'OpenMenu'">
					<xsl:value-of select="/info/layoutData/openH3" disable-output-escaping="yes"/>
					<xsl:value-of select="itemTitle" disable-output-escaping="yes"/>
					<xsl:value-of select="/info/layoutData/closeH3" disable-output-escaping="yes"/>

					<xsl:if test="itemHasSubItems = 'true'">
						<xsl:value-of select="/info/layoutData/openLeft" disable-output-escaping="yes"/>
					</xsl:if>
				</xsl:if>

				<!-- Close Menu -->
				<xsl:if test="itemType = 'CloseMenu'">
					<xsl:if test="itemHasSubItems = 'true'">
						<xsl:value-of select="/info/layoutData/closeLeft" disable-output-escaping="yes"/>
					</xsl:if>
				</xsl:if>

			</xsl:for-each>

			<xsl:value-of select="/info/data[@name='footer']/leftMenu/bottom" disable-output-escaping="yes"/>
		</xsl:if>

		<xsl:value-of select="/info/data[@name='footer']/lowerfooter" disable-output-escaping="yes"/>
	</xsl:template>

	<!-- Template used to render a Menu Item's <a href="...">...</a> element -->
	<xsl:template name="menuitem">
		<xsl:param name="item"/>

		<!-- Open the "a" element: <a -->
		<xsl:text disable-output-escaping="yes">&lt;a</xsl:text>

		<!-- Add the "id" attribute: id="..." <a -->
		<xsl:if test="$item/itemID">
			<xsl:text disable-output-escaping="yes"> id="</xsl:text>
				<xsl:value-of select="$item/itemID" disable-output-escaping="yes"/>
			<xsl:text disable-output-escaping="yes">"</xsl:text>
		</xsl:if>

		<!-- Add the "href" attribute: href="..." <a -->
		<xsl:text disable-output-escaping="yes"> href="</xsl:text>
			<xsl:value-of select="$item/itemLink" disable-output-escaping="yes"/>
		<xsl:text disable-output-escaping="yes">"</xsl:text>

		<!-- Add the "title" attribute: title="..." <a -->
		<xsl:text disable-output-escaping="yes"> title="</xsl:text>
			<xsl:value-of select="$item/itemToolTip" disable-output-escaping="yes"/>
		<xsl:text disable-output-escaping="yes">"</xsl:text>

		<!-- Close the opening "a" element: > -->
		<xsl:text disable-output-escaping="yes">&gt;</xsl:text>

		<!-- Add the URL text -->
		<xsl:value-of select="$item/itemTitle" disable-output-escaping="yes"/>

		<!-- Close the "a" element: </a> -->
		<xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
	</xsl:template>
</xsl:stylesheet>