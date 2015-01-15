﻿using System;
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

        // Source/Current/Target width and height
        private int                 srcWidth, srcHeight;
        private int                 currWidth, currHeight;
        private int                 tarWidth, tarHeight;

        // Energy value, vertical/horizontal seam matrix: [x, y, id mod 2]
        private int[,,]             energy;
        private int[,,]             verSeamMat, horSeamMat;

        private int[]               verSeam, horSeam;

        private int[,]              seamMap;
        private Stack<int[]>        verSeams, horSeams;

        public SeamCarving()
        {
            InitializeComponent();

            myImg = new Image<Bgr, byte>[2];
            verSeams = new Stack<int[]>();
            horSeams = new Stack<int[]>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /**
         * Calculate the energy of point (x, y)
         */
        private int getEnergy(int x, int y)
        {
            int result = 0, diff = 0;
            int currID = id % 2;
            try
            {
                // Dual gradient energy function: The energy of pixel (x, y) is Δx2(x, y) + Δy2(x, y)
                // Learn this funcion from http://www.cs.princeton.edu/courses/archive/spring13/cos226/assignments/seamCarving.html
                for (int c = 0; c <= 2; ++c)
                {
                    if (x == 0)
                    {
                        diff = myImg[currID].Data[x + 1, y, c] - myImg[currID].Data[currHeight - 1, y, c];
                        result += diff * diff;
                    }
                    else if (x == currHeight - 1)
                    {
                        diff = myImg[currID].Data[0, y, c] - myImg[currID].Data[x - 1, y, c];
                        result += diff * diff;
                    }
                    else
                    {
                        diff = myImg[currID].Data[x + 1, y, c] - myImg[currID].Data[x - 1, y, c];
                        result += diff * diff;
                    }

                    if (y == 0)
                    {
                        diff = myImg[currID].Data[x, y + 1, c] - myImg[currID].Data[x, currWidth - 1, c];
                        result += diff * diff;
                    }
                    else if (y == currWidth - 1)
                    {
                        diff = myImg[currID].Data[x, 0, c] - myImg[currID].Data[x, y - 1, c];
                        result += diff * diff;
                    }
                    else
                    {
                        diff = myImg[currID].Data[x, y + 1, c] - myImg[currID].Data[x, y - 1, c];
                        result += diff * diff;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return result;
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
                    energy[i, j, newID] = energy[i, j, currID];
                }

                //myImg[currID][i, col] = new Bgr(Color.Red);

                // Move [col + 1..old width - 1] to left
                for (int j = col; j < currWidth - 1; ++j)
                {
                    myImg[newID][i, j] = myImg[currID][i, j + 1];
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
                    col = newCol;
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

            // show the temp img
            // pictureBoxTar.Image = myImg[currID].ToBitmap();
        }

        private void carveHorizontalSeam()
        {
            int currID = id % 2;
            // Calculate energy matrix with DP
            for (int j = 0; j < currWidth; ++j)
            {
                for (int i = 0; i < currHeight; ++i)
                {
                    if (j > 0)
                    {
                        horSeamMat[i, j, currID] = energy[i, j, currID] + horSeamMat[i, j - 1, currID];
                        if (i > 0)
                        {
                            horSeamMat[i, j, currID] = Math.Min(energy[i, j, currID] + horSeamMat[i - 1, j - 1, currID],
                                horSeamMat[i, j, currID]);
                        }
                        if (i < currHeight - 1)
                        {
                            horSeamMat[i, j, currID] = Math.Min(energy[i, j, currID] + horSeamMat[i + 1, j - 1, currID],
                                horSeamMat[i, j, currID]);
                        }
                    }
                    else
                    {
                        horSeamMat[i, j, currID] = energy[i, j, currID];
                    }
                }
            }

            ++id;
            int newID = id % 2;

            // Get the row No. of the seam on the last column
            int row = 0, minMatrix = horSeamMat[0, currWidth - 1, currID];
            for (int i = 1; i < currHeight; ++i)
            {
                if (horSeamMat[i, currWidth - 1, currID] < minMatrix)
                {
                    // update min
                    row = i;
                    minMatrix = horSeamMat[i, currWidth - 1, currID];
                }
            }
            horSeam[currWidth - 1] = row;

            // Get row No. on the other columns and get new image and energy
            for (int j = currWidth - 1; j >= 0; --j)
            {
                // Copy [0..row - 1]
                for (int i = 0; i < row; ++i)
                {
                    myImg[newID][i, j] = myImg[currID][i, j];
                    // myImgGray[newID][i, j] = myImgGray[currID][i, j];
                    energy[i, j, newID] = energy[i, j, currID];
                }

                // myImg[currID][row, j] = new Bgr(Color.Red);

                // Move [row + 1..old height - 1] to left
                for (int i = row; i < currHeight - 1; ++i)
                {
                    myImg[newID][i, j] = myImg[currID][i + 1, j];
                    // myImgGray[newID][i, j] = myImgGray[currID][i + 1, j];
                    energy[i, j, newID] = energy[i + 1, j, currID];
                }

                // Get next row No.
                if (j > 0)
                {
                    int newRow = row;
                    minMatrix = horSeamMat[newRow, j - 1, currID];
                    if (row > 0 && horSeamMat[row - 1, j - 1, currID] < minMatrix)
                    {
                        newRow = row - 1;
                        minMatrix = horSeamMat[row - 1, j - 1, currID];
                    }
                    if (row < currHeight - 1 && horSeamMat[row + 1, j - 1, currID] < minMatrix)
                    {
                        newRow = row + 1;
                    }
                    horSeam[j - 1] = newRow;
                    row = newRow;
                }
            }

            // Calculate other energy at point near the seam
            // The energy of most points does not change
            for (int j = 0; j < currWidth; ++j)
            {
                for (int i = horSeam[j] - 1; i <= horSeam[j]; ++i)
                {
                    // Valid point in the new image
                    if (i >= 0 && i < currHeight - 1)
                    {
                        energy[i, j, newID] = getEnergy(i, j);
                    }
                }
            }

            // show the temp img
            // pictureBoxTar.Image = myImg[currID].ToBitmap();
        }

        // Enlarge the seam
        private void enlargeVerticalSeam()
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
                    energy[i, j, newID] = energy[i, j, currID];
                }

                //myImg[currID][i, col] = new Bgr(Color.Red);

                // Move [col + 1..old width - 1] to left
                for (int j = col; j < currWidth - 1; ++j)
                {
                    myImg[newID][i, j] = myImg[currID][i, j + 1];
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
                    col = newCol;
                }
            }

            verSeams.Push(verSeam.ToArray<int>());

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

            // show the temp img
            // pictureBoxTar.Image = myImg[currID].ToBitmap();
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

                    Debug.WriteLine(String.Format("Seam No.{0} - Vertical - Done.", id));
                }
            }
            else
            {
                int newTarWidth = tarWidth - currWidth - currWidth;
                while (currWidth > newTarWidth)
                {
                    enlargeVerticalSeam();
                    --currWidth;

                    Debug.WriteLine(String.Format("Seam No.{0} - Vertical - Done.", id));
                }

                foreach (int[] seam in verSeams)
                {
                    int currID = id % 2;
                    ++id;
                    int newID = id % 2;

                    for (int i = 0; i < currHeight; ++i)
                    {
                        int col = seam[i];
                        for (int j = 0; j < col; ++j)
                        {
                            myImg[newID][i, j] = myImg[currID][i, j];
                        }

                    }
                }
            }

            if (currHeight > tarHeight)
            {
                while (currHeight > tarHeight)
                {
                    carveHorizontalSeam();
                    --currHeight;

                    Debug.WriteLine(String.Format("Seam No.{0} - Horizontal - Done.", id));
                }
            }

            int _currID = id % 2;
            Image<Bgr, Byte> tarImg = new Image<Bgr, Byte>(tarWidth, tarHeight);
            for (int i = 0; i < tarHeight; ++i)
                for (int j = 0; j < tarWidth; ++j)
                    tarImg[i, j] = myImg[_currID][i, j];
            pictureBoxTar.Image = tarImg.ToBitmap();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // Load the image
                srcImg = new Image<Bgr, Byte>(openFile.FileName);
                srcWidth = srcImg.Width;
                srcHeight = srcImg.Height;

                // Display the image. 
                // I(x, y, {B,G,R}): img.Data[x, y, {0,1,2}]. x: row No. y: col No.
                pictureBoxSrc.Image = srcImg.ToBitmap();
                textBoxHeight.Text = Convert.ToString(srcHeight);
                textBoxWidth.Text = Convert.ToString(srcWidth);

                // Output debug info
                Debug.WriteLine(String.Format("Name:{0}, Height:{1}, Width:{2}", openFile.FileName, srcHeight, srcWidth));
                //Debug.WriteLine(String.Format("I(0, 499): B:{0}, G:{1}, R:{2}",
                //    myImg[0].Data[0, 499, 0], myImg[0].Data[0, 499, 1], myImg[0].Data[0, 499, 2]));
            }
        }

        private void buttonResize_Click(object sender, EventArgs e)
        {
            // Initialize
            id = 0;
            int currID = id % 2;
            currHeight = srcHeight;
            currWidth = srcWidth;
            seamMap = new int[srcHeight, srcWidth];
            verSeams.Clear();
            horSeams.Clear();

            tarWidth = Convert.ToInt32(textBoxWidth.Text);
            tarHeight = Convert.ToInt32(textBoxHeight.Text);
            Debug.WriteLine(String.Format("Begin: {0}x{1} -> {2}x{3}", srcHeight, srcWidth, tarHeight, tarWidth));
            int height = Math.Max(srcHeight, tarHeight), width = Math.Max(srcWidth, tarWidth);

            myImg[0] = new Image<Bgr, Byte>(width, height);
            for (int i = 0; i < srcHeight; ++i)
            {
                for (int j = 0; j < srcWidth; ++j)
                {
                    myImg[0][i, j] = srcImg[i, j];
                }
            }
            myImg[1] = myImg[0].Copy();

            // Get energy value
            energy = new int[height, width, 2];
            verSeamMat = new int[height, width, 2];
            horSeamMat = new int[height, width, 2];
            verSeam = new int[height];
            horSeam = new int[width];
            for (int i = 0; i < srcHeight; ++i)
            {
                for (int j = 0; j < srcWidth; ++j)
                {
                    energy[i, j, currID] = getEnergy(i, j);
                }
            }
            Debug.WriteLine("Energy value done.");

            //Image<Gray, Byte> energyImg = new Image<Gray, Byte>(srcWidth, srcHeight);
            //for (int i = 0; i < srcHeight; ++i)
            //{
            //    for (int j = 0; j < srcWidth; ++j)
            //    {
            //        energyImg[i, j] = new Gray(energy[i, j, currID] / 4);
            //    }
            //}
            //pictureBoxTar.Image = energyImg.ToBitmap();

            resize();
        }
    }
}
