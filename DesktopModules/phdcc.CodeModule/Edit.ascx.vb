'	phdcc.CodeModule programmable module for DotNetNuke-based systems
'	http://www.phdcc.com/phdcc.CodeModule/
'	Copyright © 2007-2009 PHD Computer Consultants Ltd, PHDCC

'	Version history:

'	ASCX and LocalResources Version history

Imports DotNetNuke

Namespace phdcc.CodeModule

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The EditDynamicModule class is used to manage content
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' </history>
	''' -----------------------------------------------------------------------------
	Partial Class Edit
		Inherits Entities.Modules.PortalModuleBase

#Region "Private Members"

		Private ItemId As Integer = Common.Utilities.Null.NullInteger

#End Region

#Region "Event Handlers"

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Page_Load runs when the control is loaded
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try


			Catch exc As Exception	  'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' cmdCancel_Click runs when the cancel button is clicked
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
			Try
				Response.Redirect(NavigateURL(), True)
			Catch exc As Exception	  'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' cmdUpdate_Click runs when the update button is clicked
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
			Try

				' Redirect back to the portal home page
				Response.Redirect(NavigateURL(), True)
			Catch exc As Exception	  'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' cmdDelete_Click runs when the delete button is clicked
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
			Try

				' Redirect back to the portal home page
				Response.Redirect(NavigateURL(), True)
			Catch exc As Exception	  'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

	End Class

End Namespace