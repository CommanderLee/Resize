using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Resize_CSharp_Emgu
{
    public partial class Form1 : Form
    {
        private Image<Bgr, Byte>    srcImg = null;
        private Image<Gray, Byte>   srcImgGray = null;
        private int                 width;
        private int                 height;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /**
         * Calculate the energy of point (x, y)
         */
        private int energy(int x, int y)
        {
            int ret = 0;
            try
            {
                // Simple version
                if (x > 0)
                {
                    ret += Math.Abs(srcImg.Data[y, x, 0] - srcImg.Data[y, x - 1, 0]);
                }
                else
                {
                    ret += Math.Abs(srcImg.Data[y, x, 0]);
                }

                if (y > 0)
                {
                    ret += Math.Abs(srcImg.Data[y, x, 0] - srcImg.Data[y - 1, x, 0]);
                }
                else
                {
                    ret += Math.Abs(srcImg.Data[y, x, 0]);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return ret;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // Load the image
                srcImg = new Image<Bgr, Byte>(openFile.FileName);
                srcImgGray = srcImg.Convert<Gray, byte>();
                width = srcImg.Width;
                height = srcImg.Height;

                // Display the image. I(x, y, {B,G,R}): img.Data[y, x, {0,1,2}]
                pictureBoxSrc.Image = srcImg.ToBitmap();

                // Output debug info
                Debug.WriteLine(String.Format("Name:{0}, X:{1}, Y{2}", openFile.FileName, width, height));
                Debug.WriteLine(String.Format("I(499,0): B:{0}, G:{1}, R:{2}",
                    srcImg.Data[0, 499, 0], srcImg.Data[0, 499, 1], srcImg.Data[0, 499, 2]));
                Debug.WriteLine(energy(0, 0));
            }
        }
    }
}
