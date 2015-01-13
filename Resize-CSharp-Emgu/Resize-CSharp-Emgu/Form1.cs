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

        // Energy value, vertical/horizontal seam matrix
        private int[,]              energy;
        private int[,]              verSeamMat;
        private int[,]              horSeamMat;

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
        private int getEnergy(int x, int y)
        {
            int ret = 0;
            try
            {
                // Simple version
                if (y > 0)
                {
                    ret += Math.Abs(srcImg.Data[x, y, 0] - srcImg.Data[x, y - 1, 0]);
                }
                else
                {
                    ret += Math.Abs(srcImg.Data[x, y, 0]);
                }

                if (x > 0)
                {
                    ret += Math.Abs(srcImg.Data[x, y, 0] - srcImg.Data[x - 1, y, 0]);
                }
                else
                {
                    ret += Math.Abs(srcImg.Data[x, y, 0]);
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

                // Display the image. 
                // I(x, y, {B,G,R}): img.Data[x, y, {0,1,2}]. x: row No. y: col No.
                pictureBoxSrc.Image = srcImg.ToBitmap();

                // Output debug info
                Debug.WriteLine(String.Format("Name:{0}, Width:{1}, Height{2}", openFile.FileName, width, height));
                Debug.WriteLine(String.Format("I(0, 499): B:{0}, G:{1}, R:{2}",
                    srcImg.Data[0, 499, 0], srcImg.Data[0, 499, 1], srcImg.Data[0, 499, 2]));
                Debug.WriteLine(getEnergy(0, 0));
            }
        }

        private void buttonResize_Click(object sender, EventArgs e)
        {
            // Get energy value
            energy = new int[height, width];
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    energy[i, j] = getEnergy(i, j);
                    // Debug.WriteLine(String.Format("{0},{1}: {2}", i, j, energy[i, j]));
                }
            }
            Debug.WriteLine("Energy value done.");
        }
    }
}
