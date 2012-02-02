Imports System.ServiceModel

' NOTE: You can use the "Rename" command on the context menu to change the interface name "IService" in both code and config file together.
<ServiceContract()> _
Public Interface IService

    <OperationContract()> _
    Function GetUserNameabc(ByVal str As String) As String

    <OperationContract()> _
    Function GetUsers() As DataTable

End Interface
