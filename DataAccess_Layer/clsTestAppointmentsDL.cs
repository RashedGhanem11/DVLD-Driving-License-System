using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsDataAccessLayer
{
    public class clsTestAppointmentsDL
    {
        static public DataTable getListTestAppointmentsShortDetailed(int testTypeID, int localDrivingLicenseApplicationID)
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = @"Select TestAppointmentID, AppointmentDate, PaidFees, IsLocked From TestAppointments
                             Where TestTypeID                       = @testTypeID
                             AND   LocalDrivingLicenseApplicationID = @localDrivingLicenseApplicationID
                             Order by TestAppointmentID Desc";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@testTypeID", testTypeID);
            command.Parameters.AddWithValue("@localDrivingLicenseApplicationID", localDrivingLicenseApplicationID);


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

        static public bool isApplicationHaveAnActiveAppointment(int testTypeID, int localDrivingLicenseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Select * From TestAppointments
                             where LocalDrivingLicenseApplicationID = @localDrivingLicenseApplicationID
                             AND                         TestTypeID = @testTypeID
                             AND                           IsLocked = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@localDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);

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

        static public int trailNumbers(int testTypeID, int localDrivingLicenseApplicationID)
        {
            int trailNumber = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Select Count (*) From TestAppointments
                             where LocalDrivingLicenseApplicationID = @localDrivingLicenseApplicationID
                             AND                         TestTypeID = @testTypeID
                             AND                           IsLocked = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@localDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int IntResult)) 
                {
                    trailNumber = IntResult;


                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return trailNumber;
        }

        static public int addNewTestAppointment(int testTypeID, int localDrivingLicenseApplicationID,
            DateTime appointmentDate, decimal paidFees, int createdByUserID,
            bool isLocked, int retakeTestApplicationID)
        {
            int testAppointmentID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Insert Into TestAppointments
                             Values (@testTypeID, @localDrivingLicenseApplicationID, @appointmentDate, @paidFees,
                                     @createdByUserID, @isLocked, @retakeTestApplicationID)


                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testTypeID", testTypeID);
            command.Parameters.AddWithValue("@localDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@appointmentDate", appointmentDate);
            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("@isLocked", isLocked);
  
            if (retakeTestApplicationID != -1)
                command.Parameters.AddWithValue("@retakeTestApplicationID", retakeTestApplicationID);
            else
                command.Parameters.AddWithValue("@retakeTestApplicationID", DBNull.Value);

            try
            {
                connection.Open();

                object objtestAppointmentID = command.ExecuteScalar();

                if (objtestAppointmentID != null && int.TryParse(objtestAppointmentID.ToString(), out int result))
                {
                    testAppointmentID = result;
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return testAppointmentID;
        }

        static public bool updateTestAppointmentDate(int testAppointmentID, DateTime appointmentDate)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update TestAppointments
                           
                             Set AppointmentDate = @appointmentDate
                             Where TestAppointmentID = @testAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);
            command.Parameters.AddWithValue("@appointmentDate", appointmentDate);          


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

        static public bool lockTestAppointment(int testAppointmentID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update TestAppointments
                           
                             Set IsLocked = 1
                             Where TestAppointmentID = @testAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);

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

        static public bool getTestAppointmentByID(int testAppointmentID, ref int testTypeID,
            ref int localDrivingLicenseApplicationID, ref DateTime appointmentDate, ref decimal paidFees,
            ref int createdByUserID, ref bool isLocked, ref int retakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From TestAppointments Where TestAppointmentID = @testAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    testTypeID = (int)reader["TestTypeID"];
                    localDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    appointmentDate = (DateTime)reader["AppointmentDate"];
                    paidFees = (decimal)reader["PaidFees"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    isLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] != DBNull.Value)
                        retakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                    else
                        retakeTestApplicationID = -1;

                 

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
