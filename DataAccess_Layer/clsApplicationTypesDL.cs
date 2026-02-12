using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsDataAccessLayer
{
    public class clsApplicationTypesDL
    {
        static public DataTable getListApplicationTypes()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = "Select * From ApplicationTypes";

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

        static public bool updateApplicationType(int applicationTypeID, string applicationTypeTitle, decimal applicationFees)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update ApplicationTypes
                           
                             Set ApplicationTypeTitle = @applicationTypeTitle,
                                 ApplicationFees      = @applicationFees
                                 
                             Where ApplicationTypeID = @applicationTypeID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationTypeID", applicationTypeID);
            command.Parameters.AddWithValue("@applicationTypeTitle", applicationTypeTitle);
            command.Parameters.AddWithValue("@applicationFees", applicationFees);



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

        static public bool getApplicationTypeByID(int applicationTypeID, ref string applicationTypeTitle, ref decimal applicationFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From ApplicationTypes Where ApplicationTypeID = @applicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationTypeID", applicationTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    applicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    applicationFees = (decimal)reader["ApplicationFees"];
                   
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