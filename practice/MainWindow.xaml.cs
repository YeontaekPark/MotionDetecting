using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenCvSharp;
using System.Collections.Generic;
using System.Threading;
using OpenCvSharp.Internal.Vectors;
using System.Diagnostics;
using System.IO.Compression;
using Emgu.CV.Structure;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Animation;

namespace practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Program program = new Program();
        }
    }

    class Program
    {
        public Program()
        {
            //Do1();
            Camera();
        }
        public void Do1()
        {
            Mat src = new Mat("./Source/picture.jpg");
            Mat gray = new Mat();
            Mat grayblur = new Mat();
            Mat binary = new Mat();
            Mat zeros = new Mat();

            Mat test = new Mat(src.Size(), src.Type(), Scalar.White);


            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, binary, 150, 255, ThresholdTypes.BinaryInv);
            Cv2.Blur(binary, binary, new OpenCvSharp.Size(3, 3), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);



            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchies;
            Cv2.FindContours(binary, out contours, out hierarchies, RetrievalModes.External, ContourApproximationModes.ApproxTC89KCOS);

            Cv2.DrawContours(test, contours, -1, Scalar.Black, -1, LineTypes.AntiAlias, null);

            Cv2.BitwiseNot(test, test); // 2진화 반전. 

            //밖이 검정색인 것을 binary에 넣고 zeros 출력해야 결과 완성
            Cv2.BitwiseAnd(src, test, zeros);


            Cv2.ImShow("zeros", zeros);
            Cv2.WaitKey(0);
        }

        public void Camera()
        {
            VideoCapture video = new VideoCapture(0);
            Mat src = new Mat();
            Mat gray = new Mat();
            Mat binary = new Mat();
            Mat binaryblur = new Mat();
            
            Mat backgroundPicture = new Mat("./Source/backgroundPicture.jpg");

            Mat dts = new Mat();

            while (Cv2.WaitKey(33) != 'q')
            {
                video.Read(src);
                
                Cv2.ImShow("src", src);           
            }

            src.Dispose();
            video.Release();
            Cv2.DestroyAllWindows();
        }
    }
}