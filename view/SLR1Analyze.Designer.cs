﻿namespace ConsoleApplication1.view
{
    partial class SLR1Analyze
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
            this.NextStep = new System.Windows.Forms.Button();
            this.FromKeyBorad = new System.Windows.Forms.Button();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LogOutput = new System.Windows.Forms.TextBox();
            this.InputOutput = new System.Windows.Forms.TextBox();
            this.stackOutput = new System.Windows.Forms.TextBox();
            this.FinishButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NextStep
            // 
            this.NextStep.Enabled = false;
            this.NextStep.Location = new System.Drawing.Point(594, 349);
            this.NextStep.Name = "NextStep";
            this.NextStep.Size = new System.Drawing.Size(75, 23);
            this.NextStep.TabIndex = 18;
            this.NextStep.Text = "下一步";
            this.NextStep.UseVisualStyleBackColor = true;
            this.NextStep.Click += new System.EventHandler(this.NextStep_Click);
            // 
            // FromKeyBorad
            // 
            this.FromKeyBorad.Location = new System.Drawing.Point(593, 28);
            this.FromKeyBorad.Name = "FromKeyBorad";
            this.FromKeyBorad.Size = new System.Drawing.Size(75, 23);
            this.FromKeyBorad.TabIndex = 17;
            this.FromKeyBorad.Text = "确定";
            this.FromKeyBorad.UseVisualStyleBackColor = true;
            this.FromKeyBorad.Click += new System.EventHandler(this.FromKeyBorad_Click);
            // 
            // inputTextBox
            // 
            this.inputTextBox.Location = new System.Drawing.Point(12, 30);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(575, 21);
            this.inputTextBox.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(549, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "输出";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "输入";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "栈";
            // 
            // LogOutput
            // 
            this.LogOutput.Location = new System.Drawing.Point(453, 99);
            this.LogOutput.Multiline = true;
            this.LogOutput.Name = "LogOutput";
            this.LogOutput.ReadOnly = true;
            this.LogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogOutput.Size = new System.Drawing.Size(215, 244);
            this.LogOutput.TabIndex = 12;
            // 
            // InputOutput
            // 
            this.InputOutput.Location = new System.Drawing.Point(232, 99);
            this.InputOutput.Multiline = true;
            this.InputOutput.Name = "InputOutput";
            this.InputOutput.ReadOnly = true;
            this.InputOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InputOutput.Size = new System.Drawing.Size(215, 244);
            this.InputOutput.TabIndex = 11;
            // 
            // stackOutput
            // 
            this.stackOutput.Location = new System.Drawing.Point(11, 99);
            this.stackOutput.Multiline = true;
            this.stackOutput.Name = "stackOutput";
            this.stackOutput.ReadOnly = true;
            this.stackOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stackOutput.Size = new System.Drawing.Size(215, 244);
            this.stackOutput.TabIndex = 10;
            // 
            // FinishButton
            // 
            this.FinishButton.Enabled = false;
            this.FinishButton.Location = new System.Drawing.Point(512, 349);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(75, 23);
            this.FinishButton.TabIndex = 19;
            this.FinishButton.Text = "执行完";
            this.FinishButton.UseVisualStyleBackColor = true;
            this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // SLR1Analyze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 381);
            this.Controls.Add(this.FinishButton);
            this.Controls.Add(this.NextStep);
            this.Controls.Add(this.FromKeyBorad);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogOutput);
            this.Controls.Add(this.InputOutput);
            this.Controls.Add(this.stackOutput);
            this.Name = "SLR1Analyze";
            this.Text = "SLR1Analyze";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button NextStep;
        private System.Windows.Forms.Button FromKeyBorad;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LogOutput;
        private System.Windows.Forms.TextBox InputOutput;
        private System.Windows.Forms.TextBox stackOutput;
        private System.Windows.Forms.Button FinishButton;
    }
}