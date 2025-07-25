Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports TaxProGST.API
Imports TaxProGST.JsonModels
Imports Newtonsoft.Json
Imports System.IO

Public Class frmSearchGSTIN
    Private Sub frmSearchGSTIN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
    End Sub
    Private Async Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        Dim fileName = Application.StartupPath + "\" + TxtSearchGSTIN.Text.ToString() + ".dat"
        Dim txnResp As TxnRespWithObj(Of SearchTaxpayerJson) = Await PublicAPI.SearchTaxPayerAsync("1613471119", "Dhanchhaya@123", "24AAGCA8671A1ZX", TxtSearchGSTIN.Text.ToString())
        If (txnResp.IsSuccess) Then
            'MsgBox(JsonConvert.SerializeObject(txnResp.RespObj))
            TxtGSTINDet.Text = Newtonsoft.Json.JsonConvert.SerializeObject(txnResp.RespObj)
            Dim fs As FileStream = New FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)
            Dim s As StreamWriter = New StreamWriter(fs)
            s.Close()
            fs.Close()
            Dim fs1 As FileStream = New FileStream(fileName, FileMode.Append, FileAccess.Write)
            Dim s1 As StreamWriter = New StreamWriter(fs1)
            s1.Write(Newtonsoft.Json.JsonConvert.SerializeObject(txnResp.RespObj))
            s1.Close()
            fs1.Close()
            MsgBox("GSTIN detail fetched successfully.", MsgBoxStyle.OkOnly)
        Else
            MsgBox(txnResp.TxnOutcome)
            TxtGSTINDet.Text = txnResp.TxnOutcome
        End If
    End Sub
End Class