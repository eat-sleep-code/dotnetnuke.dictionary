Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.ModuleSettingsBase
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Entities.Users


Namespace DONEIN_NET.Dictionary

	Public Class Category
		Inherits DotNetNuke.Entities.Modules.PortalModuleBase
		Implements Entities.Modules.IActionable



		#Region " Declare: Shared Classes "
			
			Private database As New database(System.Configuration.ConfigurationSettings.AppSettings("SiteSqlServer"), "SqlClient")
			
		#End Region



		#Region " Declare: Local Objects "
		
			Protected WithEvents btn_create As System.Web.UI.WebControls.LinkButton		
			Protected WithEvents btn_update As System.Web.UI.WebControls.LinkButton		
			Protected WithEvents btn_cancel As System.Web.UI.WebControls.LinkButton		
			
			Protected WithEvents dg_category As System.Web.UI.WebControls.DataGrid
			Protected WithEvents tbl_category_edit As System.Web.UI.HtmlControls.HtmlTable
				
			Protected pl_category As UI.UserControls.LabelControl
			
			Protected WithEvents txt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
			Protected WithEvents txt_category As System.Web.UI.WebControls.TextBox
			
			Private obj_user As New UserController
			Private obj_user_info As UserInfo  = obj_user.GetCurrentUserInfo
			
		#End Region
		
		

		#Region " Page: Load "

			Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
				Try
					If Not IsPostBack Then
						If ModuleId > 0 Then
							module_localize() '// LOCALIZE THE MODULE
							dg_category_bind() '// BIND THE DATAGRID
							tbl_category_edit.Visible = False
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
				
				pl_category.Text = DotNetNuke.Services.Localization.Localization.GetString("pl_category.Text", LocalResourceFile)
				
			End Sub 

		#End Region



		#Region " Bind: DataGrid (dg_category) "

 			Private Sub dg_category_bind()
 				Services.Localization.Localization.LocalizeDataGrid(dg_category, Me.LocalResourceFile)
 				
				Dim dt_category As New DataTable
				database.CreateCommand("donein_dictionary_category_R", CommandType.StoredProcedure)
				database.AddParameter("@int_ID", 0)
				database.AddParameter("@int_module", ModuleID)
				database.Execute(dt_category)
				If dt_category.Rows.Count > 0 Then
					dg_category.DataSource = dt_category
					dg_category.DataBind
					dg_category.Visible = True
				Else
					dg_category.Visible = False
				End If			
				btn_create.Visible = True
			End Sub 

		#End Region
		
		
		
		#Region " Handle: DataGrid ItemCommand (dg_category) "

 			Private Sub dg_category_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_category.ItemCommand
 				If e.CommandName.ToLower.Trim = "delete" Then
 					database.CreateCommand("donein_dictionary_category_CUD", CommandType.StoredProcedure)
					database.AddParameter("@int_ID", (CType(e.CommandArgument,Integer) * -1))
					database.ExecuteNonQuery()
					dg_category_bind()
				Else
					Dim dt_category_edit As New DataTable
					database.CreateCommand("donein_dictionary_category_R", CommandType.StoredProcedure)
					database.AddParameter("@int_ID", CType(e.CommandArgument,Integer))
					database.Execute(dt_category_edit)
					If dt_category_edit.Rows.Count > 0 Then
						txt_ID.Value = dt_category_edit.Rows(0).Item("ID").ToString
						txt_category.Text = dt_category_edit.Rows(0).Item("vch_category").ToString	
						dg_category.Visible = False
						btn_create.Visible = False
						tbl_category_edit.Visible = True
					End If					
				End If 				
			End Sub 

		#End Region
		
		

		#Region " Handle: Update Button (btn_update) "

  			Private Sub btn_update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_update.Click	
				Try
					Dim dt_category_new As New DataTable
					database.CreateCommand("donein_dictionary_category_CUD", CommandType.StoredProcedure)
					database.AddParameter("@int_ID", CType(txt_ID.Value, Integer))
					database.AddParameter("@vch_category", txt_category.Text)
					database.AddParameter("@int_module", ModuleID)
					database.AddParameter("@int_author", CType(obj_user_info.UserID, Integer))
					database.AddParameter("@int_status", 1)
					database.Execute(dt_category_new)
					If dt_category_new.Rows.Count > 0 Then
						dg_category_bind()	
						tbl_category_edit.Visible = False
					Else
						tbl_category_edit.Visible = False
						Exit Sub													
					End If		
				Catch ex As Exception
					ProcessModuleLoadException(Me, ex)
				End Try
			End Sub 

		#End Region
		
		
		
		#Region " Handle: Create Button (btn_create) "

  			Private Sub btn_create_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_create.Click	
				Try
					dg_category.Visible = False
					btn_create.Visible = False
					tbl_category_edit.Visible = True
					txt_ID.Value = "0"
					txt_category.Text = ""
					Catch ex As Exception
					ProcessModuleLoadException(Me, ex)
				End Try
			End Sub 

		#End Region


		
		#Region " Handle: Cancel Button (btn_cancel) "

 			Private Sub btn_cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_cancel.Click	
				Try
					dg_category.Visible = True
					btn_create.Visible = True
					tbl_category_edit.Visible = False	
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

		#End Region


		
	End Class

End NameSpace
