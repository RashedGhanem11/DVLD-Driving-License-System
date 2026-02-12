using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace clsDataAccessLayer
{
    public class clsApplicationsDL
    {
        static public int addNewApplication(int personID, DateTime applicationDate, int applicationTypeID,
            int applicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            int applicationID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Insert Into Applications
                             Values (@personID, @applicationDate, @applicationTypeID, @applicationStatus,
                                     @lastStatusDate, @paidFees, @createdByUserID)


                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@applicationDate", applicationDate);
            command.Parameters.AddWithValue("@applicationTypeID", applicationTypeID);
            command.Parameters.AddWithValue("@applicationStatus", applicationStatus);
            command.Parameters.AddWithValue("@lastStatusDate", lastStatusDate);
            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

         
            try
            {
                connection.Open();

                object objApplicationID = command.ExecuteScalar();

                if (objApplicationID != null && int.TryParse(objApplicationID.ToString(), out int result))
                {
                    applicationID = result;
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return applicationID;
        }

        static public bool deleteApplication(int applicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Delete From Applications
                             Where ApplicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);

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

        static public bool cancelApplication(int applicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update Applications
                             Set ApplicationStatus = 2,
                                 LastStatusDate = GETDATE() 
                             Where ApplicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);

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

        static public bool completeApplication(int applicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update Applications
                             Set ApplicationStatus = 3,
                                 LastStatusDate = GETDATE() 
                             Where ApplicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);

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

        static public bool getApplicationByID(int applicationID, ref int personID, ref DateTime applicationDate,
            ref int applicationTypeID, ref int applicationStatus, ref DateTime lastStatusDate,
            ref decimal paidFees, ref int createdByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From Applications Where ApplicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    personID = (int)reader["ApplicantPersonID"];
                    applicationDate = (DateTime)reader["ApplicationDate"];
                    applicationTypeID = (int)reader["ApplicationTypeID"];
                    applicationStatus = (byte)reader["ApplicationStatus"];
                    lastStatusDate = (DateTime)reader["LastStatusDate"];
                    paidFees = (decimal)reader["PaidFees"];
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
    }
}
