namespace ImageToBinary
{
    partial class ImageToBinary
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ChooseFile = new System.Windows.Forms.Button();
            this.Passes = new System.Windows.Forms.CheckedListBox();
            this.Output = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.Files = new System.Windows.Forms.Panel();
            this.FileDisplay = new System.Windows.Forms.Label();
            this.Files.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChooseFile
            // 
            this.ChooseFile.Location = new System.Drawing.Point(12, 12);
            this.ChooseFile.Name = "ChooseFile";
            this.ChooseFile.Size = new System.Drawing.Size(200, 23);
            this.ChooseFile.TabIndex = 0;
            this.ChooseFile.Text = "ChooseFile";
            this.ChooseFile.UseVisualStyleBackColor = true;
            this.ChooseFile.Click += new System.EventHandler(this.ChooseFile_Click);
            // 
            // Passes
            // 
            this.Passes.FormattingEnabled = true;
            this.Passes.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Alpha"});
            this.Passes.Location = new System.Drawing.Point(668, 104);
            this.Passes.Name = "Passes";
            this.Passes.Size = new System.Drawing.Size(120, 84);
            this.Passes.TabIndex = 2;
            this.Passes.SelectedIndexChanged += new System.EventHandler(this.Passes_SelectedIndexChanged);
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(691, 12);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(75, 23);
            this.Output.TabIndex = 3;
            this.Output.Text = "Output";
            this.Output.UseVisualStyleBackColor = true;
            this.Output.Click += new System.EventHandler(this.Output_Click);
            // 
            // Exit
            // 
            this.Exit.Location = new System.Drawing.Point(691, 58);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 23);
            this.Exit.TabIndex = 4;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Files
            // 
            this.Files.Controls.Add(this.FileDisplay);
            this.Files.Location = new System.Drawing.Point(14, 41);
            this.Files.Name = "Files";
            this.Files.Size = new System.Drawing.Size(648, 397);
            this.Files.TabIndex = 5;
            // 
            // FileDisplay
            // 
            this.FileDisplay.AutoSize = true;
            this.FileDisplay.Location = new System.Drawing.Point(-2, 0);
            this.FileDisplay.Name = "FileDisplay";
            this.FileDisplay.Size = new System.Drawing.Size(53, 12);
            this.FileDisplay.TabIndex = 1;
            this.FileDisplay.Text = "文件显示";
            // 
            // ImageToBinary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Files);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Passes);
            this.Controls.Add(this.ChooseFile);
            this.Name = "ImageToBinary";
            this.Text = "ImageToBinary";
            this.Files.ResumeLayout(false);
            this.Files.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ChooseFile;
        private System.Windows.Forms.CheckedListBox Passes;
        private System.Windows.Forms.Button Output;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Panel Files;
        private System.Windows.Forms.Label FileDisplay;
    }
}

