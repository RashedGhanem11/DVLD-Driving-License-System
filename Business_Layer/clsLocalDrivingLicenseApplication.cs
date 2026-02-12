using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public int localDrivingLicenseAppliactionID { get; set; }
        public int licenseClassID { get; set; }

        public bool _addNewLocalDrivingLicenseApplication()
        {
            this.localDrivingLicenseAppliactionID = clsLocalDrivingLicenseApplicationsDL.addNewLocalDrivingLicenseApplication(applicationID, licenseClassID);

            return localDrivingLicenseAppliactionID != -1;
        }

        public bool _changeLicenseClass()
        {
            return clsLocalDrivingLicenseApplicationsDL.changeLicenseClass(localDrivingLicenseAppliactionID, licenseClassID);
        }

        public clsLocalDrivingLicenseApplication() : base(1)
        {
            this.localDrivingLicenseAppliactionID = -1;
            this.licenseClassID = -1;
        }

        private clsLocalDrivingLicenseApplication(int localDrivingLicenseAppliactionID, int licenseClassID, int applicationID)
        {
            this.localDrivingLicenseAppliactionID = localDrivingLicenseAppliactionID;
            this.licenseClassID = licenseClassID;
            this.applicationID = applicationID;
        }

        /// <summary>
        /// If found returns applicationID else returns -1
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="licenseClassID"></param>
        /// <returns></returns>
        
        static public int isPersonHaveAnActiveLocalLicenseApplication(int personID, int licenseClassID)
        {
            int applicationID = -1;

            if (clsLocalDrivingLicenseApplicationsDL.isPersonHaveAnActiveLocalLicenseApplication(personID, licenseClassID, ref applicationID)) 
                return applicationID;
            else
                return -1;
        }

        public bool save()
        {
            switch(nowMode)
            {
                case enMode.addMode:
                    {
                        if (_addNewApplication())
                        {
                            nowMode = enMode.updateMode;
                            return _addNewLocalDrivingLicenseApplication();
                        }
                        return false;
                    }

                case enMode.updateMode:
                    return _changeLicenseClass();

                    

            }



            return false;
        }

        static public bool deleteLocalDrivingLicenseApplication(int localDrivingLicenseApplicationID)
        {
            int applicationID = getLocalDrivingLicenseApplicationByID(localDrivingLicenseApplicationID).applicationID;

            if (clsLocalDrivingLicenseApplicationsDL.deleteLocalDrivingLicenseApplication(localDrivingLicenseApplicationID))
                return clsApplication.deleteApplication(applicationID);

            return false;
        }

        static public clsLocalDrivingLicenseApplication getLocalDrivingLicenseApplicationByID(int localDrivingLicenseApplicationID)
        {
            int licenseClassID = -1, applicationID = -1;

            if (clsLocalDrivingLicenseApplicationsDL.getLocalDrivingLicenseApplicationByID(
                localDrivingLicenseApplicationID, ref licenseClassID, ref applicationID)) 
            {
                clsLocalDrivingLicenseApplication LDLA = new clsLocalDrivingLicenseApplication(
                    localDrivingLicenseApplicationID, licenseClassID, applicationID);

                if (LDLA.getApplicationByID(applicationID))
                    return LDLA;
                
            }

            return null;
        }

        static public clsLocalDrivingLicenseApplication getLocalDrivingLicenseApplicationByApplicationID(int applicationID)
        {
            int localDrivingLicenseApplicationID = -1, licenseClassID = -1;
            
            if (clsLocalDrivingLicenseApplicationsDL.getLocalDrivingLicenseApplicationByApplicationID(
                applicationID, ref localDrivingLicenseApplicationID, ref licenseClassID))
            {
                clsLocalDrivingLicenseApplication LDLA = new clsLocalDrivingLicenseApplication(
                    localDrivingLicenseApplicationID, licenseClassID, applicationID);

                if (LDLA.getApplicationByID(applicationID))
                    return LDLA;

            }

            return null;
        }

        static public DataTable getListLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationsDL.getListLocalDrivingLicenseApplications();
        }
    }

}