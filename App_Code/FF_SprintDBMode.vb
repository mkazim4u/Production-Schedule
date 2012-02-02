Imports Microsoft.VisualBasic

Public Class FF_SprintDBMode

    Private Shared mode As Boolean = True

    Public Shared ReadOnly Property GetDebugMode() As Boolean
        Get
            Return mode
        End Get
    End Property

End Class
