Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security
Imports DotNetNuke.Security.Roles
Imports System.Text



Namespace DONEIN_NET.Dictionary

    Public MustInherit Class Base
    	Inherits Entities.Modules.PortalModuleBase
		Implements Entities.Modules.IActionable
		'Implements Entities.Modules.IPortable
		Implements Entities.Modules.ISearchable



		#Region " Declare: Shared Classes "

			Private database As New database(System.Configuration.ConfigurationSettings.AppSettings("SiteSqlServer"), "SqlClient")
			Private mail As New Mail()
			Private module_info As New Module_Info()
			
		#End Region

		

		#Region " Declare: Local Objects "
		
			Protected WithEvents btn_create As System.Web.UI.WebControls.LinkButton
			Protected WithEvents spn_divider As System.Web.UI.HtmlControls.HtmlGenericControl
			Protected WithEvents btn_moderate As System.Web.UI.WebControls.LinkButton
			Protected WithEvents btn_update As System.Web.UI.WebControls.LinkButton
			Protected WithEvents btn_cancel As System.Web.UI.WebControls.LinkButton
			
			Protected WithEvents tbl_search As System.Web.UI.HtmlControls.HtmlTable
			Protected WithEvents tr_search_category As System.Web.UI.HtmlControls.HtmlTableRow
			Protected WithEvents lbl_ddl_category As System.Web.UI.WebControls.Label
			Protected WithEvents ddl_category As System.Web.UI.WebControls.DropDownList
			Protected WithEvents lbl_ipt_keyword As System.Web.UI.WebControls.Label
			Protected WithEvents ipt_keyword As System.Web.UI.HtmlControls.HtmlInputText
			Protected WithEvents ltr_script As System.Web.UI.WebControls.Literal
			
			Protected WithEvents tbl_edit As System.Web.UI.HtmlControls.HtmlTable
			Protected WithEvents tr_edit_category As System.Web.UI.HtmlControls.HtmlTableRow
			Protected WithEvents ddl_category_edit As System.Web.UI.WebControls.DropDownList
			Protected WithEvents txt_key As System.Web.UI.WebControls.TextBox
			Protected WithEvents txt_value As System.Web.UI.WebControls.TextBox
			Protected WithEvents txt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
		
			Private obj_user As New UserController
			Private obj_user_info As UserInfo = obj_user.GetCurrentUserInfo
			
			Private tmp_page_url As String	
			Protected str_script As String
			Protected bln_moderator As Boolean = False
			
		#End Region


		
		#Region " Page: Load "
		
			Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
				
				If Request.QueryString.Item("debug") <> "" Then
					module_info.get_info(Request.QueryString.Item("debug").Trim, ModuleID, TabID)
				End If
			
				Try
					If Not Page.IsPostBack Then
					
						tbl_edit.Visible = False
					
						Dim tmp_dictionary_role_moderate As Integer = CType(Settings("donein_dictionary_role_moderate"), Integer)
						Dim tmp_dictionary_result_limit As Integer = CType(Settings("donein_dictionary_result_limit"), Integer)
						Dim tmp_dictionary_form_width As Integer = CType(Settings("donein_dictionary_form_width"), Integer)
						Dim tmp_dictionary_enable_submission As Integer = CType(Settings("donein_dictionary_enable_submission"), Integer)
						Dim tmp_dictionary_enable_category_selection As Integer = CType(Settings("donein_dictionary_enable_category_selection"), Integer)
						Dim tmp_dictionary_default_category As String = CType(Settings("donein_dictionary_default_category"), String)
						
						Dim obj_role As New RoleController
						Dim obj_role_info As RoleInfo 
						
						
						If tmp_dictionary_role_moderate > 0 Or obj_user_info.IsSuperUser = True Then
							obj_role_info = obj_role.GetRole(tmp_dictionary_role_moderate, PortalSettings.PortalId)	
							If PortalSecurity.IsInRole(obj_role_info.RoleName) Then
								bln_moderator = True
								spn_divider.Visible = True
								btn_moderate.Visible = True	
							Else
								bln_moderator = False
								spn_divider.Visible = False
								btn_moderate.Visible = False
							End If
						Else
							bln_moderator = False
							spn_divider.Visible = False
							btn_moderate.Visible = False						
						End If
						
						ddl_category_bind()
						
						'// SET THE WIDTH OF THE FORM ELEMENTS
						If tmp_dictionary_form_width > 0 Then
							tbl_search.Width = tmp_dictionary_form_width.ToString
							ddl_category.Width = System.Web.UI.WebControls.Unit.Parse(tmp_dictionary_form_width.ToString)
							ipt_keyword.Style.Add("width", tmp_dictionary_form_width.ToString + "px")
							tbl_edit.Width = tmp_dictionary_form_width.ToString
							ddl_category_edit.Width = System.Web.UI.WebControls.Unit.Parse(tmp_dictionary_form_width.ToString)
							txt_key.Width = System.Web.UI.WebControls.Unit.Parse(tmp_dictionary_form_width.ToString)
							txt_value.Width = System.Web.UI.WebControls.Unit.Parse(tmp_dictionary_form_width.ToString)
						Else
							tbl_search.Width = "480"
							ddl_category.Width = System.Web.UI.WebControls.Unit.Parse("480")
							ipt_keyword.Style.Add("width", "480px")
							tbl_edit.Width = "480"
							ddl_category_edit.Width = System.Web.UI.WebControls.Unit.Parse("480")
							txt_key.Width = System.Web.UI.WebControls.Unit.Parse("480")
							txt_value.Width = System.Web.UI.WebControls.Unit.Parse("480")
						End If
						
						If tmp_dictionary_enable_category_selection >= 0 Then
							tr_search_category.Visible = True
							tr_edit_category.Visible = True
						Else
							tr_search_category.Visible = False
							tr_edit_category.Visible = False
						End If	
						
						If tmp_dictionary_enable_submission >= 0 Or obj_user_info.IsSuperUser = True Then
							btn_create.Visible = True
						Else
							btn_create.Visible = False
						End If					
						
						If tmp_dictionary_default_category = "" Then
							ddl_category.SelectedIndex = 0
							ddl_category_edit.SelectedIndex = 0
						Else
							ddl_category.SelectedValue = tmp_dictionary_default_category
							ddl_category_edit.SelectedValue = tmp_dictionary_default_category
						End If										
						
						create_script(0, tmp_dictionary_result_limit)
						tmp_page_url = Request.Url.Scheme + Request.Url.SchemeDelimiter + Request.Url.Authority + ResolveUrl("processor.aspx")

						tmp_page_url = ResolveUrl("processor.aspx")
					End If
				Catch ex As Exception
					ProcessModuleLoadException(Me, ex)
				End Try
				
			End Sub
				
		#End Region
		
		
		
		#Region " Page: PreRender "

			Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
				module_localize() '// LOCALIZE THE MODULE
			End Sub

		#End Region



		#Region " Page: Localization "

 			Private Sub module_localize()
 				btn_create.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_btn_create.Text", LocalResourceFile)
 				btn_moderate.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_btn_moderate.Text", LocalResourceFile)
				btn_update.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_btn_update.Text", LocalResourceFile)
				btn_cancel.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_btn_cancel.Text", LocalResourceFile)
			End Sub 

		#End Region

		
		
		#Region " Bind: Category Dropdown Lists (ddl_category, ddl_category_edit) "

			Private Sub ddl_category_bind()

				Dim dt_category As New DataTable
				database.CreateCommand("donein_dictionary_category_R", CommandType.StoredProcedure)
				database.AddParameter("@int_ID", 0)
				database.AddParameter("@int_module", ModuleID)
				database.AddParameter("@int_status", 1)
				database.Execute(dt_category)
				If dt_category.Rows.Count > 0 Then
					ddl_category.DataSource = dt_category
					ddl_category.DataTextField = "vch_category"
					ddl_category.DataValueField = "ID"
					ddl_category.DataBind
					ddl_category.Visible = True
					
					ddl_category_edit.DataSource = dt_category
					ddl_category_edit.DataTextField = "vch_category"
					ddl_category_edit.DataValueField = "ID"
					ddl_category_edit.DataBind
					ddl_category_edit.Visible = True
				End If			
				ddl_category.Items.Insert(0, New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_all_categories.Text", LocalResourceFile), "0"))
				ddl_category_edit.Items.Insert(0, New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_uncategorized.Text", LocalResourceFile), "0"))
			End Sub	
		
		#End Region	
		
		
				
		#Region " Handle: Category Dropdown List Change "

			Private Sub ddl_category_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_category.SelectedIndexChanged
				Dim tmp_dictionary_result_limit As Integer = CType(Settings("donein_dictionary_result_limit"), Integer)
				Dim tmp_category As Integer = CType(ddl_category.SelectedValue, Integer)
				ipt_keyword.Value = ""
				create_script(tmp_category, tmp_dictionary_result_limit) 
			End Sub	
			
		#End Region
		
		
		
		#Region " Create: AJAX Script "
					
			Private Sub create_script(Optional ByVal tmp_category As Integer = 0, Optional ByVal tmp_limit As Integer = 50)
				Dim tmp_url As String
				tmp_url = Request.Url.Scheme + Request.Url.SchemeDelimiter + Request.Url.Authority + ResolveUrl("processor.aspx")
				
				str_script = ""
				str_script += "<SCRIPT LANGUAGE=""JavaScript"">" + vbcrlf 
				str_script += "<!--" + vbcrlf
				str_script += "" + vbcrlf
				str_script += "	var req;" + vbcrlf
				str_script += "" + vbcrlf
				str_script += "function initialize_query()" + vbcrlf
				str_script += "{" + vbcrlf
				str_script += "		try" + vbcrlf
				str_script += "		{" + vbcrlf
				str_script += "			req=new ActiveXObject(""Msxml2.XMLHTTP"");" + vbcrlf
				str_script += "		}" + vbcrlf
				str_script += "		catch(e)" + vbcrlf
				str_script += "		{" + vbcrlf
				str_script += "			try" + vbcrlf
				str_script += "			{" + vbcrlf
				str_script += "				req=new ActiveXObject(""Microsoft.XMLHTTP"");" + vbcrlf
				str_script += "			}" + vbcrlf
				str_script += "			catch(oc)" + vbcrlf
				str_script += "			{" + vbcrlf
				str_script += "				req=null;" + vbcrlf
				str_script += "			}" + vbcrlf
				str_script += "		}" + vbcrlf
				str_script += "		if(!req&&typeof XMLHttpRequest!=""undefined"")" + vbcrlf
				str_script += "		{" + vbcrlf
				str_script += "			req= new" + vbcrlf
				str_script += "			XMLHttpRequest();" + vbcrlf
				str_script += "		}" + vbcrlf
				str_script += "}" + vbcrlf 
				str_script += "" + vbcrlf
				str_script += "function send_query(key)" + vbcrlf
				str_script += "{" + vbcrlf
				str_script += "		initialize_query();" + vbcrlf 
				str_script += "		var url=""" + tmp_url + "?key=""+key+""&category=" + tmp_category.ToString + "&limit=" + tmp_limit.ToString + """;" + vbcrlf
				str_script += "		if(req!=null)" + vbcrlf
				str_script += "		{" + vbcrlf
				str_script += "			req.onreadystatechange = process_query;" + vbcrlf
				str_script += "			req.open(""GET"", url, true);" + vbcrlf
				str_script += "			req.send(null);" + vbcrlf
				str_script += "		}" + vbcrlf
				str_script += "}" + vbcrlf
				str_script += "" + vbcrlf
				str_script += "function process_query()" + vbcrlf
				str_script += "{" + vbcrlf
				str_script += "		if (req.readyState == 4)" + vbcrlf
				str_script += "			{" + vbcrlf
				str_script += "			if (req.status == 200)" + vbcrlf
				str_script += "			{" + vbcrlf
				str_script += "				if(req.responseText=="""")" + vbcrlf
				str_script += "					div_hide(""div_results"");" + vbcrlf
				str_script += "				else" + vbcrlf
				str_script += "				{" + vbcrlf
				str_script += "					div_show(""div_results"");" + vbcrlf
				str_script += "					document.getElementById(""div_results"").innerHTML=req.responseText;" + vbcrlf
				str_script += "				}" + vbcrlf
				str_script += "			}" + vbcrlf
				str_script += "			else" + vbcrlf
				str_script += "			{" + vbcrlf
				str_script += "				document.getElementById(""div_results"").innerHTML=""" + DotNetNuke.Services.Localization.Localization.GetString("pl_message_error.Text", LocalResourceFile).Trim + ": ""+ req.statusText;" + vbcrlf
				str_script += "			}" + vbcrlf
				str_script += "		}" + vbcrlf
				str_script += "}" + vbcrlf
				str_script += "" + vbcrlf
				str_script += "function div_show(div_ID)" + vbcrlf
				str_script += "{" + vbcrlf
				str_script += "		if (document.layers) document.layers[div_ID].visibility=""show"";" + vbcrlf
				str_script += "		else document.getElementById(""div_results"").style.visibility=""visible"";" + vbcrlf
				str_script += "}" + vbcrlf
				str_script += "function div_hide(div_ID)" + vbcrlf
				str_script += "{" + vbcrlf
				str_script += "		if (document.layers) document.layers[div_ID].visibility=""hide"";" + vbcrlf
				str_script += "		else document.getElementById(""div_results"").style.visibility=""hidden"";" + vbcrlf
				str_script += "}" + vbcrlf
				str_script += "" + vbcrlf
				str_script += "// -->" + vbcrlf
				str_script += "</SCRIPT>
				
				ltr_script.Text = str_script
				str_script = NOTHING
				
			End Sub		
		
		#End Region
		
					
		
		#Region " Handle: Update Button (btn_update) "

  			Private Sub btn_update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_update.Click
  				
				If txt_key.Text.Trim.Length > 0 And txt_value.Text.Trim.Length > 0 Then

					Try
							
						Dim tmp_dictionary_role_moderate As Integer = CType(Settings("donein_dictionary_role_moderate"), Integer)
						Dim tmp_dictionary_enable_submission As Integer = CType(Settings("donein_dictionary_enable_submission"), Integer)
							
						Dim obj_role As New RoleController
						Dim obj_role_info As RoleInfo 
								
						tbl_search.Visible = True
						btn_create.Visible = True
								
						If tmp_dictionary_role_moderate > 0 Or obj_user_info.IsSuperUser = True Then
							obj_role_info = obj_role.GetRole(tmp_dictionary_role_moderate, PortalSettings.PortalId)
							If PortalSecurity.IsInRole(obj_role_info.RoleName) Then
								bln_moderator = True
							Else
								bln_moderator = False
							End If						
						Else
							bln_moderator = False
						End If	
							
						Dim dt_dictionary_new As New DataTable
						database.CreateCommand("donein_dictionary_CUD", CommandType.StoredProcedure)
						database.AddParameter("@int_ID", CType(txt_ID.Value, Integer))
						database.AddParameter("@int_category", ddl_category_edit.SelectedValue)
						database.AddParameter("@vch_key", txt_key.Text)
						database.AddParameter("@vch_value", txt_value.Text)
						database.AddParameter("@int_module", ModuleID)
						database.AddParameter("@int_author", CType(obj_user_info.UserID, Integer))
						If bln_moderator = True Then
							database.AddParameter("@int_status", 1) '// IF USER IS IN MODERATOR GROUP, APPROVE SUBMISSION AUTOMATICALLY
						Else
							database.AddParameter("@int_status", -1)
							
							'// SEND NOTIFICATION EMAIL 
							
							Dim tmp_dictionary_email_sender As String = CType(Settings("donein_dictionary_email_sender"), String)
							Dim tmp_dictionary_email_moderator As String = CType(Settings("donein_dictionary_email_moderator"), String)
							Dim tmp_sender As String
							Dim tmp_recipient As String
							If tmp_dictionary_email_sender = "" Then
								tmp_sender = DotNetNuke.Common.HostSettings("HostEmail").ToString.Trim
							Else
								tmp_sender = tmp_dictionary_email_sender.Trim
							End If		
							If tmp_dictionary_email_moderator = "" Then
								tmp_recipient = PortalSettings.Email.Trim
							Else
								tmp_recipient = tmp_dictionary_email_moderator.Trim
							End If
							Dim tmp_subject As String = DotNetNuke.Services.Localization.Localization.GetString("pl_subject.Text", LocalResourceFile) + ""
							Dim tmp_body As String = DotNetNuke.Services.Localization.Localization.GetString("pl_body.Text", LocalResourceFile) + "" 	
							If tmp_body.Trim <> "" Then	
								Dim obj_tab As New TabController
								Dim obj_tab_info As TabInfo = obj_tab.GetTab(TabID)
								Dim tmp_url As String
								If Common.HostSettings.Item("UseFriendlyUrls").ToString = "Y" Then
									tmp_url = FriendlyUrl(obj_tab_info,  ApplicationURL(obj_tab_info.TabID)).Replace("//","/")
								Else
									tmp_url = ApplicationPath + "/Default.aspx?tabid=" + CType(obj_tab_info.TabID, String)
								End If
								tmp_url = "http://" + PortalSettings.PortalAlias.HTTPAlias.Replace("http:","").Replace(ApplicationPath, "") + tmp_url
								tmp_url = "<A HREF=""" + tmp_url + """ TARGET=""_blank"">" + tmp_url + "</A>"
								tmp_body = "<DIV STYLE=""font-family: Arial; font-size: 11pt;""><BR />" + tmp_body.Replace("[BR]","<BR />").Replace("[KEY]",txt_key.Text).Replace("[VALUE]",txt_value.Text).Replace("[LINK]",tmp_url) + "<BR /><BR /><BR /><BR /><BR /></DIV>"		
								Dim arr_recipient_list As Array = Split(tmp_recipient.Replace(",", ";"), ";")
								For Each tmp_individual_recipient As String In arr_recipient_list
									If tmp_individual_recipient.Trim.Length >= 5 Then
										mail.mail_send(tmp_individual_recipient.Trim, DotNetNuke.Common.HostSettings("HostEmail").ToString.Trim, tmp_body, tmp_subject, "", "", tmp_sender, "Normal", DotNetNuke.Common.HostSettings("SMTPServer").ToString.Trim) 
									End If
								Next							
							End If
							
						End If
						database.Execute(dt_dictionary_new)
						If dt_dictionary_new.Rows.Count > 0 Then
							tbl_edit.Visible = False
							tbl_search.Visible = True
							If tmp_dictionary_enable_submission >= 0  Or obj_user_info.IsSuperUser = True Then
								btn_create.Visible = True
							Else
								btn_create.Visible = False
							End If	
							spn_divider.Visible = True
						Else
							If tmp_dictionary_enable_submission >= 0  Or obj_user_info.IsSuperUser = True Then
								btn_create.Visible = True
							Else
								btn_create.Visible = False
							End If	
							tbl_edit.Visible = False
							tbl_search.Visible = True
							btn_create.Visible = True
							spn_divider.Visible = True
							Exit Sub													
						End If	
						ipt_keyword.Value = ""	
					Catch ex As Exception
						ProcessModuleLoadException(Me, ex)
					End Try
					
				Else
					Exit Sub
				End If
				
			End Sub 

		#End Region
		
		
		
		#Region " Handle: Create Button (btn_create) "

  			Private Sub btn_create_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_create.Click	
				Try
					tbl_search.Visible = False
					btn_create.Visible = False
					spn_divider.Visible = False
					tbl_edit.Visible = True
					txt_ID.Value = "0"
					txt_key.Text = ""
					txt_value.Text = ""
					Catch ex As Exception
					ProcessModuleLoadException(Me, ex)
				End Try
			End Sub 

		#End Region



		#Region " Handle: Moderate Button (btn_moderate) "
		
			Private Sub btn_moderate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_moderate.Click
				'If Common.HostSettings.Item("UseFriendlyUrls").ToString = "Y" Then
				'	Dim obj_tab As New TabController
				'	Dim obj_tab_info As TabInfo = obj_tab.GetTab(TabID)
				'	Response.Redirect(FriendlyURL(obj_tab_info, EditUrl("Moderate")), True)
				'Else
					Response.Redirect(EditUrl("Moderate"), True)
				'End If				
			End	Sub
			
		#End Region

	
		
		#Region " Handle: Cancel Button (btn_cancel) "

 			Private Sub btn_cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_cancel.Click	
				Try
					Dim tmp_dictionary_role_moderate As Integer = CType(Settings("donein_dictionary_role_moderate"), Integer)
					Dim tmp_dictionary_enable_submission As Integer = CType(Settings("donein_dictionary_enable_submission"), Integer)
					Dim obj_role As New RoleController
					Dim obj_role_info As RoleInfo 
							
					tbl_search.Visible = True
					If tmp_dictionary_enable_submission >= 0  Or obj_user_info.IsSuperUser = True Then
						btn_create.Visible = True
					Else
						btn_create.Visible = False
					End If	
							
					If tmp_dictionary_role_moderate > 0 Or obj_user_info.IsSuperUser = True Then
						obj_role_info = obj_role.GetRole(tmp_dictionary_role_moderate, PortalSettings.PortalId)
						If PortalSecurity.IsInRole(obj_role_info.RoleName) Then
							bln_moderator = True
							spn_divider.Visible = True
							btn_moderate.Visible = True	
						Else
							bln_moderator = False
							spn_divider.Visible = False
							btn_moderate.Visible = False
						End If						
					Else
						bln_moderator = False
						spn_divider.Visible = False
						btn_moderate.Visible = False
					End If				
					tbl_edit.Visible = False
					ipt_keyword.Value = ""		
				Catch ex As Exception		  
					ProcessModuleLoadException(Me, ex)
				End Try
			End Sub 

		 #End Region
		 
		

		#Region " Web Form Designer Generated Code "

				'This call is required by the Web Form Designer.
				<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

				End Sub

				'NOTE: The following placeholder declaration is required by the Web Form Designer.
				'Do not delete or move it.
				Private designerPlaceholderDeclaration As System.Object

				Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
					'CODEGEN: This method call is required by the Web Form Designer
					'Do not modify it using the code editor.
					InitializeComponent()
				End Sub

		#End Region



		#Region "Optional Interfaces"

				Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
				Get
					Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
						Actions.Add(GetNextActionID, DotNetNuke.Services.Localization.Localization.GetString("pl_action_update.Text", LocalResourceFile), "", "", "", get_update_url("DONEIN_NET\Dictionary"), False, DotNetNuke.Security.SecurityAccessLevel.Host, True, True)
						Actions.Add(GetNextActionID, DotNetNuke.Services.Localization.Localization.GetString("pl_action_moderate.Text", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", EditUrl("Moderate"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
						Actions.Add(GetNextActionID, DotNetNuke.Services.Localization.Localization.GetString("pl_action_category.Text", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", EditUrl("Category"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
						Actions.Add(GetNextActionID, DotNetNuke.Services.Localization.Localization.GetString(Entities.Modules.Actions.ModuleActionType.ContentOptions, LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", EditUrl("Edit"), False, Security.SecurityAccessLevel.Edit, True, False)
					Return Actions
					End Get
				End Property

				Private Function get_update_url(ByVal module_name As String) As String
					Dim obj_tab As DotNetNuke.Entities.Tabs.TabInfo
					With New DotNetNuke.Entities.Tabs.TabController 
						obj_tab = .GetTabByName("DNN Update", DotNetNuke.Common.Utilities.Null.NullInteger)
					End With

					If obj_tab Is Nothing Then
						Return "http://www.dnnupdate.com/module-intro.content?module=" + module_name
					Else
						Return obj_tab.Url + "?tabid=" + obj_tab.TabID.ToString + "&module=" + module_name
					End If
				End Function

				'Public Function ExportModule(ByVal ModuleID As Integer) As String Implements Entities.Modules.IPortable.ExportModule
				'	' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
				'End Function

				'Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements Entities.Modules.IPortable.ImportModule
				'	' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
				'End Sub

				Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As Services.Search.SearchItemInfoCollection Implements Entities.Modules.ISearchable.GetSearchItems
					' included as a stub only so that the core knows this module Implements Entities.Modules.ISearchable
				End Function

		#End Region
		



    End Class

End Namespace
