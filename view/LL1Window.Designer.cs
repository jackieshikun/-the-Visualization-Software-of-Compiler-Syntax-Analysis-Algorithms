namespace ConsoleApplication1.view
{
    partial class LL1Window
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
            this.OpenFile = new System.Windows.Forms.Button();
            this.GetFirstAndFollow = new System.Windows.Forms.Button();
            this.GetTable = new System.Windows.Forms.Button();
            this.AnalyzeInput = new System.Windows.Forms.Button();
            this.RuleDisplay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backing = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OpenFile
            // 
            this.OpenFile.Location = new System.Drawing.Point(61, 65);
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(180, 40);
            this.OpenFile.TabIndex = 0;
            this.OpenFile.Text = "输入规则式";
            this.OpenFile.UseVisualStyleBackColor = true;
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // GetFirstAndFollow
            // 
            this.GetFirstAndFollow.Enabled = false;
            this.GetFirstAndFollow.Location = new System.Drawing.Point(61, 127);
            this.GetFirstAndFollow.Name = "GetFirstAndFollow";
            this.GetFirstAndFollow.Size = new System.Drawing.Size(180, 35);
            this.GetFirstAndFollow.TabIndex = 1;
            this.GetFirstAndFollow.Text = "查看first和follow集";
            this.GetFirstAndFollow.UseVisualStyleBackColor = true;
            this.GetFirstAndFollow.Click += new System.EventHandler(this.GetFirstAndFollow_Click);
            // 
            // GetTable
            // 
            this.GetTable.Enabled = false;
            this.GetTable.Location = new System.Drawing.Point(61, 180);
            this.GetTable.Name = "GetTable";
            this.GetTable.Size = new System.Drawing.Size(180, 35);
            this.GetTable.TabIndex = 2;
            this.GetTable.Text = "查看分析表";
            this.GetTable.UseVisualStyleBackColor = true;
            this.GetTable.Click += new System.EventHandler(this.GetTable_Click);
            // 
            // AnalyzeInput
            // 
            this.AnalyzeInput.Enabled = false;
            this.AnalyzeInput.Location = new System.Drawing.Point(61, 233);
            this.AnalyzeInput.Name = "AnalyzeInput";
            this.AnalyzeInput.Size = new System.Drawing.Size(180, 35);
            this.AnalyzeInput.TabIndex = 3;
            this.AnalyzeInput.Text = "分析输入串";
            this.AnalyzeInput.UseVisualStyleBackColor = true;
            this.AnalyzeInput.Click += new System.EventHandler(this.AnalyzeInput_Click);
            // 
            // RuleDisplay
            // 
            this.RuleDisplay.Location = new System.Drawing.Point(380, 65);
            this.RuleDisplay.Multiline = true;
            this.RuleDisplay.Name = "RuleDisplay";
            this.RuleDisplay.ReadOnly = true;
            this.RuleDisplay.Size = new System.Drawing.Size(164, 203);
            this.RuleDisplay.TabIndex = 4;
            this.RuleDisplay.TextChanged += new System.EventHandler(this.RuleDisplay_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(436, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "syntax";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // backing
            // 
            this.backing.Location = new System.Drawing.Point(2, 3);
            this.backing.Name = "backing";
            this.backing.Size = new System.Drawing.Size(47, 20);
            this.backing.TabIndex = 6;
            this.backing.Text = "返回";
            this.backing.UseVisualStyleBackColor = true;
            this.backing.Click += new System.EventHandler(this.backing_Click);
            // 
            // LL1Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 359);
            this.Controls.Add(this.backing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RuleDisplay);
            this.Controls.Add(this.AnalyzeInput);
            this.Controls.Add(this.GetTable);
            this.Controls.Add(this.GetFirstAndFollow);
            this.Controls.Add(this.OpenFile);
            this.Name = "LL1Window";
            this.Text = "LL1Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenFile;
        private System.Windows.Forms.Button GetFirstAndFollow;
        private System.Windows.Forms.Button GetTable;
        private System.Windows.Forms.Button AnalyzeInput;
        private System.Windows.Forms.TextBox RuleDisplay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button backing;

    }
}