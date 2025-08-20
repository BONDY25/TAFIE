namespace TAFIE
{
    partial class MenuForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuForm));
            btnExit = new Button();
            btnToolBox = new Button();
            lblTitle = new Label();
            btnCarrLabels = new Button();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(11, 159, 161);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnExit.ForeColor = Color.Black;
            btnExit.Location = new Point(12, 388);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(229, 43);
            btnExit.TabIndex = 3;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            btnExit.MouseEnter += btnExit_MouseEnter;
            btnExit.MouseLeave += btnExit_MouseLeave;
            // 
            // btnToolBox
            // 
            btnToolBox.BackColor = Color.FromArgb(11, 159, 161);
            btnToolBox.Cursor = Cursors.Hand;
            btnToolBox.FlatStyle = FlatStyle.Flat;
            btnToolBox.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnToolBox.ForeColor = Color.Black;
            btnToolBox.Location = new Point(12, 102);
            btnToolBox.Name = "btnToolBox";
            btnToolBox.Size = new Size(229, 100);
            btnToolBox.TabIndex = 4;
            btnToolBox.Text = "ToolBox";
            btnToolBox.UseVisualStyleBackColor = false;
            btnToolBox.Click += btnTooBox_Click;
            btnToolBox.MouseEnter += btnToolBox_MouseEnter;
            btnToolBox.MouseLeave += btnToolBox_MouseLeave;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 60F, FontStyle.Italic, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(12, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(229, 90);
            lblTitle.TabIndex = 5;
            lblTitle.Text = "TAFIE";
            // 
            // btnCarrLabels
            // 
            btnCarrLabels.BackColor = Color.FromArgb(11, 159, 161);
            btnCarrLabels.Cursor = Cursors.Hand;
            btnCarrLabels.FlatStyle = FlatStyle.Flat;
            btnCarrLabels.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnCarrLabels.ForeColor = Color.Black;
            btnCarrLabels.Location = new Point(12, 208);
            btnCarrLabels.Name = "btnCarrLabels";
            btnCarrLabels.Size = new Size(229, 100);
            btnCarrLabels.TabIndex = 4;
            btnCarrLabels.Text = "Carrier Labels";
            btnCarrLabels.UseVisualStyleBackColor = false;
            btnCarrLabels.Click += btnCarrLabels_Click;
            btnCarrLabels.MouseEnter += btnCarrLabels_MouseEnter;
            btnCarrLabels.MouseLeave += btnCarrLabels_MouseLeave;
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(253, 443);
            ControlBox = false;
            Controls.Add(lblTitle);
            Controls.Add(btnCarrLabels);
            Controls.Add(btnToolBox);
            Controls.Add(btnExit);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MenuForm";
            Load += MenuForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnExit;
        private Button btnToolBox;
        private Label lblTitle;
        private Button btnCarrLabels;
    }
}