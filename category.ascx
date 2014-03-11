<%@ Control Language="vb" AutoEventWireup="false" Codebehind="category.ascx.vb" Inherits="DONEIN_NET.Dictionary.Category" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<DIV STYLE="text-align: left;">
	<BR />
	<ASP:LINKBUTTON RUNAT="server" ID="btn_create" />
	<BR />
	<ASP:DATAGRID RUNAT="server" ID="dg_category" WIDTH="480" BORDERWIDTH="0" CELLPADDING="2" CELLSPACING="1" AUTOGENERATECOLUMNS="False" ALLOWPAGING="False" ALLOWSORTING="False" >
		<HEADERSTYLE CSSCLASS="HEADER" FONT-BOLD="True" HORIZONTALALIGN="Left" VERTICALALIGN="Top" />
		<ITEMSTYLE BACKCOLOR="#FEFEFE" HORIZONTALALIGN="Left" VERTICALALIGN="Top" />
		<ALTERNATINGITEMSTYLE BACKCOLOR="#EEEEEE" HORIZONTALALIGN="Left" VERTICALALIGN="Top" />
		<COLUMNS>
			<ASP:BOUNDCOLUMN DATAFIELD="ID" READONLY="True" HEADERTEXT="" ITEMSTYLE-WIDTH="30" ITEMSTYLE-HORIZONTALALIGN="Right" VISIBLE="False" />
			<ASP:BOUNDCOLUMN DATAFIELD="vch_category" HEADERTEXT="pl_category" />
			<ASP:TEMPLATECOLUMN ITEMSTYLE-HORIZONTALALIGN="Right" ITEMSTYLE-WIDTH="54"> 
				<ITEMTEMPLATE> 
					<ASP:LINKBUTTON RUNAT="server" ID="btn_dg_edit" COMMANDNAME="edit" COMMANDARGUMENT='<%# DataBinder.Eval(Container, "DataItem.ID") %>'><IMG SRC="images/edit.gif" BORDER="0" WIDTH="16" HEIGHT="16"></ASP:LINKBUTTON>
					<ASP:LINKBUTTON RUNAT="server" ID="btn_dg_delete" COMMANDNAME="delete" COMMANDARGUMENT='<%# DataBinder.Eval(Container, "DataItem.ID") %>'><IMG SRC="images/delete.gif" BORDER="0" WIDTH="16" HEIGHT="16"></ASP:LINKBUTTON>
				</ITEMTEMPLATE> 
			</ASP:TEMPLATECOLUMN>
		</COLUMNS>
	</ASP:DATAGRID>

	<TABLE RUNAT="server" ID="tbl_category_edit" BORDER="0" WIDTH="480" CELLPADDING="3" CELLSPACING="1">
		<TR VALIGN="top">
			<TD WIDTH="120" CLASS="SubHead" ALIGN="left" VALIGN="top">
				<DNN:LABEL RUNAT="server" ID="pl_category" CONTROLNAME="txt_category" SUFFIX=":" />
			</TD>
			<TD WIDTH="360" ALIGN="left" VALIGN="top">
				<ASP:TEXTBOX RUNAT="server" ID="txt_category" CSSCLASS="NormalText" WIDTH="360" MAXLENGTH="256" />							
			</TD>        
		</TR>
		<TR VALIGN="top">
			<TD WIDTH="120" CLASS="SubHead" ALIGN="left" VALIGN="top">
				<INPUT TYPE="hidden" RUNAT="server" ID="txt_ID" NAME="txt_ID"/>
			</TD>
			<TD WIDTH="360" ALIGN="left" VALIGN="top">
				<ASP:LINKBUTTON RUNAT="server" ID="btn_update" />
				<SPAN STYLE="font-weight: bold;">&nbsp;&nbsp;|&nbsp;&nbsp;</SPAN>	
				<ASP:LINKBUTTON RUNAT="server" ID="btn_cancel" />
			</TD>        
		</TR>
	</TABLE>
</DIV>

