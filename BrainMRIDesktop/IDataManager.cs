using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainMRIDesktop
{
    interface IDataManager
    {
        List<Patient> GetPatients();

        void SetPatient(int age, byte gender, DateTime medDate, byte diseaseID, int diseaseProb);

        void VerifyDiagnosis(int id, int isVerified);
    }
}
