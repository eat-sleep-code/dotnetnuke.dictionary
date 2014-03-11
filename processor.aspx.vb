Imports DotNetNuke
Imports DotNetNuke.Entities.Users
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Reflection

Namespace DONEIN_NET.Dictionary

	Public Class Processor
		Inherits System.Web.UI.Page



		#Region " Declare: Shared Classes "

			Private database As New database(System.Configuration.ConfigurationSettings.AppSettings("SiteSqlServer"), "SqlClient")
			
		#End Region

		

		#Region " Declare: Local Objects "
		
			Private obj_user As New UserController
			Private obj_user_info As UserInfo  = obj_user.GetCurrentUserInfo
			
		#End Region



		#Region " Page: Load "

			Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
				
				Dim tmp_keyword As String = Request.QueryString.Item("key")
				Dim tmp_category As Integer = CType(Request.QueryString.Item("category"), Integer)
				Dim tmp_limit As Integer = CType(Request.QueryString.Item("limit"), Integer)
				tmp_keyword = tmp_keyword.Replace("%", "").Trim()
				tmp_keyword = tmp_keyword.Replace("_", "").Trim()
				If (Not tmp_keyword Is Nothing) And tmp_keyword.Length > 0 Then
					Dim dt_results As New DataTable
					database.CreateCommand("donein_dictionary_R", CommandType.StoredProcedure)
					database.AddParameter("@int_ID", 0)
					database.AddParameter("@int_category", tmp_category)
					database.AddParameter("@vch_key", tmp_keyword + "%") 
					database.AddParameter("@int_status", 1)
					database.AddParameter("@int_limit", tmp_limit)
					database.Execute(dt_results)
					If dt_results.Rows.Count > 0 Then
						Response.Write("<TABLE WIDTH=""100%"" CELLPADDING=""2"" CELLSPACING=""1"">" + vbcrlf)
						For i As Integer = 0 To dt_results.Rows.Count - 1
							Response.Write("	<TR>" + vbcrlf)
							Response.Write("		<TD ALIGN=""LEFT"" VALIGN=""TOP"" STYLE=""font-weight: bold; padding-right: 5px;"">" + dt_results.Rows(i).Item("vch_key").ToString.Trim + "</TD>" + vbcrlf)
							Response.Write("		<TD ALIGN=""LEFT"" VALIGN=""TOP"">" + dt_results.Rows(i).Item("vch_value").ToString.Trim + "</TD>" + vbcrlf)
							Response.Write("	</TR>" + vbcrlf)
						Next		
						Response.Write("</TABLE>" + vbcrlf)			
					Else

					End If			
				End If

			End Sub
			
		#End Region
		
		
		
		#Region " Web Form Designer Generated Code "

			'This call is required by the Web Form Designer.
			<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

			End Sub
			Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink

			'NOTE: The following placeholder declaration is required by the Web Form Designer.
			'Do not delete or move it.
			Private designerPlaceholderDeclaration As System.Object

			Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
				'CODEGEN: This method call is required by the Web Form Designer
				'Do not modify it using the code editor.
				InitializeComponent()
			End Sub

		#End Region



	End Class
		
End NameSpace
