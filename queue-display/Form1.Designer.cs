namespace queue_display
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
            lblTeller1 = new Label();
            lblTeller2 = new Label();
            lblTeller3 = new Label();
            SuspendLayout();
            // 
            // lblTeller1
            // 
            lblTeller1.AutoSize = true;
            lblTeller1.Location = new Point(23, 35);
            lblTeller1.Name = "lblTeller1";
            lblTeller1.Size = new Size(156, 32);
            lblTeller1.TabIndex = 0;
            lblTeller1.Text = "TELLER 1 : ---\n";
            // 
            // lblTeller2
            // 
            lblTeller2.AutoSize = true;
            lblTeller2.Location = new Point(320, 35);
            lblTeller2.Name = "lblTeller2";
            lblTeller2.Size = new Size(156, 32);
            lblTeller2.TabIndex = 1;
            lblTeller2.Text = "TELLER 2 s: ---\n";
            // 
            // lblTeller3
            // 
            lblTeller3.AutoSize = true;
            lblTeller3.Location = new Point(578, 35);
            lblTeller3.Name = "lblTeller3";
            lblTeller3.Size = new Size(156, 32);
            lblTeller3.TabIndex = 2;
            lblTeller3.Text = "TELLER 3 : ---\n";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblTeller3);
            Controls.Add(lblTeller2);
            Controls.Add(lblTeller1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTeller1;
        private Label lblTeller2;
        private Label lblTeller3;
    }
}
