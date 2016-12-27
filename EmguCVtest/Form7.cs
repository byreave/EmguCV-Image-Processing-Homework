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
    public partial class Form7 : Form
    {
        Image<Bgr, Byte> srcImg;
        Image<Bgr, Byte> grayImg;
        public Form7()
        {
            InitializeComponent();
        }

        public void dilateImg(Image<Bgr, Byte> img , int iter)
        {
            Image<Bgr, Byte> dstImg = img.CopyBlank();
            Point anchor = new Point(-1, -1);
            MCvScalar scalar = new MCvScalar();
            CvInvoke.Dilate(img, dstImg, null, anchor ,iter, BorderType.Default, scalar);
            imageBox3.Image = dstImg;
        }

        public void erodeImg(Image<Bgr, Byte> img, int iter)
        {
            Image<Bgr, Byte> dstImg = img.CopyBlank();
            Point anchor = new Point(-1, -1);
            MCvScalar scalar = new MCvScalar();
            CvInvoke.Erode(img, dstImg, null, anchor, iter, BorderType.Default, scalar);
            imageBox4.Image = dstImg;
        }

        public void openImg(Image<Bgr, Byte> img , int iter)
        {
            Image<Bgr, Byte> dstImg = img.CopyBlank();
            Point anchor = new Point(-1, -1);
            MCvScalar scalar = new MCvScalar();
            CvInvoke.Erode(img, dstImg, null, anchor, iter, BorderType.Default, scalar);
            CvInvoke.Dilate(dstImg, dstImg, null, anchor, iter, BorderType.Default, scalar);
            imageBox4.Image = dstImg;

        }

        public void closeImg(Image<Bgr, Byte> img, int iter)
        {
            Image<Bgr, Byte> dstImg = img.CopyBlank();
            Point anchor = new Point(-1, -1);
            MCvScalar scalar = new MCvScalar();
            CvInvoke.Dilate(img, dstImg, null, anchor, iter, BorderType.Default, scalar);
            CvInvoke.Erode(dstImg, dstImg, null, anchor, iter, BorderType.Default, scalar);
            imageBox3.Image = dstImg;
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
            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            UMat dst = new UMat();
            CvInvoke.CvtColor(srcImg, uimage, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);

            grayImg = uimage.ToImage<Bgr, Byte>();

            imageBox2.Image = grayImg;
            
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if(srcImg == null)
            {
                MessageBox.Show("请选择图片！");
                return;
            }
            if(radioButton1.Checked)
            {
                dilateImg(srcImg, trackBar1.Value);
            }
            else
            {
                dilateImg(grayImg, trackBar1.Value);
            }
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (srcImg == null)
            {
                MessageBox.Show("请选择图片！");
                return;
            }
            if (radioButton1.Checked)
            {
                erodeImg(srcImg, trackBar2.Value);
            }
            else
            {
                erodeImg(grayImg, trackBar2.Value);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (srcImg == null)
            {
                MessageBox.Show("请选择图片！");
                return;
            }
            if (radioButton1.Checked)
            {
                openImg(srcImg, trackBar1.Value);
            }
            else
            {
                openImg(grayImg, trackBar1.Value);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (srcImg == null)
            {
                MessageBox.Show("请选择图片！");
                return;
            }
            if (radioButton1.Checked)
            {
                closeImg(srcImg, trackBar2.Value);
            }
            else
            {
                closeImg(grayImg, trackBar2.Value);
            }
        }
    }
}
