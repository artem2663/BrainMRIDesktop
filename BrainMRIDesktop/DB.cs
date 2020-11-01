using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace BrainMRIDesktop
{
    class DB : IDataManager
    {
        SqlConnection sqlCon;

        public DB()
        {
            sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlCon"].ConnectionString);
        }

        public List<Patient> GetPatients()
        {
            List<Patient> list = new List<Patient>();
            SqlCommand cmd = new SqlCommand("prcPatientsGet", sqlCon);

            cmd.CommandType = CommandType.StoredProcedure;

            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();

            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                list.Add(
                    new Patient(
                        Convert.ToInt32(dataReader["ID"]),
                        dataReader["MedDate"].ToString(),
                        Convert.ToInt32(dataReader["Age"]),
                        dataReader["Gender"].ToString(),
                        dataReader["Disease"].ToString(),
                        0
                   )
               );
            }

            sqlCon.Close();

            return list;
        }



        public void SetPatient(int age, byte gender, DateTime medDate, byte diseaseID, int diseaseProb)
        {
            List<Patient> list = new List<Patient>();
            SqlCommand cmd = new SqlCommand("prcPatientDataSet", sqlCon);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@MedDate", medDate);
            cmd.Parameters.AddWithValue("@DiseaseID", diseaseID);

            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();

            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }


        public void VerifyDiagnosis(int id, int isVerified)
        {

        }
    }
}
