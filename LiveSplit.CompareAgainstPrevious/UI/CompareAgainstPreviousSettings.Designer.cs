namespace LiveSplit.CompareAgainstPrevious.UI
{
    partial class CompareAgainstPreviousSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblCmpName = new System.Windows.Forms.Label();
            this.txtComparisonName = new System.Windows.Forms.TextBox();
            this.chkResetRuns = new System.Windows.Forms.CheckBox();
            this.grpRunOptions = new System.Windows.Forms.GroupBox();
            this.ttResetRuns = new System.Windows.Forms.ToolTip(this.components);
            this.resetRunAmount = new System.Windows.Forms.TrackBar();
            this.txtResetRunPct = new System.Windows.Forms.TextBox();
            this.grpRunOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resetRunAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCmpName
            // 
            this.lblCmpName.AutoSize = true;
            this.lblCmpName.Location = new System.Drawing.Point(5, 7);
            this.lblCmpName.Name = "lblCmpName";
            this.lblCmpName.Size = new System.Drawing.Size(96, 13);
            this.lblCmpName.TabIndex = 0;
            this.lblCmpName.Text = "Comparison Name:";
            // 
            // txtComparisonName
            // 
            this.txtComparisonName.Location = new System.Drawing.Point(305, 4);
            this.txtComparisonName.Name = "txtComparisonName";
            this.txtComparisonName.Size = new System.Drawing.Size(164, 20);
            this.txtComparisonName.TabIndex = 1;
            // 
            // chkResetRuns
            // 
            this.chkResetRuns.AutoSize = true;
            this.chkResetRuns.Location = new System.Drawing.Point(6, 19);
            this.chkResetRuns.Name = "chkResetRuns";
            this.chkResetRuns.Size = new System.Drawing.Size(104, 17);
            this.chkResetRuns.TabIndex = 3;
            this.chkResetRuns.Text = "Use Reset Runs";
            this.ttResetRuns.SetToolTip(this.chkResetRuns, "By default runs that are reset early are not tracked via previous run comparison");
            this.chkResetRuns.UseVisualStyleBackColor = true;
            this.chkResetRuns.CheckedChanged += new System.EventHandler(this.chkResetRuns_CheckedChanged);
            // 
            // grpRunOptions
            // 
            this.grpRunOptions.Controls.Add(this.txtResetRunPct);
            this.grpRunOptions.Controls.Add(this.resetRunAmount);
            this.grpRunOptions.Controls.Add(this.chkResetRuns);
            this.grpRunOptions.Location = new System.Drawing.Point(8, 30);
            this.grpRunOptions.Name = "grpRunOptions";
            this.grpRunOptions.Size = new System.Drawing.Size(461, 127);
            this.grpRunOptions.TabIndex = 4;
            this.grpRunOptions.TabStop = false;
            this.grpRunOptions.Text = "Run Options";
            // 
            // ttResetRuns
            // 
            this.ttResetRuns.Tag = "";
            // 
            // resetRunAmount
            // 
            this.resetRunAmount.Location = new System.Drawing.Point(6, 42);
            this.resetRunAmount.Maximum = 100;
            this.resetRunAmount.Name = "resetRunAmount";
            this.resetRunAmount.Size = new System.Drawing.Size(449, 45);
            this.resetRunAmount.TabIndex = 4;
            this.resetRunAmount.Value = 50;
            this.resetRunAmount.ValueChanged += new System.EventHandler(this.resetRunAmount_ValueChanged);
            // 
            // txtResetRunPct
            // 
            this.txtResetRunPct.Location = new System.Drawing.Point(116, 16);
            this.txtResetRunPct.Name = "txtResetRunPct";
            this.txtResetRunPct.ReadOnly = true;
            this.txtResetRunPct.Size = new System.Drawing.Size(100, 20);
            this.txtResetRunPct.TabIndex = 5;
            // 
            // CompareAgainstPreviousSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpRunOptions);
            this.Controls.Add(this.txtComparisonName);
            this.Controls.Add(this.lblCmpName);
            this.Name = "CompareAgainstPreviousSettings";
            this.Size = new System.Drawing.Size(472, 160);
            this.grpRunOptions.ResumeLayout(false);
            this.grpRunOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resetRunAmount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCmpName;
        private System.Windows.Forms.TextBox txtComparisonName;
        private System.Windows.Forms.CheckBox chkResetRuns;
        private System.Windows.Forms.GroupBox grpRunOptions;
        private System.Windows.Forms.ToolTip ttResetRuns;
        private System.Windows.Forms.TextBox txtResetRunPct;
        private System.Windows.Forms.TrackBar resetRunAmount;
    }
}
