<%@ Register TagPrefix="dnn" TagName="Label" Src="~/conTRols/LabelConTRol.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="base.ascx.vb" Inherits="DONEIN_NET.Dictionary.Base" %>

<DIV STYLE="TEXT-ALIGN: left">
	<BR />
	<ASP:LINKBUTTON RUNAT="server" ID="btn_create" STYLE="padding-right: 10px;" />
	<SPAN RUNAT="server" ID="spn_divider" STYLE="FONT-WEIGHT: bold"></SPAN>
	<ASP:LINKBUTTON RUNAT="server" ID="btn_moderate" STYLE="padding-right: 10px;" />
	<BR />
	<BR />
	<TABLE RUNAT="server" ID="tbl_search" BORDER="0" CELLPADDING="0" CELLSPACING="0">
		<TR RUNAT="server" ID="tr_search_category">
			<TD ALIGN="left" VALIGN="top" STYLE="padding-bottom: 5px;">
				<ASP:LABEL RUNAT="server" ID="lbl_ddl_category" RESOURCEKEY="lbl_ddl_category" /><BR />
				<ASP:DROPDOWNLIST RUNAT="server" ID="ddl_category" NAME="ddl_category" AUTOPOSTBACK="True" />		
			</TD>
		</TR>
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<ASP:LABEL RUNAT="server" ID="lbl_ipt_keyword" RESOURCEKEY="lbl_ipt_keyword" /><BR />
				<INPUT RUNAT="server" ID="ipt_keyword" NAME="keyword" ONKEYUP="send_query(this.value)" div_results="off" >
			</TD>
		</TR>
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<DIV ID="div_results" ALIGN="left" STYLE="width: 100%">
		
				</DIV>
				<ASP:LITERAL RUNAT="server" ID="ltr_script" />	
			</TD>
		</TR>
	</TABLE>
	
	
	

	<TABLE RUNAT="server" ID="tbl_edit" BORDER="0" CELLPADDING="0" CELLSPACING="0">
		<TR RUNAT="server" ID="tr_edit_category">
			<TD ALIGN="left" VALIGN="top">
				<DNN:LABEL RUNAT="server" ID="pl_ddl_category_edit" CONTROLNAME="ddl_category_edit" SUFFIX=":" />
				<ASP:DROPDOWNLIST RUNAT="server" ID="ddl_category_edit" CSSCLASS="NormalText" />	
			</TD>
		</TR>	
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<DNN:LABEL RUNAT="server" ID="pl_txt_key" CONTROLNAME="txt_key" SUFFIX=":" />
				<ASP:TEXTBOX RUNAT="server" ID="txt_key" CSSCLASS="NormalText" />	
			</TD>
		</TR>
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<DNN:LABEL RUNAT="server" ID="pl_txt_value" CONTROLNAME="txt_value" SUFFIX=":" />
				<ASP:TEXTBOX RUNAT="server" ID="txt_value" CSSCLASS="NormalText" />	
			</TD>
		</TR>	
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<INPUT TYPE="hidden" RUNAT="server" ID="txt_ID" NAME="txt_ID">
				<BR >
				<ASP:LINKBUTTON RUNAT="server" ID="btn_update" RESOURCEKEY="pl_btn_update" CSSCLASS="CommandButton" CAUSESVALIDATION="true" />
				<SPAN STYLE="FONT-WEIGHT: bold">&nbsp;&nbsp;|&nbsp;&nbsp;</SPAN>
				<ASP:LINKBUTTON RUNAT="server" ID="btn_cancel" RESOURCEKEY="pl_btn_cancel" CSSCLASS="CommandButton" CAUSESVALIDATION="false" />
			</TD>
		</TR>
	</TABLE>
	
</DIV>
