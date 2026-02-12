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
    public partial class frmAddInternationalLicense : Form
    {
        int personID = -1;
        int internationalLicenseID = -1;
        int localLicenseID = -1;

        public frmAddInternationalLicense()
        {
            InitializeComponent();
        }

        void loadDefaultData()
        {
            personID = -1;
            internationalLicenseID = -1;
            lblInternationalLicenseID.Text = "???";
            lblInternationalAppID.Text = "???";
            lblLicenseID.Text = "???";
            lblFees.Text = decimal.ToSingle(clsApplicationType.getApplicationTypeByID(6).applicationFees).ToString();
            lblCreatedBy.Text = clsGlobalSettings.currentUser.userName;
            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = (new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day)).ToShortDateString();
            btnIssue.Enabled = false;
            lblShowLicenseInfo.Enabled = false;
            lblShowLicensesHistory.Enabled = false;
        }

        bool checkData(int licenseID)
        {
            clsLicense license = clsLicense.getLicenseByID(licenseID);
            lblLicenseID.Text = licenseID.ToString();

            personID = clsDriver.getDriverByID(license.driverID).personID;
            localLicenseID = license.licenseID;
            clsInternationalLicense internationalLicense = clsInternationalLicense.getInternationalLicenseByLocalLicenseID(licenseID);

            if (internationalLicense != null)
            {
                MessageBox.Show($"Driver already have an international license ID with ID {internationalLicense.internationalLicenseID}", "NOT Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblInternationalLicenseID.Text = internationalLicense.internationalLicenseID.ToString();
                lblInternationalAppID.Text = internationalLicense.applicationID.ToString();
                lblFees.Text = decimal.ToSingle(internationalLicense.paidFees).ToString();
                lblCreatedBy.Text = clsUser.getUserByID(internationalLicense.createdByUserID).userName;
                lblAppDate.Text = internationalLicense.applicationDate.ToShortDateString();
                lblIssueDate.Text = internationalLicense.applicationDate.ToShortDateString();
                lblExpirationDate.Text = internationalLicense.expirationDate.ToShortDateString();
                internationalLicenseID = internationalLicense.internationalLicenseID;
                lblShowLicenseInfo.Enabled = true;
                return false;
            }

            if (!license.isActive) 
            {
                MessageBox.Show($"Local license is NOT active", "NOT Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (license.licenseClassID != 3) 
            {
                MessageBox.Show($"Local license must be from class 3", "NOT Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (DateTime.Compare(license.expirationDate, DateTime.Now) < 1) 
            {
                MessageBox.Show($"Local license expiration date is passed", "NOT Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                lblShowLicensesHistory.Enabled = true;

            if (checkData(obj)) 
            {
                btnIssue.Enabled = true;
            }    
        }

        private void frmAddInternationalLicense_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlLicenseInfoWithFindBy1.clearImage();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddInternationalLicense_Load(object sender, EventArgs e)
        {
            loadDefaultData();
        }

        private void lblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicensesHistory frm = new frmPersonLicensesHistory(personID);
            frm.ShowDialog();
        }

        private void lblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInternationalLicenseDetails frm = new frmInternationalLicenseDetails(internationalLicenseID);
            frm.ShowDialog();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            clsLicense localLicense = clsLicense.getLicenseByID(localLicenseID);

            clsInternationalLicense internationalLicense = new clsInternationalLicense();

            internationalLicense.issuedUsingLocalLicenseID = localLicense.licenseID;
            internationalLicense.driverID = localLicense.driverID;
            internationalLicense.createdByUserID = clsGlobalSettings.currentUser.userID;
            internationalLicense.personID = personID;

            if (internationalLicense.save()) 
            {
                MessageBox.Show($"International License Added Successfully with ID = {internationalLicense.internationalLicenseID}", "Operation Completed", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                internationalLicenseID = internationalLicense.internationalLicenseID;
                btnIssue.Enabled = false;
                lblShowLicenseInfo.Enabled = true;
                cnrlLicenseInfoWithFindBy1.disableFilter();
                lblInternationalLicenseID.Text = internationalLicense.internationalLicenseID.ToString();
                lblInternationalAppID.Text = internationalLicense.applicationID.ToString();
            }
            else
                MessageBox.Show($"International License Added Failed", "Operation Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

        }
    }
}
