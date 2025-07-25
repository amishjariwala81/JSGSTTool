namespace EWayBillTool
{
    partial class SearchGSTIN
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
            this.TxtGSTINDet = new System.Windows.Forms.TextBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.TxtSearchGSTIN = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TxtGSTINDet
            // 
            this.TxtGSTINDet.Location = new System.Drawing.Point(12, 34);
            this.TxtGSTINDet.Multiline = true;
            this.TxtGSTINDet.Name = "TxtGSTINDet";
            this.TxtGSTINDet.Size = new System.Drawing.Size(265, 192);
            this.TxtGSTINDet.TabIndex = 20;
            //this.TxtGSTINDet.TextChanged += new System.EventHandler(this.TxtGSTINDet_TextChanged);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(144, 8);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(100, 23);
            this.BtnSearch.TabIndex = 19;
            this.BtnSearch.Text = "Search GSTIN";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // TxtSearchGSTIN
            // 
            this.TxtSearchGSTIN.Location = new System.Drawing.Point(12, 8);
            this.TxtSearchGSTIN.Name = "TxtSearchGSTIN";
            this.TxtSearchGSTIN.Size = new System.Drawing.Size(126, 20);
            this.TxtSearchGSTIN.TabIndex = 18;
            this.TxtSearchGSTIN.Text = "24AADCS3531J1ZI";
            //this.TxtSearchGSTIN.TextChanged += new System.EventHandler(this.TxtSearchGSTIN_TextChanged);
            // 
            // SearchGSTIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 237);
            this.Controls.Add(this.TxtGSTINDet);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.TxtSearchGSTIN);
            this.Name = "SearchGSTIN";
            this.Text = "SearchGSTIN";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox TxtGSTINDet;
        internal System.Windows.Forms.Button BtnSearch;
        internal System.Windows.Forms.TextBox TxtSearchGSTIN;
    }
}