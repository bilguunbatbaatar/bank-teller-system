namespace teller_app
{
    partial class Form1
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
            cmbTeller = new ComboBox();
            lblTicket = new Label();
            btnNext = new Button();
            btnComplete = new Button();
            btnCancel = new Button();
            txtAccountNumber = new TextBox();
            btnSearch = new Button();
            lblName = new Label();
            lblBalance = new Label();
            txtAmount = new TextBox();
            btnDeposit = new Button();
            btnWithdraw = new Button();
            txtToAccount = new TextBox();
            btnTransfer = new Button();
            btnHistory = new Button();
            gridHistory = new DataGridView();
            txtOwnerName = new TextBox();
            btnCreateAccount = new Button();
            btnCloseAccount = new Button();
            txtCurrency = new TextBox();
            txtBuyRate = new TextBox();
            txtSellRate = new TextBox();
            btnUpdateRate = new Button();
            ((System.ComponentModel.ISupportInitialize)gridHistory).BeginInit();
            SuspendLayout();
            // 
            // cmbTeller
            // 
            cmbTeller.FormattingEnabled = true;
            cmbTeller.Location = new Point(12, 12);
            cmbTeller.Name = "cmbTeller";
            cmbTeller.Size = new Size(242, 40);
            cmbTeller.TabIndex = 0;
            cmbTeller.Text = "—";
            // 
            // lblTicket
            // 
            lblTicket.AutoSize = true;
            lblTicket.Location = new Point(58, 91);
            lblTicket.Name = "lblTicket";
            lblTicket.Size = new Size(54, 32);
            lblTicket.TabIndex = 1;
            lblTicket.Text = "----";
            // 
            // btnNext
            // 
            btnNext.Location = new Point(57, 172);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(150, 46);
            btnNext.TabIndex = 2;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnComplete
            // 
            btnComplete.Location = new Point(57, 248);
            btnComplete.Name = "btnComplete";
            btnComplete.Size = new Size(150, 46);
            btnComplete.TabIndex = 3;
            btnComplete.Text = "Complete";
            btnComplete.UseVisualStyleBackColor = true;
            btnComplete.Click += btnComplete_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(57, 329);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(150, 46);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // txtAccountNumber
            // 
            txtAccountNumber.Location = new Point(369, 20);
            txtAccountNumber.Name = "txtAccountNumber";
            txtAccountNumber.Size = new Size(200, 39);
            txtAccountNumber.TabIndex = 5;
            txtAccountNumber.Text = "Account";
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(369, 95);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(150, 46);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(381, 170);
            lblName.Name = "lblName";
            lblName.Size = new Size(44, 32);
            lblName.TabIndex = 7;
            lblName.Text = "---";
            // 
            // lblBalance
            // 
            lblBalance.AutoSize = true;
            lblBalance.Location = new Point(381, 221);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(44, 32);
            lblBalance.TabIndex = 8;
            lblBalance.Text = "---";
            // 
            // txtAmount
            // 
            txtAmount.Location = new Point(381, 275);
            txtAmount.Name = "txtAmount";
            txtAmount.Size = new Size(200, 39);
            txtAmount.TabIndex = 9;
            txtAmount.Text = "Amount";
            // 
            // btnDeposit
            // 
            btnDeposit.Location = new Point(380, 385);
            btnDeposit.Name = "btnDeposit";
            btnDeposit.Size = new Size(150, 46);
            btnDeposit.TabIndex = 10;
            btnDeposit.Text = "Deposit";
            btnDeposit.UseVisualStyleBackColor = true;
            btnDeposit.Click += btnDeposit_Click;
            // 
            // btnWithdraw
            // 
            btnWithdraw.BackColor = SystemColors.Window;
            btnWithdraw.Location = new Point(381, 450);
            btnWithdraw.Name = "btnWithdraw";
            btnWithdraw.Size = new Size(150, 46);
            btnWithdraw.TabIndex = 11;
            btnWithdraw.Text = "Withdraw";
            btnWithdraw.UseVisualStyleBackColor = false;
            btnWithdraw.Click += btnWithdraw_Click;
            // 
            // txtToAccount
            // 
            txtToAccount.Location = new Point(381, 320);
            txtToAccount.Name = "txtToAccount";
            txtToAccount.Size = new Size(200, 39);
            txtToAccount.TabIndex = 12;
            txtToAccount.Text = "To Account";
            // 
            // btnTransfer
            // 
            btnTransfer.Location = new Point(381, 523);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(150, 46);
            btnTransfer.TabIndex = 13;
            btnTransfer.Text = "Transfer";
            btnTransfer.UseVisualStyleBackColor = true;
            btnTransfer.Click += btnTransfer_Click;
            // 
            // btnHistory
            // 
            btnHistory.Location = new Point(381, 590);
            btnHistory.Name = "btnHistory";
            btnHistory.Size = new Size(150, 46);
            btnHistory.TabIndex = 14;
            btnHistory.Text = "History";
            btnHistory.UseVisualStyleBackColor = true;
            btnHistory.Click += btnHistory_Click;
            // 
            // gridHistory
            // 
            gridHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridHistory.Location = new Point(974, 20);
            gridHistory.Name = "gridHistory";
            gridHistory.RowHeadersWidth = 82;
            gridHistory.Size = new Size(480, 300);
            gridHistory.TabIndex = 15;
            // 
            // txtOwnerName
            // 
            txtOwnerName.Location = new Point(638, 20);
            txtOwnerName.Name = "txtOwnerName";
            txtOwnerName.Size = new Size(200, 39);
            txtOwnerName.TabIndex = 16;
            txtOwnerName.Text = "OwnerName";
            // 
            // btnCreateAccount
            // 
            btnCreateAccount.Location = new Point(638, 95);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(210, 46);
            btnCreateAccount.TabIndex = 17;
            btnCreateAccount.Text = "Create Account";
            btnCreateAccount.UseVisualStyleBackColor = true;
            btnCreateAccount.Click += btnCreateAccount_Click;
            // 
            // btnCloseAccount
            // 
            btnCloseAccount.Location = new Point(380, 653);
            btnCloseAccount.Name = "btnCloseAccount";
            btnCloseAccount.Size = new Size(150, 46);
            btnCloseAccount.TabIndex = 18;
            btnCloseAccount.Text = "Close Account";
            btnCloseAccount.UseVisualStyleBackColor = true;
            btnCloseAccount.Click += btnCloseAccount_Click;
            // 
            // txtCurrency
            // 
            txtCurrency.Location = new Point(1037, 424);
            txtCurrency.Name = "txtCurrency";
            txtCurrency.Size = new Size(200, 39);
            txtCurrency.TabIndex = 19;
            txtCurrency.Text = "Currency";
            // 
            // txtBuyRate
            // 
            txtBuyRate.Location = new Point(1042, 486);
            txtBuyRate.Name = "txtBuyRate";
            txtBuyRate.Size = new Size(200, 39);
            txtBuyRate.TabIndex = 20;
            txtBuyRate.Text = "Buy";
            // 
            // txtSellRate
            // 
            txtSellRate.Location = new Point(1043, 551);
            txtSellRate.Name = "txtSellRate";
            txtSellRate.Size = new Size(200, 39);
            txtSellRate.TabIndex = 21;
            txtSellRate.Text = "Sell";
            // 
            // btnUpdateRate
            // 
            btnUpdateRate.Location = new Point(1044, 617);
            btnUpdateRate.Name = "btnUpdateRate";
            btnUpdateRate.Size = new Size(150, 46);
            btnUpdateRate.TabIndex = 22;
            btnUpdateRate.Text = "Update";
            btnUpdateRate.UseVisualStyleBackColor = true;
            btnUpdateRate.Click += btnUpdateRate_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.WindowFrame;
            ClientSize = new Size(1913, 780);
            Controls.Add(btnUpdateRate);
            Controls.Add(txtSellRate);
            Controls.Add(txtBuyRate);
            Controls.Add(txtCurrency);
            Controls.Add(btnCloseAccount);
            Controls.Add(btnCreateAccount);
            Controls.Add(txtOwnerName);
            Controls.Add(gridHistory);
            Controls.Add(btnHistory);
            Controls.Add(btnTransfer);
            Controls.Add(txtToAccount);
            Controls.Add(btnWithdraw);
            Controls.Add(btnDeposit);
            Controls.Add(txtAmount);
            Controls.Add(lblBalance);
            Controls.Add(lblName);
            Controls.Add(btnSearch);
            Controls.Add(txtAccountNumber);
            Controls.Add(btnCancel);
            Controls.Add(btnComplete);
            Controls.Add(btnNext);
            Controls.Add(lblTicket);
            Controls.Add(cmbTeller);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)gridHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbTeller;
        private Label lblTicket;
        private Button btnNext;
        private Button btnComplete;
        private Button btnCancel;
        private TextBox txtAccountNumber;
        private Button btnSearch;
        private Label lblName;
        private Label lblBalance;
        private TextBox txtAmount;
        private Button btnDeposit;
        private Button btnWithdraw;
        private TextBox txtToAccount;
        private Button btnTransfer;
        private Button btnHistory;
        private DataGridView gridHistory;
        private TextBox txtOwnerName;
        private Button btnCreateAccount;
        private Button btnCloseAccount;
        private TextBox txtCurrency;
        private TextBox txtBuyRate;
        private TextBox txtSellRate;
        private Button btnUpdateRate;
    }
}
