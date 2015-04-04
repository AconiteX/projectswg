namespace SWGPacket_Workshop
{
    partial class warningFrm
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
            this.warningText = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.continueBtn = new System.Windows.Forms.Button();
            this.reminderCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // warningText
            // 
            this.warningText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.warningText.Location = new System.Drawing.Point(12, 9);
            this.warningText.Name = "warningText";
            this.warningText.Size = new System.Drawing.Size(312, 45);
            this.warningText.TabIndex = 0;
            this.warningText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(168, 57);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "&Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // continueBtn
            // 
            this.continueBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.continueBtn.Location = new System.Drawing.Point(249, 57);
            this.continueBtn.Name = "continueBtn";
            this.continueBtn.Size = new System.Drawing.Size(75, 23);
            this.continueBtn.TabIndex = 2;
            this.continueBtn.Text = "&Continue";
            this.continueBtn.UseVisualStyleBackColor = true;
            this.continueBtn.Click += new System.EventHandler(this.continueBtn_Click);
            // 
            // reminderCheck
            // 
            this.reminderCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reminderCheck.AutoSize = true;
            this.reminderCheck.Location = new System.Drawing.Point(12, 61);
            this.reminderCheck.Name = "reminderCheck";
            this.reminderCheck.Size = new System.Drawing.Size(129, 17);
            this.reminderCheck.TabIndex = 3;
            this.reminderCheck.Text = "Suppress this warning";
            this.reminderCheck.UseVisualStyleBackColor = true;
            // 
            // warningFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(336, 92);
            this.Controls.Add(this.reminderCheck);
            this.Controls.Add(this.continueBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.warningText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "warningFrm";
            this.Text = "Warning";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label warningText;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button continueBtn;
        private System.Windows.Forms.CheckBox reminderCheck;
    }
}