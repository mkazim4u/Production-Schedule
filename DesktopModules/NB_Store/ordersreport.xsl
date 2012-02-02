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
            .bluefont {
            font-family: Arial, Helvetica;
            font-size: 22px;
            font-weight: bold;
            color: #045ba4;
            }
            A:link, A:visited, A:active {
            text-decoration: underline;
            color: black;
            }
            A:hover {
            text-decoration: underline;
            color: #ff3300;
            }
            .small {
            font-size: 10px;
            }
          </style>
        </head>
        <body>
      
      <TABLE id="Table1" style="WIDTH: 512px; HEIGHT: 88px" cellSpacing="1" cellPadding="1" width="512"
  border="0">
        <TR>
          <TD width="248" height="23" valign="top">
          </TD>
          <TD height="23" align="right">
            <font class="bluefont">Rapport<br/>Commande en ligne</font>
          </TD>
        </TR>
      </TABLE>
      <P>
      </P>
      <TABLE id="Table3" style="WIDTH: 512px; HEIGHT: 96px" cellSpacing="0" cellPadding="6" width="512"
        border="0">
        <TR vAlign="top">
          <TD width="100" bgColor="#e6e6e6" height="35">
            <P align="center">
              <B>Date</B>
            </P>
          </TD>
          <TD width="300" bgColor="#e6e6e6" height="35">
            <P align="center">
              <B>Ref</B>
            </P>
          </TD>
          <TD width="71" bgColor="#e6e6e6" height="35">
            <P align="center">
              <B>Total</B>
            </P>
          </TD>
        </TR>

        <xsl:for-each select="/root/NB_Store_OrdersInfo">
          
        <TR vAlign="top">
          <TD width="100">
            <P>
              <B>
                <xsl:value-of select="substring(OrderDate,1,10)" disable-output-escaping="yes" />
              </B>
            </P>
          </TD>
          <TD width="300">
            <P>
              <B>
                <xsl:value-of select="OrderNumber" disable-output-escaping="yes" /></B>
            </P>
          </TD>
          <TD width="71" align="right">
            <xsl:value-of select="format-number(Total,'0.00')" disable-output-escaping="yes" />
          </TD>
        </TR>

        </xsl:for-each>

      </TABLE>

        </body>
      </html>
   
    </xsl:template>

</xsl:stylesheet>
