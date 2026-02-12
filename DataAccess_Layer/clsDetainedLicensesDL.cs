using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsDataAccessLayer
{
    public class clsDetainedLicensesDL
    {
        static public int detainLicense(int licenseID, DateTime detainDate, decimal fineFees, int createdByUserID)
        {
            int detainID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Insert Into DetainedLicenses
                             Values (@licenseID, @detainDate, @fineFees, @createdByUserID, 0, null, null, null)


                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);
            command.Parameters.AddWithValue("@detainDate", detainDate);
            command.Parameters.AddWithValue("@fineFees", fineFees);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
      
            try
            {
                connection.Open();

                object objdetainID = command.ExecuteScalar();
                
                if (objdetainID != null && int.TryParse(objdetainID.ToString(), out int result))
                {
                    detainID = result;
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return detainID;
        }

        static public bool isLicenseDetained(int licenseID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From DetainedLicenses Where LicenseID = @licenseID AND IsReleased = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);

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

        static public bool getDetainedLicenseByLicenseID(int licenseID, ref int detainID, ref DateTime detainDate,
            ref decimal fineFees, ref int createdByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From DetainedLicenses Where LicenseID = @licenseID AND IsReleased = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    detainID = (int)reader["DetainID"];      
                    detainDate = (DateTime)reader["DetainDate"];
                    fineFees = (decimal)reader["fineFees"];
                    createdByUserID = (int)reader["createdByUserID"];
         

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

        static public bool releaseDetainedLicense(int detainID, DateTime releaseDate, int releasedByUserID,
            int releaseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update DetainedLicenses
                           
                             Set ReleaseDate          = @releaseDate,
                                 ReleasedByUserID     = @releasedByUserID,
                                 ReleaseApplicationID = @releaseApplicationID,
                                 IsReleased           = 1

                             Where DetainID = @detainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@detainID", detainID);
            command.Parameters.AddWithValue("@releaseDate", releaseDate);
            command.Parameters.AddWithValue("@releasedByUserID", releasedByUserID);
            command.Parameters.AddWithValue("@releaseApplicationID", releaseApplicationID);


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

        static public DataTable getListDetainedLicenses()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = "Select * From DetainedLicenses_View";

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
