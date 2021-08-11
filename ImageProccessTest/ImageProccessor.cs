using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ImageProccessTest
{
    public class ImageProccessor
    {
        private Mat _originMat;

        private float _key = -0.5f;

        private HashSet<float> _keyList = new HashSet<float>();

        public ImageProccessor(string path)
        {
            _originMat = Mat.FromStream(new FileStream(path, FileMode.Open), ImreadModes.Color);
        }

        public Image Convolution()
        {
            var perRate = 0.01f;
            var maxDiff = new float[] { 0f, 0f };
            var data = DualIn();
            var lastDiff = 0f;
            var diffRate = 0f;
            while (diffRate < 0.9f)
            {
                data = DualIn();
                lastDiff = GetComapreResult(_originMat, data);
                //if (maxDiff[0])
                //{

                //}
                if (lastDiff < diffRate)
                {
                    perRate = -perRate;
                }

                if (lastDiff > diffRate && _keyList.Contains(_key + perRate))
                {
                    perRate = perRate / 10;
                }
                _key += perRate;


                //if (lastDiff > diffRate)
                //{
                //    _key -= perRate;
                //    if (_keyList.Contains(_key) && _keyList.Contains(_key + perRate))
                //    {
                //        perRate = perRate / 10;
                //        continue;
                //    }
                //}
                //if (lastDiff <= diffRate)
                //{
                //    _key += perRate;
                //    if (_keyList.Contains(_key) && _keyList.Contains(_key - perRate))
                //    {
                //        perRate = perRate / 10;
                //        continue;
                //    }
                //}
                _keyList.Add(_key);
                diffRate = lastDiff;
            }
            return BitmapConverter.ToBitmap(data);
        }

        public Mat DualIn()
        {
            try
            {

                var image = new Mat(_originMat, new Rect(0, 0, _originMat.Width, _originMat.Height));
                var sizedImage = new Mat(_originMat.Rows * 2, _originMat.Cols * 2, MatType.CV_8UC3);


                float Row_B = image.Rows * 2;
                float Col_B = image.Cols * 2;



                for (int i = 2; i < Row_B - 4; i++)
                {
                    for (int j = 2; j < Col_B - 4; j++)
                    {
                        float x = i * (image.Rows / Row_B);//放大后的图像的像素位置相对于源图像的位置
                        float y = j * (image.Cols / Col_B);

                        /*if (int(x) > 0 && int(x) < image.rows - 2 && int(y)>0 && int(y) < image.cols - 2){*/
                        var w_x = new float[4];//行列方向的加权系数
                        var w_y = new float[4];
                        getW_x(w_x, x);
                        getW_y(w_y, y);

                        Vec3f temp = new Vec3f(0, 0, 0);
                        for (int s = 0; s <= 3; s++)
                        {
                            for (int t = 0; t <= 3; t++)
                            {
                                temp = temp + (image.At<Vec3b>((int)x + s - 1, (int)y + t - 1)).ToVec3f() * w_x[s] * w_y[t];
                            }
                        }
                        var item0 = (temp.Item0 > 255 ? 255 : (temp.Item0 < 0 ? 0 : temp.Item0));
                        var item1 = (temp.Item1 > 255 ? 255 : (temp.Item1 < 0 ? 0 : temp.Item1));
                        var item2 = (temp.Item2 > 255 ? 255 : (temp.Item2 < 0 ? 0 : temp.Item2));
                        sizedImage.At<Vec3b>(i, j) = new Vec3b(byte.Parse(((int)item0).ToString()),
                            byte.Parse(((int)(item1)).ToString()),
                            byte.Parse(((int)(item2)).ToString()));
                    }
                }

                return sizedImage;
            }
            catch (OpenCvSharp.OpenCvSharpException e)
            {
                throw e;
            }
        }

        //public int[] GetKernelResult(object input, object output)
        //{

        //}

        /*计算系数*/
        private void getW_x(float[] w_x, float x)
        {
            int X = (int)x;//取整数部分
            var stemp_x = new float[4];
            stemp_x[0] = 1 + (x - X);
            stemp_x[1] = x - X;
            stemp_x[2] = 1 - (x - X);
            stemp_x[3] = 2 - (x - X);

            w_x[0] = _key * Math.Abs(stemp_x[0] * stemp_x[0] * stemp_x[0]) - 5 * _key * stemp_x[0] * stemp_x[0] + 8 * _key * Math.Abs(stemp_x[0]) - 4 * _key;
            w_x[1] = (_key + 2) * Math.Abs(stemp_x[1] * stemp_x[1] * stemp_x[1]) - (_key + 3) * stemp_x[1] * stemp_x[1] + 1;
            w_x[2] = (_key + 2) * Math.Abs(stemp_x[2] * stemp_x[2] * stemp_x[2]) - (_key + 3) * stemp_x[2] * stemp_x[2] + 1;
            w_x[3] = _key * Math.Abs(stemp_x[3] * stemp_x[3] * stemp_x[3]) - 5 * _key * stemp_x[3] * stemp_x[3] + 8 * _key * Math.Abs(stemp_x[3]) - 4 * _key;
        }

        private void getW_y(float[] w_y, float y)
        {
            int Y = (int)y;
            var stemp_y = new float[4];
            stemp_y[0] = 1 + (y - Y);
            stemp_y[1] = y - Y;
            stemp_y[2] = 1 - (y - Y);
            stemp_y[3] = 2 - (y - Y);

            w_y[0] = _key * Math.Abs(stemp_y[0] * stemp_y[0] * stemp_y[0]) - 5 * _key * stemp_y[0] * stemp_y[0] + 8 * _key * Math.Abs(stemp_y[0]) - 4 * _key;
            w_y[1] = (_key + 2) * Math.Abs(stemp_y[1] * stemp_y[1] * stemp_y[1]) - (_key + 3) * stemp_y[1] * stemp_y[1] + 1;
            w_y[2] = (_key + 2) * Math.Abs(stemp_y[2] * stemp_y[2] * stemp_y[2]) - (_key + 3) * stemp_y[2] * stemp_y[2] + 1;
            w_y[3] = _key * Math.Abs(stemp_y[3] * stemp_y[3] * stemp_y[3]) - 5 * _key * stemp_y[3] * stemp_y[3] + 8 * _key * Math.Abs(stemp_y[3]) - 4 * _key;
        }

        public float GetComapreResult(Mat image1, Mat image2)
        {
            var resizeImage1 = new Mat();
            Cv2.Resize(image1, resizeImage1, image2.Size());
            var matArr1 = Cv2.Split(resizeImage1);
            var matArr2 = Cv2.Split(image2);
            float sub_data = 0;
            for (int i = 0; i < 3; i++)
            {
                sub_data += calculate(matArr1[i], matArr2[i]);
            }
            sub_data = sub_data / 3;
            return sub_data;


        }

        public float calculate(Mat image1, Mat image2)
        {
            // 灰度直方图算法
            // 计算单通道的直方图的相似值]
            var ret1 = new Mat();
            var ret2 = new Mat();
            Cv2.CalcHist(new Mat[] { image1 }, new int[] { 0 }, null, ret1, 1, new int[] { 256 }, new float[][] { new float[] { 0f, 180f }, new float[] { 0f, 256f } });
            Cv2.CalcHist(new Mat[] { image2 }, new int[] { 0 }, null, ret2, 1, new int[] { 256 }, new float[][] { new float[] { 0f, 180f }, new float[] { 0f, 256f } });
            // 计算直方图的重合度
            var degree = 0f;

            for (var i = 0; i < ret1.Rows; i++)
            {
                if (ret1.At<float>(0, i) != ret2.At<float>(0, i))
                {
                    degree += (1 - Math.Abs(ret1.At<float>(0, i) - ret2.At<float>(0, i)) / Math.Max(ret1.At<float>(0, i), ret2.At<float>(0, i)));
                }
                else
                {
                    degree += 1;
                }
            }
            degree = degree / ret1.Rows;
            return degree;
            //for i in range(len(hist1)) :
            //    if hist1[i] != hist2[i]:
            //        degree = degree + \
            //            (1 - abs(hist1[i] - hist2[i]) / max(hist1[i], hist2[i]))
            //    else:
            //        degree = degree + 1
            //degree = degree / len(hist1)
            //return degree
        }
    }
}
