using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrainMRIDesktop
{
    class FileManager : IDataManager
    {
        public List<Patient> GetPatients()
        {
            List<Patient> list = new List<Patient>();

            string line = "";
            StreamReader file = new StreamReader("patients");
            while ((line = file.ReadLine()) != null)
            {
                string[] strArr = line.Split('|');
                list.Add(new Patient(
                    Convert.ToInt16(strArr[0]), 
                    strArr[3], 
                    Convert.ToInt16(strArr[1]), 
                    strArr[2], 
                    strArr[4],
                    Convert.ToInt16(strArr[5])
                    )
                );
            }

            file.Close();

            return list;
        }

        public void SetPatient(int age, byte gender, DateTime medDate, byte diseaseID, int diseaseProb)
        {
            string diagnosis = "";
            if (diseaseID == 0)
                diagnosis = "Астроцитома (ступінь 2)";
            if (diseaseID == 1)
                diagnosis = "Гліобластома";
            if (diseaseID == 2)
                diagnosis = "Астроцитома (ступінь 3)";
            if (diseaseID == 3)
                diagnosis = "Олігодендрагліома";

            string genderCode = "Жін.";
            if (gender == 1)
                genderCode = "Чол.";


            var lastLine = File.ReadLines("patients");
            int id = 1;
            if (lastLine.Count() > 0)
                id = Convert.ToInt16(lastLine.Last().Split('|')[0]) + 1;

            string lines = String.Concat(id, "|", age, "|", genderCode, "|", medDate.ToString("yyyy-MM-dd"), "|", diagnosis, " - ", diseaseProb.ToString(), "% |", 0);

            StreamWriter file = File.AppendText("patients");
            file.WriteLine(lines);

            file.Close();
        }

        public void VerifyDiagnosis(int id, int isVerified)
        {
            string[] arrLine = File.ReadAllLines("patients");

            int i = 0;
            foreach(string line in arrLine)
            {
                string[] lineArr = line.Split('|');
                if (lineArr[0] == id.ToString())
                {
                    lineArr[5] = isVerified.ToString();
                    arrLine[i] = string.Join("|", lineArr);
                }
                i++;
            }
            

            File.WriteAllLines("patients", arrLine);
            
        }
    }
}
