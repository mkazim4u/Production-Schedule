Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.Common

Public Class DBConvert
    '	-----------------------------------------------------------------------------
    '	This template is used to generate a simple class which is used for the sole
    '	purpose of converting a DataReader to a DataTable.
    '	-----------------------------------------------------------------------------

#Region "Class DbConvert"

    '	The sole purpose of this class is to convert a DataReader to a DataTable,
    '	as required by the SqlQuery method of the Db class above.

    ' Friend Class DbConvert
    Inherits DbDataAdapter

    Public Function FillFromReader(ByVal oDt As DataTable, ByVal oDr As IDataReader) As Integer
        Dim intRetVal As Integer = 0

        Try
            intRetVal = Me.Fill(oDt, oDr)

        Catch
            Throw
        End Try

        Return intRetVal
    End Function

    'End Class

#End Region

End Class
