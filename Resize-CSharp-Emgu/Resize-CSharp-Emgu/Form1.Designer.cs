namespace Resize_CSharp_Emgu
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelImg = new System.Windows.Forms.Panel();
            this.pictureBoxSrc = new System.Windows.Forms.PictureBox();
            this.panelBtn = new System.Windows.Forms.Panel();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonResize = new System.Windows.Forms.Button();
            this.panelImg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSrc)).BeginInit();
            this.panelBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelImg
            // 
            this.panelImg.BackColor = System.Drawing.Color.AliceBlue;
            this.panelImg.Controls.Add(this.pictureBoxSrc);
            this.panelImg.Location = new System.Drawing.Point(20, 20);
            this.panelImg.Name = "panelImg";
            this.panelImg.Size = new System.Drawing.Size(500, 500);
            this.panelImg.TabIndex = 0;
            // 
            // pictureBoxSrc
            // 
            this.pictureBoxSrc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxSrc.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSrc.Name = "pictureBoxSrc";
            this.pictureBoxSrc.Size = new System.Drawing.Size(500, 500);
            this.pictureBoxSrc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSrc.TabIndex = 0;
            this.pictureBoxSrc.TabStop = false;
            // 
            // panelBtn
            // 
            this.panelBtn.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelBtn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBtn.Controls.Add(this.buttonResize);
            this.panelBtn.Controls.Add(this.buttonLoad);
            this.panelBtn.Location = new System.Drawing.Point(558, 20);
            this.panelBtn.Name = "panelBtn";
            this.panelBtn.Size = new System.Drawing.Size(204, 500);
            this.panelBtn.TabIndex = 1;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(0, 0);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(200, 50);
            this.buttonLoad.TabIndex = 0;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonResize
            // 
            this.buttonResize.Location = new System.Drawing.Point(0, 200);
            this.buttonResize.Name = "buttonResize";
            this.buttonResize.Size = new System.Drawing.Size(200, 50);
            this.buttonResize.TabIndex = 1;
            this.buttonResize.Text = "Resize";
            this.buttonResize.UseVisualStyleBackColor = true;
            this.buttonResize.Click += new System.EventHandler(this.buttonResize_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.panelBtn);
            this.Controls.Add(this.panelImg);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelImg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSrc)).EndInit();
            this.panelBtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelImg;
        private System.Windows.Forms.Panel panelBtn;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.PictureBox pictureBoxSrc;
        private System.Windows.Forms.Button buttonResize;
    }
}

