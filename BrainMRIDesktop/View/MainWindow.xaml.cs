using System.Windows;


namespace BrainMRIDesktop
{

    public partial class MainWindow : Window
    {

        PatientsCtrl patientsCtrl;
        AddNewCtrl newPatientCtrl;

        public MainWindow()
        {
            patientsCtrl = new PatientsCtrl();
            newPatientCtrl = new AddNewCtrl();
            InitializeComponent();

        }

        

        private void AddPatient_Selected(object sender, RoutedEventArgs e)
        {
            renderForm.Children.Clear();
            renderForm.Children.Add(newPatientCtrl);
        }

        private void PatientList_Selected(object sender, RoutedEventArgs e)
        {
            renderForm.Children.Clear();
            renderForm.Children.Add(patientsCtrl);

        }





    }
}
