namespace ConsoleApplication1.view
{
    partial class LL1AnalyzeTable
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
            this.stackOutput = new System.Windows.Forms.TextBox();
            this.InputOutput = new System.Windows.Forms.TextBox();
            this.LogOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.FromKeyBorad = new System.Windows.Forms.Button();
            this.NextStep = new System.Windows.Forms.Button();
            this.finishButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // stackOutput
            // 
            this.stackOutput.Location = new System.Drawing.Point(12, 98);
            this.stackOutput.Multiline = true;
            this.stackOutput.Name = "stackOutput";
            this.stackOutput.ReadOnly = true;
            this.stackOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stackOutput.Size = new System.Drawing.Size(215, 244);
            this.stackOutput.TabIndex = 0;
            // 
            // InputOutput
            // 
            this.InputOutput.Location = new System.Drawing.Point(233, 98);
            this.InputOutput.Multiline = true;
            this.InputOutput.Name = "InputOutput";
            this.InputOutput.ReadOnly = true;
            this.InputOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InputOutput.Size = new System.Drawing.Size(215, 244);
            this.InputOutput.TabIndex = 1;
            // 
            // LogOutput
            // 
            this.LogOutput.Location = new System.Drawing.Point(454, 98);
            this.LogOutput.Multiline = true;
            this.LogOutput.Name = "LogOutput";
            this.LogOutput.ReadOnly = true;
            this.LogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogOutput.Size = new System.Drawing.Size(215, 244);
            this.LogOutput.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "栈";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(327, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "输入";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(550, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "输出";
            // 
            // inputTextBox
            // 
            this.inputTextBox.Location = new System.Drawing.Point(13, 29);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(575, 21);
            this.inputTextBox.TabIndex = 6;
            // 
            // FromKeyBorad
            // 
            this.FromKeyBorad.Location = new System.Drawing.Point(594, 27);
            this.FromKeyBorad.Name = "FromKeyBorad";
            this.FromKeyBorad.Size = new System.Drawing.Size(75, 23);
            this.FromKeyBorad.TabIndex = 7;
            this.FromKeyBorad.Text = "确定";
            this.FromKeyBorad.UseVisualStyleBackColor = true;
            this.FromKeyBorad.Click += new System.EventHandler(this.FromKeyBorad_Click);
            // 
            // NextStep
            // 
            this.NextStep.Enabled = false;
            this.NextStep.Location = new System.Drawing.Point(593, 346);
            this.NextStep.Name = "NextStep";
            this.NextStep.Size = new System.Drawing.Size(75, 23);
            this.NextStep.TabIndex = 9;
            this.NextStep.Text = "下一步";
            this.NextStep.UseVisualStyleBackColor = true;
            this.NextStep.Click += new System.EventHandler(this.NextStep_Click);
            // 
            // finishButton
            // 
            this.finishButton.Enabled = false;
            this.finishButton.Location = new System.Drawing.Point(512, 346);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(75, 23);
            this.finishButton.TabIndex = 10;
            this.finishButton.Text = "执行完";
            this.finishButton.UseVisualStyleBackColor = true;
            this.finishButton.Click += new System.EventHandler(this.finishButton_Click);
            // 
            // LL1AnalyzeTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 381);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.NextStep);
            this.Controls.Add(this.FromKeyBorad);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogOutput);
            this.Controls.Add(this.InputOutput);
            this.Controls.Add(this.stackOutput);
            this.Name = "LL1AnalyzeTable";
            this.Text = "AnalyzeTable";
            this.Load += new System.EventHandler(this.AnalyzeTable_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox stackOutput;
        private System.Windows.Forms.TextBox InputOutput;
        private System.Windows.Forms.TextBox LogOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Button FromKeyBorad;
        private System.Windows.Forms.Button NextStep;
        private System.Windows.Forms.Button finishButton;
    }
}