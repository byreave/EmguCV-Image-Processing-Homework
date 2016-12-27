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
    public partial class Form8 : Form
    {
        Image<Bgr, Byte> srcImg;
        public Form8()
        {
            InitializeComponent();
        }


        public Image<Bgr, Byte> Roberts(Image<Bgr, Byte> img)
        {
            Image<Bgr, Byte> dstImg = img.CopyBlank();
            int nRows = img.Rows;
            int nCols = img.Cols;
            int channels = img.NumberOfChannels;
            for(int i = 0; i < nRows-1; ++ i)
            {
                for(int j = 0; j < nCols-1; ++ j)
                {
                    for (int k = 0; k < channels; ++ k)
                    {
                        int t1 = ( img.Data[i, j, k] - img.Data[i + 1, j + 1, k] ) * ( img.Data[i, j, k] - img.Data[i + 1, j + 1, k] );
                        int t2 = ( img.Data[i + 1, j, k] - img.Data[i, j + 1, k] ) * ( img.Data[i + 1, j, k] - img.Data[i, j + 1, k] );
                        dstImg.Data[i, j, k] = (Byte)Math.Sqrt((double)(t1 + t2));
                    }
                }
            }
            return dstImg;
        }

        public Image<Bgr, Byte> Prewitt(Image<Bgr, Byte> img)
        {
            Image<Bgr, Byte> dstImg = img.CopyBlank();
            int nRows = img.Rows;
            int nCols = img.Cols;
            int channels = img.NumberOfChannels;

            float [,]prewittx = new float[3,3]              
                {      
                    {-1,0,1},      
                    {-1,0,1},      
                    {-1,0,1}      
                };
            float[,] prewitty = new float[3,3]              
                {      
                    {1,1,1},      
                    {0,0,0},      
                    {-1,-1,-1}      
                };

            Matrix<Single> px = new Matrix<float>(prewittx);
            Matrix<Single> py = new Matrix<float>(prewitty);

            Image<Bgr, Byte> dstx = img.CopyBlank();
            Image<Bgr, Byte> dsty = img.CopyBlank();

            CvInvoke.Filter2D(img, dstx, px, new Point(-1, -1));
            CvInvoke.Filter2D(img, dsty, px, new Point(-1, -1));

            double tempx, tempy, temp;
            for (int i = 0 ; i < nRows; i++)
            {
                for (int j = 0 ; j < nCols ; j++)
                {
                    for (int k = 0 ; k < channels ; ++k)
                    {
                        tempx = dstx.Data[i, j, k];
                        tempy = dsty.Data[i, j, k];
                        temp = Math.Sqrt(tempx * tempx + tempy * tempy);
                        dstImg.Data[i, j, k] = (Byte)temp;
                    }
                }
            }    

            return dstImg;
        }

        //这个具体指什么不知道...估计是左右差分和上下差分吧 和Roberts有一点点区别
        public Image<Bgr, Byte> Difference(Image<Bgr, Byte> img)
        {
            Image<Bgr, Byte> dstImg = img.CopyBlank();
            int nRows = img.Rows;
            int nCols = img.Cols;
            int channels = img.NumberOfChannels;
            for (int i = 0 ; i < nRows - 1 ; ++i)
            {
                for (int j = 0 ; j < nCols - 1 ; ++j)
                {
                    for (int k = 0 ; k < channels ; ++k)
                    {
                        int t1 = ( img.Data[i, j, k] - img.Data[i + 1, j, k] ) * ( img.Data[i, j, k] - img.Data[i + 1, j, k] ); //左右差分
                        int t2 = ( img.Data[i , j, k] - img.Data[i, j + 1, k] ) * ( img.Data[i, j, k] - img.Data[i, j + 1, k] ); //上下差分
                        dstImg.Data[i, j, k] = (Byte)Math.Sqrt((double)( t1 + t2 ));
                    }
                }
            }
            return dstImg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            Openfile.Filter = "图片|*.jpg;*.bmp;*.png";
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                srcImg = new Image<Bgr, byte>(Openfile.FileName);
                //sourceImg = new Mat(Openfile.FileName, LoadImageType.Color);
                //进行高斯平滑 因为算子对噪声敏感
                srcImg.SmoothGaussian(3);
                imageBox1.Image = srcImg;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(srcImg == null)
            {
                MessageBox.Show("请选择原图片！");
                return;
            }
            Image<Bgr, Byte> dstImg = srcImg.CopyBlank();
            Image<Bgr, Byte> dstImg2 = srcImg.CopyBlank();

            CvInvoke.Laplacian(srcImg, dstImg, DepthType.Default);
            imageBox2.Image = dstImg; //梯度图
            CvInvoke.ConvertScaleAbs(dstImg, dstImg2, 1, 0); // 和下面一样 只是试试这种方法
            dstImg2 = srcImg.Add(dstImg2);
            imageBox3.Image = dstImg2;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (srcImg == null)
            {
                MessageBox.Show("请选择原图片！");
                return;
            }
            Image<Bgr, Byte> dstImg = srcImg.CopyBlank();
            Image<Bgr, Byte> dstImg2 = srcImg.CopyBlank();

            CvInvoke.Sobel(srcImg, dstImg, DepthType.Default, 1, 0);
            imageBox2.Image = dstImg;
            dstImg2 = srcImg.Add(dstImg);
            imageBox3.Image = dstImg2;

            //显示梯度图2
            Image<Bgr, Byte> grad_x = srcImg.CopyBlank();
            Image<Bgr, Byte> grad_y = srcImg.CopyBlank();
            Image<Bgr, Byte> abs_x = srcImg.CopyBlank();
            Image<Bgr, Byte> abs_y = srcImg.CopyBlank();
            Image<Bgr, Byte> gradImg = srcImg.CopyBlank();


            CvInvoke.Sobel(srcImg, grad_x, DepthType.Default, 1, 0);
            CvInvoke.Sobel(srcImg, grad_y, DepthType.Default, 0, 1);

            //绝对值
            CvInvoke.ConvertScaleAbs(grad_x, abs_x, 1, 0);
            CvInvoke.ConvertScaleAbs(grad_y, abs_y, 1, 0);

            //两个方向平方
            abs_x.Pow(2);
            abs_y.Pow(2);

            //结果是梯度平方
            CvInvoke.Add(abs_x, abs_y, gradImg);

            imageBox4.Image = gradImg;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (srcImg == null)
            {
                MessageBox.Show("请选择原图片！");
                return;
            }
            Image<Bgr, Byte> dstImg = srcImg.CopyBlank();
            Image<Bgr, Byte> dstImg2 = srcImg.CopyBlank();

            dstImg = Roberts(srcImg);
            imageBox2.Image = dstImg;
            dstImg2 = srcImg.Add(dstImg);
            imageBox3.Image = dstImg2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (srcImg == null)
            {
                MessageBox.Show("请选择原图片！");
                return;
            }
            Image<Bgr, Byte> dstImg = srcImg.CopyBlank();
            Image<Bgr, Byte> dstImg2 = srcImg.CopyBlank();

            dstImg = Prewitt(srcImg);
            imageBox2.Image = dstImg;
            dstImg2 = srcImg.Add(dstImg);
            imageBox3.Image = dstImg2;
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (srcImg == null)
            {
                MessageBox.Show("请选择原图片！");
                return;
            }
            Image<Bgr, Byte> dstImg = srcImg.CopyBlank();
            Image<Bgr, Byte> dstImg2 = srcImg.CopyBlank();

            dstImg = Difference(srcImg);
            imageBox2.Image = dstImg;
            dstImg2 = srcImg.Add(dstImg);
            imageBox3.Image = dstImg2;
        }
    }
}
