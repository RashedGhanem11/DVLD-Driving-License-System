using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clsDataAccessLayer;

namespace Bussiness_Layer
{
    public class clsPerson
    {
        public int personID { get; set; }
        public string nationalNo { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string thirdName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public short gendor { get; set; }
        public string address { get; set;}
        public string phone { get; set; }
        public string email { get; set; }
        public int nationalityCountryID { get; set; }
        public string imagePath { get; set; }

        private enum enMode { addMode, updateMode };
        private enMode nowMode { get; set; }

        private bool _addNewPerson()
        {
            this.personID = clsPeopleDL.addNewPerson(nationalNo, firstName, secondName, thirdName, lastName,
                dateOfBirth, gendor, address, phone, email, nationalityCountryID, imagePath);

            return personID != -1;
        }

        private bool _updatePerson()
        {
            return clsPeopleDL.updatePerson(personID, nationalNo, firstName, secondName, thirdName, lastName,
                dateOfBirth, gendor, address, phone, email, nationalityCountryID, imagePath);
        }

        public clsPerson()
        {
            this.personID = -1;
            this.nationalNo = "";
            this.firstName = "";
            this.secondName = "";
            this.thirdName = "";
            this.lastName = "";
            this.dateOfBirth = DateTime.Now;
            this.gendor = -1;
            this.address = "";
            this.phone = "";
            this.email = "";
            this.nationalityCountryID = -1;
            this.imagePath = "";
            this.nowMode = enMode.addMode;
        }

        private clsPerson(int personID, string nationalNo, string firstName, string secondName,
            string thirdName, string lastName, DateTime dateOfBirth, short gendor, string address,
            string phone, string email, int nationalityCountryID, string imagePath)
        {
            this.personID = personID;
            this.nationalNo = nationalNo;
            this.firstName = firstName;
            this.secondName = secondName;
            this.thirdName = thirdName;
            this.lastName = lastName;
            this.dateOfBirth = dateOfBirth;
            this.gendor = gendor;
            this.address = address;
            this.phone = phone;
            this.email = email;
            this.nationalityCountryID = nationalityCountryID;
            this.imagePath = imagePath;
            nowMode = enMode.updateMode;
        }

        static public bool deletePerson(int personID)
        {
            return clsPeopleDL.deletePerson(personID);
        }

        static public DataTable getListPeople()
        {
            return clsPeopleDL.getListPeople();
        }

        static public DataTable getListPeopleShortDetailed()
        {
            return clsPeopleDL.getListPeopleShortDetailed();
        }

        static public clsPerson getPersonByID(int personID)
        {
            string nationalNo = "", firstName = "", secondName = "", thirdName = "",
                lastName = "", address = "", phone = "", email = "", imagePath = "";

            DateTime dateOfBirth = DateTime.Now;
            short gendor = -1;
            int nationalityCountryID = -1;

            if (clsPeopleDL.getPersonByID(personID, ref nationalNo, ref firstName, ref secondName, ref thirdName,
                ref lastName, ref dateOfBirth, ref gendor, ref address, ref phone,
                ref email, ref nationalityCountryID, ref imagePath)) 
            {
                return new clsPerson(personID, nationalNo, firstName, secondName, thirdName, lastName,
                    dateOfBirth, gendor, address, phone, email, nationalityCountryID, imagePath);
            }
            else
                return null;

        }

        static public clsPerson getPersonByNationalNo(string nationalNo)
        {
            string firstName = "", secondName = "", thirdName = "",
                lastName = "", address = "", phone = "", email = "", imagePath = "";

            DateTime dateOfBirth = DateTime.Now;
            short gendor = -1;
            int nationalityCountryID = -1;
            int personID = -1;

            if (clsPeopleDL.getPersonByNationalNo(nationalNo, ref personID, ref firstName, ref secondName, ref thirdName,
                ref lastName, ref dateOfBirth, ref gendor, ref address, ref phone,
                ref email, ref nationalityCountryID, ref imagePath)) 
            {
                return new clsPerson(personID, nationalNo, firstName, secondName, thirdName, lastName,
                    dateOfBirth, gendor, address, phone, email, nationalityCountryID, imagePath);
            }
            else
                return null;

        }

        static public bool isPersonExists(int personID)
        {
            return clsPeopleDL.isPersonExists(personID);
        }

        static public bool isPersonExists(string nationalNo)
        {
            return clsPeopleDL.isPersonExists(nationalNo);
        }

        public bool save()
        {
            switch(nowMode)
            {
                case enMode.addMode:
                {
                    if (_addNewPerson())
                    {
                        nowMode = enMode.updateMode;
                        return true;
                    }
                    else
                        return false;
                }
                
                case enMode.updateMode:
                    return _updatePerson();
            }

            return false;
        }

        static public string getPath(string guid)
        {
            return "F:/Rashed/Programming/Projects/DVLD/People Images/" + guid + ".png";
        }

        public int calculateAge()
        {
            /* 
            Example Person DateBirth(6/11/2004) , NowDate(7/11/2024)
            (Make them same year)
            Copmare( (6/11/2024) With (7/11/2024) ) if first date earlier(True)
            then his datebirth have come this year return this year - year of birth (2024 - 2004 = 20)

            Example 2 Person DateBirth(7/11/2004) , NowDate(6/11/2024)
            (Make them same year)
            Copmare( (7/11/2024) With (6/11/2024) ) if first date earlier(False)
            then his datebirth did NOT come this year return this year - year of birth - 1 (2024 - 2004 - 1 = 19)
            */

            DateTime temp = new DateTime(DateTime.Now.Year, dateOfBirth.Month, dateOfBirth.Day);

            //Means First Date Earlier
            if (0 > DateTime.Compare(DateTime.Now, temp))
            {
                return DateTime.Now.Year - dateOfBirth.Year - 1;
            }
            else
                return DateTime.Now.Year - dateOfBirth.Year;
        }

        static public string getPersonFullName(int personID)
        {
            clsPerson person = clsPerson.getPersonByID(personID);

            if (person != null)
            {
                if (person.thirdName != "")
                    return person.firstName + " " + person.secondName + " " + person.thirdName + " " + person.lastName;
                else
                    return person.firstName + " " + person.secondName + " " + person.lastName;

            }

            return "";
        }

    }
}
