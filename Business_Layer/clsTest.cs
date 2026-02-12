using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bussiness_Layer
{
    public class clsTest
    {
        public int testID { get; set; }
        public int testAppointmentID { get; set; }
        public bool testResult { get; set; }
        public string notes { get; set; }
        public int createdByUserID { get; set; }

        private bool _addTest()
        {
            this.testID = clsTestsDL.addNewTest(testAppointmentID, testResult, notes, createdByUserID);

            return testID != -1;
        }

        public clsTest()
        {
            this.testID = -1;
            this.testAppointmentID = -1;
            this.testResult = false;
            this.notes = "";
            this.createdByUserID = -1;
        }

        private clsTest(int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            this.testID = testID;
            this.testAppointmentID = testAppointmentID;
            this.testResult = testResult;
            this.notes = notes;    
            this.createdByUserID = createdByUserID;
        }

        static public int passedTestsNumber(int localDrivingLicenseApplicationID)
        {
            return clsTestsDL.passedTestsNumber(localDrivingLicenseApplicationID);
        }

        static public bool isTestTypePassed(int testTypeID, int localDrivingLicenseApplicationID)
        {
            return clsTestsDL.isTestTypePassed(testTypeID, localDrivingLicenseApplicationID);
        }

        static public clsTest getTestByID(int testID)
        {
            int testAppointmentID = -1, createdByUserID = -1;
            bool testResult = false;
            string notes = "";

            if (clsTestsDL.getTestByID(testID, ref testAppointmentID,
                ref testResult, ref notes, ref createdByUserID))
            {
                return new clsTest(testID, testAppointmentID, testResult, notes, createdByUserID);
            }
            else
                return null;

        }

        static public clsTest getTestByTestAppointmentID(int testAppointmentID)
        {
            int testID = -1, createdByUserID = -1;
            bool testResult = false;
            string notes = "";

            if (clsTestsDL.getTestByTestAppointmentID(testAppointmentID, ref testID,
                ref testResult, ref notes, ref createdByUserID))
            {
                return new clsTest(testID, testAppointmentID, testResult, notes, createdByUserID);
            }
            else
                return null;

        }

        public bool save()
        {
            if (clsTestAppointmentsDL.lockTestAppointment(testAppointmentID))
                return _addTest();

            return false;

        }
    }
}
