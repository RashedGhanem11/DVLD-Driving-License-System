using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsDataAccessLayer
{
    public class clsLicensesDL
    {
        static public int addNewLicense(int applicationID, int driverID, int licenseClassID,
            DateTime issueDate, DateTime expirationDate, string notes,
            decimal paidFees, bool isActive, short issueReason, int createdByUserID)
        { 
            int licenseID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Insert Into Licenses
                             Values (@applicationID, @driverID, @licenseClassID, @issueDate, @expirationDate, @notes, 
                                     @paidFees, @isActive, @issueReason, @createdByUserID)


                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);    
            command.Parameters.AddWithValue("@issueDate", issueDate);
            command.Parameters.AddWithValue("@expirationDate", expirationDate);

            if (notes != "") 
                command.Parameters.AddWithValue("@notes", notes);
            else
                command.Parameters.AddWithValue("@notes", DBNull.Value);

            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@isActive", isActive);
            command.Parameters.AddWithValue("@issueReason", issueReason);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

  
            try
            {
                connection.Open();

                object objLicenseID = command.ExecuteScalar();

                if (objLicenseID != null && int.TryParse(objLicenseID.ToString(), out int result))
                {
                    licenseID = result;
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return licenseID;
        }

        static public bool isLicenseExists(int applicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From Licenses Where ApplicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);

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

        static public bool getLicenseByApplicationID(int applicationID, ref int licenseID, ref int driverID, ref int licenseClassID,
            ref DateTime issueDate, ref DateTime expirationDate, ref string notes,
            ref decimal paidFees, ref bool isActive, ref short issueReason, ref int createdByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From Licenses Where ApplicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    licenseID = (int)reader["LicenseID"];
                    driverID = (int)reader["DriverID"];
                    licenseClassID = (int)reader["LicenseClass"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];

                    if (reader["Notes"] != DBNull.Value)
                        notes = (string)reader["Notes"];
                    else
                        notes = "";

                    paidFees = (decimal)reader["PaidFees"];
                    isActive = (bool)reader["IsActive"];
                    issueReason = (byte)reader["IssueReason"];
                    createdByUserID = (int)reader["CreatedByUserID"];

                  

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

        static public bool getLicenseByID(int licenseID, ref int applicationID, ref int driverID, ref int licenseClassID,
            ref DateTime issueDate, ref DateTime expirationDate, ref string notes,
            ref decimal paidFees, ref bool isActive, ref short issueReason, ref int createdByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From Licenses Where LicenseID = @licenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    applicationID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    licenseClassID = (int)reader["LicenseClass"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];

                    if (reader["Notes"] != DBNull.Value)
                        notes = (string)reader["Notes"];
                    else
                        notes = "";

                    paidFees = (decimal)reader["PaidFees"];
                    isActive = (bool)reader["IsActive"];
                    issueReason = (byte)reader["IssueReason"];
                    createdByUserID = (int)reader["CreatedByUserID"];



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

        static public DataTable getListLicensesShortDetails(int driverID)
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = @"Select LicenseID, ApplicationID, ClassName, IssueDate, ExpirationDate, IsActive
                             From Licenses Inner Join LicenseClasses
                             On Licenses.LicenseClass = LicenseClasses.LicenseClassID
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

        static public bool deactiveLicense(int licenseID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update Licenses
                             Set IsActive = 0
                             Where licenseID = @licenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);
        

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
    }
}
