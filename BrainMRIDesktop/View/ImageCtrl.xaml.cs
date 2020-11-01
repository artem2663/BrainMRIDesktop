using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dicom;
using Dicom.Imaging;
using Microsoft.Win32;

namespace BrainMRIDesktop
{
    public partial class ImageCtrl : UserControl
    {
        public List<DicomImage> dicomList { get; private set; }



        string[] files;
        MedImage medImage;
        int i = 0;
        AddNewCtrl parentCtrl;

        public ImageCtrl(AddNewCtrl parent)
        {
            InitializeComponent();
            dicomList = new List<DicomImage>();
            parentCtrl = parent;
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            files = e.Data.GetData(DataFormats.FileDrop) as string[];

            RenderImages();
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Multiselect = true;

            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == true)
            {
                files = openFileDlg.FileNames;

                RenderImages();
            }
        }


        private void RenderImages()
        {
            sliderRect.Visibility = Visibility.Visible;
            sliderRect.Margin = new Thickness(30, 22, 0, 22);

            foreach (string d in files)
                dicomList.Add(new DicomImage(d));


            dicomList = dicomList.OrderBy(o => decimal.Parse(o.Dataset.GetString(DicomTag.InstanceNumber), NumberStyles.Float)).ToList();

            dragedImage.Source = ImageManager.ConvertDicomToImage(dicomList[0]);

            medImage = ImageManager.GetDicomMetadata(dicomList[0]);

            qtyLbl.Content = files.Length.ToString();
            imgTypeLbl.Content = medImage.ImgType;
            intervalLbl.Content = medImage.Interval;

            parentCtrl.AgeTxtBox.Text = medImage.Age;
            parentCtrl.checkDatePicker.Text = medImage.CreationDate.ToString();
            parentCtrl.maleRadio.IsChecked = (medImage.Gender == "M") ? true : false;
            parentCtrl.femaleRadio.IsChecked = (medImage.Gender == "F") ? true : false;
        }



        private void Slider_Scrolled(object sender, MouseWheelEventArgs e)
        {
            if (i == dicomList.Count - 1 && e.Delta < 0 || i == 0 && e.Delta > 0)
                return;

            if (e.Delta > 0)
                i--;
            else
                i++;

            double step = 365 / Convert.ToInt16(qtyLbl.Content);

            sliderRect.Margin = new Thickness(30 + i * step, 22, 0, 22);

            sliceLocLbl.Content = String.Concat(
                Math.Round(
                    decimal.Parse(dicomList[i].Dataset.GetString(DicomTag.SliceLocation), NumberStyles.Float)
                    , 1).ToString(), " мм");
            dragedImage.Source = ImageManager.ConvertDicomToImage(dicomList[i]);

        }


        public void Clear()
        {
            i = 0;
            dicomList.Clear();
            files = new string[] { };
            dragedImage.Source = null;
            imgTypeLbl.Content = "-";
            qtyLbl.Content = "-";
            intervalLbl.Content = "-";
            sliceLocLbl.Content = "-";
            sliderRect.Visibility = Visibility.Hidden;
        }

        
    }


}
