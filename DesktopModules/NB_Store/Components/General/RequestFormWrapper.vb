'
''  DotNetNuke -  http://www.dotnetnuke.com
''  Copyright (c) 2002-2005
''  by Shaun Walker ( sales@perpetualmotion.ca ) of Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'' 
''  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
''  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
''  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
''  to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'' 
''  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
''  of the Software.
'' 
''  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
''  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
''  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
''  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
''  DEALINGS IN THE SOFTWARE.
'


Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Reflection
Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities

Namespace NEvoWeb.Modules.NB_Store
    ''' <summary>
    ''' Summary description for RequestFormWrapper.
    ''' </summary>
    Public MustInherit Class RequestFormWrapper
#Region "Constructors ==========================================================="

        Public Sub New()
        End Sub

        Public Sub New(ByVal requestForm As NameValueCollection)
            LoadRequestForm(requestForm)
        End Sub

#End Region

#Region "Public Methods ========================================================="

        ''' <summary>
        ''' Parses the Request Form parameters and sets properties, if they exist,
        ''' in the derived object.
        ''' </summary>
        ''' <param name="requestForm"></param>
        Public Sub LoadRequestForm(ByVal requestForm As NameValueCollection)
            ' Iterate thru all properties for this type
            Dim propertyList As PropertyInfo() = Me.[GetType]().GetProperties()
            For Each [property] As PropertyInfo In propertyList
                ' Do we have a value for this property?
                Dim val As String = requestForm([property].Name)
                If val IsNot Nothing Then
                    Dim objValue As Object = Nothing

                    Try
                        ' Cast to the appropriate type
                        Select Case [property].PropertyType.Name
                            Case "String"
                                objValue = DirectCast(val, Object)
                                Exit Select
                            Case "Int32"
                                objValue = DirectCast(Convert.ToInt32(val), Object)
                                Exit Select
                            Case "Boolean"
                                objValue = DirectCast(Convert.ToBoolean(val), Object)
                                Exit Select
                            Case "Decimal"
                                objValue = DirectCast(Convert.ToDecimal(val), Object)
                                Exit Select
                        End Select
                        'Cast failed - Skip this property
                    Catch
                    End Try

                    ' Set the value
                    If objValue IsNot Nothing Then
                        [property].SetValue(Me, objValue, Nothing)
                    End If
                End If
            Next
        End Sub

#End Region
    End Class
End Namespace

