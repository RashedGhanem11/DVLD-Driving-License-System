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
    public class clsPeopleDL
    {
        static public int addNewPerson(string nationalNo, string firstName, string secondName,
            string thirdName, string lastName, DateTime dateOfBirth, short gendor, string address,
            string phone, string email, int nationalityCountryID, string imagePath)
        {
            int personID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Insert Into People
                             Values (@nationalNo, @firstName, @secondName, @thirdName, @lastName, @dateOfBirth, 
                                     @gendor, @address, @phone, @email, @nationalityCountryID, @imagePath)


                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@nationalNo", nationalNo);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@secondName", secondName);

            if (thirdName != "")
                command.Parameters.AddWithValue("@thirdName", thirdName);
            else
                command.Parameters.AddWithValue("@thirdName", DBNull.Value);

            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
            command.Parameters.AddWithValue("@gendor", gendor);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@phone", phone);

            if (email != "")
                command.Parameters.AddWithValue("@email", email);
            else
                command.Parameters.AddWithValue("@email", DBNull.Value);

            command.Parameters.AddWithValue("@nationalityCountryID", nationalityCountryID);

            if (imagePath != "")
                command.Parameters.AddWithValue("@imagePath", imagePath);
            else
                command.Parameters.AddWithValue("@imagePath", DBNull.Value);



            try
            {
                connection.Open();

                object objPersonID = command.ExecuteScalar();

                if (objPersonID != null && int.TryParse(objPersonID.ToString(), out int result)) 
                {
                    personID = result;
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return personID;
        }

        static public bool updatePerson(int personID, string nationalNo, string firstName, string secondName,
            string thirdName, string lastName, DateTime dateOfBirth, short gendor, string address,
            string phone, string email, int nationalityCountryID, string imagePath)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Update People
                           
                             Set NationalNo           = @nationalNo,
                                 FirstName            = @firstName,
                                 SecondName           = @secondName,
                                 ThirdName            = @thirdName,
                                 LastName             = @lastName,
                                 DateOfBirth          = @dateOfBirth,
                                 Gendor               = @gendor,
                                 Address              = @address,
                                 Phone                = @phone,
                                 Email                = @email,
                                 NationalityCountryID = @nationalityCountryID,
                                 ImagePath            = @imagePath
                
                             Where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@nationalNo", nationalNo);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@secondName", secondName);

            if (thirdName != "")
                command.Parameters.AddWithValue("@thirdName", thirdName);
            else
                command.Parameters.AddWithValue("@thirdName", DBNull.Value);

            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
            command.Parameters.AddWithValue("@gendor", gendor);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@phone", phone);

            if (email != "")
                command.Parameters.AddWithValue("@email", email);
            else
                command.Parameters.AddWithValue("@email", DBNull.Value);

            command.Parameters.AddWithValue("@nationalityCountryID", nationalityCountryID);

            if (imagePath != "")
                command.Parameters.AddWithValue("@imagePath", imagePath);
            else
                command.Parameters.AddWithValue("@imagePath", DBNull.Value);



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

        static public bool deletePerson(int personID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = @"Delete From People
                             Where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);

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

        static public DataTable getListPeople()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = "Select * From People";

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

        static public DataTable getListPeopleShortDetailed()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string Query = "Select * From vPeopleShortDetailed";

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

        static public bool getPersonByID(int personID, ref string nationalNo, ref string firstName, ref string secondName,
            ref string thirdName, ref string lastName, ref DateTime dateOfBirth, ref short gendor, ref string address,
            ref string phone, ref string email, ref int nationalityCountryID, ref string imagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From People Where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) 
                {
                    isFound = true;

                    nationalNo = (string)reader["NationalNo"];
                    firstName = (string)reader["FirstName"];
                    secondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                        thirdName = (string)reader["ThirdName"];
                    else
                        thirdName = "";

                    lastName = (string)reader["LastName"];
                    dateOfBirth = (DateTime)reader["DateOfBirth"];
                    gendor = (byte)reader["Gendor"];
                    address = (string)reader["Address"];
                    phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                        email = (string)reader["Email"];
                    else
                        email = "";

                    nationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] != DBNull.Value)
                        imagePath = (string)reader["ImagePath"];
                    else
                        imagePath = "";

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

        static public bool getPersonByNationalNo(string nationalNo, ref int personID, ref string firstName, ref string secondName,
            ref string thirdName, ref string lastName, ref DateTime dateOfBirth, ref short gendor, ref string address,
            ref string phone, ref string email, ref int nationalityCountryID, ref string imagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From People Where NationalNo = @nationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@nationalNo", nationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    personID = (int)reader["PersonID"];
                    firstName = (string)reader["FirstName"];
                    secondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                        thirdName = (string)reader["ThirdName"];
                    else
                        thirdName = "";

                    lastName = (string)reader["LastName"];
                    dateOfBirth = (DateTime)reader["DateOfBirth"];
                    gendor = (byte)reader["Gendor"];
                    address = (string)reader["Address"];
                    phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                        email = (string)reader["Email"];
                    else
                        email = "";

                    nationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] != DBNull.Value)
                        imagePath = (string)reader["ImagePath"];
                    else
                        imagePath = "";

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

        static public bool isPersonExists(int personID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From People Where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);

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

        static public bool isPersonExists(string nationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.connectionString);

            string query = "Select * From People Where NationalNo = @nationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@nationalNo", nationalNo);

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

    }
}
