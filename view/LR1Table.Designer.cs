﻿namespace ConsoleApplication1.view
{
    partial class LR1Table
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
            this.tableOutput = new System.Windows.Forms.TextBox();
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.FinishButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // NextStep
            // 
            this.NextStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NextStep.Location = new System.Drawing.Point(688, 2);
            this.NextStep.Name = "NextStep";
            this.NextStep.Size = new System.Drawing.Size(65, 35);
            this.NextStep.TabIndex = 8;
            this.NextStep.Text = "下一步";
            this.NextStep.UseVisualStyleBackColor = true;
            this.NextStep.Click += new System.EventHandler(this.NextStep_Click);
            // 
            // tableOutput
            // 
            this.tableOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableOutput.Location = new System.Drawing.Point(-1, 2);
            this.tableOutput.Multiline = true;
            this.tableOutput.Name = "tableOutput";
            this.tableOutput.ReadOnly = true;
            this.tableOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tableOutput.Size = new System.Drawing.Size(683, 70);
            this.tableOutput.TabIndex = 7;
            this.tableOutput.TextChanged += new System.EventHandler(this.tableOutput_TextChanged);
            // 
            // dataTable
            // 
            this.dataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTable.Location = new System.Drawing.Point(-11, 78);
            this.dataTable.Name = "dataTable";
            this.dataTable.ReadOnly = true;
            this.dataTable.RowTemplate.Height = 23;
            this.dataTable.Size = new System.Drawing.Size(763, 282);
            this.dataTable.TabIndex = 6;
            this.dataTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTable_CellContentClick);
            // 
            // FinishButton
            // 
            this.FinishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FinishButton.Location = new System.Drawing.Point(687, 37);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(65, 35);
            this.FinishButton.TabIndex = 9;
            this.FinishButton.Text = "执行完";
            this.FinishButton.UseVisualStyleBackColor = true;
            this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // LR1Table
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 370);
            this.Controls.Add(this.FinishButton);
            this.Controls.Add(this.NextStep);
            this.Controls.Add(this.tableOutput);
            this.Controls.Add(this.dataTable);
            this.Name = "LR1Table";
            this.Text = "LR1Table";
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button NextStep;
        private System.Windows.Forms.TextBox tableOutput;
        private System.Windows.Forms.DataGridView dataTable;
        private System.Windows.Forms.Button FinishButton;
    }
}