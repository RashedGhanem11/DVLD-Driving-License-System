using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsTestAppointment
    {
              
        public int testAppointmentID { get; set; }
        public int testTypeID { get; set; }
        public int localDrivingLicenseApplicationID { get; set; }
        public DateTime appointmentDate { get; set; }
        public decimal paidFees { get; set; }
        public int createdByUserID { get; set; }
        public bool isLocked { get; set; }
        public int retakeTestApplicationID { get; set; }
 
        private enum enMode { addModeWithNORetakeApp, addModeWithRetakeApp, updateMode, lockedMode }
        private enMode nowMode = enMode.addModeWithNORetakeApp;

        private bool _addTestAppointment()
        {
            this.testAppointmentID = clsTestAppointmentsDL.addNewTestAppointment(testTypeID, localDrivingLicenseApplicationID,
                appointmentDate, paidFees, createdByUserID, isLocked, retakeTestApplicationID);

            return testAppointmentID != -1;
        }

        private bool _updateTestAppointmentDate()
        {
            return clsTestAppointmentsDL.updateTestAppointmentDate(testAppointmentID, appointmentDate);
        }

        public clsTestAppointment(bool retakeApp)
        {
            this.testAppointmentID = -1;
            this.testTypeID = -1;
            this.localDrivingLicenseApplicationID = -1;
            this.appointmentDate = DateTime.Now;
            this.paidFees = -1;
            this.createdByUserID = -1;
            this.isLocked = false;
            this.retakeTestApplicationID = -1;

            if (retakeApp)
                nowMode = enMode.addModeWithRetakeApp;
            else
                nowMode = enMode.addModeWithNORetakeApp;

        }

        private clsTestAppointment(int testAppointmentID, int testTypeID, int localDrivingLicenseApplicationID,
            DateTime appointmentDate, decimal paidFees, int createdByUserID
            , bool isLocked, int retakeTestApplicationID)
        {
            this.testAppointmentID = testAppointmentID;
            this.testTypeID = testTypeID;
            this.localDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            this.appointmentDate = appointmentDate;
            this.paidFees = paidFees;
            this.createdByUserID = createdByUserID;
            this.isLocked = isLocked;
            this.retakeTestApplicationID = retakeTestApplicationID;

            if (isLocked)
                nowMode = enMode.lockedMode;
            else
                nowMode = enMode.updateMode;
        }

        static public DataTable getListTestAppointmentsShortDetailed(int testTypeID, int localDrivingLicenseApplicationID)
        {
            return clsTestAppointmentsDL.getListTestAppointmentsShortDetailed(testTypeID, localDrivingLicenseApplicationID); 
        }

        static public bool isApplicationHaveAnActiveAppointment(int testTypeID, int localDrivingLicenseApplicationID)
        {
            return clsTestAppointmentsDL.isApplicationHaveAnActiveAppointment(testTypeID, localDrivingLicenseApplicationID);
        }

        static public int trailNumbers(int testTypeID, int localDrivingLicenseApplicationID)
        {
            return clsTestAppointmentsDL.trailNumbers(testTypeID, localDrivingLicenseApplicationID);
        }

        public bool save()
        {
            switch (nowMode)
            {
                case enMode.addModeWithNORetakeApp:
                    return _addTestAppointment();

                case enMode.addModeWithRetakeApp:
                    {
                        clsRetakeTestApplication reTakeTest = new clsRetakeTestApplication();

                        reTakeTest.personID = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(localDrivingLicenseApplicationID).personID;
                        reTakeTest.createdByUserID = this.createdByUserID;
                        reTakeTest.applicationStatus = 3;

                        if (reTakeTest.save())
                        {
                            this.retakeTestApplicationID = reTakeTest.applicationID;

                            return _addTestAppointment();
                        }

                        return false;
                    }

                case enMode.updateMode:
                    return _updateTestAppointmentDate();
       
            }

            return false;

        }

        static public clsTestAppointment getTestAppointmentByID(int testAppointmentID)
        {
            int testTypeID = -1, localDrivingLicenseApplicationID = -1, createdByUserID = -1, retakeTestApplicationID = -1;

            DateTime appointmentDate = DateTime.Now;
            decimal paidFees = -1;
            bool isLocked = false;

            if (clsTestAppointmentsDL.getTestAppointmentByID(testAppointmentID, ref testTypeID,
                ref localDrivingLicenseApplicationID, ref appointmentDate, ref paidFees, ref createdByUserID,
                ref isLocked, ref retakeTestApplicationID)) 
            {
                return new clsTestAppointment(testAppointmentID, testTypeID, localDrivingLicenseApplicationID,
                    appointmentDate, paidFees, createdByUserID, isLocked, retakeTestApplicationID);
            }
            else
                return null;

        }

    }
}
