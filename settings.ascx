<%@ Control Language="vb" AutoEventWireup="false" Codebehind="settings.ascx.vb" Inherits="DONEIN_NET.Dictionary.Settings" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/conTRols/LabelConTRol.ascx" %>

<DIV STYLE="text-align: left;">

	<ASP:LABEL RUNAT="server" ID="lbl_message" CSSCLASS="NormalRed" STYLE="padding-left: 10px;" />

	<TABLE WIDTH="480" BORDER="0" CELLPADDING="2" CELLSPACING="1">
		<TR VALIGN="top">
			<TD WIDTH="180px" ALIGN="left">
				<DNN:LABEL RUNAT="server" ID="pl_ddl_role_moderate" CONTROLNAME="ddl_role_moderate" SUFFIX=":" />
			</TD>	
			<TD ALIGN="left">
				<ASP:DROPDOWNLIST RUNAT="server" ID="ddl_role_moderate" CSSCLASS="NormalText" WIDTH="300px" />
			</TD>
		</TR>
		<TR VALIGN="top">
			<TD WIDTH="180px" ALIGN="left">
				<DNN:LABEL RUNAT="server" ID="pl_ddl_result_limit" CONTROLNAME="ddl_result_limit" SUFFIX=":" />
			</TD>	
			<TD ALIGN="left">
				<ASP:DROPDOWNLIST RUNAT="server" ID="ddl_result_limit" CSSCLASS="NormalText" WIDTH="300px" STYLE="text-align: right;" />
			</TD>
		</TR>
		<TR VALIGN="top">
			<TD WIDTH="180px" ALIGN="left">
				<DNN:LABEL RUNAT="server" ID="pl_txt_form_width" CONTROLNAME="txt_form_width" SUFFIX=":" />
			</TD>	
			<TD ALIGN="left">
				<ASP:TEXTBOX RUNAT="server" ID="txt_form_width" CSSCLASS="NormalText" WIDTH="36px" MAXLENGTH="3" STYLE="text-align: right;" />
			</TD>
		</TR>
		<TR RUNAT="server" ID="Tr1" VALIGN="top">
			<TD WIDTH="180px" ALIGN="left">
				<DNN:LABEL RUNAT="server" ID="pl_rad_enable_submission" CONTROLNAME="rad_enable_submission" SUFFIX=":" />
			</TD>	
			<TD ALIGN="left">
				<ASP:RADIOBUTTONLIST RUNAT="server" ID="rad_enable_submission" CSSCLASS="NormalText" REPEATDIRECTION="Horizontal" />
			</TD>
		</TR>
		<TR RUNAT="server" ID="tr_enable_category_selection" VALIGN="top">
			<TD WIDTH="180px" ALIGN="left">
				<DNN:LABEL RUNAT="server" ID="pl_rad_enable_category_selection" CONTROLNAME="rad_enable_category_selection" SUFFIX=":" />
			</TD>	
			<TD ALIGN="left">
				<ASP:RADIOBUTTONLIST RUNAT="server" ID="rad_enable_category_selection" CSSCLASS="NormalText" REPEATDIRECTION="Horizontal" />
			</TD>
		</TR>
		<TR RUNAT="server" ID="tr_default_category" VALIGN="top">
			<TD WIDTH="180px" ALIGN="left">
				<DNN:LABEL RUNAT="server" ID="pl_ddl_default_category" CONTROLNAME="ddl_default_category" SUFFIX=":" />
			</TD>	
			<TD ALIGN="left">
				<ASP:DROPDOWNLIST RUNAT="server" ID="ddl_default_category" CSSCLASS="NormalText" WIDTH="300px" />
			</TD>
		</TR>
		<TR VALIGN="top">
			<TD WIDTH="180px" ALIGN="left">
				<DNN:LABEL RUNAT="server" ID="pl_txt_email_sender" CONTROLNAME="txt_email_sender" SUFFIX=":" />
			</TD>	
			<TD ALIGN="left">
				<ASP:TEXTBOX RUNAT="server" ID="txt_email_sender" CSSCLASS="NormalText" WIDTH="300px" MAXLENGTH="512" />
			</TD>
		</TR>
		<TR VALIGN="top">
			<TD WIDTH="180px" ALIGN="left">
				<DNN:LABEL RUNAT="server" ID="pl_txt_email_moderator" CONTROLNAME="txt_email_moderator" SUFFIX=":" />
			</TD>	
			<TD ALIGN="left">
				<ASP:TEXTBOX RUNAT="server" ID="txt_email_moderator" CSSCLASS="NormalText" WIDTH="300px" MAXLENGTH="512"  />
			</TD>
		</TR>
		<TR VALIGN="top">
			<TD ALIGN="left">
				&nbsp;
			</TD>	
			<TD ALIGN="left">
				<BR />
				<ASP:LINKBUTTON RUNAT="server" ID="btn_update" RESOURCEKEY="pl_btn_update" CSSCLASS="CommandButton" CAUSESVALIDATION="true" />
				<SPAN STYLE="font-weight: bold;">&nbsp;&nbsp;|&nbsp;&nbsp;</SPAN>
				<ASP:LINKBUTTON RUNAT="server" ID="btn_cancel" RESOURCEKEY="pl_btn_cancel" CSSCLASS="CommandButton" CAUSESVALIDATION="false" />
			</TD>
		</TR>
	</TABLE>

</DIV>