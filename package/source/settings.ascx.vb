Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.ModuleSettingsBase
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions



Namespace DONEIN_NET.Dictionary

	Public Class Settings
		Inherits DotNetNuke.Entities.Modules.PortalModuleBase
		Implements Entities.Modules.IActionable
        'Implements Entities.Modules.IPortable
        Implements Entities.Modules.ISearchable



		#Region " Declare: Shared Classes "

			Private database As New database(System.Configuration.ConfigurationSettings.AppSettings("SiteSqlServer"), "SqlClient")
			Private mail As New Mail()
			
		#End Region

		

		#Region " Declare: Local Objects "
			
			Protected WithEvents lbl_message As System.Web.UI.WebControls.Label
			Protected WithEvents btn_update As System.Web.UI.WebControls.LinkButton
			Protected WithEvents btn_cancel As System.Web.UI.WebControls.LinkButton
			
			Protected WithEvents ddl_role_moderate As System.Web.UI.WebControls.DropDownList
			Protected WithEvents ddl_result_limit As System.Web.UI.WebControls.DropDownList
			Protected WithEvents txt_form_width As System.Web.UI.WebControls.TextBox
			Protected WithEvents rad_enable_submission As System.Web.UI.WebControls.RadioButtonList
			Protected WithEvents tr_enable_category_selection As System.Web.UI.HtmlControls.HtmlTableRow
			Protected WithEvents rad_enable_category_selection As System.Web.UI.WebControls.RadioButtonList
			Protected WithEvents tr_default_category As System.Web.UI.HtmlControls.HtmlTableRow
			Protected WithEvents ddl_default_category As System.Web.UI.WebControls.DropDownList
			Protected WithEvents txt_email_sender As System.Web.UI.WebControls.TextBox
			Protected WithEvents txt_email_moderator As System.Web.UI.WebControls.TextBox
		
			Private obj_user As New UserController
			Private obj_user_info As UserInfo  = obj_user.GetCurrentUserInfo
			
		#End Region
			
			
			
		#Region " Page: Load "

			Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
				Try
					If ModuleId > 0 Then
					
						If Not IsPostBack Then
						
							module_localize() '// LOCALIZE THE MODULE
							ddl_role_moderate_bind()
							ddl_result_limit_bind()
							rad_enable_category_selection_bind()
							rad_enable_submission_bind()
							ddl_default_category_bind()
							
							Dim tmp_dictionary_role_moderate As String = CType(Settings("donein_dictionary_role_moderate"), String)
							Dim tmp_dictionary_result_limit As String = CType(Settings("donein_dictionary_result_limit"), String)
							Dim tmp_dictionary_form_width As String = CType(Settings("donein_dictionary_form_width"), String)
							Dim tmp_dictionary_enable_submission As String = CType(Settings("donein_dictionary_enable_submission"), String)
							Dim tmp_dictionary_enable_category_selection As String = CType(Settings("donein_dictionary_enable_category_selection"), String)
							Dim tmp_dictionary_default_category As String = CType(Settings("donein_dictionary_default_category"), String)
							Dim tmp_dictionary_email_sender As String = CType(Settings("donein_dictionary_email_sender"), String)
							Dim tmp_dictionary_email_moderator As String = CType(Settings("donein_dictionary_email_moderator"), String)
						
							If tmp_dictionary_role_moderate = "" Then
								ddl_role_moderate.SelectedIndex = 0
							Else
								ddl_role_moderate.SelectedValue = tmp_dictionary_role_moderate
							End If
							
							If tmp_dictionary_result_limit = "" Then
								ddl_result_limit.SelectedValue = "50"
							Else
								ddl_result_limit.SelectedValue = tmp_dictionary_result_limit
							End If
							
							If tmp_dictionary_form_width = "" Then
								txt_form_width.Text = ""
							Else
								txt_form_width.Text = tmp_dictionary_form_width 
							End If
							
							If tmp_dictionary_enable_submission = "" Then
								rad_enable_submission.SelectedValue = "1"
							Else
								rad_enable_submission.SelectedValue = tmp_dictionary_enable_submission
							End If
							
							If tmp_dictionary_enable_category_selection = "" Then
								rad_enable_category_selection.SelectedValue = "1"
							Else
								rad_enable_category_selection.SelectedValue = tmp_dictionary_enable_category_selection
							End If
							
							If tmp_dictionary_default_category = "" Then
								ddl_role_moderate.SelectedIndex = 0
							Else
								ddl_role_moderate.SelectedValue = tmp_dictionary_default_category.Trim
								Response.Write("test")
							End If
							
							If tmp_dictionary_email_sender = "" Then
								txt_email_sender.Text = DotNetNuke.Common.HostSettings("HostEmail").ToString.Trim
							Else
								txt_email_sender.Text = tmp_dictionary_email_sender
							End If		
							
							If tmp_dictionary_email_moderator = "" Then
								txt_email_moderator.Text = PortalSettings.Email
							Else
								txt_email_moderator.Text = tmp_dictionary_email_moderator
							End If
														
						End If
					End If
				Catch ex As Exception
					ProcessModuleLoadException(Me, ex)
				End Try
			End Sub
		
		
		#End Region



		#Region " Page: PreRender "

			Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
				 
			End Sub

		#End Region



		#Region " Page: Localization "

 			Private Sub module_localize()
				btn_update.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_btn_update.Text", LocalResourceFile)
				btn_cancel.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_btn_cancel.Text", LocalResourceFile)
				
			End Sub 

		#End Region
			
			
			
		#Region " Bind: Moderate Role Dropdown List (ddl_role_moderate)"

			Private Sub ddl_role_moderate_bind()
				Dim obj_role As New RoleController
				Dim arr_role_info As ArrayList = obj_role.GetPortalRoles(PortalId)
				ddl_role_moderate.Items.Insert(0, New ListItem("","0"))
				For Each obj_role_info As RoleInfo In arr_role_info
					ddl_role_moderate.Items.Add(New ListItem(obj_role_info.RoleName,obj_role_info.RoleID.ToString))
				Next	
			End Sub

		#End Region
		
		
		
		#Region " Bind: Limit Dropdown List (ddl_result_limit)"

			Private Sub ddl_result_limit_bind()
				ddl_result_limit.Items.Add(New ListItem("  5", 5))
				ddl_result_limit.Items.Add(New ListItem(" 10", 10))
				ddl_result_limit.Items.Add(New ListItem(" 20", 20))
				ddl_result_limit.Items.Add(New ListItem(" 25", 25))
				ddl_result_limit.Items.Add(New ListItem(" 50", 50))
				ddl_result_limit.Items.Add(New ListItem(" 75", 75))
				ddl_result_limit.Items.Add(New ListItem("100", 100))
			End Sub

		#End Region
		
		
		
		#Region " Bind: Enable Submission Radiobutton List (rad_enable_submission)"

			Private Sub rad_enable_submission_bind()
				rad_enable_submission.Items.Add(New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_yes.Text", LocalResourceFile), "1"))
				rad_enable_submission.Items.Add(New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_no.Text", LocalResourceFile), "-1"))
			End Sub

		#End Region
		
		
		
		#Region " Bind: Enable Category Selection Radiobutton List (rad_enable_category_selection)"

			Private Sub rad_enable_category_selection_bind()
				rad_enable_category_selection.Items.Add(New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_yes.Text", LocalResourceFile), "1"))
				rad_enable_category_selection.Items.Add(New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_no.Text", LocalResourceFile), "-1"))
			End Sub

		#End Region
		
		
		
		#Region " Bind: Category Dropdown List (ddl_default_category)"

			Private Sub ddl_default_category_bind()
				Dim dt_category As New DataTable
				database.CreateCommand("donein_dictionary_category_R", CommandType.StoredProcedure)
				database.AddParameter("@int_ID", 0)
				database.AddParameter("@int_status", 0)
				database.Execute(dt_category)
				If dt_category.Rows.Count > 0 Then
					ddl_default_category.DataTextField = "vch_category"
					ddl_default_category.DataValueField = "ID"
					ddl_default_category.DataSource = dt_category
					ddl_default_category.DataBind()	
				Else
					tr_default_category.Visible = False
				End If
				ddl_default_category.Items.Insert(0, New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_uncategorized.Text", LocalResourceFile), "0"))
			End Sub

		#End Region
	    	
	    	    
	    
	    #Region " Handle: Update Button (btn_update) "

			Private Sub btn_update_click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_update.Click
				Try
					Dim obj_modules As New ModuleController
					obj_modules.UpdateModuleSetting(ModuleId, "donein_dictionary_role_moderate", ddl_role_moderate.SelectedValue.ToString)
					obj_modules.UpdateModuleSetting(ModuleId, "donein_dictionary_result_limit", ddl_result_limit.SelectedValue.ToString)
					Try
						obj_modules.UpdateModuleSetting(ModuleId, "donein_dictionary_form_width", CType(txt_form_width.Text, Integer))
					Catch
						obj_modules.UpdateModuleSetting(ModuleId, "donein_dictionary_form_width", 480)
					End Try
					obj_modules.UpdateModuleSetting(ModuleId, "donein_dictionary_enable_submission", rad_enable_submission.SelectedValue.ToString)
					obj_modules.UpdateModuleSetting(ModuleId, "donein_dictionary_enable_category_selection", rad_enable_category_selection.SelectedValue.ToString)
					obj_modules.UpdateModuleSetting(ModuleId, "donein_dictionary_default_category", ddl_default_category.SelectedValue.ToString)
					obj_modules.UpdateModuleSetting(ModuleId, "donein_dictionary_email_sender", txt_email_sender.Text.Trim)
					obj_modules.UpdateModuleSetting(ModuleId, "donein_dictionary_email_moderator", txt_email_moderator.Text.Trim)
					Response.Redirect(NavigateURL(), True)
				Catch ex As Exception
					ProcessModuleLoadException(Me, ex)
				End Try
			End Sub			
			
		#End Region	    
		
		
		
		#Region " Handle: Cancel Button (btn_cancel) "
			
			Private Sub btn_cancel_click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_cancel.Click
				lbl_message.Text = ""
				lbl_message.Visible = False
				Try
					Response.Redirect(NavigateURL(), True)
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
		
		
		
		#Region " Optional Interfaces "

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

End NameSpace
