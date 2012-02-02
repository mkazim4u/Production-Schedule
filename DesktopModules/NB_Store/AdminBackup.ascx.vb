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


Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class AdminBackup
        Inherits BaseAdminModule

        Private _BType As String

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                pnlBackup.Visible = False
                pnlRestore.Visible = False

                chkExpOrders.Text = Localization.GetString("chkExpOrders", LocalResourceFile)

                'Sample code to get data
                If Not Request.QueryString("spg") Is Nothing Then
                    Select Case Request.QueryString("spg")
                        Case "bck"
                            _BType = Request.QueryString("spg")
                        Case "res"
                            _BType = Request.QueryString("spg")
                    End Select

                    If Not Page.IsPostBack Then
                        Select Case _BType
                            Case "bck"
                                pnlBackup.Visible = True
                                pnlRestore.Visible = False
                            Case "res"
                                pnlBackup.Visible = False
                                pnlRestore.Visible = True
                        End Select

                    End If
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        Private Sub cmdDoBackup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDoBackup.Click
            Dim exp As New Export
            Dim strBackupFile As String = ""

            'make sure doc directory exists
            CreateDir(PortalSettings, PRODUCTDOCSFOLDER)
            'make sure image directory exists
            CreateDir(PortalSettings, PRODUCTIMAGESFOLDER)

            If rbAllOrders.Checked Then
                strBackupFile = PortalSettings.HomeDirectoryMapPath & PortalAlias.HTTPAlias & "_ExportOrders.xml"
                strBackupFile = exp.ExportOrders(PortalId, False, strBackupFile)
            End If
            If rbAllProducts.Checked Then
                strBackupFile = PortalSettings.HomeDirectoryMapPath & PortalAlias.HTTPAlias & "_ExportProducts.xml"
                strBackupFile = exp.ExportProducts(PortalId, strBackupFile, chkExpOrders.Checked)
            End If
            If rbProductImages.Checked Then
                strBackupFile = PortalSettings.HomeDirectoryMapPath & PortalAlias.HTTPAlias & "_ExportImages.zip"
                strBackupFile = exp.ExportImages(PortalId, strBackupFile)
            End If
            If rbProductDocs.Checked Then
                strBackupFile = PortalSettings.HomeDirectoryMapPath & PortalAlias.HTTPAlias & "_ExportDocs.zip"
                strBackupFile = exp.ExportDocs(PortalId, strBackupFile)
            End If
            If rbShipping.Checked Then
                strBackupFile = PortalSettings.HomeDirectoryMapPath & PortalAlias.HTTPAlias & "_ExportShipping.xml"
                strBackupFile = exp.ExportShipping(PortalId, strBackupFile)
            End If
            If rbPurgeStore.Checked Then
                strBackupFile = PortalSettings.HomeDirectoryMapPath & PortalAlias.HTTPAlias & "_ExportPurged.xml"
                strBackupFile = exp.ExportOrders(PortalId, True, strBackupFile)
                If strBackupFile <> "" Then
                    Dim objOCtrl As New OrderController
                    Dim objPCtrl As New ProductController
                    objOCtrl.PurgeArchivedOrders(PortalId)
                    objPCtrl.PurgeProducts(PortalId)
                    objPCtrl.ImageValidation(PortalId, MapPath("\"), True)
                    objPCtrl.PurgeImages(PortalSettings, PRODUCTIMAGESFOLDER)
                    objPCtrl.DocValidation(PortalId, PortalSettings.HomeDirectoryMapPath & PRODUCTDOCSFOLDER, True)
                    objPCtrl.PurgeDocs(PortalSettings, PRODUCTDOCSFOLDER)
                    objPCtrl.PurgeModels(PortalId)
                End If
            End If

            If strBackupFile <> "" Then
                ForceDownload(strBackupFile)
            Else
                lblMsg.Text = Localization.GetString("ExportError", LocalResourceFile)
            End If
        End Sub

        Private Sub ForceDownload(ByVal DownloadFile As String)
            Response.AppendHeader("content-disposition", "attachment; filename=" & System.IO.Path.GetFileName(DownloadFile))
            Response.ContentType = "application/octet-stream"
            Response.WriteFile(DownloadFile)
            Response.End()
        End Sub


        Private Sub cmdDoImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDoImport.Click
            Dim imp As New Import
            Dim impCSV As New ImportCSV
            Dim strMsg As String = ""
            Dim ImportCSVFile As String = PortalSettings.HomeDirectoryMapPath & "ImportProducts.csv"
            Dim ImportMCSVFile As String = PortalSettings.HomeDirectoryMapPath & "ImportModels.csv"
            Dim ImportFile As String = PortalSettings.HomeDirectoryMapPath & "ImportProducts.xml"
            Dim ImportFileZip As String = PortalSettings.HomeDirectoryMapPath & "ImportProductImages.zip"
            Dim ImportFileDoc As String = PortalSettings.HomeDirectoryMapPath & "ImportProductDocs.zip"
            Dim ImportFileShip As String = PortalSettings.HomeDirectoryMapPath & "ImportShipping.xml"

            If FileUploadXML.FileName <> "" Then
                If System.IO.Path.GetExtension(FileUploadXML.FileName).ToLower = ".csv" Then
                    FileUploadXML.SaveAs(ImportCSVFile)
                    Dim rowD As String = GetStoreSetting(PortalId, "rowdelimeter.importmapping", "None")
                    Dim fieldD As String = GetStoreSetting(PortalId, "fielddelimeter.importmapping", "None")

                    Dim fieldQualifier As String = GetStoreSetting(PortalId, "fieldqualifier.importmapping", "None")
                    Dim fieldEscapedQualifier As String = GetStoreSetting(PortalId, "fieldescapedqualifier.importmapping", "None")
                    'use html decode to convert any special characters (e.g.: &#09; = horizontal tab; &#13;&#10; = vbCrLf) more at http://www.tedmontgomery.com/tutorial/htmlchrc.html
                    rowD = HttpUtility.HtmlDecode(rowD)
                    fieldD = HttpUtility.HtmlDecode(fieldD)
                    fieldQualifier = HttpUtility.HtmlDecode(fieldQualifier)
                    fieldEscapedQualifier = HttpUtility.HtmlDecode(fieldEscapedQualifier)

                    strMsg = impCSV.ImportCSVProducts(PortalSettings, ImportCSVFile, rbImportUpdate.Checked, chkCreateCat.Checked, rowD, fieldD, fieldQualifier, fieldEscapedQualifier)


                Else
                    If System.IO.Path.GetExtension(FileUploadXML.FileName).ToLower = ".xml" Then
                        FileUploadXML.SaveAs(ImportFile)
                        If chkArchiveProd.Checked Then
                            'set all products to archive, new imports will be un-archived on import.
                            Dim aryList As ArrayList
                            Dim objPCtrl As New ProductController
                            aryList = objPCtrl.GetProductList(PortalId, -1, GetCurrentCulture, True)
                            For Each obj As NB_Store_ProductsInfo In aryList
                                obj.Archived = True
                                objPCtrl.UpdateObjProduct(obj)
                            Next
                        End If
                        strMsg = imp.ImportProducts(PortalSettings, ImportFile, rbImportUpdate.Checked, chkCreateCat.Checked)
                    Else
                        strMsg = Localization.GetString("InvalidImportFile", LocalResourceFile)
                    End If
                End If
            End If

            If FileUploadZip.FileName <> "" Then
                FileUploadZip.SaveAs(ImportFileZip)
                strMsg &= imp.ImportProductImages(PortalSettings, ImportFileZip)
            End If
            If FileUploadDocs.FileName <> "" Then
                FileUploadDocs.SaveAs(ImportFileDoc)
                strMsg &= imp.ImportProductDocs(PortalSettings, ImportFileDoc)
            End If
            If FileUploadShip.FileName <> "" Then
                FileUploadShip.SaveAs(ImportFileShip)
                strMsg &= imp.ImportShipping(PortalSettings, ImportFileShip)
                Dim strCacheKey As String = "DefaultShipMethod" & PortalId.ToString
                DataCache.RemoveCache(strCacheKey)
            End If

            '---------- Model update ------------
            If FileUploadModel.FileName <> "" Then
                If System.IO.Path.GetExtension(FileUploadModel.FileName).ToLower = ".csv" Then
                    FileUploadModel.SaveAs(ImportMCSVFile)
                    Dim rowD As String = GetStoreSetting(PortalId, "rowdelimeter.importmapping", "None")
                    Dim fieldD As String = GetStoreSetting(PortalId, "fielddelimeter.importmapping", "None")

                    Dim fieldQualifier As String = GetStoreSetting(PortalId, "fieldqualifier.importmapping", "None")
                    Dim fieldEscapedQualifier As String = GetStoreSetting(PortalId, "fieldescapedqualifier.importmapping", "None")
                    'use html decode to convert any special characters (e.g.: &#09; = horizontal tab; &#13;&#10; = vbCrLf) more at http://www.tedmontgomery.com/tutorial/htmlchrc.html
                    rowD = HttpUtility.HtmlDecode(rowD)
                    fieldD = HttpUtility.HtmlDecode(fieldD)
                    fieldQualifier = HttpUtility.HtmlDecode(fieldQualifier)
                    fieldEscapedQualifier = HttpUtility.HtmlDecode(fieldEscapedQualifier)

                    strMsg = impCSV.ImportCSVModels(PortalSettings, ImportMCSVFile, rowD, fieldD, fieldQualifier, fieldEscapedQualifier)

                End If
            End If


            lblMsg.Text = strMsg
        End Sub

    End Class

End Namespace
