using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TaxProEWB.API;
using TaxProEInvoice.API;
using System.Security.Principal;
using System.Data;
using static System.Windows.Forms.AxHost;
using System.ComponentModel;
using static System.Net.WebRequestMethods;
using static System.Collections.Specialized.BitVector32;
using System.Drawing;
//using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using System.Text;
using System.Linq;
using JSGSTTool;

// Hello How are you.

//Hello 2

namespace EWayBillTool
{
    class AppMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static string[] Args;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int WritePrivateProfileString(string lpBuffer, string fIRSTKEY, string cvalue, string iniName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileString(string lpBuffer, string fIRSTKEY, string cvalue, StringBuilder lretval, int lsize, string iniName);


        //public string cSQLServer = @"(local)\SQL2019"; Remove Comment
        //public string cYrsDBO = "Rawal_Years"; Remove Comment
        //public string cCompDBO = "GJSRT2526"; Remove Comment
        //public string cEWBTble = "EWB_GJSRT2526"; Remove Comment
        //public string cInvTble = "EINV_GJSRT2526"; Remove Comment


        public string cSQLServer = @"(local)\SQL2019";
        public string cYrsDBO = "LotusBloom_Years";
        public string cCompDBO = "LBPL2526";
        public string cEWBTble = "EWB_LBPL2526";
        public string cInvTble = "EINV_LBPL2526";

        public string cPtyGstIn, cPtyGSPUsr, cPtyGSPPwd, cASPName, cASPUsr, cASPPwd, cASPURL, cSearchGSTIN;
        public string cSQLPwd = "cspl@123";
        public int nChkRow = 0;
        public int nDebug = 1;
        public static string FuncType = "";

        public SqlConnection sqlCnn = new SqlConnection();
        public EWBSession EwbSession = new EWBSession();
        public eInvoiceSession eInvSession = new eInvoiceSession();

        //public APISession PubAPISession = new APISession(); Remove Comment
        //public static string cBaseUrl, cEwbByIRN, cCancelEwbUrl, cAuthUrl; Remove Comment

        public string GetCmdArgs()
        {
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;            
            try
            {
                int iCtr = 0;                
                foreach (string arg in Args)
                {
                    iCtr += 1;
                    if (iCtr == 3)
                    {
                        cCompDBO = arg;
                    }
                    else if (iCtr == 4)
                    {
                        if (FuncType == "EINVOICE")
                        {
                            cInvTble = arg;
                        }
                        else
                        {
                            cEWBTble = arg;
                        }
                    }
                    else if (iCtr == 5)
                    {
                        cPtyGstIn = arg;
                    }
                    else if (iCtr == 6)
                    {
                        cPtyGSPUsr = arg;
                    }
                    else if (iCtr == 7)
                    {
                        cPtyGSPPwd = arg;
                    }
                    else if (iCtr == 8)
                    {
                        cASPName = arg;
                    }
                    else if (iCtr == 9)
                    {
                        cASPUsr = arg;
                    }
                    else if (iCtr == 10)
                    {
                        cASPPwd = arg;
                    }
                    else if (iCtr == 11)
                    {
                        cASPURL = arg;
                        cASPURL = "https://einvapi.charteredinfo.com/v1.03"; 
                    }
                    else if (iCtr == 12)
                    {
                        cSearchGSTIN = arg;
                    }
                }

            } 
            catch (Exception ex)
            {
                MessageBox.Show("Command Line Loading Error : " + ex.Message);
            }            
            if (string.IsNullOrEmpty(cASPName))
            {
                MessageBox.Show("Running In Developement mode.");
                cASPName = "TaxPro_Production";
                cASPUsr = "1613471119";
                cASPPwd = "Dhanchhaya@123";

                //cPtyGstIn = "24AABCR1217N1ZI";
                //cPtyGSPUsr = "rawalgujar_API_GUJ";
                //cPtyGSPPwd = "Sumit@Gujarat";

                cPtyGstIn = "24AAFCL0310G1Z7"; 
                cPtyGSPUsr = "AAFCL0310G_API_501";
                cPtyGSPPwd = "Equal@01042011";


                SharedVar.cAuthUrl = "https://einvapi.charteredinfo.com/eivital/v1.04";
                SharedVar.cBaseUrl = "https://einvapi.charteredinfo.com/eicore/v1.03";                                      
                SharedVar.cEwbByIRN = "https://einvapi.charteredinfo.com/eiewb/v1.03";                                       
                SharedVar.cCancelEwbUrl = "https://einvapi.charteredinfo.com/v1.03";                                         
            }
            return "";
        }

        [STAThread]
        public static void Main()
        {            
            Args = Environment.GetCommandLineArgs();            
            int iCtr = 0;
            foreach (string arg in Args)
            {
                iCtr += 1;
                if (iCtr == 2)
                {                    
                    FuncType = arg;
                }
            }            
            //FuncType = "EINVOICE" 
            System.Windows.Forms.Form formToShow;
            
            if (FuncType == "SEARCHGSTIN")
            {
                MessageBox.Show("SEARCHGSTIN");
                formToShow = new frmGetGSTINDet();
            }
            else if (FuncType == "EINVOICE")
            {
                SharedVar.cAuthUrl = "https://einvapi.charteredinfo.com/eivital/v1.04";
                SharedVar.cBaseUrl = "https://einvapi.charteredinfo.com/eicore/v1.03";
                SharedVar.cEwbByIRN = "https://einvapi.charteredinfo.com/eiewb/v1.03";
                SharedVar.cCancelEwbUrl = "https://einvapi.charteredinfo.com/v1.03";
                formToShow = new frmEInvoice();                
            }
            else if (FuncType == "EWAYBILL")
            {
                //MessageBox.Show("EWayBill");
                formToShow = new frmEWayBill();                
            }
            else
            {
                formToShow = new frmEInvoice();
            }
            if (!System.IO.Directory.Exists(Application.StartupPath + @"\Errors\"))
            {                
                System.IO.Directory.CreateDirectory(Application.StartupPath + @"\Errors\");
            }

            if (!System.IO.Directory.Exists(Application.StartupPath + @"\QRCode\"))
            {                
                System.IO.Directory.CreateDirectory(Application.StartupPath + @"\QRCode\");
            }

            FileStream fs;            
            if (FuncType == "EINVOICE")
            {
                fs = new FileStream(Application.StartupPath + @"\Errors\EInvErrlog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            else
            {
                fs = new FileStream(Application.StartupPath + @"\Errors\EWBErrlog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }

            StreamWriter s = new StreamWriter(fs);
            s.Close();
            fs.Close();
            formToShow.ShowDialog();

            //Comment part
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(formToShow); 
            //Application.Run(new frmEWayBill());
        }

        public string LoadTokenDetail(string pFuncType)
        {           
            if (nDebug == 0)
            {
                MessageBox.Show("In Load Token Detail");
            }

            try
            {
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter(); ;
                DataSet ds = new DataSet();
                int nRecCount;
                string strSelect;
                strSelect = " Select  * from " + cCompDBO + ".dbo.EWBTokenDet where ECCompanyId = '" + pFuncType + "'";
                if (nDebug == 0)
                {
                    MessageBox.Show(strSelect);
                }

                try
                {
                    command = new SqlCommand(strSelect, sqlCnn);
                    adapter.SelectCommand = command;
                    adapter.Fill(ds, "GETTKN");
                    adapter.Dispose();
                    command.Dispose();
                    nRecCount = ds.Tables[0].Rows.Count;
                    if (nRecCount == 0)
                    {
                        if (pFuncType == "EWAYBILL")
                        {
                            MessageBox.Show("No EWB Token Entry Found.");
                            if (nDebug == 0)
                            {
                                MessageBox.Show("EWB");
                            }

                            EwbSession.EwbApiLoginDetails.EwbTokenExp = null;
                            EwbSession.EwbApiLoginDetails.EwbAppKey = "";
                            EwbSession.EwbApiLoginDetails.EwbAuthToken = "";
                            EwbSession.EwbApiLoginDetails.EwbSEK = "";
                            if (nDebug == 0)
                            {
                                MessageBox.Show("Load Token Detail: " + JsonConvert.SerializeObject(EwbSession));
                            }
                        }
                        else if (pFuncType == "EINVOICE")
                        {
                            MessageBox.Show("No IRN Token Entry Found.");
                            if (nDebug == 0)
                            {
                                MessageBox.Show("IRN");
                            }

                            eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp = null;
                            eInvSession.eInvApiLoginDetails.AppKey = "";
                            eInvSession.eInvApiLoginDetails.AuthToken = "";
                            eInvSession.eInvApiLoginDetails.Sek = "";
                        }
                        //SetTokenDetail() ' started 25 / 08 / 21
                    }
                    else
                    {
                        if (nDebug == 0)
                        {
                            MessageBox.Show("Token Found");
                        }
                        if (pFuncType == "EWAYBILL")
                        {
                            EwbSession.EwbApiLoginDetails.EwbAppKey = ds.Tables["GETTKN"].Rows[0]["AppKey"].ToString().Trim();
                            EwbSession.EwbApiLoginDetails.EwbAuthToken = ds.Tables["GETTKN"].Rows[0]["EwbAuthToken"].ToString().Trim();
                            EwbSession.EwbApiLoginDetails.EwbSEK = ds.Tables["GETTKN"].Rows[0]["EwbSEK"].ToString().Trim();
                            EwbSession.EwbApiLoginDetails.EwbTokenExp = Convert.ToDateTime(ds.Tables["GETTKN"].Rows[0]["EwbTokenExp"]);
                        }
                        else if (pFuncType == "EINVOICE")
                        {
                            eInvSession.eInvApiLoginDetails.AppKey = ds.Tables["GETTKN"].Rows[0]["AppKey"].ToString().Trim();
                            eInvSession.eInvApiLoginDetails.AuthToken = ds.Tables["GETTKN"].Rows[0]["EwbAuthToken"].ToString().Trim();
                            eInvSession.eInvApiLoginDetails.Sek = ds.Tables["GETTKN"].Rows[0]["EwbSEK"].ToString().Trim();
                            eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp = Convert.ToDateTime(ds.Tables["GETTKN"].Rows[0]["EwbTokenExp"]);
                        }
                        if (nDebug == 0)
                        {
                            MessageBox.Show("Saved Token Assigned To Session");
                        }
                    }

                    if (pFuncType == "EWAYBILL")
                    {
                        EwbSession.RefreshAuthTokenCompleted += SetTokenDetailEWB;
                    }
                    else if (pFuncType == "EINVOICE")
                    {
                        eInvSession.RefreshAuthTokenCompleted += SetTokenDetailIRN;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Token Load Error : " + ex.Message);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Token Load Error 2 : " + ex.Message);
            }
            return "";
        }

        public void SetTokenDetailEWB(object sender, EventArgs e)
        {

            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter(); ;
            DataSet ds = new DataSet();
            int nRecCount;
            string strSelect;

            try
            {
                strSelect = " Select  * from " + cCompDBO + ".dbo.EWBTokenDet where ECCompanyId = 'EWAYBILL' ";
                command = new SqlCommand(strSelect, sqlCnn);
                adapter.SelectCommand = command;
                adapter.Fill(ds, "SETTKN");
                adapter.Dispose();
                command.Dispose();

                nRecCount = ds.Tables[0].Rows.Count;

                if (nDebug == 0)
                {
                    MessageBox.Show(cCompDBO);
                    MessageBox.Show(strSelect);
                    MessageBox.Show(nRecCount.ToString());
                }

                if (nRecCount == 0)
                {
                    if (nDebug == 0)
                    {
                        MessageBox.Show("Set Token Detail : No token Found in Database -- EWAYBILL");
                        MessageBox.Show(JsonConvert.SerializeObject(EwbSession));
                    }                    
                    strSelect = " Insert into " + cCompDBO + ".dbo.EWBTokenDet (APPKEY, EWBAUTHTOKEN, EWBSEK, EWBTOKENEXP, ECCompanyId )" +
                            " VALUES ('" + EwbSession.EwbApiLoginDetails.EwbAppKey + "','" + EwbSession.EwbApiLoginDetails.EwbAuthToken + "','" + EwbSession.EwbApiLoginDetails.EwbSEK + "','" + EwbSession.EwbApiLoginDetails.EwbTokenExp + "','EWAYBILL')";

                    if (nDebug == 0)
                    {
                        MessageBox.Show(strSelect);
                    }

                    command = new SqlCommand(strSelect, sqlCnn);
                    command.ExecuteNonQueryAsync();
                }
                else
                {
                    if (nDebug == 0)
                    {
                        MessageBox.Show("EWAYBILL Token found " + JsonConvert.SerializeObject(EwbSession));
                    }

                    strSelect = "Update " + cCompDBO + ".dbo.EWBTokenDet " +
                                " set APPKEY = '" + EwbSession.EwbApiLoginDetails.EwbAppKey + "'" +
                                ", EWBAUTHTOKEN = '" + EwbSession.EwbApiLoginDetails.EwbAuthToken + "'" +
                                ", EWBSEK = '" + EwbSession.EwbApiLoginDetails.EwbSEK + "'" +
                                ", EWBTOKENEXP = '" + EwbSession.EwbApiLoginDetails.EwbTokenExp.ToString() + "'" +
                                " Where ECCompanyId = 'EWAYBILL' ";

                    if (nDebug == 0)
                    {
                        MessageBox.Show(strSelect);
                    }

                    command = new SqlCommand(strSelect, sqlCnn);
                    command.ExecuteNonQueryAsync();

                    if (nDebug == 0)
                    {
                        MessageBox.Show("EWAYBILL Set Token Detail : Token Updation.");
                    }
                }
                MessageBox.Show("EWAYBILL Token updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("EWAYBILL Token Updation Error : " + ex.Message);
            }
        }

        public void SetTokenDetailEWBFrmIRN()
        {
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter(); ;
            DataSet ds = new DataSet();
            int nRecCount;
            string strSelect;

            try
            {
                strSelect = " Select  * from " + cCompDBO + ".dbo.EWBTokenDet where ECCompanyId = 'EWAYBILL' ";
                command = new SqlCommand(strSelect, sqlCnn);
                adapter.SelectCommand = command;
                adapter.Fill(ds, "SETTKN");
                adapter.Dispose();
                command.Dispose();

                nRecCount = ds.Tables[0].Rows.Count;
                if (nDebug == 0)
                {
                    MessageBox.Show(cCompDBO);
                    MessageBox.Show(strSelect);
                    MessageBox.Show(nRecCount.ToString());
                }

                if (nRecCount == 0)
                {
                    if (nDebug == 0)
                    {
                        MessageBox.Show("Set Token Detail : No token Found in Database -- EWAYBILL from IRN");
                        MessageBox.Show(JsonConvert.SerializeObject(EwbSession));
                    }

                    strSelect = " Insert into " + cCompDBO + ".dbo.EWBTokenDet (APPKEY, EWBAUTHTOKEN, EWBSEK, EWBTOKENEXP, ECCompanyId )" +
                            " VALUES ('" + eInvSession.eInvApiLoginDetails.AppKey + "','" + eInvSession.eInvApiLoginDetails.AuthToken + "','" + eInvSession.eInvApiLoginDetails.Sek + "','" + eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp + "','EWAYBILL')";

                    if (nDebug == 0)
                    {
                        MessageBox.Show(strSelect);
                    }

                    command = new SqlCommand(strSelect, sqlCnn);
                    command.ExecuteNonQueryAsync();
                }
                else
                {
                    if (nDebug == 0)
                    {
                        MessageBox.Show("EWAYBILL from IRN Token found " + JsonConvert.SerializeObject(EwbSession));
                    }

                    strSelect = "Update " + cCompDBO + ".dbo.EWBTokenDet " +
                                " set APPKEY = '" + eInvSession.eInvApiLoginDetails.AppKey + "'" +
                                ", EWBAUTHTOKEN = '" + eInvSession.eInvApiLoginDetails.AuthToken + "'" +
                                ", EWBSEK = '" + eInvSession.eInvApiLoginDetails.Sek + "'" +
                                ", EWBTOKENEXP = '" + eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp.ToString() + "'" +
                                " Where ECCompanyId = 'EWAYBILL' ";

                    if (nDebug == 0)
                    {
                        MessageBox.Show(strSelect);
                    }

                    command = new SqlCommand(strSelect, sqlCnn);
                    command.ExecuteNonQueryAsync();

                    if (nDebug == 0)
                    {
                        MessageBox.Show("EWAYBILL from IRN Set Token Detail : Token Updation.");
                    }
                }
                MessageBox.Show("EWAYBILL from IRN Token updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("EWAYBILL from IRN Token Updation Error : " + ex.Message);
            }
        }

        public void SetTokenDetailIRN(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter(); ;
            DataSet ds = new DataSet();
            int nRecCount;
            string strSelect;

            try
            {
                SetTokenDetailEWBFrmIRN(); // it will update EWaybill token auto on refreshing or generating new token for IRN

                strSelect = " Select  * from " + cCompDBO + ".dbo.EWBTokenDet where ECCompanyId = 'EINVOICE'";
                command = new SqlCommand(strSelect, sqlCnn);
                adapter.SelectCommand = command;
                adapter.Fill(ds, "SETTKN");
                adapter.Dispose();
                command.Dispose();

                nRecCount = ds.Tables[0].Rows.Count;
                if (nDebug == 0)
                {
                    MessageBox.Show(cCompDBO);
                    MessageBox.Show(strSelect);
                    MessageBox.Show(nRecCount.ToString());
                }

                if (nRecCount == 0)
                {
                    if (nDebug == 0)
                    {
                        MessageBox.Show("IRN Set Token Detail : No token Found in Database");
                        MessageBox.Show(JsonConvert.SerializeObject(eInvSession));
                    }

                    strSelect = " Insert into " + cCompDBO + ".dbo.EWBTokenDet (APPKEY, EWBAUTHTOKEN, EWBSEK, EWBTOKENEXP, ECCompanyId )" +
                                " VALUES ('" + eInvSession.eInvApiLoginDetails.AppKey + "','" + eInvSession.eInvApiLoginDetails.AuthToken + "','" + eInvSession.eInvApiLoginDetails.Sek + "','" + eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp + "','EINVOICE')";

                    if (nDebug == 0)
                    {
                        MessageBox.Show(strSelect);
                    }

                    command = new SqlCommand(strSelect, sqlCnn);
                    command.ExecuteNonQueryAsync();

                }
                else
                {
                    MessageBox.Show("Token found");
                    strSelect = "Update " + cCompDBO + ".dbo.EWBTokenDet " +
                            " set APPKEY = '" + eInvSession.eInvApiLoginDetails.AppKey + "'" +
                            ", EWBAUTHTOKEN = '" + eInvSession.eInvApiLoginDetails.AuthToken + "'" +
                            ", EWBSEK = '" + eInvSession.eInvApiLoginDetails.Sek + "'" +
                            ", EWBTOKENEXP = '" + eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp.ToString() + "'" +
                            " Where ECCompanyId = 'EINVOICE'";
                    if (nDebug == 0)
                    {
                        MessageBox.Show(strSelect);

                    }
                    command = new SqlCommand(strSelect, sqlCnn);
                    command.ExecuteNonQueryAsync();

                    if (nDebug == 0)
                    {
                        MessageBox.Show("IRN Set Token Detail : Token Updation.");
                    }
                }
                MessageBox.Show("IRN Token updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("IRN Token Updation Error : " + ex.Message);
            }
        }

        public EWBSession MakeEWayBillSession()
        {
            if (nDebug == 0)
            {
                MessageBox.Show("In Making EWay Bill Session");
            }
            //MessageBox.Show("In Making EWay Bill Session");

            EwbSession = new EWBSession(false, false);

            EwbSession.EwbApiSetting = new EWBAPISetting();
            if (nDebug == 0)
            {
                MessageBox.Show("Making EWB Session");
            }
            EwbSession.EwbApiSetting.GSPName = cASPName;
            EwbSession.EwbApiSetting.AspUserId = cASPUsr;
            EwbSession.EwbApiSetting.AspPassword = cASPPwd;
            EwbSession.EwbApiSetting.BaseUrl = cASPURL;

            if (nDebug == 0)
            {
                MessageBox.Show("EWB Session " + JsonConvert.SerializeObject(EwbSession));
            }

            EwbSession.EwbApiSetting.EWBClientId = "";
            EwbSession.EwbApiSetting.EWBClientSecret = "";
            EwbSession.EwbApiSetting.EWBGSPUserID = "";

            if (nDebug == 0)
            {
                MessageBox.Show("EWB Session 2 " + JsonConvert.SerializeObject(EwbSession));
            }

            if (nDebug == 0)
            {
                MessageBox.Show("EWB Party GSTIN Details " + cPtyGstIn + Environment.NewLine + cPtyGSPUsr + Environment.NewLine + cPtyGSPPwd);
            }

            EwbSession.EwbApiLoginDetails = new EWBAPILoginDetails();
            EwbSession.EwbApiLoginDetails.EwbGstin = cPtyGstIn;
            EwbSession.EwbApiLoginDetails.EwbUserID = cPtyGSPUsr;
            EwbSession.EwbApiLoginDetails.EwbPassword = cPtyGSPPwd;
            EwbSession.EwbApiLoginDetails.IRPUrl = 1;
            EwbSession.EwbApiLoginDetails.IRP = 1;
            LoadTokenDetail("EWAYBILL");

            if (nDebug == 0)
            {
                MessageBox.Show("EWB Session 3 " + JsonConvert.SerializeObject(EwbSession));
            }
            return EwbSession;
        }

        public eInvoiceSession MakeEInvoiceSession()
        {

            if (nDebug == 0)
            {
                MessageBox.Show("In Making EINV Session");

            }

            eInvSession = new eInvoiceSession(false, false);

            eInvSession.eInvApiSetting = new eInvoiceAPISetting();
            if (nDebug == 0)
            {
                MessageBox.Show("Making E-Invoice Session");
            }            

            eInvSession.eInvApiSetting.GSPName = cASPName;
            eInvSession.eInvApiSetting.AspUserId = cASPUsr;
            eInvSession.eInvApiSetting.AspPassword = cASPPwd;
            eInvSession.eInvApiSetting.client_id = "";
            eInvSession.eInvApiSetting.client_secret = "";
            eInvSession.eInvApiSetting.AuthUrl = SharedVar.cAuthUrl;
            eInvSession.eInvApiSetting.BaseUrl = SharedVar.cBaseUrl;
            eInvSession.eInvApiSetting.EwbByIRN = SharedVar.cEwbByIRN;
            eInvSession.eInvApiSetting.CancelEwbUrl = SharedVar.cCancelEwbUrl;

            if (nDebug == 0)
            {
                MessageBox.Show("EINV Session 2 " + JsonConvert.SerializeObject(eInvSession));
            }


            if (nDebug == 0)
            {
                MessageBox.Show("EINV Party GSTIN Details " + cPtyGstIn + Environment.NewLine + cPtyGSPUsr + Environment.NewLine + cPtyGSPPwd);
            }


            eInvSession.eInvApiLoginDetails = new eInvoiceAPILoginDetails();
            eInvSession.eInvApiLoginDetails.UserName = cPtyGSPUsr;
            eInvSession.eInvApiLoginDetails.Password = cPtyGSPPwd;
            eInvSession.eInvApiLoginDetails.GSTIN = cPtyGstIn;
            eInvSession.eInvApiLoginDetails.IRPUrl = 1;
            eInvSession.eInvApiLoginDetails.IRP = 1;

            LoadTokenDetail("EINVOICE");

            if (nDebug == 0)
            {
                MessageBox.Show("EINV Session 3 " + JsonConvert.SerializeObject(eInvSession));
            }
            return eInvSession;
        }

        
        public int SetIniValue(string Section, string Keyname, string lpString, string IniFilePath)
        {
            return WritePrivateProfileString(Section, Keyname, lpString, Application.StartupPath + @"\" + IniFilePath);
        }

        public string GetIniValue(string Section, string Keyname, string IniFilePathName)
        {
            //string inBuf = "                                   ";
            //GetPrivateProfileString(Section, Keyname, "", inBuf, 80, Application.StartupPath + @"\" + IniFilePathName);
            //return inBuf;
            if (!System.IO.File.Exists(Application.StartupPath + @"\" + IniFilePathName))
            {
                return null;
            }
           
            StringBuilder sb = new StringBuilder(4096);
            int n = GetPrivateProfileString(Section, Keyname, "", sb, 4096, Application.StartupPath + @"\" + IniFilePathName);
            if (n < 1) return string.Empty;
            return sb.ToString();

            //if (!System.IO.File.Exists(IniFilePathName))
            //{
            //    return null;
            //}

            //string sectionHeader = $"[{Section}]";
            //return System.IO.File.ReadLines(IniFilePathName)
            //    .SkipWhile(l => !l.Equals(sectionHeader, StringComparison.InvariantCultureIgnoreCase))
            //    .Skip(1) // skip header
            //    .TakeWhile(l => l.Contains('='))
            //    .Select(GetMatchingKeyValueOrNull)
            //    .FirstOrDefault(value => value != null);

            //string GetMatchingKeyValueOrNull(string line)
            //{
            //    string[] arr = line.Split('=');
            //    return arr.Length > 1 && arr[0].Trim().Equals(Keyname, StringComparison.InvariantCultureIgnoreCase)
            //        ? arr[1].Trim() : null;
            //}
        }

        public bool MakeSQLConnObj()
        {
            cSQLServer = GetIniValue("DATABASE : YEARS", "SERVER", "Settings.ini");
            cSQLPwd = GetIniValue("DATABASE : YEARS", "PASSWORD", "Settings.ini").ToString().Trim();
            cYrsDBO = GetIniValue("DATABASE : YEARS", "DATABASE", "Settings.ini").ToString().Trim();
            
            string connetionString;
            connetionString = "Persist Security Info=False;User ID=sa;Password=" + cSQLPwd + ";Data Source=" + cSQLServer + ";Initial Catalog=" + cCompDBO;
            if (nDebug == 0)
            {
                MessageBox.Show(connetionString);
            }
            sqlCnn = new SqlConnection(connetionString);
            sqlCnn.Open();
            if (sqlCnn.State == ConnectionState.Open)
            {
                if (nDebug == 0)
                {
                    MessageBox.Show("SQL Connection OK");
                }
                return true;
            }
            else
            {
                if (nDebug == 0)
                {
                    MessageBox.Show("SQL Connection Error");
                }
                return false;
            }
        }
        public string UnChkAll(DataGridView oObj)
        {
            nChkRow = 0;
            int nRowIndex;
            int nTotRow = oObj.RowCount ;
            for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
            {
                oObj.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.White;
                oObj.Rows[nRowIndex].Cells[0].Value = false;
            }
            return "";
        }

        public bool ChkAll(DataGridView oObj, bool eInv)
        {
            nChkRow = 0;
            int nRowIndex;
            int nTotRow = oObj.RowCount ;            
            bool Retval = true;
            for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
            {
                oObj.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                oObj.Rows[nRowIndex].Cells[0].Value = true;
                nChkRow += 1;
            }
            Retval = true;
            return Retval;
        }

        public string RetStateCode(string cState)
        {
            string cCode;
            cState = cState.ToUpper().Trim();
            if (cState == "JAMMU AND KASHMIR" || cState == "JAMMU & KASHMIR" || cState == "JAMMU & KASMIR" || cState == "JAMMU AND KASMIR" )
            {
                cCode = "1";
            }            
            else if (cState == "HIMACHAL PRADESH")
            {
                cCode = "2";
            }                
            else if (cState == "PUNJAB")
            {
                cCode = "3";
            }
            else if (cState == "CHANDIGARH" )
            {
                cCode = "4";
            }
            else if (cState == "UTTARAKHAND")
            {
                cCode = "5";
            }
            else if (cState == "HARYANA" )
            {
                cCode = "6";
            }
            else if (cState == "DELHI" )
            {
                cCode = "7";
            }
            else if( cState == "RAJASTHAN" )
            {
                cCode = "8";
            }
            else if (cState == "UTTAR PRADESH" )
            {
                cCode = "9";
            }
            else if (cState == "BIHAR" )
            {
                cCode = "10";
            }
            else if (cState == "SIKKIM")
            {
                cCode = "11";
            }
            else if (cState == "ARUNACHAL PRADESH" )
            {
                cCode = "12";
            }
            else if( cState == "NAGALAND" )
            {
                cCode = "13";
            }
            else if (cState == "MANIPUR" )
            {
                cCode = "14";
            }
            else if (cState == "MIZORAM")
            {
                cCode = "15";
            }
            else if (cState == "TRIPURA" )
            {
                cCode = "16";
            }
            else if (cState == "MEGHALAYA" )
            {
                cCode = "17";
            }
            else if (cState == "ASSAM" )
            {
                cCode = "18";
            }
            else if (cState == "WEST BENGAL" )
            {
                cCode = "19";
            }
            else if (cState == "JHARKHAND")
            {
                cCode = "20";
            }
            else if (cState == "ORISSA" )
            {
                cCode = "21";
            }
            else if (cState == "CHHATTISGARH" )
            {
                cCode = "22";
            }
            else if (cState == "MADHYA PRADESH")
            {
                cCode = "23";
            }
            else if (cState == "GUJARAT")
            {
                cCode = "24";
            }
            else if (cState == "DAMAN & DIU" )
            {
                cCode = "26";
            }
            else if (cState == "DADRA & NAGAR HAVELI" )
            {
                cCode = "26";
            }    
            else if (cState == "MAHARASHTRA")
            {
                cCode = "27";
            }                                
            else if (cState == "KARNATAKA" )
            {
                cCode = "29";
            }
            else if (cState == "GOA" )
            {
                cCode = "30";
            }
            else if (cState == "LAKSHADWEEP" )
            {
                cCode = "31";
            }
            else if (cState == "KERALA" )
            {
                cCode = "32";
            }
            else if (cState == "TAMIL NADU" )
            {
                cCode = "33";
            }
            else if (cState == "PUDUCHERRY" )
            {
                cCode = "34";
            }
            else if (cState == "ANDAMAN & NICOBAR ISLANDS" )
            {
                cCode = "35";
            }
            else if (cState == "TELANGANA" || cState == "TELENGANA" )
            {
                cCode = "36";
            }
            else if (cState == "ANDHRA PRADESH")
            {
                cCode = "37";
            }                
            else if (cState == "DEXP" )
            {
                cCode = "96";
            }
            else if (cState == "OTHER COUNTRIES")
            {
                cCode = "99";
            }
            else
            {
                cCode = "0";
            }            
            if (nDebug == 0)
            {
                MessageBox.Show(cState);
                MessageBox.Show(cCode);
            }
            return cCode;
        }
        public string WriteToErrorLog(string msg)
        {
            FileStream fs1;
            if (FuncType == "EINVOICE")
            {
                fs1 = new FileStream(Application.StartupPath + @"\Errors\EInvErrlog.txt", FileMode.Append, FileAccess.Write);                
            }
            else
            {
                fs1 = new FileStream(Application.StartupPath + @"\Errors\EWBErrlog.txt", FileMode.Append, FileAccess.Write);
            }
            StreamWriter s1 = new StreamWriter(fs1);
            s1.Write(msg + "\n");
            s1.Close();
            fs1.Close();
            return "";

        }

        public string GetAuthToken(string cMode )
        {
            if (nDebug == 0)
            {
                MessageBox.Show("In Auth Token Generation process.");
            }
            string strMsg;

            strMsg = "Clicking this button will generate Authorisation token for " + cMode + "." + Environment.NewLine + Environment.NewLine +
                    "Unnecessarily clicking this button will be extra charged for API consumption." + Environment.NewLine + Environment.NewLine +
                    "Still you want to generate Authorisation Token?";

            DialogResult result = MessageBox.Show(strMsg, "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string strSelect;
                    strSelect = " delete from " + cCompDBO + ".dbo.EWBTokenDet where ECCompanyId = '" + cMode + "'";
                    if (nDebug == 0)
                    {
                        MessageBox.Show(strSelect);
                    }

                    try
                    {
                        SqlCommand Command;
                        if (nDebug == 0)
                        {
                            MessageBox.Show(strSelect);
                        }
                        WriteToErrorLog("Authorisation token generation for " + cMode + strSelect);

                        Command = new SqlCommand(strSelect, sqlCnn);
                        Command.ExecuteNonQueryAsync();
                        Command.Dispose();


                        if (cMode == "EWAYBILL")
                        {                            
                            TxnRespWithObjAndInfo<EWBSession> TxnResp = Task.Run(() => EWBAPI.GetAuthTokenAsync(EwbSession)).Result;
                            var cAPIResponse = JsonConvert.SerializeObject(TxnResp.RespObj);

                            if (!TxnResp.IsSuccess)
                            {
                                WriteToErrorLog("EWay Bill Authorised Token failed response. \n" +  cAPIResponse + "\n" + TxnResp.TxnOutcome);
                                MessageBox.Show("Authorisation Token not generated. " + TxnResp.TxnOutcome);

                            }
                            else
                            {
                                EwbSession.EwbApiLoginDetails.EwbTokenExp = TxnResp.RespObj.EwbApiLoginDetails.EwbTokenExp;
                                EwbSession.EwbApiLoginDetails.EwbAppKey = TxnResp.RespObj.EwbApiLoginDetails.EwbAppKey.ToString().Trim();
                                EwbSession.EwbApiLoginDetails.EwbAuthToken = TxnResp.RespObj.EwbApiLoginDetails.EwbAuthToken.ToString().Trim();
                                EwbSession.EwbApiLoginDetails.EwbSEK = TxnResp.RespObj.EwbApiLoginDetails.EwbSEK.ToString().Trim();
                                object eventSender = null; // replace with actual sender
                                EventArgs eventArgs = new EventArgs();
                                SetTokenDetailEWB(eventSender, eventArgs);
                                MessageBox.Show("Auth Token generation and Setting Token Detail to EWB Session and database successful.");
                            }
                            if (nDebug == 0 )
                            {
                                MessageBox.Show("Explicit EWB Token " + JsonConvert.SerializeObject(EwbSession));
                            }
                        }
                        else
                        {                            
                            TaxProEInvoice.API.TxnRespWithObj<eInvoiceSession> TxnRespIRN = Task.Run(() => eInvoiceAPI.GetAuthTokenAsync(eInvSession)).Result;
                            var cAPIResponseIRN = JsonConvert.SerializeObject(TxnRespIRN.RespObj);

                            if (!TxnRespIRN.IsSuccess )
                            {
                                WriteToErrorLog("EINV Authorised Token failed response. /n" + cAPIResponseIRN + "/n" + TxnRespIRN.TxnOutcome);
                                MessageBox.Show("Authorisation Token not generated. " + TxnRespIRN.TxnOutcome);
                            }
                            else
                            {
                                EwbSession.EwbApiLoginDetails.EwbTokenExp = TxnRespIRN.RespObj.eInvApiLoginDetails.E_InvoiceTokenExp;
                                EwbSession.EwbApiLoginDetails.EwbAppKey = TxnRespIRN.RespObj.eInvApiLoginDetails.AppKey.ToString().Trim();
                                EwbSession.EwbApiLoginDetails.EwbAuthToken = TxnRespIRN.RespObj.eInvApiLoginDetails.AuthToken.ToString().Trim();
                                EwbSession.EwbApiLoginDetails.EwbSEK = TxnRespIRN.RespObj.eInvApiLoginDetails.Sek.ToString().Trim();
                                MessageBox.Show("Auth Token generation and Setting Token Detail to EWB Session.");

                                eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp = TxnRespIRN.RespObj.eInvApiLoginDetails.E_InvoiceTokenExp;
                                eInvSession.eInvApiLoginDetails.AppKey = TxnRespIRN.RespObj.eInvApiLoginDetails.AppKey.ToString().Trim();
                                eInvSession.eInvApiLoginDetails.AuthToken = TxnRespIRN.RespObj.eInvApiLoginDetails.AuthToken.ToString().Trim();
                                eInvSession.eInvApiLoginDetails.Sek = TxnRespIRN.RespObj.eInvApiLoginDetails.Sek.ToString().Trim();
                                if (nDebug == 0 )
                                {
                                    MessageBox.Show("Setting Token Detail to EINV Session.");
                                }
                                object eventSender = null; // replace with actual sender
                                EventArgs eventArgs = new EventArgs();
                                SetTokenDetailIRN(eventSender, eventArgs);
                                MessageBox.Show("Auth Token generation and Setting Token Detail to IRN Session and database successful.");
                            }
                            if (nDebug == 0)
                            {
                                MessageBox.Show("Explicit IRN Token " + JsonConvert.SerializeObject(eInvSession));
                            }                                                
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Token Load Error : " + ex.Message);
                    }
                }
                catch(Exception  ex)
                {
                    MessageBox.Show("Token Load Error 2 : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Token Generation Process Aborted.");
            }
            return "";
        }

        public string ResetGrid(DataGridView Grid, System.Windows.Forms.TextBox ErroObj)
        {
            nChkRow = 0;
            int nRowIndex;
            var nTotRow = Grid.RowCount - 1;
            for (nRowIndex = 0; nRowIndex < nTotRow; nRowIndex++)
            {
                Grid.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.White;
                Grid.Rows[nRowIndex].DefaultCellStyle.ForeColor = Color.Black;
                Grid.Rows[nRowIndex].Cells["ErrorList"].Value = "";
            }
            ErroObj.Text = "";
            return "";
        }
    }    
}