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
    public partial class frmAdd_UpdateTestAppointment : Form
    {
        enum enTestMode { eVisionMode, eWrittenMode, eStreetMode }
        enTestMode nowTestMode;

        enum enMode { addModeWithNORetakeApp, addModeWithRetakeApp, updateMode, lockedMode }
        enMode nowMode = enMode.addModeWithNORetakeApp;

        int localDrivingLicenseApplicationID = -1;
        int trailNum = 0;

        clsTestAppointment testAppointment = new clsTestAppointment(false);
        clsTestType testType;

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

        void fillLicenseApplicationData()
        {
            clsLocalDrivingLicenseApplication localApp = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(localDrivingLicenseApplicationID);

            lblDLAppID.Text = localDrivingLicenseApplicationID.ToString();
            lblDClass.Text = clsLicenseClass.getLicenseClassByID(localApp.licenseClassID).className;
            lblName.Text = clsPerson.getPersonFullName(localApp.personID);
            lblTrail.Text = trailNum.ToString();
            dateTimePicker1.MinDate = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;
            
            decimal testFees = testType.testTypeFees;
            lblFees.Text = decimal.ToSingle(testFees).ToString();
            lblTotalFees.Text = decimal.ToSingle(testFees).ToString();
            

           
        }

        public frmAdd_UpdateTestAppointment(int localDrivingLicenseApplicationID, int mode, int testAppointmentID)
        {
            InitializeComponent();

            this.localDrivingLicenseApplicationID = localDrivingLicenseApplicationID;

            testType = clsTestType.getTestTypeByID(mode);

            switch (mode)
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

            trailNum = clsTestAppointment.trailNumbers(mode, localDrivingLicenseApplicationID);

            if (testAppointmentID == -1)
            {
                if (trailNum > 0)
                {
                    nowMode = enMode.addModeWithRetakeApp;
                    testAppointment = new clsTestAppointment(true);
                }
                else
                {
                    nowMode = enMode.addModeWithNORetakeApp;
                    testAppointment = new clsTestAppointment(false);
                }
            }
            else
            {
                testAppointment = clsTestAppointment.getTestAppointmentByID(testAppointmentID);

                if (testAppointment.isLocked)
                    nowMode = enMode.lockedMode;
                else
                    nowMode = enMode.updateMode;

            }
        
        }

        private void frmAdd_UpdateTestAppointment_Load(object sender, EventArgs e)
        {
            fillLicenseApplicationData();

            if (nowMode == enMode.addModeWithRetakeApp)
            {
                gbRetakeTestInfo.Enabled = true;
                decimal retakeTestFees = clsApplicationType.getApplicationTypeByID(7).applicationFees;
                lblRAppFees.Text = decimal.ToSingle(retakeTestFees).ToString();
                lblTotalFees.Text = decimal.ToSingle((retakeTestFees + testType.testTypeFees)).ToString();
            }

            if (nowMode == enMode.updateMode || nowMode == enMode.lockedMode)
            {             
                lblFees.Text = decimal.ToSingle(testAppointment.paidFees).ToString();

                if (testAppointment.retakeTestApplicationID != -1) 
                {
                    gbRetakeTestInfo.Enabled = true;
                    clsRetakeTestApplication retakeTestApplication = clsRetakeTestApplication.getRetakeTestApplicationByID(testAppointment.retakeTestApplicationID);

                    lblRTestAppID.Text = retakeTestApplication.applicationID.ToString();
                    lblRAppFees.Text = decimal.ToSingle(retakeTestApplication.paidFees).ToString();
                    lblTotalFees.Text = decimal.ToSingle((testAppointment.paidFees + retakeTestApplication.paidFees)).ToString();
                }
            }

            if (nowMode == enMode.lockedMode)
            {
                lblLockedNote.Visible = true;
                btnSave.Enabled = false;
                dateTimePicker1.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (nowMode == enMode.updateMode) 
            {
                testAppointment.appointmentDate = dateTimePicker1.Value;
                if (testAppointment.save()) 
                {
                    MessageBox.Show("Appointment saved successfully", "Appointment Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }

                MessageBox.Show("Appointment saved Failed", "Appointment Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;         
            }

            testAppointment.testTypeID = testType.testTypeID;
            testAppointment.localDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            testAppointment.appointmentDate = dateTimePicker1.Value;
            testAppointment.paidFees = testType.testTypeFees;
            testAppointment.createdByUserID = clsGlobalSettings.currentUser.userID;
            testAppointment.isLocked = false;

            if (testAppointment.save())
            {
                MessageBox.Show("Appointment saved successfully", "Appointment Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
            else
                MessageBox.Show("Appointment saved Failed", "Appointment Save", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }
    }
}
