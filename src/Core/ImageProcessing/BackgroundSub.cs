using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenCvSharp.ConnectedComponents;

namespace Core.ImageProcessing
{
    public class BackgroundSub
    {
        private BackgroundSubtractorMOG2 _mog;
        private readonly string preVideoPath = @"";
        private Mat _remove = new Mat();
        public BackgroundSub()
        {

        }

        public void PreTrain()
        {

        }

        public void Apply()
        {
            // _mog.Apply();
        }

        /// <summary>
        /// OpenCVSharp4 버전으로 변경 필요함
        /// </summary>
        /// <param name="obj"></param>
        private void RunBlobDetection(Mat obj)
        {
            _mog.Apply(obj, _remove);
            //CvBlobs blobs = new CvBlobs();
            //blobs.Label(_remove);
            //foreach (var pair in blobs)
            //{
            //    CvBlob blob = pair.Value;
            //    if (blob.Area < 100)
            //        continue;
            //    Cv2.Rectangle(obj, new OpenCvSharp.Point(blob.MinX, blob.MinY), new OpenCvSharp.Point(blob.MaxX, blob.MaxY), Scalar.Red, 1, LineTypes.AntiAlias);
            //}
        }
    }
}
