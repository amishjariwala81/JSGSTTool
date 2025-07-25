namespace EWayBillTool
{
    partial class frmEWayBill
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEWayBill));
            this.Label2 = new System.Windows.Forms.Label();
            this.BtnGetDetail = new System.Windows.Forms.Button();
            this.btnTKN = new System.Windows.Forms.Button();
            this.oEWBStatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnDetPrint = new System.Windows.Forms.Button();
            this.BtnDebug = new System.Windows.Forms.Button();
            this.TxtVehNo = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtkm = new System.Windows.Forms.TextBox();
            this.txtErrMsg = new System.Windows.Forms.TextBox();
            this.txtCanRem = new System.Windows.Forms.TextBox();
            this.lblCanRem = new System.Windows.Forms.Label();
            this.lblCancelRes = new System.Windows.Forms.Label();
            this.CmbCancelRes = new System.Windows.Forms.ComboBox();
            this.btnUnChkAll = new System.Windows.Forms.Button();
            this.btnChkAll = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.EWBStatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.EWBBillDetGrid = new System.Windows.Forms.DataGridView();
            this.EWBStatusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EWBBillDetGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Label2.Location = new System.Drawing.Point(816, 44);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(29, 13);
            this.Label2.TabIndex = 65;
            this.Label2.Text = "KM:";
            // 
            // BtnGetDetail
            // 
            this.BtnGetDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.BtnGetDetail.Image = ((System.Drawing.Image)(resources.GetObject("BtnGetDetail.Image")));
            this.BtnGetDetail.Location = new System.Drawing.Point(114, 3);
            this.BtnGetDetail.Name = "BtnGetDetail";
            this.BtnGetDetail.Size = new System.Drawing.Size(31, 31);
            this.BtnGetDetail.TabIndex = 64;
            this.BtnGetDetail.UseVisualStyleBackColor = true;
            this.BtnGetDetail.Click += new System.EventHandler(this.BtnGetDetail_Click);
            // 
            // btnTKN
            // 
            this.btnTKN.Location = new System.Drawing.Point(1155, 3);
            this.btnTKN.Name = "btnTKN";
            this.btnTKN.Size = new System.Drawing.Size(39, 31);
            this.btnTKN.TabIndex = 63;
            this.btnTKN.Text = "TKN";
            this.btnTKN.UseVisualStyleBackColor = true;
            this.btnTKN.Click += new System.EventHandler(this.btnTKN_Click);
            // 
            // oEWBStatusMsg
            // 
            this.oEWBStatusMsg.Name = "oEWBStatusMsg";
            this.oEWBStatusMsg.Size = new System.Drawing.Size(146, 17);
            this.oEWBStatusMsg.Text = "E-Way Bill Generation Tool";
            // 
            // btnDetPrint
            // 
            this.btnDetPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnDetPrint.Image")));
            this.btnDetPrint.Location = new System.Drawing.Point(77, 3);
            this.btnDetPrint.Name = "btnDetPrint";
            this.btnDetPrint.Size = new System.Drawing.Size(31, 31);
            this.btnDetPrint.TabIndex = 62;
            this.btnDetPrint.UseVisualStyleBackColor = true;
            this.btnDetPrint.Click += new System.EventHandler(this.btnDetPrint_Click);
            // 
            // BtnDebug
            // 
            this.BtnDebug.Image = ((System.Drawing.Image)(resources.GetObject("BtnDebug.Image")));
            this.BtnDebug.Location = new System.Drawing.Point(1196, 3);
            this.BtnDebug.Name = "BtnDebug";
            this.BtnDebug.Size = new System.Drawing.Size(31, 31);
            this.BtnDebug.TabIndex = 61;
            this.BtnDebug.UseVisualStyleBackColor = true;
            this.BtnDebug.Click += new System.EventHandler(this.BtnDebug_Click);
            // 
            // TxtVehNo
            // 
            this.TxtVehNo.Location = new System.Drawing.Point(969, 8);
            this.TxtVehNo.Name = "TxtVehNo";
            this.TxtVehNo.Size = new System.Drawing.Size(107, 20);
            this.TxtVehNo.TabIndex = 60;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(808, 11);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(112, 13);
            this.Label1.TabIndex = 59;
            this.Label1.Text = "Vehicle No. for Part-B:";
            // 
            // txtkm
            // 
            this.txtkm.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtkm.Location = new System.Drawing.Point(850, 40);
            this.txtkm.Name = "txtkm";
            this.txtkm.Size = new System.Drawing.Size(37, 21);
            this.txtkm.TabIndex = 66;
            // 
            // txtErrMsg
            // 
            this.txtErrMsg.BackColor = System.Drawing.Color.White;
            this.txtErrMsg.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtErrMsg.ForeColor = System.Drawing.Color.Red;
            this.txtErrMsg.Location = new System.Drawing.Point(2, 433);
            this.txtErrMsg.Multiline = true;
            this.txtErrMsg.Name = "txtErrMsg";
            this.txtErrMsg.ReadOnly = true;
            this.txtErrMsg.Size = new System.Drawing.Size(1227, 55);
            this.txtErrMsg.TabIndex = 58;
            // 
            // txtCanRem
            // 
            this.txtCanRem.Location = new System.Drawing.Point(525, 8);
            this.txtCanRem.Name = "txtCanRem";
            this.txtCanRem.Size = new System.Drawing.Size(249, 20);
            this.txtCanRem.TabIndex = 57;
            // 
            // lblCanRem
            // 
            this.lblCanRem.AutoSize = true;
            this.lblCanRem.Location = new System.Drawing.Point(455, 11);
            this.lblCanRem.Name = "lblCanRem";
            this.lblCanRem.Size = new System.Drawing.Size(52, 13);
            this.lblCanRem.TabIndex = 56;
            this.lblCanRem.Text = "Remarks:";
            // 
            // lblCancelRes
            // 
            this.lblCancelRes.AutoSize = true;
            this.lblCancelRes.Location = new System.Drawing.Point(158, 11);
            this.lblCancelRes.Name = "lblCancelRes";
            this.lblCancelRes.Size = new System.Drawing.Size(108, 13);
            this.lblCancelRes.TabIndex = 55;
            this.lblCancelRes.Text = "Cancellation Reason:";
            // 
            // CmbCancelRes
            // 
            this.CmbCancelRes.FormattingEnabled = true;
            this.CmbCancelRes.Items.AddRange(new object[] {
            "Duplicate",
            "Order Cancellled",
            "Data Entry Mistake",
            "Others"});
            this.CmbCancelRes.Location = new System.Drawing.Point(303, 8);
            this.CmbCancelRes.Name = "CmbCancelRes";
            this.CmbCancelRes.Size = new System.Drawing.Size(149, 21);
            this.CmbCancelRes.TabIndex = 54;
            // 
            // btnUnChkAll
            // 
            this.btnUnChkAll.Image = ((System.Drawing.Image)(resources.GetObject("btnUnChkAll.Image")));
            this.btnUnChkAll.Location = new System.Drawing.Point(1121, 3);
            this.btnUnChkAll.Name = "btnUnChkAll";
            this.btnUnChkAll.Size = new System.Drawing.Size(31, 31);
            this.btnUnChkAll.TabIndex = 53;
            this.btnUnChkAll.UseVisualStyleBackColor = true;
            this.btnUnChkAll.Click += new System.EventHandler(this.btnUnChkAll_Click);
            // 
            // btnChkAll
            // 
            this.btnChkAll.Image = ((System.Drawing.Image)(resources.GetObject("btnChkAll.Image")));
            this.btnChkAll.Location = new System.Drawing.Point(1084, 3);
            this.btnChkAll.Name = "btnChkAll";
            this.btnChkAll.Size = new System.Drawing.Size(31, 31);
            this.btnChkAll.TabIndex = 52;
            this.btnChkAll.UseVisualStyleBackColor = true;
            this.btnChkAll.Click += new System.EventHandler(this.btnChkAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(776, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(31, 31);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // EWBStatusStrip1
            // 
            this.EWBStatusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.EWBStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oEWBStatusMsg});
            this.EWBStatusStrip1.Location = new System.Drawing.Point(0, 496);
            this.EWBStatusStrip1.Name = "EWBStatusStrip1";
            this.EWBStatusStrip1.Size = new System.Drawing.Size(1232, 22);
            this.EWBStatusStrip1.TabIndex = 50;
            this.EWBStatusStrip1.Text = "StatusStrip1";
            // 
            // btnPrint
            // 
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.Location = new System.Drawing.Point(40, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(31, 31);
            this.btnPrint.TabIndex = 49;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerate.Image")));
            this.btnGenerate.Location = new System.Drawing.Point(3, 3);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(31, 31);
            this.btnGenerate.TabIndex = 48;
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // EWBBillDetGrid
            // 
            this.EWBBillDetGrid.AllowUserToAddRows = false;
            this.EWBBillDetGrid.AllowUserToDeleteRows = false;
            this.EWBBillDetGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.EWBBillDetGrid.BackgroundColor = System.Drawing.Color.White;
            this.EWBBillDetGrid.CausesValidation = false;
            this.EWBBillDetGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EWBBillDetGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.EWBBillDetGrid.Location = new System.Drawing.Point(2, 68);
            this.EWBBillDetGrid.Name = "EWBBillDetGrid";
            this.EWBBillDetGrid.ReadOnly = true;
            this.EWBBillDetGrid.RowHeadersWidth = 51;
            this.EWBBillDetGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.EWBBillDetGrid.Size = new System.Drawing.Size(1218, 358);
            this.EWBBillDetGrid.TabIndex = 47;
            // 
            // frmEWayBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 518);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.BtnGetDetail);
            this.Controls.Add(this.btnTKN);
            this.Controls.Add(this.btnDetPrint);
            this.Controls.Add(this.BtnDebug);
            this.Controls.Add(this.TxtVehNo);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.txtkm);
            this.Controls.Add(this.txtErrMsg);
            this.Controls.Add(this.txtCanRem);
            this.Controls.Add(this.lblCanRem);
            this.Controls.Add(this.lblCancelRes);
            this.Controls.Add(this.CmbCancelRes);
            this.Controls.Add(this.btnUnChkAll);
            this.Controls.Add(this.btnChkAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.EWBStatusStrip1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.EWBBillDetGrid);
            this.Name = "frmEWayBill";
            this.Text = "E-Way Bill Generation Tool Ver. 1.015";
            //this.Load += new System.EventHandler(this.frmEWayBill_Load);
            this.EWBStatusStrip1.ResumeLayout(false);
            this.EWBStatusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EWBBillDetGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button BtnGetDetail;
        internal System.Windows.Forms.Button btnTKN;
        internal System.Windows.Forms.ToolStripStatusLabel oEWBStatusMsg;
        internal System.Windows.Forms.Button btnDetPrint;
        internal System.Windows.Forms.Button BtnDebug;
        internal System.Windows.Forms.TextBox TxtVehNo;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.TextBox txtkm;
        internal System.Windows.Forms.TextBox txtErrMsg;
        internal System.Windows.Forms.TextBox txtCanRem;
        internal System.Windows.Forms.Label lblCanRem;
        internal System.Windows.Forms.Label lblCancelRes;
        internal System.Windows.Forms.ComboBox CmbCancelRes;
        internal System.Windows.Forms.Button btnUnChkAll;
        internal System.Windows.Forms.Button btnChkAll;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.StatusStrip EWBStatusStrip1;
        internal System.Windows.Forms.Button btnPrint;
        internal System.Windows.Forms.Button btnGenerate;
        internal System.Windows.Forms.DataGridView EWBBillDetGrid;
    }
}