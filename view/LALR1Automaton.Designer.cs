namespace ConsoleApplication1.view
{
    partial class LALR1Automaton
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
            this.FinishButon = new System.Windows.Forms.Button();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.NextStep = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FinishButon
            // 
            this.FinishButon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FinishButon.Location = new System.Drawing.Point(904, 45);
            this.FinishButon.Name = "FinishButon";
            this.FinishButon.Size = new System.Drawing.Size(80, 27);
            this.FinishButon.TabIndex = 7;
            this.FinishButon.Text = "执行完";
            this.FinishButon.UseVisualStyleBackColor = true;
            this.FinishButon.Click += new System.EventHandler(this.FinishButon_Click);
            // 
            // outputTextBox
            // 
            this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTextBox.Location = new System.Drawing.Point(12, 12);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextBox.Size = new System.Drawing.Size(886, 60);
            this.outputTextBox.TabIndex = 6;
            // 
            // NextStep
            // 
            this.NextStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NextStep.Location = new System.Drawing.Point(904, 12);
            this.NextStep.Name = "NextStep";
            this.NextStep.Size = new System.Drawing.Size(80, 27);
            this.NextStep.TabIndex = 5;
            this.NextStep.Text = "下一步";
            this.NextStep.UseVisualStyleBackColor = true;
            this.NextStep.Click += new System.EventHandler(this.NextStep_Click);
            // 
            // LALR1Automaton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 432);
            this.Controls.Add(this.FinishButon);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.NextStep);
            this.Name = "LALR1Automaton";
            this.Text = "LALR1Automaton";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button FinishButon;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button NextStep;
    }
}