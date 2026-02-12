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
    public partial class frmReplaceLicense : Form
    {
        int personID = -1;
        int newLicenseID = -1;
        int oldLicenseID = -1;

        enum enMode { Damaged, Lost}
        enMode mode = enMode.Damaged;

        public frmReplaceLicense()
        {
            InitializeComponent();
        }

        void issueLostReplacement()
        {
            clsReplacementForLostLicense newLicense = new clsReplacementForLostLicense(oldLicenseID);

            newLicense.personID = personID;
            newLicense.createdByUserID = clsGlobalSettings.currentUser.userID;

            if (newLicense.save())
            {
                clsLicense license = clsLicense.getLicenseByApplicationID(newLicense.applicationID);
                newLicenseID = license.licenseID;
                MessageBox.Show($"New License Saved Successfully with ID = {license.licenseID}", "License Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cnrlLicenseInfoWithFindBy1.disableFilter();
                btnIssue.Enabled = false;
                lblShowNewLicenseInfo.Enabled = true;
                lblReplacementLicenseID.Text = license.licenseID.ToString();
                lblReplacementAppID.Text = license.applicationID.ToString();
                gbReplacmentFor.Enabled = false;
            }
            else
            {
                MessageBox.Show("New License Saved Failed ", "License Saved Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        void issueDamagedReplacement()
        {
            clsReplacementForDamagedLicense newLicense = new clsReplacementForDamagedLicense(oldLicenseID);

            newLicense.personID = personID;
            newLicense.createdByUserID = clsGlobalSettings.currentUser.userID;

            if (newLicense.save())
            {
                clsLicense license = clsLicense.getLicenseByApplicationID(newLicense.applicationID);
                newLicenseID = license.licenseID;
                MessageBox.Show($"New License Saved Successfully with ID = {license.licenseID}", "License Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cnrlLicenseInfoWithFindBy1.disableFilter();
                btnIssue.Enabled = false;
                lblShowNewLicenseInfo.Enabled = true;
                lblReplacementLicenseID.Text = license.licenseID.ToString();
                lblReplacementAppID.Text = license.applicationID.ToString();
                gbReplacmentFor.Enabled = false;
            }
            else
            {
                MessageBox.Show("New License Saved Failed ", "License Saved Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        void loadDefaultData()
        {
            personID = -1;
            newLicenseID = -1;
            lblOldLicenseID.Text = "???";
            btnIssue.Enabled = false;
            lblShowNewLicenseInfo.Enabled = false;
            lblShowLicensesHistory.Enabled = false;
        }

        bool checkData(int licenseID)
        {
            clsLicense oldlicense = clsLicense.getLicenseByID(licenseID);
            lblOldLicenseID.Text = licenseID.ToString();


            personID = clsDriver.getDriverByID(oldlicense.driverID).personID;
            oldLicenseID = oldlicense.licenseID;

            if (!oldlicense.isActive)
            {
                MessageBox.Show($"Local license is NOT active", "NOT Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void frmReplaceLicense_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = clsGlobalSettings.currentUser.userName;
            lblAppDate.Text = DateTime.Now.ToShortDateString();
            rbDamaged.Checked = true;
        }

        private void rbDamaged_CheckedChanged(object sender, EventArgs e)
        {
            this.Text = "Replacement For Damaged License";
            clsApplicationType appType = clsApplicationType.getApplicationTypeByID(4);
            lblFees.Text = decimal.ToSingle(appType.applicationFees).ToString();
            mode = enMode.Damaged;
        }

        private void rbLost_CheckedChanged(object sender, EventArgs e)
        {
            this.Text = "Replacement For Lost License";
            clsApplicationType appType = clsApplicationType.getApplicationTypeByID(3);
            lblFees.Text = decimal.ToSingle(appType.applicationFees).ToString();
            mode = enMode.Lost;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            switch (mode)
            {
                case enMode.Lost:
                    {
                        issueLostReplacement();
                        break;
                    }
                case enMode.Damaged:
                    {
                        issueDamagedReplacement();
                        break;
                    }
            }
        }

        private void frmReplaceLicense_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlLicenseInfoWithFindBy1.clearImage();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicensesHistory frm = new frmPersonLicensesHistory(personID);
            frm.ShowDialog();
        }

        private void lblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseDetails frm = new frmLicenseDetails(newLicenseID);
            frm.ShowDialog();
        }

    }
}
