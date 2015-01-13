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
    public partial class SeamCarving : Form
    {
        private int                 id = 0;

        // Image: [id mod 2]
        private Image<Bgr, Byte>    srcImg;
        private Image<Bgr, Byte>[]  myImg;
        private Image<Gray, Byte>[] myImgGray;

        // Source/Current/Target width and height
        private int                 srcWidth, srcHeight;
        private int                 currWidth, currHeight;
        private int                 tarWidth, tarHeight;

        // Energy value, vertical/horizontal seam matrix: [x, y, id mod 2]
        private int[,,]             energy;
        private int[,,]             verSeamMat, horSeamMat;

        private int[]               verSeam, horSeam;

        public SeamCarving()
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

        private void carveVerticalSeam()
        {
            int currID = id % 2;
            // Calculate energy matrix with DP
            for (int i = 0; i < currHeight; ++i)
            {
                for (int j = 0; j < currWidth; ++j)
                {
                    if (i > 0)
                    {
                        verSeamMat[i, j, currID] = energy[i, j, currID] + verSeamMat[i - 1, j, currID];
                        if (j > 0)
                        {
                            verSeamMat[i, j, currID] = Math.Min(energy[i, j, currID] + verSeamMat[i - 1, j - 1, currID],
                                verSeamMat[i, j, currID]);
                        }
                        if (j < currWidth - 1)
                        {
                            verSeamMat[i, j, currID] = Math.Min(energy[i, j, currID] + verSeamMat[i - 1, j + 1, currID],
                                verSeamMat[i, j, currID]);
                        }
                    }
                    else
                    {
                        verSeamMat[i, j, currID] = energy[i, j, currID];
                    }
                }
            }
            Debug.WriteLine(String.Format("Seam No.{0} - Vertical - Matrix done.", id));

            ++id;
            int newID = id % 2;

            // Get the column No. of the seam on the last row
            int col = 0, minMatrix = verSeamMat[currHeight - 1, 0, currID];
            for (int j = 1; j < currWidth; ++j)
            {
                if (verSeamMat[currHeight - 1, j, currID] < minMatrix)
                {
                    // update min
                    col = j;
                    minMatrix = verSeamMat[currHeight - 1, j, currID];
                }
            }
            verSeam[currHeight - 1] = col;

            // Get column No. on the other rows and get new image and energy
            for (int i = currHeight - 1; i >= 0; --i)
            {
                // Copy [0..col - 1]
                for (int j = 0; j < col; ++j)
                {
                    myImg[newID][i, j] = myImg[currID][i, j];
                    myImgGray[newID][i, j] = myImgGray[currID][i, j];
                    energy[i, j, newID] = energy[i, j, currID];
                }

                // Move [col + 1..old width - 1] to left
                for (int j = col; j < currWidth - 1; ++j)
                {
                    myImg[newID][i, j] = myImg[currID][i, j + 1];
                    myImgGray[newID][i, j] = myImgGray[currID][i, j + 1];
                    energy[i, j, newID] = energy[i, j + 1, currID];
                }

                // Get next column No.
                if (i > 0)
                {
                    int newCol = col;
                    minMatrix = verSeamMat[i - 1, newCol, currID];
                    if (col > 0 && verSeamMat[i - 1, col - 1, currID] < minMatrix)
                    {
                        newCol = col - 1;
                        minMatrix = verSeamMat[i - 1, col - 1, currID];
                    }
                    if (col < currWidth - 1 && verSeamMat[i - 1, col + 1, currID] < minMatrix)
                    {
                        newCol = col + 1;
                    }
                    verSeam[i - 1] = newCol;
                }
            }

            // Calculate other energy at point near the seam
            // The energy of most points doesnot change
            for (int i = 0; i < currHeight; ++i)
            {
                for (int j = verSeam[i] - 1; j <= verSeam[i]; ++j)
                {
                    // Valid point in the new image
                    if (j >= 0 && j < currWidth - 1)
                    {
                        energy[i, j, newID] = getEnergy(i, j);
                    }
                }
            }
        }

        private void carveHorizontalSeam()
        {
        }

        private void resize()
        {
            currWidth = srcWidth;
            currHeight = srcHeight;

            if (currWidth > tarWidth)
            {
                while (currWidth > tarWidth)
                {
                    carveVerticalSeam();
                    --currWidth;
                }
            }

            int currID = id % 2;
            Image<Bgr, Byte> tarImg = new Image<Bgr, Byte>(tarWidth, tarHeight);
            for (int i = 0; i < tarHeight; ++i)
                for (int j = 0; j < tarWidth; ++j)
                    tarImg[i, j] = myImg[currID][i, j];
            pictureBoxTar.Image = tarImg.ToBitmap();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // Load the image
                srcImg = new Image<Bgr, Byte>(openFile.FileName);
                myImg[0] = new Image<Bgr, Byte>(openFile.FileName);
                myImg[1] = new Image<Bgr, Byte>(openFile.FileName);
                myImgGray[0] = myImg[0].Convert<Gray, byte>();
                myImgGray[1] = myImg[1].Convert<Gray, byte>();
                srcWidth = myImg[0].Width;
                srcHeight = myImg[0].Height;

                // Display the image. 
                // I(x, y, {B,G,R}): img.Data[x, y, {0,1,2}]. x: row No. y: col No.
                pictureBoxSrc.Image = myImg[0].ToBitmap();

                // Output debug info
                Debug.WriteLine(String.Format("Name:{0}, Height:{1}, Width:{2}", openFile.FileName, srcHeight, srcWidth));
                //Debug.WriteLine(String.Format("I(0, 499): B:{0}, G:{1}, R:{2}",
                //    myImg[0].Data[0, 499, 0], myImg[0].Data[0, 499, 1], myImg[0].Data[0, 499, 2]));
            }
        }

        private void buttonResize_Click(object sender, EventArgs e)
        {
            id = 0;
            myImg[0] = srcImg;
            myImgGray[0] = srcImg.Convert<Gray, Byte>();
            tarWidth = Convert.ToInt32(textBoxWidth.Text);
            tarHeight = Convert.ToInt32(textBoxHeight.Text);
            Debug.WriteLine(String.Format("Begin: {0}x{1} -> {2}x{3}", srcHeight, srcWidth, tarHeight, tarWidth));

            int currID = id % 2;

            // Get Gray image
            // myImgGray[currID] = myImg[currID].Convert<Gray, byte>();

            // Get energy value
            energy = new int[srcHeight, srcWidth, 2];
            verSeamMat = new int[srcHeight, srcWidth, 2];
            horSeamMat = new int[srcHeight, srcWidth, 2];
            verSeam = new int[srcHeight];
            horSeam = new int[srcWidth];
            for (int i = 0; i < srcHeight; ++i)
            {
                for (int j = 0; j < srcWidth; ++j)
                {
                    energy[i, j, currID] = getEnergy(i, j);
                    // Debug.WriteLine(String.Format("{0},{1}: {2}", i, j, energy[i, j]));
                }
            }
            Debug.WriteLine("Energy value done.");

            resize();
        }
    }
}
