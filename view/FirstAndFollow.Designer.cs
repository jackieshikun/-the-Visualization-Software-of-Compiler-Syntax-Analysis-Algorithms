namespace ConsoleApplication1.view
{
    partial class FirstAndFollow
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
            this.firstSetOutput = new System.Windows.Forms.TextBox();
            this.followSetOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.firstNextStep = new System.Windows.Forms.Button();
            this.followNextStep = new System.Windows.Forms.Button();
            this.firstSet = new System.Windows.Forms.TextBox();
            this.followSet = new System.Windows.Forms.TextBox();
            this.first集 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FirstFinishButton = new System.Windows.Forms.Button();
            this.FollowFinishButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // firstSetOutput
            // 
            this.firstSetOutput.Location = new System.Drawing.Point(73, 34);
            this.firstSetOutput.Multiline = true;
            this.firstSetOutput.Name = "firstSetOutput";
            this.firstSetOutput.ReadOnly = true;
            this.firstSetOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.firstSetOutput.Size = new System.Drawing.Size(230, 129);
            this.firstSetOutput.TabIndex = 0;
            // 
            // followSetOutput
            // 
            this.followSetOutput.Location = new System.Drawing.Point(73, 207);
            this.followSetOutput.Multiline = true;
            this.followSetOutput.Name = "followSetOutput";
            this.followSetOutput.ReadOnly = true;
            this.followSetOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.followSetOutput.Size = new System.Drawing.Size(230, 131);
            this.followSetOutput.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(113, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "first集的处理过程";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "follow集的处理过程";
            // 
            // firstNextStep
            // 
            this.firstNextStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.firstNextStep.Location = new System.Drawing.Point(339, 140);
            this.firstNextStep.Name = "firstNextStep";
            this.firstNextStep.Size = new System.Drawing.Size(75, 23);
            this.firstNextStep.TabIndex = 4;
            this.firstNextStep.Text = "下一步";
            this.firstNextStep.UseVisualStyleBackColor = true;
            this.firstNextStep.Click += new System.EventHandler(this.firstNextStep_Click);
            // 
            // followNextStep
            // 
            this.followNextStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.followNextStep.Location = new System.Drawing.Point(339, 315);
            this.followNextStep.Name = "followNextStep";
            this.followNextStep.Size = new System.Drawing.Size(75, 23);
            this.followNextStep.TabIndex = 5;
            this.followNextStep.Text = "下一步";
            this.followNextStep.UseVisualStyleBackColor = true;
            this.followNextStep.Click += new System.EventHandler(this.followNextStep_Click);
            // 
            // firstSet
            // 
            this.firstSet.Location = new System.Drawing.Point(444, 34);
            this.firstSet.Multiline = true;
            this.firstSet.Name = "firstSet";
            this.firstSet.ReadOnly = true;
            this.firstSet.Size = new System.Drawing.Size(205, 129);
            this.firstSet.TabIndex = 6;
            // 
            // followSet
            // 
            this.followSet.Location = new System.Drawing.Point(444, 221);
            this.followSet.Multiline = true;
            this.followSet.Name = "followSet";
            this.followSet.ReadOnly = true;
            this.followSet.Size = new System.Drawing.Size(205, 117);
            this.followSet.TabIndex = 7;
            // 
            // first集
            // 
            this.first集.AutoSize = true;
            this.first集.Location = new System.Drawing.Point(512, 19);
            this.first集.Name = "first集";
            this.first集.Size = new System.Drawing.Size(47, 12);
            this.first集.TabIndex = 8;
            this.first集.Text = "first集";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(518, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "follow集";
            // 
            // FirstFinishButton
            // 
            this.FirstFinishButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FirstFinishButton.Location = new System.Drawing.Point(339, 111);
            this.FirstFinishButton.Name = "FirstFinishButton";
            this.FirstFinishButton.Size = new System.Drawing.Size(75, 23);
            this.FirstFinishButton.TabIndex = 10;
            this.FirstFinishButton.Text = "执行完";
            this.FirstFinishButton.UseVisualStyleBackColor = true;
            this.FirstFinishButton.Click += new System.EventHandler(this.FirstFinishButton_Click);
            // 
            // FollowFinishButton
            // 
            this.FollowFinishButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FollowFinishButton.Location = new System.Drawing.Point(339, 286);
            this.FollowFinishButton.Name = "FollowFinishButton";
            this.FollowFinishButton.Size = new System.Drawing.Size(75, 23);
            this.FollowFinishButton.TabIndex = 11;
            this.FollowFinishButton.Text = "执行完";
            this.FollowFinishButton.UseVisualStyleBackColor = true;
            this.FollowFinishButton.Click += new System.EventHandler(this.FollowFinishButton_Click);
            // 
            // FirstAndFollow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 350);
            this.Controls.Add(this.FollowFinishButton);
            this.Controls.Add(this.FirstFinishButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.first集);
            this.Controls.Add(this.followSet);
            this.Controls.Add(this.firstSet);
            this.Controls.Add(this.followNextStep);
            this.Controls.Add(this.firstNextStep);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.followSetOutput);
            this.Controls.Add(this.firstSetOutput);
            this.Name = "FirstAndFollow";
            this.Text = "FirstAndFollow";
            this.Load += new System.EventHandler(this.FirstAndFollow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox firstSetOutput;
        private System.Windows.Forms.TextBox followSetOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button firstNextStep;
        private System.Windows.Forms.Button followNextStep;
        private System.Windows.Forms.TextBox firstSet;
        private System.Windows.Forms.TextBox followSet;
        private System.Windows.Forms.Label first集;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button FirstFinishButton;
        private System.Windows.Forms.Button FollowFinishButton;
    }
}