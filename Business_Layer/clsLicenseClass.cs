using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsLicenseClass
    {
        public int licenseClassID { get; set; }
        public string className { get; set; }
        public string classDescription { get; set; }
        public byte minimumAllowedAge { get; set; }
        public byte defaultValidityLength { get; set; }
        public decimal classFees { get; set; }

        private clsLicenseClass(int licenseClassID, string className, string classDescription,
            byte minimumAllowedAge, byte defaultValidityLength, decimal classFees)
        {
            this.licenseClassID = licenseClassID;
            this.className = className;
            this.classDescription = classDescription;
            this.minimumAllowedAge = minimumAllowedAge;
            this.defaultValidityLength = defaultValidityLength;
            this.classFees = classFees;
        }

        static public DataTable getListLicenseClasses()
        {
            return clsLicenseClassesDL.getListLicenseClasses();
        }

        static public clsLicenseClass getLicenseClassByID(int licenseClassID)
        {
            string className = "", classDescription = "";
            byte minimumAllowedAge = 0, defaultValidityLength = 0;
            decimal classFees = -1;

            if (clsLicenseClassesDL.Find(licenseClassID, ref className, ref classDescription,
                ref minimumAllowedAge, ref defaultValidityLength, ref classFees)) 
            {
                return new clsLicenseClass(licenseClassID, className, classDescription,
                    minimumAllowedAge, defaultValidityLength, classFees);
            }
            else
                return null;

        }

        static public clsLicenseClass getLicenseClassByName(string className)
        {
            string classDescription = "";
            int licenseClassID = -1;
            byte minimumAllowedAge = 0, defaultValidityLength = 0;
            decimal classFees = -1;

            if (clsLicenseClassesDL.Find(className, ref licenseClassID, ref classDescription,
                ref minimumAllowedAge, ref defaultValidityLength, ref classFees))
            {
                return new clsLicenseClass(licenseClassID, className, classDescription,
                    minimumAllowedAge, defaultValidityLength, classFees);
            }
            else
                return null;

        }
    }
}
