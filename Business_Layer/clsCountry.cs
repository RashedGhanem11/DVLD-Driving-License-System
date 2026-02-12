using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsCountry
    {
        public int countryID { get; set; }
        public string countryName { get; set; }

        private clsCountry(int countryID, string countryName)
        {
            this.countryID = countryID;
            this.countryName = countryName;
        }

        static public DataTable getListCountries()
        {
            return clsCountriesDL.getListCountries();
        }

        static public clsCountry getCountryByID(int countryID)
        {
            string countryName = "";

            if (clsCountriesDL.Find(countryID, ref countryName))
            {
                return new clsCountry(countryID, countryName);
            }
            else
                return null;

        }

        static public clsCountry getCountryByCountryName(string countryName)
        {
            int countryID = -1;

            if (clsCountriesDL.Find(countryName, ref countryID))
            {
                return new clsCountry(countryID, countryName);
            }
            else
                return null;

        }


    }
}
