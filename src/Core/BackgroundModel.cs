using OpenCvSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BackgroundModel
    {
        #region Fields
        public int[,,] BackgroundArray;
        #endregion

        #region Constructor
        public BackgroundModel()
        {
            BackgroundArray = new int[480, 640, 256];
        }

        public BackgroundModel(Size size)
        {
            BackgroundArray = new int[size.Height, size.Width, 256];
        }

        public BackgroundModel(int width, int height)
        {
            BackgroundArray = new int[height, width, 256];
        }
        #endregion

        #region Public Method
        public void CreateBackground(List<string> videoFiles)
        {
            for (int index = 0; index < videoFiles.Count; index++)
            {
                using (VideoCapture video = new VideoCapture(videoFiles[index]))
                {
                    Mat frame = new Mat();
                    while (true)
                    {
                        video.Read(frame);
                        if (frame.Empty())
                            break;

                        if (video.PosFrames > video.FrameCount)
                            break;

                        if (video.PosFrames % 24 != 0)
                            continue;

                        Cv2.CvtColor(frame, frame, ColorConversionCodes.BGR2GRAY);
                        AppendPixel(frame);
                    }
                }
            }
        }

        private unsafe void AppendPixel(Mat img)
        {
            if (img.Empty())
                return;
            byte* data = (byte*)img.DataPointer;
            for (int y = 0; y < img.Rows; ++y)
            {
                for (int x = 0; x < img.Cols; ++x)
                {
                    int pixel = data[y * img.Step() + x * img.ElemSize()];
                    Console.WriteLine($"{x}, {y} : {pixel}");
                    BackgroundArray[y, x, pixel]++;
                }
            }
        }

        public void SubtractImage(Mat img)
        {

        }
        #endregion
    }
}
