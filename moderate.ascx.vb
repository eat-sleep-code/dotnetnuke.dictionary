Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.ModuleSettingsBase
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Entities.Users


Namespace DONEIN_NET.Dictionary

	Public Class Moderate
		Inherits DotNetNuke.Entities.Modules.PortalModuleBase
		Implements Entities.Modules.IActionable



		#Region " Declare: Shared Classes "
			
			Private database As New database(System.Configuration.ConfigurationSettings.AppSettings("SiteSqlServer"), "SqlClient")
			
		#End Region



		#Region " Declare: Local Objects "
		
			Protected WithEvents btn_create As System.Web.UI.WebControls.LinkButton		
			Protected WithEvents btn_update As System.Web.UI.WebControls.LinkButton		
			Protected WithEvents btn_cancel As System.Web.UI.WebControls.LinkButton		
			
			Protected WithEvents tbl_search As System.Web.UI.HtmlControls.HtmlTable
			Protected WithEvents txt_key_filter As System.Web.UI.WebControls.TextBox
			Protected WithEvents ddl_status As System.Web.UI.WebControls.DropDownList
			Protected WithEvents ddl_category As System.Web.UI.WebControls.DropDownList
			Protected WithEvents dg_dictionary_entry As System.Web.UI.WebControls.DataGrid
			
			Protected WithEvents tbl_edit As System.Web.UI.HtmlControls.HtmlTable
			Protected WithEvents ddl_category_edit As System.Web.UI.WebControls.DropDownList
			Protected WithEvents txt_key As System.Web.UI.WebControls.TextBox
			Protected WithEvents txt_value As System.Web.UI.WebControls.TextBox
			Protected WithEvents txt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
			Protected WithEvents txt_author As System.Web.UI.HtmlControls.HtmlInputHidden
			
			Private obj_user As New UserController
			Private obj_user_info As UserInfo  = obj_user.GetCurrentUserInfo
			
			Public image_path As String = "/DesktopModules/DONEIN_NET/Dictionary/images/"
		#End Region
		
		

		#Region " Page: Load "

			Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
				Try
					If Not IsPostBack Then
						If ModuleId > 0 Then
						
							module_localize() '// LOCALIZE THE MODULE
							dg_dictionary_entry_bind(0,-1,"") '// BIND THE DATAGRID
							ddl_category_edit_bind() '// BIND THE CATEGORY DROPDOWN
							tbl_edit.Visible = False
							
							Dim tmp_dictionary_default_category As String = CType(Settings("donein_dictionary_default_category"), String)
							
							If tmp_dictionary_default_category = "" Then
								ddl_category_edit.SelectedIndex = 0
							Else
								ddl_category_edit.SelectedValue = tmp_dictionary_default_category
							End If	
								
						End If
					End If
				Catch ex As Exception
					ProcessModuleLoadException(Me, ex)
				End Try
			End Sub

		#End Region


		
		#Region " Page: Localize "

 			Private Sub module_localize()
 				
 				btn_create.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_btn_create.Text", LocalResourceFile)
				btn_update.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_btn_update.Text", LocalResourceFile)
				btn_cancel.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_btn_cancel.Text", LocalResourceFile)
				
				ddl_status.Items.Add(New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_unapproved.Text", LocalResourceFile),"-1"))
				ddl_status.Items.Add(New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_approved.Text", LocalResourceFile),"1"))
			End Sub 

		#End Region


		
		#Region " Bind: Category Dropdown List (ddl_category, ddl_category_edit) "

			Private Sub ddl_category_edit_bind()

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
				ddl_category.Items.Insert(0, New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_uncategorized.Text", LocalResourceFile), "0"))
				ddl_category_edit.Items.Insert(0, New ListItem(DotNetNuke.Services.Localization.Localization.GetString("pl_uncategorized.Text", LocalResourceFile), "0"))
			End Sub	
		
		#End Region	
		
		
		
		#Region " Handle: Category Dropdown List Change (ddl_category) "
		
			Private Sub ddl_category_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_category.SelectedIndexChanged
				dg_dictionary_entry_bind(CType(ddl_category.SelectedValue, Integer), CType(ddl_status.SelectedValue, Integer), txt_key_filter.Text)
			End Sub
			
		#End Region		
		
		
		
		#Region " Handle: Status Dropdown List Change (ddl_status) "
		
			Private Sub ddl_status_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_status.SelectedIndexChanged
				dg_dictionary_entry_bind(CType(ddl_category.SelectedValue, Integer), CType(ddl_status.SelectedValue, Integer), txt_key_filter.Text)
			End Sub
			
		#End Region	
		
		
		
		#Region " Handle: Keyword Text Change (txt_key_filter) "
		
			Private Sub txt_key_filter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_key_filter.TextChanged
				dg_dictionary_entry_bind(CType(ddl_category.SelectedValue, Integer), CType(ddl_status.SelectedValue, Integer), txt_key_filter.Text)
			End Sub
			
		#End Region	
		
		

		#Region " Bind: DataGrid (dg_dictionary_entry) "

 			Private Sub dg_dictionary_entry_bind(Optional ByVal tmp_category As Integer = 0, Optional ByVal tmp_status As Integer = 0, Optional ByVal tmp_keyword As String = "")
 				Services.Localization.Localization.LocalizeDataGrid(dg_dictionary_entry, Me.LocalResourceFile)
 				tmp_keyword = tmp_keyword.Replace("%", "").Trim()
				tmp_keyword = tmp_keyword.Replace("_", "").Trim()
				Dim dt_dictionary_entry As New DataTable
				database.CreateCommand("donein_dictionary_R", CommandType.StoredProcedure)
				database.AddParameter("@int_ID", 0)
				database.AddParameter("@int_category", tmp_category)
				database.AddParameter("@int_status", tmp_status)
				database.AddParameter("@vch_key", tmp_keyword + "%")
				database.AddParameter("@int_limit", 20)
				database.Execute(dt_dictionary_entry)
				If dt_dictionary_entry.Rows.Count > 0 Then
					dg_dictionary_entry.DataSource = dt_dictionary_entry
					dg_dictionary_entry.DataBind
					dg_dictionary_entry.Visible = True
				Else
					dg_dictionary_entry.DataSource = dt_dictionary_entry
					dg_dictionary_entry.DataBind
					dg_dictionary_entry.Visible = True
				End If			
				btn_create.Visible = True
			End Sub 

		#End Region
		
		
		
		#Region " Handle: DataGrid ItemCommand (dg_dictionary_entry) "

 			Private Sub dg_dictionary_entry_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_dictionary_entry.ItemCommand
 				If e.CommandName.ToLower.Trim = "delete" Then
 					database.CreateCommand("donein_dictionary_CUD", CommandType.StoredProcedure)
					database.AddParameter("@int_ID", (CType(e.CommandArgument,Integer) * -1))
					database.ExecuteNonQuery()
					dg_dictionary_entry_bind(CType(ddl_category.SelectedValue, Integer), CType(ddl_status.SelectedValue, Integer), txt_key_filter.Text)
				Else If e.CommandName.ToLower.Trim = "approve" Then
 					database.CreateCommand("donein_dictionary_CUD", CommandType.StoredProcedure)
					database.AddParameter("@int_ID", CType(e.CommandArgument,Integer))
					database.AddParameter("@int_status", 10)
					database.ExecuteNonQuery()
					dg_dictionary_entry_bind(CType(ddl_category.SelectedValue, Integer), CType(ddl_status.SelectedValue, Integer), txt_key_filter.Text)
				Else
					Dim dt_dictionary_entry_edit As New DataTable
					database.CreateCommand("donein_dictionary_R", CommandType.StoredProcedure)
					database.AddParameter("@int_ID", CType(e.CommandArgument,Integer))
					database.Execute(dt_dictionary_entry_edit)
					If dt_dictionary_entry_edit.Rows.Count > 0 Then
						txt_ID.Value = dt_dictionary_entry_edit.Rows(0).Item("ID").ToString
						txt_key.Text = dt_dictionary_entry_edit.Rows(0).Item("vch_key").ToString	
						txt_value.Text = dt_dictionary_entry_edit.Rows(0).Item("vch_value").ToString	
						txt_author.Value = dt_dictionary_entry_edit.Rows(0).Item("int_author").ToString
						tbl_search.Visible = False
						btn_create.Visible = False
						tbl_edit.Visible = True
					End If					
				End If 				
			End Sub 

		#End Region
		
		

		#Region " Handle: Update Button (btn_update) "

  			Private Sub btn_update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_update.Click
  				If txt_key.Text.Trim.Length > 0 And txt_value.Text.Trim.Length > 0 Then
					Try
						Dim dt_dictionary_entry_new As New DataTable
						database.CreateCommand("donein_dictionary_CUD", CommandType.StoredProcedure)
						database.AddParameter("@int_ID", CType(txt_ID.Value, Integer))
						database.AddParameter("@int_category", ddl_category_edit.SelectedValue)
						database.AddParameter("@vch_key", txt_key.Text)
						database.AddParameter("@vch_value", txt_value.Text)
						database.AddParameter("@int_module", ModuleID)
						database.AddParameter("@int_author", CType(txt_author.Value, Integer))
						database.AddParameter("@int_status", 1)
						database.Execute(dt_dictionary_entry_new)
						If dt_dictionary_entry_new.Rows.Count > 0 Then
							dg_dictionary_entry_bind()	
							tbl_search.Visible = True
							tbl_edit.Visible = False
						Else
							tbl_search.Visible = True
							tbl_edit.Visible = False
							Exit Sub													
						End If		
						dg_dictionary_entry_bind(CType(ddl_category.SelectedValue, Integer), CType(ddl_status.SelectedValue, Integer), txt_key_filter.Text)
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
					dg_dictionary_entry.Visible = False
					btn_create.Visible = False
					tbl_edit.Visible = True
					txt_ID.Value = "0"
					txt_author.Value = obj_user_info.UserID.ToString
					ddl_category_edit.SelectedIndex = 0
					txt_key.Text = ""
					txt_value.Text = ""
					Catch ex As Exception
					ProcessModuleLoadException(Me, ex)
				End Try
			End Sub 

		#End Region


		
		#Region " Handle: Cancel Button (btn_cancel) "

 			Private Sub btn_cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_cancel.Click	
				Try
					dg_dictionary_entry.Visible = True
					btn_create.Visible = True
					tbl_edit.Visible = False	
					tbl_search.Visible = True
				Catch ex As Exception		  
					ProcessModuleLoadException(Me, ex)
				End Try
			End Sub 

		 #End Region

           
           
		#Region " Convert: Boolean "

 			Public Shared Function int_to_bln(ByVal tmp_int As Integer, Optional ByVal tmp_reverse As Boolean = False) As Boolean
 				
 				If tmp_reverse = True Then
					If tmp_int = 1 Then 
						Return False
					Else
						Return True
					End If 	
				Else
					If tmp_int = 1 Then 
						Return True
					Else
						Return False
					End If 	
				End If 				
 				
			End Function

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

		#End Region


		


	End Class

End NameSpace
