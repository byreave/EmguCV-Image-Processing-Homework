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
using Emgu.CV.XFeatures2D;
using Emgu.CV.Features2D;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace EmguCVtest
{
    public partial class VideoProcessor : Form
    {
        VideoCapture vc;
        Mat frame, siftFrame;
        double rate, frameCount, currentFrame, numOfKeyPoints;
        bool pause;

        public VideoProcessor()
        {
            InitializeComponent();
            pause = false;
            frame = new Mat();
            siftFrame = new Mat();
            numOfKeyPoints = 0;
            currentFrame = 0;
            Application.Idle += Application_Idle;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            if(vc != null && !pause)
            {
                SIFT detector = new SIFT();

                Emgu.CV.Util.VectorOfKeyPoint keypoints = new Emgu.CV.Util.VectorOfKeyPoint();

                vc.Read(frame);
                System.Threading.Thread.Sleep((int)(1000.0 / rate - 5));
                //imageBox1.Image = frame;

                frLbl.Text = rate.ToString();
                cfLbl.Text = currentFrame.ToString();
                fcLbl.Text = frameCount.ToString();

                vc.Read(frame);
                imageBox1.Image = frame;
                //detector.Detect(frame);
                detector.DetectRaw(frame, keypoints);
                numOfKeyPoints = keypoints.Size;
                kpLbl.Text = numOfKeyPoints.ToString();
                Features2DToolbox.DrawKeypoints(frame, keypoints, siftFrame, new Bgr(Color.Blue));
                imageBox2.Image = siftFrame;
                GC.Collect();

                currentFrame++;

                if (currentFrame >= frameCount)
                {
                    pause = true;
                    button4.Enabled = false;
                }
            }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            Openfile.Filter = "视频|*.avi;*.mp4;";
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                vc = new VideoCapture(Openfile.FileName);
                if (vc.IsOpened)
                    MessageBox.Show("打开视频成功!");
                else
                    MessageBox.Show("打开视频失败!");
            }
            rate = vc.GetCaptureProperty(CapProp.Fps);
            frameCount = vc.GetCaptureProperty(CapProp.FrameCount); 
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            pause = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pause = false;
        }
    }
}
