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
    public partial class frmManageDetainedLicneses : Form
    {
        DataTable dataTable = new DataTable();
        DataView dataView = new DataView();

        public frmManageDetainedLicneses()
        {
            InitializeComponent();
        }

        void changeColumnsWidth()
        {
            dgvManageDetainedLicensesList.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvManageDetainedLicensesList.Columns[1].Width = 100;
            dgvManageDetainedLicensesList.Columns[2].Width = 150;
            dgvManageDetainedLicensesList.Columns[3].Width = 100;
            dgvManageDetainedLicensesList.Columns[4].Width = 100;
            dgvManageDetainedLicensesList.Columns[5].Width = 120;
            dgvManageDetainedLicensesList.Columns[8].Width = 120;

        }

        void changeColumnsNames()
        {
            dataTable.Columns[0].ColumnName = "Detain ID";
            dataTable.Columns[1].ColumnName = "License ID";
            dataTable.Columns[2].ColumnName = "Detain Date";
            dataTable.Columns[3].ColumnName = "Is Released";
            dataTable.Columns[4].ColumnName = "Fine Fees";
            dataTable.Columns[5].ColumnName = "Release Date";
            dataTable.Columns[6].ColumnName = "National No.";
            dataTable.Columns[7].ColumnName = "Full Name";
            dataTable.Columns[8].ColumnName = "Release App ID";

        }

        void fillDGVDetainedLicenses()
        {
            dataTable = clsDetainedLicense.getListDetainedLicenses();
            changeColumnsNames();
            dataView = new DataView(dataTable);
            lblRecords.Text = dataView.Count.ToString();

            dgvManageDetainedLicensesList.DataSource = dataView;
            changeColumnsWidth();
        }

        void resetFiltering()
        {
            dataView.RowFilter = string.Empty;
            tbFilter.Visible = true;
            tbFilter.Text = string.Empty;
            lblRecords.Text = dataView.Count.ToString();
        }

        private void frmManageDetainedLicneses_Load(object sender, EventArgs e)
        {
            fillDGVDetainedLicenses();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetFiltering();

            if (cbFilterBy.Text == "Is Released")
            {
                tbFilter.Visible = false;
                cbIsReleased.Visible = true;
            }
            else
            {
                tbFilter.Visible = true;
                cbIsReleased.Visible = false;
                cbIsReleased.SelectedIndex = 0;
            }

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


            if (cbFilterBy.Text == "Detain ID" || cbFilterBy.Text == "Release App ID")
                dataView.RowFilter = "[" + cbFilterBy.Text + "] =" + "'" + tbFilter.Text + "'";
            else
            {
                // dataView.RowFilter = "[National No.] like 'N3%'"; example

                string formatResults = "'" + tbFilter.Text + "%'";
                string formatFilterBy = "[" + cbFilterBy.Text + "]";
                string query = formatFilterBy + " like " + formatResults;
                dataView.RowFilter = query;
            }

            lblRecords.Text = dataView.Count.ToString();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Detain ID" || cbFilterBy.Text == "Release App ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsReleased.Text)
            {

                case "Yes":
                    dataView.RowFilter = "[Is Released] = " + "1";
                    break;

                case "No":
                    dataView.RowFilter = "[Is Released] = " + "0";
                    break;

                default:
                    dataView.RowFilter = string.Empty;
                    break;

            }
            lblRecords.Text = dataView.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
            fillDGVDetainedLicenses();
            cbFilterBy.SelectedIndex = 0;
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(-1);
            frm.ShowDialog();
            fillDGVDetainedLicenses();
            cbFilterBy.SelectedIndex = 0;
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageDetainedLicensesList.SelectedCells[0].RowIndex;

            string nationalNo = (string)dgvManageDetainedLicensesList.Rows[selectedRow].Cells[6].Value;

            clsPerson person = clsPerson.getPersonByNationalNo(nationalNo);
            frmPersonDetails frm = new frmPersonDetails(person.personID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageDetainedLicensesList.SelectedCells[0].RowIndex;

            int licenseID = (int)dgvManageDetainedLicensesList.Rows[selectedRow].Cells[1].Value;
           
            frmLicenseDetails frm = new frmLicenseDetails(licenseID);
            frm.ShowDialog();
        }

        private void showPersonLicensesHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageDetainedLicensesList.SelectedCells[0].RowIndex;

            string nationalNo = (string)dgvManageDetainedLicensesList.Rows[selectedRow].Cells[6].Value;

            clsPerson person = clsPerson.getPersonByNationalNo(nationalNo);
            frmPersonLicensesHistory frm = new frmPersonLicensesHistory(person.personID);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageDetainedLicensesList.SelectedCells[0].RowIndex;

            int licenseID = (int)dgvManageDetainedLicensesList.Rows[selectedRow].Cells[1].Value;

            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(licenseID);
            frm.ShowDialog();
            fillDGVDetainedLicenses();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cmsDetainedLicenses_Opening(object sender, CancelEventArgs e)
        {
            int selectedRow = dgvManageDetainedLicensesList.SelectedCells[0].RowIndex;

            bool isReleased = (bool)dgvManageDetainedLicensesList.Rows[selectedRow].Cells[3].Value;

            releaseDetainedLicenseToolStripMenuItem.Enabled = !isReleased;
            
        }
    }
    
}
