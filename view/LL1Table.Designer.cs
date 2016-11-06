namespace ConsoleApplication1.view
{
    partial class LL1Table
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
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.tableOutput = new System.Windows.Forms.TextBox();
            this.NextStep = new System.Windows.Forms.Button();
            this.FinishButon = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // dataTable
            // 
            this.dataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTable.Location = new System.Drawing.Point(-2, 76);
            this.dataTable.Name = "dataTable";
            this.dataTable.ReadOnly = true;
            this.dataTable.RowTemplate.Height = 23;
            this.dataTable.Size = new System.Drawing.Size(679, 276);
            this.dataTable.TabIndex = 0;
            this.dataTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTable_CellContentClick);
            // 
            // tableOutput
            // 
            this.tableOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableOutput.Location = new System.Drawing.Point(-2, 2);
            this.tableOutput.Multiline = true;
            this.tableOutput.Name = "tableOutput";
            this.tableOutput.ReadOnly = true;
            this.tableOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tableOutput.Size = new System.Drawing.Size(620, 68);
            this.tableOutput.TabIndex = 1;
            this.tableOutput.TextChanged += new System.EventHandler(this.tableOutput_TextChanged);
            // 
            // NextStep
            // 
            this.NextStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NextStep.Location = new System.Drawing.Point(617, 2);
            this.NextStep.Name = "NextStep";
            this.NextStep.Size = new System.Drawing.Size(60, 31);
            this.NextStep.TabIndex = 2;
            this.NextStep.Text = "下一步";
            this.NextStep.UseVisualStyleBackColor = true;
            this.NextStep.Click += new System.EventHandler(this.NextStep_Click);
            // 
            // FinishButon
            // 
            this.FinishButon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FinishButon.Location = new System.Drawing.Point(617, 39);
            this.FinishButon.Name = "FinishButon";
            this.FinishButon.Size = new System.Drawing.Size(60, 31);
            this.FinishButon.TabIndex = 3;
            this.FinishButon.Text = "执行完";
            this.FinishButon.UseVisualStyleBackColor = true;
            this.FinishButon.Click += new System.EventHandler(this.FinishButon_Click);
            // 
            // LL1Table
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 353);
            this.Controls.Add(this.FinishButon);
            this.Controls.Add(this.NextStep);
            this.Controls.Add(this.tableOutput);
            this.Controls.Add(this.dataTable);
            this.Name = "LL1Table";
            this.Text = "Table";
            this.Load += new System.EventHandler(this.Table_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataTable;
        private System.Windows.Forms.TextBox tableOutput;
        private System.Windows.Forms.Button NextStep;
        private System.Windows.Forms.Button FinishButon;
    }
}