namespace Resize_CSharp_Emgu
{
    partial class SeamCarving
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
            this.panelImg1 = new System.Windows.Forms.Panel();
            this.pictureBoxSrc = new System.Windows.Forms.PictureBox();
            this.panelBtn = new System.Windows.Forms.Panel();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.buttonResize = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.panelImg2 = new System.Windows.Forms.Panel();
            this.pictureBoxTar = new System.Windows.Forms.PictureBox();
            this.panelImg1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSrc)).BeginInit();
            this.panelBtn.SuspendLayout();
            this.panelImg2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTar)).BeginInit();
            this.SuspendLayout();
            // 
            // panelImg1
            // 
            this.panelImg1.BackColor = System.Drawing.Color.AliceBlue;
            this.panelImg1.Controls.Add(this.pictureBoxSrc);
            this.panelImg1.Location = new System.Drawing.Point(20, 20);
            this.panelImg1.Name = "panelImg1";
            this.panelImg1.Size = new System.Drawing.Size(700, 500);
            this.panelImg1.TabIndex = 0;
            // 
            // pictureBoxSrc
            // 
            this.pictureBoxSrc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxSrc.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSrc.Name = "pictureBoxSrc";
            this.pictureBoxSrc.Size = new System.Drawing.Size(700, 500);
            this.pictureBoxSrc.TabIndex = 0;
            this.pictureBoxSrc.TabStop = false;
            // 
            // panelBtn
            // 
            this.panelBtn.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelBtn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBtn.Controls.Add(this.textBoxHeight);
            this.panelBtn.Controls.Add(this.textBoxWidth);
            this.panelBtn.Controls.Add(this.labelHeight);
            this.panelBtn.Controls.Add(this.labelWidth);
            this.panelBtn.Controls.Add(this.buttonResize);
            this.panelBtn.Controls.Add(this.buttonLoad);
            this.panelBtn.Location = new System.Drawing.Point(758, 20);
            this.panelBtn.Name = "panelBtn";
            this.panelBtn.Size = new System.Drawing.Size(204, 500);
            this.panelBtn.TabIndex = 1;
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(70, 55);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(120, 22);
            this.textBoxHeight.TabIndex = 5;
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(70, 90);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(120, 22);
            this.textBoxWidth.TabIndex = 4;
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelHeight.Location = new System.Drawing.Point(0, 55);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(63, 20);
            this.labelHeight.TabIndex = 3;
            this.labelHeight.Text = "Height:";
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelWidth.Location = new System.Drawing.Point(0, 90);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(62, 20);
            this.labelWidth.TabIndex = 2;
            this.labelWidth.Text = " Width:";
            // 
            // buttonResize
            // 
            this.buttonResize.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonResize.Location = new System.Drawing.Point(0, 200);
            this.buttonResize.Name = "buttonResize";
            this.buttonResize.Size = new System.Drawing.Size(200, 50);
            this.buttonResize.TabIndex = 1;
            this.buttonResize.Text = "Resize";
            this.buttonResize.UseVisualStyleBackColor = true;
            this.buttonResize.Click += new System.EventHandler(this.buttonResize_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonLoad.Location = new System.Drawing.Point(0, 0);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(200, 50);
            this.buttonLoad.TabIndex = 0;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // panelImg2
            // 
            this.panelImg2.Controls.Add(this.pictureBoxTar);
            this.panelImg2.Location = new System.Drawing.Point(1000, 20);
            this.panelImg2.Name = "panelImg2";
            this.panelImg2.Size = new System.Drawing.Size(700, 500);
            this.panelImg2.TabIndex = 2;
            // 
            // pictureBoxTar
            // 
            this.pictureBoxTar.BackColor = System.Drawing.Color.AliceBlue;
            this.pictureBoxTar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxTar.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTar.Name = "pictureBoxTar";
            this.pictureBoxTar.Size = new System.Drawing.Size(700, 500);
            this.pictureBoxTar.TabIndex = 0;
            this.pictureBoxTar.TabStop = false;
            // 
            // SeamCarving
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1732, 553);
            this.Controls.Add(this.panelImg2);
            this.Controls.Add(this.panelBtn);
            this.Controls.Add(this.panelImg1);
            this.Name = "SeamCarving";
            this.Text = "SeamCarving - Zhen";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelImg1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSrc)).EndInit();
            this.panelBtn.ResumeLayout(false);
            this.panelBtn.PerformLayout();
            this.panelImg2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelImg1;
        private System.Windows.Forms.Panel panelBtn;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.PictureBox pictureBoxSrc;
        private System.Windows.Forms.Button buttonResize;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.Panel panelImg2;
        private System.Windows.Forms.PictureBox pictureBoxTar;
    }
}

