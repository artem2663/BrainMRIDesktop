using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace BrainMRIDesktop
{
    public partial class AddNewCtrl : UserControl
    {
        IDataManager db;
        ImageCtrl imageCtrlPrimary;
        ImageCtrl imageCtrlSecondary;


        public AddNewCtrl()
        {
            InitializeComponent();

            db = new FileManager();
            imageCtrlPrimary = new ImageCtrl(this);
            imageCtrlSecondary = new ImageCtrl(this);
            imageGridPrimary.Children.Add(imageCtrlPrimary);
            imageGridSecondary.Children.Add(imageCtrlSecondary);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            AgeTxtBox.Text = "";
            maleRadio.IsChecked = false;
            femaleRadio.IsChecked = false;
            checkDatePicker.Text = "";

            imageCtrlPrimary.Clear();
            imageCtrlSecondary.Clear();
        }



        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = (MainWindow)Window.GetWindow(this);
            ResultCtrl resCtrl = new ResultCtrl();
            parentWindow.resGrid.Children.Add(resCtrl);

            int age = Convert.ToInt16(AgeTxtBox.Text);
            byte gender = 0;
            if (maleRadio.IsChecked == true)
                gender = 1;
            if (femaleRadio.IsChecked == true)
                gender = 0;

            await Task.Delay(1000);
            
            Dictionary<string,int> result = Clasifier.PredictClass(imageCtrlPrimary.dicomList, age, gender);

            string disease = result.First().Key;
            int diseaseProb = result.First().Value;


            byte diseaseID = (byte)(
                disease == "Астроцитома (ступінь 2)" ? 0 :
                disease == "Гліобластома" ? 1 : 
                disease == "Астроцитома (ступінь 3)" ? 2 : 3);


            DateTime medDate = Convert.ToDateTime(checkDatePicker.ToString());
            db.SetPatient(age, gender, medDate, diseaseID, diseaseProb);

            int di3 = 100 - result.First().Value - result.Last().Value;
            resCtrl.di1Name.Text = disease;
            resCtrl.di2Name.Text = result.Last().Key;
            resCtrl.progressDi1Prc.Value = result.First().Value;
            resCtrl.progressDi2Prc.Value = result.Last().Value;
            resCtrl.progressDi3Prc.Value = di3;
            resCtrl.di1Prc.Text = String.Concat(result.First().Value, "%");
            resCtrl.di2Prc.Text = String.Concat(result.Last().Value, "%");
            resCtrl.di3Prc.Text = String.Concat(di3, "%"); ;
            

            resCtrl.loadingMes.Visibility = Visibility.Hidden;
            resCtrl.realResGrid.Visibility = Visibility.Visible;
        }

        private void FirstImageTab_Click(object sender, RoutedEventArgs e)
        {
            imageCtrlSecondary.Visibility = Visibility.Hidden;
            imageCtrlPrimary.Visibility = Visibility.Visible;

            btnPrimary.Opacity = 1;
            btnSecondary.Opacity = 0.5;

        }

        private void SecondImageTab_Click(object sender, RoutedEventArgs e)
        {
            imageCtrlSecondary.Visibility = Visibility.Visible;
            imageCtrlPrimary.Visibility = Visibility.Hidden;

            btnPrimary.Opacity = 0.5;
            btnSecondary.Opacity = 1;

        }
    }

    
}
