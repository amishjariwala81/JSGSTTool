using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxProEInvoice.API;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EWayBillTool
{
    public partial class frmGetGSTINDet : Form
    {

        private AppMain Appmain = new AppMain();

        public frmGetGSTINDet()
        {
            MessageBox.Show("1");
            InitializeComponent();
            
            frmGetGSTINDet_Load();
            
        }

        private async void frmGetGSTINDet_Load()
        {
            //MessageBox.Show("2");
            Appmain.GetCmdArgs();
            MessageBox.Show(Appmain.cASPUsr);
            Appmain.MakeSQLConnObj();
            Appmain.MakeEInvoiceSession();

            if (Appmain.cASPUsr != "" )
            {
                //'MsgBox("IN API")

                MessageBox.Show("IN API");
                var fileName = Application.StartupPath + @"\" + Appmain.cSearchGSTIN.ToString() + ".txt";
                MessageBox.Show(fileName.ToString());
                MessageBox.Show(Appmain.cASPUsr.ToString() + " " + Appmain.cASPPwd.ToString() + " " + Appmain.cPtyGstIn.ToString() + " " + Appmain.cSearchGSTIN.ToString());                
                //TxnRespWithObj<SearchTaxpayerJson> txnResp = await PublicAPI.SearchTaxPayerAsync(Appmain.cASPUsr.Trim(), Appmain.cASPPwd.Trim(), Appmain.cPtyGstIn.Trim(), Appmain.cSearchGSTIN.Trim());

                TaxProEInvoice.API.TxnRespWithObj<RespPlGetGSTIN> txnResp = await eInvoiceAPI.GetGSTINDetailsAsync(Appmain.eInvSession, Appmain.cSearchGSTIN.Trim());

                MessageBox.Show(txnResp.IsSuccess.ToString());
                if (txnResp.IsSuccess)
                {
                    MessageBox.Show(JsonConvert.SerializeObject(txnResp.RespObj));
                    FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter s = new StreamWriter(fs);
                    s.Close();
                    fs.Close();
                    FileStream fs1 = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                    StreamWriter s1 = new StreamWriter(fs1);
                    //'Dim strData As String
                    //'strData = ""
                    //'strData += "gstin:" + txnResp.RespObj.gstin
                    //'strData += "lgnm:" + txnResp.RespObj.lgnm
                    //'strData += "rgdt:" + txnResp.RespObj.rgdt
                    //'strData += "tradenm:" + txnResp.RespObj.tradeNam
                    //'strData += "Sts:" + txnResp.RespObj.sts

                    //'Dim AddrJSOn = txnResp.RespObj.adadr.
                    //'strData += "flno:" + txnResp.RespObj.adadr
                    //'strData += "bnm:" + txnResp.RespObj.gstin
                    //'strData += "st:" + txnResp.RespObj.gstin
                    //'strData += "loc:" + txnResp.RespObj.gstin
                    //'strData += "fst:" + txnResp.RespObj.gstin
                    //'strData += "pncd:" + txnResp.RespObj.gstin
                    //'strData += "stcd:" + txnResp.RespObj.gstin

                    //'s1.Write(Newtonsoft.Json.JsonConvert.SerializeObject(txnResp.RespObj))
                    s1.Write(Newtonsoft.Json.JsonConvert.SerializeObject(txnResp.RespObj, Formatting.Indented));
                    s1.Close();
                    fs1.Close();
                    MessageBox.Show("GSTIN detail fetched successfully.", "Information", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show(txnResp.TxnOutcome);
                }
            }
            else
            {
                MessageBox.Show("Parameter Missing.", "Search GSTIN", MessageBoxButtons.OK);
            }
            Application.Exit();
        }
    }
}