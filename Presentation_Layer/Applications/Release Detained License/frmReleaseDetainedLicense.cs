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
    public partial class frmReleaseDetainedLicense : Form
    {
        int personID = -1;
        int licenseID = -1;
        decimal appFees = 0;
        int detainID = -1;

        public frmReleaseDetainedLicense(int licenseID)
        {
            InitializeComponent();
            this.licenseID = licenseID;
        }

        void loadDefaultData()
        {
            personID = -1;
            licenseID = -1;
            detainID = -1;
            lblDetainID.Text = "???";
            lblLicenseID.Text = "???";
            lblDetainedBy.Text = "???";
            lblDetainDate.Text = "???";
            lblFineFees.Text = "???";
            lblTotalFees.Text = "???";
            btnRelease.Enabled = false;
            lblShowLicenseInfo.Enabled = false;
            lblShowLicensesHistory.Enabled = false;
        }

        bool checkData(int licenseID)
        {
            clsLicense license = clsLicense.getLicenseByID(licenseID);
            lblLicenseID.Text = licenseID.ToString();

            personID = clsDriver.getDriverByID(license.driverID).personID;
            this.licenseID = license.licenseID;

            if (!clsDetainedLicense.isLicenseDetained(licenseID))
            {
                MessageBox.Show($"Local license is NOT detained", "NOT Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            clsDetainedLicense detainedLicense = clsDetainedLicense.getDetainedLicenseByLicenseID(licenseID);
            lblDetainID.Text = detainedLicense.detainID.ToString();
            lblDetainDate.Text = detainedLicense.detainDate.ToString();
            lblDetainedBy.Text = clsUser.getUserByID(detainedLicense.createdByUserID).userName;
            lblFineFees.Text = decimal.ToSingle(detainedLicense.fineFees).ToString();
            lblTotalFees.Text = decimal.ToSingle(detainedLicense.fineFees + appFees).ToString();
            detainID = detainedLicense.detainID;

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
                btnRelease.Enabled = true;
            }
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {          
            appFees = clsApplicationType.getApplicationTypeByID(5).applicationFees;

            if (licenseID != -1)
            {
                cnrlLicenseInfoWithFindBy1_OnFindLicenseButtonClick(licenseID);
                cnrlLicenseInfoWithFindBy1.disableFilter();
                cnrlLicenseInfoWithFindBy1.loadLicenseInfo(licenseID);
            }


            lblAppFees.Text = decimal.ToSingle(appFees).ToString();
        }

        private void frmReleaseDetainedLicense_FormClosed(object sender, FormClosedEventArgs e)
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

        private void btnRelease_Click(object sender, EventArgs e)
        {
            clsReleaseDetainedLicense releaseDetainedLicense = new clsReleaseDetainedLicense(detainID);

            releaseDetainedLicense.personID = personID;
            releaseDetainedLicense.createdByUserID = clsGlobalSettings.currentUser.userID;

            if (releaseDetainedLicense.save()) 
            {
                MessageBox.Show("License released successfully", "Application saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblReleasedAppID.Text = releaseDetainedLicense.applicationID.ToString();
                btnRelease.Enabled = false;
                cnrlLicenseInfoWithFindBy1.disableFilter();
            }
            else
                MessageBox.Show("License released failed", "Application NOT saved", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
