<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="moderate.ascx.vb" Inherits="DONEIN_NET.Dictionary.Moderate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<DIV STYLE="TEXT-ALIGN: left">


	<TABLE RUNAT="server" ID="tbl_search" BORDER="0" CELLPADDING="0" CELLSPACING="0">
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<BR />
				<ASP:LINKBUTTON RUNAT="server" ID="btn_create" />
				<BR />
				<BR />
				<DNN:LABEL RUNAT="server" ID="pl_txt_key_filter" CONTROLNAME="txt_key_filter" SUFFIX=":" />
				<ASP:TEXTBOX RUNAT="server" ID="txt_key_filter" CSSCLASS="NormalText" AUTOPOSTBACK="True" WIDTH="360" />
				<BR />	
				<DNN:LABEL RUNAT="server" ID="pl_ddl_category" CONTROLNAME="ddl_category" SUFFIX=":" />
				<ASP:DROPDOWNLIST RUNAT="server" ID="ddl_category" CSSCLASS="NormalText" AUTOPOSTBACK="True" WIDTH="360" />
				<BR />	
				<DNN:LABEL RUNAT="server" ID="pl_ddl_status" CONTROLNAME="ddl_status" SUFFIX=":" />
				<ASP:DROPDOWNLIST RUNAT="server" ID="ddl_status" CSSCLASS="NormalText" AUTOPOSTBACK="True" WIDTH="360" />
				<ASP:DATAGRID RUNAT="server" ID="dg_dictionary_entry" WIDTH="100%" BORDERWIDTH="0" CELLPADDING="2" CELLSPACING="1" AUTOGENERATECOLUMNS="False" ALLOWPAGING="False" ALLOWSORTING="False"  >
					<HEADERSTYLE CSSCLASS="HEADER" FONT-BOLD="True" HORIZONTALALIGN="Left" VERTICALALIGN="Top" />
					<ITEMSTYLE BACKCOLOR="#FEFEFE" HORIZONTALALIGN="Left" VERTICALALIGN="Top" />
					<ALTERNATINGITEMSTYLE BACKCOLOR="#EEEEEE" HORIZONTALALIGN="Left" VERTICALALIGN="Top" />
					<COLUMNS>
						<ASP:BOUNDCOLUMN DATAFIELD="ID" READONLY="True" HEADERTEXT="" ITEMSTYLE-WIDTH="30" ITEMSTYLE-HORIZONTALALIGN="Right" VISIBLE="False" />
						<ASP:BOUNDCOLUMN DATAFIELD="vch_key" HEADERTEXT="pl_key" ITEMSTYLE-WIDTH="120" />
						<ASP:BOUNDCOLUMN DATAFIELD="vch_value" HEADERTEXT="pl_value" ITEMSTYLE-WIDTH="260" />
						<ASP:TEMPLATECOLUMN HEADERTEXT="pl_author" ITEMSTYLE-WIDTH="120" ITEMSTYLE-WRAP="False"> 
							<ITEMTEMPLATE>
								<A HREF='mailto:<%# DataBinder.Eval(Container, "DataItem.vch_email") %>' TARGET="_blank"><%# DataBinder.Eval(Container, "DataItem.vch_name_first") + "&nbsp;" + DataBinder.Eval(Container, "DataItem.vch_name_last") %></A>
							</ITEMTEMPLATE> 
						</ASP:TEMPLATECOLUMN>
						<ASP:TEMPLATECOLUMN ITEMSTYLE-HORIZONTALALIGN="Right" ITEMSTYLE-WIDTH="58" ITEMSTYLE-WRAP="False"> 
							<ITEMTEMPLATE> 
								<ASP:LINKBUTTON RUNAT="server" ID="btn_dg_approve" COMMANDNAME="approve" COMMANDARGUMENT='<%# DataBinder.Eval(Container, "DataItem.ID") %>' VISIBLE='<%# Me.int_to_bln(DataBinder.Eval(Container, "DataItem.int_status"),True) %>'><IMG SRC="images/add.gif" BORDER="0" WIDTH="16" HEIGHT="16"></ASP:LINKBUTTON>
								<ASP:LINKBUTTON RUNAT="server" ID="btn_dg_edit" COMMANDNAME="edit" COMMANDARGUMENT='<%# DataBinder.Eval(Container, "DataItem.ID") %>'><IMG SRC="images/edit.gif" BORDER="0" WIDTH="16" HEIGHT="16"></ASP:LINKBUTTON>
								<ASP:LINKBUTTON RUNAT="server" ID="btn_dg_delete" COMMANDNAME="delete" COMMANDARGUMENT='<%# DataBinder.Eval(Container, "DataItem.ID") %>'><IMG SRC="images/delete.gif" BORDER="0" WIDTH="16" HEIGHT="16"></ASP:LINKBUTTON>
							</ITEMTEMPLATE> 
						</ASP:TEMPLATECOLUMN>
					</COLUMNS>
				</ASP:DATAGRID>
			</TD>
		</TR>
	</TABLE>

	<TABLE RUNAT="server" ID="tbl_edit" BORDER="0" CELLPADDING="0" CELLSPACING="0">
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<BR />
				<BR />
				<DNN:LABEL RUNAT="server" ID="pl_ddl_category_edit" CONTROLNAME="ddl_category_edit" SUFFIX=":" />
				<ASP:DROPDOWNLIST RUNAT="server" ID="ddl_category_edit" CSSCLASS="NormalText" WIDTH="360" />	
			</TD>
		</TR>	
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<DNN:LABEL RUNAT="server" ID="pl_txt_key" CONTROLNAME="txt_key" SUFFIX=":" />
				<ASP:TEXTBOX RUNAT="server" ID="txt_key" CSSCLASS="NormalText" WIDTH="360" />	
			</TD>
		</TR>
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<DNN:LABEL RUNAT="server" ID="pl_txt_value" CONTROLNAME="txt_value" SUFFIX=":" />
				<ASP:TEXTBOX RUNAT="server" ID="txt_value" CSSCLASS="NormalText" WIDTH="360" />	
			</TD>
		</TR>	
		<TR>
			<TD ALIGN="left" VALIGN="top">
				<INPUT TYPE="hidden" RUNAT="server" ID="txt_ID" NAME="txt_ID">
				<INPUT TYPE="hidden" RUNAT="server" ID="txt_author" NAME="txt_author">
				<BR >
				<ASP:LINKBUTTON RUNAT="server" ID="btn_update" RESOURCEKEY="pl_btn_update" CSSCLASS="CommandButton" CAUSESVALIDATION="true" />
				<SPAN STYLE="FONT-WEIGHT: bold">&nbsp;&nbsp;|&nbsp;&nbsp;</SPAN>
				<ASP:LINKBUTTON RUNAT="server" ID="btn_cancel" RESOURCEKEY="pl_btn_cancel" CSSCLASS="CommandButton" CAUSESVALIDATION="false" />
			</TD>
		</TR>
	</TABLE>
</DIV>
