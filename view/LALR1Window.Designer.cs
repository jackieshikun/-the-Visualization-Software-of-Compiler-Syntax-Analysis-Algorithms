﻿namespace ConsoleApplication1.view
{
    partial class LALR1Window
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
            this.backing = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.RuleDisplay = new System.Windows.Forms.TextBox();
            this.AnalyzeInput = new System.Windows.Forms.Button();
            this.GetTable = new System.Windows.Forms.Button();
            this.GetLR0 = new System.Windows.Forms.Button();
            this.OpenFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // backing
            // 
            this.backing.Location = new System.Drawing.Point(5, 7);
            this.backing.Name = "backing";
            this.backing.Size = new System.Drawing.Size(47, 20);
            this.backing.TabIndex = 26;
            this.backing.Text = "返回";
            this.backing.UseVisualStyleBackColor = true;
            this.backing.Click += new System.EventHandler(this.backing_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(460, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "syntax";
            // 
            // RuleDisplay
            // 
            this.RuleDisplay.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RuleDisplay.Location = new System.Drawing.Point(404, 102);
            this.RuleDisplay.Multiline = true;
            this.RuleDisplay.Name = "RuleDisplay";
            this.RuleDisplay.ReadOnly = true;
            this.RuleDisplay.Size = new System.Drawing.Size(164, 203);
            this.RuleDisplay.TabIndex = 24;
            // 
            // AnalyzeInput
            // 
            this.AnalyzeInput.Enabled = false;
            this.AnalyzeInput.Location = new System.Drawing.Point(36, 270);
            this.AnalyzeInput.Name = "AnalyzeInput";
            this.AnalyzeInput.Size = new System.Drawing.Size(180, 35);
            this.AnalyzeInput.TabIndex = 23;
            this.AnalyzeInput.Text = "分析输入串";
            this.AnalyzeInput.UseVisualStyleBackColor = true;
            this.AnalyzeInput.Click += new System.EventHandler(this.AnalyzeInput_Click);
            // 
            // GetTable
            // 
            this.GetTable.Enabled = false;
            this.GetTable.Location = new System.Drawing.Point(36, 200);
            this.GetTable.Name = "GetTable";
            this.GetTable.Size = new System.Drawing.Size(180, 35);
            this.GetTable.TabIndex = 22;
            this.GetTable.Text = "查看分析表";
            this.GetTable.UseVisualStyleBackColor = true;
            this.GetTable.Click += new System.EventHandler(this.GetTable_Click);
            // 
            // GetLR0
            // 
            this.GetLR0.Enabled = false;
            this.GetLR0.Location = new System.Drawing.Point(36, 133);
            this.GetLR0.Name = "GetLR0";
            this.GetLR0.Size = new System.Drawing.Size(180, 35);
            this.GetLR0.TabIndex = 21;
            this.GetLR0.Text = "合并同心集";
            this.GetLR0.UseVisualStyleBackColor = true;
            this.GetLR0.Click += new System.EventHandler(this.GetLR0_Click);
            // 
            // OpenFile
            // 
            this.OpenFile.Location = new System.Drawing.Point(36, 64);
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(180, 40);
            this.OpenFile.TabIndex = 20;
            this.OpenFile.Text = "输入规则式";
            this.OpenFile.UseVisualStyleBackColor = true;
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // LALR1Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 370);
            this.Controls.Add(this.backing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RuleDisplay);
            this.Controls.Add(this.AnalyzeInput);
            this.Controls.Add(this.GetTable);
            this.Controls.Add(this.GetLR0);
            this.Controls.Add(this.OpenFile);
            this.Name = "LALR1Window";
            this.Text = "LALR1Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button backing;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RuleDisplay;
        private System.Windows.Forms.Button AnalyzeInput;
        private System.Windows.Forms.Button GetTable;
        private System.Windows.Forms.Button GetLR0;
        private System.Windows.Forms.Button OpenFile;
    }
}