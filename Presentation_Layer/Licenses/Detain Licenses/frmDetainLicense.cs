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
    public partial class frmDetainLicense : Form
    {
        int personID = -1;
        int licenseID = -1;

        public frmDetainLicense()
        {
            InitializeComponent();
        }

        void loadDefaultData()
        {
            personID = -1;
            licenseID = -1;
            lblLicenseID.Text = "???";
            lblCreatedBy.Text = clsGlobalSettings.currentUser.userName;
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            btnDetain.Enabled = false;
            lblShowLicenseInfo.Enabled = false;
            lblShowLicensesHistory.Enabled = false;
        }

        bool checkData(int licenseID)
        {
            clsLicense license = clsLicense.getLicenseByID(licenseID);
            lblLicenseID.Text = licenseID.ToString();

            personID = clsDriver.getDriverByID(license.driverID).personID;
            this.licenseID = license.licenseID;          

            if (clsDetainedLicense.isLicenseDetained(licenseID))
            {
                MessageBox.Show($"Local license is already detained", "NOT Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void cnrlLicenseInfoWithFindBy1_OnFindLicenseButtonClick(int obj)
        {
            loadDefaultData();

            if (obj == -1)
                return;
            else
            {
                lblShowLicensesHistory.Enabled = true;
                lblShowLicenseInfo.Enabled = true;
            }

            if (checkData(obj))
            {
                btnDetain.Enabled = true;
            }
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            loadDefaultData();
        }

        private void frmDetainLicense_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlLicenseInfoWithFindBy1.clearImage();
        }

        private void lblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicensesHistory frm = new frmPersonLicensesHistory(personID);
            frm.ShowDialog();
        }

        private void lblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseDetails frm = new frmLicenseDetails(licenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFineFees.Text)) 
            {
                MessageBox.Show("Please enter the fine fees", "Fine fees must be entered!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDetainedLicense detainLicense = new clsDetainedLicense();
            detainLicense.licenseID = this.licenseID;
            detainLicense.fineFees = Decimal.Parse(tbFineFees.Text);
            detainLicense.createdByUserID = clsGlobalSettings.currentUser.userID;

            if (detainLicense.save())
            {
                MessageBox.Show("License detained successfully", "License Detain", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblDetainID.Text = detainLicense.detainID.ToString();
                btnDetain.Enabled = false;
                cnrlLicenseInfoWithFindBy1.disableFilter();
            }
            else
                MessageBox.Show("License detained failed", "License Detain", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
