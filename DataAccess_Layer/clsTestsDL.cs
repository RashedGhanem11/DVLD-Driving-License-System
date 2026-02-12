using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsDataAccessLayer
{
    public class clsTestsDL
    {
        static public int passedTestsNumber(int localDrivingLicenseApplicationID)
        {
            int numOfRows = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Select Count(*)
                             From TestAppointments Inner Join Tests
                             ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             Where LocalDrivingLicenseApplicationID = @localDrivingLicenseApplicationID
                             AND TestResult = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@localDrivingLicenseApplicationID", localDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int num))
                {
                    numOfRows = num;
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            
            return numOfRows;
        }

        static public bool isTestTypePassed(int testTypeID, int localDrivingLicenseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Select * From Tests Inner Join TestAppointments
                             On Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             Where LocalDrivingLicenseApplicationID = @localDrivingLicenseApplicationID
                             AND                         TestTypeID = @testTypeID
                             AND                         TestResult = 1";

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

        static public bool getTestByID(int testID, ref int testAppointmentID,
            ref bool testResult, ref string notes, ref int createdByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From Tests Where TestID = @testID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testID", testID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    testAppointmentID = (int)reader["TestAppointmentID"];
                    testResult = (bool)reader["TestResult"];

                    if (reader["Notes"] != DBNull.Value)
                        notes = (string)reader["Notes"];
                    else
                        notes = "";

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

        static public bool getTestByTestAppointmentID(int testAppointmentID, ref int testID,
            ref bool testResult, ref string notes, ref int createdByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From Tests Where TestAppointmentID = @testAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    testID = (int)reader["TestID"];
                    testResult = (bool)reader["TestResult"];

                    if (reader["Notes"] != DBNull.Value)
                        notes = (string)reader["Notes"];
                    else
                        notes = "";

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

        static public int addNewTest(int testAppointmentID, bool testResult,
            string notes, int createdByUserID)
        {
            int testID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Insert Into Tests
                             Values (@testAppointmentID, @testResult, @notes, @createdByUserID)


                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);
            command.Parameters.AddWithValue("@testResult", testResult);

            if (notes != "")
                command.Parameters.AddWithValue("@notes", notes);
            else
                command.Parameters.AddWithValue("@notes", DBNull.Value);
         
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            try
            {
                connection.Open();

                object objtestID = command.ExecuteScalar();

                if (objtestID != null && int.TryParse(objtestID.ToString(), out int result))
                {
                    testID = result;
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return testID;
        }
    }
}
