using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsUser
    {
        public int userID { get; set; }
        public int personID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public bool isActive { get; set; }

        private enum enMode { addMode, updateMode };
        private enMode nowMode { get; set; }

        private bool _addNewUser()
        {
            this.userID = clsUsersDL.addNewUser(personID, userName, password, isActive);

            return userID != -1;
        }

        private bool _updateUser()
        {
            return clsUsersDL.updateUser(userID, personID, userName, password, isActive);
        }

        public clsUser()
        {
            this.userID = -1;
            this.personID = -1;
            this.userName = "";
            this.password = "";
            this.isActive = false;

            this.nowMode = enMode.addMode;
        }

        private clsUser(int userID, int personID, string userName, string password, bool isActive)
        {
            this.userID = userID;
            this.personID = personID;
            this.userName = userName;
            this.password = password;
            this.isActive = isActive;

            this.nowMode = enMode.updateMode;
        }

        public bool save()
        {
            switch (nowMode)
            {
                case enMode.addMode:
                    {
                        if (_addNewUser())
                        {
                            nowMode = enMode.updateMode;
                            return true;
                        }
                        else
                            return false;
                    }

                case enMode.updateMode:
                    return _updateUser();
            }

            return false;
        }

        static public bool deleteUser(int userID)
        {
            return clsUsersDL.deleteUser(userID);
        }

        static public clsUser getUserByID(int userID)
        {
            int personID = -1;
            string userName = "", password = "";
            bool isActive = false;
            

            if (clsUsersDL.getUserByID(userID, ref personID, ref userName, ref password, ref isActive))
            {
                return new clsUser(userID, personID, userName, password, isActive);
            }
            else
                return null;

        }

        static public clsUser getUserByUserName(string userName)
        {
            int userID = -1, personID = -1;
            string password = "";
            bool isActive = false;


            if (clsUsersDL.getUserByUserName(userName, ref userID, ref personID, ref password, ref isActive)) 
            {
                return new clsUser(userID, personID, userName, password, isActive);
            }
            else
                return null;

        }

        static public DataTable getListUsers()
        {
            return clsUsersDL.getListUsers();
        }

        static public DataTable getListUsersDetailed()
        {
            return clsUsersDL.getListUsersDetailed();
        }

        static public bool isUserExists(int personID)
        {
            return clsUsersDL.isUserExists(personID);
        }

        static public bool isUserExists(string userName)
        {
            return clsUsersDL.isUserExists(userName);
        }

    }
}
