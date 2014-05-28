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
</xsl:stylesheet>