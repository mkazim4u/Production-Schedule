' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. BSD License.
' Author: D.C.Lee
' ------------------------------------------------------------------------
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' ------------------------------------------------------------------------
' This copyright notice may NOT be removed, obscured or modified without written consent from the author.
' --- End copyright notice --- 


Imports System
Imports System.IO

Namespace NEvoWeb.Modules.NB_Store

    Public Class FileObj
        Public Function GetFileContents(ByVal FullPath As String, Optional ByRef ErrInfo As String = "") As String

            Dim strContents As String
            Dim objReader As StreamReader
            Try
                objReader = New StreamReader(FullPath, System.Text.Encoding.UTF8)
                strContents = objReader.ReadToEnd()
                objReader.Close()
                Return strContents
            Catch Ex As Exception
                ErrInfo = Ex.Message
            End Try
            Return ""
        End Function

        Public Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean

            Dim bAns As Boolean = False
            Dim objReader As StreamWriter

            Try
                objReader = New StreamWriter(FullPath, False, System.Text.Encoding.UTF8)
                objReader.Write(strData)
                objReader.Close()
                bAns = True
            Catch Ex As Exception
                ErrInfo = Ex.Message

            End Try
            Return bAns
        End Function

    End Class

End Namespace