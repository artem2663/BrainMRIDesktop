using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace BrainMRIDesktop
{
    /// <summary>
    /// Interaction logic for Patients.xaml
    /// </summary>
    public partial class PatientsCtrl : UserControl
    {
        IDataManager db;

        public PatientsCtrl()
        {
            InitializeComponent();
            db = new FileManager();
        }


        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                List<Patient> patients = db.GetPatients();

                PatientList.ItemsSource = patients;
            }
        }


        private void Verified_OnChecked(object sender, RoutedEventArgs e)
        {
            Patient patient = (Patient)((CheckBox)sender).Tag;

            int isVerified = (((CheckBox)sender).IsChecked == true) ? 1 : 0;
            db.VerifyDiagnosis(patient.ID, isVerified);
        }
    }
}
