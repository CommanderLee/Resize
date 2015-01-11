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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // Load the image
                Image<Bgr, Byte> srcImg = new Image<Bgr, Byte>(openFile.FileName);

                // Display the image
                pictureBoxSrc.Image = srcImg.ToBitmap();

                // Output debug info
                Debug.WriteLine(String.Format("Name:{0}, X:{1}, Y{2}", openFile.FileName, srcImg.Width, srcImg.Height));
            }
        }
    }
}
