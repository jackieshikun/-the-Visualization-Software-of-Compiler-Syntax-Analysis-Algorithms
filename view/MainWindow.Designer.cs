namespace ConsoleApplication1.view
{
    partial class MainWindow
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
            this.LL1 = new System.Windows.Forms.Button();
            this.SLR1 = new System.Windows.Forms.Button();
            this.LR1 = new System.Windows.Forms.Button();
            this.LALR1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LL1
            // 
            this.LL1.Location = new System.Drawing.Point(168, 87);
            this.LL1.Name = "LL1";
            this.LL1.Size = new System.Drawing.Size(180, 37);
            this.LL1.TabIndex = 0;
            this.LL1.Text = "LL1";
            this.LL1.UseVisualStyleBackColor = true;
            this.LL1.Click += new System.EventHandler(this.LL1_Click);
            // 
            // SLR1
            // 
            this.SLR1.Location = new System.Drawing.Point(168, 130);
            this.SLR1.Name = "SLR1";
            this.SLR1.Size = new System.Drawing.Size(180, 40);
            this.SLR1.TabIndex = 1;
            this.SLR1.Text = "SLR1";
            this.SLR1.UseVisualStyleBackColor = true;
            this.SLR1.Click += new System.EventHandler(this.SLR1_Click);
            // 
            // LR1
            // 
            this.LR1.Location = new System.Drawing.Point(168, 176);
            this.LR1.Name = "LR1";
            this.LR1.Size = new System.Drawing.Size(180, 40);
            this.LR1.TabIndex = 2;
            this.LR1.Text = "LR1";
            this.LR1.UseVisualStyleBackColor = true;
            this.LR1.Click += new System.EventHandler(this.LR1_Click);
            // 
            // LALR1
            // 
            this.LALR1.Location = new System.Drawing.Point(168, 222);
            this.LALR1.Name = "LALR1";
            this.LALR1.Size = new System.Drawing.Size(180, 40);
            this.LALR1.TabIndex = 3;
            this.LALR1.Text = "LALR1";
            this.LALR1.UseVisualStyleBackColor = true;
            this.LALR1.Click += new System.EventHandler(this.LALR1_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 336);
            this.Controls.Add(this.LALR1);
            this.Controls.Add(this.LR1);
            this.Controls.Add(this.SLR1);
            this.Controls.Add(this.LL1);
            this.Name = "MainWindow";
            this.Text = "MainPage";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LL1;
        private System.Windows.Forms.Button SLR1;
        private System.Windows.Forms.Button LR1;
        private System.Windows.Forms.Button LALR1;
    }
}