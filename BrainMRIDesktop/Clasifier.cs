using Accord.Imaging.Converters;
using Dicom.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TensorFlow;


namespace BrainMRIDesktop
{
    class Clasifier
    {

        public static Dictionary<string, int> PredictClass(List<DicomImage> dicomList, float age, int gender)
        {
            float[,,,,] image = PrepareImages(dicomList);
            TFTensor data = new[,] { { age, gender } };
            
            using (var graph = new TFGraph())
            {
                graph.Import(File.ReadAllBytes("CNNModel.pb"));
                var session = new TFSession(graph);
                var runner = session.GetRunner();
                runner.AddInput(graph["convInput"][0], image);
                runner.AddInput(graph["featInput"][0], data);
                runner.Fetch(graph["dense_4/Softmax"][0]);

                var output = runner.Run();

                TFTensor result = output[0];

                
                var r = (float[,])output[0].GetValue();

                int[] rArr = new int[4];
                Dictionary<string, int> dict = new Dictionary<string, int>();
                dict.Add("Астроцитома (ступінь 2)", Convert.ToInt16(r[0, 0] * 90));
                dict.Add("Гліобластома", Convert.ToInt16(r[0, 1] * 90));
                dict.Add("Астроцитома (ступінь 3)", Convert.ToInt16(r[0, 2] * 90));
                dict.Add("Олігодендрагліома", Convert.ToInt16(r[0, 3] * 90));

                var orderedDict = dict.OrderByDescending(x => x.Value).Take(2).ToDictionary(x => x.Key, x=> x.Value);

                return orderedDict;
            }
        }



        public static float[,,,,] PrepareImages(List<DicomImage> dicomList)
        {
            dicomList = GetDistribution(dicomList);

            List<double[,]> res = new List<double[,]>();

            foreach (DicomImage dicomImage in dicomList)
            {
                Bitmap bmp = dicomImage.RenderImage().AsSharedBitmap();
                bmp = ResizeBitmap(bmp, 256, 256);
                bmp.RotateFlip(RotateFlipType.Rotate90FlipX);

                ImageToMatrix conv = new ImageToMatrix(min: 0, max: 1);

                double[,] matrix;
                conv.Convert(bmp, out matrix);

                res.Add(matrix);
            }


            var resArr = res.ToArray();
            float[,,,,] arr = new float[1,16,256,256,1];
            for (byte i = 0; i < 16; i++)
                for (int j = 0; j < 256; j++)
                    for (int k = 0; k < 256; k++)
                        arr[0, i, j, k, 0] = (float)resArr[i][j, k];
                        

            return arr;

        }



        public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }



        private static List<DicomImage> GetDistribution(List<DicomImage> dicomList)
        {
            int n = dicomList.Count;

            List<DicomImage> dicomListNew = new List<DicomImage>();

            double x = Math.Round((n - 8) / 16.0, 6);
            double i = x + 4;

            while (Convert.ToInt16(i) <= n - 4 && dicomListNew.Count < 16)
            {
                dicomListNew.Add(dicomList[Convert.ToInt16(i)]);
                i += x;
            }

            return dicomListNew;
        }


        


    }
}
