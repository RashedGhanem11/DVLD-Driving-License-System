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
    public partial class frmPersonLicensesHistory : Form
    {

        int personID = -1;

        void fillDGVs()
        {
            dgvLocalLicenses.DataSource = clsLicense.getListLicensesShortDetails(personID);
            lblLocalRecords.Text = dgvLocalLicenses.RowCount.ToString();

            dgvInternationalLicenses.DataSource = clsInternationalLicense.getListInternationalLicensesShortDetails(personID);
            lblInternationalRecords.Text = dgvInternationalLicenses.RowCount.ToString();

            if (dgvLocalLicenses.RowCount > 0)
            {
                dgvLocalLicenses.Columns[0].HeaderText = "License ID";
                dgvLocalLicenses.Columns[1].HeaderText = "Application ID";
                dgvLocalLicenses.Columns[2].HeaderText = "Class Name";
                dgvLocalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicenses.Columns[5].HeaderText = "Is Active";


                dgvLocalLicenses.Columns[0].Width = 130;
                dgvLocalLicenses.Columns[1].Width = 130;
                dgvLocalLicenses.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvLocalLicenses.Columns[3].Width = 200;
                dgvLocalLicenses.Columns[4].Width = 200;

            }

            if (dgvInternationalLicenses.RowCount > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "International License ID";
                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[2].HeaderText = "Local License ID";
                dgvInternationalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].HeaderText = "Is Active";


                dgvInternationalLicenses.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvInternationalLicenses.Columns[1].Width = 150;
                dgvInternationalLicenses.Columns[2].Width = 150;
                dgvInternationalLicenses.Columns[3].Width = 250;
                dgvInternationalLicenses.Columns[4].Width = 250;
            }
        }
        

        public frmPersonLicensesHistory(int personID)
        {
            InitializeComponent();
            this.personID = personID;
            cnrlPersonInfo1.loadPersonInfo(personID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPersonLicensesHistory_Load(object sender, EventArgs e)
        {
            fillDGVs();
        }

        private void frmPersonLicensesHistory_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlPersonInfo1.clearImage();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvLocalLicenses.SelectedCells[0].RowIndex;

            int licenseID = (int)dgvLocalLicenses.Rows[selectedRow].Cells[0].Value;

            frmLicenseDetails frm = new frmLicenseDetails(licenseID);
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvInternationalLicenses.SelectedCells[0].RowIndex;

            int licenseID = (int)dgvInternationalLicenses.Rows[selectedRow].Cells[0].Value;

            frmInternationalLicenseDetails frm = new frmInternationalLicenseDetails(licenseID);
            frm.ShowDialog();
        }
    }
}
