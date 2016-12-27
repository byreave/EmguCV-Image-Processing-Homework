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
    public partial class Form5 : Form
    {
        Image<Bgr, Byte> srcImg;
        Image<Bgr, Byte> dstImg;
        Image<Bgr, Byte> dstImg1;
        Image<Bgr, Byte> dstImg2;

        public Form5()
        {
            InitializeComponent();
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
            if(srcImg == null)
            {
                MessageBox.Show("请选择图片！");
                return;
            }
            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            UMat dst = new UMat();
            CvInvoke.CvtColor(srcImg, uimage, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);

            CvInvoke.Threshold(uimage, dst, 0, 255, ThresholdType.Otsu);

            dstImg = srcImg.CopyBlank();

            dstImg = dst.ToImage<Bgr, Byte>();

            imageBox4.Image = dstImg;
            //MessageBox.Show(dstImg.Data[10, 10, 0].ToString());
            dstImg1 = srcImg.CopyBlank();
            dstImg2 = srcImg.CopyBlank();

            //遍历图像像素已完成分割
            int nRows = dstImg.Rows;
            int nCols = dstImg.Cols;
            int channels = dstImg.NumberOfChannels;
            int i, j, k;

            for(i = 0; i < nRows; ++ i)
            {
                for(j = 0; j < nCols; ++ j)
                {
                    //如果二值化图像中是白色的像素点就放在dstImg1中 反之dstImg2
                    if (dstImg.Data[i, j, 0] == 255)
                    {
                        for (k = 0 ; k < channels ; ++k)
                        {
                            dstImg1.Data[i, j, k] = srcImg.Data[i, j, k];
                        }
                    }
                    else
                    {
                        for (k = 0 ; k < channels ; ++k)
                        {
                            dstImg2.Data[i, j, k] = srcImg.Data[i, j, k];
                        }
                    }
                    
                }
            }
            //显示图像
            
            imageBox2.Image = dstImg1;
            imageBox3.Image = dstImg2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
        }
    }
}
