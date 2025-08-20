namespace TAFIE
{
    partial class xmlView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xmlView));
            rtbxXML = new RichTextBox();
            btnClose = new Button();
            SuspendLayout();
            // 
            // rtbxXML
            // 
            rtbxXML.BorderStyle = BorderStyle.FixedSingle;
            rtbxXML.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            rtbxXML.Location = new Point(12, 12);
            rtbxXML.Name = "rtbxXML";
            rtbxXML.ReadOnly = true;
            rtbxXML.Size = new Size(776, 669);
            rtbxXML.TabIndex = 12;
            rtbxXML.Text = "";
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(11, 159, 161);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnClose.ForeColor = Color.Black;
            btnClose.Location = new Point(12, 687);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(95, 46);
            btnClose.TabIndex = 13;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;
            // 
            // xmlView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(800, 745);
            ControlBox = false;
            Controls.Add(btnClose);
            Controls.Add(rtbxXML);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "xmlView";
            Text = "xmlView";
            Load += xmlView_Load;
            KeyDown += xmlView_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox rtbxXML;
        private Button btnClose;
    }
}