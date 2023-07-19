using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MotionDetecting
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
            Do2();
        }
        //public void Do1()
        //{
        //    int thresh = 25;
        //    int max_diff = 5;
        //    Mat a = new Mat();
        //    Mat b = new Mat();

        //    using (VideoCapture cap = new VideoCapture(0))
        //    {
        //        cap.Set(VideoCaptureProperties.FrameWidth, 600);    // 비디오 영상 크기 조절
        //        cap.Set(VideoCaptureProperties.FrameHeight, 400);

        //        if (cap.IsOpened()) // 만약 카메라에서 사진을 불러왔으면
        //        {
        //            cap.Read(a);    // 읽어라

        //            while (true)    // 무한루프
        //            {
        //                cap.Read(b);    // 읽은 1장의 사진을 b에 저장해라.
        //                if (b.Empty()) break;   // 만약 b가 비어있으면 while문을 뿌셔라

        //                using (Mat a_gray = new Mat(), b_gray = new Mat())
        //                using (Mat diff1 = new Mat(), diff_t = new Mat())

        //                using (Mat k = Cv2.GetStructuringElement(MorphShapes.Cross, new OpenCvSharp.Size(3, 3))) // 모폴로지 연산. 행렬값을 k에 저장. 
        //                using (Mat diff = new Mat())
        //                {
        //                    Cv2.CvtColor(a, a_gray, ColorConversionCodes.BGR2GRAY);
        //                    Cv2.CvtColor(b, b_gray, ColorConversionCodes.BGR2GRAY);

        //                    Cv2.Absdiff(a_gray, b_gray, diff1);     // a_gray와 b_gray 사이의 절대 차이를 계산. ( 출력값 있음 )
        //                    Cv2.Threshold(diff1, diff_t, thresh, 255, ThresholdTypes.Binary);   // 차이를 이진화 해서 출력.

        //                    Cv2.MorphologyEx(diff_t, diff, MorphTypes.Open, k); // 모폴리지 사용으로 잡티 제거

        //                    int diff_cnt = Cv2.CountNonZero(diff);
        //                    if (diff_cnt > max_diff)
        //                    {
        //                        Cv2.MinMaxLoc(diff, out _, out _, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc);
        //                        Cv2.Rectangle(b, minLoc, maxLoc, Scalar.Green, 2);

        //                        Cv2.PutText(b, "Motion detected!!", new OpenCvSharp.Point(10, 30), HersheyFonts.Duplex, 0.5, Scalar.Red);
        //                    }

        //                    Cv2.ImShow("motion", b);

        //                    a = b;

        //                    if (Cv2.WaitKey(1) == 27)
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //}
        public void Do2()
        {
            VideoCapture video = new VideoCapture(0);
            Mat a = new Mat();
            Mat b = new Mat();

            Mat morphElement = Cv2.GetStructuringElement(MorphShapes.Cross, new OpenCvSharp.Size(3, 3));

            while (Cv2.WaitKey(33) != 'q')
            {
                video.Read(a);
                video.Read(b);

                Mat aGray = new Mat();
                Mat bGray = new Mat();
                Mat abDiff = new Mat();
                Mat abDiffBinary = new Mat();
                Mat morph = new Mat();

                Cv2.CvtColor(a, aGray, ColorConversionCodes.BGR2GRAY);  //a를 gray로 변환
                Cv2.CvtColor(b, bGray, ColorConversionCodes.BGR2GRAY);  //b를 gray로 변환
                Cv2.Absdiff(aGray, bGray, abDiff);  // a, b의 절대치의 차이를 구함
                Cv2.Threshold(abDiff, abDiffBinary, 25, 255, ThresholdTypes.Binary);    // 절대치 차이를 2진화
                Cv2.MorphologyEx(abDiffBinary, morph, MorphTypes.Open, morphElement);   // 모폴로지 변환 -> 잡티 제거

                /* 모든 contour를 포함하는 최소 사각형 그리기 */
                OpenCvSharp.Point[][] contours = Cv2.FindContoursAsArray(morph, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                if (contours.Length > 0 ) // contours가 없으면 사각형 그리기 하지 않음. 
                {
                    OpenCvSharp.Rect boundingRect = Cv2.BoundingRect(contours[0]); // 사각형이 있다면 첫번째 사각형 저장. 
                    foreach (var contour in contours)   
                    {
                        OpenCvSharp.Rect rect = Cv2.BoundingRect(contour);

                        boundingRect = rect | boundingRect; // 두개의 사각형을 포함하는 제일 작은 사각형 리턴. 
                    }
                    Cv2.Rectangle(b, boundingRect, Scalar.Green, 3);    // 사각형 그리기 
                }
                Cv2.ImShow("b", b); // 출력
                Cv2.ImShow("morph", morph); // 출력
            }
            Cv2.DestroyAllWindows();
        }
    }
}

