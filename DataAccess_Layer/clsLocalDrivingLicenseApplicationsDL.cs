using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace clsDataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationsDL
    {
        static public int addNewLocalDrivingLicenseApplication(int applicationID, int licenseCLassID)
        {
            int localDrivingLicenseApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Insert Into LocalDrivingLicenseApplications
                             Values (@applicationID, @licenseCLassID)


                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@licenseCLassID", licenseCLassID);

            try
            {
                connection.Open();

                object objlocalDrivingLicenseApplicationID = command.ExecuteScalar();

                if (objlocalDrivingLicenseApplicationID != null && int.TryParse(objlocalDrivingLicenseApplicationID.ToString(), out int result))
                {
                    localDrivingLicenseApplicationID = result;
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return localDrivingLicenseApplicationID;
        }

        static public bool deleteLocalDrivingLicenseApplication(int localDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Delete From LocalDrivingLicenseApplications
                             Where LocalDrivingLicenseApplicationID = @localDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@localDrivingLicenseApplicationID", localDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        static public bool isPersonHaveAnActiveLocalLicenseApplication(int personID, int licenseClassID, ref int applicationID)
        { 
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Select Applications.ApplicationID From Applications
                             Inner Join LocalDrivingLicenseApplications 
                             On Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                             Where
                             	 ApplicantPersonID  = @personID
                             AND LicenseClassID     = @licenseClassID
                             AND ApplicationStatus != 2";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);

            try
            {
                connection.Open();

                object objApplicationID = command.ExecuteScalar();

                if (objApplicationID != null)
                {
                    isFound = true;
                    applicationID = (int)objApplicationID;

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

        static public bool changeLicenseClass(int localDrivingLicenseApplicationID, int licenseClassID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update LocalDrivingLicenseApplications
                           
                             Set LicenseClassID = @licenseClassID
                
                             Where LocalDrivingLicenseApplicationID = @localDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@localDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);
           
            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        static public bool getLocalDrivingLicenseApplicationByID(int localDrivingLicenseApplicationID,
            ref int licenseClassID, ref int applicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From LocalDrivingLicenseApplications Where localDrivingLicenseApplicationID = @localDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@localDrivingLicenseApplicationID", localDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    licenseClassID = (int)reader["licenseClassID"];
                    applicationID = (int)reader["ApplicationID"];


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

        static public bool getLocalDrivingLicenseApplicationByApplicationID(int applicationID,
            ref int localDrivingLicenseApplicationID, ref int licenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From LocalDrivingLicenseApplications Where ApplicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
             
                    localDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    licenseClassID = (int)reader["licenseClassID"];


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

        static public DataTable getListLocalDrivingLicenseApplications()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = "Select * From LocalDrivingLicenseApplications_View";

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

    }
}
