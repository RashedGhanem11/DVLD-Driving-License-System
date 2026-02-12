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
    public partial class frmTestsAppointments : Form
    {
        enum enMode { eVisionMode, eWrittenMode, eStreetMode }
        enMode nowMode;
        int localDrivingLicenseApplicationID = -1;
        int testTypeID = -1;

        public frmTestsAppointments(int localDrivingLicenseApplicationID, int mode)
        {
            InitializeComponent();

            this.localDrivingLicenseApplicationID = localDrivingLicenseApplicationID;

            cnrlLocalDrivingLicenseApplicationInfo1.loadLocalDrivingLicenseApplicationInfo(localDrivingLicenseApplicationID);
            switch(mode)
            {
                
                case 1:
                    nowMode = enMode.eVisionMode;
                    break;

                case 2:
                    nowMode = enMode.eWrittenMode;
                    lblTitle.Text = "Written Test Appointments";
                    pbTestType.Image = Properties.Resources.Written_Test_512;
                    break;

                case 3:
                    nowMode = enMode.eStreetMode;
                    lblTitle.Text = "Street Test Appointments";
                    pbTestType.Image = Properties.Resources.driving_test_512;
                    break;
                    
            }

            testTypeID = getTestTypeID();

            this.Text = lblTitle.Text;
        }

        int getTestTypeID()
        {
            switch(nowMode)
            {
                case enMode.eVisionMode:
                    return 1;

                case enMode.eWrittenMode:
                    return 2;

                case enMode.eStreetMode:
                    return 3;

            }

            return 0;
        }

        void changeColumnsData()
        {
            dgvAppointmentsList.Columns[0].HeaderText = "Appointment ID";
            dgvAppointmentsList.Columns[1].HeaderText = "Appointment Date";
            dgvAppointmentsList.Columns[2].HeaderText = "Paid Fees";
            dgvAppointmentsList.Columns[3].HeaderText = "Is Locked";


            dgvAppointmentsList.Columns[0].Width = 130;
            dgvAppointmentsList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAppointmentsList.Columns[2].Width = 130;
        }

        void fillDGVTestAppointments()
        {
            dgvAppointmentsList.DataSource = clsTestAppointment.getListTestAppointmentsShortDetailed(testTypeID, localDrivingLicenseApplicationID);
            lblRecords.Text = dgvAppointmentsList.RowCount.ToString();

            if (dgvAppointmentsList.RowCount > 0)
                changeColumnsData();
        }

        private void frmTestsAppointments_Load(object sender, EventArgs e)
        {
            fillDGVTestAppointments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddTestAppointment_Click(object sender, EventArgs e)
        {
            if (clsTestAppointment.isApplicationHaveAnActiveAppointment(testTypeID, localDrivingLicenseApplicationID)) 
            {
                MessageBox.Show("Person Already have an active appointment for this test, you can NOT add a new appointment", "Active appointment already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsTest.isTestTypePassed(testTypeID, localDrivingLicenseApplicationID))
            {
                MessageBox.Show("Person Already passed this test", "Test passed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmAdd_UpdateTestAppointment frm = new frmAdd_UpdateTestAppointment(localDrivingLicenseApplicationID, testTypeID, -1);
            frm.ShowDialog();

            fillDGVTestAppointments();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvAppointmentsList.SelectedCells[0].RowIndex;

            int testAppointmentID = (int)dgvAppointmentsList.Rows[selectedRow].Cells[0].Value;

            frmAdd_UpdateTestAppointment frm = new frmAdd_UpdateTestAppointment(localDrivingLicenseApplicationID, testTypeID, testAppointmentID);
            frm.ShowDialog();
            fillDGVTestAppointments();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvAppointmentsList.SelectedCells[0].RowIndex;

            int testAppointmentID = (int)dgvAppointmentsList.Rows[selectedRow].Cells[0].Value;

            frmTakeTest frm = new frmTakeTest(testAppointmentID);
            frm.ShowDialog();
            fillDGVTestAppointments();
            cnrlLocalDrivingLicenseApplicationInfo1.refreshData();
        }
    }
}
