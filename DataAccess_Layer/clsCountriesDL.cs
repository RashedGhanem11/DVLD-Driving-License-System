using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Policy;

namespace clsDataAccessLayer
{
    public class clsCountriesDL
    {
        static public DataTable getListCountries()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = "Select * From Countries";

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

        static public bool Find(int countryID, ref string countryName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From Countries Where CountryID = @countryID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@countryID", countryID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    countryName = (string)reader["CountryName"];

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

        static public bool Find(string countryName, ref int countryID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From Countries Where CountryName = @countryName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@countryName", countryName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    countryID = (int)reader["CountryID"];

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
