using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bussiness_Layer
{
    public class clsInternationalLicense : clsApplication
    {

        public int internationalLicenseID { get; set; }
        public int driverID { get; set; }
        public int issuedUsingLocalLicenseID { get; set; }
        public DateTime expirationDate { get; set; }
        public bool isActive { get; set; }

        private bool _addNewInternationalLicense()
        {
            this.internationalLicenseID = clsInternationalLicensesDL.addNewInternationalLicense(applicationID, driverID, issuedUsingLocalLicenseID, applicationDate, expirationDate, isActive, createdByUserID);

            return internationalLicenseID != -1;
        }

        public clsInternationalLicense() : base(6)
        {
            this.internationalLicenseID = -1;
            this.driverID = -1;
            this.issuedUsingLocalLicenseID = -1;
            this.expirationDate = DateTime.Now.AddYears(1);
            this.isActive = true;
            this.applicationStatus = 3;
        }

        private clsInternationalLicense(int internationalLicenseID, int applicationID, int driverID,
            int issuedUsingLocalLicenseID, DateTime expirationDate, bool isActive)
        {
            this.internationalLicenseID = internationalLicenseID;
            this.applicationID = applicationID;
            this.driverID = driverID;
            this.issuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            this.expirationDate = expirationDate;
            this.isActive = isActive;
        }

        static public clsInternationalLicense getInternationalLicenseByID(int internationalLicenseID)
        {
            int applicationID = -1, driverID = -1, issuedUsingLocalLicenseID = -1;

            DateTime expirationDate = DateTime.Now;
            bool isActive = false;


            if (clsInternationalLicensesDL.getInternationalLicenseByID(internationalLicenseID, ref applicationID,
                ref driverID, ref issuedUsingLocalLicenseID, ref expirationDate, ref isActive)) 
            {
                clsInternationalLicense internationalLicense = new clsInternationalLicense(internationalLicenseID, applicationID, driverID,
                    issuedUsingLocalLicenseID, expirationDate, isActive);

                if (internationalLicense.getApplicationByID(applicationID))
                    return internationalLicense;
            }
            
                return null;
        }

        static public clsInternationalLicense getInternationalLicenseByLocalLicenseID(int issuedUsingLocalLicenseID)
        {
            int internationalLicenseID = -1, applicationID = -1, driverID = -1;

            DateTime expirationDate = DateTime.Now;
            bool isActive = false;


            if (clsInternationalLicensesDL.getInternationalLicenseByLocalLicenseID(issuedUsingLocalLicenseID,
                ref internationalLicenseID, ref applicationID, ref driverID, ref expirationDate, ref isActive)) 
            {
                clsInternationalLicense internationalLicense = new clsInternationalLicense(internationalLicenseID, applicationID, driverID,
                    issuedUsingLocalLicenseID, expirationDate, isActive);

                if (internationalLicense.getApplicationByID(applicationID))
                    return internationalLicense;
            }

            return null;
        }

        static public bool isInternationalLicenseExists(int issuedUsingLocalLicenseID)
        {
            return clsInternationalLicensesDL.isInternationalLicenseExists(issuedUsingLocalLicenseID);
        }

        static public DataTable getListInternationalLicensesShortDetails(int personID)
        {
            return clsInternationalLicensesDL.getListInternationalLicensesShortDetails(clsDriver.getDriverByPersonID(personID).driverID);
        }

        public bool save()
        {
            if (_addNewApplication())
            {
                nowMode = enMode.updateMode;
                return _addNewInternationalLicense();
            }
            return false;
        }

        static public DataTable getListInternationalLicenses()
        {
            return clsInternationalLicensesDL.getListInternationalLicenses();
        }
    }
}