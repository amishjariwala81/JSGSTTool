using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxProEInvoice.API;
using TaxProEWB.API;
using static EWayBillTool.frmEInvoice;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace EWayBillTool
{
    public partial class frmEWayBill : Form
    {
        private AppMain Appmain = new AppMain();
        private CommFunc Commfunc;
        //AppMain Appmain;
        //CommFunc Commfunc;
        public frmEWayBill()
        {
            
            InitializeComponent();
            frmEWayBill_Load();
            EWBBillDetGrid.KeyUp += BillDetGrid_KeyUp;
        }

        private void frmEWayBill_FormClosed(object sender , FormClosedEventArgs e )
        {
            try
            {
                Appmain.sqlCnn.Close();
            }
            catch(Exception ex) { }
            
        }

        private void frmEWayBill_Load()//object sender, EventArgs e
        {
            DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
            checkCol.HeaderText = "X";
            EWBBillDetGrid.Columns.Add(checkCol);
            EWBBillDetGrid.Columns[0].Frozen = true;
            
            try
            {
                
                Appmain.GetCmdArgs();
                CntrlBtn(false);
                oEWBStatusMsg.Text = "Please wait while loading details...";

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet ds = new DataSet();
                string sql;
                try
                {
                    
                    oEWBStatusMsg.Text = "Please wait while establishing database connection...";
                    Appmain.MakeSQLConnObj();
                    oEWBStatusMsg.Text = "Please wait while loading data...";
                    sql = " Select  * from " + Appmain.cEWBTble;
                    if (Appmain.nDebug == 0 )
                    {
                        MessageBox.Show(sql);
                    }
                    //Appmain.sqlCnn.Open();
                    adapter = new SqlDataAdapter(sql, Appmain.sqlCnn);
                    adapter.Fill(ds);
                    EWBBillDetGrid.DataSource = ds.Tables[0];

                }
                catch (Exception ex)
                {                                        
                   
                    MessageBox.Show("SQL Connection/Data Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    oEWBStatusMsg.Text = ex.Message; 
                }

                Appmain.UnChkAll(EWBBillDetGrid);
                oEWBStatusMsg.Text = "Data loaded successfully...";
                if (Appmain.nDebug == 0 )
                {
                    MessageBox.Show("Data Loaded");
                }
                Appmain.WriteToErrorLog("Data loaded successfully...");
                Appmain.MakeEWayBillSession();
            }
            catch(Exception ex)
            {
                MessageBox.Show("EWB Module Load Error : " + ex.ToString());
            }

            ToolTip buttonToolTip = new ToolTip();
            buttonToolTip.UseFading = true;
            buttonToolTip.UseAnimation = true;
            buttonToolTip.IsBalloon = true;
            buttonToolTip.ShowAlways = true;
            buttonToolTip.AutoPopDelay = 5000;
            buttonToolTip.InitialDelay = 1000;
            buttonToolTip.ReshowDelay = 500;
            buttonToolTip.IsBalloon = true;
            buttonToolTip.ShowAlways = true;
            buttonToolTip.SetToolTip(btnGenerate, "Generate EWay Bill.");
            buttonToolTip.SetToolTip(btnPrint, "EWay Bill Print - Summary.");
            buttonToolTip.SetToolTip(btnDetPrint, "EWay Bill Print - Detail.");
            buttonToolTip.SetToolTip(BtnDebug, "Enable Debug Mode.");
            buttonToolTip.SetToolTip(btnCancel, "Cancel Generated EWay Bill.");
            buttonToolTip.SetToolTip(btnChkAll, "Select All Bills.");
            buttonToolTip.SetToolTip(btnUnChkAll, "Unselect All Bills.");
            buttonToolTip.SetToolTip(BtnGetDetail, "Get EWB Details.");
        }

        private void BillDetGrid_KeyUp(object sender , KeyEventArgs e )
        {
            if (e.KeyCode == Keys.Space )
            {
                MarkSelectedRow();
            }
            else
            {
                txtErrMsg.Text = (string)EWBBillDetGrid.CurrentRow.Cells["EWbNo"].Value + EWBBillDetGrid.CurrentRow.Cells["ErrorList"].Value;
            }            
        }

        private void MarkSelectedRow()
        {
            

            var nRowIndex = EWBBillDetGrid.CurrentCell.RowIndex;
            var currentRow = EWBBillDetGrid.Rows[nRowIndex];
            var lTrueObj = currentRow.Cells[0].Value;
            var lTrue = lTrueObj != null && (bool)lTrueObj;
            //bool lTrue = (bool)EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value;
            var cBillNo = currentRow.Cells["BillNo"]?.Value?.ToString();
            var nTotRow = EWBBillDetGrid.RowCount;
            if ((bool)lTrue)
            {
                EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.White;
                EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value = false;
                for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
                {
                    if (cBillNo == EWBBillDetGrid.Rows[nRowIndex].Cells["BillNo"].Value.ToString() )
                    {
                        EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.White;
                        EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value = false;
                    }
                }
            }
            else
            {
                EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value = true;
                for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
                {
                    if (cBillNo == EWBBillDetGrid.Rows[nRowIndex].Cells["BillNo"].Value.ToString() )
                    {
                        EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                        EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value = true;
                        
                    }
                }
            }
            Appmain.nChkRow = 0;
            
            for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
            {
                if (EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value != null && (bool)EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value)//if ((bool)EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value)
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



            //var nRowIndex = EWBBillDetGrid.CurrentCell.RowIndex;
            //bool lTrue = (bool)EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value;
            //string cBillNo = EWBBillDetGrid.Rows[nRowIndex].Cells["BillNo"].Value.ToString().Trim();
            //int nTotRow = EWBBillDetGrid.RowCount - 1;
            //if (lTrue)
            //{
            //    EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.White;
            //    EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value = false;
            //    for (nRowIndex = 0; nRowIndex<  nTotRow; nRowIndex++)
            //    {
            //        if (cBillNo == EWBBillDetGrid.Rows[nRowIndex].Cells["BillNo"].Value)
            //        {
            //            EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.White;
            //            EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value = false;
            //        }
            //        else
            //        {
            //            break;
            //        }                                           
            //    }                
            //}
            //else
            //{
            //    EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
            //    EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value = true;

            //    for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
            //    {
            //        if (cBillNo == EWBBillDetGrid.Rows[nRowIndex].Cells["BillNo"].Value)
            //        {
            //            EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
            //            EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value = true;
            //        }
            //        else
            //        {
            //            break;
            //        }                                            
            //    }                
            //}


        }

        private void btnChkAll_Click(object sender , EventArgs e  )
        {
            Appmain.ChkAll(EWBBillDetGrid, false);
            CntrlBtn(true);
        }    
        private void btnUnChkAll_Click(object sender , EventArgs e )
        {
            Appmain.UnChkAll(EWBBillDetGrid);
            CntrlBtn(false);
        }
       
        private void CntrlBtn(bool lEnable)
        {
            
            if (lEnable)
            {
                btnGenerate.Enabled = true;
                btnPrint.Enabled = true;
                btnDetPrint.Enabled = true;
                btnCancel.Enabled = true;
                TxtVehNo.Enabled = true;
                CmbCancelRes.Enabled = true;
                txtCanRem.Enabled = true;
            }
            else
            {
                btnGenerate.Enabled = false;
                btnPrint.Enabled = false;
                btnCancel.Enabled = false;
                btnDetPrint.Enabled = false;
                TxtVehNo.Enabled = false;
                CmbCancelRes.Enabled = false;
                txtCanRem.Enabled = false;
            }            
        }   

        private async void btnGenerate_Click(object sender, EventArgs e )
        {
            string cValue, cRetvalue;
            ReqGenEwbPl ewbGen = new ReqGenEwbPl();
            string cBillNo = "";
            string cLstBillNo = "";
            bool IsIRNGen = false;
            Appmain.WriteToErrorLog("****************************************************************************************************************");
            Appmain.WriteToErrorLog("Computer Name: " + Environment.MachineName);
            Appmain.WriteToErrorLog("Date/Time: " + DateTime.Now.ToString());
            if (Appmain.nDebug == 0)
            {
                MessageBox.Show("EWB101");
            }
            try
            {
                foreach (DataGridViewRow row in EWBBillDetGrid.Rows)
                {
                    if (row.Cells["EWbNo"].Value.ToString().Trim() != "")
                    {
                        MessageBox.Show("You are about to generate EWB but we detected some bills for which you have already generated EWB. Please start tool selecting proper criteria.");
                    }
                }
                cLstBillNo = "";
                foreach (DataGridViewRow row in EWBBillDetGrid.Rows)
                {
                    if (Appmain.nDebug == 0)
                    {
                        MessageBox.Show("EWB102");
                    }

                    if ((bool)row.Cells[0].Value)
                    {
                        Appmain.WriteToErrorLog("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                        Appmain.WriteToErrorLog("******* Start cLstBillNo : " + cLstBillNo.ToString() + " BillNo : " + row.Cells["BillNo"].Value.ToString().Trim() + "*******");
                        txtErrMsg.Text = "Please wait while generating E-Way Bill No. for Bill No.: " + row.Cells["BillNo"].Value.ToString().Trim();
                        Appmain.WriteToErrorLog(txtErrMsg.Text);

                        if (Appmain.nDebug == 0)
                        {
                            MessageBox.Show("Generating EWB for Bill No." + row.Cells["BillNo"].Value.ToString().Trim());
                        }
                        ewbGen = new ReqGenEwbPl();
                        if (cLstBillNo != row.Cells["BillNo"].Value.ToString().Trim())
                        {
                            IsIRNGen = false;
                        }
                        cBillNo = row.Cells["BillNo"].Value.ToString().Trim();
                        Appmain.WriteToErrorLog(" cBillNo : " + cBillNo + " row get Bill No :" + row.Cells["BillNo"].Value.ToString().Trim());
                        if (!IsIRNGen)
                        {

                            cValue = row.Cells["SupplyType"].Value.ToString().Trim();
                            ewbGen.supplyType = cValue.Substring(0, 1);
                            cValue = row.Cells["SubType"].Value.ToString().Trim();
                            Appmain.WriteToErrorLog(" SubType : " + cValue + " cBillNo : " + cBillNo);
                            Appmain.WriteToErrorLog(" EWB Process 1 cBillNo : " + cBillNo);

                            if (cValue == "Supply")
                            {
                                cRetvalue = "1";
                            }
                            else if (cValue == "Import")
                            {
                                cRetvalue = "2";
                            }
                            else if (cValue == "Export")
                            {
                                cRetvalue = "3";
                            }
                            else if (cValue == "Job Work")
                            {
                                cRetvalue = "4";
                            }
                            else if (cValue == "For Own Use")
                            {
                                cRetvalue = "5";
                            }
                            else if (cValue == "Job Work Returns")
                            {
                                cRetvalue = "6";
                            }
                            else if (cValue == "Sales Return")
                            {
                                cRetvalue = "7";
                            }
                            else if (cValue == "Others")
                            {
                                cRetvalue = "8";
                            }
                            else if (cValue == "SKD/CKD")
                            {
                                cRetvalue = "9";
                            }
                            else if (cValue == "Line Sales")
                            {
                                cRetvalue = "10";
                            }
                            else if (cValue == "Recipient Not Known")
                            {
                                cRetvalue = "11";
                            }
                            else if (cValue == "Exhibition or Fairs")
                            {
                                cRetvalue = "12";
                            }
                            else
                            {
                                cRetvalue = "";
                            }

                            ewbGen.subSupplyType = cRetvalue;
                            ewbGen.subSupplyDesc = "";
                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("EWB104");
                            }
                            Appmain.WriteToErrorLog(" EWB Process 2 cBillNo : " + cBillNo);
                            cValue = row.Cells["DocType"].Value.ToString().Trim();
                            Appmain.WriteToErrorLog(" DocType : " + cValue);

                            if (cValue == "Tax Invoice")
                            {
                                cRetvalue = "INV";
                            }
                            else if (cValue == "Bill of Supply")
                            {
                                cRetvalue = "BIL";
                            }
                            else if (cValue == "Bill of Entry")
                            {
                                cRetvalue = "BOE";
                            }
                            else if (cValue == "Delivery Challan")
                            {
                                cRetvalue = "CHL";
                            }
                            else if (cValue == "Credit Note")
                            {
                                cRetvalue = "CNT";
                            }
                            else
                            {
                                cRetvalue = "OTH";
                            }
                            ewbGen.docType = cRetvalue;
                            ewbGen.docNo = row.Cells["DocNo"].Value.ToString().Trim();
                            ewbGen.docDate = row.Cells["DocDate"].Value.ToString().Trim();
                            Appmain.WriteToErrorLog(" EWB Process 3 cBillNo : " + cBillNo);
                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("EWB105");
                            }

                            ewbGen.fromGstin = row.Cells["From_GSTIN"].Value.ToString().Trim();
                            ewbGen.fromTrdName = row.Cells["From_OtherPartyName"].Value.ToString().Trim();
                            ewbGen.fromAddr1 = row.Cells["From_Address1"].Value.ToString().Trim();
                            ewbGen.fromAddr2 = row.Cells["From_Address2"].Value.ToString().Trim();
                            ewbGen.fromPlace = row.Cells["From_Place"].Value.ToString().Trim();

                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("EWB106");
                            }

                            ewbGen.fromPincode = Convert.ToInt32(row.Cells["Dispatch_Pincode"].Value);
                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("EWB107");
                            }

                            ewbGen.fromStateCode = Convert.ToInt32(Appmain.RetStateCode(row.Cells["Bill From_State"].Value.ToString().Trim()));
                            ewbGen.actFromStateCode = Convert.ToInt32(Appmain.RetStateCode(row.Cells["Dispatch From_State"].Value.ToString().Trim()));
                            Appmain.WriteToErrorLog(" EWB Process 4 cBillNo : " + cBillNo);

                            ewbGen.toGstin = row.Cells["To_GSTIN"].Value.ToString().Trim();
                            ewbGen.toTrdName = row.Cells["To_OtherPartyName"].Value.ToString().Trim();
                            ewbGen.toAddr1 = row.Cells["To_Address1"].Value.ToString().Trim();
                            ewbGen.toAddr2 = row.Cells["To_Address2"].Value.ToString().Trim();
                            ewbGen.toPlace = row.Cells["To_Place"].Value.ToString().Trim();
                            ewbGen.toPincode = Convert.ToInt32(row.Cells["Ship To_PinCode"].Value.ToString().Trim());
                            ewbGen.toStateCode = Convert.ToInt32(Appmain.RetStateCode(row.Cells["Bill To_State"].Value.ToString().Trim()));
                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("EWB108");
                            }

                            ewbGen.actToStateCode = Convert.ToInt32(Appmain.RetStateCode(row.Cells["Ship To State"].Value.ToString().Trim()));
                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("EWB109");
                            }
                            Appmain.WriteToErrorLog(" EWB Process 5 cBillNo : " + cBillNo);
                            cValue = row.Cells["Transaction Type"].Value.ToString().Trim();
                            cRetvalue = "";

                            if (cValue == "Regular")
                            {
                                cRetvalue = "1";
                            }
                            else if (cValue == "Bill To-Ship To")
                            {
                                cRetvalue = "2";
                            }
                            else if (cValue == "Bill From-Dispatch From")
                            {
                                cRetvalue = "3";
                            }
                            else if (cValue == "Combination Of 2 And 3")
                            {
                                cRetvalue = "4";
                            }
                            Appmain.WriteToErrorLog(" EWB Process cRetvalue : " + cRetvalue.ToString());
                            ewbGen.transactionType = Convert.ToInt32(cRetvalue);
                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("EWB1010");
                            }
                            Appmain.WriteToErrorLog(" EWB Process 6 cBillNo : " + cBillNo);

                            ewbGen.dispatchFromGSTIN = row.Cells["DispFromGSTIN"].Value.ToString().Trim();
                            ewbGen.dispatchFromTradeName = row.Cells["DispFromPtyName"].Value.ToString().Trim();
                            
                            ewbGen.shipToGSTIN = row.Cells["To_GSTIN"].Value.ToString().Trim();
                            ewbGen.shipToTradeName = row.Cells["To_OtherPartyName"].Value.ToString().Trim();

                            ewbGen.otherValue = Convert.ToDouble(row.Cells["Others"].Value.ToString().Trim());
                            ewbGen.totalValue = Convert.ToDouble(row.Cells["Assessable Value"].Value.ToString().Trim());
                            ewbGen.cgstValue = Convert.ToDouble(row.Cells["CGST Amount"].Value.ToString().Trim());
                            ewbGen.sgstValue = Convert.ToDouble(row.Cells["SGST Amount"].Value.ToString().Trim());
                            ewbGen.igstValue = Convert.ToDouble(row.Cells["IGST Amount"].Value.ToString().Trim());
                            ewbGen.cessValue = Convert.ToDouble(row.Cells["Cess Amount"].Value.ToString().Trim());
                            ewbGen.cessNonAdvolValue = Convert.ToDouble(row.Cells["Cess Non Advol Amount"].Value.ToString().Trim());
                            ewbGen.transporterId = row.Cells["TransId"].Value.ToString().Trim();
                            ewbGen.transporterName = row.Cells["Trans Name"].Value.ToString().Trim();
                            ewbGen.transDocNo = row.Cells["Trans DocNo"].Value.ToString().Trim();
                            ewbGen.totInvValue = Convert.ToDouble(row.Cells["Total Invoice Value"].Value.ToString().Trim());
                            Appmain.WriteToErrorLog(" EWB Process 7 cBillNo : " + cBillNo);
                            if (string.IsNullOrEmpty(txtkm.Text))
                            {
                                ewbGen.transDistance = row.Cells["Distance Level(Km)"].Value.ToString().Trim();
                            }
                            else
                            {
                                ewbGen.transDistance = txtkm.Text;
                            }

                            ewbGen.transDocDate = row.Cells["TransDate"].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(TxtVehNo.Text))
                            {
                                string cVehicle = row.Cells["VehicleNo"].Value.ToString().Trim(); // if vehicle no is found in any case then it will also generate Part - B
                                if (string.IsNullOrEmpty(cVehicle))
                                {
                                    ewbGen.transMode = "";
                                }
                                else
                                {
                                    ewbGen.vehicleNo = cVehicle;
                                    cValue = row.Cells["Trans Mode"].Value.ToString().Trim();

                                    if (cValue == "Road")
                                    {
                                        cRetvalue = "1";
                                    }
                                    else if (cValue == "Rail")
                                    {
                                        cRetvalue = "2";
                                    }
                                    else if (cValue == "Air")
                                    {
                                        cRetvalue = "3";
                                    }
                                    else if (cValue == "Ship")
                                    {
                                        cRetvalue = "4";
                                    }
                                    else
                                    {
                                        cRetvalue = "";
                                    }
                                    ewbGen.transMode = cRetvalue;
                                }
                            }
                            else
                            {
                                ewbGen.vehicleNo = TxtVehNo.Text.Trim();
                                ewbGen.transMode = "1";
                            }
                            Appmain.WriteToErrorLog(" EWB Process 8 cBillNo : " + cBillNo);
                            cValue = row.Cells["Vehicle Type"].Value.ToString().Trim();

                            if (cValue == "Regular")
                            {
                                cRetvalue = "R";
                            }
                            else
                            {
                                cRetvalue = "O";
                            }

                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("EWB1011");
                            }
                            ewbGen.vehicleType = cRetvalue;
                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("EWB1012");
                            }
                            Appmain.WriteToErrorLog(" EWB Process 9 cBillNo : " + cBillNo);
                            ewbGen.itemList = new List<ReqGenEwbPl.ItemListInReqEWBpl>();
                            ReqGenEwbPl.ItemListInReqEWBpl itm = new ReqGenEwbPl.ItemListInReqEWBpl();
                            foreach (DataGridViewRow DetRow in EWBBillDetGrid.Rows)
                            {
                                Appmain.WriteToErrorLog(cBillNo + " - cBillNo in Details");
                                if (cBillNo == DetRow.Cells["BillNo"].Value.ToString().Trim())
                                {
                                    Appmain.WriteToErrorLog("Item Details for Doc No.: " + cBillNo + " Row Index : " + DetRow.Index.ToString());
                                    itm = new ReqGenEwbPl.ItemListInReqEWBpl
                                    {
                                        productName = DetRow.Cells["Product"].Value.ToString().Trim(),
                                        productDesc = DetRow.Cells["Description"].Value.ToString().Trim(),
                                        hsnCode = Convert.ToInt32(DetRow.Cells["HSN"].Value),
                                        quantity = Convert.ToDouble(Convert.ToDecimal(DetRow.Cells["Qty"].Value)),
                                        qtyUnit = DetRow.Cells["UQC"].Value.ToString().Trim(),
                                        cgstRate = Convert.ToDouble(Convert.ToDecimal(DetRow.Cells["CGSTPer"].Value)),
                                        sgstRate = Convert.ToDouble(Convert.ToDecimal(DetRow.Cells["SGSTPer"].Value)),
                                        igstRate = Convert.ToDouble(Convert.ToDecimal(DetRow.Cells["IGSTPer"].Value)),
                                        cessRate = Convert.ToDouble(Convert.ToDecimal(DetRow.Cells["CessPer"].Value)),
                                        cessNonAdvol = 0,
                                        taxableAmount = Convert.ToDouble(Convert.ToDecimal(DetRow.Cells["Assessable Value"].Value))
                                    };
                                    ewbGen.itemList.Add(itm);
                                }
                            }
                            Appmain.WriteToErrorLog(" EWB Process 10 cBillNo : " + cBillNo);
                            Appmain.WriteToErrorLog("JSON " + JsonConvert.SerializeObject(ewbGen) + " for Doc No.: " + cBillNo);
                            Appmain.WriteToErrorLog("Before CallEWBGenAPI for Doc No.: " + cBillNo);

                            await CallEWBGenAPI(Appmain.EwbSession, ewbGen, row.Index, EWBBillDetGrid);
                            cLstBillNo = cBillNo;
                            Appmain.WriteToErrorLog("After CallEWBGenAPI for Doc No.: " + cBillNo);
                            Appmain.WriteToErrorLog("Process ended for EWB generation for Doc No.: " + cBillNo);
                            Appmain.WriteToErrorLog("******* End cLstBillNo : " + cLstBillNo.ToString() + " cBillNo : " + cBillNo.ToString() + "*******");
                            Appmain.WriteToErrorLog("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                        }
                    }
                    else
                    {
                        Appmain.WriteToErrorLog("Skipping Rows as found same Doc No " + row.Cells["BillNo"].Value.ToString() + " with different Item @ Row no. : " + row.Index.ToString());
                    }
                    
                }
                Appmain.WriteToErrorLog("****************************************************************************************************************");
            }
            catch (Exception ex)
            {
                MessageBox.Show("EWB Generation Error : " + ex.Message);
            }

            //while (nRowIndex<EWBBillDetGrid.RowCount -1)
            //{ 
            //    if (EWBBillDetGrid.Rows[nRowIndex].Cells["EWbNo"].Value.ToString().Trim() != "" )
            //    {
            //        MessageBox.Show("You are about to generate EWB but we detected some bills for which you have already generated EWB. Please start tool selecting proper criteria.");
            //        //Application.Exit();
            //    }
            //    nRowIndex += 1;
            //}
            //nRowIndex = 0;

            //for (nRowIndex = 0; nRowIndex < EWBBillDetGrid.RowCount - 1; nRowIndex++)
            //{
            //    if (Appmain.nDebug == 0)
            //    {
            //        MessageBox.Show("31");

            //    }

            //    if ((bool)EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value == true)
            //    {
            //        txtErrMsg.Text = "Please wait while generating E-Way Bill No. for Bill No.: " + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
            //        Appmain.WriteToErrorLog(txtErrMsg.Text);
            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("Generating EWB for Bill No." + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim());
            //        }

            //        ewbGen = new ReqGenEwbPl();
            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("33");
            //        }

            //        cBillNo = EWBBillDetGrid.Rows[nRowIndex].Cells["BillNo"].Value.ToString().Trim();

            //        cValue = EWBBillDetGrid.Rows[nRowIndex].Cells["SupplyType"].Value.ToString().Trim();
            //        ewbGen.supplyType = cValue.Substring(0, 1);
            //        cValue = EWBBillDetGrid.Rows[nRowIndex].Cells["SubType"].Value.ToString().Trim();


            //        if (cValue == "Supply")
            //        {
            //            cRetvalue = "1";
            //        }
            //        else if (cValue == "Import")
            //        {
            //            cRetvalue = "2";
            //        }
            //        else if (cValue == "Export")
            //        {
            //            cRetvalue = "3";
            //        }
            //        else if (cValue == "Job Work")
            //        {
            //            cRetvalue = "4";
            //        }
            //        else if (cValue == "For Own Use")
            //        {
            //            cRetvalue = "5";
            //        }
            //        else if (cValue == "Job Work Returns")
            //        {
            //            cRetvalue = "6";
            //        }
            //        else if (cValue == "Sales Return")
            //        {
            //            cRetvalue = "7";
            //        }
            //        else if (cValue == "Others")
            //        {
            //            cRetvalue = "8";
            //        }
            //        else if (cValue == "SKD/CKD")
            //        {
            //            cRetvalue = "9";
            //        }
            //        else if (cValue == "Line Sales")
            //        {
            //            cRetvalue = "10";
            //        }
            //        else if (cValue == "Recipient Not Known")
            //        {
            //            cRetvalue = "11";
            //        }
            //        else if (cValue == "Exhibition or Fairs")
            //        {
            //            cRetvalue = "12";
            //        }
            //        else
            //        {
            //            cRetvalue = "";
            //        }

            //        ewbGen.subSupplyType = cRetvalue;
            //        ewbGen.subSupplyDesc = "";
            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("34");
            //        }

            //        cValue = EWBBillDetGrid.Rows[nRowIndex].Cells["DocType"].Value.ToString().Trim();                        

            //        if (cValue == "Tax Invoice")
            //        {
            //            cRetvalue = "INV";
            //        }
            //        else if (cValue == "Bill of Supply")
            //        {
            //            cRetvalue = "BIL";
            //        }
            //        else if (cValue == "Bill of Entry")
            //        {
            //            cRetvalue = "BOE";
            //        }
            //        else if (cValue == "Delivery Challan")
            //        {
            //            cRetvalue = "CHL";
            //        }
            //        else if (cValue == "Credit Note")
            //        {
            //            cRetvalue = "CNT";
            //        }
            //        else
            //        {
            //            cRetvalue = "OTH";
            //        }
            //        ewbGen.docType = cRetvalue;

            //        ewbGen.docNo = EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
            //        ewbGen.docDate = EWBBillDetGrid.Rows[nRowIndex].Cells["DocDate"].Value.ToString().Trim();

            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("35");
            //        }

            //        ewbGen.fromGstin = EWBBillDetGrid.Rows[nRowIndex].Cells["From_GSTIN"].Value.ToString().Trim();
            //        ewbGen.fromTrdName = EWBBillDetGrid.Rows[nRowIndex].Cells["From_OtherPartyName"].Value.ToString().Trim();
            //        ewbGen.fromAddr1 = EWBBillDetGrid.Rows[nRowIndex].Cells["From_Address1"].Value.ToString().Trim();
            //        ewbGen.fromAddr2 = EWBBillDetGrid.Rows[nRowIndex].Cells["From_Address2"].Value.ToString().Trim();
            //        ewbGen.fromPlace = EWBBillDetGrid.Rows[nRowIndex].Cells["From_Place"].Value.ToString().Trim();

            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("36");
            //        }

            //        ewbGen.fromPincode = Convert.ToInt32(EWBBillDetGrid.Rows[nRowIndex].Cells["Dispatch_Pincode"].Value);
            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("37");
            //        }

            //        ewbGen.fromStateCode =  Convert.ToInt32( Appmain.RetStateCode(EWBBillDetGrid.Rows[nRowIndex].Cells["Bill From_State"].Value.ToString().Trim()));

            //        ewbGen.actFromStateCode = Convert.ToInt32(Appmain.RetStateCode(EWBBillDetGrid.Rows[nRowIndex].Cells["Dispatch From_State"].Value.ToString().Trim()));

            //        ewbGen.toGstin = EWBBillDetGrid.Rows[nRowIndex].Cells["To_GSTIN"].Value.ToString().Trim();
            //        ewbGen.toTrdName = EWBBillDetGrid.Rows[nRowIndex].Cells["To_OtherPartyName"].Value.ToString().Trim();
            //        ewbGen.toAddr1 = EWBBillDetGrid.Rows[nRowIndex].Cells["To_Address1"].Value.ToString().Trim();
            //        // If EWBBillDetGrid.Rows[nRowIndex].Cells["To_Address2"].Value.ToString().Trim() != "" Then
            //        ewbGen.toAddr2 = EWBBillDetGrid.Rows[nRowIndex].Cells["To_Address2"].Value.ToString().Trim();
            //        // End If

            //        ewbGen.toPlace = EWBBillDetGrid.Rows[nRowIndex].Cells["To_Place"].Value.ToString().Trim();
            //        ewbGen.toPincode = Convert.ToInt32(EWBBillDetGrid.Rows[nRowIndex].Cells["Ship To_PinCode"].Value.ToString().Trim());
            //        ewbGen.toStateCode = Convert.ToInt32(Appmain.RetStateCode(EWBBillDetGrid.Rows[nRowIndex].Cells["Bill To_State"].Value.ToString().Trim()));
            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("37A");
            //        }

            //        ewbGen.actToStateCode = Convert.ToInt32(Appmain.RetStateCode(EWBBillDetGrid.Rows[nRowIndex].Cells["Ship To State"].Value.ToString().Trim()));
            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("37B");
            //        }

            //        cValue = EWBBillDetGrid.Rows[nRowIndex].Cells["Transaction Type"].Value.ToString().Trim();
            //        cValue = "";

            //        if (cValue == "Regular")
            //        {
            //            cRetvalue = "1";
            //        }
            //        else if (cValue == "Bill To-Ship To")
            //        {
            //            cRetvalue = "2";
            //        }
            //        else if (cValue == "Bill From-Dispatch From")
            //        {
            //            cRetvalue = "3";
            //        }
            //        else if (cValue == "Combination Of 2 And 3")
            //        {
            //            cRetvalue = "4";
            //        }

            //        ewbGen.transactionType = int.Parse(cRetvalue);
            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("38");
            //        }

            //        ewbGen.dispatchFromGSTIN = EWBBillDetGrid.Rows[nRowIndex].Cells["DispFromGSTIN"].Value.ToString().Trim();
            //        ewbGen.dispatchFromTradeName = EWBBillDetGrid.Rows[nRowIndex].Cells["DispFromPtyName"].Value.ToString().Trim();
            //        ewbGen.shipToGSTIN = EWBBillDetGrid.Rows[nRowIndex].Cells["To_GSTIN"].Value.ToString().Trim();
            //        ewbGen.shipToTradeName = EWBBillDetGrid.Rows[nRowIndex].Cells["To_OtherPartyName"].Value.ToString().Trim();

            //        ewbGen.otherValue = Convert.ToDouble(EWBBillDetGrid.Rows[nRowIndex].Cells["Others"].Value.ToString().Trim());
            //        ewbGen.totalValue = Convert.ToDouble( EWBBillDetGrid.Rows[nRowIndex].Cells["Assessable Value"].Value.ToString().Trim());
            //        ewbGen.cgstValue =  Convert.ToDouble(EWBBillDetGrid.Rows[nRowIndex].Cells["CGST Amount"].Value.ToString().Trim());
            //        ewbGen.sgstValue = Convert.ToDouble(EWBBillDetGrid.Rows[nRowIndex].Cells["SGST Amount"].Value.ToString().Trim());
            //        ewbGen.igstValue = Convert.ToDouble(EWBBillDetGrid.Rows[nRowIndex].Cells["IGST Amount"].Value.ToString().Trim());
            //        ewbGen.cessValue = Convert.ToDouble(EWBBillDetGrid.Rows[nRowIndex].Cells["Cess Amount"].Value.ToString().Trim());
            //        ewbGen.cessNonAdvolValue = Convert.ToDouble(EWBBillDetGrid.Rows[nRowIndex].Cells["Cess Non Advol Amount"].Value.ToString().Trim());
            //        ewbGen.transporterId = EWBBillDetGrid.Rows[nRowIndex].Cells["TransId"].Value.ToString().Trim();
            //        ewbGen.transporterName = EWBBillDetGrid.Rows[nRowIndex].Cells["Trans Name"].Value.ToString().Trim();
            //        ewbGen.transDocNo = EWBBillDetGrid.Rows[nRowIndex].Cells["Trans DocNo"].Value.ToString().Trim();
            //        ewbGen.totInvValue = Convert.ToDouble(EWBBillDetGrid.Rows[nRowIndex].Cells["Total Invoice Value"].Value.ToString().Trim());

            //        if (string.IsNullOrEmpty(txtkm.Text))
            //        {
            //            ewbGen.transDistance = EWBBillDetGrid.Rows[nRowIndex].Cells["Distance Level(Km)"].Value.ToString().Trim();
            //        }
            //        else
            //        {
            //            ewbGen.transDistance = txtkm.Text;
            //        }

            //        ewbGen.transDocDate = EWBBillDetGrid.Rows[nRowIndex].Cells["TransDate"].Value.ToString().Trim();

            //        if (string.IsNullOrEmpty(TxtVehNo.Text))
            //        {
            //            string cVehicle = EWBBillDetGrid.Rows[nRowIndex].Cells["VehicleNo"].Value.ToString().Trim(); // if vehicle no is found in any case then it will also generate Part - B
            //            if (string.IsNullOrEmpty(cVehicle))
            //            {
            //                ewbGen.transMode = "";
            //            }
            //            else
            //            {
            //                ewbGen.vehicleNo = cVehicle;
            //                cValue = EWBBillDetGrid.Rows[nRowIndex].Cells["Trans Mode"].Value.ToString().Trim();                                

            //                if (cValue == "Road")
            //                {
            //                    cRetvalue = "1";
            //                }
            //                else if (cValue == "Rail")
            //                {
            //                    cRetvalue = "2";
            //                }
            //                else if (cValue == "Air")
            //                {
            //                    cRetvalue = "3";
            //                }
            //                else if (cValue == "Ship")
            //                {
            //                    cRetvalue = "4";
            //                }
            //                else
            //                {
            //                    cRetvalue = "";
            //                }
            //                ewbGen.transMode = cRetvalue;
            //            }
            //        }
            //        else
            //        {
            //            ewbGen.vehicleNo = TxtVehNo.Text.Trim();
            //            ewbGen.transMode = "1";
            //        }

            //        cValue = EWBBillDetGrid.Rows[nRowIndex].Cells["Vehicle Type"].Value.ToString().Trim();                        

            //        if (cValue == "Regular")
            //        {
            //            cRetvalue = "R";
            //        }
            //        else
            //        {
            //            cRetvalue = "O";
            //        }

            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("39");
            //        }

            //        ewbGen.vehicleType = cRetvalue;
            //        ewbGen.itemList = new List<ReqGenEwbPl.ItemListInReqEWBpl>();

            //        ewbGen.itemList.Add(new ReqGenEwbPl.ItemListInReqEWBpl
            //        {
            //            productName = EWBBillDetGrid.Rows[nRowIndex].Cells["Product"].Value.ToString().Trim(),
            //            productDesc = EWBBillDetGrid.Rows[nRowIndex].Cells["Description"].Value.ToString().Trim(),
            //            hsnCode = Convert.ToInt32(EWBBillDetGrid.Rows[nRowIndex].Cells["HSN"].Value),
            //            quantity = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nRowIndex].Cells["Qty"].Value)),
            //            qtyUnit = EWBBillDetGrid.Rows[nRowIndex].Cells["UQC"].Value.ToString().Trim(),
            //            cgstRate = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nRowIndex].Cells["CGSTPer"].Value)),
            //            sgstRate = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nRowIndex].Cells["SGSTPer"].Value)),
            //            igstRate = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nRowIndex].Cells["IGSTPer"].Value)),
            //            cessRate = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nRowIndex].Cells["CessPer"].Value)),
            //            cessNonAdvol = 0,
            //            taxableAmount = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nRowIndex].Cells["Assessable Value"].Value))
            //        });

            //        if (Appmain.nDebug == 0)
            //        {
            //            MessageBox.Show("40");
            //        }
            //        nRowIndex++;
            //        nRowCtr = nRowIndex;

            //        for (nItemCtr = nRowCtr; nItemCtr < EWBBillDetGrid.RowCount; nItemCtr++)
            //        {
            //            // MessageBox.Show("B " + nItemCtr.ToString());
            //            if (cBillNo == EWBBillDetGrid.Rows[nItemCtr].Cells["BillNo"].Value.ToString().Trim())
            //            {
            //                ewbGen.itemList.Add(new ReqGenEwbPl.ItemListInReqEWBpl
            //                {
            //                    productName = EWBBillDetGrid.Rows[nItemCtr].Cells["Product"].Value.ToString().Trim(),
            //                    productDesc = EWBBillDetGrid.Rows[nItemCtr].Cells["Description"].Value.ToString().Trim(),
            //                    hsnCode = Convert.ToInt32(EWBBillDetGrid.Rows[nItemCtr].Cells["HSN"].Value),
            //                    quantity = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nItemCtr].Cells["Qty"].Value)),
            //                    qtyUnit = EWBBillDetGrid.Rows[nItemCtr].Cells["UQC"].Value.ToString().Trim(),
            //                    cgstRate = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nItemCtr].Cells["CGSTPer"].Value)),
            //                    sgstRate = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nItemCtr].Cells["SGSTPer"].Value)),
            //                    igstRate = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nItemCtr].Cells["IGSTPer"].Value)),
            //                    cessRate = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nItemCtr].Cells["CessPer"].Value)),
            //                    cessNonAdvol = 0,
            //                    taxableAmount = Convert.ToDouble(Convert.ToDecimal(EWBBillDetGrid.Rows[nItemCtr].Cells["Assessable Value"].Value))
            //                });
            //            }
            //            else
            //            {
            //                break;
            //            }
            //        }
            //        nRowIndex = nItemCtr - 1;
            //        await CallEWBGenAPI(Appmain.EwbSession, ewbGen, nRowIndex);
            //    }
            //}

        }

        public async Task CallEWBGenAPI(object EwbSession, TaxProEWB.API.ReqGenEwbPl ewbGen, int nRowIndex, DataGridView EWBBillDetGrid)
        {
            string cAPIResponse, cMsg;
            string cTblName, cIdFld;
            Appmain.WriteToErrorLog("CallEWBGenAPI start");
            Appmain.WriteToErrorLog("In CallEWBGenAPI Method - 1 for Doc No. " + EWBBillDetGrid.Rows[nRowIndex].Cells["BillNo"].Value.ToString() + " RowIndex " + nRowIndex.ToString());
            if (Appmain.nDebug == 0)
            {
                MessageBox.Show(JsonConvert.SerializeObject(ewbGen));
            }

            try
            {
                var TxnResp = await EWBAPI.GenEWBAsync(Appmain.EwbSession,  ewbGen);

                if (Appmain.nDebug == 0)
                {
                    MessageBox.Show(TxnResp.TxnOutcome);
                }

                if (TxnResp.IsSuccess)
                {
                    Appmain.WriteToErrorLog("CallEWBGenAPI TxnResp - IsSuccess");
                    if (Appmain.nDebug == 0)
                    {
                        MessageBox.Show("41");
                    }

                    cAPIResponse = JsonConvert.SerializeObject(TxnResp.RespObj);
                    Appmain.WriteToErrorLog("EWay Bill Generation Success Response " + Environment.NewLine + cAPIResponse);
                    var result = JObject.Parse(cAPIResponse);
                    Appmain.WriteToErrorLog("In CallEWBGenAPI Method - 2" + " RowIndex " + nRowIndex.ToString());

                    var cEwayBillNo = result["ewayBillNo"].ToString();
                    var cewayBillDate = result["ewayBillDate"].ToString();
                    var cvalidUpto = result["validUpto"].ToString();

                    Appmain.WriteToErrorLog("In CallEWBGenAPI cEwayBillNo : "+ cEwayBillNo + " cewayBillDate : "+ cewayBillDate + " RowIndex " + nRowIndex.ToString());

                    cTblName = EWBBillDetGrid.Rows[nRowIndex].Cells["TblName"].Value.ToString();
                    cIdFld = EWBBillDetGrid.Rows[nRowIndex].Cells["IdFld"].Value.ToString();

                    Appmain.WriteToErrorLog("In CallEWBGenAPI Method - 3" + " RowIndex " + nRowIndex.ToString());

                    var strSelect = "UPDATE " + cTblName + " SET EwbNo = '" + cEwayBillNo + "', EwbDt = '" + cewayBillDate + "'" +
                                    " WHERE " + cIdFld + " = '" + EWBBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value.ToString() + "'";

                    Appmain.WriteToErrorLog("In CallEWBGenAPI Method - 4 - After Update EWB" + " RowIndex " + nRowIndex.ToString());
                    Appmain.WriteToErrorLog("EWB detail updation query \n" + strSelect + " RowIndex " + nRowIndex.ToString());

                    using (var da = new SqlDataAdapter())
                    {
                        using (var Command = new SqlCommand(strSelect, Appmain.sqlCnn))
                        {
                            da.UpdateCommand = Command;
                            await da.UpdateCommand.ExecuteNonQueryAsync();
                        }
                    }

                    EWBBillDetGrid.Rows[nRowIndex].Cells["EWbNo"].Value = cEwayBillNo;
                    EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    cMsg = "Generated E-Way Bill No: " + cEwayBillNo + " for Bill No." + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
                    txtErrMsg.Text = cMsg;
                    EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;
                    Appmain.WriteToErrorLog(cMsg);
                }
                else
                {
                    Appmain.WriteToErrorLog("CallEWBGenAPI TxnResp Failure");
                    if (Appmain.nDebug == 0)
                    {
                        MessageBox.Show("42");
                    }

                    EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                    txtErrMsg.Text = TxnResp.TxnOutcome + TxnResp.TxnOutcome;
                    Appmain.WriteToErrorLog("EWay Bill Failure Response " + Environment.NewLine + TxnResp.TxnOutcome + " --- " + TxnResp.Info);

                    if (TxnResp.TxnOutcome.Contains("702"))
                    {
                        if (Appmain.nDebug == 0)
                        {
                            MessageBox.Show("Distance Checking.");
                        }

                        var respInfoPl = JsonConvert.DeserializeObject<RespInfoPl>(TxnResp.Info);

                        Appmain.WriteToErrorLog("Update Pincode Distance " + JsonConvert.SerializeObject(respInfoPl));
                        if (Appmain.nDebug == 0)
                        {
                            MessageBox.Show(JsonConvert.SerializeObject(respInfoPl));
                        }

                        ewbGen.transDistance = respInfoPl.distance;
                        EWBBillDetGrid.Rows[nRowIndex].Cells["Distance Level(Km)"].Value = respInfoPl.distance;
                        await UpdtPinToPinDist(respInfoPl.fromPincode, respInfoPl.toPincode, respInfoPl.distance);
                        await CallEWBGenAPI(EwbSession, ewbGen, nRowIndex, EWBBillDetGrid);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("EWB Generation API Call Error: " + ex.Message);
            }
            Appmain.WriteToErrorLog("CallEWBGenAPI end");
        }
        private async void btnCancel_Click(object sender  , EventArgs e )
        {
            ReqCancelEwbPl reqCancelEWB;
            string cEWBNo;

            try
            {
                
                if (!string.IsNullOrEmpty(CmbCancelRes.Text) && !string.IsNullOrEmpty(txtCanRem.Text))
                {
                    var da = new SqlDataAdapter();
                    SqlCommand Command;
                    string cMsg = "";
                    foreach (DataGridViewRow row in EWBBillDetGrid.Rows)
                    {
                        cEWBNo = row.Cells["EWbNo"].Value.ToString().Trim();
                        cMsg = "Cancelling E-Way Bill No: " + cEWBNo + " of Bill No." + row.Cells["DocNo"].Value.ToString().Trim();
                        txtErrMsg.Text = cMsg;
                        EWBBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                        Appmain.WriteToErrorLog(cMsg);

                        if ((bool)row.Cells[0].Value && !string.IsNullOrEmpty(cEWBNo))
                        {
                            if (Appmain.nDebug == 0)
                            {
                                MessageBox.Show("Cancelling EWB for Bill No." + row.Cells["DocNo"].Value.ToString().Trim());
                            }

                            reqCancelEWB = new ReqCancelEwbPl();
                            reqCancelEWB.ewbNo = long.Parse(cEWBNo);

                            if (CmbCancelRes.Text == "Duplicate")
                            {
                                reqCancelEWB.cancelRsnCode = 1;
                            }
                            else if (CmbCancelRes.Text == "Order Cancelled")
                            {
                                reqCancelEWB.cancelRsnCode = 2;
                            }
                            else if (CmbCancelRes.Text == "Data Entry Mistake")
                            {
                                reqCancelEWB.cancelRsnCode = 3;
                            }
                            else if (CmbCancelRes.Text == "Others")
                            {
                                reqCancelEWB.cancelRsnCode = 4;
                            }


                            var respCancelEWB = await EWBAPI.CancelEWBAsync(Appmain.EwbSession, reqCancelEWB);
                            
                            if (respCancelEWB.IsSuccess)
                            {
                                string cTblName = row.Cells["TblName"].Value.ToString();
                                string cIdFld = row.Cells["IdFld"].Value.ToString();

                                string strSelect = $"UPDATE {cTblName} SET EwbNo = '', EwbDt = '' WHERE {cIdFld} = '{row.Cells["InvId"].Value}'";

                                Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                                da.UpdateCommand = new SqlCommand(strSelect, Appmain.sqlCnn);
                                await da.UpdateCommand.ExecuteNonQueryAsync();
                                Command.Dispose();

                                cMsg = "Successfully Cancelled E-Way Bill No: " + cEWBNo + " of Bill No." + row.Cells["DocNo"].Value.ToString().Trim();
                                row.Cells["ErrorList"].Value = cMsg;
                                txtErrMsg.Text = cMsg;
                                Appmain.WriteToErrorLog(cMsg);
                                row.DefaultCellStyle.ForeColor = Color.Blue;
                            }
                            else
                            {
                                row.Cells["ErrorList"].Value = respCancelEWB.TxnOutcome;
                                row.DefaultCellStyle.ForeColor = Color.Red;
                                txtErrMsg.Text = respCancelEWB.TxnOutcome;
                                Appmain.WriteToErrorLog("EWay Bill Cancellation Failure " + Environment.NewLine + respCancelEWB.TxnOutcome);
                            }
                        }
                        else
                        {
                            cMsg = "EWB is not generated for Bill No." + row.Cells["DocNo"].Value.ToString().Trim() + ". So EWB Cancellation not possible.";
                            txtErrMsg.Text = cMsg;
                            row.Cells["ErrorList"].Value = cMsg;
                            row.DefaultCellStyle.ForeColor = Color.Red;
                            Appmain.WriteToErrorLog("EWay Bill Cancellation Failure " + cMsg);
                        }
                    }

                    //for (int nRowIndex = 0; nRowIndex < EWBBillDetGrid.RowCount; nRowIndex++)
                    //{
                    //    cEWBNo = EWBBillDetGrid.Rows[nRowIndex].Cells["EWbNo"].Value.ToString().Trim();
                    //    cMsg = "Cancelling E-Way Bill No: " + cEWBNo + " of Bill No." + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
                    //    txtErrMsg.Text = cMsg;
                    //    EWBBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                    //    Appmain.WriteToErrorLog(cMsg);

                    //    if ((bool)EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value && !string.IsNullOrEmpty(cEWBNo))
                    //    {
                    //        if (Appmain.nDebug == 0)
                    //        {
                    //            MessageBox.Show("Cancelling EWB for Bill No." + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim());
                    //        }

                    //        reqCancelEWB = new ReqCancelEwbPl();
                    //        reqCancelEWB.ewbNo = long.Parse(cEWBNo);

                    //        if (CmbCancelRes.Text == "Duplicate")
                    //        {
                    //            reqCancelEWB.cancelRsnCode = 1;
                    //        }
                    //        else if (CmbCancelRes.Text == "Order Cancelled")
                    //        {
                    //            reqCancelEWB.cancelRsnCode = 2;
                    //        }
                    //        else if (CmbCancelRes.Text == "Data Entry Mistake")
                    //        {
                    //            reqCancelEWB.cancelRsnCode = 3;
                    //        }
                    //        else if (CmbCancelRes.Text == "Others")
                    //        {
                    //            reqCancelEWB.cancelRsnCode = 4;
                    //        }


                    //        var respCancelEWB = await EWBAPI.CancelEWBAsync(Appmain.EwbSession, reqCancelEWB);

                    //        if (respCancelEWB.IsSuccess)
                    //        {
                    //            string cTblName = EWBBillDetGrid.Rows[nRowIndex].Cells["TblName"].Value.ToString();
                    //            string cIdFld = EWBBillDetGrid.Rows[nRowIndex].Cells["IdFld"].Value.ToString();

                    //            string strSelect = $"UPDATE {cTblName} SET EwbNo = '', EwbDt = '' WHERE {cIdFld} = {Commfunc.V2C(EWBBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value)}";

                    //            Command = new SqlCommand(strSelect, Appmain.sqlCnn);
                    //            da.UpdateCommand = new SqlCommand(strSelect, Appmain.sqlCnn);
                    //            await da.UpdateCommand.ExecuteNonQueryAsync();
                    //            Command.Dispose();

                    //            cMsg = "Successfully Cancelled E-Way Bill No: " + cEWBNo + " of Bill No." + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
                    //            EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;
                    //            txtErrMsg.Text = cMsg;
                    //            Appmain.WriteToErrorLog(cMsg);
                    //            EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    //        }
                    //        else
                    //        {
                    //            EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = respCancelEWB.TxnOutcome;
                    //            EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //            txtErrMsg.Text = respCancelEWB.TxnOutcome;
                    //            Appmain.WriteToErrorLog("EWay Bill Cancellation Failure " + Environment.NewLine + respCancelEWB.TxnOutcome);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        cMsg = "EWB is not generated for Bill No." + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim() + ". So EWB Cancellation not possible.";
                    //        txtErrMsg.Text = cMsg;
                    //        EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;
                    //        EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //        Appmain.WriteToErrorLog("EWay Bill Cancellation Failure " + cMsg);
                    //    }
                    //}
                }
                else
                {
                    MessageBox.Show("Before cancelling E-Way Bill, please provide Cancellation Reason and Remarks compulsory.", "Reason Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("EWB Cancellation Error: " + ex.Message);
            }
        }

        private async void btnPrint_Click(object sender , EventArgs e  )
        {
            await PrintEWB(false);
        }
        //private async void Button1_Click(object sender , EventArgs e )
        //{
        //    await PrintEWB(true);
        //}
        private async void btnDetPrint_Click(object sender, EventArgs e)
        {
            await PrintEWB(true);
        }
        public async Task PrintEWB(bool lDetail )
        {
            string cEWBNo;
            string cMsg  = "";
            Appmain.WriteToErrorLog(" EWB Process PrintEWB");
            try
            {
                foreach (DataGridViewRow Row in EWBBillDetGrid.Rows)
                {
                    cEWBNo = Row.Cells["EWbNo"].Value.ToString().Trim();
                    cMsg = "Printing E-Way Bill No: " + cEWBNo + " of Bill No. " + Row.Cells["DocNo"].Value.ToString().Trim();
                    EWBBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                    txtErrMsg.Text = cMsg;
                    Appmain.WriteToErrorLog(cMsg);

                    if ((bool)Row.Cells[0].Value && !string.IsNullOrEmpty(cEWBNo))
                    {
                        if (Appmain.nDebug == 0)
                        {
                            MessageBox.Show("Printing EWB for Bill No. " + Row.Cells["DocNo"].Value.ToString().Trim());
                        }
                        TxnRespWithObjAndInfo<RespGetEWBDetail> TxnResp = await EWBAPI.GetEWBDetailAsync(Appmain.EwbSession, long.Parse(cEWBNo));

                        //var TxnResp = await EWBAPI.GetEWBDetailAsync(Appmain.EwbSession, long.Parse(cEWBNo));
                        // SetTokenDetail();
                        var strJson = JsonConvert.SerializeObject(TxnResp.RespObj);
                        TaxProEWB.API.ReqPrintEWB reqPrintEWB = new TaxProEWB.API.ReqPrintEWB();
                        reqPrintEWB = JsonConvert.DeserializeObject<TaxProEWB.API.ReqPrintEWB>(strJson);

                        if (TxnResp.IsSuccess)
                        {
                            EWBAPI.PrintEWB(Appmain.EwbSession, reqPrintEWB, "", true, lDetail);
                            //EWBAPI.PrintEWB(Appmain.EwbSession, (TaxProEWB.API.ReqPrintEWB)TxnResp.RespObj, "", true, lDetail);
                            cMsg = "Successfully Printed E-Way Bill No: " + cEWBNo + " of Bill No. " + Row.Cells["DocNo"].Value.ToString().Trim();
                            Row.Cells["ErrorList"].Value = cMsg;
                            txtErrMsg.Text = cMsg;
                            Row.DefaultCellStyle.ForeColor = Color.Blue;
                            Appmain.WriteToErrorLog(cMsg);
                        }
                        else
                        {
                            Row.Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                            Row.DefaultCellStyle.ForeColor = Color.Red;
                            txtErrMsg.Text = TxnResp.TxnOutcome;
                            Appmain.WriteToErrorLog("EWay Bill Printing Failure " + TxnResp.TxnOutcome);
                        }
                    }
                    else
                    {
                        cMsg = "EWB is not generated for Bill No. " + Row.Cells["DocNo"].Value.ToString().Trim() + ". So Printing not available.";
                        txtErrMsg.Text = cMsg;
                        Row.Cells["ErrorList"].Value = cMsg;
                        Appmain.WriteToErrorLog("EWay Bill Printing Failure " + cMsg);
                        Row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }

                //for (int nRowIndex = 0; nRowIndex < EWBBillDetGrid.RowCount; nRowIndex++)
                //{
                //    cEWBNo = EWBBillDetGrid.Rows[nRowIndex].Cells["EWbNo"].Value.ToString().Trim();
                //    cMsg = "Printing E-Way Bill No: " + cEWBNo + " of Bill No. " + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
                //    EWBBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                //    txtErrMsg.Text = cMsg;
                //    Appmain.WriteToErrorLog(cMsg);

                //    if ((bool)EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value && !string.IsNullOrEmpty(cEWBNo))
                //    {
                //        if (Appmain.nDebug == 0)
                //        {
                //            MessageBox.Show("Printing EWB for Bill No. " + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim());
                //        }

                //        var TxnResp = await EWBAPI.GetEWBDetailAsync(Appmain.EwbSession, long.Parse(cEWBNo));
                //        // SetTokenDetail();

                //        if (TxnResp.IsSuccess)
                //        {
                //            EWBAPI.PrintEWB(Appmain.EwbSession, (TaxProEWB.API.ReqPrintEWB)TxnResp.RespObj, "", true, lDetail);
                //            cMsg = "Successfully Printed E-Way Bill No: " + cEWBNo + " of Bill No. " + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
                //            EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;
                //            txtErrMsg.Text = cMsg;
                //            EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //            Appmain.WriteToErrorLog(cMsg);
                //        }
                //        else
                //        {
                //            EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                //            EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //            txtErrMsg.Text = TxnResp.TxnOutcome;
                //            Appmain.WriteToErrorLog("EWay Bill Printing Failure " + TxnResp.TxnOutcome);
                //        }
                //    }
                //    else
                //    {
                //        cMsg = "EWB is not generated for Bill No. " + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim() + ". So Printing not available.";
                //        txtErrMsg.Text = cMsg;
                //        EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;
                //        Appmain.WriteToErrorLog("EWay Bill Printing Failure " + cMsg);
                //        EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("EWB Printing Error: " + ex.Message);
            }
        }

        private async Task UpdtPinToPinDist(object fromPin, object toPin, object Dist)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int nRecCount;

            string strSelect;
            try
            {
                strSelect = "SELECT * FROM " + Appmain.cYrsDBO + ".dbo.PincodeDet " +
                            "WHERE FPin = @fromPin AND TPin = @toPin";

                command = new SqlCommand(strSelect, Appmain.sqlCnn);
                command.Parameters.AddWithValue("@fromPin", fromPin.ToString().Trim());
                command.Parameters.AddWithValue("@toPin", toPin.ToString().Trim());

                adapter.SelectCommand = command;
                adapter.Fill(ds, "PIN");

                nRecCount = ds.Tables[0].Rows.Count;

                if (nRecCount == 0)
                {
                    strSelect = "INSERT INTO " + Appmain.cYrsDBO + ".dbo.PincodeDet (FPin, TPin, Distance) " +
                                "VALUES (@fromPin, @toPin, @distance)";
                    Appmain.WriteToErrorLog("New Pincode combination Insert " + fromPin.ToString().Trim() + " to " + toPin.ToString().Trim() + " Dist: " + Dist.ToString().Trim());
                }
                else
                {
                    strSelect = "UPDATE " + Appmain.cYrsDBO + ".dbo.PincodeDet " +
                                "SET Distance = @distance " +
                                "WHERE FPin = @fromPin AND TPin = @toPin";
                    Appmain.WriteToErrorLog("Existing Pincode combination Update " + fromPin.ToString().Trim() + " to " + toPin.ToString().Trim() + " Dist: " + Dist.ToString().Trim());
                }

                command = new SqlCommand(strSelect, Appmain.sqlCnn);
                command.Parameters.AddWithValue("@fromPin", fromPin.ToString().Trim());
                command.Parameters.AddWithValue("@toPin", toPin.ToString().Trim());
                command.Parameters.AddWithValue("@distance", Dist.ToString().Trim());

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Distance Error: " + ex.Message);
            }            
        }

        private void BtnDebug_Click(object sender , EventArgs e)
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
                string pwd = Microsoft.VisualBasic.Interaction.InputBox("Enter Key", "Enable Debug Mode", "");

                if (pwd == "1723306907")
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
        private void btnTKN_Click(object sender, EventArgs e)
        {
            Appmain.GetAuthToken("EWAYBILL");
        }
        private async void BtnGetDetail_Click(object sender, EventArgs e)
        {
            string cEWBNo = "";
            try
            {
                Appmain.WriteToErrorLog("****************************************************************************************************************");
                Appmain.WriteToErrorLog("Computer Name: " + Environment.MachineName);
                Appmain.WriteToErrorLog("Date/Time: " + DateTime.Now.ToString());

                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand command;
                string cMsg = "";
                foreach (DataGridViewRow row in EWBBillDetGrid.Rows)
                {
                    cEWBNo = row.Cells["EWbNo"].Value.ToString().Trim();
                    Appmain.WriteToErrorLog("===============================================================================================");
                    Appmain.WriteToErrorLog("Initializing Get Detail Processes for " + cEWBNo + " RowIndex " + row.Index.ToString());

                    cMsg = "Fetching E-Way Bill No: " + cEWBNo + " of Bill No. " + row.Cells["DocNo"].Value.ToString().Trim();
                    txtErrMsg.Text = cMsg;
                    EWBBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                    Appmain.WriteToErrorLog(cMsg);
                    
                    if ((bool)row.Cells[0].Value && !string.IsNullOrEmpty(row.Cells["DocNo"].Value.ToString().Trim()) && string.IsNullOrEmpty(cEWBNo))
                    {
                        if (Appmain.nDebug == 0)
                        {
                            MessageBox.Show("Getting EWB details for Bill No. " + row.Cells["DocNo"].Value.ToString().Trim());
                        }

                        string cDocNo = row.Cells["DocNo"].Value.ToString().Trim();
                        string cValue = row.Cells["DocType"].Value.ToString().Trim();
                        string cDocType;

                        switch (cValue)
                        {
                            case "Tax Invoice":
                                cDocType = "INV";
                                break;
                            case "Bill of Supply":
                                cDocType = "BIL";
                                break;
                            case "Bill of Entry":
                                cDocType = "BOE";
                                break;
                            case "Delivery Challan":
                                cDocType = "CHL";
                                break;
                            case "Credit Note":
                                cDocType = "CNT";
                                break;
                            default:
                                cDocType = "OTH";
                                break;
                        }

                        var TxnResp = await EWBAPI.GetEwayBillGeneratedByConsignor(Appmain.EwbSession, cDocType, cDocNo);
                        if (TxnResp.IsSuccess)
                        {
                            string cTblName = row.Cells["TblName"].Value.ToString();
                            string cIdFld = row.Cells["IdFld"].Value.ToString();

                            string cAPIResponse = JsonConvert.SerializeObject(TxnResp.RespObj);
                            Appmain.WriteToErrorLog("cAPIResponse : " + cAPIResponse);
                            JObject result = JObject.Parse(cAPIResponse);
                            string cEwayBillNo = result["ewayBillNo"].ToString();
                            string ceWayBillDate = result["ewayBillDate"].ToString();
                            string cvalidUpto = result["validUpto"].ToString();

                            string strSelect = "UPDATE " + cTblName + " SET EwbNo = '" + cEwayBillNo + "'"+
                                               ", EwbDt = '" + ceWayBillDate + "'" +
                                               " WHERE " + cIdFld + " = '" + row.Cells["InvId"].Value.ToString() + "'";

                            command = new SqlCommand(strSelect, Appmain.sqlCnn);
                            da.UpdateCommand = command;

                            await da.UpdateCommand.ExecuteNonQueryAsync();
                            command.Dispose();

                            cMsg = "Successfully fetched E-Way Bill No: " + cEWBNo + " of Bill No. " + row.Cells["DocNo"].Value.ToString().Trim();
                            row.Cells["ErrorList"].Value = cMsg;
                            txtErrMsg.Text = cMsg;
                            Appmain.WriteToErrorLog(cMsg);
                            row.DefaultCellStyle.ForeColor = Color.Blue;
                        }
                        else
                        {
                            row.Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                            row.DefaultCellStyle.ForeColor = Color.Red;
                            txtErrMsg.Text = TxnResp.TxnOutcome;
                            Appmain.WriteToErrorLog("Get EWay Bill Detail Failure. " + TxnResp.TxnOutcome);
                        }
                    }
                }
                
                //for (int nRowIndex = 0; nRowIndex < EWBBillDetGrid.RowCount; nRowIndex++)
                //{
                //    cEWBNo = EWBBillDetGrid.Rows[nRowIndex].Cells["EWbNo"].Value.ToString().Trim();
                //    cMsg = "Fetching E-Way Bill No: " + cEWBNo + " of Bill No. " + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
                //    txtErrMsg.Text = cMsg;
                //    EWBBillDetGrid.CurrentRow.Cells["ErrorList"].Value = cMsg;
                //    Appmain.WriteToErrorLog(cMsg);

                //    if ((bool)EWBBillDetGrid.Rows[nRowIndex].Cells[0].Value &&
                //        !string.IsNullOrEmpty(EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim()) &&
                //        string.IsNullOrEmpty(cEWBNo))
                //    {
                //        if (Appmain.nDebug == 0)
                //        {
                //            MessageBox.Show("Getting EWB details for Bill No. " + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim());
                //        }

                //        string cDocNo = EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
                //        string cValue = EWBBillDetGrid.Rows[nRowIndex].Cells["DocType"].Value.ToString().Trim();
                //        string cDocType;

                //        switch (cValue)
                //        {
                //            case "Tax Invoice":
                //                cDocType = "INV";
                //                break;
                //            case "Bill of Supply":
                //                cDocType = "BIL";
                //                break;
                //            case "Bill of Entry":
                //                cDocType = "BOE";
                //                break;
                //            case "Delivery Challan":
                //                cDocType = "CHL";
                //                break;
                //            case "Credit Note":
                //                cDocType = "CNT";
                //                break;
                //            default:
                //                cDocType = "OTH";
                //                break;
                //        }

                //        var TxnResp = await EWBAPI.GetEwayBillGeneratedByConsignor(Appmain.EwbSession, cDocType, cDocNo);
                //        if (TxnResp.IsSuccess)
                //        {
                //            string cTblName = EWBBillDetGrid.Rows[nRowIndex].Cells["TblName"].Value.ToString();
                //            string cIdFld = EWBBillDetGrid.Rows[nRowIndex].Cells["IdFld"].Value.ToString();

                //            string cAPIResponse = JsonConvert.SerializeObject(TxnResp.RespObj);
                //            Appmain.WriteToErrorLog(cAPIResponse);
                //            JObject result = JObject.Parse(cAPIResponse);
                //            string cEwayBillNo = result["ewayBillNo"].ToString();
                //            string ceWayBillDate = result["ewayBillDate"].ToString();
                //            string cvalidUpto = result["validUpto"].ToString();

                //            string strSelect = "UPDATE " + cTblName + " SET EwbNo = " + Commfunc.V2C(cEwayBillNo) +
                //                               ", EwbDt = " + Commfunc.V2C(ceWayBillDate) +
                //                               " WHERE " + cIdFld + " = " + Commfunc.V2C(EWBBillDetGrid.Rows[nRowIndex].Cells["InvId"].Value);

                //            command = new SqlCommand(strSelect, Appmain.sqlCnn);
                //            da.UpdateCommand = command;

                //            await da.UpdateCommand.ExecuteNonQueryAsync();
                //            command.Dispose();

                //            cMsg = "Successfully fetched E-Way Bill No: " + cEWBNo + " of Bill No. " + EWBBillDetGrid.Rows[nRowIndex].Cells["DocNo"].Value.ToString().Trim();
                //            EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = cMsg;
                //            txtErrMsg.Text = cMsg;
                //            Appmain.WriteToErrorLog(cMsg);
                //            EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //        }
                //        else
                //        {
                //            EWBBillDetGrid.Rows[nRowIndex].Cells["ErrorList"].Value = TxnResp.TxnOutcome;
                //            EWBBillDetGrid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //            txtErrMsg.Text = TxnResp.TxnOutcome;
                //            Appmain.WriteToErrorLog("Get EWay Bill Detail Failure. " + TxnResp.TxnOutcome);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fetch EWB Detail Error: " + ex.Message);
            }
            Appmain.WriteToErrorLog("Process ended for Get Detail process Doc No.: " + cEWBNo);
            Appmain.WriteToErrorLog("=============================================================================================");
        }

       
    }
    public class RespInfoPl
    {
        public string toPincode;
        public string distance;
        public string fromPincode;
    }

}
