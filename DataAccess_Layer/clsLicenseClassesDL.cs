using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsDataAccessLayer
{
    public class clsLicenseClassesDL
    {
        static public DataTable getListLicenseClasses()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = "Select * From LicenseClasses";

            SqlCommand command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    dataTable.Load(reader);

                reader.Close();
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }



            return dataTable;
        }

        static public bool Find(int licenseClassID, ref string className, ref string classDescription,
            ref byte minimumAllowedAge, ref byte defaultValidityLength, ref decimal classFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From LicenseClasses Where LicenseClassID = @licenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);
            
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    className = (string)reader["ClassName"];
                    classDescription = (string)reader["ClassDescription"];
                    minimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    defaultValidityLength = (byte)reader["DefaultValidityLength"];
                    classFees = (decimal)reader["classFees"];

                    reader.Close();

                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool Find(string className, ref int licenseClassID, ref string classDescription,
            ref byte minimumAllowedAge, ref byte defaultValidityLength, ref decimal classFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From LicenseClasses Where ClassName = @className";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@className", className);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    licenseClassID = (int)reader["LicenseClassID"];
                    classDescription = (string)reader["ClassDescription"];
                    minimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    defaultValidityLength = (byte)reader["DefaultValidityLength"];
                    classFees = (decimal)reader["classFees"];

                    reader.Close();

                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}