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
    public partial class Form6 : Form
    {
        Image<Bgr, Byte> srcImg;
        Image<Bgr, Byte> [] dstImg;

        Byte [,] ColorIndex; //颜色表

        public Form6()
        {
            InitializeComponent();
            ColorIndex = new Byte[5,3];
            ColorIndex[0, 0] = 255;
            ColorIndex[0, 1] = 255;
            ColorIndex[0, 2] = 0;
            ColorIndex[1, 0] = 255;
            ColorIndex[1, 1] = 0;
            ColorIndex[1, 2] = 0;
            ColorIndex[2, 0] = 0;
            ColorIndex[2, 1] = 255;
            ColorIndex[2, 2] = 0;
            ColorIndex[3, 0] = 0;
            ColorIndex[3, 1] = 0;
            ColorIndex[3, 2] = 255;
            ColorIndex[4, 0] = 255;
            ColorIndex[4, 1] = 0;
            ColorIndex[4, 2] = 255;
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

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            KLbl.Text = "K的值:" + trackBar1.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(srcImg == null)
            {
                MessageBox.Show("选择图像！");
                return;
            }
            //Matrix<Single> data = new Matrix<float>(new float[3]);
            int K = trackBar1.Value;
            //遍历图像像素
            int nRows = srcImg.Rows;
            int nCols = srcImg.Cols;
            int channels = srcImg.NumberOfChannels;
            float[,] samples = new float[nRows * nCols, channels];
            Matrix<int> label = new Matrix<int>(nRows * nCols, 1);
            int i, j;
            int n = 0;
            for (i = 0 ; i < nRows ; ++i)
            {
                for (j = 0 ; j < nCols ; ++j)
                {
                    samples[n, 0] = srcImg.Data[i, j, 0];
                    samples[n, 1] = srcImg.Data[i, j, 1];
                    samples[n, 2] = srcImg.Data[i, j, 2];
                    n++;
                }
            }
            Matrix<Single> sampleMatrix = new Matrix<float>(samples);
            //计算出label
            CvInvoke.Kmeans(sampleMatrix, K, label, new MCvTermCriteria(10, 1.0), 2, 0);
            
            //开始分割
            dstImg = new Image<Bgr, byte>[K];
            for (i = 0 ; i < K ; ++i)
                dstImg[i] = srcImg.CopyBlank();
            //初始化计数 遍历Label
            n = 0;
            int k;
            for (i = 0 ; i < nRows ; ++i)
            {
                for (j = 0 ; j < nCols ; ++j)
                {
                    for (k = 0 ; k < channels ; ++k)
                    {
                        //不同种类对应不同颜色
                        dstImg[label[n, 0]].Data[i, j, k] = ColorIndex[label[n,0],k];
                    }
                    n++;
                }
            }
            //各个imageBox分别显示
            switch (K)
            {
                case 2:
                    imageBox2.Image = dstImg[0];
                    imageBox3.Image = dstImg[1];
                    break;
                case 3:
                    imageBox2.Image = dstImg[0];
                    imageBox3.Image = dstImg[1];
                    imageBox4.Image = dstImg[2];

                    break;
                case 4:
                    imageBox2.Image = dstImg[0];
                    imageBox3.Image = dstImg[1];
                    imageBox4.Image = dstImg[2];
                    imageBox5.Image = dstImg[3];
                    break;
                case 5:
                    imageBox2.Image = dstImg[0];
                    imageBox3.Image = dstImg[1];
                    imageBox4.Image = dstImg[2];
                    imageBox5.Image = dstImg[3];
                    imageBox6.Image = dstImg[4];

                    break;
                default:
                    MessageBox.Show("K值应为2-5之间的整数 K =" + K.ToString());
                    break;
            }

        }
    }
}
