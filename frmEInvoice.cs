using JSGSTTool;
using JSGSTTool.Properties;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxProEInvoice.API;
using TaxProEWB.API;
using ZXing;
using ZXing.QrCode;
using static System.Net.Mime.MediaTypeNames;
//using JSGSTTool.CommFunc;

namespace EWayBillTool
{
    public partial class frmEInvoice : Form
    {

        private AppMain Appmain = new AppMain();
        private CommFunc Commfunc;
        private ReqPlGenIRN.TranDetails TranDtls;
        public string path = System.Windows.Forms.Application.StartupPath + @"\QRCode\";
        public QrCodeEncodingOptions QROption;
        public bool DefServerLink = true;

        public frmEInvoice()
        {
            InitializeComponent();
            FrmEInvoice_Load();
            EInvBillDetGrid.KeyUp += BillDetGrid_KeyUp;
            //btnChkAll.Click += BtnChkAll_Click;
            //btnUnChkAll.Click+= BtnUnChkAll_Click;
            //BtnGetDetail.Click += BtnGetDetail_Click;
            ////BtnEInvGen.Click += BtnEInvGen_Click;
            //QRCodeBtn.Click += QRCodeBtn_Click;
            //BtnEWBGen.Click += BtnEWBGen_Click;
            //BtnEWBPrint.Click += BtnEWBPrint_Click;
            //BtnEWBDetPrn.Click += BtnEWBDetPrn_Click;
            //BtnIRNTKN.Click += BtnIRNTKN_Click;
            //BtnDebug.Click += BtnDebug_Click;
            //BtnEWBCancel.Click += BtnEWBCancel_Click;

        }
        private void FrmEInvoice_FormClosed(object sender, EventArgs e)
        {
            try
            {
                Appmain.sqlCnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("FrmEInvoice_FormClosed " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmEInvoice_Load()
        {
            QROption = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 250,
                Height = 250,
                Margin = 0
            };

            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = QROption;
            DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
            checkCol.HeaderText = "X";
            EInvBillDetGrid.Columns.Add(checkCol);
            EInvBillDetGrid.Columns[0].Frozen = true;
            DataGridViewCheckBoxColumn Status = new DataGridViewCheckBoxColumn();
            Status.HeaderText = "Status";
            try
            {

                Appmain.GetCmdArgs();

                CntrlBtn(false);
                oEInvStatusMsg.Text = "Please wait while loading details...";

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet ds = new DataSet();
                string sql;
                try
                {
                    oEInvStatusMsg.Text = "Please wait while establishing database connection...";
                    Appmain.MakeSQLConnObj();
                    oEInvStatusMsg.Text = "Please wait while loading data...";
                    sql = " Select  * from " + Appmain.cInvTble;
                    //Appmain.sqlCnn.Open();
                    adapter = new SqlDataAdapter(sql, Appmain.sqlCnn);
                    adapter.Fill(ds);
                    EInvBillDetGrid.DataSource = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Connection/Data Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    oEInvStatusMsg.Text = ex.Message;
                }
                Appmain.UnChkAll(EInvBillDetGrid);
                oEInvStatusMsg.Text = "Data loaded successfully...";
                Appmain.MakeEInvoiceSession();
                Appmain.MakeEWayBillSession();
                HideColumns(EInvBillDetGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show("IRN Module Load 2: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }


            ToolTip buttonToolTip = new ToolTip
            {
                UseFading = true,
                UseAnimation = true,
                IsBalloon = true,
                ShowAlways = true,
                AutoPopDelay = 5000,
                InitialDelay = 1000,
                ReshowDelay = 500
            };
            buttonToolTip.IsBalloon = true;
            buttonToolTip.ShowAlways = true;

            buttonToolTip.SetToolTip(BtnEInvGen, "Generate E-Invoice.");
            buttonToolTip.SetToolTip(BtnEWBGen, "Generate EWay Bill.");
            buttonToolTip.SetToolTip(BtnEWBPrint, "EWay Bill Print - Summary.");
            buttonToolTip.SetToolTip(BtnEWBDetPrn, "EWay Bill Print - Detail.");
            buttonToolTip.SetToolTip(BtnDebug, "Enable Debug Mode.");
            buttonToolTip.SetToolTip(BtnEWBCancel, "Cancel Generated EWay Bill.");
            buttonToolTip.SetToolTip(BtnGetDetail, "Get IRN Details.");

            buttonToolTip.SetToolTip(btnChkAll, "Select All Bills.");
            buttonToolTip.SetToolTip(btnUnChkAll, "Unselect All Bills.");
        }

        private void CntrlBtn(bool lEnable)
        {
            if (lEnable)
            {
                EInvGrpBox.Enabled = true;
                BtnEInvGen.Enabled = true;
                EWBGrpBox.Enabled = true;
                txtEWBCanRem.Enabled = true;
                lblEWBCanRem.Enabled = true;
                lblEWBCanRes.Enabled = true;
                txtEWBCanRes.Enabled = true;
                TxtEWBVeh.Enabled = true;
                lblEWBVehNo.Enabled = true;
                BtnEWBDetPrn.Enabled = true;
                BtnEWBPrint.Enabled = true;
                BtnEWBGen.Enabled = true;
            }

            else
            {
                EInvGrpBox.Enabled = false;
                EWBGrpBox.Enabled = false;
                txtEWBCanRem.Enabled = false;
                lblEWBCanRem.Enabled = false;
                lblEWBCanRes.Enabled = false;
                txtEWBCanRes.Enabled = false;
                TxtEWBVeh.Enabled = false;
                lblEWBVehNo.Enabled = false;
                BtnEInvGen.Enabled = false;
                BtnEWBDetPrn.Enabled = false;
                BtnEWBPrint.Enabled = false;
                BtnEWBGen.Enabled = false;
            }
        }

        private void BillDetGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                MarkSelectedRow();
            }
            txtErrMsg.Text = EInvBillDetGrid.CurrentRow.Cells["ErrorList"].Value.ToString().Trim();
            txtInvDet.Text = "Inv. No. :" + EInvBillDetGrid.CurrentRow.Cells["DocDtls_No"].Value + "   IRN : " + EInvBillDetGrid.CurrentRow.Cells["IRN"].Value +
                            "  IRNAck : " + EInvBillDetGrid.CurrentRow.Cells["IRNACK"].Value + "\n" + "   EWB : " + EInvBillDetGrid.CurrentRow.Cells["EWBNo"].Value;
        }

        private void MarkSelectedRow()
        {
            var nRowIndex = EInvBillDetGrid.CurrentCell.RowIndex;
            var currentRow = EInvBillDetGrid.Rows[nRowIndex];
            var lTrueObj = currentRow.Cells[0].Value;
            var lTrue = lTrueObj != null && (bool)lTrueObj;
            var cBillNo = currentRow.Cells["DocDtls_No"]?.Value?.ToString();
            var cInvId = currentRow.Cells["InvId"]?.Value?.ToString();
            var nTotRow = EInvBillDetGrid.RowCount;
            if ((bool)lTrue)
            {
                EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.White;
                EInvBillDetGrid.Rows[nRowIndex].Cells[0].Value = false;
                for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
                {
                    if (cBillNo == EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value && cInvId == EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value)
                    {
                        EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.White;
                        EInvBillDetGrid.Rows[nRowIndex].Cells[0].Value = false;
                    }
                }
            }
            else
            {
                EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                EInvBillDetGrid.Rows[nRowIndex].Cells[0].Value = true;
                for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
                {
                    if (cBillNo == EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value && cInvId == EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value)
                    {
                        EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                        EInvBillDetGrid.Rows[nRowIndex].Cells[0].Value = true;
                    }
                }
            }
            Appmain.nChkRow = 0;

            for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
            {
                if (EInvBillDetGrid.Rows[nRowIndex].Cells[0].Value != null && (bool)EInvBillDetGrid.Rows[nRowIndex].Cells[0].Value)
                {
                    Appmain.nChkRow += 1;
                    CntrlBtn(true);
                    break;
                }
            }
            if (Appmain.nChkRow == 0)
            {
                CntrlBtn(false);
            }
        }

        private void BtnChkAll_Click(object sender, EventArgs e)
        {
            var Retval = Appmain.ChkAll(EInvBillDetGrid, true);
            CntrlBtn(true);
            if (!Retval)
            {
                BtnEInvGen.Enabled = false;
            }
        }
        private void BtnUnChkAll_Click(object sender, EventArgs e)
        {
            Appmain.UnChkAll(EInvBillDetGrid);
            CntrlBtn(false);
        }

        private async void BtnEInvGen_Click(object sender, EventArgs e)
        {
            ReqPlGenIRN eInvGen;
            EInvBillDetGrid.Enabled = false;
            bool IsIRNGen = false;
            List<string> BillNos = new List<string>();

            try
            {
                Appmain.WriteToErrorLog("****************************************************************************************************************");
                Appmain.WriteToErrorLog("Computer Name: " + Environment.MachineName);
                Appmain.WriteToErrorLog("Date/Time: " + DateTime.Now.ToString());
                Appmain.ResetGrid(EInvBillDetGrid, txtErrMsg);
                
                int nTotRec = EInvBillDetGrid.RowCount;
                string cBillNo, cInvId, cLstBillNo;
                Appmain.WriteToErrorLog("TotRec - " + nTotRec.ToString());
                foreach (DataGridViewRow row in EInvBillDetGrid.Rows)
                {
                    BillNos.Add(row.Cells["DocDtls_No"].Value.ToString().Trim());
                    if (row.Cells["IRN"].Value.ToString().Trim() != "")
                    {
                        MessageBox.Show("You are about to generate IRN but we detected some bills for which you have already generated IRN. Please start tool selecting proper criteria.");
                        //Application.Exit();
                    }
                }
                BillNos = BillNos.Distinct().ToList();
                cLstBillNo = "";
                foreach (DataGridViewRow row in EInvBillDetGrid.Rows)
                {
                    if ((bool)row.Cells[0].Value)
                    {
                        Appmain.WriteToErrorLog("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                        Appmain.WriteToErrorLog("Process BillNo : " + string.Join("', '", BillNos.ToArray())); 
                        Appmain.WriteToErrorLog("Process Start BillNo : " + row.Cells["DocDtls_No"].Value.ToString().Trim() + " Computer Name: " + Environment.MachineName + "Date/Time: " + DateTime.Now.ToString());
                        Appmain.WriteToErrorLog("******* start cLstBillNo : " + cLstBillNo.ToString() + " BillNo : " + row.Cells["DocDtls_No"].Value.ToString().Trim() + "*******");
                        Appmain.WriteToErrorLog(" RowIndex " + row.Index.ToString() + " IsIRNGen " + IsIRNGen.ToString() );
                        if (cLstBillNo != row.Cells["DocDtls_No"].Value.ToString().Trim())
                        {
                            IsIRNGen = false;
                        }
                        cBillNo = row.Cells["DocDtls_No"].Value.ToString().Trim();
                        cInvId = row.Cells["InvId"].Value.ToString().Trim();
                        Appmain.WriteToErrorLog(" cBillNo : " + cBillNo + " row get Bill No :"+ row.Cells["DocDtls_No"].Value.ToString().Trim());
                        
                        if (!IsIRNGen)
                        {
                        
                            Appmain.WriteToErrorLog("=============================================================================================");
                            Appmain.WriteToErrorLog(" InvId : " + cInvId + " cBillNo : " + cBillNo);
                            txtErrMsg.Text = "Process started for IRN generation for Doc No.: " + cBillNo;
                            Appmain.WriteToErrorLog(txtErrMsg.Text + " RowIndex " + row.Index.ToString());

                            eInvGen = new ReqPlGenIRN
                            {
                                Version = row.Cells["Version"].Value.ToString().Trim(),
                                TranDtls = new ReqPlGenIRN.TranDetails()
                            };

                            eInvGen.TranDtls.TaxSch = "GST";
                            eInvGen.TranDtls.SupTyp = row.Cells["TranDtls_SupTyp"].Value.ToString().Trim();
                            eInvGen.TranDtls.RegRev = "N";
                            eInvGen.TranDtls.EcmGstin = null;
                            eInvGen.TranDtls.IgstOnIntra = "N";

                            eInvGen.DocDtls = new ReqPlGenIRN.DocSetails
                            {
                                Typ = row.Cells["DocDtls_Typ"].Value.ToString().Trim(),
                                No = cBillNo,
                                Dt = row.Cells["DocDtls_Dt"].Value.ToString().Trim()
                            };

                            eInvGen.SellerDtls = new ReqPlGenIRN.SellerDetails
                            {
                                Gstin = row.Cells["SellerDtls_Gstin"].Value.ToString().Trim(),
                                LglNm = row.Cells["SellerDtls_LglNm"].Value.ToString().Trim(),
                                TrdNm = row.Cells["SellerDtls_TrdNm"].Value.ToString().Trim(),
                                Addr1 = row.Cells["SellerDtls_Addr1"].Value.ToString().Trim()
                            };

                            if (row.Cells["SellerDtls_Addr2"].Value.ToString().Trim() == "")
                            {
                                eInvGen.SellerDtls.Addr2 = null;
                            }
                            else
                            {
                                eInvGen.SellerDtls.Addr2 = row.Cells["SellerDtls_Addr2"].Value.ToString().Trim();
                            }
                            eInvGen.SellerDtls.Pin = int.Parse(row.Cells["SellerDtls_Pin"].Value.ToString().Trim());
                            eInvGen.SellerDtls.Loc = row.Cells["SellerDtls_Loc"].Value.ToString().Trim();
                            eInvGen.SellerDtls.Stcd = Appmain.RetStateCode(row.Cells["SellerDtls_State"].Value.ToString().Trim());
                            eInvGen.SellerDtls.Ph = null;
                            eInvGen.SellerDtls.Em = null;
                            eInvGen.BuyerDtls = new ReqPlGenIRN.BuyerDetails
                            {
                                Gstin = row.Cells["BuyerDtls_Gstin"].Value.ToString().Trim(),
                                LglNm = row.Cells["BuyerDtls_LglNm"].Value.ToString().Trim(),
                                TrdNm = row.Cells["BuyerDtls_TrdNm"].Value.ToString().Trim(),
                                Pos = row.Cells["BuyerDtls_Pos"].Value.ToString().Trim(),
                                Addr1 = row.Cells["BuyerDtls_Addr1"].Value.ToString().Trim()
                            };
                            if (row.Cells["BuyerDtls_Addr2"].Value.ToString().Trim() == "")
                            {
                                eInvGen.BuyerDtls.Addr2 = null;
                            }
                            else
                            {
                                eInvGen.BuyerDtls.Addr2 = row.Cells["BuyerDtls_Addr2"].Value.ToString().Trim();
                            }
                            eInvGen.BuyerDtls.Loc = row.Cells["BuyerDtls_Loc"].Value.ToString().Trim();
                            eInvGen.BuyerDtls.Pin = int.Parse(row.Cells["BuyerDtls_Pin"].Value.ToString().Trim());
                            eInvGen.BuyerDtls.Stcd = Appmain.RetStateCode(row.Cells["BuyerDtls_State"].Value.ToString().Trim());


                            if (row.Cells["DispDtls_Nm"].Value.ToString().Trim() != "")
                            {
                                eInvGen.DispDtls = new ReqPlGenIRN.DispatchedDetails
                                {
                                    Nm = row.Cells["DispDtls_Nm"].Value.ToString().Trim(),
                                    Addr1 = row.Cells["DispDtls_Addr1"].Value.ToString().Trim(),
                                    Addr2 = row.Cells["DispDtls_Addr2"].Value.ToString().Trim(),
                                    Loc = row.Cells["DispDtls_Loc"].Value.ToString().Trim(),
                                    Pin = int.Parse(row.Cells["DispDtls_Pin"].Value.ToString().Trim()),
                                    Stcd = row.Cells["DispDtls_Stcd"].Value.ToString().Trim()
                                };
                            }
                            else
                            {
                                eInvGen.DispDtls = null;
                            }

                            if (row.Cells["ShipDtls_Gstin"].Value.ToString().Trim() != "")
                            {
                                eInvGen.ShipDtls = new ReqPlGenIRN.ShippedDetails
                                {
                                    Gstin = row.Cells["ShipDtls_Gstin"].Value.ToString().Trim(),
                                    LglNm = row.Cells["ShipDtls_LglNm"].Value.ToString().Trim(),
                                    TrdNm = row.Cells["ShipDtls_TrdNm"].Value.ToString().Trim(),
                                    Addr1 = row.Cells["ShipDtls_Addr1"].Value.ToString().Trim()
                                };
                                if (row.Cells["ShipDtls_Addr2"].Value.ToString().Trim() == "")
                                {
                                    eInvGen.ShipDtls.Addr2 = null;
                                }
                                else
                                {
                                    eInvGen.ShipDtls.Addr2 = row.Cells["ShipDtls_Addr2"].Value.ToString().Trim();
                                }
                                eInvGen.ShipDtls.Loc = row.Cells["ShipDtls_Loc"].Value.ToString().Trim();
                                eInvGen.ShipDtls.Pin = int.Parse(row.Cells["ShipDtls_Pin"].Value.ToString().Trim());
                                eInvGen.ShipDtls.Stcd = row.Cells["ShipDtls_StCd"].Value.ToString().Trim();
                            }
                            else
                            {
                                eInvGen.ShipDtls = null;
                            }

                            eInvGen.PayDtls = null;
                            eInvGen.RefDtls = null;
                            eInvGen.AddlDocDtls = null;
                            eInvGen.ExpDtls = null;

                            eInvGen.ValDtls = new ReqPlGenIRN.ValDetails
                            {
                                AssVal = Convert.ToDouble(row.Cells["ValDtls_AssVal"].Value.ToString()),
                                CgstVal = Convert.ToDouble(row.Cells["ValDtls_CgstVal"].Value.ToString()),
                                SgstVal = Convert.ToDouble(row.Cells["ValDtls_SgstVal"].Value.ToString()),
                                IgstVal = Convert.ToDouble(row.Cells["ValDtls_IgstVal"].Value.ToString()),
                                CesVal = Convert.ToDouble(row.Cells["ValDtls_CesVal"].Value.ToString()),
                                StCesVal = Convert.ToDouble(row.Cells["ValDtls_StCesVal"].Value.ToString()),
                                RndOffAmt = Convert.ToDouble(row.Cells["ValDtls_RndOffAmt"].Value.ToString()),
                                OthChrg = Convert.ToDouble(row.Cells["ValDtls_OthChrg"].Value.ToString()),
                                TotInvVal = Convert.ToDouble(row.Cells["ValDtls_TotInvVal"].Value.ToString())
                            };

                            eInvGen.EwbDtls = null;
                            eInvGen.ItemList = new List<ReqPlGenIRN.ItmList>();
                            ReqPlGenIRN.ItmList itm = new ReqPlGenIRN.ItmList();
                            foreach (DataGridViewRow DetRow in EInvBillDetGrid.Rows)
                            {
                                //Appmain.WriteToErrorLog(cInvId + " - cInvId");
                                //Appmain.WriteToErrorLog(DetRow.Cells["InvId"].Value.ToString() + " - InvId");
                                if (cInvId == DetRow.Cells["InvId"].Value.ToString().Trim())
                                {
                                    Appmain.WriteToErrorLog("Item Details for Doc No.: " + cBillNo + " Row Index : " + DetRow.Index.ToString());
                                    itm = new ReqPlGenIRN.ItmList
                                    {
                                        SlNo = DetRow.Cells["itm_SlNo"].Value.ToString().Trim(),
                                        IsServc = DetRow.Cells["itm_IsServc"].Value.ToString().Trim(),
                                        PrdDesc = DetRow.Cells["itm_PrdDesc"].Value.ToString().Trim(),
                                        HsnCd = DetRow.Cells["itm_HsnCd"].Value.ToString().Trim(),
                                        BchDtls = null,
                                        Qty = Convert.ToDouble(DetRow.Cells["itm_Qty"].Value),
                                        Unit = DetRow.Cells["itm_Unit"].Value.ToString().Trim(),
                                        UnitPrice = Convert.ToDouble(DetRow.Cells["itm_UnitPrice"].Value),
                                        TotAmt = Convert.ToDouble(DetRow.Cells["itm_TotAmt"].Value),
                                        Discount = Convert.ToDouble(DetRow.Cells["itm_Discount"].Value),
                                        AssAmt = Convert.ToDouble(DetRow.Cells["itm_AssAmt"].Value),
                                        GstRt = Convert.ToDouble(DetRow.Cells["itm_GstRt"].Value),
                                        SgstAmt = Convert.ToDouble(DetRow.Cells["itm_SgstAmt"].Value),
                                        IgstAmt = Convert.ToDouble(DetRow.Cells["itm_IgstAmt"].Value),
                                        CgstAmt = Convert.ToDouble(DetRow.Cells["itm_CgstAmt"].Value),
                                        CesRt = Convert.ToDouble(DetRow.Cells["itm_CesRt"].Value),
                                        CesAmt = Convert.ToDouble(DetRow.Cells["itm_CesAmt"].Value),
                                        CesNonAdvlAmt = Convert.ToDouble(DetRow.Cells["itm_CesNonAdvlAmt"].Value),
                                        StateCesRt = Convert.ToDouble(DetRow.Cells["itm_StateCesRt"].Value),
                                        StateCesAmt = Convert.ToDouble(DetRow.Cells["itm_StateCesAmt"].Value),
                                        StateCesNonAdvlAmt = Convert.ToDouble(DetRow.Cells["itm_StateCesNonAdvlAmt"].Value),
                                        OthChrg = Convert.ToDouble(DetRow.Cells["itm_OthChrg"].Value),
                                        TotItemVal = Convert.ToDouble(DetRow.Cells["itm_TotItemVal"].Value),
                                        AttribDtls = null

                                    };
                                    eInvGen.ItemList.Add(itm);
                                }

                            }

                            Appmain.WriteToErrorLog("JSON " + JsonConvert.SerializeObject(eInvGen) + " for Doc No.: " + cBillNo + " Row Index : " + row.Index.ToString());

                            Appmain.WriteToErrorLog("Before CallEInvGenAPI for Doc No.: " + cBillNo + " Row Index : " + row.Index.ToString());


                            TaxProEInvoice.API.TxnRespWithObj<RespPlGenIRN> TxnResp = await eInvoiceAPI.GenIRNAsync(Appmain.eInvSession, eInvGen, 250);

                            Appmain.WriteToErrorLog("TxnResp -" + TxnResp.IsSuccess.ToString() + " for Doc No.: " + cBillNo  + " Row Index : " + row.Index.ToString());

                            Appmain.WriteToErrorLog("After API Call in CallEInvGen Method for Doc No.: " + cBillNo + " RowIndex " + row.Index.ToString());
                            if (TxnResp != null)
                            {
                                IsIRNGen = await ProcessEInvAPIResponse(TxnResp, EInvBillDetGrid, row.Index, true);
                            }
                            else
                            {
                                Appmain.WriteToErrorLog("API response returned null/error for Doc No.: " + cBillNo + " RowIndex " + row.Index.ToString() + " Error : " + TxnResp.ErrorDetails.ToString());
                            }
                            cLstBillNo = cBillNo;
                            Appmain.WriteToErrorLog("End BillNo : " + row.Cells["DocDtls_No"].Value.ToString().Trim() + " Computer Name: " + Environment.MachineName + "Date/Time: " + DateTime.Now.ToString());
                            Appmain.WriteToErrorLog("******* End cLstBillNo : " + cLstBillNo.ToString() + " cBillNo : " + cBillNo.ToString() + "*******");
                            Appmain.WriteToErrorLog("After CallEInvGenAPI for Doc No.: " + cBillNo + " Row Index : " + row.Index.ToString());
                            Appmain.WriteToErrorLog("Process ended for IRN generation for Doc No.: " + cBillNo + " Row Index : " + row.Index.ToString());
                            Appmain.WriteToErrorLog("=============================================================================================");
                        }
                    }
                    else
                    {
                        Appmain.WriteToErrorLog("Skipping Rows as found same Doc No " + row.Cells["DocDtls_No"].Value.ToString() + " with different Item @ Row no. : " + row.Index.ToString());
                    }
                    Appmain.WriteToErrorLog("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in IRN Generation: " + ex.Message);
            }
            EInvBillDetGrid.Enabled = true;
        }

        public async Task CallEInvGenAPI(eInvoiceSession eInvSession, ReqPlGenIRN eInvGen, int nRowIndex)
        {
            MessageBox.Show("Call");
            Appmain.WriteToErrorLog("Before API Call in CallEInvGen Method. RowIndex " + nRowIndex.ToString());
            try
            {
                MessageBox.Show(JsonConvert.SerializeObject(eInvSession) + " einvoicesession");
                MessageBox.Show(JsonConvert.SerializeObject(eInvGen) + " json");
                TaxProEInvoice.API.TxnRespWithObj<RespPlGenIRN> TxnResp = await eInvoiceAPI.GenIRNAsync(eInvSession, eInvGen, 250);

                MessageBox.Show("Call1");
                Appmain.WriteToErrorLog("After API Call in CallEInvGen Method" + " RowIndex " + nRowIndex.ToString());
                if (TxnResp != null)
                {
                    await ProcessEInvAPIResponse(TxnResp, EInvBillDetGrid, nRowIndex, true);
                }

            }
            catch (Exception ex)
            {
                Appmain.WriteToErrorLog(ex.Message.ToString() + ex.InnerException.ToString() + " RowIndex " + nRowIndex.ToString());
            }
        }

        private async void BtnGetDetail_Click(object sender, EventArgs e)
        {
            string DocNo, DocDate, DocType;
            int nRowIndex;
            DialogResult result = MessageBox.Show("IRN can be retrieved using this API within 2 days from the date of generation of IRN.\n\nStill you want to proceed?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    CntrlBtn(true);
                    Appmain.WriteToErrorLog("****************************************************************************************************************");
                    Appmain.WriteToErrorLog("Computer Name: " + Environment.MachineName);
                    Appmain.WriteToErrorLog("Date/Time: " + DateTime.Now.ToString());

                    nRowIndex = 0;

                    Appmain.ResetGrid(EInvBillDetGrid, txtErrMsg);
                    string LstDocNo = "";
                    foreach (DataGridViewRow row in EInvBillDetGrid.Rows)
                    {
                        DocNo = row.Cells["DocDtls_No"].Value.ToString().Trim();
                        DocDate = row.Cells["DocDtls_Dt"].Value.ToString().Trim();
                        DocType = row.Cells["DocDtls_Typ"].Value.ToString().Trim();
                        Appmain.WriteToErrorLog("===============================================================================================");
                        Appmain.WriteToErrorLog("Initializing Get Detail Processes for " + DocNo + " RowIndex " + nRowIndex.ToString());
                        if (row.Cells[0].Value != null && (bool)row.Cells[0].Value)
                        {
                            if (LstDocNo != DocNo)
                            {
                                Appmain.WriteToErrorLog("Getting IRN details for Bill No." + row.Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex : " + nRowIndex.ToString());
                                if (!string.IsNullOrEmpty(DocNo) && !string.IsNullOrEmpty(DocDate))
                                {
                                    TaxProEInvoice.API.TxnRespWithObj<RespPlGenIRN> txnRespWithObj = await eInvoiceAPI.GetIRNDetailsByDocDetailsAsync(Appmain.eInvSession, DocType, DocNo, DocDate);
                                    Appmain.WriteToErrorLog("Before API call for  Getting IRN details for Bill No." + row.Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex : " + nRowIndex.ToString());
                                    //await ProcessEInvAPIResponse(txnRespWithObj, EInvBillDetGrid, nRowIndex, false);
                                    await ProcessEInvAPIResponse(txnRespWithObj, EInvBillDetGrid, row.Index, false);
                                    Appmain.WriteToErrorLog("After completing API call for Getting IRN details for Bill No." + row.Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex : " + nRowIndex.ToString());
                                }
                                LstDocNo = DocNo;
                            }
                        }
                        else
                        {
                            Appmain.WriteToErrorLog("Initializing failed Get Detail Process for not selected Bill No. " + DocNo + " RowIndex " + nRowIndex.ToString());
                        }
                        Appmain.WriteToErrorLog("Finished Process for Bill No. " + DocNo + " RowIndex " + nRowIndex.ToString());

                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in Get Detail by IRN: " + ex.Message);
                }
            }
        }

        public async Task<bool> ProcessEInvAPIResponse(TaxProEInvoice.API.TxnRespWithObj<RespPlGenIRN> TxnResp, DataGridView EInvBillDetGrid, int nRowIndex, bool lNew)
        {
            bool IsIRNGen = false;
            var cMsg = "";
            string cMsgDet;
            string strSelect, cTblName, cIdFld;
            //Appmain.WriteToErrorLog("112A");
            Appmain.WriteToErrorLog("In ProcessEInvAPIResponse Method - 1 for Doc No. " + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString() + " RowIndex " + nRowIndex.ToString());
            try
            {
                if (lNew)
                {
                    cMsgDet = "E-Invoice Bill Generation";
                }
                else
                {
                    cMsgDet = "Get E-Invoice Bill Detail ";
                }

                if (TxnResp.IsSuccess)
                {
                    //Appmain.WriteToErrorLog("112B");
                    string cAPIResponse = JsonConvert.SerializeObject(TxnResp.RespObj);
                    Appmain.WriteToErrorLog("Success Response " + cAPIResponse + "\n" + cMsgDet + " RowIndex " + nRowIndex.ToString());
                    RespPlGenIRN respPlGenIRN = TxnResp.RespObj;
                    Appmain.WriteToErrorLog("In ProcessEInvAPIResponse Method - 2" + " RowIndex " + nRowIndex.ToString());
                    if (lNew)
                    {
                        respPlGenIRN.QrCodeImage.Save(path + EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value + ".png");
                    }
                    Appmain.WriteToErrorLog("In ProcessEInvAPIResponse Method - 3" + " RowIndex " + nRowIndex.ToString());
                    var cIRN = respPlGenIRN.Irn.ToString();
                    var cIRNAck = respPlGenIRN.AckNo;
                    var cEwayBillNo = (!string.IsNullOrEmpty(respPlGenIRN.EwbNo) ? respPlGenIRN.EwbNo : "");
                    var cEWayBillDate = respPlGenIRN.EwbDt;
                    var cIRNDtTime = respPlGenIRN.AckDt;
                    var cEwbDt = (!string.IsNullOrEmpty(cEWayBillDate) ? cEWayBillDate.ToString() : "null");
                    cTblName = (string)EInvBillDetGrid.Rows[nRowIndex].Cells["TblName"].Value.ToString().Trim();
                    cIdFld = (string)EInvBillDetGrid.Rows[nRowIndex].Cells["IdFld"].Value.ToString().Trim();
                    Appmain.WriteToErrorLog("In ProcessEInvAPIResponse Method - 4 - Before Update IRN" + " RowIndex " + nRowIndex.ToString());
                    //Appmain.WriteToErrorLog("112C");
                    //MessageBox.Show(cTblName.ToString());
                    //MessageBox.Show(cIRN.ToString());
                    //MessageBox.Show(cIRNAck.ToString());
                    //MessageBox.Show(cEwayBillNo.ToString());
                    //MessageBox.Show(cIRNDtTime.ToString());
                    //MessageBox.Show(cEwbDt.ToString());

                    if (lNew)
                    {
                        strSelect = " Update " + cTblName + " Set IRN = '" + cIRN.ToString() + "', IRNAck = '" + cIRNAck.ToString() + "', IRNDt = '" + cIRNDtTime.ToString() +
                                    "' where " + cIdFld + " = '" + EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value.ToString() + "'";
                    }
                    else
                    {
                        strSelect = " Update " + cTblName + " Set IRN = '" + cIRN.ToString() + "', IRNAck = '" + cIRNAck.ToString() + "', EwbNo = '" + cEwayBillNo.ToString() +
                                    "', IRNDt = '" + cIRNDtTime.ToString() + "', EwbDt = '" + cEwbDt.ToString() + "'" +
                                    " where " + cIdFld + " = '" + EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value.ToString() + "'";
                    }

                    SqlCommand Command = new SqlCommand();
                    Appmain.WriteToErrorLog("In ProcessEInvAPIResponse Method - 5 - After Update IRN" + " RowIndex " + nRowIndex.ToString());
                    Appmain.WriteToErrorLog("E-Invoice detail updation query \n" + strSelect + " RowIndex " + nRowIndex.ToString());

                    Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    //Appmain.WriteToErrorLog("112D");
                    //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    //''''''   Saving IRN Data Details
                    //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    string cTime;
                    cTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
                    Appmain.WriteToErrorLog("In ProcessEInvAPIResponse Method - 6 - Before Saving IRN data" + " RowIndex " + nRowIndex.ToString());

                    strSelect = "Delete from EInvIRNMst where ModId= '" + EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value.ToString() + "'";
                    Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    //Appmain.WriteToErrorLog("112E");
                    strSelect = "Delete from EInvIRNDet where ModId= '" + EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value.ToString() + "'";
                    Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    //Appmain.WriteToErrorLog("112F");
                    //MessageBox.Show(EInvBillDetGrid.Rows[nRowIndex].Cells["ModCode"].Value.ToString());
                    //MessageBox.Show(EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value.ToString());
                    //MessageBox.Show(cEwayBillNo);//
                    //MessageBox.Show(cIRNAck);
                    //MessageBox.Show(cIRNDtTime);
                    //MessageBox.Show(cIRN);
                    //MessageBox.Show(cTime);
                    //Appmain.WriteToErrorLog("112F");
                    strSelect = " insert into EInvIRNMst ( ModCode, ModId, EWBNo,EWBDt,IRNAck, IRNAckDtTm, IRN, ECInsDate, ECInsTime )" +
                                " VALUES (" +
                                "'" + EInvBillDetGrid.Rows[nRowIndex].Cells["ModCode"].Value.ToString() + "'," +
                                "'" + EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value.ToString() + "'," +
                                "'" + cEwayBillNo + "'," +
                                "'" + (!string.IsNullOrEmpty(cEWayBillDate) ? cEWayBillDate.ToString() : "null") + "'," +
                                "'" + cIRNAck.ToString() + "'," +
                                "'" + cIRNDtTime + "'," +
                                "'" + cIRN.ToString() + "'" +
                                ", GetDate(), '" + cTime.ToString() + "') ";

                    Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Appmain.WriteToErrorLog("In ProcessEInvAPIResponse Method - 7 - After Saving IRN data" + " RowIndex " + nRowIndex.ToString());
                    //Appmain.WriteToErrorLog("112G");

                    string strSignedQR = respPlGenIRN.SignedQRCode.ToString();
                    //Appmain.WriteToErrorLog("112G1");
                    List<string> QRArray = DevideByStringLength(strSignedQR, 1000);
                    //string[] QRArray = Commfunc.VToken(strSignedQR, 1000);
                    Appmain.WriteToErrorLog(QRArray.Count.ToString() + "QrArray-Count");
                    int QRCtr = 0;
                    if (QRArray.Count > 0)
                    {
                        foreach (var QR in QRArray)
                        {
                            if (QR != null)
                            {
                                strSelect = "insert into EInvIRNDet ( ModCode, ModId, Srl, IRNData, ECInsDate, ECInsTime)" +
                                            " VALUES (" +
                                            "'" + EInvBillDetGrid.Rows[nRowIndex].Cells["ModCode"].Value.ToString() + "'," +
                                            "'" + EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value.ToString() + "'," +
                                           (QRCtr + 1) + "," +
                                            "'" + QRArray[QRCtr].ToString() + "'" +
                                            ", GetDate(), '" + cTime.ToString() + "') ";
                                Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                                Command.ExecuteNonQuery();
                                Command.Dispose();
                                //Appmain.WriteToErrorLog("112H");
                                Appmain.WriteToErrorLog("E-Invoice EInvIRNDet insert query \n" + strSelect);
                            }
                        }
                    }

                    //Appmain.WriteToErrorLog("112I");
                    Appmain.WriteToErrorLog("In ProcessEInvAPIResponse Method - 8 - After Saving IRN data 2" + " RowIndex " + nRowIndex.ToString());

                    //EInvBillDetGrid.Rows[nRowIndex].Cells["IRN"].Value = cIRN;
                    //EInvBillDetGrid.Rows[nRowIndex].Cells["IRNAck"].Value = cIRNAck;
                    //EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    //EInvBillDetGrid.Rows[nRowIndex].Cells["EWbNo"].Value = cEwayBillNo;

                    string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();

                    foreach (DataGridViewRow Row in EInvBillDetGrid.Rows)
                    {
                        if (DocNo == Row.Cells["DocDtls_No"].ToString().Trim())
                        {
                            Row.Cells["IRN"].Value = cIRN;
                            Row.Cells["IRNAck"].Value = cIRNAck;
                            Row.DefaultCellStyle.ForeColor = Color.Blue;
                            Row.Cells["EWbNo"].Value = cEwayBillNo;
                        }
                    }
                    //Appmain.WriteToErrorLog("112J");
                    if (lNew)
                    {
                        cMsg = "Generated IRN Details for Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim() + " => Ack No : " + cIRNAck + " - IRN : " + cIRN;
                        Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());
                    }
                    else
                    {
                        cMsg = "Fetched IRN Details for Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim() + " => Ack No : " + cIRNAck + " - IRN : " + cIRN + " - EWB : " + cEwayBillNo;
                        Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());
                    }
                    txtErrMsg.Text = cMsg;
                    //Appmain.WriteToErrorLog("112K");
                    EInvBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;

                    foreach (DataGridViewRow Row in EInvBillDetGrid.Rows)
                    {
                        if (DocNo == Row.Cells["DocDtls_No"].ToString().Trim())
                        {
                            Row.Cells["ErrorList"].Value = cMsg;
                        }
                    }
                    //Appmain.WriteToErrorLog("112L");
                    IsIRNGen = true;
                }
                else
                {
                    //Appmain.WriteToErrorLog("112M");
                    //EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //EInvBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = TxnResp.TxnOutcome;

                    string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();

                    foreach (DataGridViewRow Row in EInvBillDetGrid.Rows)
                    {
                        if (DocNo == Row.Cells["DocDtls_No"].Value.ToString().Trim())
                        {
                            Row.Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                            Row.DefaultCellStyle.ForeColor = Color.Red;
                        }

                    }
                    txtErrMsg.Text = TxnResp.TxnOutcome;
                    Appmain.WriteToErrorLog("Invoice Failure Response \n" + TxnResp.TxnOutcome + " RowIndex " + nRowIndex.ToString());
                }
                //Appmain.WriteToErrorLog("112N");
                txtInvDet.Text = "Inv. No. :" + EInvBillDetGrid.CurrentRow.Cells["DocDtls_No"].Value + "   IRN : " + EInvBillDetGrid.CurrentRow.Cells["IRN"].Value +
                                " IRNAck : " + EInvBillDetGrid.CurrentRow.Cells["IRNACK"].Value + "\n" + "   EWB : " + EInvBillDetGrid.CurrentRow.Cells["EWBNo"].Value;
            }
            catch (Exception ex)
            {
                Appmain.WriteToErrorLog("IRN Response Process Error : " + ex.Message);
                Appmain.WriteToErrorLog(ex.Message.ToString() + ex.InnerException.ToString() + " RowIndex " + nRowIndex.ToString());
            }
            //Appmain.WriteToErrorLog("114");
            Appmain.WriteToErrorLog("ProcessEInvAPIResponse Method - IsIRNGen " + IsIRNGen);
            Appmain.WriteToErrorLog("Out from ProcessEInvAPIResponse Method for RowIndex " + nRowIndex.ToString());
            return IsIRNGen;
        }

        public List<string> DevideByStringLength(string text, int chunkSize)
        {
            double a = (double)text.Length / chunkSize;
            var numberOfChunks = Math.Ceiling(a);

            Console.WriteLine($"{text.Length} | {numberOfChunks}");

            List<string> chunkList = new List<string>();
            for (int i = 0; i < numberOfChunks; i++)
            {
                string subString = string.Empty;
                if (i == (numberOfChunks - 1))
                {
                    subString = text.Substring(chunkSize * i,
                                               text.Length - chunkSize * i);
                    chunkList.Add(subString);
                    continue;
                }
                subString = text.Substring(chunkSize * i, chunkSize);
                chunkList.Add(subString);
            }
            return chunkList;
        }
        public async void BtnEWBGen_Click(object sender, EventArgs e)
        {
            string IRN;
            try
            {
                Appmain.WriteToErrorLog("****************************************************************************************************************");
                Appmain.WriteToErrorLog("Computer Name: " + Environment.MachineName);
                Appmain.WriteToErrorLog("Date/Time: " + DateTime.Now.ToString());

                Appmain.ResetGrid(EInvBillDetGrid, txtErrMsg);
                string LstDocNo = "";

                foreach (DataGridViewRow Row in EInvBillDetGrid.Rows)
                {
                    if (Row.Cells["EWBNo"].ToString().Trim() != "")
                    {
                        MessageBox.Show("You are about to generate EWB but we detected some bills for which you have already generated EWB. Please start tool selecting proper criteria.");
                        //System.Windows.Forms.Application.Exit();
                    }
                }
                Appmain.WriteToErrorLog(" EInvBillDetGrid.Rows.count - " + EInvBillDetGrid.Rows.Count.ToString());
                foreach (DataGridViewRow Row in EInvBillDetGrid.Rows)
                {
                    if ((bool)Row.Cells[0].Value)//if ((bool)Row.Cells[0].Value == true)
                    {
                        if (LstDocNo != Row.Cells["DocDtls_No"].Value.ToString().Trim())
                        {
                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("Generating EWB for Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim());
                            }
                            Appmain.WriteToErrorLog("========================================================================================");
                            Appmain.WriteToErrorLog("Generating EWB for Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex " + Row.Index.ToString());
                            //'IRN = "2dc6c50eed828551b3913ec69aae61bde43606cfe5baea301cc8c084258be366"
                            IRN = Row.Cells["IRN"].Value.ToString().Trim();
                            Appmain.WriteToErrorLog("IRN - " + IRN.ToString());

                            if (IRN.Length == 64)
                            {
                                Appmain.WriteToErrorLog("EWBA1");
                                ReqPlGenEwbByIRN reqPlGenEwbByIRN = new ReqPlGenEwbByIRN();

                                reqPlGenEwbByIRN.Irn = Row.Cells["IRN"].Value.ToString().Trim();
                                if (Row.Cells["TransId"].Value.ToString().Trim().Length == 15)
                                {
                                    reqPlGenEwbByIRN.TransId = Row.Cells["TransId"].Value.ToString().Trim();
                                }
                                if (!string.IsNullOrEmpty(Row.Cells["TransDocNo"].Value.ToString().Trim()))
                                {
                                    reqPlGenEwbByIRN.TransDocNo = Row.Cells["TransDocNo"].Value.ToString().Trim();
                                }
                                reqPlGenEwbByIRN.TransName = Row.Cells["TransName"].Value.ToString().Trim();
                                Appmain.WriteToErrorLog("EWBA2");
                                if (txtkm.Text == "")
                                {
                                    reqPlGenEwbByIRN.Distance = Convert.ToInt32(Row.Cells["Distance"].Value.ToString());
                                }
                                else
                                {
                                    reqPlGenEwbByIRN.Distance = Convert.ToInt32(txtkm.Text);
                                }
                                Appmain.WriteToErrorLog("EWBA3");
                                if (TxtEWBVeh.Text != "")
                                {
                                    reqPlGenEwbByIRN.VehNo = TxtEWBVeh.Text.ToString().Trim();
                                    reqPlGenEwbByIRN.TransMode = "1";
                                    reqPlGenEwbByIRN.VehType = "R";
                                }
                                Appmain.WriteToErrorLog("EWBA4");
                                object ErrorCodes, ErrorDesc, cResponse;

                                TaxProEInvoice.API.TxnRespWithObj<RespPlGenEwbByIRN> txnRespWithObj = await eInvoiceAPI.GenEwbByIRNAsync(Appmain.eInvSession, reqPlGenEwbByIRN);
                                if (txnRespWithObj.IsSuccess)
                                {
                                    Appmain.WriteToErrorLog("EWBA5");
                                    cResponse = JsonConvert.SerializeObject(txnRespWithObj.RespObj);
                                    Appmain.WriteToErrorLog("EWay Bill By IRN Generation Success Response " + cResponse + " RowIndex " + Row.Index.ToString());
                                    //'Dim result As JObject = JObject.Parse(cResponse)

                                    string cEwayBillNo = txnRespWithObj.RespObj.EwbNo.ToString();
                                    string cewayBillDate = txnRespWithObj.RespObj.EwbDt.ToString();

                                    string cTblName, cIdFld;
                                    cTblName = Row.Cells["TblName"].ToString().Trim();
                                    cIdFld = Row.Cells["IdFld"].ToString().Trim();
                                    string strSelect = " Update " + cTblName + " Set EwbNo = '" + cEwayBillNo.ToString() + "', EwbDt = '" + cewayBillDate.ToString() + "'" +
                                                       " where " + cIdFld + " = '" + Row.Cells["InvId"].Value.ToString() + "'";
                                    SqlDataAdapter da = new SqlDataAdapter();
                                    SqlCommand Command = new SqlCommand();

                                    Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                                    da.UpdateCommand = new SqlCommand(strSelect, Appmain.sqlCnn);
                                    da.UpdateCommand.ExecuteNonQuery();
                                    Command.Dispose();
                                    Appmain.WriteToErrorLog("EWBA6");
                                    string cMsg = "Generated EWay Bill No: By IRN " + cEwayBillNo + " for Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                    txtErrMsg.Text = cMsg;
                                    Row.Cells["ErrorList"].Value = cMsg;
                                    Row.Cells["EWbNo"].Value = cEwayBillNo;
                                    Row.DefaultCellStyle.ForeColor = Color.Blue;

                                    string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();

                                    foreach (DataGridViewRow Row2 in EInvBillDetGrid.Rows)
                                    {
                                        if (DocNo == Row2.Cells["DocDtls_No"].Value.ToString().Trim())
                                        {
                                            Row2.Cells["EWbNo"].Value = cEwayBillNo;
                                            Row2.DefaultCellStyle.ForeColor = Color.Blue;
                                            Row2.Cells["ErrorList"].Value = cMsg;
                                        }

                                    }
                                    Appmain.WriteToErrorLog("EWBA7");
                                    Appmain.WriteToErrorLog(cMsg + " RowIndex " + Row.Index.ToString());
                                }
                                else
                                {
                                    Appmain.WriteToErrorLog("EWBA8");
                                    Row.DefaultCellStyle.ForeColor = Color.Red;
                                    cResponse = "";
                                    if (txnRespWithObj.ErrorDetails != null)
                                    {
                                        ErrorCodes = "";
                                        ErrorDesc = "";
                                        foreach (TaxProEInvoice.API.RespErrDetailsPl errPl in txnRespWithObj.ErrorDetails)
                                        {
                                            ErrorCodes += errPl.ErrorCode + ",";
                                            ErrorDesc += errPl.ErrorCode + ": " + errPl.ErrorMessage + Environment.NewLine;
                                            cResponse = ErrorDesc;

                                        }

                                        Row.Cells["ErrorList"].Value = cResponse;

                                        string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();

                                        foreach (DataGridViewRow row2 in EInvBillDetGrid.Rows)
                                        {
                                            if (DocNo == row2.Cells["DocDtls_No"].Value.ToString().Trim())
                                            {
                                                row2.Cells["ErrorList"].Value = cResponse;
                                                row2.DefaultCellStyle.ForeColor = Color.Red;

                                            }
                                        }
                                        Appmain.WriteToErrorLog("EWay Bill by IRN Failure Response \n" + cResponse + " Sent JSON: " + JsonConvert.SerializeObject(reqPlGenEwbByIRN) + " RowIndex " + Row.Index.ToString());
                                    }
                                    Appmain.WriteToErrorLog("EWBA9");
                                }
                            }
                            else
                            {
                                Appmain.WriteToErrorLog("EWBA10");
                                string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();

                                foreach (DataGridViewRow row2 in EInvBillDetGrid.Rows)
                                {
                                    if (DocNo == row2.Cells["DocDtls_No"].Value.ToString().Trim())
                                    {
                                        row2.DefaultCellStyle.ForeColor = Color.Red;
                                        row2.Cells["ErrorList"].Value = "IRN is not generated for Bill No." + row2.Cells["DocDtls_No"].Value.ToString().Trim();
                                        Appmain.WriteToErrorLog("IRN is not generated for Bill No." + row2.Cells["DocDtls_No"].Value.ToString().Trim() + ". EWB not genereated." + " RowIndex " + Row.Index.ToString());
                                    }
                                }
                            }
                            LstDocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                            Appmain.WriteToErrorLog("Finished Generating EWB for Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex " + Row.Index.ToString());
                            Appmain.WriteToErrorLog("EWBA11");
                        }
                    }
                    txtInvDet.Text = "Inv. No. :" + Row.Cells["DocDtls_No"].Value + "   IRN : " + Row.Cells["IRN"].Value +
                                     "   IRNAck : " + Row.Cells["IRNACK"].Value + "\n" + "   EWB : " + Row.Cells["EWBNo"].Value;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("EWB Generation Error : " + ex.Message);
            }
        }
        private void BtnDebug_Click(object sender, EventArgs e)
        {
            if (Appmain.nDebug == 0)
            {
                Appmain.nDebug = 1;
            }
            else
            {
                Appmain.nDebug = 0;
            }
            if (Appmain.nDebug == 0)
            {
                var Pwd = Interaction.InputBox("Enter Key", "Enable Debug Mode", "");
                if (Pwd == "1723306907")
                {
                    Appmain.nDebug = 0;
                }
                else
                {
                    MessageBox.Show("Please enter valid key.");
                    Appmain.nDebug = 1;
                }
            }
        }

        private async void BtnEWBCancel_Click(object sender, EventArgs e)
        {
            string cEWBNo;

            try
            {
                Appmain.WriteToErrorLog("****************************************************************************************************************");
                Appmain.WriteToErrorLog("Computer Name: " + Environment.MachineName);
                Appmain.WriteToErrorLog("Date/Time: " + DateTime.Now.ToString());

                Appmain.ResetGrid(EInvBillDetGrid, txtErrMsg);

                if (txtEWBCanRes.Text != "" && txtEWBCanRem.Text != " ")
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand Command = new SqlCommand();
                    string cMsg = "";
                    foreach (DataGridViewRow Row in EInvBillDetGrid.Rows)
                    {
                        string LstDocNo = "";

                        cEWBNo = Row.Cells["EWbNo"].Value.ToString().Trim();
                        cMsg = "Cancelling E-Way Bill No: " + cEWBNo + " of Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim();
                        txtErrMsg.Text = cMsg;
                        EInvBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                        Appmain.WriteToErrorLog(cMsg + " RowIndex " + Row.Index.ToString());

                        if ((bool)Row.Cells[0].Value == true && cEWBNo != "")
                        {
                            //'And EInvBillDetGrid.Rows(nRowIndex).Cells(1).Value <> True
                            if (Row.Cells["DocDtls_No"].Value.ToString().Trim() != LstDocNo)
                            {
                                ReqPlCancelEWB reqPlCancelEWB = new ReqPlCancelEWB();
                                reqPlCancelEWB.ewbNo = long.Parse(cEWBNo);
                                if (txtEWBCanRes.Text == "Duplicate")
                                {
                                    reqPlCancelEWB.cancelRsnCode = 1;
                                }
                                else if (txtEWBCanRes.Text == "Order Cancelled")
                                {
                                    reqPlCancelEWB.cancelRsnCode = 2;
                                }
                                else if (txtEWBCanRes.Text == "Data Entry Mistake")
                                {
                                    reqPlCancelEWB.cancelRsnCode = 3;
                                }
                                else if (txtEWBCanRes.Text == "Others")
                                {
                                    reqPlCancelEWB.cancelRsnCode = 4;
                                }
                                reqPlCancelEWB.cancelRmrk = txtEWBCanRes.Text.ToString().Trim();
                                TaxProEInvoice.API.TxnRespWithObj<RespPlCancelEWB> txnRespWithObj = await eInvoiceAPI.CancelEWBAsync(Appmain.eInvSession, reqPlCancelEWB, "CANEWB");

                                if (txnRespWithObj.IsSuccess)
                                {
                                    string cTblName, cIdFld;
                                    cTblName = Row.Cells["TblName"].Value.ToString().Trim();
                                    cIdFld = Row.Cells["IdFld"].Value.ToString().Trim();

                                    string strSelect = " Update " + cTblName + " Set EwbNo = '' , EwbDt = '' " +
                                                    " where " + cIdFld + " = '" + Row.Cells["InvId"].Value + "'";

                                    Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                                    da.UpdateCommand = new SqlCommand(strSelect, Appmain.sqlCnn);
                                    da.UpdateCommand.ExecuteNonQuery();
                                    Command.Dispose();
                                    cMsg = "Succesfully Cancelled E-Way Bill No: " + cEWBNo + " of Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim();

                                    Row.DefaultCellStyle.ForeColor = Color.Blue;
                                    Row.Cells["ErrorList"].Value = cMsg;

                                    string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                    foreach (DataGridViewRow Row1 in EInvBillDetGrid.Rows)
                                    {
                                        if (DocNo == Row1.Cells["DocDtls_No"].Value.ToString().Trim())
                                        {
                                            Row1.DefaultCellStyle.ForeColor = Color.Blue;
                                            Row1.Cells["ErrorList"].Value = cMsg;
                                        }
                                    }
                                    //for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                                    //{
                                    //    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                                    //    {
                                    //        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Blue;
                                    //        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                                    //    }
                                    //}
                                    txtErrMsg.Text = cMsg;
                                    Appmain.WriteToErrorLog(cMsg + " RowIndex " + Row.Index.ToString());
                                }
                                else
                                {
                                    Row.Cells["ErrorList"].Value = txnRespWithObj.TxnOutcome;
                                    Row.DefaultCellStyle.ForeColor = Color.Red;

                                    string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                    foreach (DataGridViewRow Row1 in EInvBillDetGrid.Rows)
                                    {
                                        if (DocNo == Row1.Cells["DocDtls_No"].Value.ToString().Trim())
                                        {
                                            Row1.Cells["ErrorList"].Value = txnRespWithObj.TxnOutcome;
                                            Row1.DefaultCellStyle.ForeColor = Color.Red;
                                        }
                                    }
                                    //for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                                    //{
                                    //    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                                    //    {
                                    //        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = txnRespWithObj.TxnOutcome;
                                    //        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                                    //    }
                                    //}
                                    txtErrMsg.Text = txnRespWithObj.TxnOutcome;
                                    Appmain.WriteToErrorLog("EWay Bill Cancellation Failure for Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim() + txnRespWithObj.TxnOutcome + " RowIndex " + Row.Index.ToString());
                                }
                                LstDocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                            }
                        }
                        else
                        {
                            cMsg = "EWB is still not generated for Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim() + ". So EWB Cancellation not possible.";
                            txtErrMsg.Text = cMsg;
                            Row.Cells["ErrorList"].Value = cMsg;
                            Row.DefaultCellStyle.ForeColor = Color.Red;
                            string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                            foreach (DataGridViewRow Row1 in EInvBillDetGrid.Rows)
                            {
                                if (DocNo == Row1.Cells["DocDtls_No"].Value.ToString().Trim())
                                {
                                    Row1.Cells["ErrorList"].Value = cMsg;
                                    Row1.DefaultCellStyle.ForeColor = Color.Red;
                                }
                            }
                            //for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                            //{
                            //    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                            //    {
                            //        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                            //        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                            //    }
                            //}
                            Appmain.WriteToErrorLog(cMsg + " RowIndex " + Row.Index.ToString());
                        }
                    }

                    //for (int nRowIndex = 0; nRowIndex< EInvBillDetGrid.RowCount - 1; nRowIndex++)
                    //{
                    //    string LstDocNo = "";

                    //    cEWBNo = EInvBillDetGrid.Rows[nRowIndex].Cells["EWbNo"].Value.ToString().Trim();
                    //    cMsg = "Cancelling E-Way Bill No: " + cEWBNo + " of Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                    //    txtErrMsg.Text = cMsg;
                    //    EInvBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                    //    Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());

                    //    if ((bool)EInvBillDetGrid.Rows[nRowIndex].Cells[0].Value == true && cEWBNo != "")
                    //    {
                    //        //'And EInvBillDetGrid.Rows(nRowIndex).Cells(1).Value <> True
                    //        if (EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim() != LstDocNo)
                    //        {
                    //            ReqPlCancelEWB reqPlCancelEWB = new ReqPlCancelEWB();
                    //            reqPlCancelEWB.ewbNo = long.Parse(cEWBNo);
                    //            if (txtEWBCanRes.Text == "Duplicate")
                    //            {
                    //                reqPlCancelEWB.cancelRsnCode = 1;
                    //            }
                    //            else if (txtEWBCanRes.Text == "Order Cancelled")
                    //            {
                    //                reqPlCancelEWB.cancelRsnCode = 2;
                    //            }
                    //            else if (txtEWBCanRes.Text == "Data Entry Mistake" )
                    //            {
                    //                reqPlCancelEWB.cancelRsnCode = 3;
                    //            }
                    //            else if (txtEWBCanRes.Text == "Others" )
                    //            {
                    //                reqPlCancelEWB.cancelRsnCode = 4;
                    //            }                               
                    //            reqPlCancelEWB.cancelRmrk = txtEWBCanRes.Text.ToString().Trim();
                    //            TaxProEInvoice.API.TxnRespWithObj<RespPlCancelEWB> txnRespWithObj = await eInvoiceAPI.CancelEWBAsync(Appmain.eInvSession, reqPlCancelEWB, "CANEWB");



                    //            //'SetTokenDetail() ' started 25 / 08 / 21

                    //            if (txnRespWithObj.IsSuccess)
                    //            {
                    //                string cTblName, cIdFld;
                    //                cTblName = EInvBillDetGrid.Rows[nRowIndex].Cells["TblName"].Value.ToString().Trim();
                    //                cIdFld = EInvBillDetGrid.Rows[nRowIndex].Cells["IdFld"].Value.ToString().Trim();

                    //                string strSelect = " Update " + cTblName + " Set EwbNo = '' , EwbDt = '' " +
                    //                                " where " + cIdFld + " = " + Commfunc.V2C(EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value);


                    //                Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                    //                da.UpdateCommand = new SqlCommand(strSelect, Appmain.sqlCnn);
                    //                da.UpdateCommand.ExecuteNonQuery();
                    //                Command.Dispose();
                    //                cMsg = "Succesfully Cancelled E-Way Bill No: " + cEWBNo + " of Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();

                    //                EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    //                EInvBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;

                    //                string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                    //                for (int nRow = 0; nRow< EInvBillDetGrid.RowCount - 1; nRow++)
                    //                {
                    //                    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim() )
                    //                    {
                    //                        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Blue;
                    //                        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                    //                    }
                    //                }
                    //                txtErrMsg.Text = cMsg;
                    //                Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());
                    //            }
                    //            else
                    //            {
                    //                EInvBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = txnRespWithObj.TxnOutcome;
                    //                EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;

                    //                string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                    //                for (int nRow = 0; nRow< EInvBillDetGrid.RowCount - 1; nRow++)
                    //                {
                    //                    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                    //                    {
                    //                        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = txnRespWithObj.TxnOutcome;
                    //                        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                    //                    }                                        
                    //                }
                    //                txtErrMsg.Text = txnRespWithObj.TxnOutcome;
                    //                Appmain.WriteToErrorLog("EWay Bill Cancellation Failure for Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim() + txnRespWithObj.TxnOutcome + " RowIndex " + nRowIndex.ToString());
                    //            }
                    //            LstDocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                    //        }                            
                    //    }
                    //    else
                    //    {
                    //        cMsg = "EWB is still not generated for Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim() + ". So EWB Cancellation not possible.";
                    //        txtErrMsg.Text = cMsg;
                    //        EInvBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;
                    //        EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //        string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                    //        for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                    //        {
                    //            if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                    //            {
                    //                EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                    //                EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                    //            }
                    //        }
                    //        Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());
                    //    }
                    //}
                }
                else
                {
                    // Show a message box with an exclamation icon and a title
                    MessageBox.Show("Before cancelling E-Way Bill, please provide Cancellation Reason and Remarks compulsory.", "Reason Required.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                txtInvDet.Text = "Inv. No. :" + EInvBillDetGrid.CurrentRow.Cells["DocDtls_No"].Value + "   IRN : " + EInvBillDetGrid.CurrentRow.Cells["IRN"].Value +
                             "   IRNAck : " + EInvBillDetGrid.CurrentRow.Cells["IRNACK"].Value + "\n" + "   EWB : " + EInvBillDetGrid.CurrentRow.Cells["EWBNo"].Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("EWB Cancellation Error : " + ex.Message);
            }
        }
        private void BtnEWBPrint_Click(object sender, EventArgs e)
        {
            PrintEWB(false);
        }

        private void BtnEWBDetPrn_Click(object sender, EventArgs e)
        {
            PrintEWB(true);
        }
        public async void PrintEWB(bool lDetail)
        {
            string cEWBNo;
            string cMsg = "";

            try
            {
                Appmain.WriteToErrorLog("****************************************************************************************************************");
                Appmain.WriteToErrorLog("Computer Name: " + Environment.MachineName);
                Appmain.WriteToErrorLog("Date/Time: " + DateTime.Now.ToString());

                Appmain.ResetGrid(EInvBillDetGrid, txtErrMsg);
                string LstDocNo = "";

                foreach (DataGridViewRow Row in EInvBillDetGrid.Rows)
                {
                    if ((bool)Row.Cells[0].Value)
                    {
                        if (Row.Cells["EWbNo"].Value.ToString().Trim() != "")
                        {
                            if (LstDocNo != Row.Cells["DocDtls_No"].Value.ToString().Trim())
                            {
                                Appmain.WriteToErrorLog("Started Printing EWB for Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex " + Row.Index.ToString());
                                cEWBNo = Row.Cells["EWbNo"].Value.ToString().Trim();
                                cMsg = "Printing E-Way Bill No: " + cEWBNo + " of Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                Row.Cells["ErrorList"].Value = cMsg;
                                txtErrMsg.Text = cMsg;
                                Appmain.WriteToErrorLog(cMsg + " RowIndex " + Row.Index.ToString());
                                TxnRespWithObjAndInfo<RespGetEWBDetail> TxnResp = await EWBAPI.GetEWBDetailAsync(Appmain.EwbSession, long.Parse(cEWBNo));

                                var strJson = JsonConvert.SerializeObject(TxnResp.RespObj);
                                TaxProEWB.API.ReqPrintEWB reqPrintEWB = new TaxProEWB.API.ReqPrintEWB();
                                reqPrintEWB = JsonConvert.DeserializeObject<TaxProEWB.API.ReqPrintEWB>(strJson);
                                reqPrintEWB.Irn = Row.Cells["IRN"].Value.ToString().Trim();

                                Appmain.WriteToErrorLog("EWay Bill Get Detail Response " + JsonConvert.SerializeObject(TxnResp) + " RowIndex " + Row.Index.ToString());
                                if (TxnResp.IsSuccess)
                                {

                                    EWBAPI.PrintEWB(Appmain.EwbSession, reqPrintEWB, "", true, lDetail);

                                    cMsg = "Successfully Printed E-Way Bill No: " + cEWBNo + " of Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                    Row.Cells["ErrorList"].Value = cMsg;
                                    Row.DefaultCellStyle.ForeColor = Color.Blue;
                                    txtErrMsg.Text = cMsg;

                                    string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                    foreach (DataGridViewRow Row1 in EInvBillDetGrid.Rows)
                                    {
                                        if (DocNo == Row1.Cells["DocDtls_No"].Value.ToString().Trim())
                                        {
                                            Row1.Cells["ErrorList"].Value = cMsg;
                                            Row1.DefaultCellStyle.ForeColor = Color.Blue;
                                        }
                                    }
                                    //for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                                    //{
                                    //if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                                    //{
                                    //    EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                                    //    EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Blue;
                                    //}
                                    //}

                                    Appmain.WriteToErrorLog(cMsg + " RowIndex " + Row.Index.ToString());
                                }
                                else
                                {
                                    Row.Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                                    Row.DefaultCellStyle.ForeColor = Color.Red;
                                    string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                    foreach (DataGridViewRow Row1 in EInvBillDetGrid.Rows)
                                    {
                                        if (DocNo == Row1.Cells["DocDtls_No"].Value.ToString().Trim())
                                        {
                                            Row1.Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                                            Row1.DefaultCellStyle.ForeColor = Color.Red;
                                        }
                                    }
                                    //for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                                    //{
                                    //    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                                    //    {
                                    //        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                                    //        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                                    //    }
                                    //}
                                    txtErrMsg.Text = TxnResp.TxnOutcome;
                                    Appmain.WriteToErrorLog("EWay Bill Printing Failure \n" + TxnResp.TxnOutcome + " RowIndex " + Row.Index.ToString());
                                }
                                LstDocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                Appmain.WriteToErrorLog("Completed Printing EWB for Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex " + Row.Index.ToString());
                            }
                        }
                        else
                        {
                            cMsg = "EWB is not generated for Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim() + ". So Printing not available ";
                            txtErrMsg.Text = cMsg;
                            Row.Cells["ErrorList"].Value = cMsg;
                            Row.DefaultCellStyle.ForeColor = Color.Red;

                            string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                            foreach (DataGridViewRow Row1 in EInvBillDetGrid.Rows)
                            {
                                if (DocNo == Row1.Cells["DocDtls_No"].Value.ToString().Trim())
                                {
                                    Row1.Cells["ErrorList"].Value = cMsg;
                                    Row1.DefaultCellStyle.ForeColor = Color.Red;
                                }
                            }

                            //for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                            //{
                            //    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                            //    {
                            //        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                            //        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                            //    }
                            //}
                            Appmain.WriteToErrorLog(cMsg + " RowIndex " + Row.Index.ToString());
                        }
                    }
                    else
                    {
                        Appmain.WriteToErrorLog("Not selected Bill No." + Row.Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex " + Row.Index.ToString());
                    }
                }

                //for (int nRowIndex = 0; nRowIndex < EInvBillDetGrid.RowCount - 1; nRowIndex++)
                //{
                //    if ((bool)EInvBillDetGrid.Rows[nRowIndex].Cells[0].Value)
                //    {
                //        if (EInvBillDetGrid.Rows[nRowIndex].Cells["EWbNo"].Value.ToString().Trim() != "")
                //        {
                //            //'And EInvBillDetGrid.Rows(nRowIndex).Cells(1).Value <> True 
                //            if (LstDocNo != EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim())
                //            {
                //                Appmain.WriteToErrorLog("Started Printing EWB for Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex " + nRowIndex.ToString());
                //                cEWBNo = EInvBillDetGrid.Rows[nRowIndex].Cells["EWbNo"].Value.ToString().Trim();
                //                cMsg = "Printing E-Way Bill No: " + cEWBNo + " of Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //                EInvBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                //                txtErrMsg.Text = cMsg;
                //                Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());
                //                TxnRespWithObjAndInfo<RespGetEWBDetail> TxnResp = await EWBAPI.GetEWBDetailAsync(Appmain.EwbSession, long.Parse(cEWBNo));

                //                var strJson = JsonConvert.SerializeObject(TxnResp.RespObj);
                //                TaxProEWB.API.ReqPrintEWB reqPrintEWB = new TaxProEWB.API.ReqPrintEWB();
                //                reqPrintEWB = JsonConvert.DeserializeObject<TaxProEWB.API.ReqPrintEWB>(strJson);
                //                reqPrintEWB.Irn = EInvBillDetGrid.Rows[nRowIndex].Cells["IRN"].Value.ToString().Trim();

                //                //'SetTokenDetail() ' started 25 / 08 / 21
                //                Appmain.WriteToErrorLog("EWay Bill Get Detail Response " + JsonConvert.SerializeObject(TxnResp) + " RowIndex " + nRowIndex.ToString());
                //                if (TxnResp.IsSuccess)
                //                {

                //                    EWBAPI.PrintEWB(Appmain.EwbSession, reqPrintEWB, "", true, lDetail);

                //                    cMsg = "Successfully Printed E-Way Bill No: " + cEWBNo + " of Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //                    EInvBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;
                //                    EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //                    txtErrMsg.Text = cMsg;

                //                    string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //                    for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                //                    {
                //                        if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                //                        {
                //                            EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                //                            EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Blue;
                //                        }
                //                    }
                //                    Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());
                //                }
                //                else
                //                {
                //                    //'BillDetGrid.CurrentRow.Cells("ErrorList").Value = TxnResp.TxnOutcome
                //                    EInvBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                //                    EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //                    string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();

                //                    for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                //                    {
                //                        if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                //                        {
                //                            EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                //                            EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                //                        }
                //                    }
                //                    txtErrMsg.Text = TxnResp.TxnOutcome;
                //                    Appmain.WriteToErrorLog("EWay Bill Printing Failure \n" + TxnResp.TxnOutcome + " RowIndex " + nRowIndex.ToString());
                //                }
                //                LstDocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //                Appmain.WriteToErrorLog("Completed Printing EWB for Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex " + nRowIndex.ToString());
                //            }
                //        }
                //        else
                //        {
                //            cMsg = "EWB is not generated for Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim() + ". So Printing not available ";
                //            txtErrMsg.Text = cMsg;
                //            EInvBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;
                //            EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;

                //            string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //            for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                //            {
                //                if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                //                {
                //                    EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                //                    EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                //                }                                
                //            }
                //            Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());
                //        }
                //    }
                //    else
                //    {
                //        Appmain.WriteToErrorLog("Not selected Bill No." + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim() + " RowIndex " + nRowIndex.ToString());
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Print EWB Error : " + ex.Message);
            }
        }

        private void QRCodeBtn_Click(object sender, EventArgs e)
        {
            string InvId = "";
            var strSelect = "";
            var cMsg = "";
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int nRecCount;
            Appmain.WriteToErrorLog("QRCode-1");
            try
            {
                Appmain.WriteToErrorLog("****************************************************************************************************************");
                Appmain.WriteToErrorLog("Computer Name: " + Environment.MachineName);
                Appmain.WriteToErrorLog("Date/Time: " + DateTime.Now.ToString());

                Appmain.ResetGrid(EInvBillDetGrid, txtErrMsg);
                string LstDocNo = "";
                Appmain.WriteToErrorLog("QRCode-2");
                foreach (DataGridViewRow Row in EInvBillDetGrid.Rows)
                {
                    if ((bool)Row.Cells[0].Value)//if ((bool)Row.Cells[0].Value == true)
                    {
                        if (LstDocNo != Row.Cells["DocDtls_No"].Value.ToString().Trim())
                        {
                            InvId = Row.Cells["InvId"].Value.ToString().Trim();
                            Appmain.WriteToErrorLog("InvId" + InvId + "QRCode-3");
                            cMsg = "Generating QR Code Image for Bill No.: " + Row.Cells["DocDtls_No"].Value.ToString().Trim();
                            Row.Cells["ErrorList"].Value = cMsg;
                            txtErrMsg.Text = cMsg;
                            Appmain.WriteToErrorLog("==================================================================================================");
                            Appmain.WriteToErrorLog(cMsg + " RowIndex " + Row.Index.ToString());
                            Appmain.WriteToErrorLog("QRCode-4");

                            strSelect = "Select IRNData from EInvIRNDet where ModId = '" + InvId + "' Order by Srl";
                            Appmain.WriteToErrorLog(cMsg + " " + strSelect + " RowIndex " + Row.Index.ToString());
                            command = new SqlCommand(strSelect, Appmain.sqlCnn);
                            adapter.SelectCommand = command;
                            adapter.Fill(ds, "QR");
                            command.Dispose();
                            nRecCount = ds.Tables["QR"].Rows.Count;
                            Appmain.WriteToErrorLog("QRCode-5 RecCount - " + nRecCount);

                            //command.Dispose();
                            //nRecCount = ds.Tables["QR"].Rows.Count;
                            if (nRecCount == 0)
                            {
                                cMsg = "No IRN data found in local table for Bill No.: " + Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                Row.DefaultCellStyle.ForeColor = Color.Red;
                                Row.Cells["ErrorList"].Value = cMsg;
                                Appmain.WriteToErrorLog("QRCode-6");

                                string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                foreach (DataGridViewRow Row2 in EInvBillDetGrid.Rows)
                                {
                                    if (DocNo == Row2.Cells["DocDtls_No"].Value.ToString().Trim())
                                    {
                                        Row2.DefaultCellStyle.ForeColor = Color.Red;
                                        Row2.Cells["ErrorList"].Value = cMsg;
                                    }
                                }
                                Appmain.WriteToErrorLog("QRCode-7");
                                //for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                                //{
                                //    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                                //    {
                                //        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                                //        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                                //    }
                                //}

                                txtErrMsg.Text = cMsg;
                                Appmain.WriteToErrorLog("QR Code Image generation failed " + cMsg + " RowIndex " + Row.Index.ToString());
                            }
                            else
                            {
                                Appmain.WriteToErrorLog("QRCode-8");
                                string strQRCode = "";  //'"eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ0NDQwNUM3ODFFNDgyNTA3MkIzNENBNEY4QkRDNjA2Qzg2QjU3MjAiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJSRVFGeDRIa2dsQnlzMHlrLUwzR0JzaHJWeUEifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjI0QUFCQ1IxMjc2TTFaOVwiLFwiQnV5ZXJHc3RpblwiOlwiMjRBQkxQTTE3NTVDMVpXXCIsXCJEb2NOb1wiOlwiNzMwMi9ZLzIwLTIxXCIsXCJEb2NUeXBcIjpcIklOVlwiLFwiRG9jRHRcIjpcIjA0LzAxLzIwMjFcIixcIlRvdEludlZhbFwiOjYzNzQ1LjAsXCJJdGVtQ250XCI6MSxcIk1haW5Ic25Db2RlXCI6XCI1NDAyXCIsXCJJcm5cIjpcIjA3NzlmMTNlM2NmYjI5MGY1NDQzOGIxMDQ2Mjc3MDMzNjllYzNiNGNhNDg4MjUwZDdkMmM5ZTQwOTkzYTc1NThcIixcIklybkR0XCI6XCIyMDIxLTAxLTA0IDE3OjA4OjAwXCJ9IiwiaXNzIjoiTklDIn0.CghOHNVekMPSgsZybO4Umk0IBvlSvxXX_6FgUpnV8f0-50bDMrP8YOu2onZdwM-ZzFsfZgf3zve3qWsuDTe4G-P3yAQemKXh4Sxp9U-xx33jim1gHlvNvSnHKSE1Dy35hrneP_PLXBGPunkyUrH-holj9krAo4V5JKSpyYoiVZ3W0e_7rY0KTf7h6LdOyrg1_ZMDwgp0OBJeBNovvRbyXU83BDtM1QjN-mpGg3V2tYKL0naWOpEG1WT8pjxhqkRhTMEhyPJG9yuZ_bmO6HW2kONAIIHjBlbztJ1-ppcBgGNQyLioZzgZ5Mtm4Ov1mCntwXwm_PtkfcolGCz8bozoFA"
                                //for (int iCtr = 0; iCtr < ds.Tables["QR"].Rows.Count - 1; iCtr++)
                                //{
                                //    strQRCode += ds.Tables["QR"].Rows[iCtr]["IRNData"].ToString().Trim();
                                //}
                                foreach (DataRow row in ds.Tables["QR"].Rows)
                                {
                                    strQRCode += row["IRNData"].ToString().Trim();
                                }

                                Appmain.WriteToErrorLog("QRCode-9");
                                Appmain.WriteToErrorLog("strQRCode - " + strQRCode);
                                Appmain.WriteToErrorLog("QRCode InvId - " + Row.Cells["InvId"].Value.ToString());

                                BarcodeWriter QRCode = new ZXing.BarcodeWriter();
                                QRCode.Options = QROption;
                                QRCode.Format = ZXing.BarcodeFormat.QR_CODE;
                                Bitmap Result = new Bitmap(QRCode.Write(strQRCode));
                                QRImg.Image = Result;
                                QRImg.Image.Save(path + Row.Cells["InvId"].Value + ".png");
                                cMsg = "Successfully generated QR Code Image for Bill No.: " + Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                Appmain.WriteToErrorLog("QRCode-10");
                                Row.DefaultCellStyle.ForeColor = Color.Blue;
                                Row.Cells["ErrorList"].Value = cMsg;
                                txtErrMsg.Text = cMsg;
                                Appmain.WriteToErrorLog(cMsg + " RowIndex " + Row.Index.ToString());
                                Appmain.WriteToErrorLog("QRCode-11");
                                txtInvDet.Text = "Inv. No. :" + Row.Cells["DocDtls_No"].Value + "   IRN : " + Row.Cells["IRN"].Value +
                                                 "   IRNAck : " + Row.Cells["IRNACK"].Value + "\n" + "   EWB : " + Row.Cells["EWBNo"].Value;

                                string DocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                                foreach (DataGridViewRow Row2 in EInvBillDetGrid.Rows)
                                {
                                    if (DocNo == Row2.Cells["DocDtls_No"].Value.ToString().Trim())
                                    {
                                        Row2.DefaultCellStyle.ForeColor = Color.Blue;
                                        Row2.Cells["ErrorList"].Value = cMsg;
                                    }
                                }
                                Appmain.WriteToErrorLog("QRCode-12");
                                //for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                                //{
                                //    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                                //    {
                                //        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Blue;
                                //        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                                //    }
                                //}
                            }
                            LstDocNo = Row.Cells["DocDtls_No"].Value.ToString().Trim();
                            Appmain.WriteToErrorLog("QRCode-13-Complete");
                        }
                    }
                }

                //for (int nRowIndex = 0; nRowIndex< EInvBillDetGrid.RowCount - 1; nRowIndex++)
                //{
                //    if ((bool)EInvBillDetGrid.Rows[nRowIndex].Cells[0].Value == true )
                //    {
                //        if (LstDocNo != EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim())
                //        {
                //            InvId = EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value.ToString().Trim();

                //            cMsg = "Generating QR Code Image for Bill No.: " + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //            EInvBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                //            txtErrMsg.Text = cMsg;
                //            Appmain.WriteToErrorLog("==================================================================================================");
                //            Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());

                //            strSelect = "Select IRNData from EInvIRNDet where ModId =" + Commfunc.V2C(InvId) + " Order by Srl";
                //            Appmain.WriteToErrorLog(cMsg + " " + strSelect + " RowIndex " + nRowIndex.ToString());
                //            command = new SqlCommand(strSelect, Appmain.sqlCnn);
                //            adapter.SelectCommand = command;
                //            adapter.Fill(ds, "QR");
                //            command.Dispose();
                //            nRecCount = ds.Tables["QR"].Rows.Count;

                //            command.Dispose();
                //            nRecCount = ds.Tables["QR"].Rows.Count;
                //            if (nRecCount == 0)
                //            {
                //                cMsg = "No IRN data found in local table for Bill No.: " + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //                EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //                EInvBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;

                //                string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //                for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                //                {
                //                    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim())
                //                    {
                //                        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Red;
                //                        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                //                    }                                    
                //                }
                //                txtErrMsg.Text = cMsg;
                //                Appmain.WriteToErrorLog("QR Code Image generation failed " + cMsg + " RowIndex " + nRowIndex.ToString());
                //            }
                //            else
                //            {
                //                string strQRCode = "";  //'"eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ0NDQwNUM3ODFFNDgyNTA3MkIzNENBNEY4QkRDNjA2Qzg2QjU3MjAiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJSRVFGeDRIa2dsQnlzMHlrLUwzR0JzaHJWeUEifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjI0QUFCQ1IxMjc2TTFaOVwiLFwiQnV5ZXJHc3RpblwiOlwiMjRBQkxQTTE3NTVDMVpXXCIsXCJEb2NOb1wiOlwiNzMwMi9ZLzIwLTIxXCIsXCJEb2NUeXBcIjpcIklOVlwiLFwiRG9jRHRcIjpcIjA0LzAxLzIwMjFcIixcIlRvdEludlZhbFwiOjYzNzQ1LjAsXCJJdGVtQ250XCI6MSxcIk1haW5Ic25Db2RlXCI6XCI1NDAyXCIsXCJJcm5cIjpcIjA3NzlmMTNlM2NmYjI5MGY1NDQzOGIxMDQ2Mjc3MDMzNjllYzNiNGNhNDg4MjUwZDdkMmM5ZTQwOTkzYTc1NThcIixcIklybkR0XCI6XCIyMDIxLTAxLTA0IDE3OjA4OjAwXCJ9IiwiaXNzIjoiTklDIn0.CghOHNVekMPSgsZybO4Umk0IBvlSvxXX_6FgUpnV8f0-50bDMrP8YOu2onZdwM-ZzFsfZgf3zve3qWsuDTe4G-P3yAQemKXh4Sxp9U-xx33jim1gHlvNvSnHKSE1Dy35hrneP_PLXBGPunkyUrH-holj9krAo4V5JKSpyYoiVZ3W0e_7rY0KTf7h6LdOyrg1_ZMDwgp0OBJeBNovvRbyXU83BDtM1QjN-mpGg3V2tYKL0naWOpEG1WT8pjxhqkRhTMEhyPJG9yuZ_bmO6HW2kONAIIHjBlbztJ1-ppcBgGNQyLioZzgZ5Mtm4Ov1mCntwXwm_PtkfcolGCz8bozoFA"
                //                for (int iCtr = 0; iCtr< ds.Tables["QR"].Rows.Count - 1; iCtr++)
                //                {
                //                    strQRCode += ds.Tables["QR"].Rows[iCtr]["IRNData"].ToString().Trim();
                //                }

                //                BarcodeWriter QRCode = new ZXing.BarcodeWriter();
                //                QRCode.Options = QROption;
                //                QRCode.Format = ZXing.BarcodeFormat.QR_CODE;
                //                Bitmap Result = new Bitmap(QRCode.Write(strQRCode));
                //                QRImg.Image = Result;
                //                QRImg.Image.Save(path + EInvBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value + ".png");
                //                cMsg = "Successfully generated QR Code Image for Bill No.: " + EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //                EInvBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //                EInvBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                //                txtErrMsg.Text = cMsg;
                //                Appmain.WriteToErrorLog(cMsg + " RowIndex " + nRowIndex.ToString());
                //                txtInvDet.Text = "Inv. No. :" + EInvBillDetGrid.CurrentRow.Cells["DocDtls_No"].Value + "   IRN : " + EInvBillDetGrid.CurrentRow.Cells["IRN"].Value +
                //                                 "   IRNAck : " + EInvBillDetGrid.CurrentRow.Cells["IRNACK"].Value + "\n" + "   EWB : " + EInvBillDetGrid.CurrentRow.Cells["EWBNo"].Value;

                //                string DocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //                for (int nRow = 0; nRow < EInvBillDetGrid.RowCount - 1; nRow++)
                //                {
                //                    if (DocNo == EInvBillDetGrid.Rows[nRow].Cells["DocDtls_No"].Value.ToString().Trim() )
                //                    {
                //                        EInvBillDetGrid.Rows[nRow].DefaultCellStyle.ForeColor = Color.Blue;
                //                        EInvBillDetGrid.Rows[nRow].Cells["ErrorList"].Value = cMsg;
                //                    }
                //                }                                
                //                //'Task.Delay(5000)
                //            }
                //            LstDocNo = EInvBillDetGrid.Rows[nRowIndex].Cells["DocDtls_No"].Value.ToString().Trim();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("QR Code Generation Error : " + ex.Message);
            }
        }
        private void BtnIRNTKN_Click(object sender, EventArgs e)
        {
            Appmain.GetAuthToken("EINVOICE");
        }

        private void OnServerChange(object sender, EventArgs e)
        {

            if (Server.Text.ToUpper() == "PRIMARY")
            {
                SharedVar.cAuthUrl = "https://einvapi.charteredinfo.com/eivital/v1.04";
                SharedVar.cBaseUrl = "https://einvapi.charteredinfo.com/eicore/v1.03";
                SharedVar.cEwbByIRN = "https://einvapi.charteredinfo.com/eiewb/v1.03";
                SharedVar.cCancelEwbUrl = "https://einvapi.charteredinfo.com/v1.03";

            }
            else if (Server.Text.ToUpper() == "BACKUP1")
            {
                SharedVar.cAuthUrl = "https://einvapimum1.charteredinfo.com/eivital/v1.04";
                SharedVar.cBaseUrl = "https://einvapimum1.charteredinfo.com/eicore/v1.03";
                SharedVar.cEwbByIRN = "https://einvapimum1.charteredinfo.com/eiewb/v1.03";
                SharedVar.cCancelEwbUrl = "https://einvapimum1.charteredinfo.com/v1.03";
            }
            else if (Server.Text.ToUpper() == "BACKUP2")
            {
                SharedVar.cAuthUrl = "https://einvapidel2.charteredinfo.com/eivital/v1.04";
                SharedVar.cBaseUrl = "https://einvapidel2.charteredinfo.com/eicore/v1.03";
                SharedVar.cEwbByIRN = "https://einvapidel2.charteredinfo.com/eiewb/v1.03";
                SharedVar.cCancelEwbUrl = "https://einvapidel2.charteredinfo.com/v1.03";
            }



            if (!DefServerLink)
            {
                Appmain.MakeEInvoiceSession();
                Appmain.MakeEWayBillSession();
            }
            DefServerLink = false;
            //MsgBox(Server.Text.ToUpper() + " " + cAuthUrl, MsgBoxStyle.Information, "URL")
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (DataGridViewColumn column in EInvBillDetGrid.Columns)
            //{
            //    DataGridViewColumnHeaderCell headerCell = column.HeaderCell;
            //    string headerCaptionText = column.HeaderText;
            //    string columnName = column.Name; // Used as a key to myDataGridView.Columns['key_name'];
            //}
            //this.RadGridView1.Columns[1].IsVisible = false;
            //foreach (DataGridView col in EInvBillDetGrid.Columns)
            //{
            //    if (col.Name.Contains("DispDtls"))
            //    {
            //        col.Visible = true;
            //    }
            //}
            Appmain.WriteToErrorLog("In checkBox1_CheckedChanged A");
            if(DispDetail.Checked)
            {
                EInvBillDetGrid.Columns["DispDtls_Nm"].Visible = true;
                EInvBillDetGrid.Columns["DispDtls_Addr1"].Visible = true;
                EInvBillDetGrid.Columns["DispDtls_Addr2"].Visible = true;
                EInvBillDetGrid.Columns["DispDtls_Loc"].Visible = true;
                EInvBillDetGrid.Columns["DispDtls_Pin"].Visible = true;
                EInvBillDetGrid.Columns["DispDtls_Stcd"].Visible = true;
            }
            else
            {
                EInvBillDetGrid.Columns["DispDtls_Nm"].Visible = false;
                EInvBillDetGrid.Columns["DispDtls_Addr1"].Visible = false;
                EInvBillDetGrid.Columns["DispDtls_Addr2"].Visible = false;
                EInvBillDetGrid.Columns["DispDtls_Loc"].Visible = false;
                EInvBillDetGrid.Columns["DispDtls_Pin"].Visible = false;
                EInvBillDetGrid.Columns["DispDtls_Stcd"].Visible = false;
            }
            
        }
        private void HideColumns(DataGridView gridView)
        {
            Appmain.WriteToErrorLog("In Hide Columns Method");
            gridView.Columns["DispDtls_Nm"].Visible = false;
            gridView.Columns["DispDtls_Addr1"].Visible = false;
            gridView.Columns["DispDtls_Addr2"].Visible = false;
            gridView.Columns["DispDtls_Loc"].Visible = false;
            gridView.Columns["DispDtls_Pin"].Visible = false;
            gridView.Columns["DispDtls_Stcd"].Visible = false;

            gridView.Columns["ShipDtls_Gstin"].Visible = false;
            gridView.Columns["ShipDtls_LglNm"].Visible = false;
            gridView.Columns["ShipDtls_TrdNm"].Visible = false;
            gridView.Columns["ShipDtls_Addr1"].Visible = false;
            gridView.Columns["ShipDtls_Addr2"].Visible = false;
            gridView.Columns["ShipDtls_Loc"].Visible = false;
            gridView.Columns["ShipDtls_Pin"].Visible = false;
            gridView.Columns["ShipDtls_StCd"].Visible = false;

            gridView.Columns["SellerDtls_Gstin"].Visible = false;
            gridView.Columns["SellerDtls_LglNm"].Visible = false;
            gridView.Columns["SellerDtls_TrdNm"].Visible = false;
            gridView.Columns["SellerDtls_Addr1"].Visible = false;
            gridView.Columns["SellerDtls_Addr2"].Visible = false;
            gridView.Columns["SellerDtls_Pin"].Visible = false;
            gridView.Columns["SellerDtls_Loc"].Visible = false;
            gridView.Columns["SellerDtls_State"].Visible = false;
            gridView.Columns["SellerDtls_Ph"].Visible = false;
            gridView.Columns["SellerDtls_Em"].Visible = false;

            gridView.Columns["TblName"].Visible = false;
            gridView.Columns["InvId"].Visible = false;
            gridView.Columns["IdFld"].Visible = false;
            gridView.Columns["PayDtls"].Visible = false;
            gridView.Columns["RefDtls"].Visible = false;
            gridView.Columns["ModCode"].Visible = false;
            gridView.Columns["AddlDocDtls"].Visible = false;
            gridView.Columns["ExpDtls"].Visible = false;
            gridView.Columns["EwbDtls"].Visible = false;
            gridView.Columns["itm_BchDtls"].Visible = false;
            gridView.Columns["itm_AttribDtls"].Visible = false;
        }

        public class ReqPrintEWB : RespGetEWBDetail
        {
            public string Irn { get; set; }
        }

        private void SellerDetail_CheckedChanged(object sender, EventArgs e)
        {
            Appmain.WriteToErrorLog("In SellerDetail_CheckedChanged A");
            if (SellerDetail.Checked)
            {
                EInvBillDetGrid.Columns["SellerDtls_Gstin"].Visible = true;
                EInvBillDetGrid.Columns["SellerDtls_LglNm"].Visible = true;
                EInvBillDetGrid.Columns["SellerDtls_TrdNm"].Visible = true;
                EInvBillDetGrid.Columns["SellerDtls_Addr1"].Visible = true;
                EInvBillDetGrid.Columns["SellerDtls_Addr2"].Visible = true;
                EInvBillDetGrid.Columns["SellerDtls_Pin"].Visible = true;
                EInvBillDetGrid.Columns["SellerDtls_Loc"].Visible = true;
                EInvBillDetGrid.Columns["SellerDtls_State"].Visible = true;
                EInvBillDetGrid.Columns["SellerDtls_Ph"].Visible = true;
                EInvBillDetGrid.Columns["SellerDtls_Em"].Visible = true;
            }
            else
            {
                EInvBillDetGrid.Columns["SellerDtls_Gstin"].Visible = false;
                EInvBillDetGrid.Columns["SellerDtls_LglNm"].Visible = false;
                EInvBillDetGrid.Columns["SellerDtls_TrdNm"].Visible = false;
                EInvBillDetGrid.Columns["SellerDtls_Addr1"].Visible = false;
                EInvBillDetGrid.Columns["SellerDtls_Addr2"].Visible = false;
                EInvBillDetGrid.Columns["SellerDtls_Pin"].Visible = false;
                EInvBillDetGrid.Columns["SellerDtls_Loc"].Visible = false;
                EInvBillDetGrid.Columns["SellerDtls_State"].Visible = false;
                EInvBillDetGrid.Columns["SellerDtls_Ph"].Visible = false;
                EInvBillDetGrid.Columns["SellerDtls_Em"].Visible = false;
            }
        }

        private void ShipDetail_CheckedChanged(object sender, EventArgs e)
        {
            Appmain.WriteToErrorLog("In ShipDetail_CheckedChanged A");
            if (ShipDetail.Checked)
            {
                EInvBillDetGrid.Columns["ShipDtls_Gstin"].Visible = true;
                EInvBillDetGrid.Columns["ShipDtls_LglNm"].Visible = true;
                EInvBillDetGrid.Columns["ShipDtls_TrdNm"].Visible = true;
                EInvBillDetGrid.Columns["ShipDtls_Addr1"].Visible = true;
                EInvBillDetGrid.Columns["ShipDtls_Addr2"].Visible = true;
                EInvBillDetGrid.Columns["ShipDtls_Loc"].Visible = true;
                EInvBillDetGrid.Columns["ShipDtls_Pin"].Visible = true;
                EInvBillDetGrid.Columns["ShipDtls_StCd"].Visible = true;
            }
            else
            {
                EInvBillDetGrid.Columns["ShipDtls_Gstin"].Visible = false;
                EInvBillDetGrid.Columns["ShipDtls_LglNm"].Visible = false;
                EInvBillDetGrid.Columns["ShipDtls_TrdNm"].Visible = false;
                EInvBillDetGrid.Columns["ShipDtls_Addr1"].Visible = false;
                EInvBillDetGrid.Columns["ShipDtls_Addr2"].Visible = false;
                EInvBillDetGrid.Columns["ShipDtls_Loc"].Visible = false;
                EInvBillDetGrid.Columns["ShipDtls_StCd"].Visible = false;
            }
        }

        private void OthDetail_CheckedChanged(object sender, EventArgs e)
        {
            Appmain.WriteToErrorLog("In OthDetail_CheckedChanged A");
            
            if (OthDetail.Checked)
            {
                EInvBillDetGrid.Columns["TblName"].Visible = true;
                EInvBillDetGrid.Columns["InvId"].Visible = true;
                EInvBillDetGrid.Columns["IdFld"].Visible = true;
                EInvBillDetGrid.Columns["PayDtls"].Visible = true;
                EInvBillDetGrid.Columns["RefDtls"].Visible = true;
                EInvBillDetGrid.Columns["ModCode"].Visible = true;
                EInvBillDetGrid.Columns["AddlDocDtls"].Visible = true;
                EInvBillDetGrid.Columns["ExpDtls"].Visible = true;
                EInvBillDetGrid.Columns["EwbDtls"].Visible = true;
                EInvBillDetGrid.Columns["itm_BchDtls"].Visible = true;
                EInvBillDetGrid.Columns["itm_AttribDtls"].Visible = true;
            }
            else
            {
                EInvBillDetGrid.Columns["TblName"].Visible = false;
                EInvBillDetGrid.Columns["InvId"].Visible = false;
                EInvBillDetGrid.Columns["IdFld"].Visible = false;
                EInvBillDetGrid.Columns["PayDtls"].Visible = false;
                EInvBillDetGrid.Columns["RefDtls"].Visible = false;
                EInvBillDetGrid.Columns["ModCode"].Visible = false;
                EInvBillDetGrid.Columns["AddlDocDtls"].Visible = false;
                EInvBillDetGrid.Columns["ExpDtls"].Visible = false;
                EInvBillDetGrid.Columns["EwbDtls"].Visible = false;
                EInvBillDetGrid.Columns["itm_BchDtls"].Visible = false;
                EInvBillDetGrid.Columns["itm_AttribDtls"].Visible = false;
            }
        }
    }
}