namespace TAFIE
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            lblTitle = new Label();
            cntLogin = new Panel();
            label1 = new Label();
            btnExit = new Button();
            btnLogin = new Button();
            txbUsername = new TextBox();
            label2 = new Label();
            lblVersion = new Label();
            ttEgg = new ToolTip(components);
            cntLogin.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 60F, FontStyle.Italic, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(0, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(209, 90);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "TAFIE";
            lblTitle.Click += lblTitle_Click;
            lblTitle.MouseEnter += lblTitle_MouseEnter;
            // 
            // cntLogin
            // 
            cntLogin.BackColor = Color.Black;
            cntLogin.Controls.Add(label1);
            cntLogin.Controls.Add(btnExit);
            cntLogin.Controls.Add(btnLogin);
            cntLogin.Controls.Add(txbUsername);
            cntLogin.Location = new Point(0, 400);
            cntLogin.Name = "cntLogin";
            cntLogin.Size = new Size(487, 67);
            cntLogin.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Impact", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(11, 159, 161);
            label1.Location = new Point(12, 8);
            label1.Name = "label1";
            label1.Size = new Size(64, 17);
            label1.TabIndex = 2;
            label1.Text = "Username";
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(11, 159, 161);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnExit.ForeColor = Color.Black;
            btnExit.Location = new Point(292, 8);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(87, 43);
            btnExit.TabIndex = 2;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            btnExit.MouseEnter += btnExit_MouseEnter;
            btnExit.MouseLeave += btnExit_MouseLeave;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(11, 159, 161);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnLogin.ForeColor = Color.Black;
            btnLogin.Location = new Point(385, 8);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(87, 43);
            btnLogin.TabIndex = 1;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            btnLogin.MouseEnter += btnLogin_MouseEnter;
            btnLogin.MouseLeave += btnLogin_MouseLeave;
            // 
            // txbUsername
            // 
            txbUsername.BackColor = Color.DarkGray;
            txbUsername.BorderStyle = BorderStyle.FixedSingle;
            txbUsername.CharacterCasing = CharacterCasing.Upper;
            txbUsername.Cursor = Cursors.IBeam;
            txbUsername.Font = new Font("Impact", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            txbUsername.ForeColor = Color.Black;
            txbUsername.Location = new Point(12, 28);
            txbUsername.MaxLength = 100;
            txbUsername.Name = "txbUsername";
            txbUsername.Size = new Size(251, 23);
            txbUsername.TabIndex = 0;
            txbUsername.Enter += txbUsername_Enter;
            txbUsername.KeyPress += txbUsername_KeyPress;
            txbUsername.Leave += txbUsername_Leave;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Impact", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(215, 24);
            label2.Name = "label2";
            label2.Size = new Size(224, 17);
            label2.TabIndex = 3;
            label2.Text = "Toolbox for APIs to Fix Integration Errors";
            // 
            // lblVersion
            // 
            lblVersion.BackColor = Color.Transparent;
            lblVersion.Font = new Font("Arial", 7F, FontStyle.Regular, GraphicsUnit.Point);
            lblVersion.Location = new Point(370, 384);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(106, 13);
            lblVersion.TabIndex = 4;
            lblVersion.Text = "Build: vXX.XX.XX";
            lblVersion.TextAlign = ContentAlignment.TopRight;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = Properties.Resources.TAFIE_Logo;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(484, 461);
            Controls.Add(lblVersion);
            Controls.Add(label2);
            Controls.Add(cntLogin);
            Controls.Add(lblTitle);
            Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            FormClosing += LoginForm_FormClosing;
            Load += LoginForm_Load;
            cntLogin.ResumeLayout(false);
            cntLogin.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Panel cntLogin;
        private Button btnLogin;
        private TextBox txbUsername;
        private Button btnExit;
        private Label label1;
        private Label label2;
        private Label lblVersion;
        private ToolTip ttEgg;
    }
}