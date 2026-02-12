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
    public partial class frmManageInternationalLicenses : Form
    {
        DataTable dataTable = new DataTable();
        DataView dataView = new DataView();

        public frmManageInternationalLicenses()
        {
            InitializeComponent();
        }

        void changeColumnsWidth()
        {         
            dgvManageInternationalLicenseApplicationsList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvManageInternationalLicenseApplicationsList.Columns[1].Width = 150;
            dgvManageInternationalLicenseApplicationsList.Columns[2].Width = 150;
            dgvManageInternationalLicenseApplicationsList.Columns[3].Width = 150;
            dgvManageInternationalLicenseApplicationsList.Columns[4].Width = 200;
            dgvManageInternationalLicenseApplicationsList.Columns[5].Width = 200;
        }

        void changeColumnsNames()
        {
            dataTable.Columns[0].ColumnName = "International License ID";
            dataTable.Columns[1].ColumnName = "Application ID";
            dataTable.Columns[2].ColumnName = "Driver ID";
            dataTable.Columns[3].ColumnName = "Local License ID";
            dataTable.Columns[4].ColumnName = "Issue Date";
            dataTable.Columns[5].ColumnName = "Expiration Date";
            dataTable.Columns[6].ColumnName = "Is Active";

        }

        void fillDGVInternationalLicenseApplications()
        {
            dataTable = clsInternationalLicense.getListInternationalLicenses();
            changeColumnsNames();
            dataView = new DataView(dataTable);
            lblRecords.Text = dataView.Count.ToString();

            dgvManageInternationalLicenseApplicationsList.DataSource = dataView;
            changeColumnsWidth();
        }

        void resetFiltering()
        {
            dataView.RowFilter = string.Empty;
            tbFilter.Visible = true;
            tbFilter.Text = string.Empty;
            lblRecords.Text = dataView.Count.ToString();
        }

        private void frmManageInternationalLicenses_Load(object sender, EventArgs e)
        {
            fillDGVInternationalLicenseApplications();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetFiltering();

            if (cbFilterBy.Text == "Is Active")
            {
                tbFilter.Visible = false;
                cbIsActive.Visible = true;
            }
            else
            {
                tbFilter.Visible = true;
                cbIsActive.Visible = false;
                cbIsActive.SelectedIndex = 0;
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

            dataView.RowFilter = "[" + cbFilterBy.Text + "] =" + "'" + tbFilter.Text + "'";
            lblRecords.Text = dataView.Count.ToString();

        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsActive.Text)
            {

                case "Yes":
                    dataView.RowFilter = "[Is Active] = " + "1";
                    break;

                case "No":
                    dataView.RowFilter = "[Is Active] = " + "0";
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

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            frmAddInternationalLicense frm = new frmAddInternationalLicense();
            frm.ShowDialog();
            fillDGVInternationalLicenseApplications();
            cbFilterBy.SelectedIndex = 0;
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageInternationalLicenseApplicationsList.SelectedCells[0].RowIndex;

            int internationalLicenseID = (int)dgvManageInternationalLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            clsInternationalLicense internationalLicense = clsInternationalLicense.getInternationalLicenseByID(internationalLicenseID);
            frmPersonDetails frm = new frmPersonDetails(internationalLicense.personID);
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageInternationalLicenseApplicationsList.SelectedCells[0].RowIndex;

            int internationalLicenseID = (int)dgvManageInternationalLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            frmInternationalLicenseDetails frm = new frmInternationalLicenseDetails(internationalLicenseID);
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvManageInternationalLicenseApplicationsList.SelectedCells[0].RowIndex;

            int internationalLicenseID = (int)dgvManageInternationalLicenseApplicationsList.Rows[selectedRow].Cells[0].Value;

            clsInternationalLicense internationalLicense = clsInternationalLicense.getInternationalLicenseByID(internationalLicenseID);
            frmPersonLicensesHistory frm = new frmPersonLicensesHistory(internationalLicense.personID);
            frm.ShowDialog();
        }
    }
}
