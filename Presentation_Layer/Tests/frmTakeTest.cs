using Bussiness_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmTakeTest : Form
    {
        enum enTestMode { eVisionMode, eWrittenMode, eStreetMode }
        enTestMode nowTestMode;

        enum enMode { newTest, lockedTest }
        enMode nowMode = enMode.newTest;

        int testAppointmentID = -1;

        int getTestTypeID()
        {
            switch (nowTestMode)
            {
                case enTestMode.eVisionMode:
                    return 1;

                case enTestMode.eWrittenMode:
                    return 2;

                case enTestMode.eStreetMode:
                    return 3;

            }

            return 0;
        }

        void fillLicenseApplicationData(int localDrivingLicenseApplicationID)
        {
            clsLocalDrivingLicenseApplication localApp = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(localDrivingLicenseApplicationID);

            lblDLAppID.Text = localDrivingLicenseApplicationID.ToString();
            lblDClass.Text = clsLicenseClass.getLicenseClassByID(localApp.licenseClassID).className;
            lblName.Text = clsPerson.getPersonFullName(localApp.personID);   
        }

        void loadLockFormData()
        {
            nowMode = enMode.lockedTest;

            clsTest test = clsTest.getTestByTestAppointmentID(testAppointmentID);

            btnSave.Enabled = false;
            rbPass.Visible = false;
            rbFail.Visible = false;
            tbNotes.Visible = false;
           
            lblLockedNote.Visible = true;
            lblResult.Visible = true;
            lblNotes.Visible = true;

            lblTestID.Text = test.testID.ToString();

            if (test.testResult)
                lblResult.Text = "Passed";
            else
                lblResult.Text = "Failed";

            if (test.notes != "")
                lblNotes.Text = test.notes;
            else
                lblNotes.Text = "No Notes";
        }

        void fillData()
        {
            clsTestAppointment testAppointment = clsTestAppointment.getTestAppointmentByID(testAppointmentID);
 
            switch (testAppointment.testTypeID)
            {

                case 1:
                    nowTestMode = enTestMode.eVisionMode;
                    break;

                case 2:
                    nowTestMode = enTestMode.eWrittenMode;
                    pbTestType.Image = Properties.Resources.Written_Test_512;
                    gbScheduleTest.Text = "Written Test";
                    break;

                case 3:
                    nowTestMode = enTestMode.eStreetMode;
                    pbTestType.Image = Properties.Resources.driving_test_512;
                    gbScheduleTest.Text = "Street Test";
                    break;

            }

            fillLicenseApplicationData(testAppointment.localDrivingLicenseApplicationID);

            lblTrail.Text = clsTestAppointment.trailNumbers(testAppointment.testTypeID, testAppointment.localDrivingLicenseApplicationID).ToString();
            lblDate.Text = testAppointment.appointmentDate.ToShortDateString();

            decimal paidFees = testAppointment.paidFees;
            lblFees.Text = decimal.ToSingle(paidFees).ToString();


            if (testAppointment.isLocked)
                loadLockFormData();
        }

        public frmTakeTest(int testAppointmentID)
        {
            InitializeComponent();

            this.testAppointmentID = testAppointmentID;
        
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            fillData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (nowMode == enMode.lockedTest)
                return;

            clsTest test = new clsTest();

            test.testAppointmentID = testAppointmentID;
            test.testResult = rbPass.Checked;

            if (string.IsNullOrEmpty(tbNotes.Text)) 
                test.notes = "";
            else
                test.notes = tbNotes.Text;

            test.createdByUserID = clsGlobalSettings.currentUser.userID;

            if (test.save())
            {
                MessageBox.Show("Test saved successfully", "Test Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("Test saved Failed", "Test Save", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }
    }
}
