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
    public partial class Form4 : Form
    {
        Image<Bgr, Byte> srcImg;
        public Form4()
        {
            InitializeComponent();
        }

        public void findCircle(Image<Bgr, Byte> img, double circleAccumulatorThreshold = 120)
        {
            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);
            double cannyThreshold = 180.0;
            //double circleAccumulatorThreshold = 150;
            CircleF[] circles = CvInvoke.HoughCircles(uimage, HoughType.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, 5);
            Image<Bgr, Byte> circleImage = img.CopyBlank();
            foreach (CircleF circle in circles)
                circleImage.Draw(circle, new Bgr(Color.Brown), 2);
            circleImageBox.Image = circleImage;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            Openfile.Filter = "图片|*.jpg;*.bmp;*.png";
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                srcImg = new Image<Bgr, byte>(Openfile.FileName);
                //sourceImg = new Mat(Openfile.FileName, LoadImageType.Color);
                imageBox1.Image = srcImg;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            findCircle(srcImg, trackBar1.Value);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            thresholdLbl.Text = trackBar1.Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
        }
    }
}
