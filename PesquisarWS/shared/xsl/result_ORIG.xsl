<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://mycompany.com/mynamespace"
                version="1.0">
	<xsl:output method="html" version="1.0"/>
	<xsl:param name="xpCPF" select="0"/>
	<msxsl:script language="JScript" implements-prefix="user">
		function returnBackground(p_intNumero){
			return ((p_intNumero / 2).toString().indexOf(".") != -1);
		}
	</msxsl:script>
	<xsl:preserve-space elements="code"/>
	<xsl:strip-space elements="text"/>
	<xsl:template match="/">
		<table border="0" align="center" class="corpo" width="100%" cellspacing="0" cellpadding="0">
			<tr>
				<td><br></br>
					<div id="divAll">
						<table align="left" border="0" cellspacing="1" cellpadding="4" width="100%" class="corpo">
							<thead>
								<tr id="trTitulo">
									<td class="tblTitulo" OnClick="javascript:refreshXSL('DDD', 'number');" 	style="cursor:hand;"  nowrap="true">DDD</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('FONE', 'number');"	style="cursor:hand;"  nowrap="true">Fone</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('NOME');"				style="cursor:hand;"  nowrap="true">Nome</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('TIPO');"				style="cursor:hand;"  nowrap="true">Tipo</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('ENDERECO');"			style="cursor:hand;"  nowrap="true">Endereço</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('NUMERO', 'number');"	style="cursor:hand;"  nowrap="true">Número</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('COMPL');"				style="cursor:hand;"  nowrap="true">Compl</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('BAIRRO');"			style="cursor:hand;"  nowrap="true">Bairro</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('CIDADE');"			style="cursor:hand;"  nowrap="true">Cidade</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('UF');"				style="cursor:hand;"  nowrap="true">UF</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('CEP', 'number');"		style="cursor:hand;"  nowrap="true">CEP</td>
									<td class="tblTitulo" OnClick="javascript:refreshXSL('CPF', 'number');"		style="cursor:hand;"  nowrap="true">CPF/CNPJ</td>
								</tr>
							</thead>
							<tBody>
								<xsl:apply-templates select="//XML">
									<xsl:sort select="@FieldSort" order="descending"/>
								</xsl:apply-templates>
							</tBody>
							<tfoot>
								<tr>
									<td colspan="12" class="Alerta" align="center"><div id="divErro" class="erro"></div></td>
								</tr>
							</tfoot>
						</table>
					</div>
				</td>
			</tr>
		</table>
	</xsl:template>
	<xsl:template match="XML">
		<tr valign="top" OnMouseOver="paintLine(this, true);" onmouseout="paintLine(this, false);">
			<xsl:attribute name="ondblclick">javascript:setFormAll(this);</xsl:attribute>
			<xsl:choose>
				<xsl:when test="@CPF=$xpCPF">
					<xsl:attribute name="class">FundoDestaque</xsl:attribute>
				</xsl:when>
				<xsl:when test="user:returnBackground(position())">
					<xsl:attribute name="class">FundoSim</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="class">FundoNao</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<td align="left" class="hand" id="tdDDD">
				<xsl:attribute name="onclick">javascript:setForm('DDD','<xsl:value-of select="@DDD"/>');</xsl:attribute>
				<xsl:value-of select="@DDD"/>
			</td>
			<td align="left" class="hand" id="tdFone">
				<xsl:attribute name="onclick">javascript:setForm('Fone','<xsl:value-of select="@FONE"/>');</xsl:attribute>
				<xsl:value-of select="@FONE"/>
			</td>
			<td align="left" class="hand" id="tdNome">
				<xsl:attribute name="onclick">javascript:setForm('Nome','<xsl:value-of select="@NOME"/>');</xsl:attribute>
				<xsl:value-of select="@NOME"/>
			</td>
			<td align="left" class="hand" id="tdTipo">
				<xsl:value-of select="@TIPO"/>
			</td>
			<td align="left" class="hand" id="tdEndereco">
				<xsl:attribute name="onclick">javascript:setForm('Endereco','<xsl:value-of select="@ENDERECO"/>');</xsl:attribute>
				<xsl:value-of select="@ENDERECO"/>
			</td>
			<td align="left" class="hand" id="tdNumero">
				<xsl:attribute name="onclick">javascript:setForm('Numero','<xsl:value-of select="@NUMERO"/>');</xsl:attribute>
				<xsl:value-of select="@NUMERO"/>
			</td>
			<td align="left" class="hand" id="tdCompl">
				<xsl:attribute name="onclick">javascript:setForm('Complemento','<xsl:value-of select="@COMPLEMENTO"/>');</xsl:attribute>
				<xsl:value-of select="@COMPLEMENTO"/>
			</td>
			<td align="left" class="hand" id="tdBairro">
				<xsl:attribute name="onclick">javascript:setForm('Bairro','<xsl:value-of select="@BAIRRO"/>');</xsl:attribute>
				<xsl:value-of select="@BAIRRO"/>
			</td>
			<td align="left" class="hand" id="tdCidade">
				<xsl:attribute name="onclick">javascript:setForm('Cidade','<xsl:value-of select="@CIDADE"/>');</xsl:attribute>
				<xsl:value-of select="@CIDADE"/>
			</td>
			<td align="left" class="hand" id="tdUF">
				<xsl:attribute name="onclick">javascript:setForm('UF','<xsl:value-of select="@UF"/>');</xsl:attribute>
				<xsl:value-of select="@UF"/>
			</td>
			<td align="left" class="hand" id="tdCEP">
				<xsl:attribute name="onclick">javascript:setForm('CEP','<xsl:value-of select="@CEP"/>');</xsl:attribute>
				<xsl:value-of select="@CEP"/>
			</td>
			<td align="left" class="hand" id="tdCPF">
				<xsl:attribute name="onclick">javascript:setForm('CPF','<xsl:value-of select="@CPF"/>');</xsl:attribute>
				<xsl:value-of select="@CPF"/>
			</td>
		</tr>	
	</xsl:template>
</xsl:stylesheet>