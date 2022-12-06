using OpenCvSharp;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Core
{
    public class Video
    {
        #region Fields
        private VideoCapture video;
        private Mat frame;
        private bool isPlay = false;
        private bool canPlay = false;
        private bool isPause = true;
        private bool state = false;
        #endregion

        #region Event
        public Action<BitmapImage, BitmapImage> UpdateImage { get; set; }
        public Action<int> UpdateFrames { get; set; }
        #endregion

        public Video()
        {

        }

        public void Load(string videoFilePath)
        {
            video = new VideoCapture(videoFilePath);
            frame = new Mat();
            canPlay = true;
            isPlay = false;
        }

        public int MaxFrames()
        {
            return video.FrameCount;
        }

        public bool IsPlay()
        {
            return isPlay;
        }

        public void Play()
        {
            isPause = false;
            state = true;
        }

        public void Pause()
        {
            isPause = true;
            state = false;
        }

        public void SetFrames(int frames)
        {
            bool prevPauseState = isPause;
            isPause = true;
            video.PosFrames = frames;
            isPause = prevPauseState;
        }

        public void Previous()
        {
            bool prevPauseState = isPause;
            isPause = true;

            int prevFrames = video.PosFrames - 300;
            if (prevFrames < 0)
                prevFrames = 0;
            video.PosFrames = prevFrames;

            isPause = prevPauseState;
            if (isPause)
            {
                video.Read(frame);
                UpdateFrame();
            }
        }

        public void Next()
        {
            bool prevPauseState = isPause;
            isPause = true;

            int prevFrames = video.PosFrames + 300;
            if (prevFrames > video.FrameCount)
                prevFrames = video.FrameCount;
            video.PosFrames = prevFrames;
            isPause = prevPauseState;
            if (isPause)
            {
                video.Read(frame);
                UpdateFrame();
            }
        }

        public void PlayVideo()
        {
            if (!canPlay)
                return;
            isPlay = true;

            while (true)
            {
                if (isPause)
                    continue;

                video.Read(frame);
                if (frame.Empty())
                    break;

                if (video.PosFrames > video.FrameCount)
                    break;

                if (video.PosFrames % 24 != 0)
                    continue;

                UpdateFrame();
                Thread.Sleep(1000);
            }
        }

        private void UpdateFrame()
        {
            Mat gray = new Mat();
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
            UpdateImage?.Invoke(MatToBitmapImage(frame), MatToBitmapImage(gray));
            UpdateFrames?.Invoke(video.PosFrames);
        }

        private BitmapImage MatToBitmapImage(Mat obj)
        {
            BitmapImage resultBitmapImage;
            Bitmap tempBitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(obj);

            if (tempBitmap == null)
                return null;

            using (MemoryStream memory = new MemoryStream())
            {
                tempBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                resultBitmapImage = new BitmapImage();
                resultBitmapImage.BeginInit();
                resultBitmapImage.StreamSource = memory;
                resultBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                resultBitmapImage.EndInit();
                resultBitmapImage.Freeze();
            }
            return resultBitmapImage;
        }
    }
}
