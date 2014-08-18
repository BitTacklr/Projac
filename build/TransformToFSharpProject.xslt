<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet 
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ns="http://schemas.microsoft.com/developer/msbuild/2003"
	xmlns="http://schemas.microsoft.com/developer/msbuild/2003"
	exclude-result-prefixes="ns">

	<xsl:output omit-xml-declaration="no" indent="yes" method="xml"/>

	<xsl:template match="node()|@*">
      <xsl:copy>
         <xsl:apply-templates select="node()|@*"/>
      </xsl:copy>
    </xsl:template>

  <xsl:template match='ns:Project/ns:PropertyGroup/ns:DefineConstants'>
		<DefineConstants><xsl:value-of select="text()"/>;FSHARP</DefineConstants>
	</xsl:template>
	<xsl:template match="ns:Project/ns:ItemGroup/ns:Reference[@Include='Microsoft.CSharp']">
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="FSharp.Core, Version=4.3.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL" />
	</xsl:template>
  <xsl:template match="ns:Project/ns:ItemGroup/ns:ProjectReference/@Include">
    <xsl:attribute name="Include">
      <xsl:value-of select="concat(substring-before(.,'.csproj'),'.FSharp.csproj')"/>
    </xsl:attribute>
  </xsl:template>
  <xsl:template match="ns:Project/ns:ItemGroup/ns:Reference[@Include='TSqlClient']">
    <Reference>
      <xsl:attribute name="Include">
        <xsl:value-of select="'TSqlClient.FSharp'"/>
      </xsl:attribute>
      <HintPath><xsl:call-template name="replace-string">
        <xsl:with-param name="text" select="ns:HintPath/text()"/>
        <xsl:with-param name="replace" select="'packages\TSqlClient'"/>
        <xsl:with-param name="with" select="'packages\TSqlClient.FSharp'"/>
    </xsl:call-template></HintPath>
    </Reference>
  </xsl:template>
  <xsl:template name="replace-string">
    <xsl:param name="text"/>
    <xsl:param name="replace"/>
    <xsl:param name="with"/>
    <xsl:choose>
      <xsl:when test="contains($text,$replace)">
        <xsl:value-of select="substring-before($text,$replace)"/>
        <xsl:value-of select="$with"/>
        <xsl:call-template name="replace-string">
          <xsl:with-param name="text" select="substring-after($text,$replace)"/>
          <xsl:with-param name="replace" select="$replace"/>
          <xsl:with-param name="with" select="$with"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>