namespace TAFIE
{
    partial class CustomMessageBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomMessageBox));
            lblSummary = new Label();
            lblDescription = new Label();
            btnNo = new Button();
            btnYesOk = new Button();
            SuspendLayout();
            // 
            // lblSummary
            // 
            lblSummary.Font = new Font("Impact", 30F, FontStyle.Italic, GraphicsUnit.Point);
            lblSummary.Location = new Point(12, 9);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(424, 49);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "Summary";
            // 
            // lblDescription
            // 
            lblDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblDescription.BackColor = Color.FromArgb(11, 159, 161);
            lblDescription.Font = new Font("Consolas", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblDescription.ForeColor = Color.Black;
            lblDescription.Location = new Point(12, 70);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(501, 214);
            lblDescription.TabIndex = 0;
            lblDescription.Text = "Description";
            // 
            // btnNo
            // 
            btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNo.BackColor = Color.FromArgb(11, 159, 161);
            btnNo.Cursor = Cursors.Hand;
            btnNo.FlatStyle = FlatStyle.Flat;
            btnNo.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnNo.ForeColor = Color.Black;
            btnNo.Location = new Point(12, 287);
            btnNo.Name = "btnNo";
            btnNo.Size = new Size(87, 43);
            btnNo.TabIndex = 1;
            btnNo.Text = "No";
            btnNo.UseVisualStyleBackColor = false;
            btnNo.Click += btnNo_Click;
            btnNo.MouseEnter += btnNo_MouseEnter;
            btnNo.MouseLeave += btnNo_MouseLeave;
            // 
            // btnYesOk
            // 
            btnYesOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnYesOk.BackColor = Color.FromArgb(11, 159, 161);
            btnYesOk.Cursor = Cursors.Hand;
            btnYesOk.FlatStyle = FlatStyle.Flat;
            btnYesOk.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnYesOk.ForeColor = Color.Black;
            btnYesOk.Location = new Point(426, 287);
            btnYesOk.Name = "btnYesOk";
            btnYesOk.Size = new Size(87, 43);
            btnYesOk.TabIndex = 1;
            btnYesOk.Text = "Yes/Ok";
            btnYesOk.UseVisualStyleBackColor = false;
            btnYesOk.Click += btnYesOk_Click;
            btnYesOk.MouseEnter += btnYesOk_MouseEnter;
            btnYesOk.MouseLeave += btnYesOk_MouseLeave;
            // 
            // CustomMessageBox
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(525, 342);
            ControlBox = false;
            Controls.Add(lblDescription);
            Controls.Add(btnYesOk);
            Controls.Add(btnNo);
            Controls.Add(lblSummary);
            Font = new Font("Impact", 9F, FontStyle.Italic, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(11, 159, 161);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CustomMessageBox";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CustomMessageBox";
            ResumeLayout(false);
        }

        #endregion

        private Label lblSummary;
        private Button btnNo;
        private Button btnYesOk;
        public Label lblDescription;
    }
}