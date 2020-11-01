using Dicom;
using Dicom.Imaging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace BrainMRIDesktop
{
    static class ImageManager
    {


        public static BitmapImage ConvertDicomToImage(DicomImage dicom)
        {
            Bitmap bmp = dicom.RenderImage().AsSharedBitmap();
            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                memory.Close();
            }

            return bitmapImage;   
        }


        


        public static MedImage GetDicomMetadata(DicomImage dicom)
        {

            DicomDataset dataset = dicom.Dataset;
            string birthDateStr = ""; 
            string seriesDateStr;
            string genderStr;
            string age = "";

            string imgType = "невідомий";
            string imgProjection = "-";
            string intervalStr = "-";

            DateTime seriesDate = new DateTime();
            DateTime birthDate = new DateTime();

            dataset.TryGetString(DicomTag.PatientBirthDate, out birthDateStr);
            dataset.TryGetString(DicomTag.SeriesDate, out seriesDateStr);
            dataset.TryGetString(DicomTag.PatientSex, out genderStr);
            dataset.TryGetString(DicomTag.SpacingBetweenSlices, out intervalStr);

            if(intervalStr != null && intervalStr.Length > 1)
                intervalStr = String.Concat(
                    Math.Round(Convert.ToDecimal(intervalStr),2).ToString(),
                    " мм");

            decimal TE = decimal.Parse(dataset.GetString(DicomTag.EchoTime), NumberStyles.Float);
            decimal TR = decimal.Parse(dataset.GetString(DicomTag.RepetitionTime), NumberStyles.Float);

            if (TE > 80 && TR > 3000)
                imgType = "FLAIR";
            else if (TE < 30 && TR < 800)
                imgType = "T1";



            if (seriesDateStr == null || seriesDateStr == "")
                dataset.TryGetString(DicomTag.StudyDate, out seriesDateStr);

            if (seriesDateStr != null && seriesDateStr.Length > 0)
                seriesDate = Convert.ToDateTime(DateTime.ParseExact(seriesDateStr, "yyyyMMdd", CultureInfo.InvariantCulture));

            if (birthDateStr != null && birthDateStr.Length > 0)
                birthDate = Convert.ToDateTime(DateTime.ParseExact(birthDateStr, "yyyyMMdd", CultureInfo.InvariantCulture));

            if (seriesDateStr != null && seriesDateStr.Length > 0 && birthDateStr != null && birthDateStr.Length > 0)
                age = ((seriesDate - birthDate).Days / 365).ToString();


            //imgTypeLbl.Content = imgType;



            return new MedImage(seriesDate, intervalStr, imgType, age, genderStr);


        }

    }
}
