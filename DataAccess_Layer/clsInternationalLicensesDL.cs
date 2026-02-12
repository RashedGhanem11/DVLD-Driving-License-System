using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace clsDataAccessLayer
{
    public class clsInternationalLicensesDL
    {
        static public DataTable getListInternationalLicensesShortDetails(int driverID)
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = @"Select InternationalLicenseID, ApplicationID, IssuedUsingLocalLicenseID,
                             IssueDate, ExpirationDate, IsActive From InternationalLicenses
                             Where DriverID = @driverID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@driverID", driverID);

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

        static public bool isInternationalLicenseExists(int issuedUsingLocalLicenseID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From InternationalLicenses Where IssuedUsingLocalLicenseID = @issuedUsingLocalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@issuedUsingLocalLicenseID", issuedUsingLocalLicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;


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

        static public bool getInternationalLicenseByID(int internationalLicenseID, ref int applicationID,
            ref int driverID, ref int issuedUsingLocalLicenseID, ref DateTime expirationDate,
            ref bool isActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From InternationalLicenses Where InternationalLicenseID = @internationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@internationalLicenseID", internationalLicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    applicationID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    issuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    isActive = (bool)reader["IsActive"];


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

        static public bool getInternationalLicenseByLocalLicenseID(int issuedUsingLocalLicenseID, ref int internationalLicenseID,
            ref int applicationID, ref int driverID, ref DateTime expirationDate,
           ref bool isActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From InternationalLicenses Where IssuedUsingLocalLicenseID = @issuedUsingLocalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@issuedUsingLocalLicenseID", issuedUsingLocalLicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
       
                    internationalLicenseID = (int)reader["InternationalLicenseID"];
                    applicationID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    isActive = (bool)reader["IsActive"];


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

        static public int addNewInternationalLicense(int applicationID, int driverID, int issuedUsingLocalLicenseID,
              DateTime issueDate, DateTime expirationDate, bool isActive, int createdByUserID)
        {
            int internationalLicenseID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Insert Into InternationalLicenses
                             Values (@applicationID, @driverID, @issuedUsingLocalLicenseID, @issueDate, @expirationDate,
                                     @isActive, @createdByUserID)


                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@issuedUsingLocalLicenseID", issuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@issueDate", issueDate);
            command.Parameters.AddWithValue("@expirationDate", expirationDate);
            command.Parameters.AddWithValue("@isActive", isActive);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            try
            {
                connection.Open();

                object objInternationalID = command.ExecuteScalar();

                if (objInternationalID != null && int.TryParse(objInternationalID.ToString(), out int result))
                {
                    internationalLicenseID = result;
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return internationalLicenseID;
        }

        static public DataTable getListInternationalLicenses()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = @"Select InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                             IssueDate, ExpirationDate, IsActive From InternationalLicenses";

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
