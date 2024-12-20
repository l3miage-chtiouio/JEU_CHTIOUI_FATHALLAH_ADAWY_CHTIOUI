<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:template match="/">
        <html>
            <head>
                <title>Scores</title>
                <style>
                    table { border-collapse: collapse; width: 50%; margin: 20px auto; }
                    th, td { border: 1px solid black; padding: 8px; text-align: center; }
                    th { background-color: #f2f2f2; }
                </style>
            </head>
            <body>
                <h1>Liste des Scores</h1>
                <table>
                    <tr>
                        <th>Score</th>
                        <th>Date</th>
                    </tr>

                    <!-- Parcourir les scores et les afficher -->
                    <xsl:for-each select="//Score">
                        <tr>
                            <td><xsl:value-of select="CurrentScore" /></td>
                            <td><xsl:value-of select="Date" /></td>
                        </tr>
                    </xsl:for-each>

                </table>
            </body>
        </html>
    </xsl:template>

</xsl:stylesheet>
