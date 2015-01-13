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
        private int                 id = 0;

        // Image: [id mod 2]
        private Image<Bgr, Byte>[]  myImg;
        private Image<Gray, Byte>[] myImgGray;

        // Source/Target width and height
        private int                 srcWidth, srcHeight;
        private int                 tarWidth, tarHeight;

        // Energy value, vertical/horizontal seam matrix: [x, y, id mod 2]
        private int[,,]             energy;
        private int[,,]             verSeamMat;
        private int[,,]             horSeamMat;

        public Form1()
        {
            InitializeComponent();

            myImg = new Image<Bgr, byte>[2];
            myImgGray = new Image<Gray, Byte>[2];
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
                int currID = id % 2;
                // Simple version
                if (y > 0)
                {
                    ret += Math.Abs(myImgGray[currID].Data[x, y, 0] - myImgGray[currID].Data[x, y - 1, 0]);
                }
                else
                {
                    ret += Math.Abs(myImgGray[currID].Data[x, y, 0]);
                }

                if (x > 0)
                {
                    ret += Math.Abs(myImgGray[currID].Data[x, y, 0] - myImgGray[currID].Data[x - 1, y, 0]);
                }
                else
                {
                    ret += Math.Abs(myImgGray[currID].Data[x, y, 0]);
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
                myImg[0] = new Image<Bgr, Byte>(openFile.FileName);
                myImg[1] = new Image<Bgr, Byte>(openFile.FileName);
                srcWidth = myImg[0].Width;
                srcHeight = myImg[0].Height;

                // Display the image. 
                // I(x, y, {B,G,R}): img.Data[x, y, {0,1,2}]. x: row No. y: col No.
                pictureBoxSrc.Image = myImg[0].ToBitmap();

                // Output debug info
                Debug.WriteLine(String.Format("Name:{0}, Width:{1}, Height{2}", openFile.FileName, srcWidth, srcHeight));
                Debug.WriteLine(String.Format("I(0, 499): B:{0}, G:{1}, R:{2}",
                    myImg[0].Data[0, 499, 0], myImg[0].Data[0, 499, 1], myImg[0].Data[0, 499, 2]));
            }
        }

        private void buttonResize_Click(object sender, EventArgs e)
        {
            tarWidth = Convert.ToInt32(textBoxWidth.Text);
            tarHeight = Convert.ToInt32(textBoxHeight.Text);

            int currID = id % 2;

            // Get Gray image
            myImgGray[currID] = myImg[currID].Convert<Gray, byte>();

            // Get energy value
            energy = new int[srcHeight, srcWidth, 2];
            for (int i = 0; i < srcHeight; ++i)
            {
                for (int j = 0; j < srcWidth; ++j)
                {
                    energy[i, j, currID] = getEnergy(i, j);
                    // Debug.WriteLine(String.Format("{0},{1}: {2}", i, j, energy[i, j]));
                }
            }
            Debug.WriteLine("Energy value done.");
        }
    }
}
