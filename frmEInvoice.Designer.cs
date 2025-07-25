using System.Runtime.InteropServices;

namespace EWayBillTool
{
    partial class frmEInvoice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                    components.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEInvoice));
            this.Server = new System.Windows.Forms.ComboBox();
            this.BtnIRNTKN = new System.Windows.Forms.Button();
            this.QRImg = new System.Windows.Forms.PictureBox();
            this.txtInvDet = new System.Windows.Forms.TextBox();
            this.txtErrMsg = new System.Windows.Forms.TextBox();
            this.oEInvStatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.EInvStatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.EInvBillDetGrid = new System.Windows.Forms.DataGridView();
            this.txtkm = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.BtnEWBCancel = new System.Windows.Forms.Button();
            this.TxtEWBVeh = new System.Windows.Forms.TextBox();
            this.lblEWBVehNo = new System.Windows.Forms.Label();
            this.BtnEWBDetPrn = new System.Windows.Forms.Button();
            this.BtnEWBPrint = new System.Windows.Forms.Button();
            this.BtnEWBGen = new System.Windows.Forms.Button();
            this.EWBGrpBox = new System.Windows.Forms.GroupBox();
            this.QRCodeBtn = new System.Windows.Forms.Button();
            this.BtnGetDetail = new System.Windows.Forms.Button();
            this.BtnEInvGen = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.EInvGrpBox = new System.Windows.Forms.GroupBox();
            this.BtnDebug = new System.Windows.Forms.Button();
            this.btnUnChkAll = new System.Windows.Forms.Button();
            this.btnChkAll = new System.Windows.Forms.Button();
            this.DispDetail = new System.Windows.Forms.CheckBox();
            this.SellerDetail = new System.Windows.Forms.CheckBox();
            this.ShipDetail = new System.Windows.Forms.CheckBox();
            this.OthDetail = new System.Windows.Forms.CheckBox();
            this.txtEWBCanRem = new System.Windows.Forms.TextBox();
            this.lblEWBCanRem = new System.Windows.Forms.Label();
            this.lblEWBCanRes = new System.Windows.Forms.Label();
            this.txtEWBCanRes = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.QRImg)).BeginInit();
            this.EInvStatusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EInvBillDetGrid)).BeginInit();
            this.EWBGrpBox.SuspendLayout();
            this.EInvGrpBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Server
            // 
            this.Server.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Server.FormattingEnabled = true;
            this.Server.Items.AddRange(new object[] {
            "PRIMARY",
            "BACKUP1",
            "BACKUP2"});
            this.Server.Location = new System.Drawing.Point(810, 64);
            this.Server.Name = "Server";
            this.Server.Size = new System.Drawing.Size(84, 21);
            this.Server.TabIndex = 58;
            this.Server.Text = "PRIMARY";
            // 
            // BtnIRNTKN
            // 
            this.BtnIRNTKN.Location = new System.Drawing.Point(899, 57);
            this.BtnIRNTKN.Name = "BtnIRNTKN";
            this.BtnIRNTKN.Size = new System.Drawing.Size(37, 31);
            this.BtnIRNTKN.TabIndex = 56;
            this.BtnIRNTKN.Text = "TKN";
            this.BtnIRNTKN.UseVisualStyleBackColor = true;
            this.BtnIRNTKN.Click += new System.EventHandler(this.BtnIRNTKN_Click);
            // 
            // QRImg
            // 
            this.QRImg.Location = new System.Drawing.Point(435, 189);
            this.QRImg.Name = "QRImg";
            this.QRImg.Size = new System.Drawing.Size(123, 131);
            this.QRImg.TabIndex = 55;
            this.QRImg.TabStop = false;
            this.QRImg.Visible = false;
            // 
            // txtInvDet
            // 
            this.txtInvDet.BackColor = System.Drawing.Color.White;
            this.txtInvDet.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvDet.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtInvDet.Location = new System.Drawing.Point(7, 451);
            this.txtInvDet.Multiline = true;
            this.txtInvDet.Name = "txtInvDet";
            this.txtInvDet.ReadOnly = true;
            this.txtInvDet.Size = new System.Drawing.Size(1031, 41);
            this.txtInvDet.TabIndex = 54;
            // 
            // txtErrMsg
            // 
            this.txtErrMsg.BackColor = System.Drawing.Color.White;
            this.txtErrMsg.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtErrMsg.ForeColor = System.Drawing.Color.Red;
            this.txtErrMsg.Location = new System.Drawing.Point(7, 393);
            this.txtErrMsg.Multiline = true;
            this.txtErrMsg.Name = "txtErrMsg";
            this.txtErrMsg.ReadOnly = true;
            this.txtErrMsg.Size = new System.Drawing.Size(1031, 53);
            this.txtErrMsg.TabIndex = 53;
            // 
            // oEInvStatusMsg
            // 
            this.oEInvStatusMsg.Name = "oEInvStatusMsg";
            this.oEInvStatusMsg.Size = new System.Drawing.Size(161, 17);
            this.oEInvStatusMsg.Text = "E-Invoice Bill Generation Tool";
            // 
            // EInvStatusStrip1
            // 
            this.EInvStatusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.EInvStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oEInvStatusMsg});
            this.EInvStatusStrip1.Location = new System.Drawing.Point(0, 498);
            this.EInvStatusStrip1.Name = "EInvStatusStrip1";
            this.EInvStatusStrip1.Size = new System.Drawing.Size(1040, 22);
            this.EInvStatusStrip1.TabIndex = 52;
            this.EInvStatusStrip1.Text = "StatusStrip1";
            // 
            // EInvBillDetGrid
            // 
            this.EInvBillDetGrid.AllowUserToAddRows = false;
            this.EInvBillDetGrid.AllowUserToDeleteRows = false;
            this.EInvBillDetGrid.AllowUserToResizeRows = false;
            this.EInvBillDetGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.EInvBillDetGrid.BackgroundColor = System.Drawing.Color.White;
            this.EInvBillDetGrid.CausesValidation = false;
            this.EInvBillDetGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EInvBillDetGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.EInvBillDetGrid.Location = new System.Drawing.Point(7, 95);
            this.EInvBillDetGrid.Name = "EInvBillDetGrid";
            this.EInvBillDetGrid.ReadOnly = true;
            this.EInvBillDetGrid.RowHeadersWidth = 51;
            this.EInvBillDetGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.EInvBillDetGrid.Size = new System.Drawing.Size(1029, 291);
            this.EInvBillDetGrid.TabIndex = 51;
            // 
            // txtkm
            // 
            this.txtkm.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtkm.Location = new System.Drawing.Point(345, 24);
            this.txtkm.Name = "txtkm";
            this.txtkm.Size = new System.Drawing.Size(28, 21);
            this.txtkm.TabIndex = 44;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Label1.Location = new System.Drawing.Point(311, 28);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(29, 13);
            this.Label1.TabIndex = 43;
            this.Label1.Text = "KM:";
            // 
            // BtnEWBCancel
            // 
            this.BtnEWBCancel.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.BtnEWBCancel.Image = ((System.Drawing.Image)(resources.GetObject("BtnEWBCancel.Image")));
            this.BtnEWBCancel.Location = new System.Drawing.Point(392, 20);
            this.BtnEWBCancel.Name = "BtnEWBCancel";
            this.BtnEWBCancel.Size = new System.Drawing.Size(30, 30);
            this.BtnEWBCancel.TabIndex = 40;
            this.BtnEWBCancel.UseVisualStyleBackColor = true;
            this.BtnEWBCancel.Click += new System.EventHandler(this.BtnEWBCancel_Click);
            // 
            // TxtEWBVeh
            // 
            this.TxtEWBVeh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtEWBVeh.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.TxtEWBVeh.Location = new System.Drawing.Point(229, 24);
            this.TxtEWBVeh.Name = "TxtEWBVeh";
            this.TxtEWBVeh.Size = new System.Drawing.Size(76, 21);
            this.TxtEWBVeh.TabIndex = 34;
            // 
            // lblEWBVehNo
            // 
            this.lblEWBVehNo.AutoSize = true;
            this.lblEWBVehNo.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblEWBVehNo.Location = new System.Drawing.Point(115, 27);
            this.lblEWBVehNo.Name = "lblEWBVehNo";
            this.lblEWBVehNo.Size = new System.Drawing.Size(112, 13);
            this.lblEWBVehNo.TabIndex = 33;
            this.lblEWBVehNo.Text = "Vehicle for Part-B:";
            // 
            // BtnEWBDetPrn
            // 
            this.BtnEWBDetPrn.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.BtnEWBDetPrn.Image = ((System.Drawing.Image)(resources.GetObject("BtnEWBDetPrn.Image")));
            this.BtnEWBDetPrn.Location = new System.Drawing.Point(80, 18);
            this.BtnEWBDetPrn.Name = "BtnEWBDetPrn";
            this.BtnEWBDetPrn.Size = new System.Drawing.Size(31, 31);
            this.BtnEWBDetPrn.TabIndex = 32;
            this.BtnEWBDetPrn.UseVisualStyleBackColor = true;
            this.BtnEWBDetPrn.Click += new System.EventHandler(this.BtnEWBDetPrn_Click);
            // 
            // BtnEWBPrint
            // 
            this.BtnEWBPrint.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.BtnEWBPrint.Image = ((System.Drawing.Image)(resources.GetObject("BtnEWBPrint.Image")));
            this.BtnEWBPrint.Location = new System.Drawing.Point(43, 18);
            this.BtnEWBPrint.Name = "BtnEWBPrint";
            this.BtnEWBPrint.Size = new System.Drawing.Size(31, 31);
            this.BtnEWBPrint.TabIndex = 31;
            this.BtnEWBPrint.UseVisualStyleBackColor = true;
            this.BtnEWBPrint.Click += new System.EventHandler(this.BtnEWBPrint_Click);
            // 
            // BtnEWBGen
            // 
            this.BtnEWBGen.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.BtnEWBGen.Image = ((System.Drawing.Image)(resources.GetObject("BtnEWBGen.Image")));
            this.BtnEWBGen.Location = new System.Drawing.Point(6, 18);
            this.BtnEWBGen.Name = "BtnEWBGen";
            this.BtnEWBGen.Size = new System.Drawing.Size(31, 31);
            this.BtnEWBGen.TabIndex = 30;
            this.BtnEWBGen.UseVisualStyleBackColor = false;
            this.BtnEWBGen.Click += new System.EventHandler(this.BtnEWBGen_Click);
            // 
            // EWBGrpBox
            // 
            this.EWBGrpBox.Controls.Add(this.txtkm);
            this.EWBGrpBox.Controls.Add(this.Label1);
            this.EWBGrpBox.Controls.Add(this.BtnEWBCancel);
            this.EWBGrpBox.Controls.Add(this.TxtEWBVeh);
            this.EWBGrpBox.Controls.Add(this.lblEWBVehNo);
            this.EWBGrpBox.Controls.Add(this.BtnEWBDetPrn);
            this.EWBGrpBox.Controls.Add(this.BtnEWBPrint);
            this.EWBGrpBox.Controls.Add(this.BtnEWBGen);
            this.EWBGrpBox.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.EWBGrpBox.Location = new System.Drawing.Point(144, 3);
            this.EWBGrpBox.Name = "EWBGrpBox";
            this.EWBGrpBox.Size = new System.Drawing.Size(888, 54);
            this.EWBGrpBox.TabIndex = 50;
            this.EWBGrpBox.TabStop = false;
            this.EWBGrpBox.Text = "E-Way Bill";
            // 
            // QRCodeBtn
            // 
            this.QRCodeBtn.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.QRCodeBtn.Image = ((System.Drawing.Image)(resources.GetObject("QRCodeBtn.Image")));
            this.QRCodeBtn.Location = new System.Drawing.Point(79, 16);
            this.QRCodeBtn.Name = "QRCodeBtn";
            this.QRCodeBtn.Size = new System.Drawing.Size(31, 31);
            this.QRCodeBtn.TabIndex = 21;
            this.QRCodeBtn.UseVisualStyleBackColor = true;
            this.QRCodeBtn.Click += new System.EventHandler(this.QRCodeBtn_Click);
            // 
            // BtnGetDetail
            // 
            this.BtnGetDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.BtnGetDetail.Image = ((System.Drawing.Image)(resources.GetObject("BtnGetDetail.Image")));
            this.BtnGetDetail.Location = new System.Drawing.Point(43, 16);
            this.BtnGetDetail.Name = "BtnGetDetail";
            this.BtnGetDetail.Size = new System.Drawing.Size(31, 31);
            this.BtnGetDetail.TabIndex = 20;
            this.BtnGetDetail.UseVisualStyleBackColor = true;
            this.BtnGetDetail.Click += new System.EventHandler(this.BtnGetDetail_Click);
            // 
            // BtnEInvGen
            // 
            this.BtnEInvGen.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.BtnEInvGen.Image = ((System.Drawing.Image)(resources.GetObject("BtnEInvGen.Image")));
            this.BtnEInvGen.Location = new System.Drawing.Point(6, 16);
            this.BtnEInvGen.Name = "BtnEInvGen";
            this.BtnEInvGen.Size = new System.Drawing.Size(31, 31);
            this.BtnEInvGen.TabIndex = 18;
            this.BtnEInvGen.UseVisualStyleBackColor = false;
            this.BtnEInvGen.Click += new System.EventHandler(this.BtnEInvGen_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(763, 66);
            this.Label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(44, 13);
            this.Label2.TabIndex = 57;
            this.Label2.Text = "Server :";
            // 
            // EInvGrpBox
            // 
            this.EInvGrpBox.Controls.Add(this.QRCodeBtn);
            this.EInvGrpBox.Controls.Add(this.BtnGetDetail);
            this.EInvGrpBox.Controls.Add(this.BtnEInvGen);
            this.EInvGrpBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.EInvGrpBox.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.EInvGrpBox.Location = new System.Drawing.Point(12, 3);
            this.EInvGrpBox.Name = "EInvGrpBox";
            this.EInvGrpBox.Size = new System.Drawing.Size(128, 53);
            this.EInvGrpBox.TabIndex = 49;
            this.EInvGrpBox.TabStop = false;
            this.EInvGrpBox.Text = "E-Invoice";
            // 
            // BtnDebug
            // 
            this.BtnDebug.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.BtnDebug.Image = ((System.Drawing.Image)(resources.GetObject("BtnDebug.Image")));
            this.BtnDebug.Location = new System.Drawing.Point(1001, 57);
            this.BtnDebug.Name = "BtnDebug";
            this.BtnDebug.Size = new System.Drawing.Size(31, 31);
            this.BtnDebug.TabIndex = 48;
            this.BtnDebug.UseVisualStyleBackColor = true;
            this.BtnDebug.Click += new System.EventHandler(this.BtnDebug_Click);
            // 
            // btnUnChkAll
            // 
            this.btnUnChkAll.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnUnChkAll.Image = ((System.Drawing.Image)(resources.GetObject("btnUnChkAll.Image")));
            this.btnUnChkAll.Location = new System.Drawing.Point(968, 57);
            this.btnUnChkAll.Name = "btnUnChkAll";
            this.btnUnChkAll.Size = new System.Drawing.Size(31, 31);
            this.btnUnChkAll.TabIndex = 47;
            this.btnUnChkAll.UseVisualStyleBackColor = true;
            this.btnUnChkAll.Click += new System.EventHandler(this.BtnUnChkAll_Click);
            // 
            // btnChkAll
            // 
            this.btnChkAll.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnChkAll.Image = ((System.Drawing.Image)(resources.GetObject("btnChkAll.Image")));
            this.btnChkAll.Location = new System.Drawing.Point(935, 57);
            this.btnChkAll.Name = "btnChkAll";
            this.btnChkAll.Size = new System.Drawing.Size(31, 31);
            this.btnChkAll.TabIndex = 46;
            this.btnChkAll.UseVisualStyleBackColor = true;
            this.btnChkAll.Click += new System.EventHandler(this.BtnChkAll_Click);
            // 
            // DispDetail
            // 
            this.DispDetail.AutoSize = true;
            this.DispDetail.Location = new System.Drawing.Point(26, 65);
            this.DispDetail.Margin = new System.Windows.Forms.Padding(0);
            this.DispDetail.Name = "DispDetail";
            this.DispDetail.Size = new System.Drawing.Size(133, 17);
            this.DispDetail.TabIndex = 59;
            this.DispDetail.Text = "Show Dispatch Details";
            this.DispDetail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DispDetail.UseVisualStyleBackColor = true;
            this.DispDetail.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SellerDetail
            // 
            this.SellerDetail.AutoSize = true;
            this.SellerDetail.Location = new System.Drawing.Point(168, 65);
            this.SellerDetail.Margin = new System.Windows.Forms.Padding(0);
            this.SellerDetail.Name = "SellerDetail";
            this.SellerDetail.Size = new System.Drawing.Size(117, 17);
            this.SellerDetail.TabIndex = 60;
            this.SellerDetail.Text = "Show Seller Details";
            this.SellerDetail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SellerDetail.UseVisualStyleBackColor = true;
            this.SellerDetail.CheckedChanged += new System.EventHandler(this.SellerDetail_CheckedChanged);
            // 
            // ShipDetail
            // 
            this.ShipDetail.AutoSize = true;
            this.ShipDetail.Location = new System.Drawing.Point(296, 65);
            this.ShipDetail.Margin = new System.Windows.Forms.Padding(0);
            this.ShipDetail.Name = "ShipDetail";
            this.ShipDetail.Size = new System.Drawing.Size(135, 17);
            this.ShipDetail.TabIndex = 61;
            this.ShipDetail.Text = "Show Shipment Details";
            this.ShipDetail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ShipDetail.UseVisualStyleBackColor = true;
            this.ShipDetail.CheckedChanged += new System.EventHandler(this.ShipDetail_CheckedChanged);
            // 
            // OthDetail
            // 
            this.OthDetail.AutoSize = true;
            this.OthDetail.Location = new System.Drawing.Point(433, 65);
            this.OthDetail.Name = "OthDetail";
            this.OthDetail.Size = new System.Drawing.Size(111, 17);
            this.OthDetail.TabIndex = 62;
            this.OthDetail.Text = "Show Other Detils";
            this.OthDetail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.OthDetail.UseVisualStyleBackColor = true;
            this.OthDetail.CheckedChanged += new System.EventHandler(this.OthDetail_CheckedChanged);
            // 
            // txtEWBCanRem
            // 
            this.txtEWBCanRem.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtEWBCanRem.Location = new System.Drawing.Point(851, 28);
            this.txtEWBCanRem.Name = "txtEWBCanRem";
            this.txtEWBCanRem.Size = new System.Drawing.Size(175, 21);
            this.txtEWBCanRem.TabIndex = 66;
            // 
            // lblEWBCanRem
            // 
            this.lblEWBCanRem.AutoSize = true;
            this.lblEWBCanRem.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblEWBCanRem.Location = new System.Drawing.Point(790, 30);
            this.lblEWBCanRem.Name = "lblEWBCanRem";
            this.lblEWBCanRem.Size = new System.Drawing.Size(63, 13);
            this.lblEWBCanRem.TabIndex = 65;
            this.lblEWBCanRem.Text = "Remarks:";
            // 
            // lblEWBCanRes
            // 
            this.lblEWBCanRes.AutoSize = true;
            this.lblEWBCanRes.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblEWBCanRes.Location = new System.Drawing.Point(573, 30);
            this.lblEWBCanRes.Name = "lblEWBCanRes";
            this.lblEWBCanRes.Size = new System.Drawing.Size(97, 13);
            this.lblEWBCanRes.TabIndex = 64;
            this.lblEWBCanRes.Text = "Cancel Reason:";
            // 
            // txtEWBCanRes
            // 
            this.txtEWBCanRes.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtEWBCanRes.FormattingEnabled = true;
            this.txtEWBCanRes.Items.AddRange(new object[] {
            "Duplicate",
            "Order Cancellled",
            "Data Entry Mistake",
            "Others"});
            this.txtEWBCanRes.Location = new System.Drawing.Point(673, 27);
            this.txtEWBCanRes.Name = "txtEWBCanRes";
            this.txtEWBCanRes.Size = new System.Drawing.Size(111, 21);
            this.txtEWBCanRes.TabIndex = 63;
            // 
            // frmEInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 520);
            this.Controls.Add(this.txtEWBCanRem);
            this.Controls.Add(this.lblEWBCanRem);
            this.Controls.Add(this.lblEWBCanRes);
            this.Controls.Add(this.txtEWBCanRes);
            this.Controls.Add(this.OthDetail);
            this.Controls.Add(this.ShipDetail);
            this.Controls.Add(this.SellerDetail);
            this.Controls.Add(this.DispDetail);
            this.Controls.Add(this.Server);
            this.Controls.Add(this.BtnIRNTKN);
            this.Controls.Add(this.QRImg);
            this.Controls.Add(this.txtInvDet);
            this.Controls.Add(this.txtErrMsg);
            this.Controls.Add(this.EInvStatusStrip1);
            this.Controls.Add(this.EInvBillDetGrid);
            this.Controls.Add(this.EWBGrpBox);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.EInvGrpBox);
            this.Controls.Add(this.BtnDebug);
            this.Controls.Add(this.btnUnChkAll);
            this.Controls.Add(this.btnChkAll);
            this.Name = "frmEInvoice";
            this.Text = "E-Invoice Generation Tool Ver. 1.015";
            ((System.ComponentModel.ISupportInitialize)(this.QRImg)).EndInit();
            this.EInvStatusStrip1.ResumeLayout(false);
            this.EInvStatusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EInvBillDetGrid)).EndInit();
            this.EWBGrpBox.ResumeLayout(false);
            this.EWBGrpBox.PerformLayout();
            this.EInvGrpBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ComboBox Server;
        internal System.Windows.Forms.Button BtnIRNTKN;
        internal System.Windows.Forms.PictureBox QRImg;
        internal System.Windows.Forms.TextBox txtInvDet;
        internal System.Windows.Forms.TextBox txtErrMsg;
        internal System.Windows.Forms.ToolStripStatusLabel oEInvStatusMsg;
        internal System.Windows.Forms.StatusStrip EInvStatusStrip1;
        internal System.Windows.Forms.DataGridView EInvBillDetGrid;
        internal System.Windows.Forms.TextBox txtkm;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button BtnEWBCancel;
        internal System.Windows.Forms.TextBox TxtEWBVeh;
        internal System.Windows.Forms.Label lblEWBVehNo;
        internal System.Windows.Forms.Button BtnEWBDetPrn;
        internal System.Windows.Forms.Button BtnEWBPrint;
        internal System.Windows.Forms.Button BtnEWBGen;
        internal System.Windows.Forms.GroupBox EWBGrpBox;
        internal System.Windows.Forms.Button QRCodeBtn;
        internal System.Windows.Forms.Button BtnGetDetail;
        internal System.Windows.Forms.Button BtnEInvGen;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.GroupBox EInvGrpBox;
        internal System.Windows.Forms.Button BtnDebug;
        internal System.Windows.Forms.Button btnUnChkAll;
        internal System.Windows.Forms.Button btnChkAll;
        private System.Windows.Forms.CheckBox DispDetail;
        private System.Windows.Forms.CheckBox SellerDetail;
        private System.Windows.Forms.CheckBox ShipDetail;
        private System.Windows.Forms.CheckBox OthDetail;
        internal System.Windows.Forms.TextBox txtEWBCanRem;
        internal System.Windows.Forms.Label lblEWBCanRem;
        internal System.Windows.Forms.Label lblEWBCanRes;
        internal System.Windows.Forms.ComboBox txtEWBCanRes;
    }
}