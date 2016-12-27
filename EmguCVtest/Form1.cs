using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace EmguCVtest
{
    public partial class Form1 : Form
    {
        private Mat sourceImg;
        private Mat destImg;
        private Mat grayImg;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            Openfile.Filter = "图片|*.jpg;*.bmp;*.png";

            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                Image<Bgr, Byte> My_Image = new Image<Bgr, byte>(Openfile.FileName);
                sourceImg = new Mat(Openfile.FileName, LoadImageType.Color);
                imageBox1.Image = My_Image;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(sourceImg == null)
            {
                MessageBox.Show("还未打开图片！");
                return;
            }
            destImg = new Mat(sourceImg.Size, sourceImg.Depth, sourceImg.NumberOfChannels);
            grayImg = new Mat(sourceImg.Size, sourceImg.Depth, sourceImg.NumberOfChannels);

            /// Convert to grayscale
            CvInvoke.CvtColor(sourceImg, grayImg, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            Image<Gray, Byte> grayimg = new Image<Gray, Byte>(grayImg.Bitmap);
            histogramBox1.GenerateHistograms(grayimg, 256);
            histogramBox1.Refresh();

            //zedGraphControl1 = histogramBox1.ZedGraphControl;
            imageBox1.Image = grayimg;
            /// Apply Histogram Equalization
            CvInvoke.EqualizeHist(grayImg, destImg);
            Image<Gray, Byte> img = new Image<Gray, Byte>(destImg.Bitmap);
            histogramBox2.GenerateHistograms(img, 256);
            histogramBox2.Refresh();

            imageBox2.Image = img;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }
    }
}
