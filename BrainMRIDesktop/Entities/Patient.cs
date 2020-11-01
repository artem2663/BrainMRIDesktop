using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainMRIDesktop
{
    public class Patient
    {
        public int ID { get; private set; }
        public string MedDate { get; private set; }
        public int Age { get; private set; }
        public string Gender { get; private set; }
        public string Diagnosis { get; private set; }
        public int IsVerified { get; private set; }


        public Patient(int Id, string medDate, int age, string gender, string diagnosis, int isVerified)
        {
            ID = Id;
            MedDate = medDate;
            Age = age;
            Gender = gender;
            Diagnosis = diagnosis;
            IsVerified = isVerified;
        }
    }
}
