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
    public class clsLicense
    {
        public int licenseID { get; set; }
        public int applicationID { get; set; }
        public int driverID { get; set; }
        public int licenseClassID { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime expirationDate { get; set; }
        public string notes { get; set; }
        public decimal paidFees { get; set; }
        public bool isActive { get; set; }
        public short issueReason { get; set; }
        public int createdByUserID { get; set; }

        private enum enMode { addMode, updateMode };
        private enMode nowMode { get; set; }

        private bool _addNewDriverIfNotExists()
        {
            if (issueReason != 1)
                return true;

            clsLocalDrivingLicenseApplication localApp = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByApplicationID(applicationID);

            clsDriver tempDriver = clsDriver.getDriverByPersonID(localApp.personID);


            if (tempDriver != null)
            {
                this.driverID = tempDriver.driverID;
                return true;
            }

            clsDriver driver = new clsDriver();
            driver.personID = localApp.personID;
            driver.createdByUserID = this.createdByUserID;

            if (driver.save()) 
            {
                this.driverID = driver.driverID;
                return true;
            }

            return false;
        }

        private bool _addNewLicense()
        {        

            licenseID = clsLicensesDL.addNewLicense(applicationID, driverID, licenseClassID, issueDate, expirationDate,
                notes, paidFees, isActive, issueReason, createdByUserID);

            return licenseID != -1;
        }

        public clsLicense()
        {
            this.licenseID = -1;
            this.applicationID = -1;
            this.driverID = -1;
            this.licenseClassID = -1;
            this.issueDate = DateTime.Now;
            this.expirationDate = DateTime.Now;
            this.notes = "";
            this.paidFees = -1;
            this.isActive = true;
            this.issueReason = 1;
            this.createdByUserID = -1;
            this.nowMode = enMode.addMode;
        }

        private clsLicense(int licenseID, int applicationID, int driverID, int licenseClassID,
            DateTime issueDate, DateTime expirationDate, string notes,
            decimal paidFees, bool isActive, short issueReason, int createdByUserID)
        {
            this.licenseID = licenseID;
            this.applicationID = applicationID;
            this.driverID = driverID;
            this.licenseClassID = licenseClassID;
            this.issueDate = issueDate;
            this.expirationDate = expirationDate;
            this.notes = notes;
            this.paidFees = paidFees;
            this.isActive = isActive;
            this.issueReason = issueReason;
            this.createdByUserID = createdByUserID;
            nowMode = enMode.updateMode;
        }

        static public bool isLicenseExists(int applicationID)
        {
            return clsLicensesDL.isLicenseExists(applicationID);
        }

        static public clsLicense getLicenseByID(int licenseID)
        {
            int applicationID = -1, driverID = -1, licenseClassID = -1, createdByUserID = -1;

            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = -1;
            bool isActive = false;
            short issueReason = -1;


            if (clsLicensesDL.getLicenseByID(licenseID, ref applicationID, ref driverID, ref licenseClassID
                , ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason,
                ref createdByUserID))
            {
                return new clsLicense(licenseID, applicationID, driverID, licenseClassID, issueDate, expirationDate,
                    notes, paidFees, isActive, issueReason, createdByUserID);
            }
            else
                return null;

        }

        static public clsLicense getLicenseByApplicationID(int applicationID)
        {
            int licenseID = -1, driverID = -1, licenseClassID = -1, createdByUserID = -1;

            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = -1;
            bool isActive = false;
            short issueReason = -1;
            

            if (clsLicensesDL.getLicenseByApplicationID(applicationID, ref licenseID, ref driverID, ref licenseClassID
                , ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason,
                ref createdByUserID))
            {
                return new clsLicense(licenseID, applicationID, driverID, licenseClassID, issueDate, expirationDate,
                    notes, paidFees, isActive, issueReason, createdByUserID);
            }
            else
                return null;

        }

        public bool save()
        {
            switch (nowMode)
            {
                case enMode.addMode:
                    {

                        if (isLicenseExists(applicationID))
                            return false;

                        if (_addNewDriverIfNotExists())
                        {
                            if (_addNewLicense()) 
                            {
                                clsApplication.completeApplication(applicationID);
                                return true;
                            }
                            else
                                return false;
                        }
                        else
                            return false;
                    }

                    
            }

            return false;
        }

        static public DataTable getListLicensesShortDetails(int personID)
        {
            return clsLicensesDL.getListLicensesShortDetails(clsDriver.getDriverByPersonID(personID).driverID);
        }

        static public bool deactiveLicense(int licenseID)
        {
            return clsLicensesDL.deactiveLicense(licenseID);
        }
    }
}