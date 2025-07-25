using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxProEInvoice.API;

namespace EWayBillTool
{
    public partial class SearchGSTIN : Form
    {
        private AppMain Appmain = new AppMain();
        public SearchGSTIN()
        {
            InitializeComponent();
        }
        private void frmSearchGSTIN_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            var fileName = Application.StartupPath + @"\" + TxtSearchGSTIN.Text.ToString() + ".dat";            
            TaxProEInvoice.API.TxnRespWithObj<RespPlGetGSTIN> txnResp = await eInvoiceAPI.GetGSTINDetailsAsync(Appmain.eInvSession, TxtSearchGSTIN.Text.ToString());
            if (txnResp.IsSuccess)
            {
                TxtGSTINDet.Text = Newtonsoft.Json.JsonConvert.SerializeObject(txnResp.RespObj);
                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter s = new StreamWriter(fs);
                s.Close();
                fs.Close();
                FileStream fs1 = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                StreamWriter s1 = new StreamWriter(fs1);
                s1.Write(Newtonsoft.Json.JsonConvert.SerializeObject(txnResp.RespObj));
                s1.Close();
                fs1.Close();                
                MessageBox.Show("GSTIN detail fetched successfully.","", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show(txnResp.TxnOutcome);
                TxtGSTINDet.Text = txnResp.TxnOutcome;
            }
        }
    }
}
