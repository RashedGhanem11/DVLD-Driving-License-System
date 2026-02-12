using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsDataAccessLayer
{
    public class clsTestTypesDL
    {
        static public DataTable getListTestTypes()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = "Select * From TestTypes";

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

        static public bool updateTestType(int testTypeID, string testTypeTitle, string testTypeDescription, decimal testTypeFees)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update TestTypes
                           
                             Set TestTypeTitle       = @testTypeTitle,
                                 TestTypeDescription = @testTypeDescription,
                                 TestTypeFees        = @testTypeFees
                                 
                             Where TestTypeID = @testTypeID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testTypeID", testTypeID);
            command.Parameters.AddWithValue("@TestTypeTitle", testTypeTitle);
            command.Parameters.AddWithValue("@testTypeDescription", testTypeDescription);
            command.Parameters.AddWithValue("@testTypeFees", testTypeFees);



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

        static public bool getTestTypeByID(int testTypeID, ref string testTypeTitle,
            ref string testTypeDescription, ref decimal testTypeFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From TestTypes Where TestTypeID = @testTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testTypeID", testTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    testTypeTitle = (string)reader["TestTypeTitle"];
                    testTypeDescription = (string)reader["TestTypeDescription"];
                    testTypeFees = (decimal)reader["TestTypeFees"];

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
