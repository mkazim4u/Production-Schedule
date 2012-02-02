<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/root">

      <html>
        <head>
          <title>Commande en ligne</title>
<style type="text/css">
body table td {
    font-family: Trebuchet MS;
    font-size: 13px;
    color: black;
}
.emailheader {
    font-family: Arial, Helvetica;
    font-size: 22px;
    font-weight: bold;
    color: #656565;
}
.cartheader {
	color: white; 
	font-size: 11px;
  font-family: Arial;
	font-weight: bold;
	background-color: #656565;
}
A:link, A:visited, A:active {
    text-decoration: underline;
    color: black;
}
A:hover {
    text-decoration: underline;
    color: #ff3300;
}
</style>
        </head>
        <body>

		<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="512" border="0">
        <TR>
          <TD align="center">
            <font class="emailheader">Orders Report</font>			
          </TD>
        </TR>
      </TABLE>
<p></p>

<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="10" /><xsl:with-param name="StatusTitle" select="'Created'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="20" /><xsl:with-param name="StatusTitle" select="'Redirected to Bank'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="30" /><xsl:with-param name="StatusTitle" select="'Cancelled'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="40" /><xsl:with-param name="StatusTitle" select="'Payed'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="45" /><xsl:with-param name="StatusTitle" select="'Payed Unverified'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="50" /><xsl:with-param name="StatusTitle" select="'Shipped'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="60" /><xsl:with-param name="StatusTitle" select="'On Hold'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="70" /><xsl:with-param name="StatusTitle" select="'Closed'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="80" /><xsl:with-param name="StatusTitle" select="'Awaiting Cheque'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="90" /><xsl:with-param name="StatusTitle" select="'Awaiting Payment'" /></xsl:call-template>
<xsl:call-template name="GetList"><xsl:with-param name="StatusID" select="100" /><xsl:with-param name="StatusTitle" select="'Awaiting Stock'" /></xsl:call-template>


        </body>
      </html>
   
    </xsl:template>

<xsl:template name="GetList">
<xsl:param name="StatusID" />
<xsl:param name="StatusTitle" />


<xsl:variable name="filteredData" select="/root/NB_Store_OrdersInfo[OrderStatusID=$StatusID]" />

<xsl:if test='count($filteredData)>0'>
		
		<TABLE cellSpacing="0" cellPadding="0" width="512" border="0">
        <TR>
          <TD align="left">
            <b><xsl:value-of select="$StatusTitle" /></b>
          </TD>
        </TR>
      </TABLE>
      <TABLE cellSpacing="0" cellPadding="5" width="512" border="0">
        <TR>
          <TD class="cartheader" align="center">Date</TD>
          <TD class="cartheader" align="center">Ref</TD>
          <TD class="cartheader" align="center">Total</TD>
        </TR>
        
        <xsl:for-each select="$filteredData">
		
		<xsl:if test='OrderStatusID=$StatusID'>
		 
        <TR>
          <TD>
                <xsl:value-of select="substring(OrderDate,1,10)" disable-output-escaping="yes" />
          </TD>
          <TD>
                <xsl:value-of select="OrderNumber" disable-output-escaping="yes" />
          </TD>
          <TD align="right">
            <xsl:value-of select="format-number(Total,'0.00')" disable-output-escaping="yes" />
          </TD>
        </TR>
		
		</xsl:if>
		
        </xsl:for-each>

      </TABLE>

		</xsl:if>

</xsl:template>
</xsl:stylesheet>
