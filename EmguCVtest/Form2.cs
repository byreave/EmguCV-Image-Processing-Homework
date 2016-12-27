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
    public partial class Form2 : Form
    {
        static bool hasSpare = false;
        static double rand1, rand2;
        Image<Bgr, Byte> sourceImg;
        Random r = new Random();
        public Form2()
        {
            InitializeComponent();
        }
        public double  generateGaussianNoise()  
        {  
            
            if(hasSpare)  
            {  
                hasSpare = false;  
                return Math.Sqrt(rand1) * Math.Sin(rand2);  
            }  
   
            hasSpare = true;  
            
            rand1 = r.NextDouble();
            //rand1 =  / ((double) );  
            if(rand1 < 1e-100) rand1 = 1e-100;  
            rand1 = -2 * Math.Log(rand1);  
            rand2 = r.NextDouble() * 2 * Math.PI;  
   
            return Math.Sqrt(rand1) * Math.Cos(rand2);  
        }  
        public void AddGaussianNoise(Image<Bgr, Byte> I)  
        {  
            // accept only char type matrices  
            //if(I.Depth != 1);  
  
            int channels = I.NumberOfChannels;  
  
            int nRows = I.Rows;  
            int nCols = I.Cols;
            int i,j, k;
            //int count = 0;
            for (i = 0 ; i < nRows ; ++i)
            {
                for (j = 0 ; j < nCols ; ++j)
                {
                    for (k = 0 ; k < channels ; ++k)
                    {
                        double val = I.Data[i, j, k] + generateGaussianNoise() * 128;
                        if (val < 0)
                            val = 0;
                        if (val > 255)
                            val = 255;
                        I.Data[i, j, k] = (byte)val;
                    }
                }
            }
        }

        public void AddPepperNoise(Image<Bgr, Byte> img, double num = 0.03)
        {
            Random ran = new Random();
            int nRows = img.Rows;
            int nCols = img.Cols;
            int channels = img.NumberOfChannels;
            int i, j, k;
            for(i = 0; i < nRows; ++ i)
            {
                for(j = 0; j < nCols; ++ j)
                {
                    if(ran.NextDouble() < num)
                    {
                        for(k = 0; k < channels; ++ k)
                        {
                            img.Data[i, j, k] = 255;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            Openfile.Filter = "图片|*.jpg;*.bmp;*.png";

            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                sourceImg = new Image<Bgr, byte>(Openfile.FileName);
                //sourceImg = new Mat(Openfile.FileName, LoadImageType.Color);
                imageBox1.Image = sourceImg;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(sourceImg == null)
            {
                MessageBox.Show("请选择图片!");
                return;
            }
            Image<Bgr, Byte> gausImg = new Image<Bgr, byte>(sourceImg.ToBitmap());
            
            AddGaussianNoise(gausImg);
            //Image<Bgr, Byte> gausImg = sourceImg;
            imageBox2.Image = gausImg;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (sourceImg == null)
            {
                MessageBox.Show("请选择图片!");
                return;
            }
            Image<Bgr, Byte> pepperImg = new Image<Bgr, byte>(sourceImg.ToBitmap());
            double num = trackBar1.Value / 100.0;
            AddPepperNoise(pepperImg, num);
            //Image<Bgr, Byte> gausImg = sourceImg;
            imageBox3.Image = pepperImg;
            
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            pepperLbl.Text = ( trackBar1.Value / 100.0 ).ToString();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (sourceImg == null)
            {
                MessageBox.Show("请选择图片!");
                return;
            }
            Image<Bgr, Byte> gausImg = new Image<Bgr, byte>(imageBox1.Image.Bitmap);
            int size = trackBar2.Value * 2 + 3;

            gausImg._SmoothGaussian(size);
            imageBox2.Image = gausImg;
            label2.Text = "高斯滤波";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            if (sourceImg == null)
            {
                MessageBox.Show("请选择图片!");
                return;
            } 
            Image<Bgr, Byte> blurImg = new Image<Bgr, byte>(imageBox1.Image.Bitmap);
            //blurImg = blurImg.SmoothBlur(blurImg.Width, blurImg.Height);
            Point anchor = new Point(-1, -1);
            int size = trackBar2.Value * 2 + 3;

            CvInvoke.Blur(blurImg, blurImg, new Size(size,size), anchor);
            
            imageBox3.Image = blurImg;
            label3.Text = "均值滤波";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (sourceImg == null)
            {
                MessageBox.Show("请选择图片!");
                return;
            }
            Image<Bgr, Byte> blurImg = new Image<Bgr, byte>(imageBox1.Image.Bitmap);
            //blurImg = blurImg.SmoothBlur(blurImg.Width, blurImg.Height);
            int size = trackBar2.Value * 2 + 3;
            CvInvoke.MedianBlur(blurImg, blurImg, size);

            imageBox4.Image = blurImg;
            label4.Text = "中值滤波";
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            label6.Text = "模板大小:" + ( trackBar2.Value * 2 + 3 ).ToString();
        }  
        
    }
}
