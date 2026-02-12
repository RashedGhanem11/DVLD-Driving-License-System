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
    public partial class FrmManageLocalDrivingLicenseApplications : Form
    {
        DataTable dataTable = new DataTable();
        DataView dataView = new DataView();

        public FrmManageLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        void changeColumnsWidth()
        {
            dgvManageLocalDrivingLicenseApplicationsList.Columns[1].Width = 200;
            dgvManageLocalDrivingLicenseApplicationsList.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvManageLocalDrivingLicenseApplicationsList.Columns[4].Width = 200;
        }

        void changeColumnsNames()
        {
            dataTable.Columns[0].ColumnName = "L.D.L.AppID";
            dataTable.Columns[1].ColumnName = "Driving Class";
            dataTable.Columns[2].ColumnName = "National No.";
            dataTable.Columns[3].ColumnName = "Full Name";
            dataTable.Columns[4].ColumnName = "Application Date";
            dataTable.Columns[5].ColumnName = "Passed Tests";
            dataTable.Columns[6].ColumnName = "Status";

        }

        void fillDGVLocalDrivingLicenseApplications()
        {
            dataTable = clsLocalDrivingLicenseApplication.getListLocalDrivingLicenseApplications();
            changeColumnsNames();
            dataView = new DataView(dataTable);
            lblRecords.Text = dataView.Count.ToString();

            dgvManageLocalDrivingLicenseApplicationsList.DataSource = dataView;
            changeColumnsWidth();
        }

        void resetFiltering()
        {
            dataView.RowFilter = string.Empty;
            tbFilter.Visible = true;
            tbFilter.Text = string.Empty;
            lblRecords.Text = dataView.Count.ToString();
        }

        private void FrmManageLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            fillDGVLocalDrivingLicenseApplications();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetFiltering();

            if (cbFilterBy.Text == "None")
                tbFilter.Visible = false;

        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter.Text == string.Empty)
            {
                resetFiltering();
                return;
            }


            if (cbFilterBy.Text == "L.D.L.AppID")
                dataView.RowFilter = "[L.D.L.AppID] = " + "'" + tbFilter.Text + "'";
            else
            {
                //dataView.RowFilter = "[National No.] like 'N3%'"; example

                string formatResults = "'" + tbFilter.Text + "%'";
                string formatFilterBy = "[" + cbFilterBy.Text + "]";
                string query = formatFilterBy + " like " + formatResults;
                dataView.RowFilter = query;
            }

            lblRecords.Text = dataView.Count.ToString();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            frmAdd_UpdateLocalLicense frm = new frmAdd_UpdateLocalLicense(-1);
            frm.ShowDialog();
            fillDGVLocalDrivingLicenseApplications();
            cbFilterBy.SelectedIndex = 0;
        }

        private void showAppliactionDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            frmLocalDrivingLicenseApplicationDetails frm = new frmLocalDrivingLicenseApplicationDetails(localAppID);
            frm.ShowDialog();

        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            frmAdd_UpdateLocalLicense frm = new frmAdd_UpdateLocalLicense(localAppID);
            frm.ShowDialog();
            fillDGVLocalDrivingLicenseApplications();
            cbFilterBy.SelectedIndex = 0;
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            if (MessageBox.Show($"Are you sure you want to delete application with local ID = {localAppID}",
                "Delete Application", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (clsLocalDrivingLicenseApplication.deleteLocalDrivingLicenseApplication(localAppID)) 
                {
                    MessageBox.Show("Application deleted successfully", "Delete Application",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fillDGVLocalDrivingLicenseApplications();
                    cbFilterBy.SelectedIndex = 0;
                }
                else
                    MessageBox.Show("Application deleted failed", "Delete Application",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            clsLocalDrivingLicenseApplication localApplication = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(localAppID);

            if (MessageBox.Show($"Are you sure you want to cancel application with local ID = {localAppID}",
                "Cancel Application", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (clsApplication.cancelApplication(localApplication.applicationID))
                {
                    fillDGVLocalDrivingLicenseApplications();
                    cbFilterBy.SelectedIndex = 0;
                }
                else
                    MessageBox.Show("Application Canceled failed", "Cancel Application",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sechduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            frmTestsAppointments frm = new frmTestsAppointments(localAppID, 1);
            frm.ShowDialog();
            fillDGVLocalDrivingLicenseApplications();
            cbFilterBy.SelectedIndex = 0;
        }

        private void sechduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            frmTestsAppointments frm = new frmTestsAppointments(localAppID, 2);
            frm.ShowDialog();
            fillDGVLocalDrivingLicenseApplications();
            cbFilterBy.SelectedIndex = 0;
        }

        private void sechduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            frmTestsAppointments frm = new frmTestsAppointments(localAppID, 3);
            frm.ShowDialog();
            fillDGVLocalDrivingLicenseApplications();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cmsManageLocalDrivingLicenseApplications_Opening(object sender, CancelEventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            clsLocalDrivingLicenseApplication localApp =  clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(localAppID);

            int testsPassedNumber = clsTest.passedTestsNumber(localAppID);

            if (localApp.applicationStatus == 2) 
            {
                editApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = false;
                cancelApplicationToolStripMenuItem.Enabled = false;
                sechduleTestsToolStripMenuItem.Enabled = false;
                return;
            }

            switch (testsPassedNumber)
            {
                case 0:
                    sechduleVisionTestToolStripMenuItem.Enabled = true;
                    break;

                case 1:
                    sechduleWrittenTestToolStripMenuItem.Enabled = true;
                    break;

                case 2:
                    sechduleStreetTestToolStripMenuItem.Enabled = true;
                    break;

                case 3:
                    editApplicationToolStripMenuItem.Enabled = false;
                    sechduleTestsToolStripMenuItem.Enabled = false;

                    if (localApp.applicationStatus == 3)
                    {
                        deleteApplicationToolStripMenuItem.Enabled = false;
                        cancelApplicationToolStripMenuItem.Enabled = false;
                        showLicenseToolStripMenuItem.Enabled = true;
                    }
                    else
                        issueDrivingLicenseToolStripMenuItem.Enabled = true;

                    

                    break;
            }
            
        }

        private void cmsManageLocalDrivingLicenseApplications_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            editApplicationToolStripMenuItem.Enabled = true;
            cancelApplicationToolStripMenuItem.Enabled = true;
            deleteApplicationToolStripMenuItem.Enabled = true;
            sechduleTestsToolStripMenuItem.Enabled = true;
            sechduleVisionTestToolStripMenuItem.Enabled = false;
            sechduleWrittenTestToolStripMenuItem.Enabled= false;
            sechduleStreetTestToolStripMenuItem.Enabled = false;
            issueDrivingLicenseToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
        }

        private void issueDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            frmIssueDrivingLicenseForFirstTime frm = new frmIssueDrivingLicenseForFirstTime(localAppID);
            frm.ShowDialog();
            fillDGVLocalDrivingLicenseApplications();
            cbFilterBy.SelectedIndex = 0;
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            clsLicense license = clsLicense.getLicenseByApplicationID(clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(localAppID).applicationID);

            frmLicenseDetails frm = new frmLicenseDetails(license.licenseID);
            frm.ShowDialog();

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageLocalDrivingLicenseApplicationsList.SelectedCells[0].RowIndex;

            int localAppID = (int)dgvManageLocalDrivingLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            int personID = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(localAppID).personID;

            frmPersonLicensesHistory frm = new frmPersonLicensesHistory(personID);
            frm.ShowDialog();
        }
    }
}