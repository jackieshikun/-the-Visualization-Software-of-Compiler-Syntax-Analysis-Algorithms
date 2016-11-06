namespace ConsoleApplication1.view
{
    partial class LR1Automaton
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
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.NextStep = new System.Windows.Forms.Button();
            this.FinishButon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // outputTextBox
            // 
            this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTextBox.Location = new System.Drawing.Point(1, 2);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextBox.Size = new System.Drawing.Size(729, 60);
            this.outputTextBox.TabIndex = 3;
            this.outputTextBox.TextChanged += new System.EventHandler(this.outputTextBox_TextChanged);
            // 
            // NextStep
            // 
            this.NextStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NextStep.Location = new System.Drawing.Point(736, 2);
            this.NextStep.Name = "NextStep";
            this.NextStep.Size = new System.Drawing.Size(80, 27);
            this.NextStep.TabIndex = 2;
            this.NextStep.Text = "下一步";
            this.NextStep.UseVisualStyleBackColor = true;
            this.NextStep.Click += new System.EventHandler(this.NextStep_Click);
            // 
            // FinishButon
            // 
            this.FinishButon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FinishButon.Location = new System.Drawing.Point(736, 35);
            this.FinishButon.Name = "FinishButon";
            this.FinishButon.Size = new System.Drawing.Size(80, 27);
            this.FinishButon.TabIndex = 4;
            this.FinishButon.Text = "执行完";
            this.FinishButon.UseVisualStyleBackColor = true;
            this.FinishButon.Click += new System.EventHandler(this.FinishButon_Click);
            // 
            // LR1Automaton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 396);
            this.Controls.Add(this.FinishButon);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.NextStep);
            this.Name = "LR1Automaton";
            this.Text = "LR1Automaton";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button NextStep;
        private System.Windows.Forms.Button FinishButon;
    }
}